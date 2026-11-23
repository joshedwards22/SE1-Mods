using System;
using System.Collections.Generic;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace JSCreativeBlockTools
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	public class JSCreativeBlockTools : MySessionComponentBase
	{
		private const int MAX_RAYCAST_DISTANCE = 10000;

		// Fully hidden blocks use negative dithering. Armor edges are separate per-grid render parts that ignore
		// block dithering; the grid only removes a block's edges when its dithering is negative (the projection path).
		private const float HIDDEN_BLOCK_DITHERING = -1.0f;
		private static readonly string[] ARMOR_KEYWORDS = new string[]
		{
			"Armor",
			"Floor",
			"Structural",
			"Beam",
			"Rounded",
			"Blast"
		};

		// Armor transparency state per grid (keyed by grid EntityId); missing entry means 0 (fully visible)
		private readonly Dictionary<long, float> armorTransparencyByGrid_ = new Dictionary<long, float>();

		// Slice mode state
		private bool sliceActive_ = false;
		private IMyCubeGrid sliceGrid_ = null;
		private List<IMyCubeGrid> sliceGrids_ = null;
		private Vector3D sliceNormalWorld_ = Vector3D.Zero;
		private int sliceDepth_ = 0;
		private List<List<IMySlimBlock>> sliceLayers_ = null;

		// Reusable raycast collections
		private readonly List<IHitInfo> raycastHits_ = new List<IHitInfo>();
		private readonly List<Vector3I> raycastCells_ = new List<Vector3I>();
		private readonly List<IMyCubeGrid> raycastGrids_ = new List<IMyCubeGrid>();

		private bool messageEnteredRegistered_ = false;

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			if (MyAPIGateway.Utilities != null && !MyAPIGateway.Utilities.IsDedicated)
			{
				MyAPIGateway.Utilities.MessageEntered += Utilities_MessageEntered;
				messageEnteredRegistered_ = true;
			}
		}

		protected override void UnloadData()
		{
			if (messageEnteredRegistered_ && MyAPIGateway.Utilities != null)
			{
				MyAPIGateway.Utilities.MessageEntered -= Utilities_MessageEntered;
				messageEnteredRegistered_ = false;
			}

			// Dithering is local-only and never saved, so slice/hide state simply goes away with the session
			sliceActive_ = false;
			sliceGrid_ = null;
			sliceGrids_ = null;
			sliceLayers_ = null;
			armorTransparencyByGrid_.Clear();

			raycastHits_.Clear();
			raycastCells_.Clear();
			raycastGrids_.Clear();

			base.UnloadData();
		}

		public override void HandleInput()
		{
			base.HandleInput();

			try
			{
				if (MyAPIGateway.Utilities == null || MyAPIGateway.Utilities.IsDedicated || MyAPIGateway.Session?.LocalHumanPlayer == null)
					return;

				// Always validate active slice grid even if a GUI screen is open
				if (sliceActive_ && (sliceGrid_ == null || sliceGrid_.Closed))
				{
					ExitSliceMode(true);
					return;
				}

				// Ignore keybinds while chat, terminal, menus, or cursor are active
				if (MyAPIGateway.Gui == null || MyAPIGateway.Gui.IsCursorVisible || MyAPIGateway.Gui.ChatEntryVisible)
					return;

				if (!HasCreativeAccess())
					return;

				// Ctrl+J: toggle slice mode
				if (MyAPIGateway.Input.IsNewKeyPressed(MyKeys.J) && MyAPIGateway.Input.IsAnyCtrlKeyPressed())
				{
					if (sliceActive_)
						ExitSliceMode(true);
					else
						EnterSliceMode();
					return;
				}

				// Numpad +/- while in slice mode
				if (sliceActive_)
				{
					if (MyAPIGateway.Input.IsNewKeyPressed(MyKeys.Add))
					{
						SliceUp();
						return;
					}
					else if (MyAPIGateway.Input.IsNewKeyPressed(MyKeys.Subtract))
					{
						SliceDown();
						return;
					}
				}

				// Existing keybinds
				if (MyAPIGateway.Input.IsNewKeyPressed(MyKeys.T) && MyAPIGateway.Input.IsAnyCtrlKeyPressed())
				{
					CycleArmorTransparency();
				}
				else if (MyAPIGateway.Input.IsNewKeyPressed(MyKeys.G) && MyAPIGateway.Input.IsAnyCtrlKeyPressed())
				{
					PickBlock();
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLineAndConsole($"CreativeBlockTools: Error in HandleInput: {ex}");
			}
		}

		private void Utilities_MessageEntered(string messageText, ref bool sendToOthers)
		{
			try
			{
				if (MyAPIGateway.Utilities == null || MyAPIGateway.Utilities.IsDedicated || MyAPIGateway.Session?.LocalHumanPlayer == null)
					return;

				if (string.IsNullOrWhiteSpace(messageText))
					return;

				string[] splits = messageText.Trim().Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
				if (splits.Length == 0)
					return;

				string command = splits[0].ToLowerInvariant();
				if (command != "/remove" && command != "/hide" && command != "/unhide")
					return;

				sendToOthers = false;

				if (!HasCreativeAccess())
				{
					MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "Creative mode or creative tools are required to use this command.");
					return;
				}

				if (command == "/remove")
				{
					if (splits.Length < 2 || string.IsNullOrWhiteSpace(splits[1]))
					{
						MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "Usage: '/remove name' - will remove all blocks whose name contains 'name' if they won't break the grid by removing them");
						return;
					}
					RemoveBlocks(splits[1].Trim());
				}
				else if (command == "/hide")
				{
					if (splits.Length < 2 || string.IsNullOrWhiteSpace(splits[1]))
					{
						MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "Usage: '/hide name' - will hide all blocks whose name contains 'name'");
						return;
					}
					HideBlocksByName(splits[1].Trim());
				}
				else if (command == "/unhide")
				{
					if (sliceActive_)
					{
						ExitSliceMode(true);
					}
					else
					{
						UnHideBlocks();
					}
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLineAndConsole($"CreativeBlockTools: Error handling chat command: {ex}");
			}
		}

		private bool HasCreativeAccess()
		{
			return MyAPIGateway.Session != null &&
				(MyAPIGateway.Session.CreativeMode || MyAPIGateway.Session.HasCreativeRights);
		}

		private IMyCubeGrid GetTargetedGrid()
		{
			var player = MyAPIGateway.Session?.LocalHumanPlayer;
			var camera = MyAPIGateway.Session?.Camera;
			if (player == null || camera == null || MyAPIGateway.Physics == null)
				return null;

			var cameraMatrix = camera.WorldMatrix;
			var forwardVector = Vector3D.Normalize(cameraMatrix.Forward);
			var fromVector = cameraMatrix.Translation;
			var toVector = fromVector + forwardVector * MAX_RAYCAST_DISTANCE;

			raycastHits_.Clear();
			MyAPIGateway.Physics.CastRay(fromVector, toVector, raycastHits_);
			if (raycastHits_.Count == 0)
				return null;

			IMyEntity localCharacter = player.Character as IMyEntity;
			IHitInfo closestValidHit = null;
			float bestFraction = float.MaxValue;

			foreach (var hit in raycastHits_)
			{
				if (hit == null || hit.HitEntity == null)
					continue;

				// Ignore the player's own character in 3rd-person view
				if (localCharacter != null && (hit.HitEntity == localCharacter || hit.HitEntity.GetTopMostParent() == localCharacter))
					continue;

				if (hit.Fraction < bestFraction)
				{
					bestFraction = hit.Fraction;
					closestValidHit = hit;
				}
			}

			if (closestValidHit == null || closestValidHit.HitEntity?.Physics == null)
				return null;

			var grid = closestValidHit.HitEntity.GetTopMostParent() as IMyCubeGrid;
			if (grid == null || grid.Closed)
				return null;

			return grid;
		}

		private IMySlimBlock GetTargetedBlock()
		{
			IMyCubeGrid grid = GetTargetedGrid();
			var camera = MyAPIGateway.Session?.Camera;
			if (grid == null || camera == null)
				return null;

			var cameraMatrix = camera.WorldMatrix;
			var forwardVector = Vector3D.Normalize(cameraMatrix.Forward);
			var fromVector = cameraMatrix.Translation;
			var toVector = fromVector + forwardVector * MAX_RAYCAST_DISTANCE;

			// Hidden blocks keep their physics, so the physics ray may stop on a hidden shell of the main grid.
			// Walk cells along the ray on every grid in the mechanical group so fully hidden/sliced blocks
			// (see IsBlockHidden) can be picked through, including into rotor/piston/hinge subgrids.
			GetMechanicalGroup(grid, raycastGrids_);

			IMySlimBlock bestBlock = null;
			double bestDistanceSq = double.MaxValue;

			foreach (var groupGrid in raycastGrids_)
			{
				raycastCells_.Clear();
				groupGrid.RayCastCells(fromVector, toVector, raycastCells_);

				foreach (var cell in raycastCells_)
				{
					IMySlimBlock block = groupGrid.GetCubeBlock(cell);
					if (block == null || block.IsDestroyed)
						continue;

					if (IsBlockHidden(block))
						continue;

					// Cells are ordered along the ray, so the first visible block is the nearest on this grid
					double distanceSq = Vector3D.DistanceSquared(fromVector, groupGrid.GridIntegerToWorld(cell));
					if (distanceSq < bestDistanceSq)
					{
						bestDistanceSq = distanceSq;
						bestBlock = block;
					}
					break;
				}
			}

			raycastGrids_.Clear();
			return bestBlock;
		}

		/// <summary>
		/// Fills result with the grid and all grids mechanically linked to it (rotors, pistons, hinges),
		/// skipping null or closed grids.
		/// </summary>
		private void GetMechanicalGroup(IMyCubeGrid grid, List<IMyCubeGrid> result)
		{
			result.Clear();
			if (grid == null || grid.Closed)
				return;

			if (MyAPIGateway.GridGroups != null)
			{
				MyAPIGateway.GridGroups.GetGroup(grid, GridLinkTypeEnum.Mechanical, result);
			}

			result.RemoveAll(g => g == null || g.Closed);

			if (!result.Contains(grid))
			{
				result.Add(grid);
			}
		}

		/// <summary>
		/// Makes every block on the given grids fully visible and clears their armor transparency state.
		/// </summary>
		private void ResetGridsVisibility(List<IMyCubeGrid> grids)
		{
			var blocks = new List<IMySlimBlock>();
			foreach (var grid in grids)
			{
				if (grid == null || grid.Closed)
					continue;

				blocks.Clear();
				grid.GetBlocks(blocks);
				foreach (var block in blocks)
				{
					SetBlockDithering(block, 0.0f);
				}

				armorTransparencyByGrid_.Remove(grid.EntityId);
			}
		}

		private void PickBlock()
		{
			var player = MyAPIGateway.Session?.LocalHumanPlayer;
			if (player == null)
				return;

			if (!(MyAPIGateway.Session.ControlledObject is VRage.Game.ModAPI.IMyCharacter))
			{
				MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "Must be on foot to pick a block");
				return;
			}

			var ctrl = MyAPIGateway.Session.ControlledObject as Sandbox.Game.Entities.IMyControllableEntity;
			if (ctrl == null || MyCubeBuilder.Static == null)
				return;

			IMySlimBlock block = GetTargetedBlock();
			if (block == null || block.BlockDefinition == null)
			{
				MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "No block found");
				return;
			}

			var cubeBlockDef = block.BlockDefinition as MyCubeBlockDefinition;
			if (cubeBlockDef != null && (!cubeBlockDef.Public || !cubeBlockDef.Enabled))
			{
				string nonBuildableName = !string.IsNullOrEmpty(cubeBlockDef.DisplayNameText)
					? cubeBlockDef.DisplayNameText
					: cubeBlockDef.Id.SubtypeName;
				MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "Block cannot be placed: " + nonBuildableName);
				return;
			}

			MyDefinitionId defId = block.BlockDefinition.Id;

			// Assign to toolbar slot 9 (index 8) and activate build mode
			MyVisualScriptLogicProvider.SetToolbarSlotToItemLocal(8, defId, player.IdentityId);

			// Switch to CubePlacer (enters build mode like pressing a number key)
			ctrl.SwitchToWeapon(new MyDefinitionId(typeof(MyObjectBuilder_CubePlacer)));
			MyCubeBuilder.Static.Activate(defId);

			string displayName = !string.IsNullOrEmpty(block.BlockDefinition.DisplayNameText)
				? block.BlockDefinition.DisplayNameText
				: defId.SubtypeName;
			MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "Picked: " + displayName);
		}

		private void CycleArmorTransparency()
		{
			if (sliceActive_)
			{
				ExitSliceMode(false);
			}

			IMyCubeGrid grid = GetTargetedGrid();
			if (grid == null)
			{
				MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "No grid targeted");
				return;
			}

			float current;
			armorTransparencyByGrid_.TryGetValue(grid.EntityId, out current);

			float next;
			if (current == 0.0f)
			{
				next = 0.5f;
			}
			else if (current == 0.5f)
			{
				next = 1.0f;
			}
			else
			{
				next = 0.0f;
			}

			var grids = new List<IMyCubeGrid>();
			GetMechanicalGroup(grid, grids);

			foreach (var groupGrid in grids)
			{
				if (next == 0.0f)
					armorTransparencyByGrid_.Remove(groupGrid.EntityId);
				else
					armorTransparencyByGrid_[groupGrid.EntityId] = next;
			}

			SetBlockTransparency(grids, next, ARMOR_KEYWORDS, "armor/structural blocks");
		}

		private void HideBlocksByName(string name)
		{
			if (sliceActive_)
			{
				ExitSliceMode(false);
			}

			IMyCubeGrid grid = GetTargetedGrid();
			if (grid == null)
			{
				MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "No grid targeted");
				return;
			}

			var grids = new List<IMyCubeGrid>();
			GetMechanicalGroup(grid, grids);

			SetBlockTransparency(grids, 1.0f, new[] { name }, "blocks matching: " + name);
		}

		private void SetBlockTransparency(List<IMyCubeGrid> grids, float transparency, string[] names, string label)
		{
			if (grids == null || names == null || names.Length == 0)
				return;

			List<IMySlimBlock> allBlocks = new List<IMySlimBlock>();
			int count = 0;

			foreach (var grid in grids)
			{
				if (grid == null || grid.Closed)
					continue;

				allBlocks.Clear();
				grid.GetBlocks(allBlocks);

				foreach (IMySlimBlock block in allBlocks)
				{
					if (block == null || block.IsDestroyed)
						continue;

					string displayName = block.BlockDefinition?.DisplayNameText;
					if (string.IsNullOrEmpty(displayName))
						continue;

					bool matched = false;
					for (int i = 0; i < names.Length; i++)
					{
						if (!string.IsNullOrEmpty(names[i]) && displayName.IndexOf(names[i], StringComparison.OrdinalIgnoreCase) >= 0)
						{
							matched = true;
							break;
						}
					}

					if (matched)
					{
						count++;
						SetBlockDithering(block, transparency);
					}
				}
			}

			int percent = (int)Math.Round(transparency * 100.0f);
			MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", $"Set transparency to {percent}% for {count} {label}");
		}

		private void UnHideBlocks()
		{
			IMyCubeGrid grid = GetTargetedGrid();
			if (grid == null)
			{
				MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "No grid targeted");
				return;
			}

			MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "Unhiding all blocks");

			var grids = new List<IMyCubeGrid>();
			GetMechanicalGroup(grid, grids);
			ResetGridsVisibility(grids);
		}

		private void RemoveBlocks(string name)
		{
			if (MyAPIGateway.Session == null || !MyAPIGateway.Session.IsServer)
			{
				MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "/remove can only be used in singleplayer or by the host/server");
				return;
			}

			IMyCubeGrid grid = GetTargetedGrid();
			if (grid == null)
			{
				MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "No grid targeted");
				return;
			}

			List<IMySlimBlock> allBlocks = new List<IMySlimBlock>();
			List<IMySlimBlock> targetBlocks = new List<IMySlimBlock>();

			grid.GetBlocks(allBlocks);
			int totalBlockCount = allBlocks.Count;

			foreach (IMySlimBlock block in allBlocks)
			{
				if (block == null || block.IsDestroyed)
					continue;

				string displayName = block.BlockDefinition?.DisplayNameText;
				if (!string.IsNullOrEmpty(displayName) && displayName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					targetBlocks.Add(block);
				}
			}

			MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "Found " + targetBlocks.Count + " blocks matching: " + name);

			int count = 0;
			bool removedAny = true;

			while (removedAny && !grid.Closed && targetBlocks.Count > 0 && (totalBlockCount - count) > 1)
			{
				removedAny = false;

				for (int i = targetBlocks.Count - 1; i >= 0; i--)
				{
					if (grid.Closed || (totalBlockCount - count) <= 1)
						break;

					IMySlimBlock target = targetBlocks[i];
					if (target == null || target.IsDestroyed)
					{
						targetBlocks.RemoveAt(i);
						continue;
					}

					if (!grid.WillRemoveBlockSplitGrid(target))
					{
						targetBlocks.RemoveAt(i);
						grid.RemoveBlock(target, true);
						count++;
						removedAny = true;
					}
				}
			}

			MyAPIGateway.Utilities.ShowMessage("CreativeBlockTools", "Removed " + count + " blocks");
		}

		// ===== Slice Mode =====

		private void EnterSliceMode()
		{
			var camera = MyAPIGateway.Session?.Camera;
			if (camera == null)
				return;

			IMyCubeGrid grid = GetTargetedGrid();
			if (grid == null)
			{
				MyAPIGateway.Utilities.ShowNotification("Slice Mode: No grid targeted", 2000, "Red");
				return;
			}

			sliceGrid_ = grid;
			sliceDepth_ = 0;

			sliceGrids_ = new List<IMyCubeGrid>();
			GetMechanicalGroup(grid, sliceGrids_);

			// Start from a clean, fully visible state so every hidden block belongs to a slice layer.
			// Otherwise blocks hidden by /hide or Ctrl+T would pop visible when slicing past and back.
			ResetGridsVisibility(sliceGrids_);

			// Determine slice axis: snap camera forward to nearest grid axis
			var cameraMatrix = camera.WorldMatrix;
			var cameraForward = Vector3D.Normalize(cameraMatrix.Forward);
			sliceNormalWorld_ = GetNearestGridAxis(grid, cameraForward);

			// Build the layer data
			BuildSliceLayers();

			if (sliceLayers_ == null || sliceLayers_.Count == 0)
			{
				MyAPIGateway.Utilities.ShowNotification("Slice Mode: No blocks found", 2000, "Red");
				sliceGrid_ = null;
				sliceGrids_ = null;
				sliceLayers_ = null;
				return;
			}

			sliceActive_ = true;
			MyAPIGateway.Utilities.ShowNotification("Slice Mode: ON (Layer 0/" + sliceLayers_.Count + ")", 2000, "White");
		}

		private void ExitSliceMode(bool showNotification)
		{
			// Restore exactly the blocks slice mode hid (layers 0..sliceDepth_-1). Using the stored block
			// references means blocks that moved to a split/detached grid, or whose main grid closed, are
			// still restored. EnterSliceMode starts from a fully visible group, so these are the only hidden blocks.
			if (sliceLayers_ != null)
			{
				int restoreCount = Math.Min(sliceDepth_, sliceLayers_.Count);
				for (int i = 0; i < restoreCount; i++)
				{
					foreach (var block in sliceLayers_[i])
					{
						SetBlockDithering(block, 0.0f);
					}
				}
			}

			sliceActive_ = false;
			sliceGrid_ = null;
			sliceGrids_ = null;
			sliceNormalWorld_ = Vector3D.Zero;
			sliceDepth_ = 0;
			sliceLayers_ = null;

			if (showNotification && MyAPIGateway.Utilities != null)
			{
				MyAPIGateway.Utilities.ShowNotification("Slice Mode: OFF", 2000, "White");
			}
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
		/// (turret heads, piston tops, rotor parts, etc.).
		/// Purely visual and local: never changes block state, so nothing is persisted to the save.
		/// </summary>
		private void SetBlockDithering(IMySlimBlock block, float dithering)
		{
			if (block == null || block.IsDestroyed || block.CubeGrid == null || block.CubeGrid.Closed)
				return;

			// Fully hidden blocks use negative dithering so the grid also removes their armor edges
			float blockDithering = dithering >= 1.0f ? HIDDEN_BLOCK_DITHERING : dithering;

			// Skip the render update when nothing changes (e.g. /unhide on an already visible grid)
			if (block.Dithering != blockDithering)
			{
				block.Dithering = blockDithering;
			}

			// Handle subparts (always checked: they can be out of sync with the block if recreated) (turret azimuth/elevation/barrel, etc.)
			var entity = block.FatBlock as MyEntity;
			if (entity != null && !entity.Closed)
			{
				SetSubpartDithering(entity, dithering);
			}
		}

		/// <summary>
		/// True when the block has been fully hidden by this mod (positive 1.0 or the negative edge-hiding value).
		/// </summary>
		private bool IsBlockHidden(IMySlimBlock block)
		{
			return block.Dithering >= 1.0f || block.Dithering <= HIDDEN_BLOCK_DITHERING;
		}

		private void SetSubpartDithering(MyEntity entity, float dithering)
		{
			if (entity == null || entity.Closed || entity.Subparts == null)
				return;

			foreach (var subpart in entity.Subparts.Values)
			{
				if (subpart == null || subpart.Closed || !subpart.InScene)
					continue;

				// Only rebuild render objects when the transparency actually changes; this is the expensive part
				if (subpart.Render != null && subpart.Render.Transparency != dithering)
				{
					subpart.Render.Transparency = dithering;
					subpart.Render.RemoveRenderObjects();
					subpart.Render.AddRenderObjects();
				}

				// Recurse for nested subparts (e.g. turret elevation -> barrel)
				SetSubpartDithering(subpart, dithering);
			}
		}

		private void BuildSliceLayers()
		{
			// sliceGrids_ is filled by EnterSliceMode before this is called

			// Project all mechanical subgrids relative to a single origin and unified step size
			// so Large and Small subgrids stay spatially aligned in the same layer dictionary
			Vector3D origin = sliceGrid_.WorldMatrix.Translation;
			double stepSize = sliceGrid_.GridSize;

			var blocksByLayer = new SortedDictionary<int, List<IMySlimBlock>>();

			foreach (var grid in sliceGrids_)
			{
				if (grid == null || grid.Closed)
					continue;

				var blocks = new List<IMySlimBlock>();
				grid.GetBlocks(blocks);

				foreach (var block in blocks)
				{
					if (block == null || block.IsDestroyed)
						continue;

					Vector3D worldPos = grid.GridIntegerToWorld(block.Position);
					double projected = Vector3D.Dot(worldPos - origin, sliceNormalWorld_);
					int layer = (int)Math.Round(projected / stepSize);

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
