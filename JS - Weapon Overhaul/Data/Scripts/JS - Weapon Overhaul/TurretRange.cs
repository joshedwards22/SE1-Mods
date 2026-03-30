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
		public override void BeforeStart()
		{
			base.BeforeStart();
			if (!MyAPIGateway.Session.IsServer)
				return;

			MyVisualScriptLogicProvider.BlockBuilt += BlockBuilt;
		}

		private void BlockBuilt(string typeId, string subtypeId, string gridName, long blockId)		//for new blocks
		{
			var block = MyEntities.GetEntityById(blockId) as MyCubeBlock;
			if (block != null && block is IMyLargeTurretBase)
			{
				var turret = block as IMyLargeTurretBase;
				turret.Range = 10000000000000000000;
			}
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			MyVisualScriptLogicProvider.BlockBuilt -= BlockBuilt;
		}
	}
}