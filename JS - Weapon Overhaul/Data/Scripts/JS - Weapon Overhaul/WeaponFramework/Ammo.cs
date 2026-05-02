using System;
using System.Collections.Generic;
using Sandbox.ModAPI;
using VRage.Game.Components;
using VRageMath;
using VanillaPlusFramework.TemplateClasses;
using static VanillaPlusFramework.TemplateClasses.GuidanceType;
using static VanillaPlusFramework.TemplateClasses.IdType;
using static VanillaPlusFramework.TemplateClasses.DamageType;

namespace JSWeaponOverhaul /// Set namespace name to something else, preferably something no other mod uses. Can be the same as Turret Definition's
{
    /// <summary>
    /// Recommend Visual Studio for editing this, all fields have descriptions that visual studio will display on hover over.
    /// Contains all Vanilla+ Stats for all subtypes. Note: Vanilla Ammo.sbc is still used
    /// Note: All implented stats are here. There are some unimplimented ones not shown, do not use them or errors will be thrown.
    /// You can have multiple files, just change the class name ("VPFAmmoDefinitions" in front of public class) or the namespace
    /// </summary>
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class JSVPFAmmoDefinitions : MySessionComponentBase
    {
        public static List<VPFAmmoDefinition> AmmoDefinitions = new List<VPFAmmoDefinition>() {
            /*
             * DON'T MODIFY ANYTHING ABOVE THIS LINE EXCEPT THE NAMESPACE & CLASS NAME
             */

            // 200mm Guided Missile
            new VPFAmmoDefinition
            { // if you are NOT using a logic type, SET IT TO NULL OR THE SCRIPT MAY NOT FUNCTION AS INTENDED
            subtypeName = "Missile", //Ammo.sbc subtype of the missile (OR projectile if its just beam weapon type) you want logic for
            VPF_MissileHitpoints = 2, // missile health, used by a few logics like prox det for anti projectile; larger = harder to shoot down
            NeedsAPHEFix = false, // determines if the projectile will use an APHE damage fix.
            EMP_Stats = null,
            GL_Stats = new GuidanceLock_Logic {
                GL_HomingPiecewisePolynomialFunction = DefinitionTools.ConvertToDoubleArrayList(new double[,] { // replace this with null if you do not want the missile to have guidance (everything from 'new' until the '},') before the next object
                    /* Incompatible with beam weapons */
                    
                    /* GL_HomingPiecewisePolynomialFunction
                  * Piecewise polynomial function for homing, input is time in seconds, output is degrees per second the missile will home
                  * For each row:[0] = Start time, [1] = const, [2] = coefficient for x, [3] = coefficient for x^2, [N+1] coefficient for x^N
                  * ex. {0, 1, 0.1, 0.2} yields a function of 0.2x^2+0.1x+1 starting at time = 0
                  * each function will be used until the next function starts
                  * ex. Say {1, 0, 12, 5} is the next row after the previous example
                  * this one will start at time = 1, and yield a function of 5x^2+12x, and when it starts the function will completely replace the old one */
                    {0, 0, 0, 0},
                    {0.2, 160, 0, 0},
                    {1.2, 20, 0, 0},

                }),
                GL_DecoyPercentChanceToCauseRetarget = 0, // chance in percent to cause a retarget if within the decoy's retarget radius of a missile's target. 100+ will have it always retarget.
                                                          // Once a missile is targeted onto a decoy it will not attempt to retarget any other entities
                                                          // note: the missile this is set to IS the decoy 
                GL_DecoyRetargetRadius = 0,
                NoGuidance = false,
                UseLockOn = true,
                UseOneTimeRaycast = true,
                UseTurretTarget = true,
            },
            PD_Stats = null,
            JDI_Stats = null,
            SCI_Stats = null,
            BWT_Stats = null
                // to add more specific blocks copy everything in the 'new SpecialComponentryInteraction_Logic { [stats] },', if you only want one interaction delete one of them currently in the template
            },

            // Torpedos
            new VPFAmmoDefinition
            { // if you are NOT using a logic type, SET IT TO NULL OR THE SCRIPT MAY NOT FUNCTION AS INTENDED
            subtypeName = "Torpedo", //Ammo.sbc subtype of the missile (OR projectile if its just beam weapon type) you want logic for
            VPF_MissileHitpoints = 2, // missile health, used by a few logics like prox det for anti projectile; larger = harder to shoot down
            NeedsAPHEFix = false, // determines if the projectile will use an APHE damage fix.
            EMP_Stats = null,
            GL_Stats = new GuidanceLock_Logic {
                GL_HomingPiecewisePolynomialFunction = DefinitionTools.ConvertToDoubleArrayList(new double[,] { // replace this with null if you do not want the missile to have guidance (everything from 'new' until the '},') before the next object
                    /* Incompatible with beam weapons */
                    
                    /* GL_HomingPiecewisePolynomialFunction
                  * Piecewise polynomial function for homing, input is time in seconds, output is degrees per second the missile will home
                  * For each row:[0] = Start time, [1] = const, [2] = coefficient for x, [3] = coefficient for x^2, [N+1] coefficient for x^N
                  * ex. {0, 1, 0.1, 0.2} yields a function of 0.2x^2+0.1x+1 starting at time = 0
                  * each function will be used until the next function starts
                  * ex. Say {1, 0, 12, 5} is the next row after the previous example
                  * this one will start at time = 1, and yield a function of 5x^2+12x, and when it starts the function will completely replace the old one */
                    {0, 0, 0, 0},
                    {0.2, 80, 0, 0},
                    {1.2, 20, 0, 0},

                }),
                GL_DecoyPercentChanceToCauseRetarget = 0, // chance in percent to cause a retarget if within the decoy's retarget radius of a missile's target. 100+ will have it always retarget.
                                                          // Once a missile is targeted onto a decoy it will not attempt to retarget any other entities
                                                          // note: the missile this is set to IS the decoy 
                GL_DecoyRetargetRadius = 0,
                NoGuidance = false,
                UseLockOn = true,
                UseOneTimeRaycast = true,
                UseTurretTarget = true,
            },
            PD_Stats = null,
            JDI_Stats = null,
            SCI_Stats = null,
            BWT_Stats = null
                // to add more specific blocks copy everything in the 'new SpecialComponentryInteraction_Logic { [stats] },', if you only want one interaction delete one of them currently in the template
            },
            // copy everything from 'new VPFAmmoDefinition' to '} },' and paste it after the '} },' to add another ammo definition to this list


            /*
             * DON'T MODIFY ANYTHING BELOW THIS LINE
             */
        };



        public override void BeforeStart()
        {
            foreach (VPFAmmoDefinition def in AmmoDefinitions)
            {
                MyAPIUtilities.Static.SendModMessage(DefinitionTools.ModMessageID, DefinitionTools.DefinitionToMessage(def));
            }
        }
    }
}
