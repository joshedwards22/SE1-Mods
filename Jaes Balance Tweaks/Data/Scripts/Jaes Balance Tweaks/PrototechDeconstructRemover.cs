using System;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;

namespace JoshsBalanceTweaks
{
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class PrototechDeconstructRemover : MySessionComponentBase
    {
        public override void LoadData()
        {
			base.LoadData();
            // Iterate through all definitions to find CubeBlocks
            foreach (var definition in MyDefinitionManager.Static.GetAllDefinitions())
            {
                var blockDef = definition as MyCubeBlockDefinition;
                if (blockDef == null)
                    continue;

                // Check if the block is a "Prototech" block by its DisplayName
                string displayName = blockDef.DisplayNameText;
                if (!string.IsNullOrEmpty(displayName) && displayName.IndexOf("Prototech", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    if (blockDef.Components != null)
                    {
                        foreach (var component in blockDef.Components)
                        {
                            // Check if the DeconstructItem is PrototechScrap
                            if (component.DeconstructItem != null && component.DeconstructItem.Id.SubtypeName == "PrototechScrap")
                            {
                                // Set the DeconstructItem to the component definition itself so it grinds back to the component
                                component.DeconstructItem = component.Definition;
                            }
                        }
                    }
                }
            }
        }
    }
}
