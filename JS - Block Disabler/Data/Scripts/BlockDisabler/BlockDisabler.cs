using Sandbox.Definitions;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Utils;

namespace JSBlockDisabler
{
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class JSBlockDisabler : MySessionComponentBase
    {
        private const string FILE_NAME = "Data\\DisabledBlocks.txt";
        private const string STORAGE_FILE_NAME = "DisabledBlocks.txt";
        private const ushort NETWORK_MESSAGE_ID = 47291;

        private readonly HashSet<MyCubeBlockDefinition> disabledBlocks_ = new HashSet<MyCubeBlockDefinition>();
        private readonly HashSet<string> worldStorageLines_ = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<IMyCubeGrid> trackedGrids_ = new HashSet<IMyCubeGrid>();
        private bool networkHandlerRegistered_ = false;
        private bool entityAddRegistered_ = false;

        public override void LoadData()
        {
            base.LoadData();

            try
            {
                if (MyAPIGateway.Multiplayer != null)
                {
                    MyAPIGateway.Multiplayer.RegisterSecureMessageHandler(NETWORK_MESSAGE_ID, OnNetworkMessageReceived);
                    networkHandlerRegistered_ = true;
                }

                HashSet<string> allLinesToDisable = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                if (MyAPIGateway.Session?.Mods != null)
                {
                    foreach (var mod in MyAPIGateway.Session.Mods)
                    {
                        if (MyAPIGateway.Utilities.FileExistsInModLocation(FILE_NAME, mod))
                        {
                            MyLog.Default.WriteLineAndConsole($"BlockDisabler: Found {FILE_NAME} in mod {mod.FriendlyName} ({mod.PublishedFileId})");
                            ReadLinesFromMod(mod, allLinesToDisable);
                        }
                    }
                }

                if (IsServer())
                {
                    if (MyAPIGateway.Utilities.FileExistsInWorldStorage(STORAGE_FILE_NAME, typeof(JSBlockDisabler)))
                    {
                        MyLog.Default.WriteLineAndConsole($"BlockDisabler: Found {STORAGE_FILE_NAME} in world storage");
                        ReadLinesFromWorldStorage(worldStorageLines_);
                        foreach (var line in worldStorageLines_)
                        {
                            allLinesToDisable.Add(line);
                        }
                    }
                    else
                    {
                        MyLog.Default.WriteLineAndConsole($"BlockDisabler: Creating {STORAGE_FILE_NAME} in world storage");
                        CreateWorldStorageFile();
                    }
                }

                foreach (var line in allLinesToDisable)
                {
                    DisableBlock(line);
                }
            }
            catch (Exception ex)
            {
                MyLog.Default.WriteLineAndConsole($"BlockDisabler: Critical error in LoadData: {ex}");
            }

            if (disabledBlocks_.Count > 0)
            {
                ProcessVariantGroups();
            }
        }

        public override void BeforeStart()
        {
            base.BeforeStart();

            if (IsServer())
            {
                if (disabledBlocks_.Count > 0)
                {
                    RemoveDisabledBlocksFromWorld();
                    if (MyAPIGateway.Entities != null)
                    {
                        MyAPIGateway.Entities.OnEntityAdd += OnEntityAdd;
                        entityAddRegistered_ = true;
                    }
                }
            }
            else if (MyAPIGateway.Multiplayer != null)
            {
                // Request server's world-storage disabled block list so client G-menu stays in sync
                MyAPIGateway.Multiplayer.SendMessageToServer(NETWORK_MESSAGE_ID, new byte[0]);
            }
        }

        protected override void UnloadData()
        {
            if (entityAddRegistered_ && MyAPIGateway.Entities != null)
            {
                MyAPIGateway.Entities.OnEntityAdd -= OnEntityAdd;
                entityAddRegistered_ = false;
            }

            foreach (var grid in trackedGrids_)
            {
                if (grid != null)
                {
                    grid.OnBlockAdded -= OnGridBlockAdded;
                    grid.OnMarkForClose -= OnGridMarkForClose;
                }
            }
            trackedGrids_.Clear();

            if (networkHandlerRegistered_ && MyAPIGateway.Multiplayer != null)
            {
                MyAPIGateway.Multiplayer.UnregisterSecureMessageHandler(NETWORK_MESSAGE_ID, OnNetworkMessageReceived);
                networkHandlerRegistered_ = false;
            }

            disabledBlocks_.Clear();
            worldStorageLines_.Clear();

            base.UnloadData();
        }

        private bool IsServer()
        {
            return MyAPIGateway.Session == null || MyAPIGateway.Session.IsServer;
        }

        private void OnNetworkMessageReceived(ushort handlerId, byte[] data, ulong senderSteamId, bool fromServer)
        {
            try
            {
                if (IsServer())
                {
                    if (worldStorageLines_.Count == 0 || MyAPIGateway.Multiplayer == null)
                        return;

                    string payloadText = string.Join("\n", worldStorageLines_);
                    byte[] payload = Encoding.UTF8.GetBytes(payloadText);
                    MyAPIGateway.Multiplayer.SendMessageTo(NETWORK_MESSAGE_ID, payload, senderSteamId);
                }
                else if (fromServer && data != null && data.Length > 0)
                {
                    string payloadText = Encoding.UTF8.GetString(data);
                    bool anyNewDisabled = false;

                    // StringReader is prohibited by the mod script whitelist, so split the payload manually
                    HashSet<string> receivedLines = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    foreach (var rawLine in payloadText.Split('\n'))
                    {
                        AddCleanLine(rawLine, receivedLines);
                    }

                    foreach (var line in receivedLines)
                    {
                        if (DisableBlock(line))
                        {
                            anyNewDisabled = true;
                        }
                    }

                    if (anyNewDisabled)
                    {
                        ProcessVariantGroups();
                    }
                }
            }
            catch (Exception ex)
            {
                MyLog.Default.WriteLineAndConsole($"BlockDisabler: Error handling network message: {ex}");
            }
        }

        private void OnEntityAdd(IMyEntity entity)
        {
            if (!IsServer() || disabledBlocks_.Count == 0 || entity == null || entity.Closed || entity.MarkedForClose)
                return;

            var grid = entity as IMyCubeGrid;
            if (grid == null)
                return;

            MyAPIGateway.Utilities.InvokeOnGameThread(() =>
            {
                if (grid == null || grid.Closed || grid.MarkedForClose || grid.Physics == null)
                    return;

                TrackGrid(grid);
                ProcessGrid(grid);
            });
        }

        private void TrackGrid(IMyCubeGrid grid)
        {
            if (grid == null || grid.Closed || grid.MarkedForClose)
                return;

            if (trackedGrids_.Add(grid))
            {
                grid.OnBlockAdded += OnGridBlockAdded;
                grid.OnMarkForClose += OnGridMarkForClose;
            }
        }

        private void OnGridMarkForClose(IMyEntity entity)
        {
            var grid = entity as IMyCubeGrid;
            if (grid == null)
                return;

            grid.OnBlockAdded -= OnGridBlockAdded;
            grid.OnMarkForClose -= OnGridMarkForClose;
            trackedGrids_.Remove(grid);
        }

        private void OnGridBlockAdded(IMySlimBlock block)
        {
            if (!IsServer() || disabledBlocks_.Count == 0 || block == null || block.IsDestroyed)
                return;

            var cubeDef = block.BlockDefinition as MyCubeBlockDefinition;
            if (cubeDef == null || !disabledBlocks_.Contains(cubeDef))
                return;

            var grid = block.CubeGrid;
            if (grid == null || grid.Closed || grid.MarkedForClose || grid.Physics == null)
                return;

            MyAPIGateway.Utilities.InvokeOnGameThread(() =>
            {
                if (block == null || block.IsDestroyed || grid == null || grid.Closed || grid.MarkedForClose || grid.Physics == null)
                    return;

                try
                {
                    grid.RemoveBlock(block, true);
                    MyLog.Default.WriteLineAndConsole($"BlockDisabler: Removed newly added disabled block {cubeDef.Id} from grid {grid.DisplayName}");
                }
                catch (Exception ex)
                {
                    MyLog.Default.WriteLineAndConsole($"BlockDisabler: Error removing newly added block {cubeDef.Id} from grid {grid.DisplayName}: {ex}");
                }
            });
        }

        private void RemoveDisabledBlocksFromWorld()
        {
            if (!IsServer() || disabledBlocks_.Count == 0 || MyAPIGateway.Entities == null)
                return;

            var entities = new HashSet<IMyEntity>();
            MyAPIGateway.Entities.GetEntities(entities);

            foreach (var entity in entities)
            {
                var grid = entity as IMyCubeGrid;
                if (grid != null && !grid.Closed && !grid.MarkedForClose && grid.Physics != null)
                {
                    TrackGrid(grid);
                    ProcessGrid(grid);
                }
            }
        }

        private void ProcessGrid(IMyCubeGrid grid)
        {
            if (grid == null || grid.Closed || grid.MarkedForClose || grid.Physics == null)
                return;

            try
            {
                var blocksToRemove = new List<IMySlimBlock>();
                var allBlocks = new List<IMySlimBlock>();
                grid.GetBlocks(allBlocks);

                foreach (var block in allBlocks)
                {
                    if (block == null || block.IsDestroyed)
                        continue;

                    var cubeDef = block.BlockDefinition as MyCubeBlockDefinition;
                    if (cubeDef != null && disabledBlocks_.Contains(cubeDef))
                    {
                        blocksToRemove.Add(block);
                    }
                }

                if (blocksToRemove.Count > 0)
                {
                    int removedCount = 0;
                    foreach (var block in blocksToRemove)
                    {
                        if (grid.Closed || grid.MarkedForClose)
                            break;

                        if (block.IsDestroyed)
                            continue;

                        grid.RemoveBlock(block, true);
                        removedCount++;
                    }

                    if (removedCount > 0)
                    {
                        MyLog.Default.WriteLineAndConsole($"BlockDisabler: Removed {removedCount} disabled blocks from grid {grid.DisplayName}");
                    }
                }
            }
            catch (Exception ex)
            {
                MyLog.Default.WriteLineAndConsole($"BlockDisabler: Error processing grid {grid.DisplayName}: {ex}");
            }
        }

        private void ProcessVariantGroups()
        {
            try
            {
                var groups = MyDefinitionManager.Static.GetBlockVariantGroupDefinitions();
                var modContext = ModContext as MyModContext;

                foreach (var group in groups.Values)
                {
                    if (group == null || group.Blocks == null || group.Blocks.Length == 0)
                        continue;

                    int originalCount = group.Blocks.Length;
                    bool changed = false;
                    List<MyCubeBlockDefinition> newBlocks = new List<MyCubeBlockDefinition>();

                    foreach (var block in group.Blocks)
                    {
                        if (block == null)
                        {
                            MyLog.Default.WriteLineAndConsole($"BlockDisabler: WARNING - Null block found in variant group {group.Id}");
                            continue;
                        }

                        if (disabledBlocks_.Contains(block))
                        {
                            changed = true;

                            // Detach disabled block from group while keeping it hidden
                            block.BlockStages = null;
                            block.BlockVariantsGroup = null;
                            block.GuiVisible = false;

                            MyLog.Default.WriteLineAndConsole($"BlockDisabler: Removing {block.Id} from variant group {group.Id}");
                        }
                        else
                        {
                            newBlocks.Add(block);
                        }
                    }

                    if (changed)
                    {
                        if (newBlocks.Count == 0)
                        {
                            // All blocks removed: empty and disable the variant group
                            group.Blocks = new MyCubeBlockDefinition[0];
                            group.Enabled = false;
                            group.Public = false;
                            if (modContext != null)
                            {
                                group.Context = modContext;
                            }

                            MyLog.Default.WriteLineAndConsole($"BlockDisabler: Variant group {group.Id} is now empty and disabled (was {originalCount} blocks)");
                        }
                        else
                        {
                            // Reset remaining blocks before Postprocess so a new primary block is cleanly promoted
                            foreach (var remaining in newBlocks)
                            {
                                remaining.BlockStages = null;
                                remaining.BlockVariantsGroup = null;
                                remaining.GuiVisible = true;
                            }

                            group.Blocks = newBlocks.ToArray();

                            if (modContext != null)
                            {
                                group.Context = modContext;
                            }

                            group.DisplayNameEnum = null;
                            group.Icons = null;
                            group.Postprocess();

                            // Re-link all remaining blocks to the group
                            foreach (var block in group.Blocks)
                            {
                                block.BlockVariantsGroup = group;
                            }

                            MyLog.Default.WriteLineAndConsole($"BlockDisabler: Variant group {group.Id} reduced from {originalCount} to {newBlocks.Count} blocks");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyLog.Default.WriteLineAndConsole($"BlockDisabler: Error processing variant groups: {ex}");
            }
        }

        private void ReadLinesFromMod(MyObjectBuilder_Checkpoint.ModItem mod, HashSet<string> lines)
        {
            try
            {
                using (var reader = MyAPIGateway.Utilities.ReadFileInModLocation(FILE_NAME, mod))
                {
                    ReadLinesFromReader(reader, lines);
                }
            }
            catch (Exception ex)
            {
                MyLog.Default.WriteLineAndConsole($"BlockDisabler: Error reading {FILE_NAME} from mod {mod.FriendlyName}: {ex}");
            }
        }

        private void ReadLinesFromWorldStorage(HashSet<string> lines)
        {
            try
            {
                using (var reader = MyAPIGateway.Utilities.ReadFileInWorldStorage(STORAGE_FILE_NAME, typeof(JSBlockDisabler)))
                {
                    ReadLinesFromReader(reader, lines);
                }
            }
            catch (Exception ex)
            {
                MyLog.Default.WriteLineAndConsole($"BlockDisabler: Error reading {STORAGE_FILE_NAME} from world storage: {ex}");
            }
        }

        private void ReadLinesFromReader(TextReader reader, HashSet<string> lines)
        {
            if (reader == null)
                return;

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                AddCleanLine(line, lines);
            }
        }

        /// <summary>
        /// Strips any // comment and surrounding whitespace (including a trailing '\r'), then adds the line if non-empty.
        /// </summary>
        private void AddCleanLine(string line, HashSet<string> lines)
        {
            if (line == null)
                return;

            int commentIndex = line.IndexOf("//", StringComparison.Ordinal);
            if (commentIndex >= 0)
            {
                line = line.Substring(0, commentIndex);
            }

            line = line.Trim();
            if (string.IsNullOrWhiteSpace(line))
                return;

            lines.Add(line);
        }

        private void CreateWorldStorageFile()
        {
            try
            {
                using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage(STORAGE_FILE_NAME, typeof(JSBlockDisabler)))
                {
                    writer.WriteLine("// Add blocks to disable here separated by new lines. Format: TypeId/SubtypeId");
                    writer.WriteLine("// Example: SmallMissileLauncherReload/LargeRailgun");
                }
            }
            catch (Exception ex)
            {
                MyLog.Default.WriteLineAndConsole($"BlockDisabler: Error writing {STORAGE_FILE_NAME} to world storage: {ex}");
            }
        }

        private bool DisableBlock(string idString)
        {
            MyDefinitionId id;
            if (!ParseDefinitionId(idString, out id))
            {
                MyLog.Default.WriteLineAndConsole($"BlockDisabler: Could not parse '{idString}' as a block ID. Expected format TypeId/SubtypeId.");
                return false;
            }

            MyCubeBlockDefinition blockDef = MyDefinitionManager.Static.GetCubeBlockDefinition(id);
            if (blockDef != null)
            {
                blockDef.Enabled = false;
                blockDef.Public = false;
                blockDef.GuiVisible = false;
                blockDef.AvailableInSurvival = false;

                bool added = disabledBlocks_.Add(blockDef);
                if (added)
                {
                    MyLog.Default.WriteLineAndConsole($"BlockDisabler: Disabled block {id}");
                }
                return added;
            }
            else
            {
                MyLog.Default.WriteLineAndConsole($"BlockDisabler: Block definition not found for {id}");
                return false;
            }
        }

        private bool ParseDefinitionId(string input, out MyDefinitionId id)
        {
            id = default(MyDefinitionId);
            if (string.IsNullOrWhiteSpace(input))
                return false;

            string trimmed = input.Trim();

            // Try direct parse first
            if (MyDefinitionId.TryParse(trimmed, out id))
                return true;

            // Normalize whitespace around '/' and try adding MyObjectBuilder_ prefix
            var parts = trimmed.Split('/');
            if (parts.Length == 2)
            {
                string type = parts[0].Trim();
                string subtype = parts[1].Trim();

                if (string.IsNullOrEmpty(type))
                    return false;

                if (MyDefinitionId.TryParse($"{type}/{subtype}", out id))
                    return true;

                if (!type.StartsWith("MyObjectBuilder_", StringComparison.OrdinalIgnoreCase))
                {
                    string newType = "MyObjectBuilder_" + type;
                    if (MyDefinitionId.TryParse($"{newType}/{subtype}", out id))
                        return true;
                }
            }

            id = default(MyDefinitionId);
            return false;
        }
    }
}
