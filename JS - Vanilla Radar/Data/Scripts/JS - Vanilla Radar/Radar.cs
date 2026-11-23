using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using Draygo.API;
using Sandbox.Game.Entities;
using Sandbox.Definitions;
using SpaceEngineers.Game.ModAPI;

namespace JSVanillaRadar.Radar
{
    public class RadarConfig
    {
        [XmlElement("MinRange")]
        public float MinRange_;
        [XmlElement("MaxRange")]
        public float MaxRange_;
        [XmlElement("LargeLidarRange")]
        public float LargeLidarRange_;
        [XmlElement("SmallLidarRange")]
        public float SmallLidarRange_;

        private const float DEFAULT_MIN_RANGE         = 2500f;
        private const float DEFAULT_MAX_RANGE         = 15000f;
        private const float DEFAULT_LARGE_LIDAR_RANGE = 15000f;
        private const float DEFAULT_SMALL_LIDAR_RANGE = 10000f;
        private const float RANGE_FLOOR               = 100f;
        private const float RANGE_CEILING             = 50000f;

        public RadarConfig()
        {
            MinRange_        = DEFAULT_MIN_RANGE;
            MaxRange_        = DEFAULT_MAX_RANGE;
            LargeLidarRange_ = DEFAULT_LARGE_LIDAR_RANGE;
            SmallLidarRange_ = DEFAULT_SMALL_LIDAR_RANGE;
        }

        public void Validate()
        {
            // NaN survives Math.Min/Math.Max, so replace non-finite values with defaults first
            MinRange_        = ReplaceNonFinite(MinRange_,        DEFAULT_MIN_RANGE);
            MaxRange_        = ReplaceNonFinite(MaxRange_,        DEFAULT_MAX_RANGE);
            LargeLidarRange_ = ReplaceNonFinite(LargeLidarRange_, DEFAULT_LARGE_LIDAR_RANGE);
            SmallLidarRange_ = ReplaceNonFinite(SmallLidarRange_, DEFAULT_SMALL_LIDAR_RANGE);

            MinRange_        = Math.Max(RANGE_FLOOR, Math.Min(MinRange_,        RANGE_CEILING));
            LargeLidarRange_ = Math.Max(RANGE_FLOOR, Math.Min(LargeLidarRange_, RANGE_CEILING));
            SmallLidarRange_ = Math.Max(RANGE_FLOOR, Math.Min(SmallLidarRange_, RANGE_CEILING));
            MaxRange_        = Math.Max(GetMaxPossibleRange(), Math.Min(MaxRange_, RANGE_CEILING));
        }

        // Largest range any scan can reach: MinRange without a lidar, or the best lidar's range
        public float GetMaxPossibleRange()
        {
            return Math.Max(MinRange_, Math.Max(LargeLidarRange_, SmallLidarRange_));
        }

        private static float ReplaceNonFinite(float value, float fallback)
        {
            return (float.IsNaN(value) || float.IsInfinity(value)) ? fallback : value;
        }
    }

    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public class Radar : MySessionComponentBase
    {
        private const string CONFIG_FILE = "VanillaRadarConfig.xml";
        private const ushort CONFIG_SYNC_MSG_ID = 47291;
        private const byte MSG_REQUEST_CONFIG = 1;
        private const byte MSG_CONFIG_DATA = 2;

        private const string SUBTYPE_LARGE_LIDAR = "Lidar";
        private const string SUBTYPE_SMALL_LIDAR = "LidarSmall";
        private const string LARGE_LIDAR_DEF_ID = "MyObjectBuilder_LargeGatlingTurret/" + SUBTYPE_LARGE_LIDAR;
        private const string SMALL_LIDAR_DEF_ID = "MyObjectBuilder_LargeGatlingTurret/" + SUBTYPE_SMALL_LIDAR;

        private RadarConfig config_ = new RadarConfig();

        // Debug setting to always show radar even outside cockpits/turrets
        private const bool DEBUG_ALWAYS_SHOW_RADAR = false;

        // How often to scan for enemy grids (milliseconds) — 500ms = 2 Hz
        private const int SCAN_INTERVAL_MS = 500;

        // Label text size
        private const double LABEL_TEXT_SIZE = 0.7;

        // Maximum label length
        private const int MAX_LABEL_LENGTH = 20;

        // Minimum corner bracket size in screen units (so it doesn't get too tiny at long range)
        private const float MIN_BRACKET_SIZE = 0.010f;

        // Minimum block count for a grid to be shown
        private const int MIN_BLOCK_COUNT = 5;

        // Thickness of the box lines in screen units
        private const float LINE_THICKNESS = 0.0015f;

        // Vanilla SE material: a plain solid square, used to draw thin lines
        private static readonly MyStringId MAT_SQUARE = MyStringId.GetOrCompute("Square");

        private HudAPIv2 hudApi_;
        private bool hudApiReady_ = false;
        private bool blockBuiltSubscribed_ = false;
        private bool networkRegistered_ = false;
        private bool syncWarningShown_ = false;
        private int tick_ = 0;
        private int scanTicks_ = 30;
        private bool isClient_ = false;
        private float aspectRatio_ = 1f;
        private double nameLineHeight_ = 0;  // measured once from the HUD API, reused for label layout
        private Vector3D scanOrigin_;  // set each scan, used for LOS raycasts and distance sorting

        private readonly TargetDistanceComparer targetDistanceComparer_ = new TargetDistanceComparer();

        private enum RadarMode
        {
            All,
            Enemies,
            EnemiesAndNeutrals,
            None
        }
        private RadarMode currentMode_ = RadarMode.Enemies;

        private class TargetEntry
        {
            public IMyCubeGrid Grid_;
            public MyRelationsBetweenPlayerAndBlock Relation_;
            public Color BoxColor_;
            public string HexColor_;
            public string SanitizedLabel_;
            public double DistanceSq_;  // squared distance from the scan origin, set at scan time
        }

        // Sorts farthest first so the closest target's HUD slot draws on top.
        // An IComparer instance avoids the per-call wrapper allocation of List.Sort(Comparison<T>).
        private class TargetDistanceComparer : IComparer<TargetEntry>
        {
            public int Compare(TargetEntry a, TargetEntry b)
            {
                return b.DistanceSq_.CompareTo(a.DistanceSq_);
            }
        }

        // List of currently tracked targets (pooled TargetEntry objects)
        private readonly List<TargetEntry> activeTargets_ = new List<TargetEntry>();
        private readonly List<TargetEntry> targetEntryPool_ = new List<TargetEntry>();

        // Cached collections to prevent memory allocations (garbage collection) during scans
        private readonly List<IMyCubeGrid> myGroupScratch_ = new List<IMyCubeGrid>();
        private readonly List<IMyCubeGrid> myLogicalGroupScratch_ = new List<IMyCubeGrid>();
        private readonly HashSet<long> myDockedGridIds_ = new HashSet<long>();  // grids logically linked to mine (connectors + mechanical)
        private readonly HashSet<long> processedGroups_ = new HashSet<long>();
        private readonly List<IMyCubeGrid> groupScratch_ = new List<IMyCubeGrid>();
        private readonly HashSet<long> subgridIds_ = new HashSet<long>();
        private readonly List<IHitInfo> rayHitsScratch_ = new List<IHitInfo>();
        private readonly StringBuilder labelScratch_ = new StringBuilder(32);

        // HUD object pool — allocated once on first use, reused every frame
        private readonly List<TargetHudSlot> hudPool_ = new List<TargetHudSlot>();
        private int activeHudSlots_ = 0;

        private class TargetHudSlot
        {
            public readonly StringBuilder nameSb_ = new StringBuilder(48);
            public readonly StringBuilder infoSb_ = new StringBuilder(48);
            public HudAPIv2.HUDMessage nameLabel_;
            public HudAPIv2.HUDMessage infoLabel_;
            public HudAPIv2.BillBoardHUDMessage boxTop_, boxBottom_, boxLeft_, boxRight_;

            public void Initialize(MyStringId mat, double textScale)
            {
                var zero = Vector2D.Zero;
                nameLabel_ = new HudAPIv2.HUDMessage(nameSb_, zero, TimeToLive: -1, Scale: textScale, HideHud: true, Shadowing: true);
                nameLabel_.Visible = false;
                infoLabel_ = new HudAPIv2.HUDMessage(infoSb_, zero, TimeToLive: -1, Scale: textScale, HideHud: true, Shadowing: true);
                infoLabel_.Visible = false;
                boxTop_    = new HudAPIv2.BillBoardHUDMessage(mat, zero, Color.White, Width: LINE_THICKNESS, Height: LINE_THICKNESS, TimeToLive: -1, HideHud: true, Shadowing: true);
                boxTop_.Visible = false;
                boxBottom_ = new HudAPIv2.BillBoardHUDMessage(mat, zero, Color.White, Width: LINE_THICKNESS, Height: LINE_THICKNESS, TimeToLive: -1, HideHud: true, Shadowing: true);
                boxBottom_.Visible = false;
                boxLeft_   = new HudAPIv2.BillBoardHUDMessage(mat, zero, Color.White, Width: LINE_THICKNESS, Height: LINE_THICKNESS, TimeToLive: -1, HideHud: true, Shadowing: true);
                boxLeft_.Visible = false;
                boxRight_  = new HudAPIv2.BillBoardHUDMessage(mat, zero, Color.White, Width: LINE_THICKNESS, Height: LINE_THICKNESS, TimeToLive: -1, HideHud: true, Shadowing: true);
                boxRight_.Visible = false;
            }

            public void Hide()
            {
                if (nameLabel_ != null) nameLabel_.Visible = false;
                if (infoLabel_ != null) infoLabel_.Visible = false;
                if (boxTop_    != null) boxTop_.Visible    = false;
                if (boxBottom_ != null) boxBottom_.Visible = false;
                if (boxLeft_   != null) boxLeft_.Visible   = false;
                if (boxRight_  != null) boxRight_.Visible  = false;
            }

            public void Delete()
            {
                if (nameLabel_ != null) { nameLabel_.DeleteMessage(); nameLabel_ = null; }
                if (infoLabel_ != null) { infoLabel_.DeleteMessage(); infoLabel_ = null; }
                if (boxTop_    != null) { boxTop_.DeleteMessage();    boxTop_    = null; }
                if (boxBottom_ != null) { boxBottom_.DeleteMessage(); boxBottom_ = null; }
                if (boxLeft_   != null) { boxLeft_.DeleteMessage();   boxLeft_   = null; }
                if (boxRight_  != null) { boxRight_.DeleteMessage();  boxRight_  = null; }
            }
        }

        // Hides every slot in the pool — call on any early exit from Draw()
        private void HideAllSlots()
        {
            for (int i = 0; i < hudPool_.Count; i++)
                hudPool_[i].Hide();
        }

        private void ClearActiveTargets()
        {
            for (int i = 0; i < activeTargets_.Count; i++)
            {
                var entry = activeTargets_[i];
                entry.Grid_ = null;
                targetEntryPool_.Add(entry);
            }
            activeTargets_.Clear();
        }

        private TargetEntry RentTargetEntry()
        {
            int last = targetEntryPool_.Count - 1;
            if (last >= 0)
            {
                var entry = targetEntryPool_[last];
                targetEntryPool_.RemoveAt(last);
                return entry;
            }
            return new TargetEntry();
        }

        public override void LoadData()
        {
            base.LoadData();

            if (MyAPIGateway.Multiplayer == null || MyAPIGateway.Multiplayer.IsServer)
            {
                LoadConfig();
            }

            ApplyLidarRanges();
        }

        private void LoadConfig()
        {
            try
            {
                if (MyAPIGateway.Utilities.FileExistsInWorldStorage(CONFIG_FILE, typeof(Radar)))
                {
                    string xml;
                    using (var reader = MyAPIGateway.Utilities.ReadFileInWorldStorage(CONFIG_FILE, typeof(Radar)))
                    {
                        xml = reader.ReadToEnd();
                    }
                    config_ = MyAPIGateway.Utilities.SerializeFromXML<RadarConfig>(xml) ?? new RadarConfig();
                }

                // Validate, then always write back so clamped values and newly added fields appear in the file
                config_.Validate();
                SaveConfig();
            }
            catch (Exception ex)
            {
                MyLog.Default.WriteLineAndConsole($"Vanilla Radar: Error loading config: {ex}");
                config_ = new RadarConfig();
            }
        }

        private void SaveConfig()
        {
            var xml = MyAPIGateway.Utilities.SerializeToXML(config_);
            using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage(CONFIG_FILE, typeof(Radar)))
            {
                writer.Write(xml);
            }
        }

        private void ApplyLidarRanges()
        {
            UpdateLidarRange(LARGE_LIDAR_DEF_ID, config_.LargeLidarRange_);
            UpdateLidarRange(SMALL_LIDAR_DEF_ID, config_.SmallLidarRange_);
        }

        private void UpdateLidarRange(string typeSubtype, float newMaxRange)
        {
            MyDefinitionId id;
            if (MyDefinitionId.TryParse(typeSubtype, out id))
            {
                MyCubeBlockDefinition blockDef = MyDefinitionManager.Static.GetCubeBlockDefinition(id);
                var turretDef = blockDef as MyLargeTurretBaseDefinition;
                if (turretDef != null)
                {
                    turretDef.MaxRangeMeters = newMaxRange;
                }
            }
        }

        public override void BeforeStart()
        {
            base.BeforeStart();
            isClient_ = !MyAPIGateway.Utilities.IsDedicated;

            if (MyAPIGateway.Multiplayer != null)
            {
                MyAPIGateway.Multiplayer.RegisterSecureMessageHandler(CONFIG_SYNC_MSG_ID, OnConfigSyncMessage);
                networkRegistered_ = true;

                if (MyAPIGateway.Session != null && !MyAPIGateway.Session.IsServer)
                {
                    MyAPIGateway.Multiplayer.SendMessageToServer(CONFIG_SYNC_MSG_ID, new byte[] { MSG_REQUEST_CONFIG }, true);
                }
            }

            if (MyAPIGateway.Session != null && MyAPIGateway.Session.IsServer)
            {
                Sandbox.Game.MyVisualScriptLogicProvider.BlockBuilt += OnBlockBuilt;
                blockBuiltSubscribed_ = true;
            }

            if (!isClient_)
            {
                // Dedicated server: nothing to scan or draw; network and BlockBuilt handlers are event-driven
                SetUpdateOrder(MyUpdateOrder.NoUpdate);
                return;
            }

            scanTicks_ = Math.Max(1, (SCAN_INTERVAL_MS * 60) / 1000);
            tick_ = scanTicks_; // scan immediately on first eligible tick
            hudApi_ = new HudAPIv2(OnHudApiReady);
        }

        private void OnConfigSyncMessage(ushort handlerId, byte[] payload, ulong senderId, bool fromServer)
        {
            try
            {
                if (payload == null || payload.Length == 0) return;

                byte msgType = payload[0];
                if (msgType == MSG_REQUEST_CONFIG && MyAPIGateway.Session != null && MyAPIGateway.Session.IsServer)
                {
                    var xml = MyAPIGateway.Utilities.SerializeToXML(config_);
                    var xmlBytes = Encoding.UTF8.GetBytes(xml);
                    var packet = new byte[xmlBytes.Length + 1];
                    packet[0] = MSG_CONFIG_DATA;
                    Array.Copy(xmlBytes, 0, packet, 1, xmlBytes.Length);
                    MyAPIGateway.Multiplayer.SendMessageTo(CONFIG_SYNC_MSG_ID, packet, senderId, true);
                }
                else if (msgType == MSG_CONFIG_DATA && fromServer && MyAPIGateway.Session != null && !MyAPIGateway.Session.IsServer)
                {
                    if (payload.Length <= 1) return;
                    var xml = Encoding.UTF8.GetString(payload, 1, payload.Length - 1);
                    var synced = MyAPIGateway.Utilities.SerializeFromXML<RadarConfig>(xml);
                    if (synced != null)
                    {
                        synced.Validate();
                        config_ = synced;
                        ApplyLidarRanges();
                        WarnIfSyncDistanceTooLow();
                    }
                }
            }
            catch (Exception ex)
            {
                MyLog.Default.WriteLineAndConsole($"Vanilla Radar: Error syncing config: {ex}");
            }
        }

        // Multiplayer clients only receive entities within the server's Sync Distance, so the radar
        // cannot detect anything beyond it. Single-player and hosts have every entity and never call this.
        private void WarnIfSyncDistanceTooLow()
        {
            if (syncWarningShown_) return;

            var settings = MyAPIGateway.Session?.SessionSettings;
            if (settings == null) return;

            int syncDistance = settings.SyncDistance;
            float radarRange = config_.GetMaxPossibleRange();
            if (syncDistance >= radarRange) return;

            syncWarningShown_ = true;
            MyLog.Default.WriteLineAndConsole($"Vanilla Radar: server Sync Distance ({syncDistance} m) is below the radar range ({radarRange:0} m). Targets beyond {syncDistance} m will not be detected.");
            MyAPIGateway.Utilities.ShowMessage("Vanilla Radar",
                $"Server Sync Distance is {syncDistance} m but radar range is up to {radarRange:0} m. " +
                $"Targets beyond {syncDistance} m won't show. Ask the server admin to raise Sync Distance to at least {radarRange:0} m.");
        }

        private void OnBlockBuilt(string typeId, string subtypeId, string gridName, long blockId)
        {
            if (subtypeId == SUBTYPE_LARGE_LIDAR || subtypeId == SUBTYPE_SMALL_LIDAR)
            {
                var block = MyAPIGateway.Entities.GetEntityById(blockId);
                var turret = block as Sandbox.ModAPI.IMyLargeTurretBase;
                if (turret != null)
                {
                    var def = turret.SlimBlock?.BlockDefinition as MyLargeTurretBaseDefinition;
                    if (def != null)
                    {
                        turret.Range = def.MaxRangeMeters;
                    }
                }
            }
        }

        protected override void UnloadData()
        {
            if (blockBuiltSubscribed_)
            {
                Sandbox.Game.MyVisualScriptLogicProvider.BlockBuilt -= OnBlockBuilt;
                blockBuiltSubscribed_ = false;
            }

            if (networkRegistered_ && MyAPIGateway.Multiplayer != null)
            {
                MyAPIGateway.Multiplayer.UnregisterSecureMessageHandler(CONFIG_SYNC_MSG_ID, OnConfigSyncMessage);
                networkRegistered_ = false;
            }

            if (hudApiReady_ && hudApi_ != null && hudApi_.Heartbeat)
            {
                for (int i = 0; i < hudPool_.Count; i++)
                    hudPool_[i].Delete();
            }
            hudPool_.Clear();
            ClearActiveTargets();
            targetEntryPool_.Clear();
            hudApiReady_ = false;

            if (hudApi_ != null)
            {
                hudApi_.Unload();
                hudApi_ = null;
            }

            base.UnloadData();
        }

        private void OnHudApiReady()
        {
            hudApiReady_ = true;
        }

        private bool IsPlayerControllingWorkingBlock(out IMyEntity controlled)
        {
            controlled = MyAPIGateway.Session?.Player?.Controller?.ControlledEntity?.Entity;
            if (DEBUG_ALWAYS_SHOW_RADAR) return true;
            if (controlled == null || controlled.Closed) return false;

            var shipController = controlled as Sandbox.ModAPI.IMyShipController;
            if (shipController != null && shipController.IsWorking) return true;

            var turretController = controlled as Sandbox.ModAPI.IMyLargeTurretBase;
            if (turretController != null && turretController.IsWorking) return true;

            var customTurretController = controlled as SpaceEngineers.Game.ModAPI.IMyTurretControlBlock;
            if (customTurretController != null && customTurretController.IsWorking) return true;

            return false;
        }

        public override void HandleInput()
        {
            base.HandleInput();
            if (!isClient_ || MyAPIGateway.Session?.Player == null) return;
            if (MyAPIGateway.Gui.ChatEntryVisible || MyAPIGateway.Gui.IsCursorVisible) return;

            if (MyAPIGateway.Input.IsNewKeyPressed(VRage.Input.MyKeys.R) && MyAPIGateway.Input.IsAnyCtrlKeyPressed())
            {
                currentMode_ = (RadarMode)(((int)currentMode_ + 1) % 4);
                tick_ = scanTicks_; // scan immediately on next simulation tick
                string modeLabel;
                switch (currentMode_)
                {
                    case RadarMode.All:                modeLabel = "Show All";                  break;
                    case RadarMode.Enemies:            modeLabel = "Show Enemies";              break;
                    case RadarMode.EnemiesAndNeutrals: modeLabel = "Show Enemies and Neutrals"; break;
                    case RadarMode.None:               modeLabel = "Radar Off";                 break;
                    default:                           modeLabel = currentMode_.ToString();     break;
                }
                MyAPIGateway.Utilities.ShowNotification($"Radar Mode: {modeLabel}", 2000, "White");
            }
        }

        public override void UpdateBeforeSimulation()
        {
            base.UpdateBeforeSimulation();

            if (!isClient_ || MyAPIGateway.Session?.Player == null)
                return;

            IMyEntity controlled;
            if (currentMode_ == RadarMode.None || !IsPlayerControllingWorkingBlock(out controlled))
            {
                ClearActiveTargets();
                tick_ = scanTicks_;
                return;
            }

            if (++tick_ >= scanTicks_)
            {
                tick_ = 0;
                ScanForTargets(controlled);
            }
        }

        public override void Draw()
        {
            if (!isClient_ || !hudApiReady_)
                return;

            if (MyAPIGateway.Session?.Player == null || currentMode_ == RadarMode.None)
            {
                HideAllSlots();
                return;
            }

            var camera = MyAPIGateway.Session.Camera;
            if (camera == null || camera.ViewportSize.Y <= 0f)
            {
                HideAllSlots();
                return;
            }

            // Update aspect ratio every frame to handle resolution/window changes
            aspectRatio_ = camera.ViewportSize.X / camera.ViewportSize.Y;

            IMyEntity controlled;
            if (!IsPlayerControllingWorkingBlock(out controlled) || activeTargets_.Count == 0)
            {
                HideAllSlots();
                return;
            }

            // Prune grids destroyed/closed since the last scan before sorting
            for (int i = activeTargets_.Count - 1; i >= 0; i--)
            {
                var entry = activeTargets_[i];
                var g = entry.Grid_;
                if (g == null || g.Closed || g.Physics == null || g.PositionComp == null)
                {
                    entry.Grid_ = null;
                    targetEntryPool_.Add(entry);
                    activeTargets_.RemoveAt(i);
                }
            }

            if (activeTargets_.Count == 0)
            {
                HideAllSlots();
                return;
            }

            var viewProj = camera.ViewMatrix * camera.ProjectionMatrix;
            var camMat = camera.WorldMatrix;
            var camPos = camera.Position;

            // Targets are already sorted farthest-first at scan time (see ScanForTargets)

            // Draw all targets using pooled HUD slots
            activeHudSlots_ = 0;
            for (int i = 0; i < activeTargets_.Count; i++)
            {
                var entry = activeTargets_[i];
                if (!ShouldDisplayRelation(entry.Relation_))
                    continue;

                if (activeHudSlots_ >= hudPool_.Count)
                {
                    var slot = new TargetHudSlot();
                    slot.Initialize(MAT_SQUARE, LABEL_TEXT_SIZE);
                    hudPool_.Add(slot);
                }
                DrawTarget(entry, ref viewProj, ref camMat, ref camPos, hudPool_[activeHudSlots_]);
                activeHudSlots_++;
            }

            // Hide any slots not used this frame
            for (int i = activeHudSlots_; i < hudPool_.Count; i++)
                hudPool_[i].Hide();
        }

        private bool ShouldDisplayRelation(MyRelationsBetweenPlayerAndBlock relation)
        {
            if (currentMode_ == RadarMode.Enemies)
                return relation == MyRelationsBetweenPlayerAndBlock.Enemies;

            if (currentMode_ == RadarMode.EnemiesAndNeutrals)
                return !IsFriendlyRelation(relation);

            return currentMode_ == RadarMode.All;
        }

        private static bool IsFriendlyRelation(MyRelationsBetweenPlayerAndBlock relation)
        {
            return relation == MyRelationsBetweenPlayerAndBlock.Owner ||
                   relation == MyRelationsBetweenPlayerAndBlock.FactionShare ||
                   relation == MyRelationsBetweenPlayerAndBlock.Friends;
        }

        private static MyRelationsBetweenPlayerAndBlock GetGridRelation(IMyCubeGrid grid, IMyPlayer player)
        {
            if (grid.BigOwners == null || grid.BigOwners.Count == 0 || grid.BigOwners[0] == 0)
                return MyRelationsBetweenPlayerAndBlock.NoOwnership;
            return player.GetRelationTo(grid.BigOwners[0]);
        }

        private void DrawTarget(TargetEntry entry, ref MatrixD viewProj, ref MatrixD camMat, ref Vector3D camPos, TargetHudSlot slot)
        {
            var grid = entry.Grid_;
            if (grid == null || grid.Closed || grid.Physics == null || grid.PositionComp == null) { slot.Hide(); return; }

            var pos = grid.PositionComp.WorldAABB.Center;
            var sc  = Vector3D.Transform(pos, viewProj);

            if (sc.Z > 1) { slot.Hide(); return; }
            if (sc.X > 1.1 || sc.X < -1.1 || sc.Y > 1.1 || sc.Y < -1.1) { slot.Hide(); return; }

            var cx = sc.X;
            var cy = sc.Y;

            var objSize       = grid.PositionComp.LocalVolume.Radius * 1.1f;
            var topRightWorld = Vector3D.Transform(pos + camMat.Up * objSize + camMat.Right * objSize, viewProj);
            float bw = Math.Max((float)Math.Abs(topRightWorld.X - cx), MIN_BRACKET_SIZE);
            float bh = Math.Max((float)Math.Abs(topRightWorld.Y - cy), MIN_BRACKET_SIZE * aspectRatio_);

            if (bw > 1.0f || bh > 1.0f) { slot.Hide(); return; }

            DrawBox(cx, cy, bw, bh, entry.BoxColor_, slot);

            // --- Labels (reuse cached StringBuilders and HUDMessage objects with zero string interpolation) ---
            double km        = Vector3D.Distance(pos, camPos) / 1000.0;
            int    gridSpeed = (int)Math.Round(grid.Physics.LinearVelocity.Length());
            int    kmHundredths = (int)Math.Round(km * 100.0);
            int    kmWhole   = kmHundredths / 100;
            int    kmFrac    = kmHundredths % 100;

            slot.nameSb_.Clear();
            slot.nameSb_.Append("<color=").Append(entry.HexColor_).Append('>').Append(entry.SanitizedLabel_);
            slot.nameLabel_.Visible = true;

            // Line height only depends on font and scale, so measure it once instead of every frame.
            // Stays 0 (and re-measures next frame) if the API hasn't laid out the text yet.
            if (nameLineHeight_ <= 0)
                nameLineHeight_ = Math.Abs(slot.nameLabel_.GetTextLength().Y);

            slot.infoSb_.Clear();
            slot.infoSb_.Append("<color=").Append(entry.HexColor_).Append('>')
                        .Append(kmWhole).Append('.');
            if (kmFrac < 10) slot.infoSb_.Append('0');
            slot.infoSb_.Append(kmFrac).Append("km  ").Append(gridSpeed).Append("m/s");
            slot.infoLabel_.Visible = true;

            double textOriginX = cx + bw + 0.005;
            slot.nameLabel_.Origin = new Vector2D(textOriginX, cy + nameLineHeight_);
            slot.infoLabel_.Origin = new Vector2D(textOriginX, cy);
        }

        // Updates the four billboard sides of a pooled TargetHudSlot rectangle.
        private void DrawBox(double cx, double cy, float bw, float bh, Color boxColor, TargetHudSlot slot)
        {
            float ty = LINE_THICKNESS * aspectRatio_;
            float sideHeight = Math.Max(0f, bh * 2f - ty);
            float sideWidth  = bw * 2f + LINE_THICKNESS;

            slot.boxTop_.Origin    = new Vector2D(cx,      cy + bh); slot.boxTop_.Width    = sideWidth;      slot.boxTop_.Height    = ty;         slot.boxTop_.BillBoardColor    = boxColor; slot.boxTop_.Visible    = true;
            slot.boxBottom_.Origin = new Vector2D(cx,      cy - bh); slot.boxBottom_.Width = sideWidth;      slot.boxBottom_.Height = ty;         slot.boxBottom_.BillBoardColor = boxColor; slot.boxBottom_.Visible = true;
            slot.boxLeft_.Origin   = new Vector2D(cx - bw, cy);      slot.boxLeft_.Width   = LINE_THICKNESS; slot.boxLeft_.Height   = sideHeight; slot.boxLeft_.BillBoardColor   = boxColor; slot.boxLeft_.Visible   = true;
            slot.boxRight_.Origin  = new Vector2D(cx + bw, cy);      slot.boxRight_.Width  = LINE_THICKNESS; slot.boxRight_.Height  = sideHeight; slot.boxRight_.BillBoardColor  = boxColor; slot.boxRight_.Visible  = true;
        }

        private void ScanForTargets(IMyEntity controlled)
        {
            ClearActiveTargets();

            var player = MyAPIGateway.Session?.Player;
            if (player == null) return;

            if (controlled == null && !DEBUG_ALWAYS_SHOW_RADAR) return;

            IMyCubeGrid myGrid = null;
            if (controlled is IMyCubeBlock)
                myGrid = ((IMyCubeBlock)controlled).CubeGrid;
            else if (controlled is IMyCubeGrid)
                myGrid = (IMyCubeGrid)controlled;

            float scanRange = GetDynamicScanRange(myGrid);

            // Logical group = mechanical links plus locked connectors, i.e. ships docked to us
            myDockedGridIds_.Clear();
            if (myGrid != null)
            {
                myLogicalGroupScratch_.Clear();
                MyAPIGateway.GridGroups.GetGroup(myGrid, GridLinkTypeEnum.Logical, myLogicalGroupScratch_);
                for (int i = 0; i < myLogicalGroupScratch_.Count; i++)
                    myDockedGridIds_.Add(myLogicalGroupScratch_[i].EntityId);
            }

            Vector3D centerPos;
            if (controlled != null && controlled.PositionComp != null)
            {
                centerPos = controlled.PositionComp.GetPosition();
            }
            else
            {
                var camera = MyAPIGateway.Session.Camera;
                if (camera == null) return;
                centerPos = camera.Position;
            }

            scanOrigin_ = centerPos;
            var sphere = new BoundingSphereD(centerPos, scanRange);
            var entities = MyAPIGateway.Entities.GetTopMostEntitiesInSphere(ref sphere);

            // Track processed mechanical group keys to avoid marking the same group twice.
            processedGroups_.Clear();

            for (int i = 0; i < entities.Count; i++)
            {
                ProcessEntityAsTarget(entities[i], myGrid, player);
            }

            // Sort farthest-first once per scan so the closest target renders on top
            activeTargets_.Sort(targetDistanceComparer_);
        }

        private float GetDynamicScanRange(IMyCubeGrid myGrid)
        {
            float scanRange = config_.MinRange_; // Minimum configured range
            if (myGrid == null) return Math.Min(scanRange, config_.MaxRange_);

            myGroupScratch_.Clear();
            MyAPIGateway.GridGroups.GetGroup(myGrid, GridLinkTypeEnum.Mechanical, myGroupScratch_);

            // Only walk fat blocks (blocks with an entity) — skips every armor block on the grid
            for (int i = 0; i < myGroupScratch_.Count; i++)
            {
                foreach (var turret in myGroupScratch_[i].GetFatBlocks<Sandbox.ModAPI.IMyLargeTurretBase>())
                {
                    if (!IsLidar(turret)) continue;
                    if (turret.IsWorking && turret.Range > scanRange)
                    {
                        scanRange = turret.Range;
                    }
                }
            }

            if (scanRange > config_.MaxRange_)
            {
                scanRange = config_.MaxRange_;
            }

            return scanRange;
        }

        private static bool IsLidar(IMyCubeBlock block)
        {
            var subtype = block.BlockDefinition.SubtypeName;
            return subtype == SUBTYPE_LARGE_LIDAR || subtype == SUBTYPE_SMALL_LIDAR;
        }

        private void ProcessEntityAsTarget(IMyEntity ent, IMyCubeGrid myGrid, IMyPlayer player)
        {
            var grid = ent as IMyCubeGrid;
            if (grid == null || grid.Closed || grid.Physics == null || (myGrid != null && grid == myGrid)) return;

            // Get the full mechanical group for this grid
            groupScratch_.Clear();
            MyAPIGateway.GridGroups.GetGroup(grid, GridLinkTypeEnum.Mechanical, groupScratch_);
            if (groupScratch_.Count == 0) return;

            // If any part of this mechanical group is the player's controlled grid, skip the whole thing
            if (myGrid != null && groupScratch_.Contains(myGrid)) return;

            // Compute group key and total block count BEFORE expensive mechanical-block scans
            long groupKey = long.MaxValue;
            int totalBlocks = 0;
            for (int i = 0; i < groupScratch_.Count; i++)
            {
                var g = groupScratch_[i];
                if (g.EntityId < groupKey) groupKey = g.EntityId;
                var cubeGrid = g as MyCubeGrid;
                totalBlocks += cubeGrid?.BlocksCount ?? 0;
            }

            // Hide grids that don't meet the minimum block threshold
            if (totalBlocks < MIN_BLOCK_COUNT) return;

            // Skip if we already processed this mechanical group
            if (!processedGroups_.Add(groupKey)) return;

            // Collect all grids that are someone else's TopGrid (i.e. subgrids)
            subgridIds_.Clear();
            for (int i = 0; i < groupScratch_.Count; i++)
            {
                foreach (var mech in groupScratch_[i].GetFatBlocks<IMyMechanicalConnectionBlock>())
                {
                    if (mech.TopGrid != null)
                        subgridIds_.Add(mech.TopGrid.EntityId);
                }
            }

            // Pick the largest non-subgrid as representative; fall back to largest grid overall if cyclic
            IMyCubeGrid bestRoot = null;
            int bestRootBlocks = -1;
            IMyCubeGrid bestAny = null;
            int bestAnyBlocks = -1;

            for (int i = 0; i < groupScratch_.Count; i++)
            {
                var g = groupScratch_[i];
                if (g == null || g.Closed || g.Physics == null || g.PositionComp == null) continue;

                var cubeGrid = g as MyCubeGrid;
                int count = cubeGrid?.BlocksCount ?? 0;

                if (count > bestAnyBlocks)
                {
                    bestAnyBlocks = count;
                    bestAny = g;
                }
                if (!subgridIds_.Contains(g.EntityId) && count > bestRootBlocks)
                {
                    bestRootBlocks = count;
                    bestRoot = g;
                }
            }

            var representative = bestRoot ?? bestAny;
            if (representative == null) return;

            // Skip ships docked to ours by connector, but only friendly ones so nothing hostile can hide by attaching
            var relation = GetGridRelation(representative, player);
            if (myDockedGridIds_.Contains(representative.EntityId) && IsFriendlyRelation(relation)) return;

            // Voxel LOS check: multi-hit raycast so player's own grid or intervening ships don't mask voxels
            var targetCenter = representative.PositionComp.WorldAABB.Center;
            rayHitsScratch_.Clear();
            MyAPIGateway.Physics.CastRay(scanOrigin_, targetCenter, rayHitsScratch_);
            for (int i = 0; i < rayHitsScratch_.Count; i++)
            {
                if (rayHitsScratch_[i]?.HitEntity is IMyVoxelBase)
                    return; // blocked by voxel
            }

            var entry = RentTargetEntry();
            PopulateTargetEntry(entry, representative, player, relation);
            entry.DistanceSq_ = Vector3D.DistanceSquared(targetCenter, scanOrigin_);
            activeTargets_.Add(entry);
        }

        private void PopulateTargetEntry(TargetEntry entry, IMyCubeGrid grid, IMyPlayer player, MyRelationsBetweenPlayerAndBlock relation)
        {
            entry.Grid_ = grid;

            string ownerStr = "Unowned";

            if (grid.BigOwners != null && grid.BigOwners.Count > 0 && grid.BigOwners[0] != 0)
            {
                var owner = grid.BigOwners[0];
                var myId  = player.IdentityId;

                if (owner == myId)
                {
                    ownerStr = "Self";
                }
                else
                {
                    var faction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(owner);
                    ownerStr = faction != null ? faction.Tag : "Unknown";
                }
            }

            entry.Relation_ = relation;

            switch (relation)
            {
                case MyRelationsBetweenPlayerAndBlock.Owner:
                    entry.BoxColor_ = new Color(30, 255, 30, 220);
                    entry.HexColor_ = "30,255,30";
                    break;
                case MyRelationsBetweenPlayerAndBlock.Friends:
                case MyRelationsBetweenPlayerAndBlock.FactionShare:
                    entry.BoxColor_ = new Color(30, 100, 255, 220);
                    entry.HexColor_ = "30,100,255";
                    break;
                case MyRelationsBetweenPlayerAndBlock.Enemies:
                    entry.BoxColor_ = new Color(255, 30, 30, 220);
                    entry.HexColor_ = "255,30,30";
                    break;
                default:
                    entry.BoxColor_ = new Color(220, 220, 220, 220);
                    entry.HexColor_ = "220,220,220";
                    break;
            }

            // Build sanitized label (strip '<' and '>' to prevent HUD markup injection)
            labelScratch_.Clear();
            AppendSanitized(labelScratch_, ownerStr, MAX_LABEL_LENGTH);
            if (labelScratch_.Length < MAX_LABEL_LENGTH)
                labelScratch_.Append('.');
            AppendSanitized(labelScratch_, grid.DisplayName, MAX_LABEL_LENGTH);
            entry.SanitizedLabel_ = labelScratch_.ToString();
        }

        private static void AppendSanitized(StringBuilder sb, string text, int maxLen)
        {
            if (string.IsNullOrEmpty(text)) return;
            for (int i = 0; i < text.Length && sb.Length < maxLen; i++)
            {
                char c = text[i];
                if (c == '<' || c == '>') continue;
                sb.Append(c);
            }
        }
    }
}
