using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;
using Sandbox.Common.ObjectBuilders;

namespace JSCreativeBlockTools
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class JSCreativeBlockTools : MySessionComponentBase
	{
		private const int MAX_RAYCAST_DISTANCE = 10000;

		// Armor transparency state
		private static float armorTransparency_ = 0.0f;

		// Slice mode state
		private bool sliceActive_ = false;
		private IMyCubeGrid sliceGrid_ = null;
		private Vector3D sliceNormalWorld_ = Vector3D.Zero;
		private int sliceDepth_ = 0;
		private List<List<IMySlimBlock>> sliceLayers_ = null;

		// Tracks functional blocks we disabled so we only re-enable ones that were originally on
		private HashSet<long> disabledBlocks_ = new HashSet<long>();

		public override void HandleInput()
		{
			base.HandleInput();
			if (!MyAPIGateway.Utilities.IsDedicated && MyAPIGateway.Session?.LocalHumanPlayer != null)
			{
				// Ctrl+J: toggle slice mode
				if (MyAPIGateway.Input.IsNewKeyPressed(VRage.Input.MyKeys.J) && MyAPIGateway.Input.IsAnyCtrlKeyPressed())
				{
					if (sliceActive_)
						ExitSliceMode();
					else
						EnterSliceMode();
					return;
				}

				// Numpad +/- while in slice mode
				if (sliceActive_)
				{
					// Validate grid still exists
					if (sliceGrid_ == null || sliceGrid_.Closed)
					{
						ExitSliceMode();
						return;
					}

					if (MyAPIGateway.Input.IsNewKeyPressed(VRage.Input.MyKeys.Add))
					{
						SliceUp();
						return;
					}
					else if (MyAPIGateway.Input.IsNewKeyPressed(VRage.Input.MyKeys.Subtract))
					{
						SliceDown();
						return;
					}
				}

				// Existing keybinds
				if (MyAPIGateway.Input.IsNewKeyPressed(VRage.Input.MyKeys.T) && MyAPIGateway.Input.IsAnyCtrlKeyPressed())
				{
					CycleArmorTransparency();
				}
				else if (MyAPIGateway.Input.IsNewKeyPressed(VRage.Input.MyKeys.G) && MyAPIGateway.Input.IsAnyCtrlKeyPressed())
				{
					PickBlock();
				}
			}
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			MyAPIGateway.Utilities.MessageEntered += Utilities_MessageEntered;
		}

		private void Utilities_MessageEntered(string messageText, ref bool sendToOthers)
		{
			if (MyAPIGateway.Utilities.IsDedicated || MyAPIGateway.Session?.LocalHumanPlayer == null) return;

			string[] splits = messageText.Split(' ');
			string command = splits[0].ToLower();

			if (command == "/remove")
			{
				if (splits.Length != 2 || string.IsNullOrWhiteSpace(splits[1]))
				{
					MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "Usage: '/remove name' - will remove all blocks whos name contains 'name' if they wont break the grid by removing them");
					sendToOthers = false;
					return;
				}
				RemoveBlocks(splits[1]);
				sendToOthers = false;
			}
			else if (command == "/hide")
			{
				if (splits.Length != 2 || string.IsNullOrWhiteSpace(splits[1]))
				{
					MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "Usage: '/hide name' - will hide all blocks whos name contains 'name'");
					sendToOthers = false;
					return;
				}
				SetBlockTransparency(1.0f, splits[1]);
				sendToOthers = false;
			}
			else if (command == "/unhide")
			{
				UnHideBlocks();
				sendToOthers = false;
			}
		}

		protected override void UnloadData()
		{
			if (sliceActive_)
				ExitSliceMode();
			base.UnloadData();
			MyAPIGateway.Utilities.MessageEntered -= Utilities_MessageEntered;
		}

		public IMyCubeGrid GetTargetedGrid()
		{
			var player = MyAPIGateway.Session?.LocalHumanPlayer;
			if (player == null) return null;

			var cameraMatrix = MyAPIGateway.Session.Camera.WorldMatrix;
			var forwardVector = Vector3D.Normalize(cameraMatrix.Forward);
			var toVector = cameraMatrix.Translation + forwardVector * MAX_RAYCAST_DISTANCE;

			IHitInfo hitInfo;
			if (!MyAPIGateway.Physics.CastRay(cameraMatrix.Translation, toVector, out hitInfo))
				return null;

			var entity = hitInfo?.HitEntity;
			if (entity?.Physics == null) return null;

			return entity.GetTopMostParent() as IMyCubeGrid;
		}

		public IMySlimBlock GetTargetedBlock()
		{
			IMyCubeGrid grid = GetTargetedGrid();
			if (grid == null) return null;

			var cameraMatrix = MyAPIGateway.Session.Camera.WorldMatrix;
			var forwardVector = Vector3D.Normalize(cameraMatrix.Forward);
			var toVector = cameraMatrix.Translation + forwardVector * MAX_RAYCAST_DISTANCE;

			Vector3I? blockPos = grid.RayCastBlocks(cameraMatrix.Translation, toVector);
			if (!blockPos.HasValue) return null;

			return grid.GetCubeBlock(blockPos.Value);
		}

		private void PickBlock()
		{
			IMySlimBlock block = GetTargetedBlock();
			if (block == null)
			{
				MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "No block found");
				return;
			}

			MyDefinitionId defId = block.BlockDefinition.Id;

			// Assign to toolbar slot 9 (index 8) and activate build mode
			long playerId = MyAPIGateway.Session.Player.IdentityId;
			MyVisualScriptLogicProvider.SetToolbarSlotToItemLocal(8, defId, playerId);

			// Switch to CubePlacer (enters build mode like pressing a number key)
			var ctrl = MyAPIGateway.Session.ControlledObject as Sandbox.Game.Entities.IMyControllableEntity;
			if (ctrl != null)
			{
				ctrl.SwitchToWeapon(new MyDefinitionId(typeof(MyObjectBuilder_CubePlacer)));
				MyCubeBuilder.Static.Activate(defId);
			}
			MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "Picked: " + block.BlockDefinition.DisplayNameText);
		}

		private void CycleArmorTransparency()
		{
			if (armorTransparency_ == 0.0f)
			{
				armorTransparency_ = 0.5f;
			}
			else if (armorTransparency_ == 0.5f)
			{
				armorTransparency_ = 1.0f;
			}
			else
			{
				armorTransparency_ = 0.0f;
			}
			SetBlockTransparency(armorTransparency_, "Armor");
			SetBlockTransparency(armorTransparency_, "Floor");
			SetBlockTransparency(armorTransparency_, "Structural");
			SetBlockTransparency(armorTransparency_, "Beam");
			SetBlockTransparency(armorTransparency_, "Rounded");
			SetBlockTransparency(armorTransparency_, "Blast");
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
					SetBlockDithering(block, transparency);
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
			armorTransparency_ = 0.0f;
			foreach (IMySlimBlock block in allBlocks)
			{
				SetBlockDithering(block, 0.0f);
			}
			disabledBlocks_.Clear();
		}

		private void RemoveBlocks(string name)
		{
			IMyCubeGrid grid = GetTargetedGrid();
			if (grid == null) 
				return;

			List<IMySlimBlock> allBlocks = new List<IMySlimBlock>();
    		List<IMySlimBlock> targetBlocks = new List<IMySlimBlock>();

			grid.GetBlocks(allBlocks);

			foreach (IMySlimBlock block in allBlocks)
			{
				if (block.BlockDefinition.DisplayNameText.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					targetBlocks.Add(block);
				}
			}
			MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "Found " + targetBlocks.Count + " blocks matching: " + name);

			int count = 0;
	        while(true)
	        {
				List<IMySlimBlock> removable = new List<IMySlimBlock>();
	        	foreach (IMySlimBlock target in targetBlocks)
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
						targetBlocks.Remove(remove);
						grid.RemoveBlock(remove, true);
						count++;
					}
				}
	        }
			MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "Removed " + count + " blocks");
		}

		// ===== Slice Mode =====

		private void EnterSliceMode()
		{
			IMyCubeGrid grid = GetTargetedGrid();
			if (grid == null)
			{
				MyAPIGateway.Utilities.ShowNotification("Slice Mode: No grid targeted", 2000, "Red");
				return;
			}

			sliceGrid_ = grid;
			sliceDepth_ = 0;

			// Determine slice axis: snap camera forward to nearest grid axis
			var cameraMatrix = MyAPIGateway.Session.Camera.WorldMatrix;
			var cameraForward = Vector3D.Normalize(cameraMatrix.Forward);
			sliceNormalWorld_ = GetNearestGridAxis(grid, cameraForward);

			// Build the layer data
			BuildSliceLayers();

			if (sliceLayers_ == null || sliceLayers_.Count == 0)
			{
				MyAPIGateway.Utilities.ShowNotification("Slice Mode: No blocks found", 2000, "Red");
				sliceGrid_ = null;
				return;
			}

			sliceActive_ = true;
			MyAPIGateway.Utilities.ShowNotification("Slice Mode: ON (Layer 0/" + sliceLayers_.Count + ")", 2000, "White");
		}

		private void ExitSliceMode()
		{
			// Restore all blocks to visible across all connected grids
			if (sliceGrid_ != null && !sliceGrid_.Closed)
			{
				var grids = new List<IMyCubeGrid>();
				MyAPIGateway.GridGroups.GetGroup(sliceGrid_, GridLinkTypeEnum.Mechanical, grids);

				foreach (var grid in grids)
				{
					var blocks = new List<IMySlimBlock>();
					grid.GetBlocks(blocks);
					foreach (var block in blocks)
					{
						SetBlockDithering(block, 0.0f);
					}
				}
			}

			disabledBlocks_.Clear();

			sliceActive_ = false;
			sliceGrid_ = null;
			sliceNormalWorld_ = Vector3D.Zero;
			sliceDepth_ = 0;
			sliceLayers_ = null;

			MyAPIGateway.Utilities.ShowNotification("Slice Mode: OFF", 2000, "White");
		}

		private void SliceUp()
		{
			if (sliceLayers_ == null || sliceDepth_ >= sliceLayers_.Count)
				return;

			// Make the next layer fully transparent
			var layer = sliceLayers_[sliceDepth_];
			foreach (var block in layer)
			{
				SetBlockDithering(block, 1.0f);
			}

			sliceDepth_++;
			MyAPIGateway.Utilities.ShowNotification("Slice Mode: Layer " + sliceDepth_ + "/" + sliceLayers_.Count, 1500, "White");
		}

		private void SliceDown()
		{
			if (sliceLayers_ == null || sliceDepth_ <= 0)
				return;

			sliceDepth_--;

			// Restore the layer that was just un-sliced
			var layer = sliceLayers_[sliceDepth_];
			foreach (var block in layer)
			{
				SetBlockDithering(block, 0.0f);
			}

			MyAPIGateway.Utilities.ShowNotification("Slice Mode: Layer " + sliceDepth_ + "/" + sliceLayers_.Count, 1500, "White");
		}

		private Vector3D GetNearestGridAxis(IMyCubeGrid grid, Vector3D cameraForward)
		{
			MatrixD gridMatrix = grid.WorldMatrix;
			Vector3D[] axes = new Vector3D[]
			{
				 gridMatrix.Right,     // +X
				-gridMatrix.Right,     // -X
				 gridMatrix.Up,        // +Y
				-gridMatrix.Up,        // -Y
				 gridMatrix.Forward,   // -Z (SE convention)
				-gridMatrix.Forward    // +Z
			};

			double bestDot = double.MinValue;
			Vector3D bestAxis = axes[0];

			for (int i = 0; i < axes.Length; i++)
			{
				double dot = Vector3D.Dot(cameraForward, axes[i]);
				if (dot > bestDot)
				{
					bestDot = dot;
					bestAxis = axes[i];
				}
			}

			return Vector3D.Normalize(bestAxis);
		}

		/// <summary>
		/// Sets dithering on a block and recursively on all its model subparts
		/// (turret heads, piston tops, rotor parts, etc.)
		/// Also toggles functional blocks off/on to suppress visual effects
		/// (thruster flames, lights, etc.)
		/// </summary>
		private void SetBlockDithering(IMySlimBlock block, float dithering)
		{
			block.Dithering = dithering;

			// Toggle functional blocks to suppress visual effects
			var functionalBlock = block.FatBlock as IMyFunctionalBlock;
			if (functionalBlock != null)
			{
				if (dithering > 0f && functionalBlock.Enabled)
				{
					// Block is being hidden and is currently on — disable it and remember
					disabledBlocks_.Add(functionalBlock.EntityId);
					functionalBlock.Enabled = false;
				}
				else if (dithering == 0f && disabledBlocks_.Remove(functionalBlock.EntityId))
				{
					// Block is being restored and we were the ones who turned it off
					functionalBlock.Enabled = true;
				}
			}

			// Handle subparts (turret azimuth/elevation/barrel, etc.)
			var entity = block.FatBlock as MyEntity;
			if (entity != null)
			{
				SetSubpartDithering(entity, dithering);
			}
		}

		private void SetSubpartDithering(MyEntity entity, float dithering)
		{
			if (entity.Subparts == null) return;

			foreach (var subpart in entity.Subparts.Values)
			{
				if (subpart.Render != null)
				{
					subpart.Render.Transparency = dithering;
					subpart.Render.RemoveRenderObjects();
					subpart.Render.AddRenderObjects();
				}
				// Recurse for nested subparts (e.g. turret elevation → barrel)
				SetSubpartDithering(subpart, dithering);
			}
		}

		private void BuildSliceLayers()
		{
			var grids = new List<IMyCubeGrid>();
			MyAPIGateway.GridGroups.GetGroup(sliceGrid_, GridLinkTypeEnum.Mechanical, grids);

			var blocksByLayer = new SortedDictionary<int, List<IMySlimBlock>>();

			foreach (var grid in grids)
			{
				double gridSize = grid.GridSize; // 2.5m for large, 0.5m for small
				var blocks = new List<IMySlimBlock>();
				grid.GetBlocks(blocks);

				foreach (var block in blocks)
				{
					Vector3D worldPos = grid.GridIntegerToWorld(block.Position);
					double projected = Vector3D.Dot(worldPos, sliceNormalWorld_);
					int layer = (int)Math.Round(projected / gridSize);

					List<IMySlimBlock> layerList;
					if (!blocksByLayer.TryGetValue(layer, out layerList))
					{
						layerList = new List<IMySlimBlock>();
						blocksByLayer[layer] = layerList;
					}
					layerList.Add(block);
				}
			}

			// SortedDictionary is ascending by key.
			// Camera-facing blocks have the lowest projected values (outside),
			// so ascending order = outside to inside.
			sliceLayers_ = new List<List<IMySlimBlock>>(blocksByLayer.Values);
		}
	}
}