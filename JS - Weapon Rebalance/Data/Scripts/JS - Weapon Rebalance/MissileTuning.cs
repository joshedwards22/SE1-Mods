using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;

namespace JSWeaponRebalance
{
    /// <summary>
    /// Missile/torpedo fields that ModAdjusterV2 cannot patch, set on the loaded definitions at world load
    /// and restored on unload. Works together with the MaxTrajectory values in Data\ModAdjuster\Ranges.xml.
    ///
    /// Lifetime (seconds before self-destruct): raised so missiles reach the longer MaxTrajectory WITHOUT
    /// making them faster (faster missiles would be harder for gatling point defense to shoot down).
    /// Flight distance = acceleration phase + (Lifetime - acceleration time) * top speed (vanilla speeds):
    ///   Missile:        100 -> 200 m/s at 100 m/s^2 (1 s, 150 m).    7.5 s -> ~1,450 m (range 1,300)
    ///   MissileHoming:  100 -> 150 m/s at 50 m/s^2  (1 s, 125 m).   12.5 s -> ~1,850 m (range 1,500, margin for curving)
    ///   Torpedo:         35 -> 150 m/s at 35 m/s^2  (3.3 s, 304 m).  22 s -> ~3,110 m (range 2,700)
    ///   Torpedo_Heavy:   30 -> 120 m/s at 30 m/s^2  (3 s, 225 m).    27 s -> ~3,105 m (range 2,700)
    ///   Torpedo_Seeking (~4,150 m) and Torpedo_Piercing (~3,400 m) already fly far enough at vanilla lifetime.
    ///
    /// GuidanceRange (seeker range): vanilla is 800 m for homing missiles and 2,000 m for torpedoes, shorter
    /// than their new ranges, so it is raised to match MaxTrajectory (1,500 / 2,700).
    /// </summary>
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class MissileTuning : MySessionComponentBase
    {
        private const string LOG_PREFIX = "[JS - Weapon Rebalance] ";
        private const string AMMO_TYPE_PREFIX = "MyObjectBuilder_AmmoDefinition/";

        private sealed class Tuning
        {
            public readonly float? lifetime_;
            public readonly float? guidanceRange_;

            public Tuning(float? lifetime, float? guidanceRange)
            {
                lifetime_ = lifetime;
                guidanceRange_ = guidanceRange;
            }
        }

        private sealed class OriginalValues
        {
            public float lifetime_;
            public float guidanceRange_;
        }

        // Ammo subtype -> new values. null = leave vanilla.
        private static readonly Dictionary<string, Tuning> TUNINGS = new Dictionary<string, Tuning>
        {
            { "Missile",          new Tuning(7.5f, null) },
            { "MissileHoming",    new Tuning(12.5f, 1500f) },
            { "Torpedo",          new Tuning(22f, 2700f) },
            { "Torpedo_Heavy",    new Tuning(27f, 2700f) },
            { "Torpedo_Seeking",  new Tuning(null, 2700f) },
            { "Torpedo_Piercing", new Tuning(null, 2700f) },
        };

        // Vanilla values, restored on unload so definitions are left as they were found.
        private readonly Dictionary<MyMissileAmmoDefinition, OriginalValues> originals_ = new Dictionary<MyMissileAmmoDefinition, OriginalValues>();

        public override void LoadData()
        {
            foreach (var entry in TUNINGS)
            {
                var def = GetMissileAmmo(entry.Key);
                if (def == null)
                {
                    MyLog.Default.WriteLineAndConsole(LOG_PREFIX + "Missile ammo not found: " + entry.Key);
                    continue;
                }

                OriginalValues original;
                if (!originals_.TryGetValue(def, out original))
                {
                    original = new OriginalValues { lifetime_ = def.Lifetime, guidanceRange_ = def.GuidanceRange };
                    originals_[def] = original;
                }

                var tuning = entry.Value;
                if (tuning.lifetime_.HasValue)
                {
                    def.Lifetime = tuning.lifetime_.Value;
                    MyLog.Default.WriteLineAndConsole(LOG_PREFIX + entry.Key + " Lifetime " + original.lifetime_ + " -> " + def.Lifetime);
                }

                if (tuning.guidanceRange_.HasValue)
                {
                    def.GuidanceRange = tuning.guidanceRange_.Value;
                    MyLog.Default.WriteLineAndConsole(LOG_PREFIX + entry.Key + " GuidanceRange " + original.guidanceRange_ + " -> " + def.GuidanceRange);
                }
            }
        }

        protected override void UnloadData()
        {
            foreach (var entry in originals_)
            {
                entry.Key.Lifetime = entry.Value.lifetime_;
                entry.Key.GuidanceRange = entry.Value.guidanceRange_;
            }

            originals_.Clear();
        }

        private static MyMissileAmmoDefinition GetMissileAmmo(string subtype)
        {
            MyDefinitionId id;
            if (!MyDefinitionId.TryParse(AMMO_TYPE_PREFIX + subtype, out id))
                return null;

            try
            {
                return MyDefinitionManager.Static.GetAmmoDefinition(id) as MyMissileAmmoDefinition;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
