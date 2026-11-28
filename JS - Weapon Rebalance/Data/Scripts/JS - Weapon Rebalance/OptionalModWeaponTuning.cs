using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;

namespace JSWeaponRebalance
{
    /// <summary>
    /// Patches WeaponDefinitions that belong to optional companion mods (currently JS - Weapon Overhaul),
    /// restoring the original values on unload.
    ///
    /// WHY THIS IS A SCRIPT INSTEAD OF MODADJUSTER XML:
    /// ModAdjusterV2 handles missing &lt;CubeBlocks&gt; entries gracefully because Space Engineers'
    /// MyDefinitionManager.Static.GetCubeBlockDefinition returns null when a block is absent.
    /// However, MyDefinitionManager.Static.GetWeaponDefinition performs a direct dictionary lookup and
    /// throws KeyNotFoundException for unknown subtypes, and ModAdjusterV2 does not wrap that lookup in
    /// try/catch. Putting optional-mod &lt;Weapon&gt; entries in Accuracy.xml or DamageBalance.xml therefore
    /// crashes world load whenever JS - Weapon Overhaul is not loaded.
    ///
    /// Wrapping GetWeaponDefinition in try/catch here lets JS - Weapon Rebalance load cleanly on its own
    /// while still applying the matching accuracy and reload tuning whenever JS - Weapon Overhaul is present.
    /// </summary>
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class OptionalModWeaponTuning : MySessionComponentBase
    {
        private const string LOG_PREFIX = "[JS - Weapon Rebalance] ";
        private const string WEAPON_TYPE_PREFIX = "MyObjectBuilder_WeaponDefinition/";

        private sealed class WeaponTuning
        {
            public readonly float? deviateShotAngleDegrees_;
            public readonly float? deviateShotAngleAimingDegrees_;
            public readonly int? reloadTimeMs_;

            public WeaponTuning(float? deviateShotAngleDegrees, float? deviateShotAngleAimingDegrees, int? reloadTimeMs)
            {
                deviateShotAngleDegrees_ = deviateShotAngleDegrees;
                deviateShotAngleAimingDegrees_ = deviateShotAngleAimingDegrees;
                reloadTimeMs_ = reloadTimeMs;
            }
        }

        private sealed class OriginalValues
        {
            public float deviateShotAngle_;
            public float deviateShotAngleAiming_;
            public int reloadTime_;
        }

        // Optional-mod WeaponDefinition subtype -> new values. null = leave untouched.
        // Kept in sync with Accuracy.xml and DamageBalance.xml.
        private static readonly Dictionary<string, WeaponTuning> TUNINGS = new Dictionary<string, WeaponTuning>
        {
            // JS - Weapon Overhaul: large-grid autocannon turret (0.2 deg spread, 2 s reload = 1,000 DPS).
            // Also covers its Contact-styled Type II variant (LargeAutocannonTurretContact), which shares this weapon.
            { "LargeAutocannonTurret", new WeaponTuning(0.2f, 0.2f, 2000) },
        };

        // Original values, restored on unload so definitions are left as they were found.
        private readonly Dictionary<MyWeaponDefinition, OriginalValues> originals_ = new Dictionary<MyWeaponDefinition, OriginalValues>();

        public override void LoadData()
        {
            foreach (var entry in TUNINGS)
            {
                var def = GetWeaponDefinition(entry.Key);
                if (def == null)
                {
                    MyLog.Default.WriteLineAndConsole(LOG_PREFIX + "Optional weapon not loaded (skipping): " + entry.Key);
                    continue;
                }

                OriginalValues original;
                if (!originals_.TryGetValue(def, out original))
                {
                    original = new OriginalValues
                    {
                        deviateShotAngle_ = def.DeviateShotAngle,
                        deviateShotAngleAiming_ = def.DeviateShotAngleAiming,
                        reloadTime_ = def.ReloadTime,
                    };
                    originals_[def] = original;
                }

                var tuning = entry.Value;
                if (tuning.deviateShotAngleDegrees_.HasValue)
                {
                    def.DeviateShotAngle = MathHelper.ToRadians(tuning.deviateShotAngleDegrees_.Value);
                    MyLog.Default.WriteLineAndConsole(
                        LOG_PREFIX + entry.Key + " DeviateShotAngle " +
                        MathHelper.ToDegrees(original.deviateShotAngle_) + " -> " + tuning.deviateShotAngleDegrees_.Value + " deg");
                }

                if (tuning.deviateShotAngleAimingDegrees_.HasValue)
                {
                    def.DeviateShotAngleAiming = MathHelper.ToRadians(tuning.deviateShotAngleAimingDegrees_.Value);
                    MyLog.Default.WriteLineAndConsole(
                        LOG_PREFIX + entry.Key + " DeviateShotAngleAiming " +
                        MathHelper.ToDegrees(original.deviateShotAngleAiming_) + " -> " + tuning.deviateShotAngleAimingDegrees_.Value + " deg");
                }

                if (tuning.reloadTimeMs_.HasValue)
                {
                    def.ReloadTime = tuning.reloadTimeMs_.Value;
                    MyLog.Default.WriteLineAndConsole(
                        LOG_PREFIX + entry.Key + " ReloadTime " + original.reloadTime_ + " -> " + def.ReloadTime + " ms");
                }
            }
        }

        protected override void UnloadData()
        {
            foreach (var entry in originals_)
            {
                entry.Key.DeviateShotAngle = entry.Value.deviateShotAngle_;
                entry.Key.DeviateShotAngleAiming = entry.Value.deviateShotAngleAiming_;
                entry.Key.ReloadTime = entry.Value.reloadTime_;
            }

            originals_.Clear();
        }

        private static MyWeaponDefinition GetWeaponDefinition(string subtype)
        {
            MyDefinitionId id;
            if (!MyDefinitionId.TryParse(WEAPON_TYPE_PREFIX + subtype, out id))
                return null;

            try
            {
                return MyDefinitionManager.Static.GetWeaponDefinition(id);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
