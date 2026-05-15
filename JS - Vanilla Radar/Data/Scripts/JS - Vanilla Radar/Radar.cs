using System;
using System.Collections.Generic;
using System.Text;
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
        public float MinRange;
        public float MaxRange;
        public float LargeLidarRange;
        public float SmallLidarRange;

        public RadarConfig()
        {
            MinRange        = 2500f;
            MaxRange        = 15000f;
            LargeLidarRange = 15000f;
            SmallLidarRange = 10000f;
        }

        public void Validate()
        {
            MinRange        = Math.Max(100f,    Math.Min(MinRange,        50000f));
            MaxRange        = Math.Max(MinRange, Math.Min(MaxRange,        50000f));
            LargeLidarRange = Math.Max(100f,    Math.Min(LargeLidarRange, 50000f));
            SmallLidarRange = Math.Max(100f,    Math.Min(SmallLidarRange, 50000f));
        }
    }

    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public class Radar : MySessionComponentBase
    {
        private const string CONFIG_FILE = "VanillaRadarConfig.xml";
        private RadarConfig config = new RadarConfig();

        // Debug setting to always show radar even outside cockpits/turrets
        private bool debugAlwaysShowRadar = false;

        // How often to scan for enemy grids (milliseconds) — 500ms = 2 Hz
        private const int SCAN_INTERVAL_MS = 500;

        // Label text size
        private const double LABEL_TEXT_SIZE = 0.7;

        // Minimum corner bracket size in screen units (so it doesn't get too tiny at long range)
        private const float MIN_BRACKET_SIZE = 0.010f;

        // Minimum block count for a grid to be shown
        private const int MIN_BLOCK_COUNT = 5;

        // Vanilla SE material: a plain solid square, used to draw thin lines
        private static readonly MyStringId MatSquare = MyStringId.GetOrCompute("Square");

        // Cached block filter delegates — prevents per-scan lambda allocation
        private static readonly Func<IMySlimBlock, bool> IsTurretFilter = b => b.FatBlock is IMyLargeTurretBase;
        private static readonly Func<IMySlimBlock, bool> IsMechFilter   = b => b.FatBlock is IMyMechanicalConnectionBlock;

        private HudAPIv2 hudApi;
        private bool hudApiReady = false;
        private int tick = 0;
        private int scanTicks;
        private bool isClient = false;
        private float aspectRatio = 1f;
        private Vector3D _scanOrigin;  // set each scan, used for LOS raycasts

        private enum RadarMode
        {
            All,
            Enemies,
            None
        }
        private RadarMode currentMode = RadarMode.All;

        // List of currently targeted grids
        private List<IMyCubeGrid> activeTargets = new List<IMyCubeGrid>();

        // Cached collections to prevent memory allocations (garbage collection) during scans
        private readonly List<IMyCubeGrid> myGroupScratch = new List<IMyCubeGrid>();
        private readonly List<IMySlimBlock> myBlocksScratch = new List<IMySlimBlock>();
        private readonly HashSet<long> processedGroups = new HashSet<long>();
        private readonly List<IMyCubeGrid> groupScratch = new List<IMyCubeGrid>();
        private readonly HashSet<long> subgridIds = new HashSet<long>();
        private readonly List<IMySlimBlock>  mechBlocksScratch = new List<IMySlimBlock>();

        // HUD object pool — allocated once on first use, reused every frame
        private readonly List<TargetHudSlot> _hudPool = new List<TargetHudSlot>();
        private int _activeHudSlots = 0;

        private class TargetHudSlot
        {
            public readonly StringBuilder NameSb = new StringBuilder(32);
            public readonly StringBuilder InfoSb = new StringBuilder(32);
            public HudAPIv2.HUDMessage          NameLabel;
            public HudAPIv2.HUDMessage          InfoLabel;
            public HudAPIv2.BillBoardHUDMessage BoxTop, BoxBottom, BoxLeft, BoxRight;

            public void Initialize(MyStringId mat, double textScale)
            {
                var zero  = Vector2D.Zero;
                var white = Color.White.ToVector4();
                const float T = 0.0015f;
                NameLabel = new HudAPIv2.HUDMessage(NameSb, zero, TimeToLive: -1, Scale: textScale, HideHud: false, Shadowing: true);
                NameLabel.Visible = false;
                InfoLabel = new HudAPIv2.HUDMessage(InfoSb, zero, TimeToLive: -1, Scale: textScale, HideHud: false, Shadowing: true);
                InfoLabel.Visible = false;
                BoxTop    = new HudAPIv2.BillBoardHUDMessage(mat, zero, white, Width: T, Height: T, TimeToLive: -1, HideHud: false, Shadowing: true);
                BoxTop.Visible = false;
                BoxBottom = new HudAPIv2.BillBoardHUDMessage(mat, zero, white, Width: T, Height: T, TimeToLive: -1, HideHud: false, Shadowing: true);
                BoxBottom.Visible = false;
                BoxLeft   = new HudAPIv2.BillBoardHUDMessage(mat, zero, white, Width: T, Height: T, TimeToLive: -1, HideHud: false, Shadowing: true);
                BoxLeft.Visible = false;
                BoxRight  = new HudAPIv2.BillBoardHUDMessage(mat, zero, white, Width: T, Height: T, TimeToLive: -1, HideHud: false, Shadowing: true);
                BoxRight.Visible = false;
            }

            public void Hide()
            {
                if (NameLabel != null) NameLabel.Visible = false;
                if (InfoLabel != null) InfoLabel.Visible = false;
                if (BoxTop    != null) BoxTop.Visible    = false;
                if (BoxBottom != null) BoxBottom.Visible = false;
                if (BoxLeft   != null) BoxLeft.Visible   = false;
                if (BoxRight  != null) BoxRight.Visible  = false;
            }
        }

        // Hides every slot in the pool — call on any early exit from Draw()
        private void HideAllSlots()
        {
            for (int i = 0; i < _hudPool.Count; i++)
                _hudPool[i].Hide();
        }

        public override void LoadData()
        {
            base.LoadData();
            LoadConfig();
            
            UpdateLidarRange("MyObjectBuilder_LargeGatlingTurret/Lidar", config.LargeLidarRange);
            UpdateLidarRange("MyObjectBuilder_LargeGatlingTurret/LidarSmall", config.SmallLidarRange);
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
                        config = MyAPIGateway.Utilities.SerializeFromXML<RadarConfig>(xml);
                        config.Validate();
                    }
                }
                else
                {
                    var xml = MyAPIGateway.Utilities.SerializeToXML(config);
                    using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage(CONFIG_FILE, typeof(Radar)))
                    {
                        writer.Write(xml);
                    }
                }
            }
            catch (Exception ex)
            {
                MyLog.Default.WriteLineAndConsole($"Vanilla Radar: Error loading config: {ex}");
                config = new RadarConfig();
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
            isClient = !MyAPIGateway.Utilities.IsDedicated;
            
            if (MyAPIGateway.Session.IsServer)
            {
                Sandbox.Game.MyVisualScriptLogicProvider.BlockBuilt += OnBlockBuilt;
            }

            if (!isClient) return;

            scanTicks = Math.Max(1, (SCAN_INTERVAL_MS * 60) / 1000);
            hudApi = new HudAPIv2(OnHudApiReady);
        }

        private void OnBlockBuilt(string typeId, string subtypeId, string gridName, long blockId)
        {
            if (subtypeId == "Lidar" || subtypeId == "LidarSmall")
            {
                var block = MyEntities.GetEntityById(blockId);
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

            if (hudApi != null)
                hudApi.Unload();
        }

        private void OnHudApiReady()
        {
            hudApiReady = true;
        }

        public override void HandleInput()
        {
            base.HandleInput();
            if (!isClient) return;
            if (MyAPIGateway.Input.IsNewKeyPressed(VRage.Input.MyKeys.R) && MyAPIGateway.Input.IsAnyCtrlKeyPressed())
            {
                currentMode = (RadarMode)(((int)currentMode + 1) % 3);
                MyAPIGateway.Utilities.ShowNotification($"Radar Mode: {currentMode}", 2000, "White");
            }
        }

        public override void Draw()
        {
            if (!isClient || !hudApiReady || MyAPIGateway.Session?.Player == null)
                return;

            if (currentMode == RadarMode.None)
            {
                activeTargets.Clear();
                HideAllSlots();
                return;
            }

            var camera = MyAPIGateway.Session.Camera;
            if (camera == null) return;

            // Update aspect ratio every frame to handle resolution/window changes
            aspectRatio = camera.ViewportSize.X / camera.ViewportSize.Y;

            // Only operate if the player is actively controlling a powered ship controller (cockpit, etc.) or a turret
            var playerEnt = MyAPIGateway.Session.Player.Controller?.ControlledEntity?.Entity;
            var shipController = playerEnt as Sandbox.ModAPI.IMyShipController;
            var turretController = playerEnt as Sandbox.ModAPI.IMyLargeTurretBase;
            var customTurretController = playerEnt as SpaceEngineers.Game.ModAPI.IMyTurretControlBlock;

            bool isControllerWorking = (shipController != null && shipController.IsWorking) || 
                                       (turretController != null && turretController.IsWorking) ||
                                       (customTurretController != null && customTurretController.IsWorking);

            if (!isControllerWorking && !debugAlwaysShowRadar)
            {
                activeTargets.Clear();
                HideAllSlots();
                return;
            }

            // Throttle scan only; drawing happens every render frame
            if (++tick >= scanTicks)
            {
                tick = 0;
                ScanForTargets(camera);
            }

            if (activeTargets.Count == 0)
            {
                HideAllSlots();
                return;
            }

            var viewProj = camera.ViewMatrix * camera.ProjectionMatrix;
            var camMat = camera.WorldMatrix;
            var camPos = camera.Position;

            // Sort targets by distance descending so closest renders on top
            activeTargets.Sort((a, b) =>
            {
                if (a == null || b == null) return 0;
                double dA = Vector3D.DistanceSquared(a.PositionComp.WorldAABB.Center, camPos);
                double dB = Vector3D.DistanceSquared(b.PositionComp.WorldAABB.Center, camPos);
                return dB.CompareTo(dA);
            });

            // Draw all targets using pooled HUD slots
            _activeHudSlots = 0;
            foreach (var grid in activeTargets)
            {
                if (_activeHudSlots >= _hudPool.Count)
                {
                    var slot = new TargetHudSlot();
                    slot.Initialize(MatSquare, LABEL_TEXT_SIZE);
                    _hudPool.Add(slot);
                }
                DrawTarget(grid, ref viewProj, ref camMat, ref camPos, _hudPool[_activeHudSlots]);
                _activeHudSlots++;
            }
            // Hide any slots not used this frame
            for (int i = _activeHudSlots; i < _hudPool.Count; i++)
                _hudPool[i].Hide();
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
            float bh = Math.Max((float)Math.Abs(topRightWorld.Y - cy), MIN_BRACKET_SIZE * aspectRatio);

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

            if (currentMode == RadarMode.Enemies &&
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

            slot.NameSb.Clear();
            slot.NameSb.Append($"<color={hexColor}>{fullLabelText}");
            slot.NameLabel.Visible = true;
            var nameLen = slot.NameLabel.GetTextLength();

            slot.InfoSb.Clear();
            slot.InfoSb.Append($"<color={hexColor}>{km:F2}km  {gridSpeed}m/s");
            slot.InfoLabel.Visible = true;

            double textOriginX = cx + bw + 0.005;
            slot.NameLabel.Origin = new Vector2D(textOriginX, cy + Math.Abs(nameLen.Y));
            slot.InfoLabel.Origin = new Vector2D(textOriginX, cy);
        }

        // Updates the four billboard sides of a pooled TargetHudSlot rectangle.
        private void DrawBox(double cx, double cy, float bw, float bh, Color boxColor, TargetHudSlot slot)
        {
            const float T = 0.0015f;
            var color = boxColor.ToVector4();

            slot.BoxTop.Origin    = new Vector2D(cx,      cy + bh); slot.BoxTop.Width    = bw * 2 + T; slot.BoxTop.Height    = T;      slot.BoxTop.BillBoardColor    = color; slot.BoxTop.Visible    = true;
            slot.BoxBottom.Origin = new Vector2D(cx,      cy - bh); slot.BoxBottom.Width = bw * 2 + T; slot.BoxBottom.Height = T;      slot.BoxBottom.BillBoardColor = color; slot.BoxBottom.Visible = true;
            slot.BoxLeft.Origin   = new Vector2D(cx - bw, cy);      slot.BoxLeft.Width   = T;          slot.BoxLeft.Height   = bh * 2; slot.BoxLeft.BillBoardColor   = color; slot.BoxLeft.Visible   = true;
            slot.BoxRight.Origin  = new Vector2D(cx + bw, cy);      slot.BoxRight.Width  = T;          slot.BoxRight.Height  = bh * 2; slot.BoxRight.BillBoardColor  = color; slot.BoxRight.Visible  = true;
        }

        private void ScanForTargets(IMyCamera camera)
        {
            activeTargets.Clear();

            var player = MyAPIGateway.Session?.Player;
            if (player == null) return;

            var controlled = player.Controller?.ControlledEntity?.Entity;
            if (controlled == null && !debugAlwaysShowRadar) return;

            IMyCubeGrid myGrid = null;
            if (controlled is IMyCubeBlock)
                myGrid = ((IMyCubeBlock)controlled).CubeGrid;
            else if (controlled is IMyCubeGrid)
                myGrid = (IMyCubeGrid)controlled;

            float scanRange = GetDynamicScanRange(myGrid);


            Vector3D centerPos = controlled != null ? controlled.PositionComp.GetPosition() : camera.Position;
            _scanOrigin = centerPos;
            var sphere = new BoundingSphereD(centerPos, scanRange);
            var entities = MyAPIGateway.Entities.GetTopMostEntitiesInSphere(ref sphere);

            // We track processed entity IDs to avoid marking the same mechanical group twice.
            processedGroups.Clear();

            foreach (var ent in entities)
            {
                ProcessEntityAsTarget(ent, myGrid);
            }
        }

        private float GetDynamicScanRange(IMyCubeGrid myGrid)
        {
            float scanRange = config.MinRange; // Minimum configured range
            if (myGrid == null) return Math.Min(scanRange, config.MaxRange);

            myGroupScratch.Clear();
            MyAPIGateway.GridGroups.GetGroup(myGrid, GridLinkTypeEnum.Mechanical, myGroupScratch);
            
            foreach (var g in myGroupScratch)
            {
                myBlocksScratch.Clear();
                g.GetBlocks(myBlocksScratch, IsTurretFilter);
                foreach (var b in myBlocksScratch)
                {
                    var turret = b.FatBlock as Sandbox.ModAPI.IMyLargeTurretBase;
                    if (turret != null && turret.IsWorking && turret.Range > scanRange)
                    {
                        scanRange = turret.Range;
                    }
                }
            }

            if (scanRange > config.MaxRange)
            {
                scanRange = config.MaxRange;
            }

            return scanRange;
        }

        private void ProcessEntityAsTarget(IMyEntity ent, IMyCubeGrid myGrid)
        {
            var grid = ent as IMyCubeGrid;
            if (grid == null || grid.Physics == null || (myGrid != null && grid == myGrid)) return;

            // Allow unowned/neutral grids to be processed (they will default to NoOwnership and draw as white)
            // Get the full mechanical group for this grid
            groupScratch.Clear();
            MyAPIGateway.GridGroups.GetGroup(grid, GridLinkTypeEnum.Mechanical, groupScratch);

            // If any part of this mechanical group is the player's controlled grid, skip the whole thing
            if (myGrid != null && groupScratch.Contains(myGrid)) return;

            // Find the root grid via hierarchy: collect all grids that are
            // someone else's TopGrid (i.e. subgrids). The root is the one that isn't.
            subgridIds.Clear();
            foreach (var g in groupScratch)
            {
                mechBlocksScratch.Clear();
                g.GetBlocks(mechBlocksScratch, IsMechFilter);
                foreach (var b in mechBlocksScratch)
                {
                    var mech = b.FatBlock as IMyMechanicalConnectionBlock;
                    if (mech?.TopGrid != null)
                        subgridIds.Add(mech.TopGrid.EntityId);
                }
            }

            // Root = the grid in the group that is not a subgrid of anything else
            IMyCubeGrid representative = grid; // fallback
            long groupKey = grid.EntityId;
            int totalBlocks = 0;
            
            foreach (var g in groupScratch)
            {
                var cubeGrid = g as MyCubeGrid;
                totalBlocks += cubeGrid?.BlocksCount ?? 0;
                if (g.EntityId < groupKey) groupKey = g.EntityId;
                if (!subgridIds.Contains(g.EntityId))
                    representative = g;
            }

            // Hide grids that don't meet the minimum block threshold
            if (totalBlocks < MIN_BLOCK_COUNT) return;

            // Skip if we already added something from this mechanical group
            if (!processedGroups.Add(groupKey)) return;

            // Voxel LOS check: discard targets occluded by asteroids or planets
            var targetCenter = representative.PositionComp.WorldAABB.Center;
            IHitInfo hitInfo;
            if (MyAPIGateway.Physics.CastRay(_scanOrigin, targetCenter, out hitInfo))
            {
                if (hitInfo?.HitEntity is IMyVoxelBase)
                    return; // blocked by voxel
            }

            activeTargets.Add(representative);
        }
    }
}
