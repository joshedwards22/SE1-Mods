using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using VRageMath;
using static VanillaPlusFramework.TemplateClasses.GuidanceType;


// all the data structures for the ammo definitions, you can ignore this file (or use it for documentation) it just ensures compilation successful.

/******************************************************************************************************************************************************
 *                                                                                                                                                    *
 *                                                            DO NOT MODIFY THIS FILE                                                                 *
 *                                                                                                                                                    *
 ******************************************************************************************************************************************************/


namespace VanillaPlusFramework.TemplateClasses
{
    /// <summary>
    /// Base class for all definitions so deserialization can occur without errors.
    /// </summary>
    [ProtoContract]
    [ProtoInclude(1000, typeof(VPFAmmoDefinition))]
    public partial class VPFDefinition
    {
    }
    /// <summary>
    /// Specifies what type the companion string points to.
    /// <para>
    /// Availible Types:
    /// <code>
    /// SubtypeId
    /// TypeId
    /// </code>
    /// </para>
    /// </summary>
    [Serializable]
    public enum IdType
    {
        SubtypeId,
        TypeId
    }

    /// <summary>
    /// Speficies what type of damage the companion float points to. Percent deals a percentage of total hp, damage deals that amount of damage.
    /// <para>
    /// Availible Types:
    /// <code>
    /// Percent
    /// Damage
    /// </code>
    /// </para>
    /// </summary>
    [Serializable]
    public enum DamageType
    {
        Percent,
        Damage
    }

    /// <summary>
    /// Speficies how a guided missile can gain a target
    /// <para>
    /// Availible Types:
    /// <code>
    /// None
    /// LockOn
    /// TurretTarget
    /// DesignatedPosition
    /// OneTimeRaycast
    /// </code>
    /// </para>
    /// </summary>
    [Serializable]
    [Flags]
    public enum GuidanceType
    {
        None = 0,
        LockOn = 1,
        TurretTarget = 2,
        DesignatedPosition = 4,
        OneTimeRaycast = 8,
    }

    /// <summary>
    /// Speficies which direction will forward be for the shrapnel spawning.
    /// Note: Defaults to ProjectileDirection if the vector used would be the zero vector (ie. no gravity, or failed to hit anything)
    /// <para>
    /// Availible Types:
    /// <code>
    /// ProjectileDirection - Projectile forward is forward
    /// NaturalGravity - Natural gravity vector is forward
    /// ArtificialGravity - Artifical gravity vector is forward
    /// TotalGravity - Overall gravity vector is forward
    /// CollsionNormal - The normal for the collision between missile and hit target is forward
    /// CollisionReflection - The reflected vector about the normal (as if it was a beam reflecting) of the collision between missile and hit target is forward
    /// </code>
    /// </para>
    /// </summary>
    [Serializable]
    public enum ShrapnelSpawnDirection
    {
        ProjectileDirection = 0,
        NaturalGravity = 1,
        ArtificialGravity = 2,
        TotalGravity = 3,
        CollsionNormal = 4,
        CollisionReflection = 5,
    }
    /// <summary>
    /// Speficies any requirements to spawn shrapnel. Multiple can be used at a time, using the separator character '|'. Defaults to an 'or' like state where if multiple are used, any one condition being true can cause shrapnel to spawn.
    /// Using 'RequiresAll' will change that to an 'and' state, where every condition listed must be met.
    /// Conditions use the missile/beam end point for these conditions.
    /// <para>
    /// ex.
    /// <code>
    /// RequiresAll | NaturalGravity | Collision
    /// </code>
    /// </para>
    /// Defaults to None
    /// <para>
    /// Availible Types:
    /// <code>
    /// NoRequirements - Shrapnel will always spawn. Mutually exclusive with everything else
    /// RequiresAll - Signals that every condition must be present for the shrapnel to spaw
    /// RequiresNaturalGravity - There must be natural gravity present
    /// RequiresArtificialGravity - There must be artificial gravity present
    /// RequiresAnyGravity - There must be some sort of gravity present
    /// RequiresNoNaturalGravity - There must be no natural gravity present
    /// RequiresNoArtificialGravity - There must be no artificial gravity present
    /// RequiresNoGravity - There must be no gravity present
    /// RequiresCollision - The missile/beam has hit something
    /// RequiresEndOfLife - The missile/beam has not hit anything
    /// RequiresMainSpawnDirection - The Main Spawn Direction listed in the definition was the direction used
    /// RequiresBackupSpawnDirection - The Backup Spawn Direction listed in the definition was the direction used
    /// RequiresShellArmed - (Missile only) requires the shell to be armed.
    /// RequiresShellNotArmed - (Missile only) requires the shell to be not armed.
    /// </code>
    /// </para>
    /// </summary>
    [Serializable]
    [Flags]
    public enum ShrapnelSpawnRequirements
    {
        NoRequirements = 0,
        RequiresAll = 1,
        RequiresNaturalGravity = 2,
        RequiresArtificialGravity = 4,
        RequiresAnyGravity = 8,
        RequiresNoNaturalGravity = 16,
        RequiresNoArtificialGravity = 32,
        RequiresNoGravity = 64,
        RequiresCollision = 128,
        RequiresEndOfLife = 256,
        RequiresMainSpawnDirection = 512,
        RequiresBackupSpawnDirection = 1024,
        RequiresShellArmed = 2048,
        RequiresShellNotArmed = 4096,
        RequiresShellHealthpoolAboveZero = 8192,
        RequiresShellHealthpoolAtZero = 16384,
    }

    /// <summary>
    /// Speficies which direction will forward be for the shrapnel spawning.
    /// Note: Defaults to TimeSinceSpawn.
    /// <para>
    /// Availible Types:
    /// <code>
    /// TimeSinceSpawn - Time since projectile spawn in seconds
    /// DistanceFromTarget - Distance from target location in meters
    /// DistanceFromOrigin - Distance from missile spawn location in meters
    /// AngleToTarget - Angle between missile forwards and target in degrees
    /// </code>
    /// </para>
    /// </summary>
    [Serializable]
    public enum GuidanceFunctionVariable
    {
        TimeSinceSpawn = 0,
        DistanceFromTarget = 1,
        DistanceFromOrigin = 2,
        AngleToTarget = 3,
    }


    ///<summary>
    /// Protobuf doesn't allow directly nested arrays or multidmensional ones, so this is necessary to serialize.
    ///</summary>
    [ProtoContract]
    public struct DoubleArray
    {
        [ProtoMember(1)]
        public double[] array;
        public DoubleArray(double[] array)
        {
            this.array = array;
        }

        public override string ToString()
        {
            string vals = "";
            foreach (double d in array)
            {
                vals += $"{d}, ";
            }
            return "{ " + vals + " },";
        }
    }

    /// <summary>
    /// Struct containing variables controlling shrapnel hitscans on missile death. Compatible with everything. Usable with projectile type weapons.
    /// Warning: The shrapnel can ONLY be beams and projectiles. Shrapnel can NOT be missile types.
    /// </summary>
    [ProtoContract]
    public struct Shrapnel_Logic
    {
        /// <summary>
        /// Name of the vanilla+ definition the shrapnel uses. Definition must point to a valid keen ammo definition.
        /// <para>
        /// String
        /// </para>
        /// <para>
        /// Should always be the name of another VPFAmmoDefinition. Should this be a missile type, the shrapnel must have <c>SPL_ShrapnelWeaponDefinition</c> be properly defined.
        /// </para>
        /// </summary>
        [ProtoMember(1)]
        public string SPL_ShrapnelDefinitionName;


        // I will find a fix for this eventually™, hopefully. But for now, commented out.
        // /// <summary>
        // /// Used for missile type shrapnel spawning only. The subtype ID of a keen weapon definition pointing to an ammo magazine definition pointing to the ammo definition listed in <c>SPL_ShrapnelDefinitionName</c>
        // /// <para>
        // /// String
        // /// </para>
        // /// <para>
        // /// Should be the subtype ID of a valid keen weapon definition. Additionally, the weapon definition must have the desired ammo magazine definition as the only ammo magazine present.
        // /// </para>
        // /// </summary>
        //[ProtoMember(11)]
        //public string SPL_ShrapnelWeaponDefinition;


        /// <summary>
        /// Angle of the cone to spawn shrapnel in. Negative values will have the cone originate backwards instead of forwards.
        /// <para>
        /// Unit: Degrees
        /// </para>
        /// <para>
        /// Should be within -180 to 180.
        /// </para>
        /// </summary>
        [ProtoMember(2)]
        public float SPL_OuterConeAngleInDegrees;

        /// <summary>
        /// Angle of the cone contained inside the outside cone where no shrapnel will spawn. Set to 0 to disable. Always positive, regardless of what SPL_OuterConeAngleInDegrees is set to.
        /// <para>
        /// Unit: Degrees
        /// </para>
        /// <para>
        /// Should be between 0 and the absolute value of what 'SPL_OuterConeAngleInDegrees' is set to.
        /// </para>
        /// </summary>
        [ProtoMember(3)]
        public float SPL_InnerConeAngleInDegrees;

        /// <summary>
        /// Minimum number of beams to spawn between the two cones. The amount chosen will be chosen randomly between Min and Max shrapnel counts.
        /// <para>
        /// Unit: Amount
        /// </para>
        /// <para>
        /// Should be greater than zero.
        /// </para>
        /// </summary>
        [ProtoMember(4)]
        public int SPL_MinShrapnelCount;

        /// <summary>
        /// Maximum number of beams to spawn between the two cones. The amount chosen will be chosen randomly between Min and Max shrapnel counts.
        /// <para>
        /// Unit: Amount
        /// </para>
        /// <para>
        /// Should be greater than zero.
        /// </para>
        /// </summary>
        [ProtoMember(5)]
        public int SPL_MaxShrapnelCount;

        /// <summary>
        /// Offset forwards (or backwards if you make it negative) to spawn the shrapnel from the projectile/beam endpoint. Note: This offsets the origin, not the beams themselves.
        /// <para>
        /// Unit: Meters
        /// </para>
        /// <para>
        /// Set to 0 to disable.
        /// </para>
        /// </summary>
        [ProtoMember(6)]
        public float SPL_ShrapnelOriginSpawnForwardOffset;

        /// <summary>
        /// Offset forwards (or backwards if you make it negative) to spawn the shrapnel from the projectile/beam endpoint. Note: This offsets the beams from the origin, not the origin.
        /// <para>
        /// Unit: Meters
        /// </para>
        /// <para>
        /// Set to 0 to disable.
        /// </para>
        /// </summary>
        [ProtoMember(7)]
        public float SPL_ShrapnelSpawnForwardOffset;

        /// <summary>
        /// Specifies which direction will forward be for the shrapnel spawning. Should this be the zero vector (ie. TotalGravity in a 0g enviroment) then this will default to the backup spawn direction.
        /// <para>
        /// Availible Types:
        /// <code>
        ///ProjectileDirection - Projectile forward is forward
        /// NaturalGravity - Natural gravity vector is forward
        /// ArtificialGravity - Artifical gravity vector is forward
        /// TotalGravity - Overall gravity vector is forward
        /// CollsionNormal - The normal for the collision between missile and hit target is forward
        /// CollisionReflection - The reflected vector about the normal (as if it was a beam reflecting) of the collision between missile and hit target is forward
        /// </code>
        /// </para>
        /// </summary>
        [ProtoMember(8)]
        public ShrapnelSpawnDirection SPL_MainShrapnelForwardsDirection;

        /// <summary>
        /// Specifies which direction will forward be for the shrapnel spawning, but is only used if the main direction is the zero vector. Should this also be the zero vector, it defaults to ProjectileDirection
        /// <para>
        /// Availible Types:
        /// <code>
        /// ProjectileDirection - Projectile forward is forward
        /// NaturalGravity - Natural gravity vector is forward
        /// ArtificialGravity - Artifical gravity vector is forward
        /// TotalGravity - Overall gravity vector is forward
        /// CollsionNormal - The normal for the collision between missile and hit target is forward
        /// CollisionReflection - The reflected vector about the normal (as if it was a beam reflecting) of the collision between missile and hit target is forward
        /// </code>
        /// </para>
        /// </summary>
        [ProtoMember(9)]
        public ShrapnelSpawnDirection SPL_BackupShrapnelForwardsDirection;

        /// <summary>
        /// Set to true to require all of the below requirements to spawn shrapnel instead of just one
        /// </summary>
        [ProtoIgnore]
        public bool RequiresAll
        {
            get
            {
                return SPL_ShrapnelSpawnRequirements.HasFlag(ShrapnelSpawnRequirements.RequiresAll);
            }
            set
            {
                if (value)
                    SPL_ShrapnelSpawnRequirements |= ShrapnelSpawnRequirements.RequiresAll;

                else
                    SPL_ShrapnelSpawnRequirements &= ~ShrapnelSpawnRequirements.RequiresAll;
            }
        }

        /// <summary>
        /// Set to true to require natural gravity be present to spawn shrapnel
        /// </summary>
        [ProtoIgnore]
        public bool RequiresNaturalGravity
        {
            get
            {
                return SPL_ShrapnelSpawnRequirements.HasFlag(ShrapnelSpawnRequirements.RequiresNaturalGravity);
            }
            set
            {
                if (value)
                    SPL_ShrapnelSpawnRequirements |= ShrapnelSpawnRequirements.RequiresNaturalGravity;

                else
                    SPL_ShrapnelSpawnRequirements &= ~ShrapnelSpawnRequirements.RequiresNaturalGravity;
            }
        }

        /// <summary>
        /// Set to true to require artificial gravity be present to spawn shrapnel
        /// </summary>
        [ProtoIgnore]
        public bool RequiresArtificialGravity
        {
            get
            {
                return SPL_ShrapnelSpawnRequirements.HasFlag(ShrapnelSpawnRequirements.RequiresArtificialGravity);
            }
            set
            {
                if (value)
                    SPL_ShrapnelSpawnRequirements |= ShrapnelSpawnRequirements.RequiresArtificialGravity;

                else
                    SPL_ShrapnelSpawnRequirements &= ~ShrapnelSpawnRequirements.RequiresArtificialGravity;
            }
        }

        /// <summary>
        /// Set to true to require any gravity be present to spawn shrapnel
        /// </summary>
        [ProtoIgnore]
        public bool RequiresAnyGravity
        {
            get
            {
                return SPL_ShrapnelSpawnRequirements.HasFlag(ShrapnelSpawnRequirements.RequiresAnyGravity);
            }
            set
            {
                if (value)
                    SPL_ShrapnelSpawnRequirements |= ShrapnelSpawnRequirements.RequiresAnyGravity;

                else
                    SPL_ShrapnelSpawnRequirements &= ~ShrapnelSpawnRequirements.RequiresAnyGravity;
            }
        }

        /// <summary>
        /// Set to true to require no natural gravity be present to spawn shrapnel
        /// </summary>
        [ProtoIgnore]
        public bool RequiresNoNaturalGravity
        {
            get
            {
                return SPL_ShrapnelSpawnRequirements.HasFlag(ShrapnelSpawnRequirements.RequiresNoNaturalGravity);
            }
            set
            {
                if (value)
                    SPL_ShrapnelSpawnRequirements |= ShrapnelSpawnRequirements.RequiresNoNaturalGravity;

                else
                    SPL_ShrapnelSpawnRequirements &= ~ShrapnelSpawnRequirements.RequiresNoNaturalGravity;
            }
        }

        /// <summary>
        /// Set to true to require no artificial gravity be present to spawn shrapnel
        /// </summary>
        [ProtoIgnore]
        public bool RequiresNoArtificialGravity
        {
            get
            {
                return SPL_ShrapnelSpawnRequirements.HasFlag(ShrapnelSpawnRequirements.RequiresNoArtificialGravity);
            }
            set
            {
                if (value)
                    SPL_ShrapnelSpawnRequirements |= ShrapnelSpawnRequirements.RequiresNoArtificialGravity;

                else
                    SPL_ShrapnelSpawnRequirements &= ~ShrapnelSpawnRequirements.RequiresNoArtificialGravity;
            }
        }

        /// <summary>
        /// Set to true to require no gravity be present to spawn shrapnel
        /// </summary>
        [ProtoIgnore]
        public bool RequiresNoGravity
        {
            get
            {
                return SPL_ShrapnelSpawnRequirements.HasFlag(ShrapnelSpawnRequirements.RequiresNoGravity);
            }
            set
            {
                if (value)
                    SPL_ShrapnelSpawnRequirements |= ShrapnelSpawnRequirements.RequiresNoGravity;

                else
                    SPL_ShrapnelSpawnRequirements &= ~ShrapnelSpawnRequirements.RequiresNoGravity;
            }
        }

        /// <summary>
        /// Set to true to require the missile/beam to have died hitting something to spawn shrapnel
        /// </summary>
        [ProtoIgnore]
        public bool RequiresCollision
        {
            get
            {
                return SPL_ShrapnelSpawnRequirements.HasFlag(ShrapnelSpawnRequirements.RequiresCollision);
            }
            set
            {
                if (value)
                    SPL_ShrapnelSpawnRequirements |= ShrapnelSpawnRequirements.RequiresCollision;

                else
                    SPL_ShrapnelSpawnRequirements &= ~ShrapnelSpawnRequirements.RequiresCollision;
            }
        }


        /// <summary>
        /// Set to true to require the missile/beam to not have died hitting something to spawn shrapnel
        /// </summary>
        [ProtoIgnore]
        public bool RequiresEndOfLife
        {
            get
            {
                return SPL_ShrapnelSpawnRequirements.HasFlag(ShrapnelSpawnRequirements.RequiresEndOfLife);
            }
            set
            {
                if (value)
                    SPL_ShrapnelSpawnRequirements |= ShrapnelSpawnRequirements.RequiresEndOfLife;

                else
                    SPL_ShrapnelSpawnRequirements &= ~ShrapnelSpawnRequirements.RequiresEndOfLife;
            }
        }


        /// <summary>
        /// Set to true to require the Main Spawn Direction listed in the definition was the direction used to spawn shrapnel
        /// </summary>
        [ProtoIgnore]
        public bool RequiresMainSpawnDirection
        {
            get
            {
                return SPL_ShrapnelSpawnRequirements.HasFlag(ShrapnelSpawnRequirements.RequiresMainSpawnDirection);
            }
            set
            {
                if (value)
                    SPL_ShrapnelSpawnRequirements |= ShrapnelSpawnRequirements.RequiresMainSpawnDirection;

                else
                    SPL_ShrapnelSpawnRequirements &= ~ShrapnelSpawnRequirements.RequiresMainSpawnDirection;
            }
        }


        /// <summary>
        /// Set to true to require the Backup Spawn Direction listed in the definition was the direction used to spawn shrapnel
        /// </summary>
        [ProtoIgnore]
        public bool RequiresBackupSpawnDirection
        {
            get
            {
                return SPL_ShrapnelSpawnRequirements.HasFlag(ShrapnelSpawnRequirements.RequiresBackupSpawnDirection);
            }
            set
            {
                if (value)
                    SPL_ShrapnelSpawnRequirements |= ShrapnelSpawnRequirements.RequiresBackupSpawnDirection;

                else
                    SPL_ShrapnelSpawnRequirements &= ~ShrapnelSpawnRequirements.RequiresBackupSpawnDirection;
            }
        }

        /// <summary>
        /// (Missile only) Set to true to require the missile to be armed.
        /// </summary>
        [ProtoIgnore]
        public bool RequiresShellArmed
        {
            get
            {
                return SPL_ShrapnelSpawnRequirements.HasFlag(ShrapnelSpawnRequirements.RequiresShellArmed);
            }
            set
            {
                if (value)
                    SPL_ShrapnelSpawnRequirements |= ShrapnelSpawnRequirements.RequiresShellArmed;

                else
                    SPL_ShrapnelSpawnRequirements &= ~ShrapnelSpawnRequirements.RequiresShellArmed;
            }
        }

        /// <summary>
        /// (Missile only) Set to true to require the missile to be not armed.
        /// </summary>
        [ProtoIgnore]
        public bool RequiresShellNotArmed
        {
            get
            {
                return SPL_ShrapnelSpawnRequirements.HasFlag(ShrapnelSpawnRequirements.RequiresShellNotArmed);
            }
            set
            {
                if (value)
                    SPL_ShrapnelSpawnRequirements |= ShrapnelSpawnRequirements.RequiresShellNotArmed;

                else
                    SPL_ShrapnelSpawnRequirements &= ~ShrapnelSpawnRequirements.RequiresShellNotArmed;
            }
        }

        /// <summary>
        /// (Missile only) Set to true to require the missile's HP value to be equal to zero (basically the missile did not get shot down by PD).
        /// </summary>
        [ProtoIgnore]
        public bool RequiresMissileHPAboveZero
        {
            get
            {
                return SPL_ShrapnelSpawnRequirements.HasFlag(ShrapnelSpawnRequirements.RequiresShellHealthpoolAboveZero);
            }
            set
            {
                if (value)
                    SPL_ShrapnelSpawnRequirements |= ShrapnelSpawnRequirements.RequiresShellHealthpoolAboveZero;

                else
                    SPL_ShrapnelSpawnRequirements &= ~ShrapnelSpawnRequirements.RequiresShellHealthpoolAboveZero;
            }
        }

        /// <summary>
        /// (Missile only) Set to true to require the missile's HP value to be equal to zero (basically the missile did get shot down by PD).
        /// </summary>
        [ProtoIgnore]
        public bool RequiresMissileHPAtZero
        {
            get
            {
                return SPL_ShrapnelSpawnRequirements.HasFlag(ShrapnelSpawnRequirements.RequiresShellHealthpoolAtZero);
            }
            set
            {
                if (value)
                    SPL_ShrapnelSpawnRequirements |= ShrapnelSpawnRequirements.RequiresShellHealthpoolAtZero;

                else
                    SPL_ShrapnelSpawnRequirements &= ~ShrapnelSpawnRequirements.RequiresShellHealthpoolAtZero;
            }
        }

        /// <summary>
        /// Speficies any requirements to spawn shrapnel. Multiple can be used at a time, using the separator character '|'. Defaults to an 'or' like state where if multiple are used, any one condition being true can cause shrapnel to spawn.
        /// Using 'RequiresAll' will change that to an 'and' state, where every condition listed must be met.
        /// Conditions use the missile/beam end point for these conditions.
        /// <para>
        /// <code>
        /// DO NOT ASSIGN ANYTHING TO THIS. IT WILL BE OVERWRITTEN BY THE BOOLEANS GIVEN.
        /// </code>
        /// </para>
        /// Defaults to NoRequirements
        /// <para>
        /// Availible Types:  
        /// <code>
        /// NoRequirements - Shrapnel will always spawn. Mutually exclusive with everything else
        /// RequiresAll - Signals that every condition must be present for the shrapnel to spaw
        /// RequiresNaturalGravity - There must be natural gravity present
        /// RequiresArtificialGravity - There must be artificial gravity present
        /// RequiresAnyGravity - There must be some sort of gravity present
        /// RequiresNoNaturalGravity - There must be no natural gravity present
        /// RequiresNoArtificialGravity - There must be no artificial gravity present
        /// RequiresNoGravity - There must be no gravity present
        /// RequiresCollision - The missile/beam has hit something
        /// RequiresEndOfLife - The missile/beam has not hit anything
        /// RequiresMainSpawnDirection - The Main Spawn Direction listed in the definition was the direction used
        /// RequiresBackupSpawnDirection - The Backup Spawn Direction listed in the definition was the direction used
        /// RequiresShellArmed - (Missile only) requires the shell to be armed.
        /// RequiresShellNotArmed - (Missile only) requires the shell to be not armed.
        /// </code>
        /// </para>
        /// </summary>
        [ProtoMember(10)]
        public ShrapnelSpawnRequirements SPL_ShrapnelSpawnRequirements
        {
            get;
            private set;
        }

        /// <summary>
        /// Delay in ticks to spawn the shrapnel. Set to zero to spawn instantly. Shrapnel will spawn at the point it would normally spawn regardless of any percieved inherited velocity. Set to zero to disable.
        /// <para>
        /// Unit: Time (ticks)
        /// </para>
        /// <para>
        /// Should be greater than or equal to zero.
        /// </para>
        /// </summary>
        [ProtoMember(11)]
        public int SPL_SpawnDelay;

        /// <summary>
        /// Randomized addition to SPL_SpawnDelay. Set to zero for no variance. Shrapnel will spawn at the point it would normally spawn regardless of any percieved inherited velocity. Set to zero to disable.
        /// <para>
        /// Unit: Time (ticks)
        /// </para>
        /// <para>
        /// Should be greater than or equal to zero.
        /// </para>
        /// </summary>
        [ProtoMember(13)]
        public int SPL_SpawnDelayVariance;

        /// <summary>
        /// Sets the maximum amount of shrapnel layers for beams only; only has an effect when beams spawning more beam shrapnel are used. Can be used to limit recursive beams for a line of explosions and such. Set to zero to disable.
        /// <para>
        /// Unit: Amount
        /// </para>
        /// <para>
        /// Should be greater than or equal to zero.
        /// </para>
        /// </summary>
        [ProtoMember(12)]
        public int SPL_MaxShrapnelRecursion;

        public override string ToString()
        {
            return $"Shrapnel Name: {SPL_ShrapnelDefinitionName}." +
                $"\n Outer Cone Radius: {SPL_OuterConeAngleInDegrees}." +
                $"\n Inner Cone Radius: {SPL_InnerConeAngleInDegrees}." +
                $"\n Shrapnel Count: {SPL_MinShrapnelCount} - {SPL_MaxShrapnelCount}.";
        }
    }

    /// <summary>
    /// Struct containing variables controlling EMP behavior
    /// </summary>
    [ProtoContract]
    public struct EMP_Logic
    {
        /// <summary>
        /// Radius the EMP effect will affect. This will turn off all blocks within this radius.
        /// <para>
        /// Unit: Meters
        /// </para>
        /// <para>
        /// Should never be zero or negative.
        /// </para>
        /// </summary>
        [ProtoMember(1)]
        public float EMP_Radius;
        /// <summary>
        /// How long anything disabled by the round will be forced turned off for.
        /// Anything EMP'd already will have their disabled time increased by half of this.
        /// <para>
        /// Unit: Ticks
        /// </para>
        /// <para>
        /// Should never be zero or negative.
        /// </para>
        /// </summary>
        [ProtoMember(2)]
        public int EMP_TimeDisabled;

        /// <summary>
        /// Determines if this logic is allowed to affect the firing ship and its subgrids.
        /// </summary>
        [ProtoMember(3)]
        public bool EMP_AllowFriendlyFire;
        public EMP_Logic(float EMP_Radius, int EMP_TimeDisabled)
        {
            this.EMP_Radius = EMP_Radius;
            this.EMP_TimeDisabled = EMP_TimeDisabled;
            EMP_AllowFriendlyFire = false;
        }

        public override string ToString()
        {
            return $" EMP_Radius: {EMP_Radius} EMP_TimeDisabled: {EMP_TimeDisabled}";
        }
    }

    /// <summary>
    /// Struct containing variables controlling behavior relating to guided missiles.
    /// </summary>
    [ProtoContract]
    public struct GuidanceLock_Logic
    {
        /// <summary>
        /// <para>
        /// Storage for the 2D array controlling the missile guidance function.
        /// </para>
        /// <para>
        /// Each horizontal row describes one function, with the start point at the leftmost value, and endpoint at the next row's start.
        /// Vertical columns describe the coefficients, with the leftmost one being the time index, and each one after that being part of the function itself, with the x^n increasing the further rightward.
        /// </para>
        /// <example>
        /// Example:
        /// <code>
        /// {
        ///     {0, 1, 5},
        ///     {5, 6, 0},
        /// }
        /// </code>
        /// Gives a function of 1+5x from time 0 to 5,
        /// and a function of 6+0x from time 5 to positive infinity.
        /// </example>
        /// <para>
        /// The units this function is in is seconds for input, and degrees per second for output, so if the function at a point returns 1, the missile will home in at 1 degree per second.
        /// </para>
        /// <para>
        /// Set to <c>null</c> to disable any guided behavior. Time index should never be negative.
        /// </para>
        /// </summary>
        [ProtoMember(1)]
        public List<DoubleArray> GL_HomingPiecewisePolynomialFunction;

        /// <summary>
        /// Percent chance that any guided missile will retarget this missile if this missile is within a certain radius of the guided missile's target when checked.
        /// This check will happen around 6 times per second.
        /// <para>
        /// Unit: Percent
        /// </para>
        /// <para>
        /// Set to 0 to disable any decoy-like behavior. Should never be negative.
        /// </para>
        /// </summary>
        [ProtoMember(2)]
        public double GL_DecoyPercentChanceToCauseRetarget;

        /// <summary>
        /// Radius that controls if a check to see if the guided missile will retarget will happen. 
        /// This check will happen around 6 times per second. The guided missile's target must be within this radius of the missile for the check to happen.
        /// <para>
        /// Unit: Meters
        /// </para>
        /// <para>
        /// Set to 0 to disable any decoy-like behavior. Should never be negative.
        /// </para>
        /// </summary>
        [ProtoMember(3)]
        public float GL_DecoyRetargetRadius;

        /// <summary>
        /// Controls how the missile will aquire a target. Topmost in this list takes priority.
        /// <para>
        /// <code>
        /// None - don't use if you intend for the missile to guide. Use on flares
        /// LockOn - Missile will target whatever the player in the grid is locked on to
        /// TurretTarget - Missile will target whatever the turret that fired it is currently targeting
        /// DesignatedPosition - Missile will use turret designators specified in TurretDefinitions to aquire targets based off of what it is targeting / pointing at
        /// OneTimeRaycast - Missile will perform a one time raycast on spawn direclty forward for its maximum range to attempt to aquire a target
        /// </code>
        /// </para>
        /// </summary>
        [ProtoMember(4)]
        public GuidanceType GL_AllowedGuidanceTypes;




        /// <summary>
        /// Set to true if missile has no guidance
        /// </summary>
        [ProtoIgnore]
        public bool NoGuidance
        {
            get
            {
                return GL_AllowedGuidanceTypes == None;
            }
            set
            {
                if (value)
                    GL_AllowedGuidanceTypes = None;
            }
        }

        /// <summary>
        /// Set to true if missile uses lock on guidance
        /// </summary>
        [ProtoIgnore]
        public bool UseLockOn
        {
            get
            {
                return GL_AllowedGuidanceTypes.HasFlag(LockOn);
            }
            set
            {
                if (value)
                    GL_AllowedGuidanceTypes |= LockOn;

                else
                    GL_AllowedGuidanceTypes &= ~LockOn;
            }
        }

        /// <summary>
        /// Set to true if missile uses turret targeting
        /// </summary>
        [ProtoIgnore]
        public bool UseTurretTarget
        {
            get
            {
                return GL_AllowedGuidanceTypes.HasFlag(TurretTarget);
            }
            set
            {
                if (value)
                    GL_AllowedGuidanceTypes |= TurretTarget;

                else
                    GL_AllowedGuidanceTypes &= ~TurretTarget;
            }
        }

        /// <summary>
        /// Set to true if missile uses designated position
        /// </summary>
        [ProtoIgnore]
        public bool UseDesignatedPosition
        {
            get
            {
                return GL_AllowedGuidanceTypes.HasFlag(DesignatedPosition);
            }
            set
            {
                if (value)
                    GL_AllowedGuidanceTypes |= DesignatedPosition;

                else
                    GL_AllowedGuidanceTypes &= ~DesignatedPosition;
            }
        }

        /// <summary>
        /// Set to true if missile uses one time raycast
        /// </summary>
        public bool UseOneTimeRaycast
        {
            get
            {
                return GL_AllowedGuidanceTypes.HasFlag(OneTimeRaycast);
            }
            set
            {
                if (value)
                    GL_AllowedGuidanceTypes |= OneTimeRaycast;

                else
                    GL_AllowedGuidanceTypes &= ~OneTimeRaycast;
            }
        }
        /// <summary>
        /// If true, then on missile death, rotates the missile to its target
        /// </summary>
        [ProtoMember(5)]
        public bool GL_RotateToTargetOnDeath;

        /// <summary>
        /// Controls what variable the missile will use for its guidance function. Valid types:
        /// <para>
        /// <code>
        /// TimeSinceSpawn - Time since spawn in seconds. Default.
        /// DistanceToTarget - Distance to target in meters.
        /// DistanceFromOrigin - Distance from missile origin (where the missile spawned) in meters.
        /// AngleToTarget - Angle to target in degrees.
        /// </code>
        /// </para>
        /// </summary>
        [ProtoMember(6)]
        public GuidanceFunctionVariable GL_GuidanceFunctionVariable;

        /// <summary>
        /// Reduces the percent chance that this missile will retarget onto a given flare by the amount stated (or increases if negative). Should the resulting percent chance be below zero the missile will not retarget, and if above 100% the missile will always retarget.
        /// Set to int.MaxValue in order to make the missile unable to retarget at all.
        /// <para>
        /// Unit: Percent
        /// </para>
        /// <para>
        /// Should be a real number.
        /// </para>
        /// </summary>
        [ProtoMember(7)]
        public int GL_FlareResistance;

        /// <summary>
        /// Controls whether the missile can only target preferred targets (true) or only prioritizes preferred targets over other blocks (false).
        /// </summary>
        [ProtoMember(8)]
        public bool GL_TargetOnlyPreferredTargets;

        /// <summary>
        /// Controls whether the <c>GL_PreferredTargetIDs</c> is a list of <c>SubtypeId</c>s or <c>TypeId</c>s
        /// </summary>
        [ProtoMember(9)]
        public IdType GL_TargetIDType;

        /// <summary>
        /// List of <c>SubtypeIds</c> or <c>TypeIds</c> that constitute the missile's perferred target. Preferred targets are always targeted first before any other blocks.
        /// </summary>
        [ProtoMember(10)]
        public List<string> GL_PreferredTargetIDs;


        /// /// <summary>
        /// Range at which the missile detonates from the target. Set to zero to disable. Useful for staging munitions while keeping lag down. Set to zero to disable.
        /// <para>
        /// Unit: Meters
        /// </para>
        /// <para>
        /// Should be a positive real number or zero.
        /// </para>
        /// </summary>
        [ProtoMember(11)]
        public float GL_MissileStageOnRangeToTarget;
        public GuidanceLock_Logic(double[,] GL_HomingPiecewisePolynomialFunction, double GL_DecoyPercentChanceToCauseRetarget, float GL_DecoyRetargetRadius, GuidanceType GL_AllowedGuidanceTypes)
        {
            this.GL_HomingPiecewisePolynomialFunction = DefinitionTools.ConvertToDoubleArrayList(GL_HomingPiecewisePolynomialFunction);
            this.GL_DecoyPercentChanceToCauseRetarget = GL_DecoyPercentChanceToCauseRetarget;
            this.GL_DecoyRetargetRadius = GL_DecoyRetargetRadius;
            this.GL_AllowedGuidanceTypes = GL_AllowedGuidanceTypes;
            GL_RotateToTargetOnDeath = false;
            GL_GuidanceFunctionVariable = GuidanceFunctionVariable.TimeSinceSpawn;
            GL_FlareResistance = 0;

            GL_TargetOnlyPreferredTargets = false;
            GL_TargetIDType = IdType.SubtypeId;
            GL_PreferredTargetIDs = null;
            GL_MissileStageOnRangeToTarget = 0;
        }

        public GuidanceLock_Logic(List<DoubleArray> GL_HomingPiecewisePolynomialFunction, double GL_DecoyPercentChanceToCauseRetarget, float GL_DecoyRetargetRadius, GuidanceType GL_AllowedGuidanceTypes)
        {
            this.GL_HomingPiecewisePolynomialFunction = GL_HomingPiecewisePolynomialFunction;
            this.GL_DecoyPercentChanceToCauseRetarget = GL_DecoyPercentChanceToCauseRetarget;
            this.GL_DecoyRetargetRadius = GL_DecoyRetargetRadius;
            this.GL_AllowedGuidanceTypes = GL_AllowedGuidanceTypes;
            GL_RotateToTargetOnDeath = false;
            GL_GuidanceFunctionVariable = GuidanceFunctionVariable.TimeSinceSpawn;
            GL_FlareResistance = 0;

            GL_TargetOnlyPreferredTargets = false;
            GL_TargetIDType = IdType.SubtypeId;
            GL_PreferredTargetIDs = null;
            GL_MissileStageOnRangeToTarget = 0;
        }

        public override string ToString()
        {
            string func = "";

            if (GL_HomingPiecewisePolynomialFunction != null)
            {
                foreach (DoubleArray arr in GL_HomingPiecewisePolynomialFunction)
                {
                    func += "{ " + $"{arr}" + " },\n";
                }
            }

            return $"Homing Function: {func} GL_DecoyPercentChanceToCauseRetarget: {GL_DecoyPercentChanceToCauseRetarget} GL_DecoyRetargetRadius: {GL_DecoyRetargetRadius}";
        }
    }

    /// <summary>
    /// Struct containing variables controlling proximity detonation behavior.
    /// </summary>
    [ProtoContract]
    public struct ProximityDetonation_Logic
    {
        /// <summary>
        /// Damage dealt to any missiles it proximity detonates against. Serves as PD. Missiles will default to 1 HP, and can be specified in the VPFAmmoDefinition
        /// <para>
        /// Units: Damage (unitless)
        /// </para>
        /// <para>
        /// Set to 0 to disable proximity detonation against missiles.
        /// </para>
        /// </summary>
        [ProtoMember(1)]
        public float PD_AntiMissileDamage;

        /// <summary>
        /// Radius at which the missile will detect any hostile grids, players, or missiles if set. If a hostile is detected, the missile detonates.
        /// Note: This does not control the explosion's radius, that is handled in Ammo.sbc.
        /// <para>
        /// Units: Meters
        /// </para>
        /// <para>
        /// Should never be zero or negative.
        /// </para>
        /// </summary>
        [ProtoMember(2)]
        public float PD_DetonationRadius;

        /// <summary>
        /// Radius at which the missile will damage nearby missiles on death.
        /// <para>
        /// Units: Meters
        /// </para>
        /// <para>
        /// Should never be negative.
        /// Set to zero to use PD_DetonationRadius.
        /// </para>
        /// </summary>
        [ProtoMember(5)]
        public float PD_MissileDamageRadiusOnDeath;

        /// <summary>
        /// Set to true to convert the missile to a timed fuse, which causes the projectile to explode after it has traveled the distance to the where the target will be given its current velocity on launch plus a random number between -PD_TimedFuseRandomRangeOffset and PD_TimedFuseRandomRangeOffset.
        /// (Sets missile max trajectory to the distance to where the target will be given its current velocity plus its random offset).
        /// <para>
        /// Requires Guidance logic in order to get a target. A missile with no Guidance target and this active will do nothing.
        /// </para>
        /// </summary>
        [ProtoMember(3)]
        public bool PD_UseToTimedFuse;

        /// <summary>
        /// Random offset to the timed fuse's max trajectory, which will add a random number between -PD_TimedFuseRandomRangeOffset and PD_TimedFuseRandomRangeOffset to a missile using timed fuse.
        /// <para>
        /// Units: Meters
        /// </para>
        /// <para>
        /// Should never be negative.
        /// Only use if PD_ConvertToTimedFuse is true.
        /// </para>
        /// </summary>
        [ProtoMember(4)]
        public float PD_TimedFuseRandomRangeOffset;

        /// <summary>
        /// Determines if this logic is allowed to damage friendly missiles.
        /// </summary>
        [ProtoMember(6)]
        public bool PD_AllowFriendlyMissileDamage;

        public ProximityDetonation_Logic(float PD_AntiMissileDamage, float PD_DetonationRadius)
        {
            this.PD_AntiMissileDamage = PD_AntiMissileDamage;
            this.PD_DetonationRadius = PD_DetonationRadius;
            PD_UseToTimedFuse = false;
            PD_TimedFuseRandomRangeOffset = 0;
            PD_MissileDamageRadiusOnDeath = 0;
            PD_AllowFriendlyMissileDamage = false;
        }

        public override string ToString()
        {
            return $" PD_AntiMissileDamage: {PD_AntiMissileDamage}\tPD_DetonationRadius: {PD_DetonationRadius}\tPD_ConvertToTimedFuse: {PD_UseToTimedFuse}\tPD_TimedFuseRandomRangeOffset: {PD_TimedFuseRandomRangeOffset}\tPD_AllowFriendlyMissileDamage: {PD_AllowFriendlyMissileDamage}";
        }
    }


    /// <summary>
    /// Struct containing variables controlling jump drive power drain on hit.
    /// </summary>
    [ProtoContract]
    public struct JumpDriveInhibition_Logic
    {
        /// <summary>
        /// How much power will be removed from any hit grid's jump drives, if any, if possible.
        /// <para>
        /// Units: Watt Hours
        /// </para>
        /// <para>
        /// Should never be zero or negative.
        /// </para>
        /// </summary>
        [ProtoMember(1)]
        public float JDI_PowerDrainInW;

        /// <summary>
        /// Should the power drain be distributed evenly across EACH jump drive (true), or remove that amount from EVERY jump drive (false).
        /// </summary>
        [ProtoMember(2)]
        public bool JDI_DistributePower;

        /// <summary>
        /// Determines if this logic is allowed to affect the firing ship and its subgrids.
        /// </summary>
        [ProtoMember(3)]
        public bool JDI_AllowFriendlyFire;

        public JumpDriveInhibition_Logic(float JDI_PowerDrainInW, bool JDI_DistributePower)
        {
            this.JDI_PowerDrainInW = JDI_PowerDrainInW;
            this.JDI_DistributePower = JDI_DistributePower;
            JDI_AllowFriendlyFire = false;
        }

        public override string ToString()
        {
            return $" JDI_PowerDrainInW: {JDI_PowerDrainInW}\tJDI_DistributePower: {JDI_DistributePower}\tJDI_AllowFriendlyFire:{JDI_AllowFriendlyFire}";
        }
    }


    /// <summary>
    /// Struct containing variables controlling any beam weaponry.
    /// </summary>
    [ProtoContract]
    public struct BeamWeaponType_Logic
    {
        /// <summary>
        /// Controls how long will the beam be rendered. Note: Only one tick of damage will still happen.
        /// <para>
        /// Units: Ticks
        /// </para>
        /// <para>
        /// Should never be zero or negative.
        /// </para>
        /// </summary>
        [ProtoMember(1)]
        public int BWT_TimeActive;
        /// <summary>
        /// <para>
        /// Storage for the 2D array controlling damage falloff for the beam as a piecewise polynomial function.
        /// </para>
        /// <para>
        /// Each horizontal row describes one function, with the start point at the leftmost value, and endpoint at the next row's start.
        /// Vertical columns describe the coefficients, with the leftmost one being the time index, and each one after that being part of the function itself, with the x^n increasing the further rightward.
        /// </para>
        /// <example>
        /// Example:
        /// <code>
        /// {
        ///     {0, 1, 5},
        ///     {5, 6, 0},
        /// }
        /// </code>
        /// Gives a function of 1+5x from time 0 to 5,
        /// and a function of 6+0x from time 5 to positive infinity.
        /// </example>
        /// <para>
        /// The units this function is in is meters for input, and percent for output, with 100 being 100% and 0 being 0%. If this evaluates to a negative number it will be set to 0. Values over 100% are possible.
        /// </para>
        /// </summary>
        [ProtoMember(2)]
        public List<DoubleArray> BWT_DamageFalloffPiecewisePolynomialFunction;
        /// <summary>
        /// Controls how thick will the beam be when rendered.
        /// <para>
        /// Units: Keen doesn't tell me, so best guess is pixels
        /// </para>
        /// <para>
        /// Should never be zero or negative.
        /// </para>
        /// </summary>
        [ProtoMember(3)]
        public float BWT_BeamThickness;
        /// <summary>
        /// Controls the color.
        /// <br>If R is negative, then the beam will use the firing block's color value multiplied by the absolute value of R. If the firing block doesn't exist (character, projectile based, shrapnel, etc) then white will be used. A is unaffected. </br>
        /// <br>Color's A value shoulD ALWAYS be below 1, even after an multiplications.</br>
        /// <para>
        /// Units: RGBA format, in Vector4.
        /// </para>
        /// <para>
        /// No part should be negative except value except R/X.
        /// </para>
        /// </summary>
        [ProtoMember(4)]
        public Vector4 BWT_BeamColor;
        /// <summary>
        /// Controls if the missile's explosion FX defined in the sbc will be rendered (true) or not (false).
        /// <para>
        /// Units: Bpp;
        /// </para>
        /// </summary>
        [ProtoMember(5)]
        public bool BWT_ShowExplosionFX;
        /// <summary>
        /// Bool to fade the beam to black over time. (Lerps alpha from its intended value to zero from start of time active to end).
        /// <para>
        /// Units: Bool
        /// </para>
        /// </summary>
        [ProtoMember(6)]
        public bool BWT_Fade;
        /// <summary>
        /// Distance to offset the start of the beam forwards/backwards. Positive is forwards, negative is backwards.
        /// <para>
        /// Units: Meters
        /// </para>
        /// </summary>
        [ProtoMember(7)]
        public float BeamRenderOffset;

        /// <summary>
        /// Specifies the what the damage source type is dealt from the beam's penetration damage, ie. "Explosion" for explosive damage, "Environment" for collisions and stuff, etc.
        /// This does NOT affect the damage type of any explosions caused by the beam. Defaults to "WeaponLaser".
        /// <para>
        /// Useful types:
        /// </para>
        /// <code>
        /// Bullet - projectile and penetration damage
        /// Deformation - damage that makes armor deform
        /// Explosion - explosive damage
        /// Environment - character collision damage and other things
        /// IgnoreShields - will make Cython Energy Shields ignore this damage
        /// </code>
        /// </summary>
        [ProtoMember(8)]
        public string BWT_DamageSourceType;

        /// <summary>
        /// When set to true, causes the beam to completely ignore collisions.
        /// <para>
        /// Unit: N/A
        /// </para>
        /// </summary>
        [ProtoMember(9)]
        public bool BWT_IgnoreCollisions;
        public BeamWeaponType_Logic(int BWT_TimeActive, double[,] BWT_DamageFalloffPiecewisePolynomialFunction, float BWT_BeamThickness, Vector4 BWT_BeamColor, bool BWT_ShowExplosionFX, bool BWT_Fade, float BeamRenderOffset)
        {
            this.BWT_TimeActive = BWT_TimeActive;
            this.BWT_DamageFalloffPiecewisePolynomialFunction = DefinitionTools.ConvertToDoubleArrayList(BWT_DamageFalloffPiecewisePolynomialFunction);
            this.BWT_BeamThickness = BWT_BeamThickness;
            this.BWT_BeamColor = BWT_BeamColor;
            this.BWT_ShowExplosionFX = BWT_ShowExplosionFX;
            this.BWT_Fade = BWT_Fade;
            this.BeamRenderOffset = BeamRenderOffset;
            BWT_DamageSourceType = "WeaponLaser";
            BWT_IgnoreCollisions = false;
        }

        public BeamWeaponType_Logic(int BWT_TimeActive, List<DoubleArray> BWT_DamageFalloffPiecewisePolynomialFunction, float BWT_BeamThickness, Vector4 BWT_BeamColor, bool BWT_ShowExplosionFX, bool BWT_Fade, float BeamRenderOffset)
        {
            this.BWT_TimeActive = BWT_TimeActive;
            this.BWT_DamageFalloffPiecewisePolynomialFunction = BWT_DamageFalloffPiecewisePolynomialFunction;
            this.BWT_BeamThickness = BWT_BeamThickness;
            this.BWT_BeamColor = BWT_BeamColor;
            this.BWT_ShowExplosionFX = BWT_ShowExplosionFX;
            this.BWT_Fade = BWT_Fade;
            this.BeamRenderOffset = BeamRenderOffset;
            BWT_DamageSourceType = "WeaponLaser";

            BWT_IgnoreCollisions = false;
        }

        public override string ToString()
        {
            string func = "";

            if (BWT_DamageFalloffPiecewisePolynomialFunction != null)
            {
                foreach (DoubleArray arr in BWT_DamageFalloffPiecewisePolynomialFunction)
                {
                    func += "{ " + $"{arr}" + " },\n";
                }
            }

            return $" BWT_TimeActive: {BWT_TimeActive}\tBWT_DamageFalloffPiecewisePolynomialFunction: {func}\tBWT_BeamThickness: {BWT_BeamThickness}\t" +
                $"BWT_BeamColor: {BWT_BeamColor}\tBWT_ShowExplosionFX: {BWT_ShowExplosionFX}\tBWT_Fade: {BWT_Fade}\t";
        }
    }

    /// <summary>
    /// <para>
    /// Struct containing variables controlling special behavior.
    /// Special behavior affects all blocks on the hit grid regardless of distance hit.
    /// </para>
    /// Useful for things like damaging and disabling remote controls for a time, or antennas, etc.
    /// </summary>
    [ProtoContract]
    public struct SpecialComponentryInteraction_Logic
    {
        /// <summary>
        /// String for the Id of the block as defined in Cubeblocks.sbc. Can be type or subtype Id, which is configured in <c>IsSubtype</c>.
        /// <para>
        /// Should never be the empty string.
        /// </para>
        /// </summary>
        [ProtoMember(1)]
        public string SCI_BlockId;
        /// <summary>
        /// Controls whether the <c>BlockIdName</c> is a <c>SubtypeId</c> or <c>TypeId</c>
        /// </summary>
        [ProtoMember(2)]
        public IdType SCI_IdType;
        /// <summary>
        /// Damage dealt to the every block with the specified type or subtype on the hit grid. Set to zero to disable damaging these blocks.
        /// <para>
        /// Units: Damage (Unitless) or Percent (Defined in the IsPercent bool)
        /// </para>
        /// <para>
        /// Should never be negative.
        /// </para>
        /// </summary>
        [ProtoMember(3)]
        public float SCI_DamageDealt;
        /// <summary>
        /// Determines if the unit of <c>SCI_DamageDealt</c> is percent (true) or actual damage (false).
        /// </summary>
        [ProtoMember(4)]
        public DamageType SCI_DamageType;
        /// <summary>
        /// Applies an EMP effect to the affected blocks for this variable's amount of time. Set to zero to disable.
        /// <para>
        /// Units: Ticks
        /// </para>
        /// <para>
        /// Should never be negative.
        /// </para>
        /// </summary>
        [ProtoMember(5)]
        public float SCI_DisableTime;

        /// <summary>
        /// Radius of effect on the grid. Set to 0 to disable. Defaults to 0.
        /// <para>
        /// Units: meters
        /// </para>
        /// <para>
        /// Should never be negative.
        /// </para>
        /// </summary>
        [ProtoMember(6)]
        public float SCI_Radius;

        /// <summary>
        /// Specifies the what the damage source type is in the DoDamage function, ie. "Explosion" for explosive damage, "Environment" for collisions and stuff, etc.
        /// <para>
        /// Useful types:
        /// </para>
        /// <code>
        /// Bullet - projectile & penetration damage
        /// Deformation
        /// Explosion - explosive damage
        /// Environment - character collision damage & other things
        /// IgnoreShields - will make Cython Energy Shields ignore this damage
        /// </code>
        /// </summary>
        [ProtoMember(7)]
        public string SCI_DamageSourceType;

        /// <summary>
        /// Determines if this logic is allowed to affect the firing ship and its subgrids.
        /// </summary>
        [ProtoMember(8)]
        public bool SCI_AllowFriendlyFire;

        public SpecialComponentryInteraction_Logic(string SCI_BlockId, IdType SCI_IdType, float SCI_DamageDealt, DamageType SCI_DamageType, float SCI_DisableTime, float SCI_Radius)
        {
            this.SCI_BlockId = SCI_BlockId;
            this.SCI_IdType = SCI_IdType;
            this.SCI_DamageDealt = SCI_DamageDealt;
            this.SCI_DamageType = SCI_DamageType;
            this.SCI_DisableTime = SCI_DisableTime;
            this.SCI_Radius = SCI_Radius;
            SCI_DamageSourceType = "WeaponLaser";
            SCI_AllowFriendlyFire = false;
        }

        public override string ToString()
        {
            return $" SCI_BlockIdName: {SCI_BlockId}\tSCI_IdType: {SCI_IdType}\tSCI_DamageDealt: {SCI_DamageDealt}\tSCI_IsDamagePercent: {SCI_DamageType}\t" +
                $" SCI_DisableTime: {SCI_DisableTime}";
        }
    }
    [ProtoContract]
    public class VPFAmmoDefinition : VPFDefinition
    {
        /// <summary>
        /// Name of the Subtype of the ammo defined in Ammo.sbc paired with the following stats.
        /// <para>
        /// Should never be the empty string or null.
        /// </para>
        /// </summary>
        [ProtoMember(1)]
        public string subtypeName = null;

        /// <summary>
        /// Name of the Subtype of the FX subtype defined in VPF FX Definitions. Set to null to disable.
        /// </summary>
        [ProtoMember(2)]
        public string FXsubtypeName = null;

        /// <summary>
        /// Actual healthpool of the missile.
        /// <para>
        /// Units: Health (Unitless)
        /// </para>
        /// <para>
        /// Should never be zero or negative. (Unless you want to cause the missile to die on firing)
        /// </para>
        /// </summary>
        [ProtoMember(3)]
        public float VPF_MissileHitpoints;

        /// <summary>
        /// Struct for all of the stats relating to the missile causing an EMP effect.
        /// <para>
        /// Incompatible with: None
        /// </para>
        /// <para>
        /// SET TO NULL IF YOU DO NOT WANT TO USE THIS LOGIC. HOW TO DO SO IS LISTED BELOW
        /// <code>
        /// EMP_Stats = null,
        /// </code>
        /// </para>
        /// </summary>
        [ProtoMember(4)]
        public EMP_Logic? EMP_Stats = null;
        /// <summary>
        /// Struct for all of the stats relating to guided missiles and decoys against them.
        /// <para>
        /// <para>
        /// Incompatible with: Beam Weapon Type. Beam will take priority
        /// </para>
        /// SET TO NULL IF YOU DO NOT WANT TO USE THIS LOGIC. HOW TO DO SO IS LISTED BELOW
        /// <code>
        /// GL_Stats = null,
        /// </code>
        /// </para>
        /// </summary>
        [ProtoMember(5)]
        public GuidanceLock_Logic? GL_Stats = null;

        /// <summary>
        /// <para>Struct for all of the stats relating to proximity detonation of the missile.
        /// <br>Incompatible with: Beam Weapon Type. Beam will take priority</br>
        /// <br>Partially Incompatible with: Jump Drive Inhibition, Special Componentry Interaction (both require a direct hit, which proximity detonation usually never allows)</br>
        /// </para>
        /// <para>
        /// SET TO NULL IF YOU DO NOT WANT TO USE THIS LOGIC. HOW TO DO SO IS LISTED BELOW
        /// <code>
        /// PD_Stats = null,
        /// </code>
        /// </para>
        /// </summary>
        [ProtoMember(6)]
        public ProximityDetonation_Logic? PD_Stats = null;
        /// <summary>
        /// Struct for all of the stats relating to jump drive inhibition by removing jump drive charge.
        /// <para>
        /// <para>
        /// Incompatible with: None
        /// Partially Incompatible with: Proximity Detonation (This logic requires a direct hit, which proximity detonation usually never allows)
        /// </para>
        /// SET TO NULL IF YOU DO NOT WANT TO USE THIS LOGIC. HOW TO DO SO IS LISTED BELOW
        /// <code>
        /// JDI_Stats = null,
        /// </code>
        /// </para>
        /// </summary>
        [ProtoMember(7)]
        public JumpDriveInhibition_Logic? JDI_Stats = null;
        /// <summary>
        /// Struct for all of the stats relating to turning the missile into a hitreg beam weapon.
        /// <para>
        /// <para>
        /// Incompatible with: Proximity detonation, Guided Missiles. Beam will take priority
        /// </para>
        /// SET TO NULL IF YOU DO NOT WANT TO USE THIS LOGIC. HOW TO DO SO IS LISTED BELOW
        /// <code>
        /// BWT_Stats = null,
        /// </code>
        /// </para>
        /// </summary>
        [ProtoMember(8)]
        public BeamWeaponType_Logic? BWT_Stats = null;
        /// <summary>
        /// <para>Struct for all of the stats relating to special componentry interaction logic.
        /// <br>Essentually, for each logic in the list, any blocks with the matching TypeId or SubtypeId (depends whats specified), will be affected by its following parameters.</br>
        /// <br>Parameters are defined in more detail  by themselves.</br></para>
        /// <example>
        /// Say something like this is written
        /// <code>
        /// SCI_BlockIdName = Thrust,
        /// SCI_IdType = TypeId,
        /// SCI_DamageDealt = 100,
        /// SCI_DamageType = Percent,
        /// SCI_DisableTime = 1
        /// </code>
        /// This will apply, to every thruster on the hit grid, 1 second of forced disable like EMP, and deal 100% of the block's health as damage.
        /// </example>
        /// <para>
        /// <para>
        /// Incompatible with: None
        /// Partially Incompatible with: Proximity Detonation (This logic requires a direct hit, which proximity detonation usually never allows)
        /// </para>
        /// SET TO NULL IF YOU DO NOT WANT TO USE THIS LOGIC. HOW TO DO SO IS LISTED BELOW
        /// <code>
        /// SCI_Stats = null,
        /// </code>
        /// </para>
        /// </summary>
        [ProtoMember(9)]
        public List<SpecialComponentryInteraction_Logic> SCI_Stats = null;

        /// <summary>
        /// List for all of the stats relating to the missile or beam releasing beam shrapnel on death.
        /// <para>
        /// <para>
        /// Incompatible with: None
        /// </para>
        /// SET TO NULL IF YOU DO NOT WANT TO USE THIS LOGIC. HOW TO DO SO IS LISTED BELOW
        /// <code>
        /// SPL_Stats = null,
        /// </code>
        /// </para>
        /// </summary>
        [ProtoMember(12)]
        public List<Shrapnel_Logic> SPL_Stats = null;

        /// <summary>
        /// <para>if a missile contains both penetration and explosive damage, setting this to true will implement a fix to make the missile damage properly.
        /// <br> - Missile will explode at the end of its penetration tunnel, instead of on the surface.</br>
        /// <br>Defaults to <c>false</c></br></para>
        /// <para>
        /// Values:
        ///  - true (impliments the fix)
        ///  - false (does nothing)
        /// Units: bool
        /// </para>
        /// </summary>
        [ProtoMember(10)]
        public bool NeedsAPHEFix = false;

        /// <summary>
        /// Removes all damage that the missile deals until the missile has lived for atleast this long. Set to zero to disable.
        /// <para>
        /// Units: Seconds
        /// </para>
        /// <para>
        /// Should never be less than zero.
        /// </para>
        /// </summary>
        [ProtoMember(11)]
        public float TimeToArm = 0;

        /// <summary>
        /// Amount of damage that the missile will deal to its firing weapon when fired. Negative values heal.
        /// <para>
        /// Units: Damage
        /// </para>
        /// <para>
        /// Should be a real number.
        /// </para>
        /// </summary>
        [ProtoMember(13)]
        public float OnFireSelfHarmDamage = 0;

        /// <summary>
        /// Type of damage that the missile will deal to its firing weapon when fired, ie. "Explosion" for explosive damage, "Environment" for collisions and stuff, etc.
        /// Defaults to "Bullet"
        /// <para>
        /// Useful types:
        /// </para>
        /// <list type="bullet">
        /// <item>Bullet - projectile and penetration damage</item>
        /// <item>Deformation - damage that makes armor deform</item>
        /// <item>Explosion - explosive damage</item>
        /// <item>Environment - character collision damage and other things</item>
        /// <item>IgnoreShields - will make Cython Energy Shields ignore this damage</item>
        /// </list>
        /// <para>
        /// Should be a valid string.
        /// </para>
        /// </summary>
        [ProtoMember(15)] public string OnFireSelfHarmDamageType;

        /// <summary>
        /// Armor value of the missile. This value reduces damage taken by the folowing formula:<list type="bullet">
        /// <item>Given Damage, Armor = 0</item>
        /// <item>Given Damage - Armor, Given Damage - Armor > 1</item>
        /// <item>Given Damage / (Armor + 1) otherwise</item>
        /// </list>
        /// <para>
        /// Units: Armor (Unitless)
        /// </para>
        /// <para>
        /// Should be a real number.
        /// </para>
        /// </summary>
        [ProtoMember(14)]
        public float VPF_MissileArmor = 0;

        /// <summary>
        /// Specifies how long the missile will remain alive for. Set to zero to disable.
        /// <br>Useful for guided missiles who may not always encounter their maximum range for a long time.</br>
        /// <para>
        /// Time in seconds
        /// </para>
        /// <para>
        /// Should be a number greater than or equal to zero.
        /// </para>
        /// </summary>
        [ProtoMember(16)] public float VPF_MissileLifeTime;

        /// <summary>
        /// Name of the Subtype of the FX subtype defined in VPF FX Definitions to be used when the missile dies. Set to null to disable.
        /// </summary>
        [ProtoMember(17)]
        public string EndOfLifeFXsubtypeName = null;

        /// <summary>
        /// Suppresses <MissileTrailEffect> in Ammos.sbc. Useful for modpacks where Vanilla+ is optional, and you want the vanilla one OR the vanilla+ one to be shown.
        /// </summary>
        [ProtoMember(18)]
        public bool VPF_SuppressMissileTrailEffect = false;

        public VPFAmmoDefinition()
        {
            subtypeName = "";
            FXsubtypeName = "";
            VPF_MissileHitpoints = -1;

            EMP_Stats = null;
            GL_Stats = null;
            PD_Stats = null;
            JDI_Stats = null;
            BWT_Stats = null;
            SCI_Stats = null;
            SPL_Stats = null;

            NeedsAPHEFix = false;
        }

        public VPFAmmoDefinition(string subtypeName, float VPF_MissileHitpoints, EMP_Logic? EMP_Stats = null, GuidanceLock_Logic? GL_Stats = null, ProximityDetonation_Logic? PD_Stats = null, JumpDriveInhibition_Logic? JDI_Stats = null, BeamWeaponType_Logic? BWT_Stats = null, List<SpecialComponentryInteraction_Logic> SCI_Stats = null)
        {
            this.subtypeName = subtypeName;
            this.VPF_MissileHitpoints = VPF_MissileHitpoints;

            this.EMP_Stats = EMP_Stats;
            this.GL_Stats = GL_Stats;
            this.PD_Stats = PD_Stats;
            this.JDI_Stats = JDI_Stats;
            this.BWT_Stats = BWT_Stats;
            this.SCI_Stats = SCI_Stats;
        }

        public override string ToString()
        {
            string str = "";

            if (SCI_Stats != null)
            {
                foreach (SpecialComponentryInteraction_Logic logic in SCI_Stats)
                {
                    str += logic.ToString() + "\n";
                }
            }

            string str2 = "";

            if (SPL_Stats != null)
            {
                foreach (var logic in SPL_Stats)
                {
                    str2 += logic.ToString() + "\n";
                }
            }


            return $" Ammo Subtype: {subtypeName}" +
                FXsubtypeName != null ? $"\n FXSubtypeName: {FXsubtypeName}" : "" +
                $"\nHitpoints: {VPF_MissileHitpoints}" +
                EMP_Stats != null ? $"\nEMP_Stats: {EMP_Stats}" : "" +
                GL_Stats != null ? $"\nGL_Stats: {GL_Stats}" : "" +
                PD_Stats != null ? $"\nPD_Stats: {PD_Stats}" : "" +
                JDI_Stats != null ? $"\nJDI_Stats: {JDI_Stats}" : "" +
                BWT_Stats != null ? $"\nBWT_Stats: {BWT_Stats}" : "" +
                SCI_Stats != null ? $"\nSCI_Stats: " + str : "" +
                SPL_Stats != null ? $"\nSPL_Stats: " + str2 : "";
        }

        public override bool Equals(object obj)
        {
            return obj is VPFAmmoDefinition ? ((VPFAmmoDefinition)obj).subtypeName == subtypeName : false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
