using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Weapons;
using Sandbox.ModAPI;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using IngameConveyorTurret = SpaceEngineers.Game.ModAPI.Ingame.IMyLargeConveyorTurretBase;
using IngameInventory = VRage.Game.ModAPI.Ingame.IMyInventory;
using IngameInventoryItem = VRage.Game.ModAPI.Ingame.MyInventoryItem;
using IngameItemType = VRage.Game.ModAPI.Ingame.MyItemType;

namespace JSWeaponAdditions
{
    /// <summary>
    /// Standalone capacitor charging and 1-slug ammo gating for RailgunTurret.
    /// - Draws 38 MW over ~59.21s (~625 kWh) per shot, matching the vanilla Large Railgun.
    /// - Keeps the turret inventory constraint locked so slugs cannot enter while recharging.
    /// - When fully charged, briefly unlocks the constraint to transfer exactly 1 LargeRailgunAmmo
    ///   from a connected conveyor inventory, then immediately re-locks.
    /// - Persists capacitor charge across world saves and displays live status in Detailed Info.
    /// </summary>
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public class RailgunCapacitorSession : MySessionComponentBase
    {
        private const string RAILGUN_TURRET_SUBTYPE = "RailgunTurret";
        private const string RAILGUN_AMMO_SUBTYPE = "LargeRailgunAmmo";

        // Vanilla Large Railgun specs (EntityComponents.sbc: Capacity 0.5 MWh, RechargeDraw 38 MW, ChargeMultiplier 0.8)
        private const float RECHARGE_DRAW_MW = 38f;
        private const float RECHARGE_TIME_SECONDS = 59.21f;
        private const float CAPACITOR_CAPACITY_MWH = (RECHARGE_DRAW_MW * RECHARGE_TIME_SECONDS) / 3600f;
        private const float MW_TO_MWH_PER_TICK = 1f / 216000f; // 1 / (3600s * 60 ticks/s)
        private const float CHARGE_COMPLETE_EPSILON = 0.0001f;
        private const float STORAGE_SCALE_MICRO_MWH = 1000000f;
        private const float STORAGE_SAVE_DELTA_RATIO = 0.01f;

        private const int LOAD_CHECK_INTERVAL_TICKS = 15;
        private const int INFO_REFRESH_INTERVAL_TICKS = 30;
        private const int FULL_SYNC_INTERVAL_TICKS = 300;
        private const ushort NETWORK_MESSAGE_ID = 49281;
        private const int NETWORK_PACKET_SIZE = 17;

        private static readonly Guid STORAGE_GUID = new Guid("8f2e4c91-3a7b-4d65-9e12-6c5b1a8d04e7");

        private enum LoaderStatus : byte
        {
            Off = 0,
            Charging = 1,
            NoPower = 2,
            ReadyToFire = 3,
            ChargedNoAmmo = 4,
            ChargedConveyorsOff = 5
        }

        private class TurretCapacitorState
        {
            public readonly IMyLargeTurretBase turret_;
            public readonly IngameConveyorTurret conveyorTurret_;
            public readonly IMyGunObject<MyGunBase> gunBase_;
            public readonly MyInventory turretInventory_;
            public readonly MyResourceSinkComponent sink_;
            public readonly float defaultPowerDrawMw_;
            public readonly Func<float> requiredPowerFunc_;

            public float targetPowerMw_ = -1f;
            public float capacitorMwh_;
            public bool isCharged_;
            public bool isUnlockedForManualLoad_;

            public float chargeRatio_;
            public float currentInputMw_;
            public LoaderStatus status_;
            public float lastSyncedChargeRatio_ = -1f;
            public LoaderStatus lastSyncedStatus_ = (LoaderStatus)255;
            public float lastSavedChargeRatio_ = -1f;

            public TurretCapacitorState(IMyLargeTurretBase turret)
            {
                turret_ = turret;
                conveyorTurret_ = turret as IngameConveyorTurret;
                gunBase_ = turret as IMyGunObject<MyGunBase>;
                turretInventory_ = turret.GetInventory(0) as MyInventory;
                sink_ = turret.Components.Get<MyResourceSinkComponent>();
                defaultPowerDrawMw_ = sink_ != null
                    ? sink_.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId)
                    : 0f;
                requiredPowerFunc_ = GetTargetPowerMw;
            }

            private float GetTargetPowerMw()
            {
                return targetPowerMw_ > 0f ? targetPowerMw_ : 0f;
            }
        }

        private readonly Dictionary<long, TurretCapacitorState> trackedTurrets_ = new Dictionary<long, TurretCapacitorState>();
        private readonly HashSet<IMyCubeGrid> trackedGrids_ = new HashSet<IMyCubeGrid>();
        private readonly HashSet<IMyCubeGrid> pendingPhysicsGrids_ = new HashSet<IMyCubeGrid>();
        private readonly List<IMyTerminalBlock> tempBlocks_ = new List<IMyTerminalBlock>();
        private readonly List<IngameInventoryItem> tempItems_ = new List<IngameInventoryItem>();
        private readonly List<long> cleanupIds_ = new List<long>();
        private readonly HashSet<IMyEntity> tempEntities_ = new HashSet<IMyEntity>();

        private MyDefinitionId ammoDefId_;
        private IngameItemType ammoItemType_;
        private MyInventoryConstraint lockedConstraint_;
        private MyInventoryConstraint unlockedConstraint_;

        private bool isServer_;
        private bool isDedicated_;
        private bool isMultiplayer_;
        private int tickCounter_;

        public override void LoadData()
        {
            isServer_ = MyAPIGateway.Multiplayer.IsServer;
            isDedicated_ = MyAPIGateway.Utilities.IsDedicated;
            isMultiplayer_ = MyAPIGateway.Multiplayer.MultiplayerActive;

            ammoDefId_ = new MyDefinitionId(typeof(MyObjectBuilder_AmmoMagazine), RAILGUN_AMMO_SUBTYPE);
            ammoItemType_ = IngameItemType.MakeAmmo(RAILGUN_AMMO_SUBTYPE);

            lockedConstraint_ = new MyInventoryConstraint("Capacitor Loader (auto-loads 1 slug from conveyors when charged)");
            lockedConstraint_.m_useDefaultIcon = false;
            lockedConstraint_.UpdateIcon();

            unlockedConstraint_ = new MyInventoryConstraint("Ammo:\n  - " + RAILGUN_AMMO_SUBTYPE);
            unlockedConstraint_.m_useDefaultIcon = false;
            unlockedConstraint_.Add(ammoDefId_);
            unlockedConstraint_.UpdateIcon();

            MyAPIGateway.Entities.OnEntityAdd += OnEntityAdded;

            if (isServer_)
            {
                MyAPIGateway.Missiles.OnMissileAdded += OnMissileAdded;
            }
            else if (isMultiplayer_)
            {
                MyAPIGateway.Multiplayer.RegisterSecureMessageHandler(NETWORK_MESSAGE_ID, OnNetworkMessageReceived);
            }
        }

        public override void BeforeStart()
        {
            tempEntities_.Clear();
            MyAPIGateway.Entities.GetEntities(tempEntities_, entity => entity is IMyCubeGrid);

            foreach (var entity in tempEntities_)
            {
                TrackGrid(entity as IMyCubeGrid);
            }

            tempEntities_.Clear();
        }

        public override void SaveData()
        {
            if (!isServer_)
            {
                return;
            }

            foreach (var state in trackedTurrets_.Values)
            {
                SaveChargeToStorage(state);
            }
        }

        protected override void UnloadData()
        {
            MyAPIGateway.Entities.OnEntityAdd -= OnEntityAdded;

            if (isServer_)
            {
                MyAPIGateway.Missiles.OnMissileAdded -= OnMissileAdded;
            }
            else if (isMultiplayer_)
            {
                MyAPIGateway.Multiplayer.UnregisterSecureMessageHandler(NETWORK_MESSAGE_ID, OnNetworkMessageReceived);
            }

            foreach (var grid in pendingPhysicsGrids_)
            {
                if (grid != null)
                {
                    grid.OnPhysicsChanged -= OnPendingGridPhysicsChanged;
                    grid.OnClose -= OnPendingGridClosed;
                }
            }
            pendingPhysicsGrids_.Clear();

            foreach (var grid in trackedGrids_)
            {
                if (grid != null)
                {
                    grid.OnBlockAdded -= OnGridBlockAdded;
                    grid.OnClose -= OnGridClosed;
                }
            }
            trackedGrids_.Clear();

            foreach (var state in trackedTurrets_.Values)
            {
                DetachTurret(state);
            }
            trackedTurrets_.Clear();

            tempBlocks_.Clear();
            tempItems_.Clear();
            cleanupIds_.Clear();
            tempEntities_.Clear();
        }

        private void OnEntityAdded(IMyEntity entity)
        {
            var grid = entity as IMyCubeGrid;
            if (grid != null)
            {
                TrackGrid(grid);
            }
        }

        private void TrackGrid(IMyCubeGrid grid)
        {
            if (grid == null || grid.Closed || grid.MarkedForClose)
            {
                return;
            }

            if (grid.Physics == null)
            {
                if (pendingPhysicsGrids_.Add(grid))
                {
                    grid.OnPhysicsChanged += OnPendingGridPhysicsChanged;
                    grid.OnClose += OnPendingGridClosed;
                }
                return;
            }

            if (!trackedGrids_.Add(grid))
            {
                return;
            }

            grid.OnBlockAdded += OnGridBlockAdded;
            grid.OnClose += OnGridClosed;

            grid.GetBlocks(null, slim =>
            {
                TryAttachSlimBlock(slim);
                return false;
            });
        }

        private void OnPendingGridPhysicsChanged(IMyEntity entity)
        {
            var grid = entity as IMyCubeGrid;
            if (grid == null || grid.Physics == null)
            {
                return;
            }

            grid.OnPhysicsChanged -= OnPendingGridPhysicsChanged;
            grid.OnClose -= OnPendingGridClosed;
            pendingPhysicsGrids_.Remove(grid);

            TrackGrid(grid);
        }

        private void OnPendingGridClosed(IMyEntity entity)
        {
            var grid = entity as IMyCubeGrid;
            if (grid == null)
            {
                return;
            }

            grid.OnPhysicsChanged -= OnPendingGridPhysicsChanged;
            grid.OnClose -= OnPendingGridClosed;
            pendingPhysicsGrids_.Remove(grid);
        }

        private void OnGridBlockAdded(IMySlimBlock slim)
        {
            TryAttachSlimBlock(slim);
        }

        private void OnGridClosed(IMyEntity entity)
        {
            var grid = entity as IMyCubeGrid;
            if (grid == null)
            {
                return;
            }

            grid.OnBlockAdded -= OnGridBlockAdded;
            grid.OnClose -= OnGridClosed;
            trackedGrids_.Remove(grid);
        }

        private void TryAttachSlimBlock(IMySlimBlock slim)
        {
            if (slim == null || slim.FatBlock == null)
            {
                return;
            }

            var turret = slim.FatBlock as IMyLargeTurretBase;
            if (turret == null || turret.BlockDefinition.SubtypeName != RAILGUN_TURRET_SUBTYPE)
            {
                return;
            }

            AttachTurret(turret);
        }

        private void AttachTurret(IMyLargeTurretBase turret)
        {
            if (turret == null || turret.Closed || turret.MarkedForClose)
            {
                return;
            }

            if (trackedTurrets_.ContainsKey(turret.EntityId))
            {
                return;
            }

            var state = new TurretCapacitorState(turret);

            if (isServer_)
            {
                LoadChargeFromStorage(state);
                LockInventory(state);
                UpdatePowerSink(state);
                turret.EnabledChanged += OnTurretEnabledChanged;
            }

            turret.OnClose += OnTurretClosed;

            if (!isDedicated_)
            {
                turret.AppendingCustomInfo += OnAppendingCustomInfo;
            }

            trackedTurrets_.Add(turret.EntityId, state);

            if (isServer_)
            {
                UpdateStatusAndUi(state, true, true, true);
            }
            else if (!isDedicated_)
            {
                turret.RefreshCustomInfo();
            }
        }

        private void DetachTurret(TurretCapacitorState state)
        {
            if (state == null || state.turret_ == null)
            {
                return;
            }

            if (isServer_)
            {
                state.turret_.EnabledChanged -= OnTurretEnabledChanged;
            }

            state.turret_.OnClose -= OnTurretClosed;

            if (!isDedicated_)
            {
                state.turret_.AppendingCustomInfo -= OnAppendingCustomInfo;
            }
        }

        private void OnTurretEnabledChanged(IMyTerminalBlock block)
        {
            TurretCapacitorState state;
            if (block != null && trackedTurrets_.TryGetValue(block.EntityId, out state))
            {
                UpdatePowerSink(state);
                UpdateStatusAndUi(state, false, true, false);
            }
        }

        private void OnTurretClosed(IMyEntity entity)
        {
            if (entity != null)
            {
                RemoveTurret(entity.EntityId);
            }
        }

        private void RemoveTurret(long entityId)
        {
            TurretCapacitorState state;
            if (trackedTurrets_.TryGetValue(entityId, out state))
            {
                DetachTurret(state);
                trackedTurrets_.Remove(entityId);
            }
        }

        private void OnMissileAdded(IMyMissile missile)
        {
            if (missile == null)
            {
                return;
            }

            TurretCapacitorState state;
            if (!trackedTurrets_.TryGetValue(missile.LauncherId, out state))
            {
                return;
            }

            state.capacitorMwh_ = 0f;
            state.isCharged_ = false;

            SaveChargeToStorage(state);
            LockInventory(state);
            UpdatePowerSink(state);
            UpdateStatusAndUi(state, false, true, true);
        }

        public override void UpdateBeforeSimulation()
        {
            if (!isServer_ || trackedTurrets_.Count == 0)
            {
                return;
            }

            tickCounter_++;
            bool checkLoad = (tickCounter_ % LOAD_CHECK_INTERVAL_TICKS) == 0;
            bool refreshInfo = (tickCounter_ % INFO_REFRESH_INTERVAL_TICKS) == 0;
            bool forceSync = (tickCounter_ % FULL_SYNC_INTERVAL_TICKS) == 0;

            cleanupIds_.Clear();

            foreach (var kvp in trackedTurrets_)
            {
                var state = kvp.Value;
                if (state.turret_ == null || state.turret_.Closed || state.turret_.MarkedForClose)
                {
                    cleanupIds_.Add(kvp.Key);
                    continue;
                }

                StepCapacitorCharge(state);

                if (checkLoad || refreshInfo || forceSync)
                {
                    UpdateStatusAndUi(state, checkLoad, refreshInfo || forceSync, forceSync);
                }
            }

            for (int i = 0; i < cleanupIds_.Count; i++)
            {
                RemoveTurret(cleanupIds_[i]);
            }
        }

        private void StepCapacitorCharge(TurretCapacitorState state)
        {
            bool active = state.turret_.Enabled && state.turret_.IsFunctional;

            UpdatePowerSink(state);

            // Re-lock immediately if a player manually loaded a slug while conveyors were off.
            if (state.isUnlockedForManualLoad_ && (!active || !state.isCharged_ || HasLoadedRound(state)))
            {
                LockInventory(state);
                UpdateStatusAndUi(state, false, true, true);
            }

            if (!active || state.isCharged_ || state.sink_ == null)
            {
                return;
            }

            float inputMw = state.sink_.CurrentInputByType(MyResourceDistributorComponent.ElectricityId);
            if (inputMw <= 0f)
            {
                return;
            }

            state.capacitorMwh_ = Math.Min(CAPACITOR_CAPACITY_MWH, state.capacitorMwh_ + (inputMw * MW_TO_MWH_PER_TICK));

            if (state.capacitorMwh_ >= (CAPACITOR_CAPACITY_MWH - CHARGE_COMPLETE_EPSILON))
            {
                state.capacitorMwh_ = CAPACITOR_CAPACITY_MWH;
                state.isCharged_ = true;
                SaveChargeToStorage(state);
                UpdatePowerSink(state);
                UpdateStatusAndUi(state, true, true, true);
            }
        }

        private void UpdatePowerSink(TurretCapacitorState state)
        {
            if (state.sink_ == null)
            {
                return;
            }

            var electricityId = MyResourceDistributorComponent.ElectricityId;
            float desiredPowerMw;

            if (!state.turret_.Enabled || !state.turret_.IsFunctional)
            {
                desiredPowerMw = 0f;
            }
            else if (!state.isCharged_)
            {
                desiredPowerMw = RECHARGE_DRAW_MW;
            }
            else
            {
                desiredPowerMw = state.defaultPowerDrawMw_;
            }

            if (Math.Abs(desiredPowerMw - state.targetPowerMw_) <= 0.00001f)
            {
                return;
            }

            state.targetPowerMw_ = desiredPowerMw;

            if (!HasAcceptedResource(state.sink_, electricityId))
            {
                var info = new MyResourceSinkInfo
                {
                    ResourceTypeId = electricityId,
                    MaxRequiredInput = desiredPowerMw,
                    RequiredInputFunc = state.requiredPowerFunc_
                };
                state.sink_.AddType(ref info);
            }

            state.sink_.SetMaxRequiredInputByType(electricityId, desiredPowerMw);
            state.sink_.SetRequiredInputFuncByType(electricityId, state.requiredPowerFunc_);
            state.sink_.Update();
        }

        private static bool HasAcceptedResource(MyResourceSinkComponent sink, MyDefinitionId resourceId)
        {
            var resources = sink.AcceptedResources;
            for (int i = 0; i < resources.Count; i++)
            {
                if (resources[i] == resourceId)
                {
                    return true;
                }
            }

            return false;
        }

        private void UpdateStatusAndUi(TurretCapacitorState state, bool checkLoad, bool refreshInfo, bool forceSync)
        {
            float chargeRatio = state.isCharged_
                ? 1f
                : Math.Max(0f, Math.Min(1f, state.capacitorMwh_ / CAPACITOR_CAPACITY_MWH));

            float currentInputMw = state.sink_ != null
                ? state.sink_.CurrentInputByType(MyResourceDistributorComponent.ElectricityId)
                : 0f;

            bool active = state.turret_.Enabled && state.turret_.IsFunctional;
            bool hasRound = HasLoadedRound(state);
            bool useConveyorSystem = state.conveyorTurret_ == null || state.conveyorTurret_.UseConveyorSystem;

            // Allow manual hand-loading only when charged, empty, active, and Use Conveyor System is OFF.
            bool allowManualLoad = active && state.isCharged_ && !hasRound && !useConveyorSystem;
            if (allowManualLoad)
            {
                UnlockInventoryForManualLoad(state);
            }
            else if (state.turretInventory_ != null && (state.isUnlockedForManualLoad_ || state.turretInventory_.Constraint != lockedConstraint_))
            {
                LockInventory(state);
            }

            if (checkLoad && state.isCharged_ && !hasRound && active && useConveyorSystem)
            {
                if (TryPullSingleSlug(state))
                {
                    hasRound = true;
                    refreshInfo = true;
                }
            }

            LoaderStatus status;
            if (!active)
            {
                status = LoaderStatus.Off;
            }
            else if (hasRound)
            {
                status = LoaderStatus.ReadyToFire;
            }
            else if (state.isCharged_)
            {
                status = useConveyorSystem ? LoaderStatus.ChargedNoAmmo : LoaderStatus.ChargedConveyorsOff;
            }
            else if (currentInputMw <= 0.01f)
            {
                status = LoaderStatus.NoPower;
            }
            else
            {
                status = LoaderStatus.Charging;
            }

            state.chargeRatio_ = chargeRatio;
            state.currentInputMw_ = currentInputMw;
            state.status_ = status;

            // Keep ModStorageComponent up to date so mid-session blueprints capture current charge.
            if (Math.Abs(chargeRatio - state.lastSavedChargeRatio_) >= STORAGE_SAVE_DELTA_RATIO)
            {
                SaveChargeToStorage(state);
            }

            if (refreshInfo && !isDedicated_)
            {
                state.turret_.RefreshCustomInfo();
            }

            if (isMultiplayer_)
            {
                bool changedEnough = forceSync
                    || status != state.lastSyncedStatus_
                    || Math.Abs(chargeRatio - state.lastSyncedChargeRatio_) >= 0.005f;

                if (refreshInfo && changedEnough)
                {
                    SendStateToClients(state);
                    state.lastSyncedChargeRatio_ = chargeRatio;
                    state.lastSyncedStatus_ = status;
                }
            }
        }

        private bool HasLoadedRound(TurretCapacitorState state)
        {
            if (state.gunBase_ != null && state.gunBase_.GunBase != null && state.gunBase_.GunBase.CurrentAmmo > 0)
            {
                return true;
            }

            var ingameInv = state.turretInventory_ as IngameInventory;
            if (ingameInv != null && ingameInv.GetItemAmount(ammoItemType_) > MyFixedPoint.Zero)
            {
                return true;
            }

            return false;
        }

        private void LockInventory(TurretCapacitorState state)
        {
            state.isUnlockedForManualLoad_ = false;

            if (state.turretInventory_ == null)
            {
                return;
            }

            state.turretInventory_.Constraint = lockedConstraint_;
            state.turretInventory_.Refresh();
        }

        private void UnlockInventoryForManualLoad(TurretCapacitorState state)
        {
            if (state.turretInventory_ == null)
            {
                return;
            }

            if (!state.isUnlockedForManualLoad_ || state.turretInventory_.Constraint != unlockedConstraint_)
            {
                state.isUnlockedForManualLoad_ = true;
                state.turretInventory_.Constraint = unlockedConstraint_;
                state.turretInventory_.Refresh();
            }
        }

        private bool TryPullSingleSlug(TurretCapacitorState state)
        {
            var turret = state.turret_;
            var turretMyInv = state.turretInventory_;
            var turretIngameInv = turretMyInv as IngameInventory;

            if (turret == null || turret.CubeGrid == null || turretMyInv == null || turretIngameInv == null)
            {
                return false;
            }

            var terminalSystem = MyAPIGateway.TerminalActionsHelper.GetTerminalSystemForGrid(turret.CubeGrid);
            if (terminalSystem == null)
            {
                return false;
            }

            tempBlocks_.Clear();
            terminalSystem.GetBlocks(tempBlocks_);

            // First pass: prefer cargo containers; second pass: any other connected non-weapon inventory (e.g., assemblers/connectors).
            for (int pass = 0; pass < 2; pass++)
            {
                for (int b = 0; b < tempBlocks_.Count; b++)
                {
                    var block = tempBlocks_[b];
                    if (block == null || block.EntityId == turret.EntityId || block.Closed || !block.HasInventory)
                    {
                        continue;
                    }

                    // Never steal ammo from other weapons/turrets.
                    if (block is IMyUserControllableGun)
                    {
                        continue;
                    }

                    bool isCargo = block is IMyCargoContainer;
                    if ((pass == 0 && !isCargo) || (pass == 1 && isCargo))
                    {
                        continue;
                    }

                    // For assemblers, only pull from output inventory (index 1).
                    int startIndex = (block is IMyAssembler && block.InventoryCount > 1) ? 1 : 0;

                    for (int invIdx = startIndex; invIdx < block.InventoryCount; invIdx++)
                    {
                        var sourceIngameInv = block.GetInventory(invIdx) as IngameInventory;
                        if (sourceIngameInv == null)
                        {
                            continue;
                        }

                        if (sourceIngameInv.GetItemAmount(ammoItemType_) < (MyFixedPoint)1)
                        {
                            continue;
                        }

                        if (!sourceIngameInv.IsConnectedTo(turretIngameInv))
                        {
                            continue;
                        }

                        tempItems_.Clear();
                        sourceIngameInv.GetItems(tempItems_);

                        for (int itemIdx = 0; itemIdx < tempItems_.Count; itemIdx++)
                        {
                            if (tempItems_[itemIdx].Type != ammoItemType_ || tempItems_[itemIdx].Amount < (MyFixedPoint)1)
                            {
                                continue;
                            }

                            bool transferred = false;
                            try
                            {
                                turretMyInv.Constraint = unlockedConstraint_;
                                transferred = sourceIngameInv.TransferItemTo(turretIngameInv, itemIdx, null, true, (MyFixedPoint)1);
                            }
                            finally
                            {
                                turretMyInv.Constraint = lockedConstraint_;
                                turretMyInv.Refresh();
                            }

                            if (transferred)
                            {
                                tempItems_.Clear();
                                tempBlocks_.Clear();
                                return true;
                            }
                        }
                    }
                }
            }

            tempItems_.Clear();
            tempBlocks_.Clear();
            return false;
        }

        private void LoadChargeFromStorage(TurretCapacitorState state)
        {
            if (state.turret_.Storage == null)
            {
                return;
            }

            string raw;
            int microMwh;
            if (state.turret_.Storage.TryGetValue(STORAGE_GUID, out raw) && int.TryParse(raw, out microMwh))
            {
                float loadedMwh = microMwh / STORAGE_SCALE_MICRO_MWH;
                state.capacitorMwh_ = Math.Max(0f, Math.Min(CAPACITOR_CAPACITY_MWH, loadedMwh));
                state.isCharged_ = state.capacitorMwh_ >= (CAPACITOR_CAPACITY_MWH - CHARGE_COMPLETE_EPSILON);
                if (state.isCharged_)
                {
                    state.capacitorMwh_ = CAPACITOR_CAPACITY_MWH;
                }
                state.lastSavedChargeRatio_ = state.isCharged_ ? 1f : (state.capacitorMwh_ / CAPACITOR_CAPACITY_MWH);
            }
        }

        private void SaveChargeToStorage(TurretCapacitorState state)
        {
            if (state.turret_ == null || state.turret_.Closed || state.turret_.MarkedForClose)
            {
                return;
            }

            if (state.turret_.Storage == null)
            {
                state.turret_.Storage = new MyModStorageComponent();
            }

            int microMwh = (int)Math.Round(state.capacitorMwh_ * STORAGE_SCALE_MICRO_MWH);
            state.turret_.Storage.SetValue(STORAGE_GUID, microMwh.ToString());
            state.lastSavedChargeRatio_ = state.isCharged_ ? 1f : (state.capacitorMwh_ / CAPACITOR_CAPACITY_MWH);
        }

        private void OnAppendingCustomInfo(IMyTerminalBlock block, StringBuilder sb)
        {
            TurretCapacitorState state;
            if (block == null || !trackedTurrets_.TryGetValue(block.EntityId, out state))
            {
                return;
            }

            float percent = state.chargeRatio_ * 100f;

            sb.AppendLine();
            sb.AppendFormat("Capacitor Charge: {0:0.0}%\n", percent);

            switch (state.status_)
            {
                case LoaderStatus.Off:
                    sb.AppendLine("Railgun Status: Offline");
                    break;
                case LoaderStatus.Charging:
                    float remainingMwh = Math.Max(0f, (1f - state.chargeRatio_) * CAPACITOR_CAPACITY_MWH);
                    float etaSeconds = state.currentInputMw_ > 0.01f ? (remainingMwh / state.currentInputMw_) * 3600f : 0f;
                    sb.AppendFormat("Charge Input: {0:0.0} / {1:0.0} MW\n", state.currentInputMw_, RECHARGE_DRAW_MW);
                    sb.AppendFormat("Railgun Status: Charging ({0:0}s remaining)\n", etaSeconds);
                    break;
                case LoaderStatus.NoPower:
                    sb.AppendFormat("Charge Input: 0.0 / {0:0.0} MW\n", RECHARGE_DRAW_MW);
                    sb.AppendLine("Railgun Status: Insufficient Power");
                    break;
                case LoaderStatus.ReadyToFire:
                    sb.AppendLine("Railgun Status: Ready to Fire");
                    break;
                case LoaderStatus.ChargedNoAmmo:
                    sb.AppendLine("Railgun Status: Charged (No Ammo on Conveyors)");
                    break;
                case LoaderStatus.ChargedConveyorsOff:
                    sb.AppendLine("Railgun Status: Charged (Conveyors Off - Ready for Manual Load)");
                    break;
            }
        }

        private void SendStateToClients(TurretCapacitorState state)
        {
            var data = new byte[NETWORK_PACKET_SIZE];
            Array.Copy(BitConverter.GetBytes(state.turret_.EntityId), 0, data, 0, 8);
            Array.Copy(BitConverter.GetBytes(state.chargeRatio_), 0, data, 8, 4);
            Array.Copy(BitConverter.GetBytes(state.currentInputMw_), 0, data, 12, 4);
            data[16] = (byte)state.status_;

            MyAPIGateway.Multiplayer.SendMessageToOthers(NETWORK_MESSAGE_ID, data);
        }

        private void OnNetworkMessageReceived(ushort messageId, byte[] data, ulong senderId, bool isFromServer)
        {
            if (messageId != NETWORK_MESSAGE_ID || !isFromServer || data == null || data.Length != NETWORK_PACKET_SIZE)
            {
                return;
            }

            long entityId = BitConverter.ToInt64(data, 0);
            float chargeRatio = BitConverter.ToSingle(data, 8);
            float currentInputMw = BitConverter.ToSingle(data, 12);
            var status = (LoaderStatus)data[16];

            TurretCapacitorState state;
            if (!trackedTurrets_.TryGetValue(entityId, out state))
            {
                IMyEntity entity;
                if (MyAPIGateway.Entities.TryGetEntityById(entityId, out entity))
                {
                    var turret = entity as IMyLargeTurretBase;
                    if (turret != null && turret.BlockDefinition.SubtypeName == RAILGUN_TURRET_SUBTYPE)
                    {
                        AttachTurret(turret);
                    }
                }

                if (!trackedTurrets_.TryGetValue(entityId, out state))
                {
                    return;
                }
            }

            state.chargeRatio_ = chargeRatio;
            state.currentInputMw_ = currentInputMw;
            state.status_ = status;

            if (!isDedicated_ && state.turret_ != null && !state.turret_.Closed)
            {
                state.turret_.RefreshCustomInfo();
            }
        }
    }
}
