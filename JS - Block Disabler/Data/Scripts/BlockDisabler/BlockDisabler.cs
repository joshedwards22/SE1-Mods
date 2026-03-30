using Sandbox.Definitions;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;
using VRage.ObjectBuilders;
using VRage.Game.ModAPI;

namespace JSBlockDisabler
{
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class JSBlockDisabler : MySessionComponentBase
    {
        private const string FILE_NAME = "Data\\DisabledBlocks.txt";
        private HashSet<MyCubeBlockDefinition> _disabledBlocks = new HashSet<MyCubeBlockDefinition>();

        public override void LoadData()
		{
			base.LoadData();
            
            MyLog.Default.WriteLineAndConsole("BlockDisabler: Session Loading...");

            try
            {
                foreach (var mod in MyAPIGateway.Session.Mods)
                {
                    if (MyAPIGateway.Utilities.FileExistsInModLocation(FILE_NAME, mod))
                    {
                        MyLog.Default.WriteLineAndConsole($"BlockDisabler: Found {FILE_NAME} in mod {mod.FriendlyName} ({mod.PublishedFileId})");
                        ProcessDisableFile(mod);
                    }
                }
            }
            catch (Exception ex)
            {
                MyLog.Default.WriteLineAndConsole($"BlockDisabler: Critical error in LoadData: {ex}");
            }
            
            if (_disabledBlocks.Count > 0)
            {
                ProcessVariantGroups();
            }
                        
            MyLog.Default.WriteLineAndConsole("BlockDisabler: Finished processing.");
        }

        public override void BeforeStart()
        {
            base.BeforeStart();
            
            if (_disabledBlocks.Count > 0)
            {
                RemoveDisabledBlocksFromWorld();
                MyAPIGateway.Entities.OnEntityAdd += OnEntityAdd;
            }
        }

        protected override void UnloadData()
        {
            MyAPIGateway.Entities.OnEntityAdd -= OnEntityAdd;
			base.UnloadData();
        }

        private void OnEntityAdd(VRage.ModAPI.IMyEntity entity)
        {
            if (_disabledBlocks.Count == 0 || entity == null) 
                return;
            
            var grid = entity as IMyCubeGrid;
            if (grid != null)
            {
                ProcessGrid(grid);
            }
        }

        private void RemoveDisabledBlocksFromWorld()
        {
            if (_disabledBlocks.Count == 0) 
                return;

            var entities = new HashSet<VRage.ModAPI.IMyEntity>();
            MyAPIGateway.Entities.GetEntities(entities);

            foreach (var entity in entities)
            {
                var grid = entity as IMyCubeGrid;
                if (grid != null)
                {
                    ProcessGrid(grid);
                }
            }
        }

        private void ProcessGrid(IMyCubeGrid grid)
        {
            try 
            {
                var blocksToRemove = new List<IMySlimBlock>();
                var allBlocks = new List<IMySlimBlock>();
                grid.GetBlocks(allBlocks);
                
                foreach (var block in allBlocks)
                {
                    var cubeDef = block.BlockDefinition as MyCubeBlockDefinition;
                    if (cubeDef != null && _disabledBlocks.Contains(cubeDef))
                    {
                        blocksToRemove.Add(block);
                    }
                }

                if (blocksToRemove.Count > 0)
                {
                    foreach (var block in blocksToRemove)
                    {
                        grid.RemoveBlock(block);
                    }
                    MyLog.Default.WriteLineAndConsole($"BlockDisabler: Removed {blocksToRemove.Count} disabled blocks from grid {grid.DisplayName}");
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
                MyLog.Default.WriteLineAndConsole($"BlockDisabler: Processing {groups.Count} variant groups");
                
                foreach (var group in groups.Values)
                {
                    if (group.Blocks == null || group.Blocks.Length == 0)
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
                        
                        if (_disabledBlocks.Contains(block))
                        {
                            changed = true;
                            
                            // Properly detach block from group (THDigi's approach)
                            block.BlockStages = null;
                            block.BlockVariantsGroup = null;
                            block.GuiVisible = true; // Reset since it was assigned by the group
                            
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
                            // If all blocks removed, set to empty array
                            group.Blocks = new MyCubeBlockDefinition[0];
                            MyLog.Default.WriteLineAndConsole($"BlockDisabler: Variant group {group.Id} is now empty (was {originalCount} blocks)");
                        }
                        else
                        {
                            // Update group with new block list
                            group.Blocks = newBlocks.ToArray();
                            
                            // Mark it as modified by this mod
                            group.Context = (VRage.Game.MyModContext)ModContext;
                            
                            // Re-process the group (THDigi's approach)
                            group.DisplayNameEnum = null;
                            group.Icons = null;
                            group.Postprocess();
                            
                            // Re-link all remaining blocks to the group (THDigi's approach)
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

        private void ProcessDisableFile(VRage.Game.MyObjectBuilder_Checkpoint.ModItem mod)
        {
            try
            {
                using (var reader = MyAPIGateway.Utilities.ReadFileInModLocation(FILE_NAME, mod))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//"))
                            continue;

                        DisableBlock(line);
                    }
                }
            }
            catch (Exception ex)
            {
                MyLog.Default.WriteLineAndConsole($"BlockDisabler: Error reading {FILE_NAME} from mod {mod.FriendlyName}: {ex}");
            }
        }

        private void DisableBlock(string idString)
        {
            MyDefinitionId id;
            if (!ParseDefinitionId(idString, out id))
            {
                MyLog.Default.WriteLineAndConsole($"BlockDisabler: Could not parse '{idString}' as a block ID. Expected format TypeId/SubtypeId.");
                return;
            }

            MyCubeBlockDefinition blockDef = MyDefinitionManager.Static.GetCubeBlockDefinition(id);
            if (blockDef != null)
            {
                // Disable the block
                blockDef.Enabled = false;
                blockDef.Public = false;
                blockDef.GuiVisible = false;
                blockDef.AvailableInSurvival = false; 

                _disabledBlocks.Add(blockDef);
                
                // Try to trigger post-processing to refresh the definition
                try
                {
                    blockDef.Postprocess();
                }
                catch (Exception ex)
                {
                    MyLog.Default.WriteLineAndConsole($"BlockDisabler: Postprocess failed for {id}: {ex.Message}");
                }

                MyLog.Default.WriteLineAndConsole($"BlockDisabler: Disabled block {id}");
            }
            else
            {
                MyLog.Default.WriteLineAndConsole($"BlockDisabler: Block definition not found for {id}");
            }
        }

        private bool ParseDefinitionId(string input, out MyDefinitionId id)
        {
            // Try direct parse
            if (MyDefinitionId.TryParse(input, out id))
                return true;

            // Try adding MyObjectBuilder_ prefix to type
            var parts = input.Split('/');
            if (parts.Length == 2)
            {
                string type = parts[0];
                string subtype = parts[1];
                
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
