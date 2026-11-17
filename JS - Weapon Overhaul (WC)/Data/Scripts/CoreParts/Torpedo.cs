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
        WeaponDefinition TorpedoLauncher => new WeaponDefinition
        {
            Assignments = new ModelAssignmentsDef
            {
                MountPoints = new[]
                {
                    new MountPointDef { 
                        SubtypeId = "TorpedoLauncher", 
                        MuzzlePartId = "None", 
                        AzimuthPartId = "None", 
                        ElevationPartId = "None", 
                        DurabilityMod = 1f },
                },
                Muzzles = new[] 
                {
                     "muzzle_missile_001" 
                },
                Scope = "muzzle_missile_001", // Where line of sight checks are performed from. Must be clear of block collision.
            },
            Targeting = new TargetingDef
            {
                Threats = new[] {
                    Grids, Characters, Meteors, // Types of threat to engage: Grids, Projectiles, Characters, Meteors, Neutrals
                },
                SubSystems = new[] {
                    Thrust, Utility, Offense, Power, Production, Any, // Subsystem targeting priority: Offense, Utility, Power, Production, Thrust, Jumping, Steering, Any
                },
                TopTargets = 4, // Maximum number of targets to randomize between; 0 = unlimited.
                TopBlocks = 4, // Maximum number of blocks to randomize between; 0 = unlimited.
                StopTrackingSpeed = 1000, // Do not track threats traveling faster than this speed; 0 = unlimited.
            },
            HardPoint = new HardPointDef
            {
                PartName = "Torpedo Launcher",
                DeviateShotAngle = 0.1f,
                AimingTolerance = 2f,
                HardWare = new HardwareDef
                {
                    InventorySize = 1.14f,
                    IdlePower = 0.001f,
                },
                Loading = new LoadingDef
                {
                    RateOfFire = 40,
                    BarrelsPerShot = 1,
                    TrajectilesPerBarrel = 1,
                    ReloadTime = 1080, 
                    MagsToLoad = 1,
                },
                Audio = new HardPointAudioDef
                {
                    FiringSound = "Torpdeo",
                    FiringSoundPerShot = true,
                    NoAmmoSound = "WepShipGatlingNoAmmo",
                    FireSoundEndDelay = 60,
                },
            },
            Ammos = new[] { TorpedoAmmo },
        };
    }
}
