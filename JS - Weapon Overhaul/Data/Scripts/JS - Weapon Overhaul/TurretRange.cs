using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace JSWeaponOverhaul
{
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 0)]
    public class JSTurretRange : MySessionComponentBase
    {
        // Max float value that won't overflow (~3.4 * 10^38), but practically
        // we use a large but sane number. This effectively sets range to 200km.
        private const float MAX_TURRET_RANGE = 20000f;

        public override void BeforeStart()
        {
            base.BeforeStart();
            if (!MyAPIGateway.Session.IsServer)
                return;

            MyVisualScriptLogicProvider.BlockBuilt += BlockBuilt;
        }

        private void BlockBuilt(string typeId, string subtypeId, string gridName, long blockId)
        {
            var block = MyEntities.GetEntityById(blockId) as MyCubeBlock;
            if (block != null && block is IMyLargeTurretBase)
            {
                var turret = block as IMyLargeTurretBase;
                turret.Range = MAX_TURRET_RANGE;
            }
        }

        protected override void UnloadData()
        {
            base.UnloadData();

            if (MyAPIGateway.Session.IsServer)
                MyVisualScriptLogicProvider.BlockBuilt -= BlockBuilt;
        }
    }
}