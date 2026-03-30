using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Weapons;
using Sandbox.ModAPI.Interfaces;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using Sandbox.Common.ObjectBuilders;

namespace JSCreativeBlockTools
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class JSCreativeBlockTools : MySessionComponentBase
	{
		public override void HandleInput()
		{
			base.HandleInput();
			if (MyAPIGateway.Input.IsNewKeyPressed(VRage.Input.MyKeys.T) && MyAPIGateway.Input.IsAnyCtrlKeyPressed() && MyAPIGateway.Session.IsServer)
			{
				CycleArmorTransparency();
			}
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			MyAPIGateway.Utilities.MessageEntered += Utilities_MessageEntered;
		}

		private void Utilities_MessageEntered(string messageText, ref bool sendToOthers)
		{
			if (messageText.StartsWith("/remove") && MyAPIGateway.Session.IsServer)
			{
				string[] splits = messageText.Split(' ');
				if (splits.Length != 2 || string.IsNullOrWhiteSpace(splits[1]))
				{
					MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "Usage: '/remove name' - will remove all blocks whos name contains 'name' if they wont break the grid by removing them");
					return;
				}
				RemoveBlocks(splits[1]);
				sendToOthers = false;
			}
			if (messageText.StartsWith("/hide") && MyAPIGateway.Session.IsServer)
			{
				string[] splits = messageText.Split(' ');
				if (splits.Length != 2 || string.IsNullOrWhiteSpace(splits[1]))
				{
					MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "Usage: '/hide name' - will hide all blocks whos name contains 'name'");
					return;
				}
				SetBlockTransparency(1.0f, splits[1]);
				sendToOthers = false;
			}
			if (messageText == "/unhide" && MyAPIGateway.Session.IsServer)
			{
				UnHideBlocks();
				sendToOthers = false;
			}
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			MyAPIGateway.Utilities.MessageEntered -= Utilities_MessageEntered;
		}

		public IMyCubeGrid GetTargetedGrid()
		{
			var MaxRaycastDistance = 10000;
			var player = MyAPIGateway.Session?.LocalHumanPlayer;
			if (player == null) return null;

			var cameraMatrix = MyAPIGateway.Session.Camera.WorldMatrix;
			var forwardVector = Vector3D.Normalize(cameraMatrix.Forward);
			var toVector = cameraMatrix.Translation + forwardVector * MaxRaycastDistance;

			IHitInfo hitInfo;
			if (!MyAPIGateway.Physics.CastRay(cameraMatrix.Translation, toVector, out hitInfo))
				return null;

			var entity = hitInfo?.HitEntity;
			if (entity?.Physics == null) return null;

			var grid = entity.GetTopMostParent() as IMyCubeGrid;
			return grid;
		}

		private static float armorTransparency = 0.0f;

        private void CycleArmorTransparency()
		{
			if (armorTransparency == 0.0f)
            {
				armorTransparency = .5f;
			}
			else if (armorTransparency == 0.5f)
			{
				armorTransparency = 1.0f;
			}
			else if (armorTransparency == 1.0f)
			{
				armorTransparency = 0.0f;
			}
			SetBlockTransparency(armorTransparency, "Light Armor");
			SetBlockTransparency(armorTransparency, "Heavy Armor");
			SetBlockTransparency(armorTransparency, "Armor Ramp");
			SetBlockTransparency(armorTransparency, "Armor Panel");
			SetBlockTransparency(armorTransparency, "Armor Corner");
			SetBlockTransparency(armorTransparency, "Armor InvCorner");
			SetBlockTransparency(armorTransparency, "Floor");
			SetBlockTransparency(armorTransparency, "Round Armor");
		}

		public void SetBlockTransparency(float transparency, string name)
		{
			IMyCubeGrid grid = GetTargetedGrid();
			if (grid == null) 
				return;

			List<IMySlimBlock> allBlocks = new List<IMySlimBlock>();

			grid.GetBlocks(allBlocks);
			int count = 0;
			foreach (IMySlimBlock block in allBlocks)
			{
				if (block.BlockDefinition.DisplayNameText.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					count++;
					block.Dithering = transparency;
				}
			}
			MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "Setting transparency for " + count + " blocks matching: " + name);
		}

		public void UnHideBlocks()
		{
			IMyCubeGrid grid = GetTargetedGrid();
			if (grid == null) 
				return;

			MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "Unhiding all blocks");
			List<IMySlimBlock> allBlocks = new List<IMySlimBlock>();

			grid.GetBlocks(allBlocks);
			armorTransparency = 0.0f;
			foreach (IMySlimBlock block in allBlocks)
			{
				block.Dithering = armorTransparency;
			}
		}

		private void RemoveBlocks(string name)
		{
			IMyCubeGrid grid = GetTargetedGrid();
			if (grid == null) 
				return;

			List<IMySlimBlock> allBlocks = new List<IMySlimBlock>();
    		List<IMySlimBlock> targetblocks = new List<IMySlimBlock>();

			grid.GetBlocks(allBlocks);

			foreach (IMySlimBlock block in allBlocks)
			{
				if (block.BlockDefinition.DisplayNameText.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					targetblocks.Add(block);
				}
			}
			MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "Found " + targetblocks.Count + " blocks matching: " + name);

			int count = 0;
	        while(true)
	        {
				List<IMySlimBlock> removable = new List<IMySlimBlock>();
	        	foreach (IMySlimBlock target in targetblocks)
				{
					if (!grid.WillRemoveBlockSplitGrid(target))
					{
						removable.Add(target);
						break;
					}
				}
				if (removable.Count == 0)
				{
					break;
				}
				else
				{
					foreach (IMySlimBlock remove in removable)
					{
						targetblocks.Remove(remove);
						grid.RemoveBlock(remove, true);
						count++;
					}
				}
	        }
			MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "Removed " + count + " blocks");
		}
	}
}