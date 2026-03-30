using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using Sandbox.Game.EntityComponents;
using Sandbox.Common.ObjectBuilders;
using VRage.ObjectBuilders;
using VanillaPlusFramework.TemplateClasses;
using static VanillaPlusFramework.TemplateClasses.TargetFlags;
using static VanillaPlusFramework.TemplateClasses.FuelType;

namespace JSWeaponOverhaul /// Set namespace name to something else, preferably something no other mod uses. Can be the same as other definitions
{
    /// <summary>
    /// Recommend renaming the file.
    /// Recommend Visual Studio for editing this, all fields have descriptions that visual studio will display on hover over. Make a project solution to put all files in as well so the descriptions show up when hovered over.
    /// Contains all Vanilla+ Stats for all subtypes. Note: Vanilla Ammo.sbc is still used
    /// Note: All implented stats are here. There are some unimplimented ones not shown, do not use them or errors will be thrown.
    /// You can have multiple files, just change the class name ("VPFAmmoDefinitions" in front of public class) or the namespace
    /// </summary>
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class JSVPFTurretDefinitions : MySessionComponentBase
    {
        List<VPFTurretDefinition> TurretDefinitions = new List<VPFTurretDefinition>() {
            /*
             * DON'T MODIFY ANYTHING ABOVE THIS LINE EXCEPT THE NAMESPACE & CLASS NAME
             */
            new VPFTurretDefinition
            {
                subtypeName = "CoilgunTurret",

                FixTurretsHavingNoRecoil = true, // When set to true, turrets will generate proper recoil using the ammo type's <c>BackkickForce</c> tag. This only works for missile turrets.

                // Mutually exclusive with the keen EntityCapacitor component
                AG_Stats = new AmmoGeneration_Logic
                {
                    // name of ammo magazine to generate
                    // Set to "" to instead have the weapon constantly draw AG_AmmoCost over AG_GenerationTime seconds.
                    // Set to "null" in order to have the weapon switch to a "capacitor mode" - it will take AG_AmmoCost over AG_GenerationTime seconds once and then stop until the weapon fires, where it will reset.
                    AG_AmmoDefinitionName = "null",
                    
                    // cost of each batch of ammo, in megawatt hours for power or liters for the gases
                    AG_AmmoCost = (16f * 6f) / 3600f, // 1 / 3600 for 1 MW input power given the 1 second of generation time. 0.0002777777778f is the decimal way of writing this and is valid as well.
                    
                    // what resource is required. Valid types: POWER, HYDROGEN, OXYGEN
                    AG_FuelType = POWER,
                    
                    // how long it takes to generate each batch of ammo in seconds. Required input will be AG_AmmoCost / AG_GenerationTime
                    AG_GenerationTime = 6,

                    // how many ammo magazines is generated each batch
                    AG_NumberGenerated = 1,
                },
            },

            new VPFTurretDefinition
            {
                subtypeName = "RailgunTurret",

                FixTurretsHavingNoRecoil = true, // When set to true, turrets will generate proper recoil using the ammo type's <c>BackkickForce</c> tag. This only works for missile turrets.

                // Mutually exclusive with the keen EntityCapacitor component
                AG_Stats = new AmmoGeneration_Logic
                {
                    // name of ammo magazine to generate
                    // Set to "" to instead have the weapon constantly draw AG_AmmoCost over AG_GenerationTime seconds.
                    // Set to "null" in order to have the weapon switch to a "capacitor mode" - it will take AG_AmmoCost over AG_GenerationTime seconds once and then stop until the weapon fires, where it will reset.
                    AG_AmmoDefinitionName = "null",
                    
                    // cost of each batch of ammo, in megawatt hours for power or liters for the gases
                    AG_AmmoCost = (48f * 12f) / 3600f, // 1 / 3600 for 1 MW input power given the 1 second of generation time. 0.0002777777778f is the decimal way of writing this and is valid as well.
                    
                    // what resource is required. Valid types: POWER, HYDROGEN, OXYGEN
                    AG_FuelType = POWER,
                    
                    // how long it takes to generate each batch of ammo in seconds. Required input will be AG_AmmoCost / AG_GenerationTime
                    AG_GenerationTime = 12,

                    // how many ammo magazines is generated each batch
                    AG_NumberGenerated = 1,
                },
            },
            /*
             * DON'T MODIFY ANYTHING BELOW THIS LINE
             */
        };

        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {
            foreach (VPFTurretDefinition def in TurretDefinitions)
            {
                MyAPIUtilities.Static.SendModMessage(DefinitionTools.ModMessageID, DefinitionTools.DefinitionToMessage(def));
            }
        }
    }
}
