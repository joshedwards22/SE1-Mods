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
            MinRange = 2500f;
            MaxRange = 15000f;
            LargeLidarRange = 15000f;
            SmallLidarRange = 10000f;
        }
    }

    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public class Radar : MySessionComponentBase
    {
        private const string CONFIG_FILE = "VanillaRadarConfig.xml";
        private RadarConfig config = new RadarConfig();

        // How often to scan for enemy grids (milliseconds)
        private const int SCAN_INTERVAL_MS = 1000;

        // Label text size
        private const double LABEL_TEXT_SIZE = 0.7;

        // Minimum corner bracket size in screen units (so it doesn't get too tiny at long range)
        private const float MIN_BRACKET_SIZE = 0.010f;

        // Minimum block count for a grid to be shown
        private const int MIN_BLOCK_COUNT = 5;

        // Vanilla SE material: a plain solid square, used to draw thin lines
        private static readonly MyStringId MatSquare = MyStringId.GetOrCompute("Square");
        private static readonly Color BracketColor = new Color(255, 30, 30, 220);

        private HudAPIv2 hudApi;
        private bool hudApiReady = false;
        private int tick = 0;
        private int scanTicks;
        private bool isClient = false;
        private float aspectRatio = 0f;

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
        private readonly List<IMySlimBlock> mechBlocksScratch = new List<IMySlimBlock>();

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
                return;
            }

            var camera = MyAPIGateway.Session.Camera;
            if (camera == null) return;

            // Capture aspect ratio once viewport is available
            if (aspectRatio == 0f)
                aspectRatio = camera.ViewportSize.X / camera.ViewportSize.Y;

            // Only operate if the player is actively controlling a powered ship controller (cockpit, etc.) or a turret
            var playerEnt = MyAPIGateway.Session.Player.Controller?.ControlledEntity?.Entity;
            var shipController = playerEnt as Sandbox.ModAPI.IMyShipController;
            var turretController = playerEnt as Sandbox.ModAPI.IMyLargeTurretBase;
            var customTurretController = playerEnt as SpaceEngineers.Game.ModAPI.IMyTurretControlBlock;

            bool isControllerWorking = (shipController != null && shipController.IsWorking) || 
                                       (turretController != null && turretController.IsWorking) ||
                                       (customTurretController != null && customTurretController.IsWorking);

            if (!isControllerWorking)
            {
                activeTargets.Clear();
                return;
            }

            // Throttle scan only; drawing happens every render frame
            tick++;
            if (tick % scanTicks == 0)
                ScanForTargets(camera);

            if (activeTargets.Count == 0) return;

            var viewProj = camera.ViewMatrix * camera.ProjectionMatrix;
            var camMat = camera.WorldMatrix;
            var camPos = camera.Position;

            // Sort targets by distance descending so closest renders on top
            activeTargets.Sort((a, b) => 
            {
                if (a == null || b == null) return 0;
                double distA = Vector3D.DistanceSquared(a.PositionComp.WorldAABB.Center, camPos);
                double distB = Vector3D.DistanceSquared(b.PositionComp.WorldAABB.Center, camPos);
                return distB.CompareTo(distA);
            });

            foreach (var grid in activeTargets)
            {
                DrawTarget(grid, ref viewProj, ref camMat, ref camPos);
            }
        }

        private void DrawTarget(IMyCubeGrid grid, ref MatrixD viewProj, ref MatrixD camMat, ref Vector3D camPos)
        {
            if (grid == null || grid.Closed || grid.Physics == null) return;

            var pos = grid.PositionComp.WorldAABB.Center;
            var sc = Vector3D.Transform(pos, viewProj);

            // Only include targets in front of camera and on screen
            if (sc.Z > 1) return;
            if (sc.X > 1.1 || sc.X < -1.1 || sc.Y > 1.1 || sc.Y < -1.1) return;

            var cx = sc.X;
            var cy = sc.Y;

            // Calculate dynamic box size based on grid's physical size projected to screen
            var objSize = grid.PositionComp.LocalVolume.Radius * 1.1f;
            
            var topRightWorld = Vector3D.Transform(pos + camMat.Up * objSize + camMat.Right * objSize, viewProj);
            float bw = (float)Math.Abs(topRightWorld.X - cx);
            float bh = (float)Math.Abs(topRightWorld.Y - cy);

            // Enforce a minimum size so distant targets don't collapse to a pixel
            bw = Math.Max(bw, MIN_BRACKET_SIZE);
            bh = Math.Max(bh, MIN_BRACKET_SIZE * aspectRatio);

            // If the box is larger than the screen dimensions, don't draw it (too close)
            // In clip space, the screen width and height are 2.0 (-1 to 1). So if half-size (bw, bh) > 1.0, it's bigger than the screen.
            if (bw > 1.0f || bh > 1.0f)
                return;

            // --- Calculate values and relations ---
            string ownerStr = "Unowned";
            MyRelationsBetweenPlayerAndBlock relation = MyRelationsBetweenPlayerAndBlock.NoOwnership;
            
            if (grid.BigOwners.Count > 0)
            {
                var owner = grid.BigOwners[0];
                var myId = MyAPIGateway.Session.Player.IdentityId;
                relation = MyAPIGateway.Session.Player.GetRelationTo(owner);
                var faction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(owner);

                if (owner == myId)
                {
                    relation = MyRelationsBetweenPlayerAndBlock.Owner;
                    ownerStr = "Self";
                }
                else if (faction != null)
                {
                    ownerStr = faction.Tag;
                    var myFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(myId);
                    
                    if (myFaction != null && myFaction.FactionId == faction.FactionId)
                    {
                        relation = MyRelationsBetweenPlayerAndBlock.FactionShare;
                    }
                    else
                    {
                        var reputation = MyAPIGateway.Session.Factions.GetReputationBetweenPlayerAndFaction(myId, faction.FactionId);
                        if (reputation > 500)
                            relation = MyRelationsBetweenPlayerAndBlock.Friends;
                        else if (reputation < -500)
                            relation = MyRelationsBetweenPlayerAndBlock.Enemies;
                        else
                            relation = MyRelationsBetweenPlayerAndBlock.Neutral;
                    }
                }
                else
                {
                    ownerStr = "Unknown";
                }
            }

            if (currentMode == RadarMode.Enemies && 
                (relation == MyRelationsBetweenPlayerAndBlock.Friends || 
                 relation == MyRelationsBetweenPlayerAndBlock.FactionShare || 
                 relation == MyRelationsBetweenPlayerAndBlock.Owner))
                return;

            // Determine color based on relation
            Color boxColor;
            string hexColor;
            switch (relation)
            {
                case MyRelationsBetweenPlayerAndBlock.Owner:
                    boxColor = new Color(30, 255, 30, 220); // Green
                    hexColor = "30,255,30";
                    break;
                case MyRelationsBetweenPlayerAndBlock.Friends:
                case MyRelationsBetweenPlayerAndBlock.FactionShare:
                    boxColor = new Color(30, 100, 255, 220); // Blue
                    hexColor = "30,100,255";
                    break;
                case MyRelationsBetweenPlayerAndBlock.Enemies:
                    boxColor = new Color(255, 30, 30, 220); // Red
                    hexColor = "255,30,30";
                    break;
                default:
                    boxColor = new Color(220, 220, 220, 220); // White/Gray for Neutral
                    hexColor = "220,220,220";
                    break;
            }

            DrawBox(cx, cy, bw, bh, boxColor);

            double km = Vector3D.Distance(pos, camPos) / 1000.0;
            int gridSpeed = grid.Physics != null ? (int)Math.Round(grid.Physics.LinearVelocity.Length()) : 0;

            // --- Build Name Label ---
            string fullLabelText = $"{ownerStr}.{grid.DisplayName}";
            if (fullLabelText.Length > 20)
                fullLabelText = fullLabelText.Substring(0, 20);

            var nameLabelSb = new StringBuilder();
            nameLabelSb.Append($"<color={hexColor}>{fullLabelText}");
            var nameLabel = new HudAPIv2.HUDMessage(
                nameLabelSb,
                Vector2D.Zero,
                TimeToLive: 2,
                Scale: LABEL_TEXT_SIZE,
                HideHud: false,
                Shadowing: true
            );
            var nameLen = nameLabel.GetTextLength();

            // --- Build Info Label (Distance & Speed) ---
            var infoLabelSb = new StringBuilder();
            infoLabelSb.Append($"<color={hexColor}>{km:F2}km  {gridSpeed}m/s");
            var infoLabel = new HudAPIv2.HUDMessage(
                infoLabelSb,
                Vector2D.Zero,
                TimeToLive: 2,
                Scale: LABEL_TEXT_SIZE,
                HideHud: false,
                Shadowing: true
            );
            var infoLen = infoLabel.GetTextLength();

            // Position them on the right side of the box.
            // X-axis: right edge of the box plus a small padding
            double textOriginX = cx + bw + 0.005;
            
            // Y-axis: Text flows down (-) from its Origin.
            // Place the Name label so its bottom edge is at the center line (cy)
            double nameOriginY = cy + Math.Abs(nameLen.Y);
            nameLabel.Origin = new Vector2D(textOriginX, nameOriginY);
            nameLabel.Visible = true;

            // Place the Info label immediately below the Name label (starting at cy and flowing down)
            double infoOriginY = cy;
            infoLabel.Origin = new Vector2D(textOriginX, infoOriginY);
            infoLabel.Visible = true;
        }

        // Draws a thin rectangle outline (4 sides) centered at cx,cy with half-extents bw,bh.
        private void DrawBox(double cx, double cy, float bw, float bh, Color boxColor)
        {
            const float T = 0.0015f; // thin line thickness in screen units
            var color = boxColor.ToVector4();

            // Top
            new HudAPIv2.BillBoardHUDMessage(MatSquare, new Vector2D(cx, cy + bh), color, Width: bw * 2 + T, Height: T, TimeToLive: 2, HideHud: false, Shadowing: true);
            // Bottom
            new HudAPIv2.BillBoardHUDMessage(MatSquare, new Vector2D(cx, cy - bh), color, Width: bw * 2 + T, Height: T, TimeToLive: 2, HideHud: false, Shadowing: true);
            // Left
            new HudAPIv2.BillBoardHUDMessage(MatSquare, new Vector2D(cx - bw, cy), color, Width: T, Height: bh * 2, TimeToLive: 2, HideHud: false, Shadowing: true);
            // Right
            new HudAPIv2.BillBoardHUDMessage(MatSquare, new Vector2D(cx + bw, cy), color, Width: T, Height: bh * 2, TimeToLive: 2, HideHud: false, Shadowing: true);
        }

        private void ScanForTargets(IMyCamera camera)
        {
            activeTargets.Clear();

            var player = MyAPIGateway.Session?.Player;
            if (player == null) return;

            var controlled = player.Controller?.ControlledEntity?.Entity;
            if (controlled == null) return;

            IMyCubeGrid myGrid = null;
            if (controlled is IMyCubeBlock)
                myGrid = ((IMyCubeBlock)controlled).CubeGrid;
            else if (controlled is IMyCubeGrid)
                myGrid = (IMyCubeGrid)controlled;

            float scanRange = GetDynamicScanRange(myGrid);

            var viewProjection = camera.ViewMatrix * camera.ProjectionMatrix;
            var sphere = new BoundingSphereD(controlled.PositionComp.GetPosition(), scanRange);
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
                g.GetBlocks(myBlocksScratch, b => b.FatBlock is Sandbox.ModAPI.IMyLargeTurretBase);
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
                g.GetBlocks(mechBlocksScratch, b => b.FatBlock is IMyMechanicalConnectionBlock);
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
                totalBlocks += ((MyCubeGrid)g).BlocksCount;
                if (g.EntityId < groupKey) groupKey = g.EntityId;
                if (!subgridIds.Contains(g.EntityId))
                    representative = g;
            }

            // Hide grids that don't meet the minimum block threshold
            if (totalBlocks < MIN_BLOCK_COUNT) return;

            // Skip if we already added something from this mechanical group
            if (!processedGroups.Add(groupKey)) return;

            activeTargets.Add(representative);
        }
    }
}
