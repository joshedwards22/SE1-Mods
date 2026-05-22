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

        public RadarConfig()
        {
            MinRange_        = 2500f;
            MaxRange_        = 15000f;
            LargeLidarRange_ = 15000f;
            SmallLidarRange_ = 10000f;
        }

        public void Validate()
        {
            MinRange_        = Math.Max(100f,    Math.Min(MinRange_,        50000f));
            MaxRange_        = Math.Max(MinRange_, Math.Min(MaxRange_,        50000f));
            LargeLidarRange_ = Math.Max(100f,    Math.Min(LargeLidarRange_, 50000f));
            SmallLidarRange_ = Math.Max(100f,    Math.Min(SmallLidarRange_, 50000f));
        }
    }

    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public class Radar : MySessionComponentBase
    {
        private const string CONFIG_FILE = "VanillaRadarConfig.xml";
        private RadarConfig config_ = new RadarConfig();

        // Debug setting to always show radar even outside cockpits/turrets
        private bool debugAlwaysShowRadar_ = false;

        // How often to scan for enemy grids (milliseconds) — 500ms = 2 Hz
        private const int SCAN_INTERVAL_MS = 500;

        // Label text size
        private const double LABEL_TEXT_SIZE = 0.7;

        // Minimum corner bracket size in screen units (so it doesn't get too tiny at long range)
        private const float MIN_BRACKET_SIZE = 0.010f;

        // Minimum block count for a grid to be shown
        private const int MIN_BLOCK_COUNT = 5;

        // Vanilla SE material: a plain solid square, used to draw thin lines
        private static readonly MyStringId MAT_SQUARE = MyStringId.GetOrCompute("Square");

        // Cached block filter delegates — prevents per-scan lambda allocation
        private static readonly Func<IMySlimBlock, bool> IS_TURRET_FILTER = b => b.FatBlock is IMyLargeTurretBase;
        private static readonly Func<IMySlimBlock, bool> IS_MECH_FILTER   = b => b.FatBlock is IMyMechanicalConnectionBlock;

        private HudAPIv2 hudApi_;
        private bool hudApiReady_ = false;
        private int tick_ = 0;
        private int scanTicks_;
        private bool isClient_ = false;
        private float aspectRatio_ = 1f;
        private Vector3D scanOrigin_;  // set each scan, used for LOS raycasts

        private enum RadarMode
        {
            All,
            Enemies,
            EnemiesAndNeutrals,
            None
        }
        private RadarMode currentMode_ = RadarMode.Enemies;

        // List of currently targeted grids
        private List<IMyCubeGrid> activeTargets_ = new List<IMyCubeGrid>();

        // Cached collections to prevent memory allocations (garbage collection) during scans
        private readonly List<IMyCubeGrid> myGroupScratch_ = new List<IMyCubeGrid>();
        private readonly List<IMySlimBlock> myBlocksScratch_ = new List<IMySlimBlock>();
        private readonly HashSet<long> processedGroups_ = new HashSet<long>();
        private readonly List<IMyCubeGrid> groupScratch_ = new List<IMyCubeGrid>();
        private readonly HashSet<long> subgridIds_ = new HashSet<long>();
        private readonly List<IMySlimBlock> mechBlocksScratch_ = new List<IMySlimBlock>();

        // HUD object pool — allocated once on first use, reused every frame
        private readonly List<TargetHudSlot> hudPool_ = new List<TargetHudSlot>();
        private int activeHudSlots_ = 0;

        private class TargetHudSlot
        {
            public readonly StringBuilder nameSb_ = new StringBuilder(32);
            public readonly StringBuilder infoSb_ = new StringBuilder(32);
            public HudAPIv2.HUDMessage nameLabel_;
            public HudAPIv2.HUDMessage infoLabel_;
            public HudAPIv2.BillBoardHUDMessage boxTop_, boxBottom_, boxLeft_, boxRight_;

            public void Initialize(MyStringId mat, double textScale)
            {
                var zero  = Vector2D.Zero;
                var white = Color.White.ToVector4();
                const float T = 0.0015f;
                nameLabel_ = new HudAPIv2.HUDMessage(nameSb_, zero, TimeToLive: -1, Scale: textScale, HideHud: false, Shadowing: true);
                nameLabel_.Visible = false;
                infoLabel_ = new HudAPIv2.HUDMessage(infoSb_, zero, TimeToLive: -1, Scale: textScale, HideHud: false, Shadowing: true);
                infoLabel_.Visible = false;
                boxTop_    = new HudAPIv2.BillBoardHUDMessage(mat, zero, white, Width: T, Height: T, TimeToLive: -1, HideHud: false, Shadowing: true);
                boxTop_.Visible = false;
                boxBottom_ = new HudAPIv2.BillBoardHUDMessage(mat, zero, white, Width: T, Height: T, TimeToLive: -1, HideHud: false, Shadowing: true);
                boxBottom_.Visible = false;
                boxLeft_   = new HudAPIv2.BillBoardHUDMessage(mat, zero, white, Width: T, Height: T, TimeToLive: -1, HideHud: false, Shadowing: true);
                boxLeft_.Visible = false;
                boxRight_  = new HudAPIv2.BillBoardHUDMessage(mat, zero, white, Width: T, Height: T, TimeToLive: -1, HideHud: false, Shadowing: true);
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
        }

        // Hides every slot in the pool — call on any early exit from Draw()
        private void HideAllSlots()
        {
            for (int i = 0; i < hudPool_.Count; i++)
                hudPool_[i].Hide();
        }

        public override void LoadData()
        {
            base.LoadData();
            LoadConfig();
            
            UpdateLidarRange("MyObjectBuilder_LargeGatlingTurret/Lidar", config_.LargeLidarRange_);
            UpdateLidarRange("MyObjectBuilder_LargeGatlingTurret/LidarSmall", config_.SmallLidarRange_);
        }

        private void LoadConfig()
        {
            try
            {
                if (MyAPIGateway.Utilities.FileExistsInWorldStorage(CONFIG_FILE, typeof(Radar)))
                {
                    using (var reader = MyAPIGateway.Utilities.ReadFileInWorldStorage(CONFIG_FILE, typeof(Radar)))
                    {
                        var xml = reader.ReadToEnd();
                        config_ = MyAPIGateway.Utilities.SerializeFromXML<RadarConfig>(xml);
                        config_.Validate();
                    }
                }
                else
                {
                    var xml = MyAPIGateway.Utilities.SerializeToXML(config_);
                    using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage(CONFIG_FILE, typeof(Radar)))
                    {
                        writer.Write(xml);
                    }
                }
            }
            catch (Exception ex)
            {
                MyLog.Default.WriteLineAndConsole($"Vanilla Radar: Error loading config: {ex}");
                config_ = new RadarConfig();
            }
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
            
            if (MyAPIGateway.Session.IsServer)
            {
                Sandbox.Game.MyVisualScriptLogicProvider.BlockBuilt += OnBlockBuilt;
            }

            if (!isClient_) return;

            scanTicks_ = Math.Max(1, (SCAN_INTERVAL_MS * 60) / 1000);
            hudApi_ = new HudAPIv2(OnHudApiReady);
        }

        private void OnBlockBuilt(string typeId, string subtypeId, string gridName, long blockId)
        {
            if (subtypeId == "Lidar" || subtypeId == "LidarSmall")
            {
                var block = MyAPIGateway.Entities.GetEntityById(blockId);
                var turret = block as Sandbox.ModAPI.IMyLargeTurretBase;
                if (turret != null)
                {
                    var def = turret.SlimBlock.BlockDefinition as MyLargeTurretBaseDefinition;
                    if (def != null)
                    {
                        turret.Range = def.MaxRangeMeters;
                    }
                }
            }
        }

        protected override void UnloadData()
        {
            if (MyAPIGateway.Session != null && MyAPIGateway.Session.IsServer)
            {
                Sandbox.Game.MyVisualScriptLogicProvider.BlockBuilt -= OnBlockBuilt;
            }

            if (hudApi_ != null)
                hudApi_.Unload();

            base.UnloadData();
        }

        private void OnHudApiReady()
        {
            hudApiReady_ = true;
        }

        public override void HandleInput()
        {
            base.HandleInput();
            if (!isClient_) return;
            if (MyAPIGateway.Input.IsNewKeyPressed(VRage.Input.MyKeys.R) && MyAPIGateway.Input.IsAnyCtrlKeyPressed())
            {
                currentMode_ = (RadarMode)(((int)currentMode_ + 1) % 4);
                string modeLabel;
                switch (currentMode_)
                {
                    case RadarMode.All:                modeLabel = "Show All";                 break;
                    case RadarMode.Enemies:            modeLabel = "Show Enemies";             break;
                    case RadarMode.EnemiesAndNeutrals: modeLabel = "Show Enemies and Neutrals"; break;
                    case RadarMode.None:               modeLabel = "Radar Off";                break;
                    default:                           modeLabel = currentMode_.ToString();     break;
                }
                MyAPIGateway.Utilities.ShowNotification($"Radar Mode: {modeLabel}", 2000, "White");
            }
        }

        public override void Draw()
        {
            if (!isClient_ || !hudApiReady_ || MyAPIGateway.Session?.Player == null)
                return;

            if (currentMode_ == RadarMode.None)
            {
                activeTargets_.Clear();
                HideAllSlots();
                return;
            }

            var camera = MyAPIGateway.Session.Camera;
            if (camera == null) return;

            // Update aspect ratio every frame to handle resolution/window changes
            aspectRatio_ = camera.ViewportSize.X / camera.ViewportSize.Y;

            // Only operate if the player is actively controlling a powered ship controller (cockpit, etc.) or a turret
            var playerEnt = MyAPIGateway.Session.Player.Controller?.ControlledEntity?.Entity;
            var shipController = playerEnt as Sandbox.ModAPI.IMyShipController;
            var turretController = playerEnt as Sandbox.ModAPI.IMyLargeTurretBase;
            var customTurretController = playerEnt as SpaceEngineers.Game.ModAPI.IMyTurretControlBlock;

            bool isControllerWorking = (shipController != null && shipController.IsWorking) || 
                                       (turretController != null && turretController.IsWorking) ||
                                       (customTurretController != null && customTurretController.IsWorking);

            if (!isControllerWorking && !debugAlwaysShowRadar_)
            {
                activeTargets_.Clear();
                HideAllSlots();
                return;
            }

            // Throttle scan only; drawing happens every render frame
            if (++tick_ >= scanTicks_)
            {
                tick_ = 0;
                ScanForTargets(camera);
            }

            if (activeTargets_.Count == 0)
            {
                HideAllSlots();
                return;
            }

            var viewProj = camera.ViewMatrix * camera.ProjectionMatrix;
            var camMat = camera.WorldMatrix;
            var camPos = camera.Position;

            // Sort targets by distance descending so closest renders on top
            activeTargets_.Sort((a, b) =>
            {
                if (a == null || b == null) return 0;
                double dA = Vector3D.DistanceSquared(a.PositionComp.WorldAABB.Center, camPos);
                double dB = Vector3D.DistanceSquared(b.PositionComp.WorldAABB.Center, camPos);
                return dB.CompareTo(dA);
            });

            // Draw all targets using pooled HUD slots
            activeHudSlots_ = 0;
            foreach (var grid in activeTargets_)
            {
                if (activeHudSlots_ >= hudPool_.Count)
                {
                    var slot = new TargetHudSlot();
                    slot.Initialize(MAT_SQUARE, LABEL_TEXT_SIZE);
                    hudPool_.Add(slot);
                }
                DrawTarget(grid, ref viewProj, ref camMat, ref camPos, hudPool_[activeHudSlots_]);
                activeHudSlots_++;
            }
            // Hide any slots not used this frame
            for (int i = activeHudSlots_; i < hudPool_.Count; i++)
                hudPool_[i].Hide();
        }

        private void DrawTarget(IMyCubeGrid grid, ref MatrixD viewProj, ref MatrixD camMat, ref Vector3D camPos, TargetHudSlot slot)
        {
            if (grid == null || grid.Closed || grid.Physics == null) { slot.Hide(); return; }

            var pos = grid.PositionComp.WorldAABB.Center;
            var sc  = Vector3D.Transform(pos, viewProj);

            if (sc.Z > 1) { slot.Hide(); return; }
            if (sc.X > 1.1 || sc.X < -1.1 || sc.Y > 1.1 || sc.Y < -1.1) { slot.Hide(); return; }

            var cx = sc.X;
            var cy = sc.Y;

            var objSize      = grid.PositionComp.LocalVolume.Radius * 1.1f;
            var topRightWorld = Vector3D.Transform(pos + camMat.Up * objSize + camMat.Right * objSize, viewProj);
            float bw = Math.Max((float)Math.Abs(topRightWorld.X - cx), MIN_BRACKET_SIZE);
            float bh = Math.Max((float)Math.Abs(topRightWorld.Y - cy), MIN_BRACKET_SIZE * aspectRatio_);

            if (bw > 1.0f || bh > 1.0f) { slot.Hide(); return; }

            // --- Ownership & relation ---
            string ownerStr = "Unowned";
            MyRelationsBetweenPlayerAndBlock relation = MyRelationsBetweenPlayerAndBlock.NoOwnership;

            if (grid.BigOwners.Count > 0)
            {
                var owner  = grid.BigOwners[0];
                var myId   = MyAPIGateway.Session.Player.IdentityId;
                relation   = MyAPIGateway.Session.Player.GetRelationTo(owner);
                var faction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(owner);

                if (owner == myId)
                {
                    relation  = MyRelationsBetweenPlayerAndBlock.Owner;
                    ownerStr  = "Self";
                }
                else if (faction != null)
                {
                    ownerStr = faction.Tag;
                    var myFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(myId);
                    if (myFaction != null && myFaction.FactionId == faction.FactionId)
                        relation = MyRelationsBetweenPlayerAndBlock.FactionShare;
                    else
                    {
                        var rep = MyAPIGateway.Session.Factions.GetReputationBetweenPlayerAndFaction(myId, faction.FactionId);
                        if      (rep >  500) relation = MyRelationsBetweenPlayerAndBlock.Friends;
                        else if (rep < -500) relation = MyRelationsBetweenPlayerAndBlock.Enemies;
                        else                relation = MyRelationsBetweenPlayerAndBlock.Neutral;
                    }
                }
                else
                    ownerStr = "Unknown";
            }

            // Filter targets based on radar mode
            if (currentMode_ == RadarMode.Enemies &&
                relation != MyRelationsBetweenPlayerAndBlock.Enemies)
            { slot.Hide(); return; }

            if (currentMode_ == RadarMode.EnemiesAndNeutrals &&
                (relation == MyRelationsBetweenPlayerAndBlock.Friends ||
                 relation == MyRelationsBetweenPlayerAndBlock.FactionShare ||
                 relation == MyRelationsBetweenPlayerAndBlock.Owner))
            { slot.Hide(); return; }

            // --- Color ---
            Color  boxColor;
            string hexColor;
            switch (relation)
            {
                case MyRelationsBetweenPlayerAndBlock.Owner:
                    boxColor = new Color(30, 255, 30, 220);  hexColor = "30,255,30";  break;
                case MyRelationsBetweenPlayerAndBlock.Friends:
                case MyRelationsBetweenPlayerAndBlock.FactionShare:
                    boxColor = new Color(30, 100, 255, 220); hexColor = "30,100,255"; break;
                case MyRelationsBetweenPlayerAndBlock.Enemies:
                    boxColor = new Color(255, 30, 30, 220);  hexColor = "255,30,30";  break;
                default:
                    boxColor = new Color(220, 220, 220, 220); hexColor = "220,220,220"; break;
            }

            DrawBox(cx, cy, bw, bh, boxColor, slot);

            // --- Labels (reuse cached StringBuilders and HUDMessage objects) ---
            double km       = Vector3D.Distance(pos, camPos) / 1000.0;
            int    gridSpeed = (int)Math.Round(grid.Physics.LinearVelocity.Length());

            string fullLabelText = $"{ownerStr}.{grid.DisplayName}";
            if (fullLabelText.Length > 20) fullLabelText = fullLabelText.Substring(0, 20);

            slot.nameSb_.Clear();
            slot.nameSb_.Append($"<color={hexColor}>{fullLabelText}");
            slot.nameLabel_.Visible = true;
            var nameLen = slot.nameLabel_.GetTextLength();

            slot.infoSb_.Clear();
            slot.infoSb_.Append($"<color={hexColor}>{km:F2}km  {gridSpeed}m/s");
            slot.infoLabel_.Visible = true;

            double textOriginX = cx + bw + 0.005;
            slot.nameLabel_.Origin = new Vector2D(textOriginX, cy + Math.Abs(nameLen.Y));
            slot.infoLabel_.Origin = new Vector2D(textOriginX, cy);
        }

        // Updates the four billboard sides of a pooled TargetHudSlot rectangle.
        private void DrawBox(double cx, double cy, float bw, float bh, Color boxColor, TargetHudSlot slot)
        {
            const float T = 0.0015f;
            var color = boxColor.ToVector4();

            slot.boxTop_.Origin    = new Vector2D(cx,      cy + bh); slot.boxTop_.Width    = bw * 2 + T; slot.boxTop_.Height    = T;      slot.boxTop_.BillBoardColor    = color; slot.boxTop_.Visible    = true;
            slot.boxBottom_.Origin = new Vector2D(cx,      cy - bh); slot.boxBottom_.Width = bw * 2 + T; slot.boxBottom_.Height = T;      slot.boxBottom_.BillBoardColor = color; slot.boxBottom_.Visible = true;
            slot.boxLeft_.Origin   = new Vector2D(cx - bw, cy);      slot.boxLeft_.Width   = T;          slot.boxLeft_.Height   = bh * 2; slot.boxLeft_.BillBoardColor   = color; slot.boxLeft_.Visible   = true;
            slot.boxRight_.Origin  = new Vector2D(cx + bw, cy);      slot.boxRight_.Width  = T;          slot.boxRight_.Height  = bh * 2; slot.boxRight_.BillBoardColor  = color; slot.boxRight_.Visible  = true;
        }

        private void ScanForTargets(VRage.ModAPI.IMyCamera camera)
        {
            activeTargets_.Clear();

            var player = MyAPIGateway.Session?.Player;
            if (player == null) return;

            var controlled = player.Controller?.ControlledEntity?.Entity;
            if (controlled == null && !debugAlwaysShowRadar_) return;

            IMyCubeGrid myGrid = null;
            if (controlled is IMyCubeBlock)
                myGrid = ((IMyCubeBlock)controlled).CubeGrid;
            else if (controlled is IMyCubeGrid)
                myGrid = (IMyCubeGrid)controlled;

            float scanRange = GetDynamicScanRange(myGrid);


            Vector3D centerPos = controlled != null ? controlled.PositionComp.GetPosition() : camera.Position;
            scanOrigin_ = centerPos;
            var sphere = new BoundingSphereD(centerPos, scanRange);
            var entities = MyAPIGateway.Entities.GetTopMostEntitiesInSphere(ref sphere);

            // We track processed entity IDs to avoid marking the same mechanical group twice.
            processedGroups_.Clear();

            foreach (var ent in entities)
            {
                ProcessEntityAsTarget(ent, myGrid);
            }
        }

        private float GetDynamicScanRange(IMyCubeGrid myGrid)
        {
            float scanRange = config_.MinRange_; // Minimum configured range
            if (myGrid == null) return Math.Min(scanRange, config_.MaxRange_);

            myGroupScratch_.Clear();
            MyAPIGateway.GridGroups.GetGroup(myGrid, GridLinkTypeEnum.Mechanical, myGroupScratch_);
            
            foreach (var g in myGroupScratch_)
            {
                myBlocksScratch_.Clear();
                g.GetBlocks(myBlocksScratch_, IS_TURRET_FILTER);
                foreach (var b in myBlocksScratch_)
                {
                    var turret = b.FatBlock as Sandbox.ModAPI.IMyLargeTurretBase;
                    if (turret != null && turret.IsWorking && turret.Range > scanRange)
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

        private void ProcessEntityAsTarget(IMyEntity ent, IMyCubeGrid myGrid)
        {
            var grid = ent as IMyCubeGrid;
            if (grid == null || grid.Physics == null || (myGrid != null && grid == myGrid)) return;

            // Allow unowned/neutral grids to be processed (they will default to NoOwnership and draw as white)
            // Get the full mechanical group for this grid
            groupScratch_.Clear();
            MyAPIGateway.GridGroups.GetGroup(grid, GridLinkTypeEnum.Mechanical, groupScratch_);

            // If any part of this mechanical group is the player's controlled grid, skip the whole thing
            if (myGrid != null && groupScratch_.Contains(myGrid)) return;

            // Find the root grid via hierarchy: collect all grids that are
            // someone else's TopGrid (i.e. subgrids). The root is the one that isn't.
            subgridIds_.Clear();
            foreach (var g in groupScratch_)
            {
                mechBlocksScratch_.Clear();
                g.GetBlocks(mechBlocksScratch_, IS_MECH_FILTER);
                foreach (var b in mechBlocksScratch_)
                {
                    var mech = b.FatBlock as IMyMechanicalConnectionBlock;
                    if (mech?.TopGrid != null)
                        subgridIds_.Add(mech.TopGrid.EntityId);
                }
            }

            // Root = the grid in the group that is not a subgrid of anything else
            IMyCubeGrid representative = grid; // fallback
            long groupKey = grid.EntityId;
            int totalBlocks = 0;
            
            foreach (var g in groupScratch_)
            {
                var cubeGrid = g as MyCubeGrid;
                totalBlocks += cubeGrid?.BlocksCount ?? 0;
                if (g.EntityId < groupKey) groupKey = g.EntityId;
                if (!subgridIds_.Contains(g.EntityId))
                    representative = g;
            }

            // Hide grids that don't meet the minimum block threshold
            if (totalBlocks < MIN_BLOCK_COUNT) return;

            // Skip if we already added something from this mechanical group
            if (!processedGroups_.Add(groupKey)) return;

            // Voxel LOS check: discard targets occluded by asteroids or planets
            var targetCenter = representative.PositionComp.WorldAABB.Center;
            IHitInfo hitInfo;
            if (MyAPIGateway.Physics.CastRay(scanOrigin_, targetCenter, out hitInfo))
            {
                if (hitInfo?.HitEntity is IMyVoxelBase)
                    return; // blocked by voxel
            }

            activeTargets_.Add(representative);
        }
    }
}
