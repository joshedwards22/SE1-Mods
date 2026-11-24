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
    /// Flight distance = acceleration phase + (Lifetime - acceleration time) * top speed (vanilla top speeds):
    ///   Missile:        100 -> 200 m/s at 100 m/s^2 (1 s, 150 m).      7.5 s -> ~1,450 m (range 1,300)
    ///   MissileHoming:   30 -> 150 m/s at 50 m/s^2  (2.4 s, 216 m).   13.5 s -> ~1,880 m (range 1,500, margin for curving)
    ///   Torpedo:         10 -> 150 m/s at 35 m/s^2  (4 s, 320 m).       23 s -> ~3,170 m (range 2,700)
    ///   Torpedo_Heavy:   10 -> 120 m/s at 30 m/s^2  (3.7 s, 238 m).     28 s -> ~3,160 m (range 2,700)
    ///   Torpedo_Seeking: 10 -> 220 m/s at 55 m/s^2  (3.8 s, 439 m).     20 s (vanilla) -> ~4,000 m
    ///   Torpedo_Piercing:10 -> 180 m/s at 45 m/s^2  (3.8 s, 359 m).     20 s (vanilla) -> ~3,280 m
    ///
    /// GuidanceRange (seeker range): vanilla is 800 m for homing missiles and 2,000 m for torpedoes, shorter
    /// than their new ranges, so it is raised to match MaxTrajectory (1,500 / 2,700).
    ///
    /// Launch turn: vanilla seekers turn at a fixed rate while speeding up, so a missile launched off-axis
    /// (e.g. a vertical torpedo tube) swings out in a ~500-1,150 m arc. Turn radius = speed / turn rate, so a slow
    /// start plus a faster turn rate gives a tight launch turn that widens on its own as the missile speeds up.
    /// Top speed and acceleration are unchanged, so point defense still faces vanilla speeds.
    ///   MissileInitialSpeed: homing 100 -> 30, torpedoes -> 10.
    ///   MaxTurnRateDegreesPerSecond (turn radius at top speed, vanilla in brackets):
    ///     MissileHoming    16 -> 100   ~86 m  (537 m)
    ///     Torpedo          14 -> 45   ~191 m  (614 m)
    ///     Torpedo_Heavy     6 -> 25   ~275 m  (1,146 m)
    ///     Torpedo_Seeking  25 -> 70   ~180 m  (504 m)
    ///     Torpedo_Piercing 12 -> 40   ~258 m  (859 m)
    ///   Tested: torpedoes hit a target at a right angle to the launcher.
    ///   MissilePreserveLaunchMomentum: kept on (vanilla) for torpedoes. Turning it off made no visible difference
    ///     to the launch turn in testing, and with it on the torpedo inherits the launching ship's velocity, so the
    ///     slow 10 m/s start cannot be overtaken by its own ship.
    /// </summary>
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class MissileTuning : MySessionComponentBase
    {
        private const string LOG_PREFIX = "[JS - Weapon Rebalance] ";
        private const string AMMO_TYPE_PREFIX = "MyObjectBuilder_AmmoDefinition/";

        private sealed class Tuning
        {
            public float? lifetime_;
            public float? guidanceRange_;
            public float? initialSpeed_;
            public float? turnRateDeg_;
            public bool? preserveMomentum_;
        }

        private sealed class OriginalValues
        {
            public float lifetime_;
            public float guidanceRange_;
            public float initialSpeed_;
            public float turnRateDeg_;
            public bool preserveMomentum_;
        }

        // Ammo subtype -> new values. null = leave vanilla.
        private static readonly Dictionary<string, Tuning> TUNINGS = new Dictionary<string, Tuning>
        {
            { "Missile",          new Tuning { lifetime_ = 7.5f } },
            { "MissileHoming",    new Tuning { lifetime_ = 13.5f, guidanceRange_ = 1500f, initialSpeed_ = 30f, turnRateDeg_ = 100f } },
            { "Torpedo",          new Tuning { lifetime_ = 23f,   guidanceRange_ = 2700f, initialSpeed_ = 10f, turnRateDeg_ = 45f, preserveMomentum_ = true } },
            { "Torpedo_Heavy",    new Tuning { lifetime_ = 28f,   guidanceRange_ = 2700f, initialSpeed_ = 10f, turnRateDeg_ = 25f, preserveMomentum_ = true } },
            { "Torpedo_Seeking",  new Tuning {                    guidanceRange_ = 2700f, initialSpeed_ = 10f, turnRateDeg_ = 70f, preserveMomentum_ = true } },
            { "Torpedo_Piercing", new Tuning {                    guidanceRange_ = 2700f, initialSpeed_ = 10f, turnRateDeg_ = 40f, preserveMomentum_ = true } },
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
                    original = new OriginalValues
                    {
                        lifetime_ = def.Lifetime,
                        guidanceRange_ = def.GuidanceRange,
                        initialSpeed_ = def.MissileInitialSpeed,
                        turnRateDeg_ = def.MaxTurnRateDegreesPerSecond,
                        preserveMomentum_ = def.MissilePreserveLaunchMomentum,
                    };
                    originals_[def] = original;
                }

                var tuning = entry.Value;
                if (tuning.lifetime_.HasValue)
                {
                    def.Lifetime = tuning.lifetime_.Value;
                    Log(entry.Key, "Lifetime", original.lifetime_, def.Lifetime);
                }

                if (tuning.guidanceRange_.HasValue)
                {
                    def.GuidanceRange = tuning.guidanceRange_.Value;
                    Log(entry.Key, "GuidanceRange", original.guidanceRange_, def.GuidanceRange);
                }

                if (tuning.initialSpeed_.HasValue)
                {
                    def.MissileInitialSpeed = tuning.initialSpeed_.Value;
                    Log(entry.Key, "MissileInitialSpeed", original.initialSpeed_, def.MissileInitialSpeed);
                }

                if (tuning.turnRateDeg_.HasValue)
                {
                    def.MaxTurnRateDegreesPerSecond = tuning.turnRateDeg_.Value;
                    Log(entry.Key, "MaxTurnRateDegreesPerSecond", original.turnRateDeg_, def.MaxTurnRateDegreesPerSecond);
                }

                if (tuning.preserveMomentum_.HasValue)
                {
                    def.MissilePreserveLaunchMomentum = tuning.preserveMomentum_.Value;
                    Log(entry.Key, "MissilePreserveLaunchMomentum", original.preserveMomentum_, def.MissilePreserveLaunchMomentum);
                }
            }
        }

        protected override void UnloadData()
        {
            foreach (var entry in originals_)
            {
                entry.Key.Lifetime = entry.Value.lifetime_;
                entry.Key.GuidanceRange = entry.Value.guidanceRange_;
                entry.Key.MissileInitialSpeed = entry.Value.initialSpeed_;
                entry.Key.MaxTurnRateDegreesPerSecond = entry.Value.turnRateDeg_;
                entry.Key.MissilePreserveLaunchMomentum = entry.Value.preserveMomentum_;
            }

            originals_.Clear();
        }

        private static void Log(string subtype, string field, object from, object to)
        {
            MyLog.Default.WriteLineAndConsole(LOG_PREFIX + subtype + " " + field + " " + from + " -> " + to);
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
