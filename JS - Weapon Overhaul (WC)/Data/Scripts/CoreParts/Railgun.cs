using static Scripts.Structure;
using static Scripts.Structure.WeaponDefinition;
using static Scripts.Structure.WeaponDefinition.ModelAssignmentsDef;
using static Scripts.Structure.WeaponDefinition.HardPointDef;
using static Scripts.Structure.WeaponDefinition.HardPointDef.Prediction;
using static Scripts.Structure.WeaponDefinition.TargetingDef.BlockTypes;
using static Scripts.Structure.WeaponDefinition.TargetingDef.Threat;
using static Scripts.Structure.WeaponDefinition.TargetingDef;
using static Scripts.Structure.WeaponDefinition.HardPointDef.HardwareDef;
using static Scripts.Structure.WeaponDefinition.HardPointDef.HardwareDef.HardwareType;

namespace Scripts
{
    partial class Parts
    {
        // Large Grid Railgun Turret
        WeaponDefinition RailgunTurret => new WeaponDefinition
        {
            Assignments = new ModelAssignmentsDef
            {
                MountPoints = new[]
                {
                    new MountPointDef { 
                        SubtypeId = "RailgunTurret", 
                        MuzzlePartId = "MissileTurretBarrels", 
                        AzimuthPartId = "MissileTurretBase1", 
                        ElevationPartId = "MissileTurretBarrels", 
                        DurabilityMod = 1f },
                },
                Muzzles = new[] 
                { 
                    "muzzle_missile_1" 
                },
                Scope = "muzzle_missile_1", // Where line of sight checks are performed from. Must be clear of block collision.
            },
            Targeting = new TargetingDef
            {
                Threats = new[] {
                    Grids,  // Types of threat to engage: Grids, Projectiles, Characters, Meteors, Neutrals
                },
                SubSystems = new[] {
                    Power, Utility, Offense, Thrust, Production, Any, // Subsystem targeting priority: Offense, Utility, Power, Production, Thrust, Jumping, Steering, Any
                },
                TopTargets = 2, // Maximum number of targets to randomize between; 0 = unlimited.
                TopBlocks = 4, // Maximum number of blocks to randomize between; 0 = unlimited.
                StopTrackingSpeed = 2000, // Do not track threats traveling faster than this speed; 0 = unlimited.
            },
            HardPoint = new HardPointDef
            {
                PartName = "Railgun Turret",
                DeviateShotAngle = 0f,
                AimingTolerance = 2f, // How many degrees off target a turret can fire at. 0 - 180 firing angle.
                AimLeadingPrediction = Advanced, // Level of turret aim prediction; Off, Basic, Accurate, Advanced
                Ai = new AiDef
                {
                    TrackTargets = true, // Whether this weapon tracks its own targets, or (for multiweapons) relies on the weapon with PrimaryTracking enabled for target designation. Turrets Need this set to True.
                    TurretAttached = true, // Whether this weapon is a turret and should have the UI and API options for such. Turrets Need this set to True.
                    TurretController = true, // Whether this weapon can physically control the turret's movement. Turrets Need this set to True.
                    PrimaryTracking = true, // For multiweapons: whether this weapon should designate targets for other weapons on the platform without their own tracking.
                },
                HardWare = new HardwareDef
                {
                    RotateRate = 0.003f, // Max traversal speed of azimuth subpart in radians per tick (0.1 is approximately 360 degrees per second).
                    ElevateRate = 0.003f, // Max traversal speed of elevation subpart in radians per tick.
                    MinAzimuth = -180,
                    MaxAzimuth = 180,
                    MinElevation = -20,
                    MaxElevation = 45,
                    InventorySize = 1.28f, // Inventory capacity in kL.
                    IdlePower = 0.02f, // Constant base power draw in MW.
                    Type = BlockWeapon, // What type of weapon this is; BlockWeapon, HandWeapon, Phantom 
                },
                Loading = new LoadingDef
                {
                    RateOfFire = 20,
                    BarrelsPerShot = 1,
                    TrajectilesPerBarrel = 1,
                    ReloadTime = 1080,
                    MagsToLoad = 1,
                    StayCharged = true, // Will start recharging whenever power cap is not full.
                },
                Audio = new HardPointAudioDef
                {
                    FiringSound = "WepRailgunLargeShot",
                    FiringSoundPerShot = true,
                    NoAmmoSound = "WepShipRailgunNoAmmo",
                    HardPointRotationSound = "WepTurretGatlingRotate",
                    FireSoundEndDelay = 120,
                },
                Graphics = new HardPointParticleDef
                {
                    Effect1 = new ParticleDef
                    {
                        Name = "Muzzle_Flash_RailgunTurret",
                        Offset = Vector(x: 0, y: 0, z: -9),
                        Extras = new ParticleOptionDef { Loop = false, Restart = false, MaxDistance = 4000, Scale = 1f },
                    },
                },
            },
            Ammos = new[] { LargeRailgunSabot },
        };
    }
}
