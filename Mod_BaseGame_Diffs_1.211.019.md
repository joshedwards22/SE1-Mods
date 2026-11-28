# Space Engineers Mod vs Base Game Diffs (1.211.019)

This document shows the differences between `.sbc` definitions and `ModAdjuster` `.xml` edits in your `Mods` folder and their targets in Base Game, your other mods, or Steam Workshop mods. Whitespace-only differences are ignored.

## Mod: JS - Conveyor Expansion (35 differences)

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section1_Position1_LargeBlocks`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -7,955 +7,15 @@
   <Name>Section1_Position1_LargeBlocks</Name>
   <StrictSearch>true</StrictSearch>
   <ItemIds>
+    <string>Conveyor/ConveyorTubeElbow</string>
+    <string>Conveyor/ConveyorTubeCross</string>
+    <string>Conveyor/ConveyorTubeDuctElbow</string>
+    <string>Conveyor/ConveyorTubeDuctCross</string>
+    <string>Conveyor/LargeBlockConveyorPipeElbow</string>
+    <string>Conveyor/LargeConveyorStraight</string>
+    <string>Conveyor/LargeConveyorCurved</string>
+    <string>Conveyor/LargeConveyorT</string>
+    <string>Conveyor/LargeConveyorElbow</string>
+    <string>Conveyor/LargeConveyorCross</string>
     </ItemIds>
   </Category>
```

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section1_Position1_SmallBlocks`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -7,579 +7,14 @@
   <Name>Section1_Position1_SmallBlocks</Name>
   <StrictSearch>true</StrictSearch>
   <ItemIds>
+    <string>Conveyor/ConveyorTubeSmallElbow</string>
+    <string>Conveyor/ConveyorTubeSmallCross</string>
+    <string>Conveyor/MediumBlockConveyor</string>
+    <string>ConveyorConnector/ConveyorTubeMedium3X</string>
+    <string>Conveyor/ConveyorTubeTMedium</string>
+    <string>Conveyor/ConveyorTubeElbowMedium</string>
+    <string>Conveyor/ConveyorTubeCrossMedium</string>
+    <string>Conveyor/ConveyorTubeDuctSmallElbow</string>
+    <string>Conveyor/ConveyorTubeDuctSmallCross</string>
     </ItemIds>
   </Category>
```

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section1_Position3_Logistics`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -7,115 +7,24 @@
   <Name>Section1_Position3_Logistics</Name>
   <StrictSearch>true</StrictSearch>
   <ItemIds>
+    <string>Conveyor/ConveyorTubeElbow</string>
+    <string>Conveyor/ConveyorTubeCross</string>
+    <string>Conveyor/ConveyorTubeDuctElbow</string>
+    <string>Conveyor/ConveyorTubeDuctCross</string>
+    <string>Conveyor/LargeBlockConveyorPipeElbow</string>
+    <string>Conveyor/LargeConveyorStraight</string>
+    <string>Conveyor/LargeConveyorCurved</string>
+    <string>Conveyor/LargeConveyorT</string>
+    <string>Conveyor/LargeConveyorElbow</string>
+    <string>Conveyor/LargeConveyorCross</string>
+    <string>Conveyor/ConveyorTubeSmallElbow</string>
+    <string>Conveyor/ConveyorTubeSmallCross</string>
+    <string>Conveyor/MediumBlockConveyor</string>
+    <string>ConveyorConnector/ConveyorTubeMedium3X</string>
+    <string>Conveyor/ConveyorTubeTMedium</string>
+    <string>Conveyor/ConveyorTubeElbowMedium</string>
+    <string>Conveyor/ConveyorTubeCrossMedium</string>
+    <string>Conveyor/ConveyorTubeDuctSmallElbow</string>
+    <string>Conveyor/ConveyorTubeDuctSmallCross</string>
     </ItemIds>
   </Category>
```

#### Tag: `BlockVariantGroup` | Type: `MyObjectBuilder_BlockVariantGroup` | Subtype: `ConveyorGroup`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\BlockVariantGroups.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockVariantGroups.sbc`

```diff
--- Base Game
+++ Mod
@@ -1,25 +1,39 @@
 <BlockVariantGroup>
   <Id Type="MyObjectBuilder_BlockVariantGroup" Subtype="ConveyorGroup" />
-  <Icon>Textures\GUI\Icons\Cubes\conveyor.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\conveyor.dds</Icon>
   <DisplayName>DisplayName_Block_Conveyor</DisplayName>
   <Description>Description_ConveyorTube</Description>
   <Blocks>
     <Block Type="MyObjectBuilder_Conveyor" Subtype="LargeBlockConveyor" />
+    <Block Type="MyObjectBuilder_Conveyor" Subtype="MediumBlockConveyor" />
+    <Block Type="MyObjectBuilder_Conveyor" Subtype="LargeConveyorStraight" />
+    <Block Type="MyObjectBuilder_Conveyor" Subtype="LargeConveyorCurved" />
+    <Block Type="MyObjectBuilder_Conveyor" Subtype="LargeConveyorT" />
+    <Block Type="MyObjectBuilder_Conveyor" Subtype="LargeConveyorElbow" />
+    <Block Type="MyObjectBuilder_Conveyor" Subtype="LargeConveyorCross" />
     <Block Type="MyObjectBuilder_ConveyorConnector" Subtype="ConveyorTube" />
     <Block Type="MyObjectBuilder_ConveyorConnector" Subtype="ConveyorTube3X" />
     <Block Type="MyObjectBuilder_ConveyorConnector" Subtype="ConveyorTubeMedium" />
-    <Block Type="MyObjectBuilder_ConveyorConnector" Subtype="ConveyorFrameMedium" />
+    <Block Type="MyObjectBuilder_ConveyorConnector" Subtype="ConveyorTubeMedium3X" />
     <Block Type="MyObjectBuilder_ConveyorConnector" Subtype="ConveyorTubeCurved" />
     <Block Type="MyObjectBuilder_ConveyorConnector" Subtype="ConveyorTubeCurvedMedium" />
     <Block Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeT" />
-    <Block Type="MyObjectBuilder_Conveyor" Subtype="SmallShipConveyorHub" />
-    <Block Type="MyObjectBuilder_ConveyorSorter" Subtype="LargeBlockConveyorSorter" />
-    <Block Type="MyObjectBuilder_ConveyorSorter" Subtype="MediumBlockConveyorSorter" />
-    <Block Type="MyObjectBuilder_Conveyor" Subtype="SmallBlockConveyorConverter" />
+    <Block Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeTMedium" />
+    <Block Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeElbow" />
+    <Block Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeElbowMedium" />
+    <Block Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeCross" />
+    <Block Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeCrossMedium" />
     <Block Type="MyObjectBuilder_ConveyorConnector" Subtype="ConveyorTubeDuct" />
     <Block Type="MyObjectBuilder_ConveyorConnector" Subtype="ConveyorTubeDuct3X" />
     <Block Type="MyObjectBuilder_ConveyorConnector" Subtype="ConveyorTubeDuctCurved" />
     <Block Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeDuctT" />
+    <Block Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeDuctElbow" />
+    <Block Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeDuctCross" />
+    <Block Type="MyObjectBuilder_ConveyorConnector" Subtype="ConveyorFrameMedium" />
+    <Block Type="MyObjectBuilder_Conveyor" Subtype="SmallShipConveyorHub" />
+    <Block Type="MyObjectBuilder_Conveyor" Subtype="SmallBlockConveyorConverter" />
+    <Block Type="MyObjectBuilder_ConveyorSorter" Subtype="LargeBlockConveyorSorter" />
+    <Block Type="MyObjectBuilder_ConveyorSorter" Subtype="MediumBlockConveyorSorter" />
     <Block Type="MyObjectBuilder_CubeBlock" Subtype="LargeBlockConveyorCap" />
     <Block Type="MyObjectBuilder_CubeBlock" Subtype="SmallBlockConveyorCapMedium" />
     </Blocks>
```

#### Tag: `BlockVariantGroup` | Type: `MyObjectBuilder_BlockVariantGroup` | Subtype: `ConveyorPipeGroup`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\BlockVariantGroups.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockVariantGroups.sbc`

```diff
--- Base Game
+++ Mod
@@ -1,12 +1,10 @@
 <BlockVariantGroup>
   <Id Type="MyObjectBuilder_BlockVariantGroup" Subtype="ConveyorPipeGroup" />
-  <Icon>Textures\GUI\Icons\Cubes\PipeNode.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\PipeNode.dds</Icon>
   <DisplayName>DisplayName_Block_ConveyorPipes</DisplayName>
   <Description>Description_ConveyorTube</Description>
   <Blocks>
     <Block Type="MyObjectBuilder_Conveyor" Subtype="LargeBlockConveyorPipeJunction" />
-    <Block Type="MyObjectBuilder_Conveyor" Subtype="LargeBlockConveyorPipeIntersection" />
-    <Block Type="MyObjectBuilder_Conveyor" Subtype="LargeBlockConveyorPipeT" />
     <Block Type="MyObjectBuilder_ConveyorConnector" Subtype="LargeBlockConveyorPipeSeamless" />
     <Block Type="MyObjectBuilder_ConveyorConnector" Subtype="LargeBlockConveyorPipeSeamless3X" />
     <Block Type="MyObjectBuilder_ConveyorConnector" Subtype="LargeBlockConveyorPipeFlange" />
@@ -14,6 +12,9 @@
     <Block Type="MyObjectBuilder_ConveyorConnector" Subtype="LargeBlockConveyorPipeEnd" />
     <Block Type="MyObjectBuilder_ConveyorConnector" Subtype="LargeBlockConveyorPipeEnd3X" />
     <Block Type="MyObjectBuilder_ConveyorConnector" Subtype="LargeBlockConveyorPipeCorner" />
+    <Block Type="MyObjectBuilder_Conveyor" Subtype="LargeBlockConveyorPipeT" />
+    <Block Type="MyObjectBuilder_Conveyor" Subtype="LargeBlockConveyorPipeElbow" />
+    <Block Type="MyObjectBuilder_Conveyor" Subtype="LargeBlockConveyorPipeIntersection" />
     <Block Type="MyObjectBuilder_ConveyorSorter" Subtype="LargeBlockConveyorSorterIndustrial" />
     <Block Type="MyObjectBuilder_CubeBlock" Subtype="LargeBlockConveyorPipeCap" />
     </Blocks>
```

#### Tag: `BlockVariantGroup` | Type: `MyObjectBuilder_BlockVariantGroup` | Subtype: `ConveyorSmallGroup`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\BlockVariantGroups.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockVariantGroups.sbc`

```diff
--- Base Game
+++ Mod
@@ -1,6 +1,6 @@
 <BlockVariantGroup>
   <Id Type="MyObjectBuilder_BlockVariantGroup" Subtype="ConveyorSmallGroup" />
-  <Icon>Textures\GUI\Icons\Cubes\ConveyorSmall.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\ConveyorSmall.dds</Icon>
   <DisplayName>DisplayName_Block_ConveyorTubeSmall</DisplayName>
   <Description>Description_ConveyorTube</Description>
   <Blocks>
@@ -9,11 +9,15 @@
     <Block Type="MyObjectBuilder_ConveyorConnector" Subtype="ConveyorTubeSmall3X" />
     <Block Type="MyObjectBuilder_ConveyorConnector" Subtype="ConveyorTubeSmallCurved" />
     <Block Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeSmallT" />
-    <Block Type="MyObjectBuilder_ConveyorSorter" Subtype="SmallBlockConveyorSorter" />
+    <Block Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeSmallElbow" />
+    <Block Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeSmallCross" />
     <Block Type="MyObjectBuilder_ConveyorConnector" Subtype="ConveyorTubeDuctSmall" />
     <Block Type="MyObjectBuilder_ConveyorConnector" Subtype="ConveyorTubeDuctSmall3X" />
     <Block Type="MyObjectBuilder_ConveyorConnector" Subtype="ConveyorTubeDuctSmallCurved" />
     <Block Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeDuctSmallT" />
+    <Block Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeDuctSmallElbow" />
+    <Block Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeDuctSmallCross" />
+    <Block Type="MyObjectBuilder_ConveyorSorter" Subtype="SmallBlockConveyorSorter" />
     <Block Type="MyObjectBuilder_CubeBlock" Subtype="SmallBlockConveyorCap" />
     </Blocks>
   </BlockVariantGroup>
```

#### Tag: `ResearchGroup` | Type: `MyObjectBuilder_ResearchGroupDefinition` | Subtype: `25`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\ResearchGroups.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\ResearchGroups.sbc`

```diff
--- Base Game
+++ Mod
@@ -39,5 +39,24 @@
     <BlockId Type="MyObjectBuilder_CubeBlock" Subtype="SmallBlockConveyorCapMedium" />
     <BlockId Type="MyObjectBuilder_CubeBlock" Subtype="SmallBlockConveyorCap" />
     <BlockId Type="MyObjectBuilder_Conveyor" Subtype="LargeBlockStructural_PlatformConveyor" />
+    <BlockId Type="MyObjectBuilder_Conveyor" Subtype="LargeConveyorStraight" />
+    <BlockId Type="MyObjectBuilder_Conveyor" Subtype="LargeConveyorCurved" />
+    <BlockId Type="MyObjectBuilder_Conveyor" Subtype="LargeConveyorT" />
+    <BlockId Type="MyObjectBuilder_Conveyor" Subtype="LargeConveyorElbow" />
+    <BlockId Type="MyObjectBuilder_Conveyor" Subtype="LargeConveyorCross" />
+    <BlockId Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeElbow" />
+    <BlockId Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeCross" />
+    <BlockId Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeDuctElbow" />
+    <BlockId Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeDuctCross" />
+    <BlockId Type="MyObjectBuilder_Conveyor" Subtype="LargeBlockConveyorPipeElbow" />
+    <BlockId Type="MyObjectBuilder_Conveyor" Subtype="MediumBlockConveyor" />
+    <BlockId Type="MyObjectBuilder_ConveyorConnector" Subtype="ConveyorTubeMedium3X" />
+    <BlockId Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeTMedium" />
+    <BlockId Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeElbowMedium" />
+    <BlockId Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeCrossMedium" />
+    <BlockId Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeSmallElbow" />
+    <BlockId Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeSmallCross" />
+    <BlockId Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeDuctSmallElbow" />
+    <BlockId Type="MyObjectBuilder_Conveyor" Subtype="ConveyorTubeDuctSmallCross" />
     </Members>
   </ResearchGroup>
```

#### Tag: `Definition` | Type: `ConveyorConnector` | Subtype: `ConveyorTubeDuct`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorDucts.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Logistics.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,14 +4,14 @@
     <SubtypeId>ConveyorTubeDuct</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorTubeDuct</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\ConveyorDuct.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\ConveyorDuct.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <GuiVisible>true</GuiVisible>
   <CubeSize>Large</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="1" z="1" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Large\ConveyorDuct.mwm</Model>
+  <Model>Models\Cubes\large\ConveyorDuct.mwm</Model>
   <Components>
     <Component Subtype="SteelPlate" Count="15" />
     <Component Subtype="InteriorPlate" Count="10" />
@@ -23,8 +23,8 @@
     </Components>
   <CriticalComponent Subtype="Motor" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\Large\ConveyorDuctConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Large\ConveyorDuctConstruction_2.mwm" />
+    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\large\ConveyorDuct_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\large\ConveyorDuct_BS2.mwm" />
     </BuildProgressModels>
   <BlockPairName>ConveyorTubeDuct</BlockPairName>
   <EdgeType>Light</EdgeType>
```

#### Tag: `Definition` | Type: `ConveyorConnector` | Subtype: `ConveyorTubeDuct3X`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorDucts.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Logistics.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,14 +4,14 @@
     <SubtypeId>ConveyorTubeDuct3X</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorTubeDuct3X</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\ConveyorTubeDuct3X.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\ConveyorTubeDuct3X.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <GuiVisible>true</GuiVisible>
   <CubeSize>Large</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="3" z="1" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Large\LongerReinforcedConveyorTubes.mwm</Model>
+  <Model>Models\Cubes\large\ConveyorDuct3x.mwm</Model>
   <Components>
     <Component Subtype="SteelPlate" Count="45" />
     <Component Subtype="InteriorPlate" Count="30" />
@@ -23,8 +23,8 @@
     </Components>
   <CriticalComponent Subtype="Motor" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\Large\LongerReinforcedConveyorTubesConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Large\LongerReinforcedConveyorTubesConstruction_2.mwm" />
+    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\large\ConveyorDuct3x_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\large\ConveyorDuct3x_BS2.mwm" />
     </BuildProgressModels>
   <BlockPairName>ConveyorTubeDuct3X</BlockPairName>
   <EdgeType>Light</EdgeType>
```

#### Tag: `Definition` | Type: `ConveyorConnector` | Subtype: `ConveyorTubeDuctCurved`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorDucts.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Logistics.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,14 +4,14 @@
     <SubtypeId>ConveyorTubeDuctCurved</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorTubeDuctCurved</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\ConveyorDuctCurved.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\ConveyorDuctCurved.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <GuiVisible>true</GuiVisible>
   <CubeSize>Large</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="1" z="1" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Large\ConveyorDuctCurved.mwm</Model>
+  <Model>Models\Cubes\large\ConveyorDuctCurved.mwm</Model>
   <Components>
     <Component Subtype="SteelPlate" Count="15" />
     <Component Subtype="InteriorPlate" Count="10" />
@@ -23,8 +23,8 @@
     </Components>
   <CriticalComponent Subtype="Motor" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\Large\ConveyorDuctCurvedConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Large\ConveyorDuctCurvedConstruction_2.mwm" />
+    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\large\ConveyorDuctCurved_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\large\ConveyorDuctCurved_BS2.mwm" />
     </BuildProgressModels>
   <BlockPairName>ConveyorTubeDuctCurved</BlockPairName>
   <EdgeType>Light</EdgeType>
```

#### Tag: `Definition` | Type: `Conveyor` | Subtype: `ConveyorTubeDuctT`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorDucts.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Logistics.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,14 +4,14 @@
     <SubtypeId>ConveyorTubeDuctT</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorTubeDuctT</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\ConveyorDuctT.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\ConveyorDuctT.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <GuiVisible>true</GuiVisible>
   <CubeSize>Large</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="1" z="1" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Large\ConveyorDuctT.mwm</Model>
+  <Model>Models\Cubes\large\ConveyorDuctT.mwm</Model>
   <Components>
     <Component Subtype="SteelPlate" Count="12" />
     <Component Subtype="InteriorPlate" Count="10" />
@@ -23,8 +23,8 @@
     </Components>
   <CriticalComponent Subtype="Motor" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\Large\ConveyorDuctTConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Large\ConveyorDuctTConstruction_2.mwm" />
+    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\large\ConveyorDuctT_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\large\ConveyorDuctT_BS2.mwm" />
     </BuildProgressModels>
   <BlockPairName>ConveyorTubeDuctT</BlockPairName>
   <EdgeType>Light</EdgeType>
```

#### Tag: `Definition` | Type: `ConveyorConnector` | Subtype: `ConveyorTubeDuctSmall`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorDucts.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Logistics.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,13 +4,13 @@
     <SubtypeId>ConveyorTubeDuctSmall</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorTubeDuctSmall</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\ConveyorDuctSmall.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\ConveyorDuctSmall.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <CubeSize>Small</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="1" z="1" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Small\ConveyorDuct.mwm</Model>
+  <Model>Models\Cubes\small\ConveyorDuctSmall.mwm</Model>
   <Components>
     <Component Subtype="SteelPlate" Count="1" />
     <Component Subtype="InteriorPlate" Count="1" />
@@ -31,9 +31,9 @@
     <MountPoint Side="Left" StartX="0" StartY="0.85" EndX="1" EndY="1.0" />
     <MountPoint Side="Left" StartX="0.45" StartY="0.45" EndX="0.55" EndY="0.55" />
     </MountPoints>
-  <CriticalComponent Subtype="Construction" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Small\ConveyorDuctConstruction_1.mwm" />
+    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\small\ConveyorDuctSmall_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\small\ConveyorDuctSmall_BS2.mwm" />
     </BuildProgressModels>
   <MirroringX>Z</MirroringX>
   <MirroringZ>X</MirroringZ>
```

#### Tag: `Definition` | Type: `ConveyorConnector` | Subtype: `ConveyorTubeDuctSmall3X`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorDucts.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Logistics.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,13 +4,13 @@
     <SubtypeId>ConveyorTubeDuctSmall3X</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorTubeDuctSmall3X</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\ConveyorTubeDuctSmall3X.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\ConveyorTubeDuctSmall3X.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <CubeSize>Small</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="1" z="3" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Small\LongerReinforcedConveyorTubes.mwm</Model>
+  <Model>Models\Cubes\small\ConveyorDuctSmall3x.mwm</Model>
   <Components>
     <Component Subtype="SteelPlate" Count="3" />
     <Component Subtype="InteriorPlate" Count="3" />
@@ -33,9 +33,9 @@
     <MountPoint Side="Left" StartX="1.45" StartY="0.45" EndX="1.55" EndY="0.55" />
     <MountPoint Side="Left" StartX="2.45" StartY="0.45" EndX="2.55" EndY="0.55" />
     </MountPoints>
-  <CriticalComponent Subtype="Construction" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Small\LongerReinforcedConveyorTubesConstruction_1.mwm" />
+    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\small\ConveyorDuctSmall3x_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\small\ConveyorDuctSmall3x_BS2.mwm" />
     </BuildProgressModels>
   <MirroringX>Z</MirroringX>
   <MirroringZ>X</MirroringZ>
```

#### Tag: `Definition` | Type: `ConveyorConnector` | Subtype: `ConveyorTubeDuctSmallCurved`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorDucts.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Logistics.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,13 +4,13 @@
     <SubtypeId>ConveyorTubeDuctSmallCurved</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorTubeDuctSmallCurved</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\ConveyorDuctCurvedSmall.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\ConveyorDuctCurvedSmall.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <CubeSize>Small</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="1" z="1" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Small\ConveyorDuctCurved.mwm</Model>
+  <Model>Models\Cubes\small\ConveyorDuctSmallCurved.mwm</Model>
   <Components>
     <Component Subtype="SteelPlate" Count="1" />
     <Component Subtype="InteriorPlate" Count="1" />
@@ -32,9 +32,9 @@
     <MountPoint Side="Left" StartX="0.45" StartY="0.45" EndX="0.55" EndY="0.55" />
     <MountPoint Side="Left" StartX="0.9" StartY="0" EndX="1" EndY="0.1" />
     </MountPoints>
-  <CriticalComponent Subtype="Construction" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Small\ConveyorDuctCurvedConstruction_1.mwm" />
+    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\small\ConveyorDuctSmallCurved_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\small\ConveyorDuctSmallCurved_BS2.mwm" />
     </BuildProgressModels>
   <MirroringX>YMinusHalfX</MirroringX>
   <MirroringY>HalfX</MirroringY>
```

#### Tag: `Definition` | Type: `Conveyor` | Subtype: `ConveyorTubeDuctSmallT`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorDucts.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Logistics.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,14 +4,14 @@
     <SubtypeId>ConveyorTubeDuctSmallT</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorTubeDuctSmallT</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\ConveyorDuctTSmall.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\ConveyorDuctTSmall.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <GuiVisible>true</GuiVisible>
   <CubeSize>Small</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="1" z="1" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Small\ConveyorDuctT.mwm</Model>
+  <Model>Models\Cubes\small\ConveyorDuctSmallT.mwm</Model>
   <Components>
     <Component Subtype="SteelPlate" Count="1" />
     <Component Subtype="InteriorPlate" Count="2" />
@@ -21,7 +21,8 @@
     </Components>
   <CriticalComponent Subtype="Construction" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Small\ConveyorDuctTConstruction_1.mwm" />
+    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\small\ConveyorDuctSmallT_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\small\ConveyorDuctSmallT_BS2.mwm" />
     </BuildProgressModels>
   <BlockPairName>ConveyorTubeDuctSmallT</BlockPairName>
   <EdgeType>Light</EdgeType>
@@ -44,7 +45,7 @@
   <EmissiveColorPreset>Default</EmissiveColorPreset>
   <DamageEffectName>Damage_HeavyMech_Damaged</DamageEffectName>
   <DamagedSound>ParticleHeavyMech</DamagedSound>
-  <DestroyEffect>BlockDestroyed_Large</DestroyEffect>
-  <DestroySound>PoofExplosionCat3</DestroySound>
+  <DestroyEffect>BlockDestroyed_Small</DestroyEffect>
+  <DestroySound>PoofExplosionCat1</DestroySound>
   <PCU>10</PCU>
   </Definition>
```

#### Tag: `Definition` | Type: `ConveyorConnector` | Subtype: `LargeBlockConveyorPipeSeamless`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorPipes.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_IndustrialPack.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,14 +4,14 @@
     <SubtypeId>LargeBlockConveyorPipeSeamless</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorPipeSeamless</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\PipeSeamless.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\PipeSeamless.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <GuiVisible>false</GuiVisible>
   <CubeSize>Large</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="1" z="1" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Large\PipeSeamless.mwm</Model>
+  <Model>Models\Cubes\large\PipeSeamless.mwm</Model>
   <Components>
     <Component Subtype="InteriorPlate" Count="10" />
     <Component Subtype="Construction" Count="20" />
@@ -21,9 +21,9 @@
     </Components>
   <CriticalComponent Subtype="Motor" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\Large\PipeSeamlessConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\Large\PipeSeamlessConstruction_2.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Large\PipeSeamlessConstruction_3.mwm" />
+    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\large\PipeSeamless_BS1.mwm" />
+    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\large\PipeSeamless_BS2.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\large\PipeSeamless_BS3.mwm" />
     </BuildProgressModels>
   <BlockPairName>ConveyorPipeSeamless</BlockPairName>
   <MirroringY>Z</MirroringY>
```

#### Tag: `Definition` | Type: `ConveyorConnector` | Subtype: `LargeBlockConveyorPipeSeamless3X`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorPipes.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_IndustrialPack.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,14 +4,14 @@
     <SubtypeId>LargeBlockConveyorPipeSeamless3X</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorPipeSeamless3X</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\PipeSeamless3X.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\PipeSeamless3X.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <GuiVisible>false</GuiVisible>
   <CubeSize>Large</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="3" z="1" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Large\PipeSeamless3X.mwm</Model>
+  <Model>Models\Cubes\large\PipeSeamless3x.mwm</Model>
   <Components>
     <Component Subtype="InteriorPlate" Count="30" />
     <Component Subtype="Construction" Count="60" />
@@ -21,9 +21,9 @@
     </Components>
   <CriticalComponent Subtype="Motor" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\Large\PipeSeamless3XConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\Large\PipeSeamless3XConstruction_2.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Large\PipeSeamless3XConstruction_3.mwm" />
+    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\large\PipeSeamless3x_BS1.mwm" />
+    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\large\PipeSeamless3x_BS2.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\large\PipeSeamless3x_BS3.mwm" />
     </BuildProgressModels>
   <BlockPairName>ConveyorPipeSeamless3X</BlockPairName>
   <MirroringY>Z</MirroringY>
```

#### Tag: `Definition` | Type: `ConveyorConnector` | Subtype: `LargeBlockConveyorPipeEnd`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorPipes.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_IndustrialPack.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,14 +4,14 @@
     <SubtypeId>LargeBlockConveyorPipeEnd</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorPipeEnd</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\PipeEnd.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\PipeEnd.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <GuiVisible>false</GuiVisible>
   <CubeSize>Large</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="1" z="1" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Large\PipeEnd.mwm</Model>
+  <Model>Models\Cubes\large\PipeEnd.mwm</Model>
   <Components>
     <Component Subtype="InteriorPlate" Count="10" />
     <Component Subtype="Construction" Count="20" />
@@ -21,9 +21,9 @@
     </Components>
   <CriticalComponent Subtype="Motor" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\Large\PipeEndConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\Large\PipeEndConstruction_2.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Large\PipeEndConstruction_3.mwm" />
+    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\large\PipeEnd_BS1.mwm" />
+    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\large\PipeEnd_BS2.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\large\PipeEnd_BS3.mwm" />
     </BuildProgressModels>
   <BlockPairName>ConveyorPipeEnd</BlockPairName>
   <MirroringY>Z</MirroringY>
@@ -40,8 +40,6 @@
     <MountPoint Side="Top" StartX="0.2" StartY="0.3" EndX="0.3" EndY="0.7" />
     <MountPoint Side="Top" StartX="0.7" StartY="0.3" EndX="0.8" EndY="0.7" />
     </MountPoints>
-  <DamageEffectName>Damage_HeavyMech_Damaged</DamageEffectName>
-  <DamagedSound>ParticleHeavyMech</DamagedSound>
   <EmissiveColorPreset>Default</EmissiveColorPreset>
   <DamageEffectName>Damage_HeavyMech_Damaged</DamageEffectName>
   <DamagedSound>ParticleHeavyMech</DamagedSound>
```

#### Tag: `Definition` | Type: `ConveyorConnector` | Subtype: `LargeBlockConveyorPipeEnd3X`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorPipes.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_IndustrialPack.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,14 +4,14 @@
     <SubtypeId>LargeBlockConveyorPipeEnd3X</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorPipeEnd3X</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\PipeEnd3X.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\PipeEnd3X.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <GuiVisible>false</GuiVisible>
   <CubeSize>Large</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="3" z="1" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Large\PipeEnd3X.mwm</Model>
+  <Model>Models\Cubes\large\PipeEnd3x.mwm</Model>
   <Components>
     <Component Subtype="InteriorPlate" Count="30" />
     <Component Subtype="Construction" Count="60" />
@@ -21,9 +21,9 @@
     </Components>
   <CriticalComponent Subtype="Motor" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\Large\PipeEnd3XConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\Large\PipeEnd3XConstruction_2.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Large\PipeEnd3XConstruction_3.mwm" />
+    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\large\PipeEnd3x_BS1.mwm" />
+    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\large\PipeEnd3x_BS2.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\large\PipeEnd3x_BS3.mwm" />
     </BuildProgressModels>
   <BlockPairName>ConveyorPipeEnd3X</BlockPairName>
   <MirroringY>Z</MirroringY>
@@ -40,8 +40,6 @@
     <MountPoint Side="Top" StartX="0.2" StartY="0.3" EndX="0.3" EndY="0.7" />
     <MountPoint Side="Top" StartX="0.7" StartY="0.3" EndX="0.8" EndY="0.7" />
     </MountPoints>
-  <DamageEffectName>Damage_HeavyMech_Damaged</DamageEffectName>
-  <DamagedSound>ParticleHeavyMech</DamagedSound>
   <EmissiveColorPreset>Default</EmissiveColorPreset>
   <DamageEffectName>Damage_HeavyMech_Damaged</DamageEffectName>
   <DamagedSound>ParticleHeavyMech</DamagedSound>
```

#### Tag: `Definition` | Type: `ConveyorConnector` | Subtype: `LargeBlockConveyorPipeFlange`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorPipes.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_IndustrialPack.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,14 +4,14 @@
     <SubtypeId>LargeBlockConveyorPipeFlange</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorPipeFlange</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\PipeFlange.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\PipeFlange.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <GuiVisible>false</GuiVisible>
   <CubeSize>Large</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="1" z="1" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Large\PipeFlange.mwm</Model>
+  <Model>Models\Cubes\large\PipeFlange.mwm</Model>
   <Components>
     <Component Subtype="InteriorPlate" Count="10" />
     <Component Subtype="Construction" Count="20" />
@@ -21,9 +21,9 @@
     </Components>
   <CriticalComponent Subtype="Motor" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\Large\PipeFlangeConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\Large\PipeFlangeConstruction_2.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Large\PipeFlangeConstruction_3.mwm" />
+    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\large\PipeFlange_BS1.mwm" />
+    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\large\PipeFlange_BS2.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\large\PipeFlange_BS3.mwm" />
     </BuildProgressModels>
   <BlockPairName>ConveyorPipeFlange</BlockPairName>
   <MirroringY>Z</MirroringY>
@@ -40,8 +40,6 @@
     <MountPoint Side="Top" StartX="0.2" StartY="0.3" EndX="0.3" EndY="0.7" />
     <MountPoint Side="Top" StartX="0.7" StartY="0.3" EndX="0.8" EndY="0.7" />
     </MountPoints>
-  <DamageEffectName>Damage_HeavyMech_Damaged</DamageEffectName>
-  <DamagedSound>ParticleHeavyMech</DamagedSound>
   <EmissiveColorPreset>Default</EmissiveColorPreset>
   <DamageEffectName>Damage_HeavyMech_Damaged</DamageEffectName>
   <DamagedSound>ParticleHeavyMech</DamagedSound>
```

#### Tag: `Definition` | Type: `ConveyorConnector` | Subtype: `LargeBlockConveyorPipeFlange3X`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorPipes.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_IndustrialPack.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,14 +4,14 @@
     <SubtypeId>LargeBlockConveyorPipeFlange3X</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorPipeFlange3X</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\PipeFlange3X.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\PipeFlange3X.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <GuiVisible>false</GuiVisible>
   <CubeSize>Large</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="3" z="1" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Large\PipeFlange3X.mwm</Model>
+  <Model>Models\Cubes\large\PipeFlange3x.mwm</Model>
   <Components>
     <Component Subtype="InteriorPlate" Count="30" />
     <Component Subtype="Construction" Count="60" />
@@ -21,9 +21,9 @@
     </Components>
   <CriticalComponent Subtype="Motor" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\Large\PipeFlange3XConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\Large\PipeFlange3XConstruction_2.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Large\PipeFlange3XConstruction_3.mwm" />
+    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\large\PipeFlange3x_BS1.mwm" />
+    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\large\PipeFlange3x_BS2.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\large\PipeFlange3x_BS3.mwm" />
     </BuildProgressModels>
   <BlockPairName>ConveyorPipeFlange3X</BlockPairName>
   <MirroringY>Z</MirroringY>
@@ -40,8 +40,6 @@
     <MountPoint Side="Top" StartX="0.2" StartY="0.3" EndX="0.3" EndY="0.7" />
     <MountPoint Side="Top" StartX="0.7" StartY="0.3" EndX="0.8" EndY="0.7" />
     </MountPoints>
-  <DamageEffectName>Damage_HeavyMech_Damaged</DamageEffectName>
-  <DamagedSound>ParticleHeavyMech</DamagedSound>
   <EmissiveColorPreset>Default</EmissiveColorPreset>
   <DamageEffectName>Damage_HeavyMech_Damaged</DamageEffectName>
   <DamagedSound>ParticleHeavyMech</DamagedSound>
```

#### Tag: `Definition` | Type: `ConveyorConnector` | Subtype: `LargeBlockConveyorPipeCorner`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorPipes.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_IndustrialPack.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,14 +4,14 @@
     <SubtypeId>LargeBlockConveyorPipeCorner</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorPipeCorner</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\PipeCorner.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\PipeCorner.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <GuiVisible>false</GuiVisible>
   <CubeSize>Large</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="1" z="1" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Large\PipeCorner.mwm</Model>
+  <Model>Models\Cubes\large\PipeCurved.mwm</Model>
   <Components>
     <Component Subtype="InteriorPlate" Count="10" />
     <Component Subtype="Construction" Count="20" />
@@ -21,9 +21,9 @@
     </Components>
   <CriticalComponent Subtype="Motor" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\Large\PipeCornerConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\Large\PipeCornerConstruction_2.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Large\PipeCornerConstruction_3.mwm" />
+    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\large\PipeCurved_BS1.mwm" />
+    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\large\PipeCurved_BS2.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\large\PipeCurved_BS3.mwm" />
     </BuildProgressModels>
   <BlockPairName>ConveyorPipeCorner</BlockPairName>
   <MirroringX>Y</MirroringX>
@@ -40,8 +40,6 @@
     <MountPoint Side="Right" StartX="0.2" StartY="0.3" EndX="0.3" EndY="0.7" />
     <MountPoint Side="Right" StartX="0.7" StartY="0.3" EndX="0.8" EndY="0.7" />
     </MountPoints>
-  <DamageEffectName>Damage_HeavyMech_Damaged</DamageEffectName>
-  <DamagedSound>ParticleHeavyMech</DamagedSound>
   <EmissiveColorPreset>Default</EmissiveColorPreset>
   <DamageEffectName>Damage_HeavyMech_Damaged</DamageEffectName>
   <DamagedSound>ParticleHeavyMech</DamagedSound>
```

#### Tag: `Definition` | Type: `Conveyor` | Subtype: `SmallBlockConveyor`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\Conveyors.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Logistics.sbc`

```diff
--- Base Game
+++ Mod
@@ -3,8 +3,8 @@
     <TypeId>Conveyor</TypeId>
     <SubtypeId>SmallBlockConveyor</SubtypeId>
     </Id>
-  <DisplayName>DisplayName_Block_ConveyorSmall</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\ConveyorSmall.dds</Icon>
+  <DisplayName>Small Conveyor Junction</DisplayName>
+  <Icon>Textures\GUI\Icons\cubes\ConveyorSmall.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <CubeSize>Small</CubeSize>
   <GuiVisible>false</GuiVisible>
```

#### Tag: `Definition` | Type: `Conveyor` | Subtype: `SmallShipConveyorHub`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\Conveyors.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Logistics.sbc`

```diff
--- Base Game
+++ Mod
@@ -3,8 +3,8 @@
     <TypeId>Conveyor</TypeId>
     <SubtypeId>SmallShipConveyorHub</SubtypeId>
     </Id>
-  <DisplayName>DisplayName_Block_MediumConveyor</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\SmallConveyor.dds</Icon>
+  <DisplayName>Conveyor Hub</DisplayName>
+  <Icon>Textures\GUI\Icons\cubes\SmallConveyor.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <CubeSize>Small</CubeSize>
   <GuiVisible>false</GuiVisible>
@@ -45,7 +45,7 @@
     <MountPoint Side="Front" StartX="0.35" StartY="0.25" EndX="2.65" EndY="0.35" />
     <MountPoint Side="Front" StartX="0.35" StartY="2.65" EndX="2.65" EndY="2.75" />
     </MountPoints>
-  <BlockPairName>Conveyor</BlockPairName>
+  <BlockPairName>ConveyorHub</BlockPairName>
   <MirroringZ>X</MirroringZ>
   <EdgeType>Light</EdgeType>
   <BuildTimeSeconds>14</BuildTimeSeconds>
```

#### Tag: `Definition` | Type: `ConveyorConnector` | Subtype: `ConveyorTube`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorTubes.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Logistics.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,14 +4,14 @@
     <SubtypeId>ConveyorTube</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorTube</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\ConveyorTube.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\ConveyorTube.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <GuiVisible>true</GuiVisible>
   <CubeSize>Large</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="1" z="1" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Large\ConveyorTube.mwm</Model>
+  <Model>Models\Cubes\large\ConveyorTube.mwm</Model>
   <Components>
     <Component Subtype="InteriorPlate" Count="10" />
     <Component Subtype="Construction" Count="20" />
@@ -21,8 +21,8 @@
     </Components>
   <CriticalComponent Subtype="Motor" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\Large\ConveyorTubeConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Large\ConveyorTubeConstruction_2.mwm" />
+    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\large\ConveyorTube_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\large\ConveyorTube_BS2.mwm" />
     </BuildProgressModels>
   <BlockPairName>ConveyorTube</BlockPairName>
   <EdgeType>Light</EdgeType>
```

#### Tag: `Definition` | Type: `ConveyorConnector` | Subtype: `ConveyorTube3X`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorTubes.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Logistics.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,14 +4,14 @@
     <SubtypeId>ConveyorTube3X</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorTube3X</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\ConveyorTube3X.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\ConveyorTube3X.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <GuiVisible>true</GuiVisible>
   <CubeSize>Large</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="3" z="1" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Large\ConveyorTube3X.mwm</Model>
+  <Model>Models\Cubes\large\ConveyorTube3x.mwm</Model>
   <Components>
     <Component Subtype="InteriorPlate" Count="30" />
     <Component Subtype="Construction" Count="60" />
@@ -21,8 +21,8 @@
     </Components>
   <CriticalComponent Subtype="Motor" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\Large\ConveyorTube3XConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Large\ConveyorTube3XConstruction_2.mwm" />
+    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\large\ConveyorTube3x_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\large\ConveyorTube3x_BS2.mwm" />
     </BuildProgressModels>
   <BlockPairName>ConveyorTube3X</BlockPairName>
   <EdgeType>Light</EdgeType>
```

#### Tag: `Definition` | Type: `ConveyorConnector` | Subtype: `ConveyorTubeCurved`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorTubes.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Logistics.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,14 +4,14 @@
     <SubtypeId>ConveyorTubeCurved</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorTubeCurved</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\ConveyorTubeCurved.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\ConveyorTubeCurved.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <GuiVisible>false</GuiVisible>
   <CubeSize>Large</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="1" z="1" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Large\ConveyorTubeCurved.mwm</Model>
+  <Model>Models\Cubes\large\ConveyorTubeCurved.mwm</Model>
   <Components>
     <Component Subtype="InteriorPlate" Count="10" />
     <Component Subtype="Construction" Count="20" />
@@ -21,8 +21,8 @@
     </Components>
   <CriticalComponent Subtype="Motor" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\Large\ConveyorTubeCurvedConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Large\ConveyorTubeCurvedConstruction_2.mwm" />
+    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\large\ConveyorTubeCurved_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\large\ConveyorTubeCurved_BS2.mwm" />
     </BuildProgressModels>
   <BlockPairName>ConveyorTubeCurved</BlockPairName>
   <EdgeType>Light</EdgeType>
```

#### Tag: `Definition` | Type: `Conveyor` | Subtype: `ConveyorTubeT`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorTubes.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Logistics.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,14 +4,14 @@
     <SubtypeId>ConveyorTubeT</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorTubeT</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\ConveyorTubeT.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\ConveyorTubeT.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <GuiVisible>false</GuiVisible>
   <CubeSize>Large</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="1" z="1" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Large\ConveyorTubeT.mwm</Model>
+  <Model>Models\Cubes\large\ConveyorTubeT.mwm</Model>
   <Components>
     <Component Subtype="InteriorPlate" Count="10" />
     <Component Subtype="Construction" Count="24" />
@@ -35,8 +35,8 @@
     <MountPoint Side="Left" StartX="0.7" StartY="0.3" EndX="0.8" EndY="0.7" />
     </MountPoints>
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\Large\ConveyorTubeTConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Large\ConveyorTubeTConstruction_2.mwm" />
+    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\large\ConveyorTubeT_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\large\ConveyorTubeT_BS2.mwm" />
     </BuildProgressModels>
   <BlockPairName>ConveyorTubeT</BlockPairName>
   <MirroringY>Z</MirroringY>
```

#### Tag: `Definition` | Type: `ConveyorConnector` | Subtype: `ConveyorTubeMedium`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorTubes.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Logistics.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,14 +4,14 @@
     <SubtypeId>ConveyorTubeMedium</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorTubeMedium</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\SmallConveyorTube.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\SmallConveyorTube.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <GuiVisible>false</GuiVisible>
   <CubeSize>Small</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="3" y="3" z="3" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Small\ConveyorTubeMedium.mwm</Model>
+  <Model>Models\Cubes\small\ConveyorTubeMedium.mwm</Model>
   <Components>
     <Component Subtype="InteriorPlate" Count="8" />
     <Component Subtype="Construction" Count="20" />
@@ -21,8 +21,8 @@
     </Components>
   <CriticalComponent Subtype="Motor" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\Small\ConveyorTubeMediumConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Small\ConveyorTubeMediumConstruction_2.mwm" />
+    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\small\ConveyorTubeMedium_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\small\ConveyorTubeMedium_BS2.mwm" />
     </BuildProgressModels>
   <BlockPairName>ConveyorTube</BlockPairName>
   <EdgeType>Light</EdgeType>
```

#### Tag: `Definition` | Type: `ConveyorConnector` | Subtype: `ConveyorTubeCurvedMedium`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorTubes.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Logistics.sbc`

```diff
--- Base Game
+++ Mod
@@ -3,15 +3,15 @@
     <TypeId>ConveyorConnector</TypeId>
     <SubtypeId>ConveyorTubeCurvedMedium</SubtypeId>
     </Id>
-  <DisplayName>DisplayName_Block_MediumConveyorTubeCurved</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\SmallConveyorTubeCurved.dds</Icon>
+  <DisplayName>Medium Curved Conveyor Tube</DisplayName>
+  <Icon>Textures\GUI\Icons\cubes\SmallConveyorTubeCurved.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <CubeSize>Small</CubeSize>
   <GuiVisible>false</GuiVisible>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="3" y="3" z="3" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Small\ConveyorTurnMedium.mwm</Model>
+  <Model>Models\Cubes\small\ConveyorTubeCurvedMedium.mwm</Model>
   <Components>
     <Component Subtype="InteriorPlate" Count="2" />
     <Component Subtype="Construction" Count="20" />
@@ -21,8 +21,8 @@
     </Components>
   <CriticalComponent Subtype="Motor" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\Small\ConveyorTurnMediumConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Small\ConveyorTurnMediumConstruction_2.mwm" />
+    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\small\ConveyorTubeCurvedMedium_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\small\ConveyorTubeCurvedMedium_BS2.mwm" />
     </BuildProgressModels>
   <MountPoints>
     <MountPoint Side="Bottom" StartX="0.4" StartY="0.2" EndX="2.6" EndY="0.4" />
```

#### Tag: `Definition` | Type: `ConveyorConnector` | Subtype: `ConveyorFrameMedium`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorTubes.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Logistics.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,14 +4,14 @@
     <SubtypeId>ConveyorFrameMedium</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorFrame</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\ConveyorFrameMedium.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\ConveyorFrameMedium.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <CubeSize>Small</CubeSize>
   <GuiVisible>false</GuiVisible>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="3" z="3" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Small\ConveyorFrameMedium.mwm</Model>
+  <Model>Models\Cubes\small\ConveyorFrameMedium.mwm</Model>
   <Components>
     <Component Subtype="InteriorPlate" Count="4" />
     <Component Subtype="Construction" Count="12" />
@@ -21,7 +21,8 @@
     </Components>
   <CriticalComponent Subtype="Motor" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Small\ConveyorFrameMediumConstruction_1.mwm" />
+    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\small\ConveyorFrameMedium_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\small\ConveyorFrameMedium_BS2.mwm" />
     </BuildProgressModels>
   <BlockPairName>ConveyorFrame</BlockPairName>
   <EdgeType>Light</EdgeType>
```

#### Tag: `Definition` | Type: `ConveyorConnector` | Subtype: `ConveyorTubeSmall`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorTubes.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Logistics.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,13 +4,13 @@
     <SubtypeId>ConveyorTubeSmall</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorTubeSmall</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\ConveyorTubeSmall.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\ConveyorTubeSmall.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <CubeSize>Small</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="1" z="1" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Small\ConveyorTube.mwm</Model>
+  <Model>Models\Cubes\small\ConveyorTubeSmall.mwm</Model>
   <Components>
     <Component Subtype="InteriorPlate" Count="1" />
     <Component Subtype="Motor" Count="1" />
@@ -27,9 +27,9 @@
     <MountPoint Side="Front" StartX="0.2" StartY="0.1" EndX="0.8" EndY="0.2" />
     <MountPoint Side="Front" StartX="0.2" StartY="0.8" EndX="0.8" EndY="0.9" />
     </MountPoints>
-  <CriticalComponent Subtype="Construction" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Small\ConveyorTubeConstruction_1.mwm" />
+    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\small\ConveyorTubeSmall_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\small\ConveyorTubeSmall_BS2.mwm" />
     </BuildProgressModels>
   <BlockPairName>ConveyorTubeSmall</BlockPairName>
   <EdgeType>Light</EdgeType>
```

#### Tag: `Definition` | Type: `ConveyorConnector` | Subtype: `ConveyorTubeSmall3X`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorTubes.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Logistics.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,13 +4,13 @@
     <SubtypeId>ConveyorTubeSmall3X</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorTubeSmall3X</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\ConveyorTube3X_Small.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\ConveyorTube3X_Small.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <CubeSize>Small</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="1" z="3" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Small\ConveyorTube3X.mwm</Model>
+  <Model>Models\Cubes\small\ConveyorTubeSmall3x.mwm</Model>
   <Components>
     <Component Subtype="InteriorPlate" Count="3" />
     <Component Subtype="Motor" Count="3" />
@@ -27,9 +27,9 @@
     <MountPoint Side="Front" StartX="0.2" StartY="0.1" EndX="0.8" EndY="0.2" />
     <MountPoint Side="Front" StartX="0.2" StartY="0.8" EndX="0.8" EndY="0.9" />
     </MountPoints>
-  <CriticalComponent Subtype="Construction" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Small\ConveyorTube3XConstruction_1.mwm" />
+    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\small\ConveyorTubeSmall3x_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\small\ConveyorTubeSmall3x_BS2.mwm" />
     </BuildProgressModels>
   <BlockPairName>ConveyorTubeSmall3X</BlockPairName>
   <EdgeType>Light</EdgeType>
```

#### Tag: `Definition` | Type: `ConveyorConnector` | Subtype: `ConveyorTubeSmallCurved`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorTubes.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Logistics.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,14 +4,14 @@
     <SubtypeId>ConveyorTubeSmallCurved</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorTubeCurvedSmall</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\ConveyorCurveSmall.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\ConveyorCurveSmall.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <GuiVisible>false</GuiVisible>
   <CubeSize>Small</CubeSize>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="1" z="1" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Small\ConveyorTubeCurved.mwm</Model>
+  <Model>Models\Cubes\small\ConveyorTubeSmallCurved.mwm</Model>
   <Components>
     <Component Subtype="InteriorPlate" Count="1" />
     <Component Subtype="Motor" Count="1" />
@@ -19,7 +19,8 @@
     </Components>
   <CriticalComponent Subtype="Construction" Index="0" />
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Small\ConveyorTubeCurvedConstruction_1.mwm" />
+    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\small\ConveyorTubeSmallCurved_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\small\ConveyorTubeSmallCurved_BS2.mwm" />
     </BuildProgressModels>
   <MountPoints>
     <MountPoint Side="Bottom" StartX="0" StartY="0" EndX="1" EndY="1" />
```

#### Tag: `Definition` | Type: `Conveyor` | Subtype: `ConveyorTubeSmallT`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Conveyor Expansion\Data\Cubeblocks\ConveyorTubes.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Logistics.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,14 +4,14 @@
     <SubtypeId>ConveyorTubeSmallT</SubtypeId>
     </Id>
   <DisplayName>DisplayName_Block_ConveyorTubeTSmall</DisplayName>
-  <Icon>Textures\GUI\Icons\Cubes\ConveyorTubeT_Small.dds</Icon>
+  <Icon>Textures\GUI\Icons\cubes\ConveyorTubeT_Small.dds</Icon>
   <Description>Description_ConveyorTube</Description>
   <CubeSize>Small</CubeSize>
   <GuiVisible>false</GuiVisible>
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="1" z="1" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Small\ConveyorTubeT.mwm</Model>
+  <Model>Models\Cubes\small\ConveyorTubeSmallT.mwm</Model>
   <Components>
     <Component Subtype="InteriorPlate" Count="2" />
     <Component Subtype="Motor" Count="1" />
@@ -24,7 +24,8 @@
     <MountPoint Side="Front" StartX="0" StartY="0" EndX="1" EndY="1" />
     </MountPoints>
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Small\ConveyorTubeTConstruction_1.mwm" />
+    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\small\ConveyorTubeSmallT_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\small\ConveyorTubeSmallT_BS2.mwm" />
     </BuildProgressModels>
   <BlockPairName>ConveyorTubeTSmall</BlockPairName>
   <MirroringY>Z</MirroringY>
```

## Mod: JS - Corridor Stair (7 differences)

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section1_Position1_LargeBlocks`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Corridor Stair\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -7,955 +7,7 @@
   <Name>Section1_Position1_LargeBlocks</Name>
   <StrictSearch>true</StrictSearch>
   <ItemIds>
+    <string>InteriorLight/JS_CorridorStair</string>
+    <string>CubeBlock/JS_CorridorStairWindow</string>
     </ItemIds>
   </Category>
```

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section1_Position2_Nonarmor`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Corridor Stair\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -7,1385 +7,7 @@
   <Name>Section1_Position2_Nonarmor</Name>
   <StrictSearch>true</StrictSearch>
   <ItemIds>
+    <string>InteriorLight/JS_CorridorStair</string>
+    <string>CubeBlock/JS_CorridorStairWindow</string>
     </ItemIds>
   </Category>
```

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section1_Position3_ShipStructure`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Corridor Stair\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -7,457 +7,7 @@
   <Name>Section1_Position3_ShipStructure</Name>
   <StrictSearch>true</StrictSearch>
   <ItemIds>
+    <string>InteriorLight/JS_CorridorStair</string>
+    <string>CubeBlock/JS_CorridorStairWindow</string>
     </ItemIds>
   </Category>
```

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section1_Position3_Utility`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Corridor Stair\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -7,193 +7,6 @@
   <Name>Section1_Position3_Utility</Name>
   <StrictSearch>true</StrictSearch>
   <ItemIds>
+    <string>InteriorLight/JS_CorridorStair</string>
     </ItemIds>
   </Category>
```

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section1_Position3_Windows`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Corridor Stair\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -7,140 +7,6 @@
   <Name>Section1_Position3_Windows</Name>
   <StrictSearch>true</StrictSearch>
   <ItemIds>
+    <string>CubeBlock/JS_CorridorStairWindow</string>
     </ItemIds>
   </Category>
```

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section2_DLC`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Corridor Stair\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -6,719 +6,8 @@
   <DisplayName>DisplayName_Category_DLCBlocks</DisplayName>
   <Name>Section2_DLC</Name>
   <StrictSearch>true</StrictSearch>
   <ItemIds>
+    <string>InteriorLight/JS_CorridorStair</string>
+    <string>CubeBlock/JS_CorridorStairWindow</string>
     </ItemIds>
   </Category>
```

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section2_DLC_Signal`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Corridor Stair\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -6,61 +6,8 @@
   <DisplayName>DisplayName_Category_DLC_Signal</DisplayName>
   <Name>Section2_DLC_Signal</Name>
   <StrictSearch>true</StrictSearch>
   <ItemIds>
+    <string>InteriorLight/JS_CorridorStair</string>
+    <string>CubeBlock/JS_CorridorStairWindow</string>
     </ItemIds>
   </Category>
```

## Mod: JS - Lighting Overhaul (2 differences)

#### Tag: `Definition` | Type: `EnvironmentDefinition` | Subtype: `Default`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Lighting Overhaul\Data\Environment.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Environment.sbc`

```diff
--- Base Game
+++ Mod
@@ -3,35 +3,13 @@
     <TypeId>EnvironmentDefinition</TypeId>
     <SubtypeId>Default</SubtypeId>
     </Id>
-  <SmallShipMaxSpeed>100</SmallShipMaxSpeed>
-  <LargeShipMaxSpeed>100</LargeShipMaxSpeed>
-  <SmallShipMaxAngularSpeed>360</SmallShipMaxAngularSpeed>
-  <LargeShipMaxAngularSpeed>180</LargeShipMaxAngularSpeed>
-  <FloatingObjectLinearDampening>0.1</FloatingObjectLinearDampening>
-  <FloatingObjectLinearDampeningInSafezone>1.5</FloatingObjectLinearDampeningInSafezone>
   <PlanetProperties>
     <AtmosphereIntensityMultiplier>17</AtmosphereIntensityMultiplier>
-    <AtmosphereIntensityAmbientMultiplier>3</AtmosphereIntensityAmbientMultiplier>
-    <CloudsIntensityMultiplier>15</CloudsIntensityMultiplier>
+    <AtmosphereIntensityAmbientMultiplier>1.9</AtmosphereIntensityAmbientMultiplier>
+    <CloudsIntensityMultiplier>13</CloudsIntensityMultiplier>
     </PlanetProperties>
-  <FogProperties>
-    <FogMultiplier>0</FogMultiplier>
-    <FogDensity>0.003</FogDensity>
-    <FogColor>
-      <X>0</X>
-      <Y>0</Y>
-      <Z>0</Z>
-      </FogColor>
-    </FogProperties>
   <SunProperties>
-    <SunIntensity>50</SunIntensity>
-    <EnvironmentProbe>
-      <TimeOut>0.3</TimeOut>
-      <DrawDistance>130</DrawDistance>
-      <AmbientScale>0.1</AmbientScale>
-      <AmbientMinClamp>0.02</AmbientMinClamp>
-      <AmbientMaxClamp>0.3</AmbientMaxClamp>
-      </EnvironmentProbe>
+    <SunIntensity>80</SunIntensity>
     <EnvironmentLight>
       <SunColor>
         <X>1</X>
@@ -48,23 +26,23 @@
       <AmbientGlobalDensity>0</AmbientGlobalDensity>
       <AmbientGlobalMultiplier>0</AmbientGlobalMultiplier>
       <SunDiscColor>
-        <X>0.88</X>
-        <Y>0.53</Y>
-        <Z>0.23</Z>
+        <X>0.99</X>
+        <Y>0.95</Y>
+        <Z>0.90</Z>
         </SunDiscColor>
       <SunDiscColor2>
         <X>1.0</X>
         <Y>1.0</Y>
         <Z>1.0</Z>
         </SunDiscColor2>
-      <SunDiscInnerDot>0.9992</SunDiscInnerDot>
-      <SunDiscOuterDot>0.99875</SunDiscOuterDot>
+      <SunDiscInnerDot>0.999</SunDiscInnerDot>
+      <SunDiscOuterDot>0.9987</SunDiscOuterDot>
       <AODirLight>1</AODirLight>
       <AOIndirectLight>1</AOIndirectLight>
       <AOPointLight>0.7</AOPointLight>
       <AOSpotLight>0.7</AOSpotLight>
-      <SkyboxBrightness>0.40</SkyboxBrightness>
-      <EnvSkyboxBrightness>2</EnvSkyboxBrightness>
+      <SkyboxBrightness>0.4</SkyboxBrightness>
+      <EnvSkyboxBrightness>3</EnvSkyboxBrightness>
       <ShadowFadeoutMultiplier>0.0</ShadowFadeoutMultiplier>
       <EnvShadowFadeoutMultiplier>0</EnvShadowFadeoutMultiplier>
       <EnvAtmosphereBrightness>1.0</EnvAtmosphereBrightness>
@@ -72,26 +50,8 @@
       <AmbientLightsGatherRadius>15.0</AmbientLightsGatherRadius>
       <SunDiscIntensity>3.8</SunDiscIntensity>
       </EnvironmentLight>
-    <TextureMultipliers>
-      <GlossMultiplier>1.0</GlossMultiplier>
-      </TextureMultipliers>
     </SunProperties>
   <PostProcessSettings>
-    <EnableTonemapping>true</EnableTonemapping>
-    <EnableEyeAdaptation>true</EnableEyeAdaptation>
-    <HighQualityBloom>true</HighQualityBloom>
-    <BloomAntiFlickerFilter>true</BloomAntiFlickerFilter>
-    <BloomSize>4</BloomSize>
-    <HistogramLogMin>-6</HistogramLogMin>
-    <HistogramLogMax>6</HistogramLogMax>
-    <HistogramFilterMin>25</HistogramFilterMin>
-    <HistogramFilterMax>98</HistogramFilterMax>
-    <HistogramSkyboxFactor>0.25</HistogramSkyboxFactor>
-    <MinEyeAdaptationLogBrightness>-5.0</MinEyeAdaptationLogBrightness>
-    <MaxEyeAdaptationLogBrightness>5.0</MaxEyeAdaptationLogBrightness>
-    <DirtTexture>Textures\SunGlare\Dirt.dds</DirtTexture>
-    <ChromaticIterations>1</ChromaticIterations>
-    <EyeAdaptationPrioritizeScreenCenter>true</EyeAdaptationPrioritizeScreenCenter>
     <Data>
       <Contrast>1.0</Contrast>
       <Brightness>1</Brightness>
@@ -114,332 +74,30 @@
       <EyeAdaptationTau>0.4</EyeAdaptationTau>
       <BloomExposure>5.04930735</BloomExposure>
       <BloomLumaThreshold>5.0</BloomLumaThreshold>
-      <BloomMult>0.02</BloomMult>
-      <BloomEmissiveness>120</BloomEmissiveness>
+      <BloomMult>0.0055</BloomMult>
+      <BloomEmissiveness>50</BloomEmissiveness>
       <BloomDepthStrength>2.0</BloomDepthStrength>
       <BloomDepthSlope>0.3</BloomDepthSlope>
       <LogLumThreshold>-4</LogLumThreshold>
       <EyeAdaptationSpeedUp>2</EyeAdaptationSpeedUp>
       <EyeAdaptationSpeedDown>0.5</EyeAdaptationSpeedDown>
       <WhitePoint>11.2</WhitePoint>
-      <BloomDirtRatio>0.75</BloomDirtRatio>
+      <BloomDirtRatio>0.01</BloomDirtRatio>
       <GrainSize>0</GrainSize>
       <GrainAmount>0.5</GrainAmount>
       <GrainStrength>0.0</GrainStrength>
-      <ChromaticFactor>0.02</ChromaticFactor>
+      <ChromaticFactor>0.0001</ChromaticFactor>
       <Vibrance>0.2</Vibrance>
       <VignetteStart>0</VignetteStart>
       <VignetteLength>1.25</VignetteLength>
       </Data>
     </PostProcessSettings>
-  <SSAOSettings>
-    <Data>
-      <MinRadius>0.115</MinRadius>
-      <MaxRadius>25</MaxRadius>
-      <RadiusGrowZScale>1.007</RadiusGrowZScale>
-      <Falloff>3.08</Falloff>
-      <RadiusBias>0.25</RadiusBias>
-      <Contrast>2.617</Contrast>
-      <Normalization>0.075</Normalization>
-      <ColorScale>0.6</ColorScale>
-      </Data>
-    </SSAOSettings>
-  <HBAOSettings>
-    <Enabled>true</Enabled>
-    <Radius>2.0</Radius>
-    <Bias>0.2</Bias>
-    <UseGBufferNormals>true</UseGBufferNormals>
-    <SmallScaleAO>1.0</SmallScaleAO>
-    <LargeScaleAO>1.5</LargeScaleAO>
-    <PowerExponent>4.0</PowerExponent>
-    <ForegroundAOEnable>true</ForegroundAOEnable>
-    <ForegroundViewDepth>66.0</ForegroundViewDepth>
-    <BackgroundAOEnable>true</BackgroundAOEnable>
-    <BackgroundViewDepth>200</BackgroundViewDepth>
-    <DepthClampToEdge>false</DepthClampToEdge>
-    <DepthThresholdEnable>false</DepthThresholdEnable>
-    <DepthThreshold>0</DepthThreshold>
-    <DepthThresholdSharpness>100</DepthThresholdSharpness>
-    <BlurEnable>true</BlurEnable>
-    <BlurRadius4>false</BlurRadius4>
-    <BlurSharpness>50.0</BlurSharpness>
-    <BlurSharpnessFunctionEnable>false</BlurSharpnessFunctionEnable>
-    <BlurSharpnessFunctionForegroundScale>4.0</BlurSharpnessFunctionForegroundScale>
-    <BlurSharpnessFunctionForegroundViewDepth>0</BlurSharpnessFunctionForegroundViewDepth>
-    <BlurSharpnessFunctionBackgroundViewDepth>1.0</BlurSharpnessFunctionBackgroundViewDepth>
-    </HBAOSettings>
-  <ShadowSettings>
-    <Data>
-      <ShowShadowCascadeSplits>false</ShowShadowCascadeSplits>
-      <UpdateCascadesEveryFrame>false</UpdateCascadesEveryFrame>
-      <EnableShadowBlur>true</EnableShadowBlur>
-      <ShadowCascadeMaxDistance>3000</ShadowCascadeMaxDistance>
-      <ShadowCascadeMaxDistanceMultiplierMedium>5</ShadowCascadeMaxDistanceMultiplierMedium>
-      <ShadowCascadeMaxDistanceMultiplierHigh>9</ShadowCascadeMaxDistanceMultiplierHigh>
-      <ShadowCascadeMaxDistanceMultiplierExtreme>10</ShadowCascadeMaxDistanceMultiplierExtreme>
-      <ShadowCascadeSpreadFactor>0</ShadowCascadeSpreadFactor>
-      <ShadowCascadeZOffset>40000</ShadowCascadeZOffset>
-      <LightDirectionChangeDelayMultiplier>18</LightDirectionChangeDelayMultiplier>
-      <DisplayFrozenShadowCascade>false</DisplayFrozenShadowCascade>
-      <ReflectorShadowDistanceLow>0</ReflectorShadowDistanceLow>
-      <ReflectorShadowDistanceMedium>0.4</ReflectorShadowDistanceMedium>
-      <ReflectorShadowDistanceHigh>1.0</ReflectorShadowDistanceHigh>
-      <ReflectorShadowDistanceExtreme>1.0</ReflectorShadowDistanceExtreme>
-      <LightDirectionDifferenceThreshold>0.0175</LightDirectionDifferenceThreshold>
-      <ZBias>0.01</ZBias>
-      <CascadesCount>6</CascadesCount>
-      </Data>
-    </ShadowSettings>
-  <LowLoddingSettings>
-    <GBuffer>
-      <LodShift>0</LodShift>
-      <MinLod>1</MinLod>
-      </GBuffer>
-    <CascadeDepths>
-      <CascadeDepth>
-        <LodShiftVisible>1</LodShiftVisible>
-        <LodShift>3</LodShift>
-        <MinLod>1</MinLod>
-        </CascadeDepth>
-      <CascadeDepth>
-        <LodShiftVisible>1</LodShiftVisible>
-        <LodShift>3</LodShift>
-        <MinLod>1</MinLod>
-        </CascadeDepth>
-      <CascadeDepth>
-        <LodShiftVisible>1</LodShiftVisible>
-        <LodShift>3</LodShift>
-        <MinLod>1</MinLod>
-        </CascadeDepth>
-      <CascadeDepth>
-        <LodShiftVisible>3</LodShiftVisible>
-        <LodShift>3</LodShift>
-        <MinLod>1</MinLod>
-        </CascadeDepth>
-      <CascadeDepth>
-        <LodShiftVisible>3</LodShiftVisible>
-        <LodShift>3</LodShift>
-        <MinLod>1</MinLod>
-        </CascadeDepth>
-      <CascadeDepth>
-        <LodShiftVisible>3</LodShiftVisible>
-        <LodShift>3</LodShift>
-        <MinLod>1</MinLod>
-        </CascadeDepth>
-      </CascadeDepths>
-    <SingleDepth>
-      <LodShiftVisible>1</LodShiftVisible>
-      <LodShift>3</LodShift>
-      <MinLod>1</MinLod>
-      </SingleDepth>
-    <Forward>
-      <LodShiftVisible>10</LodShiftVisible>
-      <LodShift>10</LodShift>
-      <MinLod>10</MinLod>
-      </Forward>
-    <Global>
-      <ObjectDistanceAdd>0</ObjectDistanceAdd>
-      <ObjectDistanceMult>4</ObjectDistanceMult>
-      <MinTransitionInSeconds>0.2</MinTransitionInSeconds>
-      <MaxTransitionInSeconds>0.4</MaxTransitionInSeconds>
-      <TransitionDeadZoneConst>2.0</TransitionDeadZoneConst>
-      <TransitionDeadZoneDistanceMult>0.5</TransitionDeadZoneDistanceMult>
-      <IsUpdateEnabled>true</IsUpdateEnabled>
-      <MaxDistanceForSmoothCameraMovement>11</MaxDistanceForSmoothCameraMovement>
-      <HisteresisRatio>0.1</HisteresisRatio>
-      </Global>
-    </LowLoddingSettings>
-  <MediumLoddingSettings>
-    <GBuffer>
-      <LodShift>0</LodShift>
-      <MinLod>1</MinLod>
-      </GBuffer>
-    <CascadeDepths>
-      <CascadeDepth>
-        <LodShift>2</LodShift>
-        <MinLod>1</MinLod>
-        </CascadeDepth>
-      <CascadeDepth>
-        <LodShift>2</LodShift>
-        <MinLod>1</MinLod>
-        </CascadeDepth>
-      <CascadeDepth>
-        <LodShift>2</LodShift>
-        <MinLod>1</MinLod>
-        </CascadeDepth>
-      <CascadeDepth>
-        <LodShift>2</LodShift>
-        <MinLod>1</MinLod>
-        </CascadeDepth>
-      <CascadeDepth>
-        <LodShift>2</LodShift>
-        <MinLod>1</MinLod>
-        </CascadeDepth>
-      <CascadeDepth>
-        <LodShift>2</LodShift>
-        <MinLod>1</MinLod>
-        </CascadeDepth>
-      </CascadeDepths>
-    <SingleDepth>
-      <LodShift>2</LodShift>
-      <MinLod>1</MinLod>
-      </SingleDepth>
-    <Forward>
-      <LodShiftVisible>10</LodShiftVisible>
-      <LodShift>10</LodShift>
-      <MinLod>10</MinLod>
-      </Forward>
-    <Global>
-      <ObjectDistanceAdd>-5</ObjectDistanceAdd>
-      <ObjectDistanceMult>2</ObjectDistanceMult>
-      <MinTransitionInSeconds>0.2</MinTransitionInSeconds>
-      <MaxTransitionInSeconds>0.4</MaxTransitionInSeconds>
-      <TransitionDeadZoneConst>2.0</TransitionDeadZoneConst>
-      <TransitionDeadZoneDistanceMult>0.5</TransitionDeadZoneDistanceMult>
-      <IsUpdateEnabled>true</IsUpdateEnabled>
-      <MaxDistanceForSmoothCameraMovement>11</MaxDistanceForSmoothCameraMovement>
-      <HisteresisRatio>0.1</HisteresisRatio>
-      </Global>
-    </MediumLoddingSettings>
-  <HighLoddingSettings>
-    <GBuffer>
-      <LodShift>0</LodShift>
-      <MinLod>0</MinLod>
-      </GBuffer>
-    <CascadeDepths>
-      <CascadeDepth>
-        <LodShift>2</LodShift>
-        <MinLod>0</MinLod>
-        </CascadeDepth>
-      <CascadeDepth>
-        <LodShift>2</LodShift>
-        <MinLod>0</MinLod>
-        </CascadeDepth>
-      <CascadeDepth>
-        <LodShift>2</LodShift>
-        <MinLod>0</MinLod>
-        </CascadeDepth>
-      <CascadeDepth>
-        <LodShift>2</LodShift>
-        <MinLod>0</MinLod>
-        </CascadeDepth>
-      <CascadeDepth>
-        <LodShift>2</LodShift>
-        <MinLod>0</MinLod>
-        </CascadeDepth>
-      <CascadeDepth>
-        <LodShift>2</LodShift>
-        <MinLod>0</MinLod>
-        </CascadeDepth>
-      </CascadeDepths>
-    <SingleDepth>
-      <LodShift>2</LodShift>
-      <MinLod>0</MinLod>
-      </SingleDepth>
-    <Forward>
-      <LodShiftVisible>10</LodShiftVisible>
-      <LodShift>10</LodShift>
-      <MinLod>10</MinLod>
-      </Forward>
-    <Global>
-      <ObjectDistanceAdd>0</ObjectDistanceAdd>
-      <ObjectDistanceMult>1</ObjectDistanceMult>
-      <MinTransitionInSeconds>0.25</MinTransitionInSeconds>
-      <MaxTransitionInSeconds>1</MaxTransitionInSeconds>
-      <TransitionDeadZoneConst>2.0</TransitionDeadZoneConst>
-      <TransitionDeadZoneDistanceMult>0.5</TransitionDeadZoneDistanceMult>
-      <IsUpdateEnabled>true</IsUpdateEnabled>
-      <MaxDistanceForSmoothCameraMovement>11</MaxDistanceForSmoothCameraMovement>
-      <HisteresisRatio>0.1</HisteresisRatio>
-      </Global>
-    </HighLoddingSettings>
-  <ExtremeLoddingSettings>
-    <GBuffer>
-      <LodShift>0</LodShift>
-      <MinLod>0</MinLod>
-      </GBuffer>
-    <CascadeDepths>
-      <CascadeDepth>
-        <LodShift>0</LodShift>
-        <MinLod>0</MinLod>
-        </CascadeDepth>
-      <CascadeDepth>
-        <LodShift>0</LodShift>
-        <MinLod>0</MinLod>
-        </CascadeDepth>
-      <CascadeDepth>
-        <LodShift>0</LodShift>
-        <MinLod>0</MinLod>
-        </CascadeDepth>
-      <CascadeDepth>
-        <LodShift>0</LodShift>
-        <MinLod>0</MinLod>
-        </CascadeDepth>
-      <CascadeDepth>
-        <LodShift>0</LodShift>
-        <MinLod>0</MinLod>
-        </CascadeDepth>
-      <CascadeDepth>
-        <LodShift>0</LodShift>
-        <MinLod>0</MinLod>
-        </CascadeDepth>
-      </CascadeDepths>
-    <SingleDepth>
-      <LodShift>0</LodShift>
-      <MinLod>0</MinLod>
-      </SingleDepth>
-    <Forward>
-      <LodShiftVisible>10</LodShiftVisible>
-      <LodShift>10</LodShift>
-      <MinLod>10</MinLod>
-      </Forward>
-    <Global>
-      <ObjectDistanceAdd>0</ObjectDistanceAdd>
-      <ObjectDistanceMult>0</ObjectDistanceMult>
-      <MinTransitionInSeconds>0.25</MinTransitionInSeconds>
-      <MaxTransitionInSeconds>1</MaxTransitionInSeconds>
-      <TransitionDeadZoneConst>2.0</TransitionDeadZoneConst>
-      <TransitionDeadZoneDistanceMult>0.5</TransitionDeadZoneDistanceMult>
-      <IsUpdateEnabled>true</IsUpdateEnabled>
-      <MaxDistanceForSmoothCameraMovement>11</MaxDistanceForSmoothCameraMovement>
-      <HisteresisRatio>0.1</HisteresisRatio>
-      </Global>
-    </ExtremeLoddingSettings>
-  <EnvironmentTexture>Textures\BackgroundCube\Final\BackgroundCube.dds</EnvironmentTexture>
-  <EnvironmentOrientation Yaw="60.3955536" Pitch="-61.1861954" Roll="90.90578" />
+  <EnvironmentTexture>Textures\BackgroundCube\Final\JS_LightingOverhaul_Sky.dds</EnvironmentTexture>
   <ContourHighlightColor>
     <X>1</X>
     <Y>1</Y>
     <Z>0</Z>
-    <W>0.05</W>
+    <W>0</W>
     </ContourHighlightColor>
-  <ContourHighlightThickness>7.5</ContourHighlightThickness>
-  <HighlightPulseInSeconds>3</HighlightPulseInSeconds>
-  <EnvironmentalParticles>
-    <ParticleType>
-      <Id>
-        <TypeId>MyObjectBuilder_EnvironmentalParticleLogicSpace</TypeId>
-        <SubtypeId>SpaceParticles</SubtypeId>
-        </Id>
-      <Material>Stardust</Material>
-      <Color>
-        <X>0.375</X>
-        <Y>0.375</Y>
-        <Z>0.375</Z>
-        <W>0.375</W>
-        </Color>
-      <MaterialPlanet>Stardust</MaterialPlanet>
-      <ColorPlanet>
-        <X>1.0</X>
-        <Y>1.0</Y>
-        <Z>1.0</Z>
-        <W>0.5</W>
-        </ColorPlanet>
-      <MaxSpawnDistance>10</MaxSpawnDistance>
-      <DespawnDistance>15</DespawnDistance>
-      <Density>0.0000235</Density>
-      <MaxLifeTime>1500</MaxLifeTime>
-      <MaxParticles>128</MaxParticles>
-      </ParticleType>
-    </EnvironmentalParticles>
+  <ContourHighlightThickness>4</ContourHighlightThickness>
   </Definition>
```

#### Tag: `Definition` | Type: `FlareDefinition` | Subtype: `Sun`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Lighting Overhaul\Data\flares.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\flares.sbc`

```diff
--- Base Game
+++ Mod
@@ -9,14 +9,14 @@
       <Type>Rotated</Type>
       <Material>SunBigGlow</Material>
       <Color>
-        <X>0.76</X>
-        <Y>0.58</Y>
-        <Z>0.38</Z>
-        <W>0.025</W>
-        </Color>
-      <Size>
-        <X>43000</X>
-        <Y>43000</Y>
+        <X>0.626</X>
+        <Y>0.601</Y>
+        <Z>0.569</Z>
+        <W>0.025</W>
+        </Color>
+      <Size>
+        <X>63000</X>
+        <Y>63000</Y>
         </Size>
       <ScreenIntensityMultiplierCenter>1</ScreenIntensityMultiplierCenter>
       <ScreenIntensityMultiplierEdge>1</ScreenIntensityMultiplierEdge>
@@ -29,9 +29,9 @@
       <Type>Oriented</Type>
       <Material>SunBigRays</Material>
       <Color>
-        <X>0.76</X>
-        <Y>0.58</Y>
-        <Z>0.38</Z>
+        <X>0.626</X>
+        <Y>0.601</Y>
+        <Z>0.569</Z>
         <W>0.025</W>
         </Color>
       <Size>
@@ -59,9 +59,9 @@
       <Type>Rotated</Type>
       <Material>SunBigRays</Material>
       <Color>
-        <X>0.66</X>
-        <Y>0.48</Y>
-        <Z>0.28</Z>
+        <X>0.522</X>
+        <Y>0.501</Y>
+        <Z>0.475</Z>
         <W>0.01</W>
         </Color>
       <Size>
@@ -79,9 +79,9 @@
       <Type>Rotated</Type>
       <Material>SunBigRays</Material>
       <Color>
-        <X>0.66</X>
-        <Y>0.48</Y>
-        <Z>0.28</Z>
+        <X>0.522</X>
+        <Y>0.501</Y>
+        <Z>0.475</Z>
         <W>0.01</W>
         </Color>
       <Size>
@@ -109,9 +109,9 @@
       <Type>Oriented</Type>
       <Material>SunFlareBlurred</Material>
       <Color>
-        <X>0.56</X>
-        <Y>0.38</Y>
-        <Z>0.18</Z>
+        <X>0.419</X>
+        <Y>0.402</Y>
+        <Z>0.381</Z>
         <W>0.0025</W>
         </Color>
       <Size>
@@ -129,9 +129,9 @@
       <Type>Oriented</Type>
       <Material>SunFlareBlurred</Material>
       <Color>
-        <X>0.56</X>
-        <Y>0.38</Y>
-        <Z>0.18</Z>
+        <X>0.419</X>
+        <Y>0.402</Y>
+        <Z>0.381</Z>
         <W>0.0025</W>
         </Color>
       <Size>
@@ -149,9 +149,9 @@
       <Type>Oriented</Type>
       <Material>SunFlareBlurred</Material>
       <Color>
-        <X>0.56</X>
-        <Y>0.38</Y>
-        <Z>0.18</Z>
+        <X>0.419</X>
+        <Y>0.402</Y>
+        <Z>0.381</Z>
         <W>0.0025</W>
         </Color>
       <Size>
@@ -169,9 +169,9 @@
       <Type>AnamorphicInverted</Type>
       <Material>SunFlareSquare</Material>
       <Color>
-        <X>0.76</X>
-        <Y>0.58</Y>
-        <Z>0.38</Z>
+        <X>0.626</X>
+        <Y>0.601</Y>
+        <Z>0.569</Z>
         <W>0.025</W>
         </Color>
       <Size>
@@ -189,9 +189,9 @@
       <Type>AnamorphicInverted</Type>
       <Material>SunFlareSquare</Material>
       <Color>
-        <X>0.76</X>
-        <Y>0.58</Y>
-        <Z>0.38</Z>
+        <X>0.626</X>
+        <Y>0.601</Y>
+        <Z>0.569</Z>
         <W>0.025</W>
         </Color>
       <Size>
@@ -209,9 +209,9 @@
       <Type>AnamorphicInverted</Type>
       <Material>SunFlareSquare</Material>
       <Color>
-        <X>0.76</X>
-        <Y>0.58</Y>
-        <Z>0.38</Z>
+        <X>0.626</X>
+        <Y>0.601</Y>
+        <Z>0.569</Z>
         <W>0.025</W>
         </Color>
       <Size>
@@ -229,9 +229,9 @@
       <Type>AnamorphicInverted</Type>
       <Material>SunFlareSquare</Material>
       <Color>
-        <X>0.76</X>
-        <Y>0.58</Y>
-        <Z>0.38</Z>
+        <X>0.626</X>
+        <Y>0.601</Y>
+        <Z>0.569</Z>
         <W>0.025</W>
         </Color>
       <Size>
@@ -249,9 +249,9 @@
       <Type>AnamorphicInverted</Type>
       <Material>SunFlareSquare</Material>
       <Color>
-        <X>0.76</X>
-        <Y>0.58</Y>
-        <Z>0.38</Z>
+        <X>0.626</X>
+        <Y>0.601</Y>
+        <Z>0.569</Z>
         <W>0.025</W>
         </Color>
       <Size>
@@ -269,9 +269,9 @@
       <Type>AnamorphicInverted</Type>
       <Material>SunFlareSquare</Material>
       <Color>
-        <X>0.76</X>
-        <Y>0.58</Y>
-        <Z>0.38</Z>
+        <X>0.626</X>
+        <Y>0.601</Y>
+        <Z>0.569</Z>
         <W>0.025</W>
         </Color>
       <Size>
@@ -289,9 +289,9 @@
       <Type>AnamorphicInverted</Type>
       <Material>SunFlareSquare</Material>
       <Color>
-        <X>0.76</X>
-        <Y>0.58</Y>
-        <Z>0.38</Z>
+        <X>0.626</X>
+        <Y>0.601</Y>
+        <Z>0.569</Z>
         <W>0.025</W>
         </Color>
       <Size>
@@ -309,9 +309,9 @@
       <Type>AnamorphicInverted</Type>
       <Material>SunFlareSquare</Material>
       <Color>
-        <X>0.76</X>
-        <Y>0.58</Y>
-        <Z>0.38</Z>
+        <X>0.626</X>
+        <Y>0.601</Y>
+        <Z>0.569</Z>
         <W>0.025</W>
         </Color>
       <Size>
@@ -329,9 +329,9 @@
       <Type>AnamorphicInverted</Type>
       <Material>SunFlareSquare</Material>
       <Color>
-        <X>0.76</X>
-        <Y>0.58</Y>
-        <Z>0.38</Z>
+        <X>0.626</X>
+        <Y>0.601</Y>
+        <Z>0.569</Z>
         <W>0.025</W>
         </Color>
       <Size>
@@ -349,9 +349,9 @@
       <Type>AnamorphicInverted</Type>
       <Material>SunFlareSquare</Material>
       <Color>
-        <X>0.76</X>
-        <Y>0.58</Y>
-        <Z>0.38</Z>
+        <X>0.626</X>
+        <Y>0.601</Y>
+        <Z>0.569</Z>
         <W>0.025</W>
         </Color>
       <Size>
@@ -369,9 +369,9 @@
       <Type>AnamorphicInverted</Type>
       <Material>SunFlareSquare</Material>
       <Color>
-        <X>0.76</X>
-        <Y>0.58</Y>
-        <Z>0.38</Z>
+        <X>0.626</X>
+        <Y>0.601</Y>
+        <Z>0.569</Z>
         <W>0.025</W>
         </Color>
       <Size>
@@ -389,9 +389,9 @@
       <Type>AnamorphicInverted</Type>
       <Material>SunFlareWhiteAnamorphic</Material>
       <Color>
-        <X>0.96</X>
-        <Y>0.78</Y>
-        <Z>0.58</Z>
+        <X>0.833</X>
+        <Y>0.8</Y>
+        <Z>0.758</Z>
         <W>0.025</W>
         </Color>
       <Size>
@@ -409,9 +409,9 @@
       <Type>Anamorphic</Type>
       <Material>SunFlareWhiteAnamorphic</Material>
       <Color>
-        <X>0.76</X>
-        <Y>0.58</Y>
-        <Z>0.38</Z>
+        <X>0.626</X>
+        <Y>0.601</Y>
+        <Z>0.569</Z>
         <W>0.025</W>
         </Color>
       <Size>
@@ -429,9 +429,9 @@
       <Type>Anamorphic</Type>
       <Material>SunFlareWhiteAnamorphic</Material>
       <Color>
-        <X>0.76</X>
-        <Y>0.58</Y>
-        <Z>0.38</Z>
+        <X>0.626</X>
+        <Y>0.601</Y>
+        <Z>0.569</Z>
         <W>0.025</W>
         </Color>
       <Size>
```

## Mod: JS - No Uranium Ammo (8 differences)

#### [ModAdjuster] Tag: `Blueprint` | Type: `BlueprintDefinition` | Subtype: `Position0101_Missile200mmHoming`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - No Uranium Ammo\Data\ModAdjuster\Ammos.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Blueprints.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -7,8 +7,7 @@
     <Item Amount="55" TypeId="Ingot" SubtypeId="Iron" />
     <Item Amount="7" TypeId="Ingot" SubtypeId="Nickel" />
     <Item Amount="0.3" TypeId="Ingot" SubtypeId="Silicon" />
-    <Item Amount="0.1" TypeId="Ingot" SubtypeId="Uranium" />
-    <Item Amount="0.04" TypeId="Ingot" SubtypeId="Platinum" />
+    <Item Amount="0.32" TypeId="Ingot" SubtypeId="Gold" />
     <Item Amount="0.8" TypeId="Ingot" SubtypeId="Magnesium" />
     </Prerequisites>
   </Blueprint>
```

#### [ModAdjuster] Tag: `Blueprint` | Type: `BlueprintDefinition` | Subtype: `Position0120_LargeCalibreAmmo`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - No Uranium Ammo\Data\ModAdjuster\Ammos.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Blueprints.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -7,6 +7,6 @@
     <Item Amount="60" TypeId="Ingot" SubtypeId="Iron" />
     <Item Amount="8" TypeId="Ingot" SubtypeId="Nickel" />
     <Item Amount="5" TypeId="Ingot" SubtypeId="Magnesium" />
-    <Item Amount="0.1" TypeId="Ingot" SubtypeId="Uranium" />
+    <Item Amount="3" TypeId="Ingot" SubtypeId="Cobalt" />
     </Prerequisites>
   </Blueprint>
```

#### [ModAdjuster] Tag: `Blueprint` | Type: `BlueprintDefinition` | Subtype: `Position0130_SmallRailgunAmmo`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - No Uranium Ammo\Data\ModAdjuster\Ammos.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Blueprints.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -7,6 +7,6 @@
     <Item Amount="4" TypeId="Ingot" SubtypeId="Iron" />
     <Item Amount="0.5" TypeId="Ingot" SubtypeId="Nickel" />
     <Item Amount="5" TypeId="Ingot" SubtypeId="Silicon" />
-    <Item Amount="0.2" TypeId="Ingot" SubtypeId="Uranium" />
+    <Item Amount="6" TypeId="Ingot" SubtypeId="Cobalt" />
     </Prerequisites>
   </Blueprint>
```

#### [ModAdjuster] Tag: `Blueprint` | Type: `BlueprintDefinition` | Subtype: `Position0140_LargeRailgunAmmo`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - No Uranium Ammo\Data\ModAdjuster\Ammos.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Blueprints.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -7,6 +7,6 @@
     <Item Amount="20" TypeId="Ingot" SubtypeId="Iron" />
     <Item Amount="3" TypeId="Ingot" SubtypeId="Nickel" />
     <Item Amount="30" TypeId="Ingot" SubtypeId="Silicon" />
-    <Item Amount="1" TypeId="Ingot" SubtypeId="Uranium" />
+    <Item Amount="30" TypeId="Ingot" SubtypeId="Cobalt" />
     </Prerequisites>
   </Blueprint>
```

#### [ModAdjuster] Tag: `Blueprint` | Type: `BlueprintDefinition` | Subtype: `Position0200_Torpedo`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - No Uranium Ammo\Data\ModAdjuster\Ammos.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Blueprints.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -7,8 +7,7 @@
     <Item Amount="400" TypeId="Ingot" SubtypeId="Iron" />
     <Item Amount="30" TypeId="Ingot" SubtypeId="Nickel" />
     <Item Amount="1" TypeId="Ingot" SubtypeId="Silicon" />
-    <Item Amount="1" TypeId="Ingot" SubtypeId="Uranium" />
-    <Item Amount="1" TypeId="Ingot" SubtypeId="Platinum" />
+    <Item Amount="5" TypeId="Ingot" SubtypeId="Gold" />
     <Item Amount="10" TypeId="Ingot" SubtypeId="Magnesium" />
     </Prerequisites>
   </Blueprint>
```

#### [ModAdjuster] Tag: `Blueprint` | Type: `BlueprintDefinition` | Subtype: `Position0210_Torpedo_Seeking`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - No Uranium Ammo\Data\ModAdjuster\Ammos.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Blueprints.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -7,8 +7,7 @@
     <Item Amount="400" TypeId="Ingot" SubtypeId="Iron" />
     <Item Amount="30" TypeId="Ingot" SubtypeId="Nickel" />
     <Item Amount="1" TypeId="Ingot" SubtypeId="Silicon" />
-    <Item Amount="1" TypeId="Ingot" SubtypeId="Uranium" />
-    <Item Amount="2" TypeId="Ingot" SubtypeId="Platinum" />
+    <Item Amount="8" TypeId="Ingot" SubtypeId="Gold" />
     <Item Amount="5" TypeId="Ingot" SubtypeId="Magnesium" />
     </Prerequisites>
   </Blueprint>
```

#### [ModAdjuster] Tag: `Blueprint` | Type: `BlueprintDefinition` | Subtype: `Position0220_Torpedo_Heavy`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - No Uranium Ammo\Data\ModAdjuster\Ammos.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Blueprints.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -7,8 +7,7 @@
     <Item Amount="400" TypeId="Ingot" SubtypeId="Iron" />
     <Item Amount="30" TypeId="Ingot" SubtypeId="Nickel" />
     <Item Amount="1" TypeId="Ingot" SubtypeId="Silicon" />
-    <Item Amount="2" TypeId="Ingot" SubtypeId="Uranium" />
-    <Item Amount="1" TypeId="Ingot" SubtypeId="Platinum" />
+    <Item Amount="7" TypeId="Ingot" SubtypeId="Gold" />
     <Item Amount="20" TypeId="Ingot" SubtypeId="Magnesium" />
     </Prerequisites>
   </Blueprint>
```

#### [ModAdjuster] Tag: `Blueprint` | Type: `BlueprintDefinition` | Subtype: `Position0230_Torpedo_Piercing`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - No Uranium Ammo\Data\ModAdjuster\Ammos.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Blueprints.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -7,8 +7,8 @@
     <Item Amount="400" TypeId="Ingot" SubtypeId="Iron" />
     <Item Amount="30" TypeId="Ingot" SubtypeId="Nickel" />
     <Item Amount="1" TypeId="Ingot" SubtypeId="Silicon" />
-    <Item Amount="3" TypeId="Ingot" SubtypeId="Uranium" />
-    <Item Amount="1" TypeId="Ingot" SubtypeId="Platinum" />
+    <Item Amount="5" TypeId="Ingot" SubtypeId="Gold" />
+    <Item Amount="60" TypeId="Ingot" SubtypeId="Cobalt" />
     <Item Amount="5" TypeId="Ingot" SubtypeId="Magnesium" />
     </Prerequisites>
   </Blueprint>
```

## Mod: JS - Thruster Rebalance (46 differences)

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockLargeHydrogenThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,5 +4,5 @@
     <SubtypeId>LargeBlockLargeHydrogenThrust</SubtypeId>
     </Id>
   <ForceMagnitude>7200000</ForceMagnitude>
-  <MaxPowerConsumption>7.5</MaxPowerConsumption>
+  <MaxPowerConsumption>7.002</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockLargeHydrogenThrustIndustrial`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_IndustrialPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,5 +4,5 @@
     <SubtypeId>LargeBlockLargeHydrogenThrustIndustrial</SubtypeId>
     </Id>
   <ForceMagnitude>7200000</ForceMagnitude>
-  <MaxPowerConsumption>7.5</MaxPowerConsumption>
+  <MaxPowerConsumption>7.002</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockLargeHydrogenThrustReskin`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_CoreSystemsPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,5 +4,5 @@
     <SubtypeId>LargeBlockLargeHydrogenThrustReskin</SubtypeId>
     </Id>
   <ForceMagnitude>7200000</ForceMagnitude>
-  <MaxPowerConsumption>7.5</MaxPowerConsumption>
+  <MaxPowerConsumption>7.002</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockSmallHydrogenThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>LargeBlockSmallHydrogenThrust</SubtypeId>
     </Id>
-  <ForceMagnitude>1080000</ForceMagnitude>
-  <MaxPowerConsumption>1.25</MaxPowerConsumption>
+  <ForceMagnitude>800000</ForceMagnitude>
+  <MaxPowerConsumption>0.88914</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockSmallHydrogenThrustIndustrial`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_IndustrialPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>LargeBlockSmallHydrogenThrustIndustrial</SubtypeId>
     </Id>
-  <ForceMagnitude>1080000</ForceMagnitude>
-  <MaxPowerConsumption>1.25</MaxPowerConsumption>
+  <ForceMagnitude>800000</ForceMagnitude>
+  <MaxPowerConsumption>0.88914</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockSmallHydrogenThrustReskin`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_CoreSystemsPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>LargeBlockSmallHydrogenThrustReskin</SubtypeId>
     </Id>
-  <ForceMagnitude>1080000</ForceMagnitude>
-  <MaxPowerConsumption>1.25</MaxPowerConsumption>
+  <ForceMagnitude>800000</ForceMagnitude>
+  <MaxPowerConsumption>0.88914</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockLargeHydrogenThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockLargeHydrogenThrust</SubtypeId>
     </Id>
-  <ForceMagnitude>480000</ForceMagnitude>
-  <MaxPowerConsumption>0.6</MaxPowerConsumption>
+  <ForceMagnitude>720000</ForceMagnitude>
+  <MaxPowerConsumption>0.80023</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockLargeHydrogenThrustIndustrial`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_IndustrialPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockLargeHydrogenThrustIndustrial</SubtypeId>
     </Id>
-  <ForceMagnitude>480000</ForceMagnitude>
-  <MaxPowerConsumption>0.6</MaxPowerConsumption>
+  <ForceMagnitude>720000</ForceMagnitude>
+  <MaxPowerConsumption>0.80023</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockLargeHydrogenThrustReskin`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_CoreSystemsPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockLargeHydrogenThrustReskin</SubtypeId>
     </Id>
-  <ForceMagnitude>480000</ForceMagnitude>
-  <MaxPowerConsumption>0.6</MaxPowerConsumption>
+  <ForceMagnitude>720000</ForceMagnitude>
+  <MaxPowerConsumption>0.80023</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockSmallHydrogenThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockSmallHydrogenThrust</SubtypeId>
     </Id>
-  <ForceMagnitude>98400</ForceMagnitude>
-  <MaxPowerConsumption>0.125</MaxPowerConsumption>
+  <ForceMagnitude>80000</ForceMagnitude>
+  <MaxPowerConsumption>0.10373</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockSmallHydrogenThrustIndustrial`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_IndustrialPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockSmallHydrogenThrustIndustrial</SubtypeId>
     </Id>
-  <ForceMagnitude>98400</ForceMagnitude>
-  <MaxPowerConsumption>0.125</MaxPowerConsumption>
+  <ForceMagnitude>80000</ForceMagnitude>
+  <MaxPowerConsumption>0.10373</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockSmallHydrogenThrustReskin`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_CoreSystemsPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockSmallHydrogenThrustReskin</SubtypeId>
     </Id>
-  <ForceMagnitude>98400</ForceMagnitude>
-  <MaxPowerConsumption>0.125</MaxPowerConsumption>
+  <ForceMagnitude>80000</ForceMagnitude>
+  <MaxPowerConsumption>0.10373</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockLargeThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,5 +4,5 @@
     <SubtypeId>LargeBlockLargeThrust</SubtypeId>
     </Id>
   <ForceMagnitude>4320000</ForceMagnitude>
-  <MaxPowerConsumption>33.6</MaxPowerConsumption>
+  <MaxPowerConsumption>28.8</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockLargeThrustSciFi`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_SparksOfTheFuturePack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,5 +4,5 @@
     <SubtypeId>LargeBlockLargeThrustSciFi</SubtypeId>
     </Id>
   <ForceMagnitude>4320000</ForceMagnitude>
-  <MaxPowerConsumption>33.6</MaxPowerConsumption>
+  <MaxPowerConsumption>28.8</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockLargeModularThruster`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Warfare2.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,5 +4,5 @@
     <SubtypeId>LargeBlockLargeModularThruster</SubtypeId>
     </Id>
   <ForceMagnitude>4320000</ForceMagnitude>
-  <MaxPowerConsumption>33.6</MaxPowerConsumption>
+  <MaxPowerConsumption>28.8</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockSmallThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>LargeBlockSmallThrust</SubtypeId>
     </Id>
-  <ForceMagnitude>345600</ForceMagnitude>
-  <MaxPowerConsumption>3.36</MaxPowerConsumption>
+  <ForceMagnitude>360000</ForceMagnitude>
+  <MaxPowerConsumption>2.76923</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockSmallThrustSciFi`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_SparksOfTheFuturePack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>LargeBlockSmallThrustSciFi</SubtypeId>
     </Id>
-  <ForceMagnitude>345600</ForceMagnitude>
-  <MaxPowerConsumption>3.36</MaxPowerConsumption>
+  <ForceMagnitude>360000</ForceMagnitude>
+  <MaxPowerConsumption>2.76923</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockSmallModularThruster`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Warfare2.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>LargeBlockSmallModularThruster</SubtypeId>
     </Id>
-  <ForceMagnitude>345600</ForceMagnitude>
-  <MaxPowerConsumption>3.36</MaxPowerConsumption>
+  <ForceMagnitude>360000</ForceMagnitude>
+  <MaxPowerConsumption>2.76923</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockLargeThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockLargeThrust</SubtypeId>
     </Id>
-  <ForceMagnitude>172800</ForceMagnitude>
-  <MaxPowerConsumption>2.4</MaxPowerConsumption>
+  <ForceMagnitude>432000</ForceMagnitude>
+  <MaxPowerConsumption>3.32308</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockLargeThrustSciFi`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_SparksOfTheFuturePack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockLargeThrustSciFi</SubtypeId>
     </Id>
-  <ForceMagnitude>172800</ForceMagnitude>
-  <MaxPowerConsumption>2.4</MaxPowerConsumption>
+  <ForceMagnitude>432000</ForceMagnitude>
+  <MaxPowerConsumption>3.32308</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockLargeModularThruster`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Warfare2.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockLargeModularThruster</SubtypeId>
     </Id>
-  <ForceMagnitude>172800</ForceMagnitude>
-  <MaxPowerConsumption>2.4</MaxPowerConsumption>
+  <ForceMagnitude>432000</ForceMagnitude>
+  <MaxPowerConsumption>3.32308</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockSmallThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockSmallThrust</SubtypeId>
     </Id>
-  <ForceMagnitude>14400</ForceMagnitude>
-  <MaxPowerConsumption>0.200</MaxPowerConsumption>
+  <ForceMagnitude>36000</ForceMagnitude>
+  <MaxPowerConsumption>0.32727</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockSmallThrustSciFi`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_SparksOfTheFuturePack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockSmallThrustSciFi</SubtypeId>
     </Id>
-  <ForceMagnitude>14400</ForceMagnitude>
-  <MaxPowerConsumption>0.200</MaxPowerConsumption>
+  <ForceMagnitude>36000</ForceMagnitude>
+  <MaxPowerConsumption>0.32727</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockSmallModularThruster`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Warfare2.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockSmallModularThruster</SubtypeId>
     </Id>
-  <ForceMagnitude>14400</ForceMagnitude>
-  <MaxPowerConsumption>0.200</MaxPowerConsumption>
+  <ForceMagnitude>36000</ForceMagnitude>
+  <MaxPowerConsumption>0.32727</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockLargeAtmosphericThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,5 +4,5 @@
     <SubtypeId>LargeBlockLargeAtmosphericThrust</SubtypeId>
     </Id>
   <ForceMagnitude>6480000</ForceMagnitude>
-  <MaxPowerConsumption>16.8</MaxPowerConsumption>
+  <MaxPowerConsumption>16.2</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockLargeAtmosphericThrustSciFi`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_SparksOfTheFuturePack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,5 +4,5 @@
     <SubtypeId>LargeBlockLargeAtmosphericThrustSciFi</SubtypeId>
     </Id>
   <ForceMagnitude>6480000</ForceMagnitude>
-  <MaxPowerConsumption>16.8</MaxPowerConsumption>
+  <MaxPowerConsumption>16.2</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockSmallAtmosphericThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>LargeBlockSmallAtmosphericThrust</SubtypeId>
     </Id>
-  <ForceMagnitude>648000</ForceMagnitude>
+  <ForceMagnitude>720000</ForceMagnitude>
   <MaxPowerConsumption>2.4</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockSmallAtmosphericThrustSciFi`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_SparksOfTheFuturePack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>LargeBlockSmallAtmosphericThrustSciFi</SubtypeId>
     </Id>
-  <ForceMagnitude>648000</ForceMagnitude>
+  <ForceMagnitude>720000</ForceMagnitude>
   <MaxPowerConsumption>2.4</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockLargeAtmosphericThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockLargeAtmosphericThrust</SubtypeId>
     </Id>
-  <ForceMagnitude>576000</ForceMagnitude>
-  <MaxPowerConsumption>2.4</MaxPowerConsumption>
+  <ForceMagnitude>648000</ForceMagnitude>
+  <MaxPowerConsumption>2.16</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockLargeAtmosphericThrustSciFi`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_SparksOfTheFuturePack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockLargeAtmosphericThrustSciFi</SubtypeId>
     </Id>
-  <ForceMagnitude>576000</ForceMagnitude>
-  <MaxPowerConsumption>2.4</MaxPowerConsumption>
+  <ForceMagnitude>648000</ForceMagnitude>
+  <MaxPowerConsumption>2.16</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockSmallAtmosphericThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockSmallAtmosphericThrust</SubtypeId>
     </Id>
-  <ForceMagnitude>96000</ForceMagnitude>
-  <MaxPowerConsumption>0.600</MaxPowerConsumption>
+  <ForceMagnitude>72000</ForceMagnitude>
+  <MaxPowerConsumption>0.36</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockSmallAtmosphericThrustSciFi`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_SparksOfTheFuturePack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockSmallAtmosphericThrustSciFi</SubtypeId>
     </Id>
-  <ForceMagnitude>96000</ForceMagnitude>
-  <MaxPowerConsumption>0.600</MaxPowerConsumption>
+  <ForceMagnitude>72000</ForceMagnitude>
+  <MaxPowerConsumption>0.36</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockLargeFlatAtmosphericThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>LargeBlockLargeFlatAtmosphericThrust</SubtypeId>
     </Id>
-  <ForceMagnitude>2600000</ForceMagnitude>
-  <MaxPowerConsumption>6.7</MaxPowerConsumption>
+  <ForceMagnitude>2400000</ForceMagnitude>
+  <MaxPowerConsumption>6</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockLargeFlatAtmosphericThrustDShape`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>LargeBlockLargeFlatAtmosphericThrustDShape</SubtypeId>
     </Id>
-  <ForceMagnitude>2600000</ForceMagnitude>
-  <MaxPowerConsumption>6.7</MaxPowerConsumption>
+  <ForceMagnitude>2400000</ForceMagnitude>
+  <MaxPowerConsumption>6</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockLargeFlatAtmosphericThrustRound`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>LargeBlockLargeFlatAtmosphericThrustRound</SubtypeId>
     </Id>
-  <ForceMagnitude>2600000</ForceMagnitude>
-  <MaxPowerConsumption>6.7</MaxPowerConsumption>
+  <ForceMagnitude>2400000</ForceMagnitude>
+  <MaxPowerConsumption>6</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockSmallFlatAtmosphericThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>LargeBlockSmallFlatAtmosphericThrust</SubtypeId>
     </Id>
-  <ForceMagnitude>200000</ForceMagnitude>
-  <MaxPowerConsumption>0.8</MaxPowerConsumption>
+  <ForceMagnitude>266667</ForceMagnitude>
+  <MaxPowerConsumption>0.88889</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockSmallFlatAtmosphericThrustDShape`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>LargeBlockSmallFlatAtmosphericThrustDShape</SubtypeId>
     </Id>
-  <ForceMagnitude>200000</ForceMagnitude>
-  <MaxPowerConsumption>0.8</MaxPowerConsumption>
+  <ForceMagnitude>266667</ForceMagnitude>
+  <MaxPowerConsumption>0.88889</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockSmallFlatAtmosphericThrustRound`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>LargeBlockSmallFlatAtmosphericThrustRound</SubtypeId>
     </Id>
-  <ForceMagnitude>200000</ForceMagnitude>
-  <MaxPowerConsumption>0.8</MaxPowerConsumption>
+  <ForceMagnitude>266667</ForceMagnitude>
+  <MaxPowerConsumption>0.88889</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockLargeFlatAtmosphericThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockLargeFlatAtmosphericThrust</SubtypeId>
     </Id>
-  <ForceMagnitude>230000</ForceMagnitude>
-  <MaxPowerConsumption>1</MaxPowerConsumption>
+  <ForceMagnitude>240000</ForceMagnitude>
+  <MaxPowerConsumption>0.8</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockLargeFlatAtmosphericThrustDShape`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockLargeFlatAtmosphericThrustDShape</SubtypeId>
     </Id>
-  <ForceMagnitude>230000</ForceMagnitude>
-  <MaxPowerConsumption>1</MaxPowerConsumption>
+  <ForceMagnitude>240000</ForceMagnitude>
+  <MaxPowerConsumption>0.8</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockLargeFlatAtmosphericThrustRound`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockLargeFlatAtmosphericThrustRound</SubtypeId>
     </Id>
-  <ForceMagnitude>230000</ForceMagnitude>
-  <MaxPowerConsumption>1</MaxPowerConsumption>
+  <ForceMagnitude>240000</ForceMagnitude>
+  <MaxPowerConsumption>0.8</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockSmallFlatAtmosphericThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockSmallFlatAtmosphericThrust</SubtypeId>
     </Id>
-  <ForceMagnitude>32000</ForceMagnitude>
-  <MaxPowerConsumption>0.200</MaxPowerConsumption>
+  <ForceMagnitude>26667</ForceMagnitude>
+  <MaxPowerConsumption>0.13333</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockSmallFlatAtmosphericThrustDShape`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockSmallFlatAtmosphericThrustDShape</SubtypeId>
     </Id>
-  <ForceMagnitude>32000</ForceMagnitude>
-  <MaxPowerConsumption>0.200</MaxPowerConsumption>
+  <ForceMagnitude>26667</ForceMagnitude>
+  <MaxPowerConsumption>0.13333</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockSmallFlatAtmosphericThrustRound`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockSmallFlatAtmosphericThrustRound</SubtypeId>
     </Id>
-  <ForceMagnitude>32000</ForceMagnitude>
-  <MaxPowerConsumption>0.200</MaxPowerConsumption>
+  <ForceMagnitude>26667</ForceMagnitude>
+  <MaxPowerConsumption>0.13333</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockPrototechThruster`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Prototech.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>LargeBlockPrototechThruster</SubtypeId>
     </Id>
-  <ForceMagnitude>14040000</ForceMagnitude>
-  <MaxPowerConsumption>67.2</MaxPowerConsumption>
+  <ForceMagnitude>12960000</ForceMagnitude>
+  <MaxPowerConsumption>48</MaxPowerConsumption>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockPrototechThruster`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Thruster Rebalance\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Prototech.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockPrototechThruster</SubtypeId>
     </Id>
-  <ForceMagnitude>561600</ForceMagnitude>
-  <MaxPowerConsumption>4.8</MaxPowerConsumption>
+  <ForceMagnitude>1296000</ForceMagnitude>
+  <MaxPowerConsumption>5.53846</MaxPowerConsumption>
   </Definition>
```

## Mod: JS - Vanilla Radar (3 differences)

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section1_Position1_LargeBlocks`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Vanilla Radar\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -7,955 +7,6 @@
   <Name>Section1_Position1_LargeBlocks</Name>
   <StrictSearch>true</StrictSearch>
   <ItemIds>
+    <string>LargeGatlingTurret/Lidar</string>
     </ItemIds>
   </Category>
```

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section1_Position1_SmallBlocks`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Vanilla Radar\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -7,579 +7,6 @@
   <Name>Section1_Position1_SmallBlocks</Name>
   <StrictSearch>true</StrictSearch>
   <ItemIds>
+    <string>LargeGatlingTurret/LidarSmall</string>
     </ItemIds>
   </Category>
```

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section1_Position3_Weapons`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Vanilla Radar\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -7,50 +7,7 @@
   <Name>Section1_Position3_Weapons</Name>
   <StrictSearch>true</StrictSearch>
   <ItemIds>
+    <string>LargeGatlingTurret/Lidar</string>
+    <string>LargeGatlingTurret/LidarSmall</string>
     </ItemIds>
   </Category>
```

## Mod: JS - Weapon Overhaul (31 differences)

#### Tag: `Sound` | Type: `MyObjectBuilder_AudioDefinition` | Subtype: `ArcWepShipGatlingShot`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\Audio.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Audio.sbc`

```diff
--- Base Game
+++ Mod
@@ -9,30 +9,33 @@
   <DynamicMusicCategory>HeavyFight</DynamicMusicCategory>
   <DynamicMusicAmount>3</DynamicMusicAmount>
   <DistantSounds>
-    <DistantSound DistanceCrossfade="10" Distance="200" Sound="WepShipGatlingShotDistant1" />
+    <DistantSound DistanceCrossfade="10" Distance="200" Sound="JSWepGatlingShotDist" />
     </DistantSounds>
-  <Volume>0.7</Volume>
+  <Volume>1.5</Volume>
   <Waves>
     <Wave Type="D3">
-      <Start>ARC\WEP\ArcWepShipGatlingShot3d01.wav</Start>
+      <Start>Audio\Wep\gatling01.wav</Start>
       </Wave>
     <Wave Type="D3">
-      <Start>ARC\WEP\ArcWepShipGatlingShot3d02.wav</Start>
+      <Start>Audio\Wep\gatling02.wav</Start>
       </Wave>
     <Wave Type="D3">
-      <Start>ARC\WEP\ArcWepShipGatlingShot3d03.wav</Start>
+      <Start>Audio\Wep\gatling03.wav</Start>
       </Wave>
     <Wave Type="D3">
-      <Start>ARC\WEP\ArcWepShipGatlingShot3d04.wav</Start>
+      <Start>Audio\Wep\gatling04.wav</Start>
       </Wave>
     <Wave Type="D3">
-      <Start>ARC\WEP\ArcWepShipGatlingShot3d05.wav</Start>
+      <Start>Audio\Wep\gatling05.wav</Start>
       </Wave>
     <Wave Type="D3">
-      <Start>ARC\WEP\ArcWepShipGatlingShot3d06.wav</Start>
+      <Start>Audio\Wep\gatling06.wav</Start>
       </Wave>
     <Wave Type="D3">
-      <Start>ARC\WEP\ArcWepShipGatlingShot3d07.wav</Start>
+      <Start>Audio\Wep\gatling07.wav</Start>
+      </Wave>
+    <Wave Type="D3">
+      <Start>Audio\Wep\gatling08.wav</Start>
       </Wave>
     </Waves>
   </Sound>
```

#### Tag: `Sound` | Type: `MyObjectBuilder_AudioDefinition` | Subtype: `ArcWepGatlingTurretShot`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\Audio.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Audio.sbc`

```diff
--- Base Game
+++ Mod
@@ -4,33 +4,38 @@
     <SubtypeId>ArcWepGatlingTurretShot</SubtypeId>
     </Id>
   <Category>SHOT</Category>
-  <MaxDistance>300</MaxDistance>
+  <MaxDistance>230</MaxDistance>
   <PreventSynchronization>0</PreventSynchronization>
+  <DynamicMusicCategory>HeavyFight</DynamicMusicCategory>
+  <DynamicMusicAmount>3</DynamicMusicAmount>
   <DistantSounds>
-    <DistantSound DistanceCrossfade="10" Distance="230" Sound="WepGatlingTurretShotDistant1" />
+    <DistantSound DistanceCrossfade="10" Distance="230" Sound="JSWepGatlingShotDist" />
     </DistantSounds>
-  <Volume>0.5</Volume>
+  <Volume>1.5</Volume>
   <Waves>
     <Wave Type="D3">
-      <Start>ARC\WEP\ArcWepTurretGatlingFire3d01.wav</Start>
+      <Start>Audio\Wep\gatling01.wav</Start>
       </Wave>
     <Wave Type="D3">
-      <Start>ARC\WEP\ArcWepTurretGatlingFire3d02.wav</Start>
+      <Start>Audio\Wep\gatling02.wav</Start>
       </Wave>
     <Wave Type="D3">
-      <Start>ARC\WEP\ArcWepTurretGatlingFire3d03.wav</Start>
+      <Start>Audio\Wep\gatling03.wav</Start>
       </Wave>
     <Wave Type="D3">
-      <Start>ARC\WEP\ArcWepTurretGatlingFire3d04.wav</Start>
+      <Start>Audio\Wep\gatling04.wav</Start>
       </Wave>
     <Wave Type="D3">
-      <Start>ARC\WEP\ArcWepTurretGatlingFire3d05.wav</Start>
+      <Start>Audio\Wep\gatling05.wav</Start>
       </Wave>
     <Wave Type="D3">
-      <Start>ARC\WEP\ArcWepTurretGatlingFire3d06.wav</Start>
+      <Start>Audio\Wep\gatling06.wav</Start>
       </Wave>
     <Wave Type="D3">
-      <Start>ARC\WEP\ArcWepTurretGatlingFire3d07.wav</Start>
+      <Start>Audio\Wep\gatling07.wav</Start>
+      </Wave>
+    <Wave Type="D3">
+      <Start>Audio\Wep\gatling08.wav</Start>
       </Wave>
     </Waves>
   </Sound>
```

#### Tag: `Sound` | Type: `MyObjectBuilder_AudioDefinition` | Subtype: `ArcWepShipAutocannonShot`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\Audio.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Audio.sbc`

```diff
--- Base Game
+++ Mod
@@ -5,21 +5,14 @@
     </Id>
   <Category>SHOT</Category>
   <MaxDistance>200</MaxDistance>
-  <PitchVariation>30</PitchVariation>
   <PreventSynchronization>1</PreventSynchronization>
   <DistantSounds>
-    <DistantSound DistanceCrossfade="20" Distance="200" Sound="ArcWepAutocannonShotDistant" />
+    <DistantSound DistanceCrossfade="20" Distance="200" Sound="JSWepAutocannonShotDist" />
     </DistantSounds>
-  <Volume>1</Volume>
+  <Volume>2</Volume>
   <Waves>
     <Wave Type="D3">
-      <Start>ARC\WEP\Autocannon_Shot_1.wav</Start>
-      </Wave>
-    <Wave Type="D3">
-      <Start>ARC\WEP\Autocannon_Shot_2.wav</Start>
-      </Wave>
-    <Wave Type="D3">
-      <Start>ARC\WEP\Autocannon_Shot_3.wav</Start>
+      <Start>Audio\Wep\autocannon.wav</Start>
       </Wave>
     </Waves>
   </Sound>
```

#### Tag: `Sound` | Type: `MyObjectBuilder_AudioDefinition` | Subtype: `ArcWepMediumCalibreShot`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\Audio.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Audio.sbc`

```diff
--- Base Game
+++ Mod
@@ -7,19 +7,13 @@
   <MaxDistance>200</MaxDistance>
   <PreventSynchronization>0</PreventSynchronization>
   <DistantSounds>
-    <DistantSound Distance="150" Sound="WepMediumCalibreShotDistA" />
-    <DistantSound Distance="500" Sound="WepMediumCalibreShotDistB" />
+    <DistantSound Distance="150" Sound="JSWepMediumCalibreShotDistA" />
+    <DistantSound Distance="500" Sound="JSWepMediumCalibreShotDistB" />
     </DistantSounds>
-  <Volume>2</Volume>
+  <Volume>3</Volume>
   <Waves>
     <Wave Type="D3">
-      <Start>ARC\WEP\Medium_C_Shot_1.wav</Start>
-      </Wave>
-    <Wave Type="D3">
-      <Start>ARC\WEP\Medium_C_Shot_2.wav</Start>
-      </Wave>
-    <Wave Type="D3">
-      <Start>ARC\WEP\Medium_C_Shot_3.wav</Start>
+      <Start>Audio\Wep\assaultcannon.wav</Start>
       </Wave>
     </Waves>
   </Sound>
```

#### Tag: `Sound` | Type: `MyObjectBuilder_AudioDefinition` | Subtype: `ArcWepLargeCalibreShot`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\Audio.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Audio.sbc`

```diff
--- Base Game
+++ Mod
@@ -7,19 +7,13 @@
   <MaxDistance>200</MaxDistance>
   <PreventSynchronization>0</PreventSynchronization>
   <DistantSounds>
-    <DistantSound Distance="150" Sound="ArcWepLargeCalibreShotDistA" />
-    <DistantSound Distance="800" Sound="ArcWepLargeCalibreShotDistB" />
+    <DistantSound Distance="150" Sound="JSWepLargeCalibreShotDistA" />
+    <DistantSound Distance="800" Sound="JSWepLargeCalibreShotDistB" />
     </DistantSounds>
-  <Volume>2</Volume>
+  <Volume>3</Volume>
   <Waves>
     <Wave Type="D3">
-      <Start>ARC\WEP\Heavy_C_Shot_1.wav</Start>
-      </Wave>
-    <Wave Type="D3">
-      <Start>ARC\WEP\Heavy_C_Shot_2.wav</Start>
-      </Wave>
-    <Wave Type="D3">
-      <Start>ARC\WEP\Heavy_C_Shot_3.wav</Start>
+      <Start>Audio\Wep\artillery.wav</Start>
       </Wave>
     </Waves>
   </Sound>
```

#### Tag: `Sound` | Type: `MyObjectBuilder_AudioDefinition` | Subtype: `ArcWepShipSmallMissileShot`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\Audio.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Audio.sbc`

```diff
--- Base Game
+++ Mod
@@ -7,41 +7,14 @@
   <MaxDistance>200</MaxDistance>
   <PreventSynchronization>2</PreventSynchronization>
   <DistantSounds>
-    <DistantSound DistanceCrossfade="10" Distance="200" Sound="WepShipSmallMissileShotDistA" />
+    <DistantSound DistanceCrossfade="10" Distance="200" Sound="JSWepRocketShotDist" />
     </DistantSounds>
   <DynamicMusicCategory>HeavyFight</DynamicMusicCategory>
   <DynamicMusicAmount>15</DynamicMusicAmount>
-  <Volume>0.380835</Volume>
+  <Volume>3</Volume>
   <Waves>
-    <Wave Type="D2">
-      <Start>ARC\WEP\ArcWepShipSmallMissileShot2d01.xwm</Start>
-      </Wave>
     <Wave Type="D3">
-      <Start>ARC\WEP\ArcWepShipSmallMissileShot3d05.xwm</Start>
-      </Wave>
-    <Wave Type="D3">
-      <Start>ARC\WEP\ArcWepShipSmallMissileShot3d04.xwm</Start>
-      </Wave>
-    <Wave Type="D3">
-      <Start>ARC\WEP\ArcWepShipSmallMissileShot3d03.xwm</Start>
-      </Wave>
-    <Wave Type="D3">
-      <Start>ARC\WEP\ArcWepShipSmallMissileShot3d02.xwm</Start>
-      </Wave>
-    <Wave Type="D3">
-      <Start>ARC\WEP\ArcWepShipSmallMissileShot3d01.xwm</Start>
-      </Wave>
-    <Wave Type="D2">
-      <Start>ARC\WEP\ArcWepShipSmallMissileShot2d02.xwm</Start>
-      </Wave>
-    <Wave Type="D2">
-      <Start>ARC\WEP\ArcWepShipSmallMissileShot2d03.xwm</Start>
-      </Wave>
-    <Wave Type="D2">
-      <Start>ARC\WEP\ArcWepShipSmallMissileShot2d04.xwm</Start>
-      </Wave>
-    <Wave Type="D2">
-      <Start>ARC\WEP\ArcWepShipSmallMissileShot2d05.xwm</Start>
+      <Start>Audio\Wep\rocket.wav</Start>
       </Wave>
     </Waves>
   </Sound>
```

#### Tag: `Sound` | Type: `MyObjectBuilder_AudioDefinition` | Subtype: `ArcTorpedoLaunch`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\Audio.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Audio.sbc`

```diff
--- Base Game
+++ Mod
@@ -5,16 +5,16 @@
     </Id>
   <Category>SHOT</Category>
   <MaxDistance>200</MaxDistance>
-  <PreventSynchronization>0</PreventSynchronization>
-  <PitchVariation>150</PitchVariation>
+  <PreventSynchronization>2</PreventSynchronization>
   <DistantSounds>
-    <DistantSound Distance="200" Sound="WepTurretMissileShotDistA" />
-    <DistantSound Distance="400" Sound="WepTurretMissileShotDistB" />
+    <DistantSound DistanceCrossfade="10" Distance="200" Sound="JSTorpedoLaunchDist" />
     </DistantSounds>
-  <Volume>2.0</Volume>
+  <DynamicMusicCategory>HeavyFight</DynamicMusicCategory>
+  <DynamicMusicAmount>15</DynamicMusicAmount>
+  <Volume>3</Volume>
   <Waves>
     <Wave Type="D3">
-      <Start>ARC\WEP\ArcTorpedoLaunch1.wav</Start>
+      <Start>Audio\Wep\torpedo.wav</Start>
       </Wave>
     </Waves>
   </Sound>
```

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section1_Position1_LargeBlocks`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -7,955 +7,10 @@
   <Name>Section1_Position1_LargeBlocks</Name>
   <StrictSearch>true</StrictSearch>
   <ItemIds>
+    <string>LargeGatlingTurret/LargeAutocannonTurret</string>
+    <string>LargeMissileTurret/RailgunTurret</string>
+    <string>LargeGatlingTurret/LargeGatlingTurretContact</string>
+    <string>LargeMissileTurret/LargeMissileTurretContact</string>
+    <string>LargeGatlingTurret/LargeAutocannonTurretContact</string>
     </ItemIds>
   </Category>
```

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section1_Position1_SmallBlocks`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -7,579 +7,13 @@
   <Name>Section1_Position1_SmallBlocks</Name>
   <StrictSearch>true</StrictSearch>
   <ItemIds>
+    <string>SmallMissileLauncherReload/SmallBlockLargeCalibreGun</string>
+    <string>SmallMissileLauncherReload/SmallBlockLargeCalibreGunType2</string>
+    <string>LargeGatlingTurret/SmallGatlingTurretContact</string>
+    <string>LargeMissileTurret/SmallMissileTurretContact</string>
+    <string>LargeGatlingTurret/AutoCannonTurretJS</string>
+    <string>LargeGatlingTurret/AutoCannonTurretContact</string>
+    <string>LargeMissileTurret/SmallBlockMediumCalibreTurretJS</string>
+    <string>LargeMissileTurret/SmallBlockMediumCalibreTurretContact</string>
     </ItemIds>
   </Category>
```

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section1_Position2_Nonarmor`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -7,1385 +7,18 @@
   <Name>Section1_Position2_Nonarmor</Name>
   <StrictSearch>true</StrictSearch>
   <ItemIds>
+    <string>LargeGatlingTurret/LargeAutocannonTurret</string>
+    <string>LargeMissileTurret/RailgunTurret</string>
+    <string>SmallMissileLauncherReload/SmallBlockLargeCalibreGun</string>
+    <string>SmallMissileLauncherReload/SmallBlockLargeCalibreGunType2</string>
+    <string>LargeGatlingTurret/LargeGatlingTurretContact</string>
+    <string>LargeGatlingTurret/SmallGatlingTurretContact</string>
+    <string>LargeMissileTurret/LargeMissileTurretContact</string>
+    <string>LargeMissileTurret/SmallMissileTurretContact</string>
+    <string>LargeGatlingTurret/LargeAutocannonTurretContact</string>
+    <string>LargeGatlingTurret/AutoCannonTurretJS</string>
+    <string>LargeGatlingTurret/AutoCannonTurretContact</string>
+    <string>LargeMissileTurret/SmallBlockMediumCalibreTurretJS</string>
+    <string>LargeMissileTurret/SmallBlockMediumCalibreTurretContact</string>
     </ItemIds>
   </Category>
```

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section1_Position3_Weapons`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -7,50 +7,18 @@
   <Name>Section1_Position3_Weapons</Name>
   <StrictSearch>true</StrictSearch>
   <ItemIds>
+    <string>LargeGatlingTurret/LargeAutocannonTurret</string>
+    <string>LargeMissileTurret/RailgunTurret</string>
+    <string>SmallMissileLauncherReload/SmallBlockLargeCalibreGun</string>
+    <string>SmallMissileLauncherReload/SmallBlockLargeCalibreGunType2</string>
+    <string>LargeGatlingTurret/LargeGatlingTurretContact</string>
+    <string>LargeGatlingTurret/SmallGatlingTurretContact</string>
+    <string>LargeMissileTurret/LargeMissileTurretContact</string>
+    <string>LargeMissileTurret/SmallMissileTurretContact</string>
+    <string>LargeGatlingTurret/LargeAutocannonTurretContact</string>
+    <string>LargeGatlingTurret/AutoCannonTurretJS</string>
+    <string>LargeGatlingTurret/AutoCannonTurretContact</string>
+    <string>LargeMissileTurret/SmallBlockMediumCalibreTurretJS</string>
+    <string>LargeMissileTurret/SmallBlockMediumCalibreTurretContact</string>
     </ItemIds>
   </Category>
```

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `ShipWeapons`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -10,25 +10,7 @@
   <IsBlockCategory>false</IsBlockCategory>
   <IsToolCategory>true</IsToolCategory>
   <ItemIds>
+    <string>SmallMissileLauncherReload/SmallBlockLargeCalibreGun</string>
+    <string>SmallMissileLauncherReload/SmallBlockLargeCalibreGunType2</string>
     </ItemIds>
   </Category>
```

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section2_DLC`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -8,717 +8,13 @@
   <StrictSearch>true</StrictSearch>
   <DLC>CompleteEdition</DLC>
   <ItemIds>
+    <string>SmallMissileLauncherReload/SmallBlockLargeCalibreGunType2</string>
+    <string>LargeGatlingTurret/LargeGatlingTurretContact</string>
+    <string>LargeGatlingTurret/SmallGatlingTurretContact</string>
+    <string>LargeMissileTurret/LargeMissileTurretContact</string>
+    <string>LargeMissileTurret/SmallMissileTurretContact</string>
+    <string>LargeGatlingTurret/LargeAutocannonTurretContact</string>
+    <string>LargeGatlingTurret/AutoCannonTurretContact</string>
+    <string>LargeMissileTurret/SmallBlockMediumCalibreTurretContact</string>
     </ItemIds>
   </Category>
```

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section2_DLC_Contact`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -8,54 +8,12 @@
   <StrictSearch>true</StrictSearch>
   <DLC>Contact</DLC>
   <ItemIds>
+    <string>LargeGatlingTurret/LargeGatlingTurretContact</string>
+    <string>LargeGatlingTurret/SmallGatlingTurretContact</string>
+    <string>LargeMissileTurret/LargeMissileTurretContact</string>
+    <string>LargeMissileTurret/SmallMissileTurretContact</string>
+    <string>LargeGatlingTurret/LargeAutocannonTurretContact</string>
+    <string>LargeGatlingTurret/AutoCannonTurretContact</string>
+    <string>LargeMissileTurret/SmallBlockMediumCalibreTurretContact</string>
     </ItemIds>
   </Category>
```

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section2_DLC_WarfarePack3`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -8,39 +8,6 @@
   <StrictSearch>true</StrictSearch>
   <DLC>Warfare3</DLC>
   <ItemIds>
+    <string>SmallMissileLauncherReload/SmallBlockLargeCalibreGunType2</string>
     </ItemIds>
   </Category>
```

#### Tag: `BlockVariantGroup` | Type: `MyObjectBuilder_BlockVariantGroup` | Subtype: `TurretGroup`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\BlockVariantGroups.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockVariantGroups.sbc`

```diff
--- Base Game
+++ Mod
@@ -8,18 +8,29 @@
     <Block Type="MyObjectBuilder_LargeGatlingTurret" Subtype="SmallGatlingTurret" />
     <Block Type="MyObjectBuilder_LargeGatlingTurret" Subtype="LargeGatlingTurretReskin" />
     <Block Type="MyObjectBuilder_LargeGatlingTurret" Subtype="SmallGatlingTurretReskin" />
+    <Block Type="MyObjectBuilder_LargeGatlingTurret" Subtype="LargeGatlingTurretContact" />
+    <Block Type="MyObjectBuilder_LargeGatlingTurret" Subtype="SmallGatlingTurretContact" />
+    <Block Type="MyObjectBuilder_LargeGatlingTurret" Subtype="LargeAutocannonTurret" />
+    <Block Type="MyObjectBuilder_LargeGatlingTurret" Subtype="AutoCannonTurret" />
+    <Block Type="MyObjectBuilder_LargeGatlingTurret" Subtype="AutoCannonTurretType2" />
+    <Block Type="MyObjectBuilder_LargeGatlingTurret" Subtype="AutoCannonTurretJS" />
+    <Block Type="MyObjectBuilder_LargeGatlingTurret" Subtype="LargeAutocannonTurretContact" />
+    <Block Type="MyObjectBuilder_LargeGatlingTurret" Subtype="AutoCannonTurretContact" />
+    <Block Type="MyObjectBuilder_LargeMissileTurret" Subtype="LargeBlockMediumCalibreTurret" />
+    <Block Type="MyObjectBuilder_LargeMissileTurret" Subtype="SmallBlockMediumCalibreTurret" />
+    <Block Type="MyObjectBuilder_LargeMissileTurret" Subtype="LargeBlockMediumCalibreTurretType2" />
+    <Block Type="MyObjectBuilder_LargeMissileTurret" Subtype="SmallBlockMediumCalibreTurretType2" />
+    <Block Type="MyObjectBuilder_LargeMissileTurret" Subtype="SmallBlockMediumCalibreTurretJS" />
+    <Block Type="MyObjectBuilder_LargeMissileTurret" Subtype="SmallBlockMediumCalibreTurretContact" />
+    <Block Type="MyObjectBuilder_LargeMissileTurret" Subtype="LargeCalibreTurret" />
+    <Block Type="MyObjectBuilder_LargeMissileTurret" Subtype="LargeCalibreTurretType2" />
+    <Block Type="MyObjectBuilder_LargeMissileTurret" Subtype="RailgunTurret" />
     <Block Type="MyObjectBuilder_LargeMissileTurret" Subtype="" />
     <Block Type="MyObjectBuilder_LargeMissileTurret" Subtype="SmallMissileTurret" />
     <Block Type="MyObjectBuilder_LargeMissileTurret" Subtype="LargeMissileTurretReskin" />
     <Block Type="MyObjectBuilder_LargeMissileTurret" Subtype="SmallMissileTurretReskin" />
-    <Block Type="MyObjectBuilder_LargeGatlingTurret" Subtype="AutoCannonTurret" />
-    <Block Type="MyObjectBuilder_LargeGatlingTurret" Subtype="AutoCannonTurretType2" />
-    <Block Type="MyObjectBuilder_LargeMissileTurret" Subtype="LargeCalibreTurret" />
-    <Block Type="MyObjectBuilder_LargeMissileTurret" Subtype="LargeCalibreTurretType2" />
-    <Block Type="MyObjectBuilder_LargeMissileTurret" Subtype="LargeBlockMediumCalibreTurret" />
-    <Block Type="MyObjectBuilder_LargeMissileTurret" Subtype="SmallBlockMediumCalibreTurret" />
-    <Block Type="MyObjectBuilder_LargeMissileTurret" Subtype="LargeBlockMediumCalibreTurretType2" />
-    <Block Type="MyObjectBuilder_LargeMissileTurret" Subtype="SmallBlockMediumCalibreTurretType2" />
+    <Block Type="MyObjectBuilder_LargeMissileTurret" Subtype="LargeMissileTurretContact" />
+    <Block Type="MyObjectBuilder_LargeMissileTurret" Subtype="SmallMissileTurretContact" />
     <Block Type="MyObjectBuilder_InteriorTurret" Subtype="LargeInteriorTurret" />
     </Blocks>
   </BlockVariantGroup>
```

#### Tag: `BlockVariantGroup` | Type: `MyObjectBuilder_BlockVariantGroup` | Subtype: `ShipWeaponStaticGroup`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\BlockVariantGroups.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockVariantGroups.sbc`

```diff
--- Base Game
+++ Mod
@@ -15,12 +15,14 @@
     <Block Type="MyObjectBuilder_SmallGatlingGun" Subtype="SmallGatlingGunWarfare2" />
     <Block Type="MyObjectBuilder_SmallGatlingGun" Subtype="SmallBlockAutocannon" />
     <Block Type="MyObjectBuilder_SmallGatlingGun" Subtype="SmallBlockAutocannonType2" />
+    <Block Type="MyObjectBuilder_SmallMissileLauncherReload" Subtype="SmallBlockMediumCalibreGun" />
+    <Block Type="MyObjectBuilder_SmallMissileLauncherReload" Subtype="SmallBlockMediumCalibreGunType2" />
+    <Block Type="MyObjectBuilder_SmallMissileLauncher" Subtype="LargeBlockLargeCalibreGun" />
+    <Block Type="MyObjectBuilder_SmallMissileLauncherReload" Subtype="SmallBlockLargeCalibreGun" />
+    <Block Type="MyObjectBuilder_SmallMissileLauncher" Subtype="LargeBlockLargeCalibreGunType2" />
+    <Block Type="MyObjectBuilder_SmallMissileLauncherReload" Subtype="SmallBlockLargeCalibreGunType2" />
     <Block Type="MyObjectBuilder_SmallMissileLauncherReload" Subtype="SmallRailgun" />
     <Block Type="MyObjectBuilder_SmallMissileLauncherReload" Subtype="LargeRailgun" />
-    <Block Type="MyObjectBuilder_SmallMissileLauncher" Subtype="LargeBlockLargeCalibreGun" />
-    <Block Type="MyObjectBuilder_SmallMissileLauncher" Subtype="LargeBlockLargeCalibreGunType2" />
-    <Block Type="MyObjectBuilder_SmallMissileLauncherReload" Subtype="SmallBlockMediumCalibreGun" />
-    <Block Type="MyObjectBuilder_SmallMissileLauncherReload" Subtype="SmallBlockMediumCalibreGunType2" />
     <Block Type="MyObjectBuilder_SmallMissileLauncher" Subtype="LargeFlareLauncher" />
     <Block Type="MyObjectBuilder_SmallMissileLauncher" Subtype="SmallFlareLauncher" />
     <Block Type="MyObjectBuilder_SmallMissileLauncher" Subtype="LargeInsetFlareLauncher" />
```

#### Tag: `ResearchGroup` | Type: `MyObjectBuilder_ResearchGroupDefinition` | Subtype: `8`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\ResearchGroups.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\ResearchGroups.sbc`

```diff
--- Base Game
+++ Mod
@@ -11,11 +11,22 @@
     <BlockId Type="MyObjectBuilder_LargeMissileTurret" Subtype="SmallMissileTurretReskin" />
     <BlockId Type="MyObjectBuilder_LargeMissileTurret" Subtype="LargeCalibreTurret" />
     <BlockId Type="MyObjectBuilder_LargeMissileTurret" Subtype="LargeCalibreTurretType2" />
+    <BlockId Type="MyObjectBuilder_LargeMissileTurret" Subtype="RailgunTurret" />
     <BlockId Type="MyObjectBuilder_LargeMissileTurret" Subtype="LargeBlockMediumCalibreTurret" />
     <BlockId Type="MyObjectBuilder_LargeMissileTurret" Subtype="SmallBlockMediumCalibreTurret" />
     <BlockId Type="MyObjectBuilder_LargeMissileTurret" Subtype="LargeBlockMediumCalibreTurretType2" />
     <BlockId Type="MyObjectBuilder_LargeMissileTurret" Subtype="SmallBlockMediumCalibreTurretType2" />
+    <BlockId Type="MyObjectBuilder_LargeGatlingTurret" Subtype="LargeAutocannonTurret" />
     <BlockId Type="MyObjectBuilder_LargeGatlingTurret" Subtype="AutoCannonTurret" />
     <BlockId Type="MyObjectBuilder_LargeGatlingTurret" Subtype="AutoCannonTurretType2" />
+    <BlockId Type="MyObjectBuilder_LargeGatlingTurret" Subtype="LargeGatlingTurretContact" />
+    <BlockId Type="MyObjectBuilder_LargeGatlingTurret" Subtype="SmallGatlingTurretContact" />
+    <BlockId Type="MyObjectBuilder_LargeMissileTurret" Subtype="LargeMissileTurretContact" />
+    <BlockId Type="MyObjectBuilder_LargeMissileTurret" Subtype="SmallMissileTurretContact" />
+    <BlockId Type="MyObjectBuilder_LargeGatlingTurret" Subtype="LargeAutocannonTurretContact" />
+    <BlockId Type="MyObjectBuilder_LargeGatlingTurret" Subtype="AutoCannonTurretJS" />
+    <BlockId Type="MyObjectBuilder_LargeGatlingTurret" Subtype="AutoCannonTurretContact" />
+    <BlockId Type="MyObjectBuilder_LargeMissileTurret" Subtype="SmallBlockMediumCalibreTurretJS" />
+    <BlockId Type="MyObjectBuilder_LargeMissileTurret" Subtype="SmallBlockMediumCalibreTurretContact" />
     </Members>
   </ResearchGroup>
```

#### Tag: `ResearchGroup` | Type: `MyObjectBuilder_ResearchGroupDefinition` | Subtype: `17`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\ResearchGroups.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\ResearchGroups.sbc`

```diff
--- Base Game
+++ Mod
@@ -6,6 +6,8 @@
     <BlockId Type="MyObjectBuilder_SmallMissileLauncher" Subtype="SmallMissileLauncherWarfare2" />
     <BlockId Type="MyObjectBuilder_SmallMissileLauncher" Subtype="LargeBlockLargeCalibreGun" />
     <BlockId Type="MyObjectBuilder_SmallMissileLauncher" Subtype="LargeBlockLargeCalibreGunType2" />
+    <BlockId Type="MyObjectBuilder_SmallMissileLauncherReload" Subtype="SmallBlockLargeCalibreGun" />
+    <BlockId Type="MyObjectBuilder_SmallMissileLauncherReload" Subtype="SmallBlockLargeCalibreGunType2" />
     <BlockId Type="MyObjectBuilder_SmallMissileLauncherReload" Subtype="SmallBlockMediumCalibreGun" />
     <BlockId Type="MyObjectBuilder_SmallMissileLauncherReload" Subtype="SmallBlockMediumCalibreGunType2" />
     <BlockId Type="MyObjectBuilder_SmallMissileLauncherReload" Subtype="LargeRailgun" />
```

#### Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `LargeCalibreTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Artillery.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game
+++ Mod
@@ -12,7 +12,7 @@
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="3" y="3" z="3" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Large\HeavyCalibreTurret.mwm</Model>
+  <Model>Models\Cubes\large\ArtilleryTurret\HeavyCalibreTurret.mwm</Model>
   <UseModelIntersection>true</UseModelIntersection>
   <Components>
     <Component Subtype="SteelPlate" Count="150" />
@@ -31,9 +31,9 @@
     <MountPoint Side="Bottom" StartX="1" StartY="2" EndX="2" EndY="2.5" />
     </MountPoints>
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\Large\HeavyCalibreTurretConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\Large\HeavyCalibreTurretConstruction_2.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Large\HeavyCalibreTurretConstruction_3.mwm" />
+    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\large\ArtilleryTurret\HeavyCalibreTurret_BS1.mwm" />
+    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\large\ArtilleryTurret\HeavyCalibreTurret_BS2.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\large\ArtilleryTurret\HeavyCalibreTurret_BS3.mwm" />
     </BuildProgressModels>
   <VoxelPlacement>
     <StaticMode>
```

#### Tag: `Definition` | Type: `SmallMissileLauncher` | Subtype: `LargeBlockLargeCalibreGun`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Artillery.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game
+++ Mod
@@ -11,7 +11,7 @@
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="1" z="4" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Large\HeavyCalibreGun.mwm</Model>
+  <Model>Models\Cubes\large\Artillery\HeavyCalibreGun.mwm</Model>
   <Components>
     <Component Subtype="SteelPlate" Count="100" />
     <Component Subtype="Construction" Count="20" />
@@ -28,9 +28,9 @@
     <MountPoint Side="Back" StartX="0.2" StartY="0.2" EndX="0.8" EndY="0.8" Default="true" />
     </MountPoints>
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\large\HeavyCalibreGunConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\large\HeavyCalibreGunConstruction_2.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\large\HeavyCalibreGunConstruction_3.mwm" />
+    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\large\Artillery\HeavyCalibreGun_BS1.mwm" />
+    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\large\Artillery\HeavyCalibreGun_BS2.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\large\Artillery\HeavyCalibreGun_BS3.mwm" />
     </BuildProgressModels>
   <MirroringY>Z</MirroringY>
   <MirroringZ>Y</MirroringZ>
```

#### Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `LargeBlockMediumCalibreTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\AssaultCannon.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game
+++ Mod
@@ -12,7 +12,7 @@
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="3" y="2" z="3" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Large\MediumCalibreTurret.mwm</Model>
+  <Model>Models\Cubes\large\AssaultCannonTurret\MediumCalibreTurret.mwm</Model>
   <UseModelIntersection>true</UseModelIntersection>
   <Components>
     <Component Subtype="SteelPlate" Count="100" />
@@ -31,9 +31,9 @@
     <MountPoint Side="Bottom" StartX="1" StartY="2" EndX="2" EndY="2.2" />
     </MountPoints>
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\Large\MediumCalibreTurretConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\Large\MediumCalibreTurretConstruction_2.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Large\MediumCalibreTurretConstruction_3.mwm" />
+    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\large\AssaultCannonTurret\MediumCalibreTurret_BS1.mwm" />
+    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\large\AssaultCannonTurret\MediumCalibreTurret_BS2.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\large\AssaultCannonTurret\MediumCalibreTurret_BS3.mwm" />
     </BuildProgressModels>
   <VoxelPlacement>
     <StaticMode>
```

#### Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `SmallBlockMediumCalibreTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\AssaultCannon.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game
+++ Mod
@@ -12,7 +12,7 @@
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="7" y="5" z="7" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Small\MediumCalibreTurret_Base.mwm</Model>
+  <Model>Models\Cubes\small\AssaultCannonTurret\MediumCalibreTurret_Base.mwm</Model>
   <UseModelIntersection>true</UseModelIntersection>
   <Components>
     <Component Subtype="SteelPlate" Count="25" />
@@ -29,8 +29,8 @@
     <MountPoint Side="Bottom" StartX="2" StartY="2" EndX="5" EndY="5" Default="true" />
     </MountPoints>
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.43" File="Models\Cubes\Small\MediumCalibreTurretConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="1.0" File="Models\Cubes\Small\MediumCalibreTurretConstruction_2.mwm" />
+    <Model BuildPercentUpperBound="0.43" File="Models\Cubes\small\AssaultCannonTurret\MediumCalibreTurret_Base_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.0" File="Models\Cubes\small\AssaultCannonTurret\MediumCalibreTurret_Base_BS2.mwm" />
     </BuildProgressModels>
   <VoxelPlacement>
     <StaticMode>
```

#### Tag: `Definition` | Type: `SmallMissileLauncherReload` | Subtype: `SmallBlockMediumCalibreGun`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\AssaultCannon.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game
+++ Mod
@@ -11,7 +11,7 @@
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="1" z="9" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Small\MediumCaliberGun.mwm</Model>
+  <Model>Models\Cubes\small\AssaultCannon\MediumCaliberGun.mwm</Model>
   <Components>
     <Component Subtype="SteelPlate" Count="10" />
     <Component Subtype="Construction" Count="10" />
@@ -29,8 +29,8 @@
     <MountPoint Side="Back" StartX="0" StartY="0" EndX="1" EndY="1" Default="true" />
     </MountPoints>
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\Small\MediumCaliberGunConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Small\MediumCaliberGunConstruction_2.mwm" />
+    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\small\AssaultCannon\MediumCaliberGun_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\small\AssaultCannon\MediumCaliberGun_BS2.mwm" />
     </BuildProgressModels>
   <MirroringY>Z</MirroringY>
   <MirroringZ>Y</MirroringZ>
```

#### Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `AutoCannonTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Autocannon.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game
+++ Mod
@@ -11,7 +11,7 @@
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="5" y="4" z="5" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Small\AutocannonTurret_Base.mwm</Model>
+  <Model>Models\Cubes\small\AutocannonTurret\AutocannonTurret_Base.mwm</Model>
   <UseModelIntersection>true</UseModelIntersection>
   <Components>
     <Component Subtype="SteelPlate" Count="10" />
@@ -28,8 +28,8 @@
     <MountPoint Side="Bottom" StartX="1.3" StartY="1.3" EndX="3.7" EndY="3.7" Default="true" />
     </MountPoints>
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.43" File="Models\Cubes\Small\AutocannonTurretConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="1.0" File="Models\Cubes\Small\AutocannonTurretConstruction_2.mwm" />
+    <Model BuildPercentUpperBound="0.43" File="Models\Cubes\small\AutocannonTurret\AutocannonTurret_Base_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.0" File="Models\Cubes\small\AutocannonTurret\AutocannonTurret_Base_BS2.mwm" />
     </BuildProgressModels>
   <VoxelPlacement>
     <StaticMode>
@@ -96,6 +96,5 @@
         </item>
       </dictionary>
     </SubpartPairing>
-  <MuzzleProjectileDummyName>muzzle_missile_001</MuzzleProjectileDummyName>
-  <CameraDummyName>muzzle_projectile</CameraDummyName>
+  <MuzzleProjectileDummyName>muzzle_projectile</MuzzleProjectileDummyName>
   </Definition>
```

#### Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: ``
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Gatling.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game
+++ Mod
@@ -11,7 +11,7 @@
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="3" y="3" z="3" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Large\GatlingTurret.mwm</Model>
+  <Model>Models\Cubes\large\GatlingTurretJS\GatlingTurret.mwm</Model>
   <UseModelIntersection>true</UseModelIntersection>
   <Components>
     <Component Subtype="SteelPlate" Count="10" />
@@ -28,9 +28,9 @@
     <MountPoint Side="Bottom" StartX="1" StartY="1" EndX="2" EndY="2" />
     </MountPoints>
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\Large\GatlingTurretConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\Large\GatlingTurretConstruction_2.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Large\GatlingTurretConstruction_3.mwm" />
+    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\large\GatlingTurretJS\GatlingTurret_BS1.mwm" />
+    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\large\GatlingTurretJS\GatlingTurret_BS2.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\large\GatlingTurretJS\GatlingTurret_BS3.mwm" />
     </BuildProgressModels>
   <VoxelPlacement>
     <StaticMode>
@@ -81,6 +81,4 @@
   <TargetingGroups>
     <string>Weapons</string>
     </TargetingGroups>
-  <UpCameraOffset>0</UpCameraOffset>
-  <ForwardCameraOffset>3.5</ForwardCameraOffset>
   </Definition>
```

#### Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `SmallGatlingTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Gatling.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game
+++ Mod
@@ -12,7 +12,7 @@
   <Size x="5" y="5" z="5" />
   <Center x="2" y="0" z="2" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Small\GatlingTurret.mwm</Model>
+  <Model>Models\Cubes\small\GatlingTurretJS\GatlingTurret.mwm</Model>
   <UseModelIntersection>true</UseModelIntersection>
   <Components>
     <Component Subtype="SteelPlate" Count="5" />
@@ -33,8 +33,8 @@
     <MountPoint Side="Front" StartX="1" StartY="0" EndX="4" EndY="1" />
     </MountPoints>
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.43" File="Models\Cubes\Small\GatlingTurretConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="1.0" File="Models\Cubes\Small\GatlingTurretConstruction_2.mwm" />
+    <Model BuildPercentUpperBound="0.43" File="Models\Cubes\small\GatlingTurretJS\GatlingTurret_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.0" File="Models\Cubes\small\GatlingTurretJS\GatlingTurret_BS2.mwm" />
     </BuildProgressModels>
   <VoxelPlacement>
     <StaticMode>
```

#### Tag: `Definition` | Type: `SmallGatlingGun` | Subtype: ``
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Gatling.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game
+++ Mod
@@ -10,7 +10,7 @@
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="1" z="4" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Small\GatlingGun.mwm</Model>
+  <Model>Models\Cubes\small\GatlingGun\GatlingGun.mwm</Model>
   <Components>
     <Component Subtype="SteelPlate" Count="1" />
     <Component Subtype="Construction" Count="1" />
@@ -29,8 +29,8 @@
     <MountPoint Side="Left" StartX="3" StartY="0" EndX="4" EndY="1" />
     </MountPoints>
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\Small\GatlingGunConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Small\GatlingGunConstruction_2.mwm" />
+    <Model BuildPercentUpperBound="0.50" File="Models\Cubes\small\GatlingGun\GatlingGun_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\small\GatlingGun\GatlingGun_BS2.mwm" />
     </BuildProgressModels>
   <BlockPairName>GatlingGun</BlockPairName>
   <Center x="0" y="0" z="3" />
@@ -38,7 +38,7 @@
   <MirroringZ>Y</MirroringZ>
   <BuildTimeSeconds>13</BuildTimeSeconds>
   <EdgeType>Light</EdgeType>
-  <WeaponDefinitionId Subtype="GatlingGun" />
+  <WeaponDefinitionId Subtype="GatlingGunWarfare" />
   <LaserPointerEnabled>false</LaserPointerEnabled>
   <ResourceSinkGroup>Defense</ResourceSinkGroup>
   <InventoryMaxVolume>0.064</InventoryMaxVolume>
```

#### Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: ``
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Rocket.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game
+++ Mod
@@ -12,7 +12,7 @@
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="3" y="3" z="3" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Large\MissileTurret.mwm</Model>
+  <Model>Models\Cubes\large\RocketTurretJS\MissileTurret.mwm</Model>
   <UseModelIntersection>true</UseModelIntersection>
   <Components>
     <Component Subtype="SteelPlate" Count="10" />
@@ -29,9 +29,9 @@
     <MountPoint Side="Bottom" StartX="1" StartY="1" EndX="2" EndY="2" />
     </MountPoints>
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\Large\MissileTurretConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\Large\MissileTurretConstruction_2.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Large\MissileTurretConstruction_3.mwm" />
+    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\large\RocketTurretJS\MissileTurret_BS1.mwm" />
+    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\large\RocketTurretJS\MissileTurret_BS2.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\large\RocketTurretJS\MissileTurret_BS3.mwm" />
     </BuildProgressModels>
   <VoxelPlacement>
     <StaticMode>
```

#### Tag: `Definition` | Type: `SmallMissileLauncher` | Subtype: `LargeMissileLauncher`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Rocket.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game
+++ Mod
@@ -11,7 +11,7 @@
   <BlockTopology>TriangleMesh</BlockTopology>
   <Size x="1" y="1" z="2" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Large\RocketLauncher.mwm</Model>
+  <Model>Models\Cubes\large\RocketLauncher\RocketLauncher.mwm</Model>
   <Components>
     <Component Subtype="SteelPlate" Count="20" />
     <Component Subtype="Construction" Count="8" />
@@ -30,9 +30,9 @@
     <MountPoint Side="Left" StartX="0" StartY="0" EndX="2" EndY="1" />
     </MountPoints>
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\Large\RocketLauncherConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\Large\RocketLauncherConstruction_2.mwm" />
-    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\Large\RocketLauncherConstruction_3.mwm" />
+    <Model BuildPercentUpperBound="0.33" File="Models\Cubes\large\RocketLauncher\RocketLauncher_BS1.mwm" />
+    <Model BuildPercentUpperBound="0.66" File="Models\Cubes\large\RocketLauncher\RocketLauncher_BS2.mwm" />
+    <Model BuildPercentUpperBound="1.00" File="Models\Cubes\large\RocketLauncher\RocketLauncher_BS3.mwm" />
     </BuildProgressModels>
   <BlockPairName>RocketLauncher</BlockPairName>
   <MirroringY>Z</MirroringY>
```

#### Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `SmallMissileTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Rocket.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game
+++ Mod
@@ -13,7 +13,7 @@
   <Size x="5" y="5" z="5" />
   <Center x="2" y="0" z="2" />
   <ModelOffset x="0" y="0" z="0" />
-  <Model>Models\Cubes\Small\MissileTurret.mwm</Model>
+  <Model>Models\Cubes\small\RocketTurretJS\MissileTurret.mwm</Model>
   <UseModelIntersection>true</UseModelIntersection>
   <Components>
     <Component Subtype="SteelPlate" Count="5" />
@@ -34,8 +34,8 @@
     <MountPoint Side="Front" StartX="1" StartY="0" EndX="4" EndY="1" />
     </MountPoints>
   <BuildProgressModels>
-    <Model BuildPercentUpperBound="0.43" File="Models\Cubes\Small\MissileTurretConstruction_1.mwm" />
-    <Model BuildPercentUpperBound="1.0" File="Models\Cubes\Small\MissileTurretConstruction_2.mwm" />
+    <Model BuildPercentUpperBound="0.43" File="Models\Cubes\small\RocketTurretJS\MissileTurret_BS1.mwm" />
+    <Model BuildPercentUpperBound="1.0" File="Models\Cubes\small\RocketTurretJS\MissileTurret_BS2.mwm" />
     </BuildProgressModels>
   <VoxelPlacement>
     <StaticMode>
```

## Mod: JS - Weapon Rebalance (118 differences)

#### [ModAdjuster] Tag: `Weapon` | Type: `WeaponDefinition` | Subtype: `LargeGatlingTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Gatling.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,8 +3,9 @@
     <TypeId>WeaponDefinition</TypeId>
     <SubtypeId>LargeGatlingTurret</SubtypeId>
     </Id>
-  <ProjectileAmmoData RateOfFire="700" ShootSoundName="WepGatlingTurretShot" ShotsInBurst="140" />
+  <ProjectileAmmoData RateOfFire="1200" ShootSoundName="WepGatlingTurretShot" ShotsInBurst="240" />
   <AmmoMagazines>
     <AmmoMagazine Subtype="NATO_25x184mm" />
     </AmmoMagazines>
+  <DamageMultiplier>0.595</DamageMultiplier>
   </Weapon>
```

#### [ModAdjuster] Tag: `Weapon` | Type: `WeaponDefinition` | Subtype: `SmallGatlingTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Gatling.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,9 +3,10 @@
     <TypeId>WeaponDefinition</TypeId>
     <SubtypeId>SmallGatlingTurret</SubtypeId>
     </Id>
-  <ProjectileAmmoData RateOfFire="700" ShootSoundName="WepGatlingTurretShot" ShotsInBurst="140" />
+  <ProjectileAmmoData RateOfFire="1200" ShootSoundName="WepGatlingTurretShot" ShotsInBurst="240" />
   <AmmoMagazines>
     <AmmoMagazine Subtype="NATO_25x184mm" />
     </AmmoMagazines>
-  <ReloadTime>6000</ReloadTime>
+  <DamageMultiplier>0.595</DamageMultiplier>
+  <ReloadTime>4000</ReloadTime>
   </Weapon>
```

#### [ModAdjuster] Tag: `Weapon` | Type: `WeaponDefinition` | Subtype: `GatlingGun`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Gatling.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,8 +3,9 @@
     <TypeId>WeaponDefinition</TypeId>
     <SubtypeId>GatlingGun</SubtypeId>
     </Id>
-  <ProjectileAmmoData RateOfFire="700" ShootSoundName="WepShipGatlingShot" ShotsInBurst="140" />
+  <ProjectileAmmoData RateOfFire="900" ShootSoundName="WepShipGatlingShot" ShotsInBurst="240" />
   <AmmoMagazines>
     <AmmoMagazine Subtype="NATO_25x184mm" />
     </AmmoMagazines>
+  <DamageMultiplier>0.595</DamageMultiplier>
   </Weapon>
```

#### [ModAdjuster] Tag: `Weapon` | Type: `WeaponDefinition` | Subtype: `GatlingGunWarfare`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Gatling.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,8 +3,9 @@
     <TypeId>WeaponDefinition</TypeId>
     <SubtypeId>GatlingGunWarfare</SubtypeId>
     </Id>
-  <ProjectileAmmoData RateOfFire="700" ShootSoundName="WepShipGatlingShot" ShotsInBurst="140" />
+  <ProjectileAmmoData RateOfFire="900" ShootSoundName="WepShipGatlingShot" ShotsInBurst="240" />
   <AmmoMagazines>
     <AmmoMagazine Subtype="NATO_25x184mm" />
     </AmmoMagazines>
+  <DamageMultiplier>0.595</DamageMultiplier>
   </Weapon>
```

#### [ModAdjuster] Tag: `AmmoMagazine` | Type: `AmmoMagazine` | Subtype: `NATO_25x184mm`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Gatling.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\AmmoMagazines.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>AmmoMagazine</TypeId>
     <SubtypeId>NATO_25x184mm</SubtypeId>
     </Id>
-  <Capacity>140</Capacity>
+  <Capacity>240</Capacity>
   </AmmoMagazine>
```

#### [ModAdjuster] Tag: `Ammo` | Type: `AmmoDefinition` | Subtype: `LargeCaliber`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Gatling.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Ammos.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,19 +4,9 @@
     <SubtypeId>LargeCaliber</SubtypeId>
     </Id>
   <BasicProperties>
-    <DesiredSpeed>800</DesiredSpeed>
-    <SpeedVariance>0</SpeedVariance>
-    <MaxTrajectory>800</MaxTrajectory>
-    <BackkickForce>200</BackkickForce>
-    <PhysicalMaterial>GunBullet</PhysicalMaterial>
-    <ExplosiveDamageMultiplier>0.16</ExplosiveDamageMultiplier>
-    <DeathMessage>CauseOfDeath_HeavyWeapons</DeathMessage>
+    <BackkickForce>116.7</BackkickForce>
     </BasicProperties>
   <ProjectileProperties>
-    <ProjectileTrailScale>0.12</ProjectileTrailScale>
-    <ProjectileHitImpulse>2</ProjectileHitImpulse>
-    <ProjectileMassDamage>90</ProjectileMassDamage>
-    <ProjectileHealthDamage>33</ProjectileHealthDamage>
-    <HeadShot>false</HeadShot>
+    <ProjectileHitImpulse>1.17</ProjectileHitImpulse>
     </ProjectileProperties>
   </Ammo>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: ``
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Gatling.xml`
- **Target (Base Game (also overridden in: JS - Weapon Overhaul))**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game (also overridden in: JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,10 +3,10 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId />
     </Id>
-  <RotationSpeed>0.0015</RotationSpeed>
-  <ElevationSpeed>0.0015</ElevationSpeed>
+  <RotationSpeed>0.003</RotationSpeed>
+  <ElevationSpeed>0.003</ElevationSpeed>
   <TieredUpdateTimes>
-    <unsignedInt>10</unsignedInt>
+    <unsignedInt>3</unsignedInt>
     <unsignedInt>0</unsignedInt>
     </TieredUpdateTimes>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `LargeGatlingTurretReskin`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Gatling.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_ContactPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,10 +3,10 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>LargeGatlingTurretReskin</SubtypeId>
     </Id>
-  <RotationSpeed>0.0015</RotationSpeed>
-  <ElevationSpeed>0.0015</ElevationSpeed>
+  <RotationSpeed>0.003</RotationSpeed>
+  <ElevationSpeed>0.003</ElevationSpeed>
   <TieredUpdateTimes>
-    <unsignedInt>10</unsignedInt>
+    <unsignedInt>3</unsignedInt>
     <unsignedInt>0</unsignedInt>
     </TieredUpdateTimes>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `LargeGatlingTurretContact`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Gatling.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Gatling.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,10 +3,10 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>LargeGatlingTurretContact</SubtypeId>
     </Id>
-  <RotationSpeed>0.0015</RotationSpeed>
-  <ElevationSpeed>0.0015</ElevationSpeed>
+  <RotationSpeed>0.003</RotationSpeed>
+  <ElevationSpeed>0.003</ElevationSpeed>
   <TieredUpdateTimes>
-    <unsignedInt>10</unsignedInt>
+    <unsignedInt>3</unsignedInt>
     <unsignedInt>0</unsignedInt>
     </TieredUpdateTimes>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `SmallGatlingTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Gatling.xml`
- **Target (Base Game (also overridden in: JS - Weapon Overhaul))**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game (also overridden in: JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,10 +3,10 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>SmallGatlingTurret</SubtypeId>
     </Id>
-  <RotationSpeed>0.002</RotationSpeed>
-  <ElevationSpeed>0.002</ElevationSpeed>
+  <RotationSpeed>0.004</RotationSpeed>
+  <ElevationSpeed>0.004</ElevationSpeed>
   <TieredUpdateTimes>
-    <unsignedInt>10</unsignedInt>
+    <unsignedInt>3</unsignedInt>
     <unsignedInt>0</unsignedInt>
     </TieredUpdateTimes>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `SmallGatlingTurretReskin`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Gatling.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_ContactPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,10 +3,10 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>SmallGatlingTurretReskin</SubtypeId>
     </Id>
-  <RotationSpeed>0.002</RotationSpeed>
-  <ElevationSpeed>0.002</ElevationSpeed>
+  <RotationSpeed>0.004</RotationSpeed>
+  <ElevationSpeed>0.004</ElevationSpeed>
   <TieredUpdateTimes>
-    <unsignedInt>10</unsignedInt>
+    <unsignedInt>3</unsignedInt>
     <unsignedInt>0</unsignedInt>
     </TieredUpdateTimes>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `SmallGatlingTurretContact`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Gatling.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Gatling.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,10 +3,10 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>SmallGatlingTurretContact</SubtypeId>
     </Id>
-  <RotationSpeed>0.002</RotationSpeed>
-  <ElevationSpeed>0.002</ElevationSpeed>
+  <RotationSpeed>0.004</RotationSpeed>
+  <ElevationSpeed>0.004</ElevationSpeed>
   <TieredUpdateTimes>
-    <unsignedInt>10</unsignedInt>
+    <unsignedInt>3</unsignedInt>
     <unsignedInt>0</unsignedInt>
     </TieredUpdateTimes>
   </Definition>
```

#### [ModAdjuster] Tag: `Ammo` | Type: `AmmoDefinition` | Subtype: `LargeCaliber`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Ammos.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,12 +4,7 @@
     <SubtypeId>LargeCaliber</SubtypeId>
     </Id>
   <BasicProperties>
-    <DesiredSpeed>800</DesiredSpeed>
-    <SpeedVariance>0</SpeedVariance>
-    <MaxTrajectory>800</MaxTrajectory>
-    <BackkickForce>200</BackkickForce>
-    <PhysicalMaterial>GunBullet</PhysicalMaterial>
-    <ExplosiveDamageMultiplier>0.16</ExplosiveDamageMultiplier>
-    <DeathMessage>CauseOfDeath_HeavyWeapons</DeathMessage>
+    <DesiredSpeed>1100</DesiredSpeed>
+    <MaxTrajectory>1200</MaxTrajectory>
     </BasicProperties>
   </Ammo>
```

#### [ModAdjuster] Tag: `Ammo` | Type: `AmmoDefinition` | Subtype: `AntiPersonnelTurretMunition`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Ammos.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,12 +4,6 @@
     <SubtypeId>AntiPersonnelTurretMunition</SubtypeId>
     </Id>
   <BasicProperties>
-    <DesiredSpeed>600</DesiredSpeed>
-    <SpeedVariance>0</SpeedVariance>
-    <MaxTrajectory>800</MaxTrajectory>
-    <BackkickForce>1.5</BackkickForce>
-    <PhysicalMaterial>RifleBullet</PhysicalMaterial>
-    <ExplosiveDamageMultiplier>0.15</ExplosiveDamageMultiplier>
-    <DeathMessage>CauseOfDeath_LightWeapons</DeathMessage>
+    <DesiredSpeed>850</DesiredSpeed>
     </BasicProperties>
   </Ammo>
```

#### [ModAdjuster] Tag: `Ammo` | Type: `AmmoDefinition` | Subtype: `AutocannonShell`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Ammos.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,12 +4,7 @@
     <SubtypeId>AutocannonShell</SubtypeId>
     </Id>
   <BasicProperties>
-    <DesiredSpeed>800</DesiredSpeed>
-    <SpeedVariance>0</SpeedVariance>
-    <MaxTrajectory>800</MaxTrajectory>
-    <BackkickForce>1000</BackkickForce>
-    <PhysicalMaterial>GunAutocannonBullet</PhysicalMaterial>
-    <ExplosiveDamageMultiplier>0.18</ExplosiveDamageMultiplier>
-    <DeathMessage>CauseOfDeath_HeavyWeapons</DeathMessage>
+    <DesiredSpeed>1100</DesiredSpeed>
+    <MaxTrajectory>1600</MaxTrajectory>
     </BasicProperties>
   </Ammo>
```

#### [ModAdjuster] Tag: `Ammo` | Type: `AmmoDefinition` | Subtype: `Missile`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Ammos.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,15 +4,6 @@
     <SubtypeId>Missile</SubtypeId>
     </Id>
   <BasicProperties>
-    <DesiredSpeed>200</DesiredSpeed>
-    <SpeedVariance>0</SpeedVariance>
-    <MaxTrajectory>800</MaxTrajectory>
-    <IsExplosive>true</IsExplosive>
-    <BackkickForce>0</BackkickForce>
-    <PhysicalMaterial>Missile</PhysicalMaterial>
-    <ExplosiveDamageMultiplier>0.4</ExplosiveDamageMultiplier>
-    <EndOfLifeSound>WepSmallWarheadExpl</EndOfLifeSound>
-    <EndOfLifeEffect>Explosion_Warhead_02</EndOfLifeEffect>
-    <DeathMessage>CauseOfDeath_HeavyWeapons</DeathMessage>
+    <MaxTrajectory>1300</MaxTrajectory>
     </BasicProperties>
   </Ammo>
```

#### [ModAdjuster] Tag: `Ammo` | Type: `AmmoDefinition` | Subtype: `MissileHoming`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Ammos.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,15 +4,6 @@
     <SubtypeId>MissileHoming</SubtypeId>
     </Id>
   <BasicProperties>
-    <DesiredSpeed>150</DesiredSpeed>
-    <SpeedVariance>0</SpeedVariance>
-    <MaxTrajectory>800</MaxTrajectory>
-    <IsExplosive>true</IsExplosive>
-    <BackkickForce>0</BackkickForce>
-    <PhysicalMaterial>Missile</PhysicalMaterial>
-    <ExplosiveDamageMultiplier>0.4</ExplosiveDamageMultiplier>
-    <EndOfLifeSound>WepSmallWarheadExpl</EndOfLifeSound>
-    <EndOfLifeEffect>Explosion_Warhead_02</EndOfLifeEffect>
-    <DeathMessage>CauseOfDeath_HeavyWeapons</DeathMessage>
+    <MaxTrajectory>1500</MaxTrajectory>
     </BasicProperties>
   </Ammo>
```

#### [ModAdjuster] Tag: `Ammo` | Type: `AmmoDefinition` | Subtype: `Torpedo`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Ammos.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,15 +4,6 @@
     <SubtypeId>Torpedo</SubtypeId>
     </Id>
   <BasicProperties>
-    <DesiredSpeed>150</DesiredSpeed>
-    <SpeedVariance>0</SpeedVariance>
-    <MaxTrajectory>2000</MaxTrajectory>
-    <IsExplosive>true</IsExplosive>
-    <BackkickForce>15000</BackkickForce>
-    <PhysicalMaterial>TorpedoStandard</PhysicalMaterial>
-    <ExplosiveDamageMultiplier>0.21</ExplosiveDamageMultiplier>
-    <EndOfLifeSound>WepLrgWarheadExpl</EndOfLifeSound>
-    <EndOfLifeEffect>Explosion_Warhead_15</EndOfLifeEffect>
-    <DeathMessage>CauseOfDeath_HeavyWeapons</DeathMessage>
+    <MaxTrajectory>2700</MaxTrajectory>
     </BasicProperties>
   </Ammo>
```

#### [ModAdjuster] Tag: `Ammo` | Type: `AmmoDefinition` | Subtype: `Torpedo_Heavy`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Ammos.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,15 +4,6 @@
     <SubtypeId>Torpedo_Heavy</SubtypeId>
     </Id>
   <BasicProperties>
-    <DesiredSpeed>120</DesiredSpeed>
-    <SpeedVariance>0</SpeedVariance>
-    <MaxTrajectory>2000</MaxTrajectory>
-    <IsExplosive>true</IsExplosive>
-    <BackkickForce>15000</BackkickForce>
-    <PhysicalMaterial>TorpedoHeavy</PhysicalMaterial>
-    <ExplosiveDamageMultiplier>0.25</ExplosiveDamageMultiplier>
-    <EndOfLifeSound>WepLrgWarheadExpl</EndOfLifeSound>
-    <EndOfLifeEffect>Explosion_Warhead_30</EndOfLifeEffect>
-    <DeathMessage>CauseOfDeath_HeavyWeapons</DeathMessage>
+    <MaxTrajectory>2700</MaxTrajectory>
     </BasicProperties>
   </Ammo>
```

#### [ModAdjuster] Tag: `Ammo` | Type: `AmmoDefinition` | Subtype: `Torpedo_Seeking`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Ammos.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,15 +4,6 @@
     <SubtypeId>Torpedo_Seeking</SubtypeId>
     </Id>
   <BasicProperties>
-    <DesiredSpeed>220</DesiredSpeed>
-    <SpeedVariance>0</SpeedVariance>
-    <MaxTrajectory>2000</MaxTrajectory>
-    <IsExplosive>true</IsExplosive>
-    <BackkickForce>15000</BackkickForce>
-    <PhysicalMaterial>TorpedoLight</PhysicalMaterial>
-    <ExplosiveDamageMultiplier>0.2</ExplosiveDamageMultiplier>
-    <EndOfLifeSound>WepSmallWarheadExpl</EndOfLifeSound>
-    <EndOfLifeEffect>Explosion_Warhead_02</EndOfLifeEffect>
-    <DeathMessage>CauseOfDeath_HeavyWeapons</DeathMessage>
+    <MaxTrajectory>2700</MaxTrajectory>
     </BasicProperties>
   </Ammo>
```

#### [ModAdjuster] Tag: `Ammo` | Type: `AmmoDefinition` | Subtype: `Torpedo_Piercing`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Ammos.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,15 +4,6 @@
     <SubtypeId>Torpedo_Piercing</SubtypeId>
     </Id>
   <BasicProperties>
-    <DesiredSpeed>180</DesiredSpeed>
-    <SpeedVariance>0</SpeedVariance>
-    <MaxTrajectory>2000</MaxTrajectory>
-    <IsExplosive>true</IsExplosive>
-    <BackkickForce>15000</BackkickForce>
-    <PhysicalMaterial>LargeShell</PhysicalMaterial>
-    <ExplosiveDamageMultiplier>0.22</ExplosiveDamageMultiplier>
-    <EndOfLifeSound>WepSmallWarheadExpl</EndOfLifeSound>
-    <EndOfLifeEffect>Explosion_Warhead_02</EndOfLifeEffect>
-    <DeathMessage>CauseOfDeath_HeavyWeapons</DeathMessage>
+    <MaxTrajectory>2700</MaxTrajectory>
     </BasicProperties>
   </Ammo>
```

#### [ModAdjuster] Tag: `Ammo` | Type: `AmmoDefinition` | Subtype: `MediumCalibreShell`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Ammos.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,36 +4,10 @@
     <SubtypeId>MediumCalibreShell</SubtypeId>
     </Id>
   <BasicProperties>
-    <DesiredSpeed>500</DesiredSpeed>
-    <SpeedVariance>0</SpeedVariance>
-    <MaxTrajectory>1400</MaxTrajectory>
-    <IsExplosive>true</IsExplosive>
-    <BackkickForce>50000</BackkickForce>
-    <PhysicalMaterial>MediumShell</PhysicalMaterial>
-    <ExplosiveDamageMultiplier>0.3</ExplosiveDamageMultiplier>
-    <DeathMessage>CauseOfDeath_HeavyWeapons</DeathMessage>
+    <DesiredSpeed>900</DesiredSpeed>
+    <MaxTrajectory>2000</MaxTrajectory>
     </BasicProperties>
   <MissileProperties>
-    <MissileMass>300</MissileMass>
-    <MissileExplosionRadius>2</MissileExplosionRadius>
-    <MissileModelName>Models\Weapons\MediumCalibreShell.mwm</MissileModelName>
-    <MissileAcceleration>0</MissileAcceleration>
-    <MissileInitialSpeed>500</MissileInitialSpeed>
-    <MissileSkipAcceleration>false</MissileSkipAcceleration>
-    <MissileExplosionDamage>200</MissileExplosionDamage>
-    <MissileHealthPool>4000</MissileHealthPool>
-    <MissileTrailEffect>MediumCalibreGun_Tracer</MissileTrailEffect>
-    <MissileGravityEnabled>true</MissileGravityEnabled>
-    <MissileMinRicochetAngle>40</MissileMinRicochetAngle>
-    <MissileMaxRicochetAngle>70</MissileMaxRicochetAngle>
-    <MissileMinRicochetProbability>0.25</MissileMinRicochetProbability>
-    <MissileMaxRicochetProbability>0.85</MissileMaxRicochetProbability>
-    <MissileRicochetDamage>2000</MissileRicochetDamage>
-    <ExplosionFlags>APPLY_FORCE_AND_DAMAGE
-                    CREATE_DECALS
-                    CREATE_SHRAPNELS
-                    APPLY_DEFORMATION
-                    CREATE_PARTICLE_DEBRIS</ExplosionFlags>
-    <CanBeTargeted>false</CanBeTargeted>
+    <MissileInitialSpeed>900</MissileInitialSpeed>
     </MissileProperties>
   </Ammo>
```

#### [ModAdjuster] Tag: `Ammo` | Type: `AmmoDefinition` | Subtype: `LargeCalibreShell`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Ammos.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,36 +4,10 @@
     <SubtypeId>LargeCalibreShell</SubtypeId>
     </Id>
   <BasicProperties>
-    <DesiredSpeed>500</DesiredSpeed>
-    <SpeedVariance>0</SpeedVariance>
-    <MaxTrajectory>2000</MaxTrajectory>
-    <IsExplosive>true</IsExplosive>
-    <BackkickForce>150000</BackkickForce>
-    <PhysicalMaterial>LargeShell</PhysicalMaterial>
-    <ExplosiveDamageMultiplier>0.33</ExplosiveDamageMultiplier>
-    <DeathMessage>CauseOfDeath_HeavyWeapons</DeathMessage>
+    <DesiredSpeed>800</DesiredSpeed>
+    <MaxTrajectory>3000</MaxTrajectory>
     </BasicProperties>
   <MissileProperties>
-    <MissileMass>800</MissileMass>
-    <MissileExplosionRadius>3</MissileExplosionRadius>
-    <MissileModelName>Models\Weapons\LargeCalibreShell.mwm</MissileModelName>
-    <MissileAcceleration>0</MissileAcceleration>
-    <MissileInitialSpeed>500</MissileInitialSpeed>
-    <MissileSkipAcceleration>false</MissileSkipAcceleration>
-    <MissileExplosionDamage>500</MissileExplosionDamage>
-    <MissileHealthPool>17000</MissileHealthPool>
-    <MissileTrailEffect>LargeCalibreGun_Tracer</MissileTrailEffect>
-    <MissileGravityEnabled>true</MissileGravityEnabled>
-    <MissileMinRicochetAngle>50</MissileMinRicochetAngle>
-    <MissileMaxRicochetAngle>80</MissileMaxRicochetAngle>
-    <MissileMinRicochetProbability>0.25</MissileMinRicochetProbability>
-    <MissileMaxRicochetProbability>0.85</MissileMaxRicochetProbability>
-    <MissileRicochetDamage>7000</MissileRicochetDamage>
-    <ExplosionFlags>APPLY_FORCE_AND_DAMAGE
-                    CREATE_DECALS
-                    CREATE_SHRAPNELS
-                    APPLY_DEFORMATION
-                    CREATE_PARTICLE_DEBRIS</ExplosionFlags>
-    <CanBeTargeted>false</CanBeTargeted>
+    <MissileInitialSpeed>800</MissileInitialSpeed>
     </MissileProperties>
   </Ammo>
```

#### [ModAdjuster] Tag: `Ammo` | Type: `AmmoDefinition` | Subtype: `LargeRailgunSlug`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Ammos.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,26 +4,10 @@
     <SubtypeId>LargeRailgunSlug</SubtypeId>
     </Id>
   <BasicProperties>
-    <DesiredSpeed>2000</DesiredSpeed>
-    <SpeedVariance>0</SpeedVariance>
-    <MaxTrajectory>2000</MaxTrajectory>
-    <IsExplosive>true</IsExplosive>
-    <BackkickForce>120000</BackkickForce>
-    <PhysicalMaterial>LargeShell</PhysicalMaterial>
-    <ExplosiveDamageMultiplier>0</ExplosiveDamageMultiplier>
-    <DeathMessage>CauseOfDeath_HeavyWeapons</DeathMessage>
+    <DesiredSpeed>2500</DesiredSpeed>
+    <MaxTrajectory>3000</MaxTrajectory>
     </BasicProperties>
   <MissileProperties>
-    <MissileMass>2000</MissileMass>
-    <MissileExplosionRadius>1</MissileExplosionRadius>
-    <MissileModelName>Models\Weapons\RailgunAmmoLarge.mwm</MissileModelName>
-    <MissileAcceleration>0</MissileAcceleration>
-    <MissileInitialSpeed>2000</MissileInitialSpeed>
-    <MissileSkipAcceleration>false</MissileSkipAcceleration>
-    <MissileExplosionDamage>0</MissileExplosionDamage>
-    <MissileHealthPool>50000</MissileHealthPool>
-    <MissileTrailEffect>RailGunTrail</MissileTrailEffect>
-    <MissileGravityEnabled>true</MissileGravityEnabled>
-    <CanBeTargeted>false</CanBeTargeted>
+    <MissileInitialSpeed>2500</MissileInitialSpeed>
     </MissileProperties>
   </Ammo>
```

#### [ModAdjuster] Tag: `Ammo` | Type: `AmmoDefinition` | Subtype: `SmallRailgunSlug`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Ammos.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,26 +4,10 @@
     <SubtypeId>SmallRailgunSlug</SubtypeId>
     </Id>
   <BasicProperties>
-    <DesiredSpeed>1000</DesiredSpeed>
-    <SpeedVariance>0</SpeedVariance>
-    <MaxTrajectory>1400</MaxTrajectory>
-    <IsExplosive>true</IsExplosive>
-    <BackkickForce>30000</BackkickForce>
-    <PhysicalMaterial>MediumShell</PhysicalMaterial>
-    <ExplosiveDamageMultiplier>0</ExplosiveDamageMultiplier>
-    <DeathMessage>CauseOfDeath_HeavyWeapons</DeathMessage>
+    <DesiredSpeed>1500</DesiredSpeed>
+    <MaxTrajectory>2000</MaxTrajectory>
     </BasicProperties>
   <MissileProperties>
-    <MissileMass>500</MissileMass>
-    <MissileExplosionRadius>0</MissileExplosionRadius>
-    <MissileModelName>Models\Weapons\RailgunAmmo.mwm</MissileModelName>
-    <MissileAcceleration>0</MissileAcceleration>
-    <MissileInitialSpeed>1000</MissileInitialSpeed>
-    <MissileSkipAcceleration>false</MissileSkipAcceleration>
-    <MissileExplosionDamage>0</MissileExplosionDamage>
-    <MissileHealthPool>8000</MissileHealthPool>
-    <MissileTrailEffect>RailGunTrailSmall</MissileTrailEffect>
-    <MissileGravityEnabled>true</MissileGravityEnabled>
-    <CanBeTargeted>false</CanBeTargeted>
+    <MissileInitialSpeed>1500</MissileInitialSpeed>
     </MissileProperties>
   </Ammo>
```

#### [ModAdjuster] Tag: `Weapon` | Type: `WeaponDefinition` | Subtype: `LargeInteriorTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>WeaponDefinition</TypeId>
     <SubtypeId>LargeInteriorTurret</SubtypeId>
     </Id>
-  <RangeMultiplier>0.75</RangeMultiplier>
+  <RangeMultiplier>1</RangeMultiplier>
   </Weapon>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: ``
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game (also overridden in: JS - Weapon Overhaul))**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game (also overridden in: JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId />
     </Id>
-  <MaxRangeMeters>800</MaxRangeMeters>
+  <MaxRangeMeters>1200</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `LargeGatlingTurretReskin`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_ContactPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>LargeGatlingTurretReskin</SubtypeId>
     </Id>
-  <MaxRangeMeters>800</MaxRangeMeters>
+  <MaxRangeMeters>1200</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `LargeGatlingTurretContact`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Gatling.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>LargeGatlingTurretContact</SubtypeId>
     </Id>
-  <MaxRangeMeters>800</MaxRangeMeters>
+  <MaxRangeMeters>1200</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `SmallGatlingTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game (also overridden in: JS - Weapon Overhaul))**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game (also overridden in: JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>SmallGatlingTurret</SubtypeId>
     </Id>
-  <MaxRangeMeters>600</MaxRangeMeters>
+  <MaxRangeMeters>900</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `SmallGatlingTurretReskin`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_ContactPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>SmallGatlingTurretReskin</SubtypeId>
     </Id>
-  <MaxRangeMeters>600</MaxRangeMeters>
+  <MaxRangeMeters>900</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `SmallGatlingTurretContact`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Gatling.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>SmallGatlingTurretContact</SubtypeId>
     </Id>
-  <MaxRangeMeters>600</MaxRangeMeters>
+  <MaxRangeMeters>900</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `InteriorTurret` | Subtype: `LargeInteriorTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>InteriorTurret</TypeId>
     <SubtypeId>LargeInteriorTurret</SubtypeId>
     </Id>
-  <MaxRangeMeters>600</MaxRangeMeters>
+  <MaxRangeMeters>800</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `LargeAutocannonTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Autocannon.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>LargeAutocannonTurret</SubtypeId>
     </Id>
-  <MaxRangeMeters>800</MaxRangeMeters>
+  <MaxRangeMeters>1600</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `LargeAutocannonTurretContact`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Autocannon.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>LargeAutocannonTurretContact</SubtypeId>
     </Id>
-  <MaxRangeMeters>800</MaxRangeMeters>
+  <MaxRangeMeters>1600</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `AutoCannonTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game (also overridden in: JS - Weapon Overhaul))**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game (also overridden in: JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>AutoCannonTurret</SubtypeId>
     </Id>
-  <MaxRangeMeters>600</MaxRangeMeters>
+  <MaxRangeMeters>1200</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `AutoCannonTurretType2`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Warfare3.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>AutoCannonTurretType2</SubtypeId>
     </Id>
-  <MaxRangeMeters>600</MaxRangeMeters>
+  <MaxRangeMeters>1200</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `AutoCannonTurretJS`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Autocannon.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>AutoCannonTurretJS</SubtypeId>
     </Id>
-  <MaxRangeMeters>600</MaxRangeMeters>
+  <MaxRangeMeters>1200</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `AutoCannonTurretContact`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Autocannon.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>AutoCannonTurretContact</SubtypeId>
     </Id>
-  <MaxRangeMeters>600</MaxRangeMeters>
+  <MaxRangeMeters>1200</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: ``
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game (also overridden in: JS - Weapon Overhaul))**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game (also overridden in: JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId />
     </Id>
-  <MaxRangeMeters>800</MaxRangeMeters>
+  <MaxRangeMeters>1300</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `LargeMissileTurretReskin`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_ContactPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>LargeMissileTurretReskin</SubtypeId>
     </Id>
-  <MaxRangeMeters>800</MaxRangeMeters>
+  <MaxRangeMeters>1300</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `LargeMissileTurretContact`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Rocket.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>LargeMissileTurretContact</SubtypeId>
     </Id>
-  <MaxRangeMeters>800</MaxRangeMeters>
+  <MaxRangeMeters>1300</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `SmallMissileTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game (also overridden in: JS - Weapon Overhaul))**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game (also overridden in: JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>SmallMissileTurret</SubtypeId>
     </Id>
-  <MaxRangeMeters>600</MaxRangeMeters>
+  <MaxRangeMeters>1000</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `SmallMissileTurretReskin`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_ContactPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>SmallMissileTurretReskin</SubtypeId>
     </Id>
-  <MaxRangeMeters>600</MaxRangeMeters>
+  <MaxRangeMeters>1000</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `SmallMissileTurretContact`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Rocket.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>SmallMissileTurretContact</SubtypeId>
     </Id>
-  <MaxRangeMeters>600</MaxRangeMeters>
+  <MaxRangeMeters>1000</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `LargeBlockMediumCalibreTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game (also overridden in: JS - Weapon Overhaul))**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game (also overridden in: JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>LargeBlockMediumCalibreTurret</SubtypeId>
     </Id>
-  <MaxRangeMeters>800</MaxRangeMeters>
+  <MaxRangeMeters>2000</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `LargeBlockMediumCalibreTurretType2`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Warfare3.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>LargeBlockMediumCalibreTurretType2</SubtypeId>
     </Id>
-  <MaxRangeMeters>800</MaxRangeMeters>
+  <MaxRangeMeters>2000</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `SmallBlockMediumCalibreTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game (also overridden in: JS - Weapon Overhaul))**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game (also overridden in: JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>SmallBlockMediumCalibreTurret</SubtypeId>
     </Id>
-  <MaxRangeMeters>600</MaxRangeMeters>
+  <MaxRangeMeters>1500</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `SmallBlockMediumCalibreTurretType2`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Warfare3.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>SmallBlockMediumCalibreTurretType2</SubtypeId>
     </Id>
-  <MaxRangeMeters>600</MaxRangeMeters>
+  <MaxRangeMeters>1500</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `SmallBlockMediumCalibreTurretJS`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\AssaultCannon.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>SmallBlockMediumCalibreTurretJS</SubtypeId>
     </Id>
-  <MaxRangeMeters>600</MaxRangeMeters>
+  <MaxRangeMeters>1500</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `SmallBlockMediumCalibreTurretContact`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\AssaultCannon.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>SmallBlockMediumCalibreTurretContact</SubtypeId>
     </Id>
-  <MaxRangeMeters>600</MaxRangeMeters>
+  <MaxRangeMeters>1500</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `LargeCalibreTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game (also overridden in: JS - Weapon Overhaul))**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game (also overridden in: JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>LargeCalibreTurret</SubtypeId>
     </Id>
-  <MaxRangeMeters>800</MaxRangeMeters>
+  <MaxRangeMeters>3000</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `LargeCalibreTurretType2`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Warfare3.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>LargeCalibreTurretType2</SubtypeId>
     </Id>
-  <MaxRangeMeters>800</MaxRangeMeters>
+  <MaxRangeMeters>3000</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `RailgunTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Railgun.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>RailgunTurret</SubtypeId>
     </Id>
-  <MaxRangeMeters>2000</MaxRangeMeters>
+  <MaxRangeMeters>3000</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `TurretControlBlock` | Subtype: `LargeTurretControlBlock`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Automation.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>TurretControlBlock</TypeId>
     <SubtypeId>LargeTurretControlBlock</SubtypeId>
     </Id>
-  <MaxRangeMeters>800</MaxRangeMeters>
+  <MaxRangeMeters>3000</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `TurretControlBlock` | Subtype: `SmallTurretControlBlock`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Ranges.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Automation.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>TurretControlBlock</TypeId>
     <SubtypeId>SmallTurretControlBlock</SubtypeId>
     </Id>
-  <MaxRangeMeters>600</MaxRangeMeters>
+  <MaxRangeMeters>2000</MaxRangeMeters>
   </Definition>
```

#### [ModAdjuster] Tag: `Weapon` | Type: `WeaponDefinition` | Subtype: `GatlingGun`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Accuracy.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>WeaponDefinition</TypeId>
     <SubtypeId>GatlingGun</SubtypeId>
     </Id>
-  <DeviateShotAngle>0.15</DeviateShotAngle>
-  <DeviateShotAngleAiming>0.15</DeviateShotAngleAiming>
+  <DeviateShotAngle>0.25</DeviateShotAngle>
+  <DeviateShotAngleAiming>0.25</DeviateShotAngleAiming>
   </Weapon>
```

#### [ModAdjuster] Tag: `Weapon` | Type: `WeaponDefinition` | Subtype: `GatlingGunWarfare`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Accuracy.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>WeaponDefinition</TypeId>
     <SubtypeId>GatlingGunWarfare</SubtypeId>
     </Id>
-  <DeviateShotAngle>0.15</DeviateShotAngle>
-  <DeviateShotAngleAiming>0.15</DeviateShotAngleAiming>
+  <DeviateShotAngle>0.25</DeviateShotAngle>
+  <DeviateShotAngleAiming>0.25</DeviateShotAngleAiming>
   </Weapon>
```

#### [ModAdjuster] Tag: `Weapon` | Type: `WeaponDefinition` | Subtype: `LargeGatlingTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Accuracy.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>WeaponDefinition</TypeId>
     <SubtypeId>LargeGatlingTurret</SubtypeId>
     </Id>
-  <DeviateShotAngle>0.225</DeviateShotAngle>
-  <DeviateShotAngleAiming>0.225</DeviateShotAngleAiming>
+  <DeviateShotAngle>0.25</DeviateShotAngle>
+  <DeviateShotAngleAiming>0.25</DeviateShotAngleAiming>
   </Weapon>
```

#### [ModAdjuster] Tag: `Weapon` | Type: `WeaponDefinition` | Subtype: `SmallGatlingTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Accuracy.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>WeaponDefinition</TypeId>
     <SubtypeId>SmallGatlingTurret</SubtypeId>
     </Id>
-  <DeviateShotAngle>0.225</DeviateShotAngle>
-  <DeviateShotAngleAiming>0.225</DeviateShotAngleAiming>
+  <DeviateShotAngle>0.25</DeviateShotAngle>
+  <DeviateShotAngleAiming>0.25</DeviateShotAngleAiming>
   </Weapon>
```

#### [ModAdjuster] Tag: `Weapon` | Type: `WeaponDefinition` | Subtype: `Autocannon`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Accuracy.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>WeaponDefinition</TypeId>
     <SubtypeId>Autocannon</SubtypeId>
     </Id>
-  <DeviateShotAngle>0.15</DeviateShotAngle>
-  <DeviateShotAngleAiming>0.15</DeviateShotAngleAiming>
+  <DeviateShotAngle>0.2</DeviateShotAngle>
+  <DeviateShotAngleAiming>0.2</DeviateShotAngleAiming>
   </Weapon>
```

#### [ModAdjuster] Tag: `Weapon` | Type: `WeaponDefinition` | Subtype: `AutocannonTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Accuracy.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>WeaponDefinition</TypeId>
     <SubtypeId>AutocannonTurret</SubtypeId>
     </Id>
-  <DeviateShotAngle>0.25</DeviateShotAngle>
-  <DeviateShotAngleAiming>0.25</DeviateShotAngleAiming>
+  <DeviateShotAngle>0.2</DeviateShotAngle>
+  <DeviateShotAngleAiming>0.2</DeviateShotAngleAiming>
   </Weapon>
```

#### [ModAdjuster] Tag: `Weapon` | Type: `WeaponDefinition` | Subtype: `MediumCalibreTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Accuracy.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>WeaponDefinition</TypeId>
     <SubtypeId>MediumCalibreTurret</SubtypeId>
     </Id>
-  <DeviateShotAngle>0.35</DeviateShotAngle>
-  <DeviateShotAngleAiming>0.35</DeviateShotAngleAiming>
+  <DeviateShotAngle>0.2</DeviateShotAngle>
+  <DeviateShotAngleAiming>0.2</DeviateShotAngleAiming>
   </Weapon>
```

#### [ModAdjuster] Tag: `Weapon` | Type: `WeaponDefinition` | Subtype: `SmallMediumCalibreTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Accuracy.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>WeaponDefinition</TypeId>
     <SubtypeId>SmallMediumCalibreTurret</SubtypeId>
     </Id>
-  <DeviateShotAngle>0.5</DeviateShotAngle>
-  <DeviateShotAngleAiming>0.5</DeviateShotAngleAiming>
+  <DeviateShotAngle>0.2</DeviateShotAngle>
+  <DeviateShotAngleAiming>0.2</DeviateShotAngleAiming>
   </Weapon>
```

#### [ModAdjuster] Tag: `Weapon` | Type: `WeaponDefinition` | Subtype: `LargeCalibreTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Accuracy.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>WeaponDefinition</TypeId>
     <SubtypeId>LargeCalibreTurret</SubtypeId>
     </Id>
-  <DeviateShotAngle>0.3</DeviateShotAngle>
-  <DeviateShotAngleAiming>0.3</DeviateShotAngleAiming>
+  <DeviateShotAngle>0.15</DeviateShotAngle>
+  <DeviateShotAngleAiming>0.15</DeviateShotAngleAiming>
   </Weapon>
```

#### [ModAdjuster] Tag: `Weapon` | Type: `WeaponDefinition` | Subtype: `Autocannon`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\DamageBalance.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,7 +3,7 @@
     <TypeId>WeaponDefinition</TypeId>
     <SubtypeId>Autocannon</SubtypeId>
     </Id>
-  <ProjectileAmmoData RateOfFire="150" ShootSoundName="WepShipAutocannonShot" ShotsInBurst="16" />
+  <ProjectileAmmoData RateOfFire="120" ShootSoundName="WepShipAutocannonShot" ShotsInBurst="16" />
   <AmmoMagazines>
     <AmmoMagazine Subtype="AutocannonClip" />
     </AmmoMagazines>
```

#### [ModAdjuster] Tag: `Weapon` | Type: `WeaponDefinition` | Subtype: `AutocannonTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\DamageBalance.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>WeaponDefinition</TypeId>
     <SubtypeId>AutocannonTurret</SubtypeId>
     </Id>
-  <ReloadTime>4000</ReloadTime>
+  <ReloadTime>2000</ReloadTime>
   </Weapon>
```

#### [ModAdjuster] Tag: `Weapon` | Type: `WeaponDefinition` | Subtype: `SmallMediumCalibreGun`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\DamageBalance.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>WeaponDefinition</TypeId>
     <SubtypeId>SmallMediumCalibreGun</SubtypeId>
     </Id>
-  <ReloadTime>6000</ReloadTime>
+  <ReloadTime>3500</ReloadTime>
   </Weapon>
```

#### [ModAdjuster] Tag: `Weapon` | Type: `WeaponDefinition` | Subtype: `SmallMediumCalibreTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\DamageBalance.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>WeaponDefinition</TypeId>
     <SubtypeId>SmallMediumCalibreTurret</SubtypeId>
     </Id>
-  <ReloadTime>6000</ReloadTime>
+  <ReloadTime>3500</ReloadTime>
   </Weapon>
```

#### [ModAdjuster] Tag: `Weapon` | Type: `WeaponDefinition` | Subtype: `MediumCalibreTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\DamageBalance.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>WeaponDefinition</TypeId>
     <SubtypeId>MediumCalibreTurret</SubtypeId>
     </Id>
-  <ReloadTime>6000</ReloadTime>
+  <ReloadTime>3500</ReloadTime>
   </Weapon>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: ``
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game (also overridden in: JS - Weapon Overhaul))**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game (also overridden in: JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId />
     </Id>
-  <Description>Description_GatlingTurret</Description>
+  <Description>Fast-shooting turret that can be loaded through conveyors. Uses Gatling Ammo Boxes.
+
+Maximum Range: 1,200 m
+Rate of Fire: 1,200 RPM
+Damage: 54</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `LargeGatlingTurretReskin`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_ContactPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>LargeGatlingTurretReskin</SubtypeId>
     </Id>
-  <Description>Description_GatlingTurret</Description>
+  <Description>Fast-shooting turret that can be loaded through conveyors. Uses Gatling Ammo Boxes.
+
+Maximum Range: 1,200 m
+Rate of Fire: 1,200 RPM
+Damage: 54</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `LargeGatlingTurretContact`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Gatling.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>LargeGatlingTurretContact</SubtypeId>
     </Id>
-  <Description>Description_GatlingTurret</Description>
+  <Description>Fast-shooting turret that can be loaded through conveyors. Uses Gatling Ammo Boxes.
+
+Maximum Range: 1,200 m
+Rate of Fire: 1,200 RPM
+Damage: 54</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `SmallGatlingTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game (also overridden in: JS - Weapon Overhaul))**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game (also overridden in: JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>SmallGatlingTurret</SubtypeId>
     </Id>
-  <Description>Description_GatlingTurret</Description>
+  <Description>Fast-shooting turret that can be loaded through conveyors. Uses Gatling Ammo Boxes.
+
+Maximum Range: 900 m
+Rate of Fire: 1,200 RPM
+Damage: 54</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `SmallGatlingTurretReskin`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_ContactPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>SmallGatlingTurretReskin</SubtypeId>
     </Id>
-  <Description>Description_GatlingTurret</Description>
+  <Description>Fast-shooting turret that can be loaded through conveyors. Uses Gatling Ammo Boxes.
+
+Maximum Range: 900 m
+Rate of Fire: 1,200 RPM
+Damage: 54</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `SmallGatlingTurretContact`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Gatling.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>SmallGatlingTurretContact</SubtypeId>
     </Id>
-  <Description>Description_GatlingTurret</Description>
+  <Description>Fast-shooting turret that can be loaded through conveyors. Uses Gatling Ammo Boxes.
+
+Maximum Range: 900 m
+Rate of Fire: 1,200 RPM
+Damage: 54</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `SmallGatlingGun` | Subtype: ``
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game (also overridden in: JS - Weapon Overhaul))**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game (also overridden in: JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>SmallGatlingGun</TypeId>
     <SubtypeId />
     </Id>
-  <Description>Description_GatlingGun</Description>
+  <Description>Fast-shooting gun that can be loaded through conveyors. Uses Gatling Ammo Boxes.
+
+Maximum Range: 1,200 m
+Rate of Fire: 900 RPM
+Damage: 54</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `SmallGatlingGun` | Subtype: `SmallGatlingGunWarfare2`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Warfare2.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>SmallGatlingGun</TypeId>
     <SubtypeId>SmallGatlingGunWarfare2</SubtypeId>
     </Id>
-  <Description>Description_GatlingGun</Description>
+  <Description>Fast-shooting gun that can be loaded through conveyors. Uses Gatling Ammo Boxes.
+
+Maximum Range: 1,200 m
+Rate of Fire: 900 RPM
+Damage: 54</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `InteriorTurret` | Subtype: `LargeInteriorTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>InteriorTurret</TypeId>
     <SubtypeId>LargeInteriorTurret</SubtypeId>
     </Id>
-  <Description>Description_InteriorTurret</Description>
+  <Description>Autonomous Anti-personnel turret. Must be manually loaded with Anti-Personnel Turret Magazines.
+
+Maximum Range: 800 m
+Rate of Fire: 600 RPM
+Damage: 30</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `SmallGatlingGun` | Subtype: `SmallBlockAutocannon`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>SmallGatlingGun</TypeId>
     <SubtypeId>SmallBlockAutocannon</SubtypeId>
     </Id>
-  <Description>Description_Autocannon</Description>
+  <Description>Medium rate of fire, with a good punch. Uses Autocannon Magazines.
+
+Maximum Range: 1,600 m
+Rate of Fire: 120 RPM
+Damage: 500</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `SmallGatlingGun` | Subtype: `SmallBlockAutocannonType2`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Warfare3.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>SmallGatlingGun</TypeId>
     <SubtypeId>SmallBlockAutocannonType2</SubtypeId>
     </Id>
-  <Description>Description_Autocannon</Description>
+  <Description>Medium rate of fire, with a good punch. Uses Autocannon Magazines.
+
+Maximum Range: 1,600 m
+Rate of Fire: 120 RPM
+Damage: 500</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `AutoCannonTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game (also overridden in: JS - Weapon Overhaul))**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game (also overridden in: JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>AutoCannonTurret</SubtypeId>
     </Id>
-  <Description>Description_AutoCannonTurret</Description>
+  <Description>Medium rate of fire, with a good punch in a small turret. Uses Autocannon Magazines.
+
+Maximum Range: 1,200 m
+Rate of Fire: 150 RPM
+Damage: 500</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `AutoCannonTurretType2`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Warfare3.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>AutoCannonTurretType2</SubtypeId>
     </Id>
-  <Description>Description_AutoCannonTurret</Description>
+  <Description>Medium rate of fire, with a good punch in a small turret. Uses Autocannon Magazines.
+
+Maximum Range: 1,200 m
+Rate of Fire: 150 RPM
+Damage: 500</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `AutoCannonTurretJS`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Autocannon.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>AutoCannonTurretJS</SubtypeId>
     </Id>
-  <Description>Description_AutoCannonTurret</Description>
+  <Description>Medium rate of fire, with a good punch in a small turret. Uses Autocannon Magazines.
+
+Maximum Range: 1,200 m
+Rate of Fire: 150 RPM
+Damage: 500</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `AutoCannonTurretContact`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Autocannon.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>AutoCannonTurretContact</SubtypeId>
     </Id>
-  <Description>Description_AutoCannonTurret</Description>
+  <Description>Medium rate of fire, with a good punch in a small turret. Uses Autocannon Magazines.
+
+Maximum Range: 1,200 m
+Rate of Fire: 150 RPM
+Damage: 500</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `LargeAutocannonTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Autocannon.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>LargeAutocannonTurret</SubtypeId>
     </Id>
-  <Description>Description_AutoCannonTurret</Description>
+  <Description>Medium rate of fire, with a good punch in a large turret. Uses Autocannon Magazines.
+
+Maximum Range: 1,600 m
+Rate of Fire: 150 RPM
+Damage: 500</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeGatlingTurret` | Subtype: `LargeAutocannonTurretContact`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Autocannon.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeGatlingTurret</TypeId>
     <SubtypeId>LargeAutocannonTurretContact</SubtypeId>
     </Id>
-  <Description>Description_AutoCannonTurret</Description>
+  <Description>Medium rate of fire, with a good punch in a large turret. Uses Autocannon Magazines.
+
+Maximum Range: 1,600 m
+Rate of Fire: 150 RPM
+Damage: 500</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `SmallMissileLauncher` | Subtype: `LargeMissileLauncher`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game (also overridden in: JS - Weapon Overhaul))**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game (also overridden in: JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>SmallMissileLauncher</TypeId>
     <SubtypeId>LargeMissileLauncher</SubtypeId>
     </Id>
-  <Description>Description_LargeRocketLauncher</Description>
+  <Description>A bigger version of the rocket launcher, capable of using the conveyor system to automatically reload. Uses Rockets and Homing Missiles.
+
+Maximum Range: 1,300 m (Rocket) / 1,500 m (Homing)
+Rate of Fire: 100 RPM
+Damage: 500 (Rocket) / 300 (Homing)</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `SmallMissileLauncher` | Subtype: ``
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>SmallMissileLauncher</TypeId>
     <SubtypeId />
     </Id>
-  <Description>Description_RocketLauncher</Description>
+  <Description>Slow-shooting, explosive damage dealing, rocket launcher that must be loaded manually. Uses Rockets and Homing Missiles.
+
+Maximum Range: 1,300 m (Rocket) / 1,500 m (Homing)
+Rate of Fire: 50 RPM
+Damage: 500 (Rocket) / 300 (Homing)</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `SmallMissileLauncher` | Subtype: `SmallMissileLauncherWarfare2`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Warfare2.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>SmallMissileLauncher</TypeId>
     <SubtypeId>SmallMissileLauncherWarfare2</SubtypeId>
     </Id>
-  <Description>Description_RocketLauncher</Description>
+  <Description>Slow-shooting, explosive damage dealing, rocket launcher that must be loaded manually. Uses Rockets and Homing Missiles.
+
+Maximum Range: 1,300 m (Rocket) / 1,500 m (Homing)
+Rate of Fire: 50 RPM
+Damage: 500 (Rocket) / 300 (Homing)</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `SmallMissileLauncherReload` | Subtype: `SmallRocketLauncherReload`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>SmallMissileLauncherReload</TypeId>
     <SubtypeId>SmallRocketLauncherReload</SubtypeId>
     </Id>
-  <Description>Description_RocketLauncherReload</Description>
+  <Description>An advanced version of the rocket launcher, capable of using the conveyor system to automatically reload. Uses Rockets and Homing Missiles.
+
+Maximum Range: 1,300 m (Rocket) / 1,500 m (Homing)
+Rate of Fire: 50 RPM
+Damage: 500 (Rocket) / 300 (Homing)</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: ``
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game (also overridden in: JS - Weapon Overhaul))**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game (also overridden in: JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId />
     </Id>
-  <Description>Description_MissileTurret</Description>
+  <Description>Slow-shooting, explosive damage dealing, rocket launcher that can be loaded through conveyors. Uses Rockets and Homing Missiles.
+
+Maximum Range: 1,300 m
+Rate of Fire: 90 RPM
+Damage: 500 (Rocket) / 300 (Homing)</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `LargeMissileTurretReskin`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_ContactPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>LargeMissileTurretReskin</SubtypeId>
     </Id>
-  <Description>Description_MissileTurret</Description>
+  <Description>Slow-shooting, explosive damage dealing, rocket launcher that can be loaded through conveyors. Uses Rockets and Homing Missiles.
+
+Maximum Range: 1,300 m
+Rate of Fire: 90 RPM
+Damage: 500 (Rocket) / 300 (Homing)</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `LargeMissileTurretContact`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Rocket.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>LargeMissileTurretContact</SubtypeId>
     </Id>
-  <Description>Description_MissileTurret</Description>
+  <Description>Slow-shooting, explosive damage dealing, rocket launcher that can be loaded through conveyors. Uses Rockets and Homing Missiles.
+
+Maximum Range: 1,300 m
+Rate of Fire: 90 RPM
+Damage: 500 (Rocket) / 300 (Homing)</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `SmallMissileTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game (also overridden in: JS - Weapon Overhaul))**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game (also overridden in: JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>SmallMissileTurret</SubtypeId>
     </Id>
-  <Description>Description_MissileTurret</Description>
+  <Description>Slow-shooting, explosive damage dealing, rocket launcher that can be loaded through conveyors. Uses Rockets and Homing Missiles.
+
+Maximum Range: 1,000 m
+Rate of Fire: 90 RPM
+Damage: 500 (Rocket) / 300 (Homing)</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `SmallMissileTurretReskin`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_ContactPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>SmallMissileTurretReskin</SubtypeId>
     </Id>
-  <Description>Description_MissileTurret</Description>
+  <Description>Slow-shooting, explosive damage dealing, rocket launcher that can be loaded through conveyors. Uses Rockets and Homing Missiles.
+
+Maximum Range: 1,000 m
+Rate of Fire: 90 RPM
+Damage: 500 (Rocket) / 300 (Homing)</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `SmallMissileTurretContact`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Rocket.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>SmallMissileTurretContact</SubtypeId>
     </Id>
-  <Description>Description_MissileTurret</Description>
+  <Description>Slow-shooting, explosive damage dealing, rocket launcher that can be loaded through conveyors. Uses Rockets and Homing Missiles.
+
+Maximum Range: 1,000 m
+Rate of Fire: 90 RPM
+Damage: 500 (Rocket) / 300 (Homing)</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `SmallMissileLauncherReload` | Subtype: `SmallBlockMediumCalibreGun`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game (also overridden in: JS - Weapon Overhaul))**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game (also overridden in: JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>SmallMissileLauncherReload</TypeId>
     <SubtypeId>SmallBlockMediumCalibreGun</SubtypeId>
     </Id>
-  <Description>Description_MediumCalibreGun</Description>
+  <Description>Slower and bigger sibling of the Autocannon. Deals decent damage to armored targets. Uses Assault Cannon Shells.
+
+Maximum Range: 2,000 m
+Rate of Fire: 17 RPM
+Damage: 4,200</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `SmallMissileLauncherReload` | Subtype: `SmallBlockMediumCalibreGunType2`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Warfare3.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>SmallMissileLauncherReload</TypeId>
     <SubtypeId>SmallBlockMediumCalibreGunType2</SubtypeId>
     </Id>
-  <Description>Description_MediumCalibreGun</Description>
+  <Description>Slower and bigger sibling of the Autocannon. Deals decent damage to armored targets. Uses Assault Cannon Shells.
+
+Maximum Range: 2,000 m
+Rate of Fire: 17 RPM
+Damage: 4,200</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `LargeBlockMediumCalibreTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game (also overridden in: JS - Weapon Overhaul))**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game (also overridden in: JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>LargeBlockMediumCalibreTurret</SubtypeId>
     </Id>
-  <Description>Description_MediumCalibreTurret</Description>
+  <Description>Slower and bigger sibling of the Autocannon. Deals decent damage to armored targets. Uses Assault Cannon Shells.
+
+Maximum Range: 2,000 m
+Rate of Fire: 31 RPM
+Damage: 4,200</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `LargeBlockMediumCalibreTurretType2`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Warfare3.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>LargeBlockMediumCalibreTurretType2</SubtypeId>
     </Id>
-  <Description>Description_MediumCalibreTurret</Description>
+  <Description>Slower and bigger sibling of the Autocannon. Deals decent damage to armored targets. Uses Assault Cannon Shells.
+
+Maximum Range: 2,000 m
+Rate of Fire: 31 RPM
+Damage: 4,200</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `SmallBlockMediumCalibreTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game (also overridden in: JS - Weapon Overhaul))**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game (also overridden in: JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>SmallBlockMediumCalibreTurret</SubtypeId>
     </Id>
-  <Description>Description_MediumCalibreGun</Description>
+  <Description>Slower and bigger sibling of the Autocannon. Deals decent damage to armored targets. Uses Assault Cannon Shells.
+
+Maximum Range: 1,500 m
+Rate of Fire: 17 RPM
+Damage: 4,200</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `SmallBlockMediumCalibreTurretType2`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Warfare3.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>SmallBlockMediumCalibreTurretType2</SubtypeId>
     </Id>
-  <Description>Description_MediumCalibreGun</Description>
+  <Description>Slower and bigger sibling of the Autocannon. Deals decent damage to armored targets. Uses Assault Cannon Shells.
+
+Maximum Range: 1,500 m
+Rate of Fire: 17 RPM
+Damage: 4,200</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `SmallBlockMediumCalibreTurretJS`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\AssaultCannon.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>SmallBlockMediumCalibreTurretJS</SubtypeId>
     </Id>
-  <Description>Description_MediumCalibreGun</Description>
+  <Description>Slower and bigger sibling of the Autocannon. Deals decent damage to armored targets. Uses Assault Cannon Shells.
+
+Maximum Range: 1,500 m
+Rate of Fire: 17 RPM
+Damage: 4,200</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `SmallBlockMediumCalibreTurretContact`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\AssaultCannon.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>SmallBlockMediumCalibreTurretContact</SubtypeId>
     </Id>
-  <Description>Description_MediumCalibreGun</Description>
+  <Description>Slower and bigger sibling of the Autocannon. Deals decent damage to armored targets. Uses Assault Cannon Shells.
+
+Maximum Range: 1,500 m
+Rate of Fire: 17 RPM
+Damage: 4,200</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `SmallMissileLauncher` | Subtype: `LargeBlockLargeCalibreGun`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game (also overridden in: JS - Weapon Overhaul))**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game (also overridden in: JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>SmallMissileLauncher</TypeId>
     <SubtypeId>LargeBlockLargeCalibreGun</SubtypeId>
     </Id>
-  <Description>Description_HeavyCalibreGun</Description>
+  <Description>A titanic gun perfectly suited for battles against heavily armored targets. Uses Artillery Shells.
+
+Maximum Range: 3,000 m
+Rate of Fire: 5 RPM
+Damage: 17,500</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `SmallMissileLauncher` | Subtype: `LargeBlockLargeCalibreGunType2`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Warfare3.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>SmallMissileLauncher</TypeId>
     <SubtypeId>LargeBlockLargeCalibreGunType2</SubtypeId>
     </Id>
-  <Description>Description_HeavyCalibreGun</Description>
+  <Description>A titanic gun perfectly suited for battles against heavily armored targets. Uses Artillery Shells.
+
+Maximum Range: 3,000 m
+Rate of Fire: 5 RPM
+Damage: 17,500</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `SmallMissileLauncherReload` | Subtype: `SmallBlockLargeCalibreGun`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Artillery.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>SmallMissileLauncherReload</TypeId>
     <SubtypeId>SmallBlockLargeCalibreGun</SubtypeId>
     </Id>
-  <Description>Description_HeavyCalibreGun</Description>
+  <Description>A titanic gun perfectly suited for battles against heavily armored targets. Uses Artillery Shells.
+
+Maximum Range: 3,000 m
+Rate of Fire: 5 RPM
+Damage: 17,500</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `SmallMissileLauncherReload` | Subtype: `SmallBlockLargeCalibreGunType2`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Artillery.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>SmallMissileLauncherReload</TypeId>
     <SubtypeId>SmallBlockLargeCalibreGunType2</SubtypeId>
     </Id>
-  <Description>Description_HeavyCalibreGun</Description>
+  <Description>A titanic gun perfectly suited for battles against heavily armored targets. Uses Artillery Shells.
+
+Maximum Range: 3,000 m
+Rate of Fire: 5 RPM
+Damage: 17,500</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `LargeCalibreTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game (also overridden in: JS - Weapon Overhaul))**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game (also overridden in: JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>LargeCalibreTurret</SubtypeId>
     </Id>
-  <Description>Description_LargeCalibreTurret</Description>
+  <Description>A titanic turret perfectly suited for battles against heavily armored targets. Uses Artillery Shells.
+
+Maximum Range: 3,000 m
+Rate of Fire: 9.5 RPM
+Damage: 17,500</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `LargeCalibreTurretType2`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Warfare3.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>LargeCalibreTurretType2</SubtypeId>
     </Id>
-  <Description>Description_LargeCalibreTurret</Description>
+  <Description>A titanic turret perfectly suited for battles against heavily armored targets. Uses Artillery Shells.
+
+Maximum Range: 3,000 m
+Rate of Fire: 9.5 RPM
+Damage: 17,500</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `SmallMissileLauncherReload` | Subtype: `LargeRailgun`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>SmallMissileLauncherReload</TypeId>
     <SubtypeId>LargeRailgun</SubtypeId>
     </Id>
-  <Description>Description_Railgun</Description>
+  <Description>High range, maximum penetration, very low rate of fire. Needs to charge up before each shot. Uses Large Railgun Sabots.
+
+Maximum Range: 3,000 m
+Rate of Fire: 1 RPM
+Damage: 50,000</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `SmallMissileLauncherReload` | Subtype: `SmallRailgun`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,9 @@
     <TypeId>SmallMissileLauncherReload</TypeId>
     <SubtypeId>SmallRailgun</SubtypeId>
     </Id>
-  <Description>Description_SmallRailgun</Description>
+  <Description>High range, maximum penetration, very low rate of fire. Needs to charge up before each shot. Uses Small Railgun Sabots.
+
+Maximum Range: 2,000 m
+Rate of Fire: 3 RPM
+Damage: 8,000</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `LargeMissileTurret` | Subtype: `RailgunTurret`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (User Mod (JS - Weapon Overhaul))**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Overhaul\Data\CubeBlocks\Railgun.sbc`

```diff
--- User Mod (JS - Weapon Overhaul)
+++ ModAdjuster
@@ -3,8 +3,9 @@
     <TypeId>LargeMissileTurret</TypeId>
     <SubtypeId>RailgunTurret</SubtypeId>
     </Id>
-  <Description>Low rate of fire extremely powerful electromagnetic mass driver. Capable of putting holes in literally anything.
+  <Description>High range, maximum penetration, very low rate of fire. Needs to charge up before each shot. Uses Large Railgun Sabots.
 
-Charging: 38MW            
-Range: 2000m</Description>
+Maximum Range: 3,000 m
+Rate of Fire: 1 RPM
+Damage: 50,000</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `SmallMissileLauncher` | Subtype: `LargeBlockTorpedoLauncher`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,10 @@
     <TypeId>SmallMissileLauncher</TypeId>
     <SubtypeId>LargeBlockTorpedoLauncher</SubtypeId>
     </Id>
-  <Description>Description_TorpedoLauncher</Description>
+  <Description>The Torpedo Launcher is a weapon to launch various types of munitions. Target lock is required for active guidance.
+Uses: Torpedo, Light Torpedo, Heavy Torpedo and Armor Piercing Torpedo.
+
+Maximum Range: 2,700 m
+Rate of Fire: 2 RPM
+Damage: 3,000 (Std) / 500 (Light) / 8,000 (Heavy) / 21,500 (AP)</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `SmallMissileLauncher` | Subtype: `SmallBlockTorpedoLauncher`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Weapons.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,10 @@
     <TypeId>SmallMissileLauncher</TypeId>
     <SubtypeId>SmallBlockTorpedoLauncher</SubtypeId>
     </Id>
-  <Description>Description_TorpedoLauncher</Description>
+  <Description>The Torpedo Launcher is a weapon to launch various types of munitions. Target lock is required for active guidance.
+Uses: Torpedo, Light Torpedo, Heavy Torpedo and Armor Piercing Torpedo.
+
+Maximum Range: 2,700 m
+Rate of Fire: 2 RPM
+Damage: 3,000 (Std) / 500 (Light) / 8,000 (Heavy) / 21,500 (AP)</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `TurretControlBlock` | Subtype: `LargeTurretControlBlock`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Automation.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,8 @@
     <TypeId>TurretControlBlock</TypeId>
     <SubtypeId>LargeTurretControlBlock</SubtypeId>
     </Id>
-  <Description>Description_TurretControlBlock</Description>
+  <Description>The Custom Turret Controller is capable of providing AI behavior to custom built rotor and hinge-based turrets.
+It can also be used to manually control the turret. A custom turret requires at least one rotor/hinge and one weapon/tool or camera to be functional.
+
+Maximum Range: 3,000 m</Description>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `TurretControlBlock` | Subtype: `SmallTurretControlBlock`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\JS - Weapon Rebalance\Data\ModAdjuster\Descriptions.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Automation.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,8 @@
     <TypeId>TurretControlBlock</TypeId>
     <SubtypeId>SmallTurretControlBlock</SubtypeId>
     </Id>
-  <Description>Description_TurretControlBlock</Description>
+  <Description>The Custom Turret Controller is capable of providing AI behavior to custom built rotor and hinge-based turrets.
+It can also be used to manually control the turret. A custom turret requires at least one rotor/hinge and one weapon/tool or camera to be functional.
+
+Maximum Range: 2,000 m</Description>
   </Definition>
```

## Mod: Jaes Balance Tweaks (43 differences)

#### Tag: `EntityComponent` | Type: `Inventory` | Subtype: `LargeBlockBulkContainerA`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\Cargo.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\EntityComponents.sbc`

```diff
--- Base Game
+++ Mod
@@ -3,5 +3,5 @@
     <TypeId>Inventory</TypeId>
     <SubtypeId>LargeBlockBulkContainerA</SubtypeId>
     </Id>
-  <Size x="10" y="10" z="8.2" />
+  <Size x="7.5" y="7.5" z="15.2" />
   </EntityComponent>
```

#### Tag: `EntityComponent` | Type: `Inventory` | Subtype: `LargeBlockBulkContainerB`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\Cargo.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\EntityComponents.sbc`

```diff
--- Base Game
+++ Mod
@@ -3,5 +3,5 @@
     <TypeId>Inventory</TypeId>
     <SubtypeId>LargeBlockBulkContainerB</SubtypeId>
     </Id>
-  <Size x="10" y="10" z="8.2" />
+  <Size x="7.5" y="7.5" z="15.2" />
   </EntityComponent>
```

#### Tag: `EntityComponent` | Type: `Inventory` | Subtype: `LargeBlockBulkContainerC`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\Cargo.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\EntityComponents.sbc`

```diff
--- Base Game
+++ Mod
@@ -3,5 +3,5 @@
     <TypeId>Inventory</TypeId>
     <SubtypeId>LargeBlockBulkContainerC</SubtypeId>
     </Id>
-  <Size x="10" y="10" z="8.2" />
+  <Size x="7.5" y="7.5" z="15.2" />
   </EntityComponent>
```

#### Tag: `Definition` | Type: `Assembler` | Subtype: `LargePrototechAssembler`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\PrototechAssembler.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Prototech.sbc`

```diff
--- Base Game
+++ Mod
@@ -75,8 +75,17 @@
   <StandbyPowerConsumption>0.001</StandbyPowerConsumption>
   <OperationalPowerConsumption>4</OperationalPowerConsumption>
   <BlueprintClasses>
+    <Class>LargeBlocks</Class>
+    <Class>SmallBlocks</Class>
     <Class>PrototechComponents</Class>
+    <Class>Components</Class>
+    <Class>EliteEquipment</Class>
+    <Class>EliteTools</Class>
+    <Class>EliteWeapons</Class>
+    <Class>EliteConsumables</Class>
+    <Class>BuildPlanner</Class>
     </BlueprintClasses>
+  <AssemblySpeed>5.5</AssemblySpeed>
   <MirroringY>Z</MirroringY>
   <MirroringZ>Y</MirroringZ>
   <DamageEffectName>Damage_Electrical_Prototech_Assembler</DamageEffectName>
```

#### [ModAdjuster] Tag: `Definition` | Type: `OreDetector` | Subtype: `LargeOreDetector`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Tools.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Tools.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>OreDetector</TypeId>
     <SubtypeId>LargeOreDetector</SubtypeId>
     </Id>
-  <MaximumRange>150</MaximumRange>
+  <MaximumRange>2000</MaximumRange>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `OreDetector` | Subtype: `LargeOreDetectorReskin`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Tools.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_ApexSurvivalPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>OreDetector</TypeId>
     <SubtypeId>LargeOreDetectorReskin</SubtypeId>
     </Id>
-  <MaximumRange>150</MaximumRange>
+  <MaximumRange>2000</MaximumRange>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `OreDetector` | Subtype: `SmallBlockOreDetector`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Tools.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Tools.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>OreDetector</TypeId>
     <SubtypeId>SmallBlockOreDetector</SubtypeId>
     </Id>
-  <MaximumRange>50</MaximumRange>
+  <MaximumRange>2000</MaximumRange>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `OreDetector` | Subtype: `SmallOreDetectorReskin`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Tools.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_ApexSurvivalPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>OreDetector</TypeId>
     <SubtypeId>SmallOreDetectorReskin</SubtypeId>
     </Id>
-  <MaximumRange>50</MaximumRange>
+  <MaximumRange>2000</MaximumRange>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `ShipGrinder` | Subtype: `SmallShipGrinder`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Tools.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Tools.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,6 @@
     <TypeId>ShipGrinder</TypeId>
     <SubtypeId>SmallShipGrinder</SubtypeId>
     </Id>
-  <SensorRadius>1.3</SensorRadius>
+  <SensorRadius>2.5</SensorRadius>
+  <SensorOffset>2.4</SensorOffset>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `ShipGrinder` | Subtype: `SmallShipGrinderReskin`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Tools.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_ApexSurvivalPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,6 @@
     <TypeId>ShipGrinder</TypeId>
     <SubtypeId>SmallShipGrinderReskin</SubtypeId>
     </Id>
-  <SensorRadius>1.3</SensorRadius>
+  <SensorRadius>2.5</SensorRadius>
+  <SensorOffset>10.75</SensorOffset>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `ShipGrinder` | Subtype: `LargeShipGrinder`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Tools.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Tools.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,6 @@
     <TypeId>ShipGrinder</TypeId>
     <SubtypeId>LargeShipGrinder</SubtypeId>
     </Id>
-  <SensorRadius>1.4</SensorRadius>
+  <SensorRadius>5.0</SensorRadius>
+  <SensorOffset>3.25</SensorOffset>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `ShipGrinder` | Subtype: `LargeShipGrinderReskin`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Tools.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_ApexSurvivalPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,6 @@
     <TypeId>ShipGrinder</TypeId>
     <SubtypeId>LargeShipGrinderReskin</SubtypeId>
     </Id>
-  <SensorRadius>1.4</SensorRadius>
+  <SensorRadius>5.0</SensorRadius>
+  <SensorOffset>4.55</SensorOffset>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `ShipWelder` | Subtype: `SmallShipWelder`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Tools.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Tools.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,6 @@
     <TypeId>ShipWelder</TypeId>
     <SubtypeId>SmallShipWelder</SubtypeId>
     </Id>
-  <SensorRadius>1.3</SensorRadius>
+  <SensorRadius>2.5</SensorRadius>
+  <SensorOffset>2.5</SensorOffset>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `ShipWelder` | Subtype: `SmallShipWelderReskin`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Tools.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_ApexSurvivalPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,6 @@
     <TypeId>ShipWelder</TypeId>
     <SubtypeId>SmallShipWelderReskin</SubtypeId>
     </Id>
-  <SensorRadius>1.3</SensorRadius>
+  <SensorRadius>2.5</SensorRadius>
+  <SensorOffset>2.35</SensorOffset>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `ShipWelder` | Subtype: `LargeShipWelder`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Tools.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Tools.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,6 @@
     <TypeId>ShipWelder</TypeId>
     <SubtypeId>LargeShipWelder</SubtypeId>
     </Id>
-  <SensorRadius>2.26</SensorRadius>
+  <SensorRadius>5.0</SensorRadius>
+  <SensorOffset>1.3</SensorOffset>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `ShipWelder` | Subtype: `LargeShipWelderReskin`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Tools.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_ApexSurvivalPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,6 @@
     <TypeId>ShipWelder</TypeId>
     <SubtypeId>LargeShipWelderReskin</SubtypeId>
     </Id>
-  <SensorRadius>2.26</SensorRadius>
+  <SensorRadius>5.0</SensorRadius>
+  <SensorOffset>4.35</SensorOffset>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Drill` | Subtype: `SmallBlockDrill`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Tools.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Tools.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,8 +3,8 @@
     <TypeId>Drill</TypeId>
     <SubtypeId>SmallBlockDrill</SubtypeId>
     </Id>
-  <SensorRadius>1.3</SensorRadius>
-  <SensorOffset>0.8</SensorOffset>
-  <CutOutRadius>1.3</CutOutRadius>
-  <CutOutOffset>0.6</CutOutOffset>
+  <SensorRadius>2.5</SensorRadius>
+  <SensorOffset>2.8</SensorOffset>
+  <CutOutRadius>2.5</CutOutRadius>
+  <CutOutOffset>2.8</CutOutOffset>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Drill` | Subtype: `SmallBlockDrillReskin`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Tools.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_ApexSurvivalPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,8 +3,8 @@
     <TypeId>Drill</TypeId>
     <SubtypeId>SmallBlockDrillReskin</SubtypeId>
     </Id>
-  <SensorRadius>1.3</SensorRadius>
-  <SensorOffset>0.8</SensorOffset>
-  <CutOutRadius>1.3</CutOutRadius>
-  <CutOutOffset>0.6</CutOutOffset>
+  <SensorRadius>2.5</SensorRadius>
+  <SensorOffset>2.8</SensorOffset>
+  <CutOutRadius>2.5</CutOutRadius>
+  <CutOutOffset>2.8</CutOutOffset>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Drill` | Subtype: `LargeBlockDrill`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Tools.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Tools.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,8 +3,8 @@
     <TypeId>Drill</TypeId>
     <SubtypeId>LargeBlockDrill</SubtypeId>
     </Id>
-  <SensorRadius>1.9</SensorRadius>
-  <SensorOffset>2.8</SensorOffset>
-  <CutOutRadius>1.9</CutOutRadius>
-  <CutOutOffset>2.8</CutOutOffset>
+  <SensorRadius>4.5</SensorRadius>
+  <SensorOffset>5.5</SensorOffset>
+  <CutOutRadius>4.5</CutOutRadius>
+  <CutOutOffset>5.5</CutOutOffset>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Drill` | Subtype: `LargeBlockDrillReskin`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Tools.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_ApexSurvivalPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,8 +3,8 @@
     <TypeId>Drill</TypeId>
     <SubtypeId>LargeBlockDrillReskin</SubtypeId>
     </Id>
-  <SensorRadius>1.9</SensorRadius>
-  <SensorOffset>2.8</SensorOffset>
-  <CutOutRadius>1.9</CutOutRadius>
-  <CutOutOffset>2.8</CutOutOffset>
+  <SensorRadius>4.5</SensorRadius>
+  <SensorOffset>5.5</SensorOffset>
+  <CutOutRadius>4.5</CutOutRadius>
+  <CutOutOffset>5.5</CutOutOffset>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Drill` | Subtype: `LargeBlockPrototechDrill`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Tools.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Prototech.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,8 +3,8 @@
     <TypeId>Drill</TypeId>
     <SubtypeId>LargeBlockPrototechDrill</SubtypeId>
     </Id>
-  <SensorRadius>4</SensorRadius>
-  <SensorOffset>3</SensorOffset>
-  <CutOutRadius>6</CutOutRadius>
-  <CutOutOffset>3</CutOutOffset>
+  <SensorRadius>9.5</SensorRadius>
+  <SensorOffset>11</SensorOffset>
+  <CutOutRadius>9.5</CutOutRadius>
+  <CutOutOffset>11</CutOutOffset>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Reactor` | Subtype: `SmallBlockSmallGenerator`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Reactors.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Energy.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -9,6 +9,7 @@
         <TypeId>Ingot</TypeId>
         <SubtypeId>Uranium</SubtypeId>
         </Id>
+      <Ratio>0.1</Ratio>
       </FuelInfo>
     </FuelInfos>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Reactor` | Subtype: `SmallBlockLargeGenerator`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Reactors.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Energy.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -9,6 +9,7 @@
         <TypeId>Ingot</TypeId>
         <SubtypeId>Uranium</SubtypeId>
         </Id>
+      <Ratio>0.1</Ratio>
       </FuelInfo>
     </FuelInfos>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Reactor` | Subtype: `LargeBlockSmallGenerator`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Reactors.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Energy.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -9,6 +9,7 @@
         <TypeId>Ingot</TypeId>
         <SubtypeId>Uranium</SubtypeId>
         </Id>
+      <Ratio>0.1</Ratio>
       </FuelInfo>
     </FuelInfos>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Reactor` | Subtype: `LargeBlockLargeGenerator`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Reactors.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Energy.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -9,6 +9,7 @@
         <TypeId>Ingot</TypeId>
         <SubtypeId>Uranium</SubtypeId>
         </Id>
+      <Ratio>0.1</Ratio>
       </FuelInfo>
     </FuelInfos>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Reactor` | Subtype: `SmallBlockSmallGeneratorWarfare2`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Reactors.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Warfare2.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -9,6 +9,7 @@
         <TypeId>Ingot</TypeId>
         <SubtypeId>Uranium</SubtypeId>
         </Id>
+      <Ratio>0.1</Ratio>
       </FuelInfo>
     </FuelInfos>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Reactor` | Subtype: `SmallBlockLargeGeneratorWarfare2`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Reactors.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Warfare2.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -9,6 +9,7 @@
         <TypeId>Ingot</TypeId>
         <SubtypeId>Uranium</SubtypeId>
         </Id>
+      <Ratio>0.1</Ratio>
       </FuelInfo>
     </FuelInfos>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Reactor` | Subtype: `LargeBlockSmallGeneratorWarfare2`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Reactors.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Warfare2.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -9,6 +9,7 @@
         <TypeId>Ingot</TypeId>
         <SubtypeId>Uranium</SubtypeId>
         </Id>
+      <Ratio>0.1</Ratio>
       </FuelInfo>
     </FuelInfos>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Reactor` | Subtype: `LargeBlockLargeGeneratorWarfare2`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Reactors.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Warfare2.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -9,6 +9,7 @@
         <TypeId>Ingot</TypeId>
         <SubtypeId>Uranium</SubtypeId>
         </Id>
+      <Ratio>0.1</Ratio>
       </FuelInfo>
     </FuelInfos>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Assembler` | Subtype: `LargePrototechAssembler`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Prototech.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Prototech.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,30 +4,18 @@
     <SubtypeId>LargePrototechAssembler</SubtypeId>
     </Id>
   <Components>
-    <Component Subtype="PrototechFrame" Count="1">
-      <DeconstructId>
-        <TypeId>Ingot</TypeId>
-        <SubtypeId>PrototechScrap</SubtypeId>
-        </DeconstructId>
-      </Component>
-    <Component Subtype="PrototechPanel" Count="160" />
-    <Component Subtype="Construction" Count="130" />
-    <Component Subtype="PrototechMachinery" Count="20">
-      <DeconstructId>
-        <TypeId>Ingot</TypeId>
-        <SubtypeId>PrototechScrap</SubtypeId>
-        </DeconstructId>
-      </Component>
-    <Component Subtype="PrototechCoolingUnit" Count="2">
-      <DeconstructId>
-        <TypeId>Ingot</TypeId>
-        <SubtypeId>PrototechScrap</SubtypeId>
-        </DeconstructId>
-      </Component>
-    <Component Subtype="MetalGrid" Count="80" />
-    <Component Subtype="Computer" Count="200" />
-    <Component Subtype="Display" Count="10" />
-    <Component Subtype="PrototechPanel" Count="80" />
+    <Component Subtype="SteelPlate" Count="60" />
+    <Component Subtype="Thrust" Count="200" />
+    <Component Subtype="Reactor" Count="200" />
+    <Component Subtype="Explosives" Count="200" />
+    <Component Subtype="Superconductor" Count="200" />
+    <Component Subtype="SteelPlate" Count="240" />
+    <Component Subtype="Construction" Count="160" />
+    <Component Subtype="Motor" Count="40" />
+    <Component Subtype="Display" Count="20" />
+    <Component Subtype="MetalGrid" Count="60" />
+    <Component Subtype="Computer" Count="320" />
+    <Component Subtype="SteelPlate" Count="40" />
     </Components>
   <CriticalComponent Subtype="Computer" Index="0" />
   </Definition>
```

#### [ModAdjuster] Tag: `Blueprint` | Type: `BlueprintDefinition` | Subtype: `PrototechScrap`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Prototech.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Blueprints.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,6 +4,12 @@
     <SubtypeId>PrototechScrap</SubtypeId>
     </Id>
   <Prerequisites>
-    <Item Amount="1" TypeId="Ingot" SubtypeId="PrototechScrap" />
+    <Item Amount="5" TypeId="Ingot" SubtypeId="Iron" />
+    <Item Amount="5" TypeId="Ingot" SubtypeId="Nickel" />
+    <Item Amount="5" TypeId="Ingot" SubtypeId="Silicon" />
+    <Item Amount="5" TypeId="Ingot" SubtypeId="Cobalt" />
+    <Item Amount="5" TypeId="Ingot" SubtypeId="Silver" />
+    <Item Amount="5" TypeId="Ingot" SubtypeId="Gold" />
+    <Item Amount="5" TypeId="Ingot" SubtypeId="Platinum" />
     </Prerequisites>
   </Blueprint>
```

#### [ModAdjuster] Tag: `Blueprint` | Type: `BlueprintDefinition` | Subtype: `PrototechFrame`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Prototech.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Blueprints.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -4,6 +4,10 @@
     <SubtypeId>PrototechFrame</SubtypeId>
     </Id>
   <Prerequisites>
-    <Item Amount="1" TypeId="Component" SubtypeId="PrototechFrame" />
+    <Item Amount="400" TypeId="Ingot" SubtypeId="Iron" />
+    <Item Amount="70" TypeId="Ingot" SubtypeId="Cobalt" />
+    <Item Amount="20" TypeId="Ingot" SubtypeId="Silver" />
+    <Item Amount="30" TypeId="Ingot" SubtypeId="Gold" />
+    <Item Amount="25" TypeId="Ingot" SubtypeId="Platinum" />
     </Prerequisites>
   </Blueprint>
```

#### [ModAdjuster] Tag: `Blueprint` | Type: `BlueprintDefinition` | Subtype: `PrototechCapacitor`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Prototech.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Blueprints.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -8,6 +8,5 @@
     <Item Amount="4" TypeId="Ingot" SubtypeId="Silicon" />
     <Item Amount="3" TypeId="Ingot" SubtypeId="Silver" />
     <Item Amount="6" TypeId="Ingot" SubtypeId="Gold" />
-    <Item Amount="1.5" TypeId="Ingot" SubtypeId="PrototechScrap" />
     </Prerequisites>
   </Blueprint>
```

#### [ModAdjuster] Tag: `Blueprint` | Type: `BlueprintDefinition` | Subtype: `PrototechPropulsionUnit`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Prototech.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Blueprints.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -8,6 +8,5 @@
     <Item Amount="24" TypeId="Ingot" SubtypeId="Cobalt" />
     <Item Amount="6" TypeId="Ingot" SubtypeId="Gold" />
     <Item Amount="3" TypeId="Ingot" SubtypeId="Platinum" />
-    <Item Amount="1.25" TypeId="Ingot" SubtypeId="PrototechScrap" />
     </Prerequisites>
   </Blueprint>
```

#### [ModAdjuster] Tag: `Blueprint` | Type: `BlueprintDefinition` | Subtype: `PrototechMachinery`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Prototech.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Blueprints.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -8,6 +8,5 @@
     <Item Amount="12" TypeId="Ingot" SubtypeId="Nickel" />
     <Item Amount="7" TypeId="Ingot" SubtypeId="Silicon" />
     <Item Amount="3" TypeId="Ingot" SubtypeId="Gold" />
-    <Item Amount="1.15" TypeId="Ingot" SubtypeId="PrototechScrap" />
     </Prerequisites>
   </Blueprint>
```

#### [ModAdjuster] Tag: `Blueprint` | Type: `BlueprintDefinition` | Subtype: `PrototechCircuitry`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Prototech.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Blueprints.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -8,6 +8,5 @@
     <Item Amount="8" TypeId="Ingot" SubtypeId="Silicon" />
     <Item Amount="2" TypeId="Ingot" SubtypeId="Gold" />
     <Item Amount="1.5" TypeId="Ingot" SubtypeId="Platinum" />
-    <Item Amount="1.75" TypeId="Ingot" SubtypeId="PrototechScrap" />
     </Prerequisites>
   </Blueprint>
```

#### [ModAdjuster] Tag: `Blueprint` | Type: `BlueprintDefinition` | Subtype: `PrototechCoolingUnit`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Prototech.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\Blueprints.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -7,6 +7,5 @@
     <Item Amount="80" TypeId="Ingot" SubtypeId="Iron" />
     <Item Amount="12" TypeId="Ingot" SubtypeId="Gold" />
     <Item Amount="3.25" TypeId="Ingot" SubtypeId="Platinum" />
-    <Item Amount="2.5" TypeId="Ingot" SubtypeId="PrototechScrap" />
     </Prerequisites>
   </Blueprint>
```

#### [ModAdjuster] Tag: `Definition` | Type: `OxygenGenerator` | Subtype: ``
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Generators.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Production.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -9,14 +9,14 @@
         <TypeId>GasProperties</TypeId>
         <SubtypeId>Oxygen</SubtypeId>
         </Id>
-      <IceToGasRatio>10</IceToGasRatio>
+      <IceToGasRatio>50</IceToGasRatio>
       </GasInfo>
     <GasInfo>
       <Id>
         <TypeId>GasProperties</TypeId>
         <SubtypeId>Hydrogen</SubtypeId>
         </Id>
-      <IceToGasRatio>20</IceToGasRatio>
+      <IceToGasRatio>100</IceToGasRatio>
       </GasInfo>
     </ProducedGases>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `OxygenGenerator` | Subtype: `OxygenGeneratorSmall`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Generators.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Production.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -9,14 +9,14 @@
         <TypeId>GasProperties</TypeId>
         <SubtypeId>Oxygen</SubtypeId>
         </Id>
-      <IceToGasRatio>10</IceToGasRatio>
+      <IceToGasRatio>50</IceToGasRatio>
       </GasInfo>
     <GasInfo>
       <Id>
         <TypeId>GasProperties</TypeId>
         <SubtypeId>Hydrogen</SubtypeId>
         </Id>
-      <IceToGasRatio>20</IceToGasRatio>
+      <IceToGasRatio>100</IceToGasRatio>
       </GasInfo>
     </ProducedGases>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `OxygenGenerator` | Subtype: `LargeBlockOxygenGeneratorLab`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Generators.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Fieldwork.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,22 +3,22 @@
     <TypeId>OxygenGenerator</TypeId>
     <SubtypeId>LargeBlockOxygenGeneratorLab</SubtypeId>
     </Id>
-  <IceConsumptionPerSecond>25</IceConsumptionPerSecond>
-  <OperationalPowerConsumption>0.5</OperationalPowerConsumption>
+  <IceConsumptionPerSecond>50</IceConsumptionPerSecond>
+  <OperationalPowerConsumption>1.0</OperationalPowerConsumption>
   <ProducedGases>
     <GasInfo>
       <Id>
         <TypeId>GasProperties</TypeId>
         <SubtypeId>Oxygen</SubtypeId>
         </Id>
-      <IceToGasRatio>10</IceToGasRatio>
+      <IceToGasRatio>50</IceToGasRatio>
       </GasInfo>
     <GasInfo>
       <Id>
         <TypeId>GasProperties</TypeId>
         <SubtypeId>Hydrogen</SubtypeId>
         </Id>
-      <IceToGasRatio>20</IceToGasRatio>
+      <IceToGasRatio>100</IceToGasRatio>
       </GasInfo>
     </ProducedGases>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `OxygenGenerator` | Subtype: `SmallBlockOxygenGeneratorLab`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Generators.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Fieldwork.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -9,14 +9,14 @@
         <TypeId>GasProperties</TypeId>
         <SubtypeId>Oxygen</SubtypeId>
         </Id>
-      <IceToGasRatio>10</IceToGasRatio>
+      <IceToGasRatio>50</IceToGasRatio>
       </GasInfo>
     <GasInfo>
       <Id>
         <TypeId>GasProperties</TypeId>
         <SubtypeId>Hydrogen</SubtypeId>
         </Id>
-      <IceToGasRatio>20</IceToGasRatio>
+      <IceToGasRatio>100</IceToGasRatio>
       </GasInfo>
     </ProducedGases>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `OxygenGenerator` | Subtype: `LargeBlockPrototechOxygenGenerator`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Generators.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Prototech.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -9,14 +9,14 @@
         <TypeId>GasProperties</TypeId>
         <SubtypeId>Oxygen</SubtypeId>
         </Id>
-      <IceToGasRatio>10</IceToGasRatio>
+      <IceToGasRatio>50</IceToGasRatio>
       </GasInfo>
     <GasInfo>
       <Id>
         <TypeId>GasProperties</TypeId>
         <SubtypeId>Hydrogen</SubtypeId>
         </Id>
-      <IceToGasRatio>20</IceToGasRatio>
+      <IceToGasRatio>100</IceToGasRatio>
       </GasInfo>
     </ProducedGases>
   </Definition>
```

#### [ModAdjuster] `BlueprintClassEntries` additions
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Balance Tweaks\Data\ModAdjuster\Prototech.xml`
- **Target**: `BlueprintClasses.sbc`

```diff
--- Base/Existing BlueprintClassEntries
+++ ModAdjuster
+  <Entry Class="PrototechComponents" BlueprintSubtypeId="PrototechFrame" />
```

## Mod: Jaes Block Additions (11 differences)

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section1_Position1_LargeBlocks`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Block Additions\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -7,955 +7,13 @@
   <Name>Section1_Position1_LargeBlocks</Name>
   <StrictSearch>true</StrictSearch>
   <ItemIds>
+    <string>ShipWelder/Aryx_Shiptool_MegaWelder</string>
+    <string>Refinery/StoneCrusher_LB</string>
+    <string>AdvancedDoor/LargeBlockSlideDoorDouble</string>
+    <string>AdvancedDoor/HatchDoorDouble</string>
+    <string>AdvancedDoor/RoundCorridorDoubleDoorAirlock</string>
+    <string>AdvancedDoor/NarrowDoorAirlock</string>
+    <string>AdvancedDoor/CentredDoubleDoorAirlock</string>
+    <string>AdvancedDoor/CentredDoubleDoorGlassAirlock</string>
     </ItemIds>
   </Category>
```

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section1_Position1_SmallBlocks`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Block Additions\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -7,579 +7,6 @@
   <Name>Section1_Position1_SmallBlocks</Name>
   <StrictSearch>true</StrictSearch>
   <ItemIds>
+    <string>Refinery/StoneCrusher_SB</string>
     </ItemIds>
   </Category>
```

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section1_Position3_ShipTools`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Block Additions\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -7,17 +7,6 @@
   <Name>Section1_Position3_ShipTools</Name>
   <StrictSearch>true</StrictSearch>
   <ItemIds>
+    <string>ShipWelder/Aryx_Shiptool_MegaWelder</string>
     </ItemIds>
   </Category>
```

#### Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Section1_Position3_Utility`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Block Additions\Data\BlockCategories.sbc`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\BlockCategories.sbc`

```diff
--- Base Game
+++ Mod
@@ -7,193 +7,7 @@
   <Name>Section1_Position3_Utility</Name>
   <StrictSearch>true</StrictSearch>
   <ItemIds>
+    <string>Refinery/StoneCrusher_LB</string>
+    <string>Refinery/StoneCrusher_SB</string>
     </ItemIds>
   </Category>
```

#### [ModAdjuster] Tag: `Definition` | Type: `ShipWelder` | Subtype: `Aryx_Shiptool_MegaWelder`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Block Additions\Data\ModAdjuster\AryxWelder.xml`
- **Target (Workshop Mod (3325231237))**: `e:\SteamLibrary\steamapps\workshop\content\244850\3325231237\Data\AryxShipTools_CubeBlocks.sbc`

```diff
--- Workshop Mod (3325231237)
+++ ModAdjuster
@@ -3,6 +3,7 @@
     <TypeId>ShipWelder</TypeId>
     <SubtypeId>Aryx_Shiptool_MegaWelder</SubtypeId>
     </Id>
-  <DisplayName>Mega Welder</DisplayName>
-  <SensorRadius>10</SensorRadius>
+  <DisplayName>Station Welder</DisplayName>
+  <SensorRadius>25</SensorRadius>
+  <SensorOffset>-1</SensorOffset>
   </Definition>
```

#### [ModAdjuster] Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `LargeBlocks`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Block Additions\Data\ModAdjuster\AryxWelder.xml`
- **Target (Workshop Mod (794471115))**: `e:\SteamLibrary\steamapps\workshop\content\244850\794471115\Data\BlockCategories.sbc`

```diff
--- Workshop Mod (794471115)
+++ ModAdjuster
@@ -5,7 +5,6 @@
     </Id>
   <DisplayName>DisplayName_Category_LargeBlocks</DisplayName>
   <Name>LargeBlocks</Name>
+  <StrictSearch>true</StrictSearch>
+  <ItemIds />
   </Category>
```

#### [ModAdjuster] Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `SmallBlocks`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Block Additions\Data\ModAdjuster\AryxWelder.xml`
- **Target (Workshop Mod (794471115))**: `e:\SteamLibrary\steamapps\workshop\content\244850\794471115\Data\BlockCategories.sbc`

```diff
--- Workshop Mod (794471115)
+++ ModAdjuster
@@ -5,7 +5,6 @@
     </Id>
   <DisplayName>DisplayName_Category_SmallBlocks</DisplayName>
   <Name>SmallBlocks</Name>
+  <StrictSearch>true</StrictSearch>
+  <ItemIds />
   </Category>
```

#### [ModAdjuster] Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `HumanoidAirlocks`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Block Additions\Data\ModAdjuster\HumanoidAirlocks.xml`
- **Target (Workshop Mod (3454269412))**: `e:\SteamLibrary\steamapps\workshop\content\244850\3454269412\Data\BlockCategories.sbc`

```diff
--- Workshop Mod (3454269412)
+++ ModAdjuster
@@ -5,12 +5,5 @@
     </Id>
   <DisplayName>Double Door Airlocks</DisplayName>
   <Name>HumanoidAirlocks</Name>
+  <ItemIds />
   </Category>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Refinery` | Subtype: `StoneCrusher_LB`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Block Additions\Data\ModAdjuster\ArchanStoneCrushers.xml`
- **Target (Workshop Mod (3245427690))**: `e:\SteamLibrary\steamapps\workshop\content\244850\3245427690\Data\CubeBlocks_StoneCrushers.sbc`

```diff
--- Workshop Mod (3245427690)
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Refinery</TypeId>
     <SubtypeId>StoneCrusher_LB</SubtypeId>
     </Id>
-  <Description>Stone crusher, specialised in refining stone into small amounts of Iron, Nickel and Silicon, but no gravel</Description>
-  <MaterialEfficiency>1</MaterialEfficiency>
+  <Description>Permanantly turns stone to dust</Description>
+  <MaterialEfficiency>0</MaterialEfficiency>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Refinery` | Subtype: `StoneCrusher_SB`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Block Additions\Data\ModAdjuster\ArchanStoneCrushers.xml`
- **Target (Workshop Mod (3245427690))**: `e:\SteamLibrary\steamapps\workshop\content\244850\3245427690\Data\CubeBlocks_StoneCrushers.sbc`

```diff
--- Workshop Mod (3245427690)
+++ ModAdjuster
@@ -3,6 +3,6 @@
     <TypeId>Refinery</TypeId>
     <SubtypeId>StoneCrusher_SB</SubtypeId>
     </Id>
-  <Description>Stone crusher, specialised in refining stone into small amounts of Iron, Nickel and Silicon, but no gravel</Description>
-  <MaterialEfficiency>1</MaterialEfficiency>
+  <Description>Permanantly turns stone to dust</Description>
+  <MaterialEfficiency>0</MaterialEfficiency>
   </Definition>
```

#### [ModAdjuster] Tag: `Category` | Type: `GuiBlockCategoryDefinition` | Subtype: `Archan`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Block Additions\Data\ModAdjuster\ArchanStoneCrushers.xml`
- **Target (Workshop Mod (3245427690))**: `e:\SteamLibrary\steamapps\workshop\content\244850\3245427690\Data\BlocksCategories.sbc`

```diff
--- Workshop Mod (3245427690)
+++ ModAdjuster
@@ -6,7 +6,5 @@
   <DisplayName>Archan's Utilities</DisplayName>
   <Name>Archan</Name>
   <StrictSearch>true</StrictSearch>
+  <ItemIds />
   </Category>
```

## Mod: Jaes Thruster Effects (12 differences)

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockLargeHydrogenThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Thruster Effects\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -5,15 +5,15 @@
     </Id>
   <FlameIdleColor>
     <X>1</X>
-    <Y>0.85</Y>
-    <Z>0.3</Z>
+    <Y>0.75</Y>
+    <Z>0.35</Z>
     <W>0.75</W>
     </FlameIdleColor>
   <FlameFullColor>
     <X>1</X>
     <Y>0.75</Y>
-    <Z>0.2</Z>
+    <Z>0.35</Z>
     <W>0.75</W>
     </FlameFullColor>
-  <FlameLengthMaterial>EngineThrustMiddle</FlameLengthMaterial>
+  <FlameLengthMaterial>SciFiThrusterFlame</FlameLengthMaterial>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockLargeHydrogenThrustIndustrial`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Thruster Effects\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_IndustrialPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -5,15 +5,15 @@
     </Id>
   <FlameIdleColor>
     <X>1</X>
-    <Y>0.85</Y>
-    <Z>0.3</Z>
+    <Y>0.75</Y>
+    <Z>0.35</Z>
     <W>0.75</W>
     </FlameIdleColor>
   <FlameFullColor>
     <X>1</X>
     <Y>0.75</Y>
-    <Z>0.2</Z>
+    <Z>0.35</Z>
     <W>0.75</W>
     </FlameFullColor>
-  <FlameLengthMaterial>EngineThrustMiddle</FlameLengthMaterial>
+  <FlameLengthMaterial>SciFiThrusterFlame</FlameLengthMaterial>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockSmallHydrogenThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Thruster Effects\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -5,15 +5,15 @@
     </Id>
   <FlameIdleColor>
     <X>1</X>
-    <Y>0.85</Y>
-    <Z>0.3</Z>
+    <Y>0.75</Y>
+    <Z>0.35</Z>
     <W>0.75</W>
     </FlameIdleColor>
   <FlameFullColor>
     <X>1</X>
     <Y>0.75</Y>
-    <Z>0.2</Z>
+    <Z>0.35</Z>
     <W>0.75</W>
     </FlameFullColor>
-  <FlameLengthMaterial>EngineThrustMiddle</FlameLengthMaterial>
+  <FlameLengthMaterial>SciFiThrusterFlame</FlameLengthMaterial>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockSmallHydrogenThrustIndustrial`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Thruster Effects\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_IndustrialPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -5,15 +5,15 @@
     </Id>
   <FlameIdleColor>
     <X>1</X>
-    <Y>0.85</Y>
-    <Z>0.3</Z>
+    <Y>0.75</Y>
+    <Z>0.35</Z>
     <W>0.75</W>
     </FlameIdleColor>
   <FlameFullColor>
     <X>1</X>
     <Y>0.75</Y>
-    <Z>0.2</Z>
+    <Z>0.35</Z>
     <W>0.75</W>
     </FlameFullColor>
-  <FlameLengthMaterial>EngineThrustMiddle</FlameLengthMaterial>
+  <FlameLengthMaterial>SciFiThrusterFlame</FlameLengthMaterial>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockLargeHydrogenThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Thruster Effects\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -5,15 +5,15 @@
     </Id>
   <FlameIdleColor>
     <X>1</X>
-    <Y>0.85</Y>
-    <Z>0.3</Z>
+    <Y>0.75</Y>
+    <Z>0.35</Z>
     <W>0.75</W>
     </FlameIdleColor>
   <FlameFullColor>
     <X>1</X>
     <Y>0.75</Y>
-    <Z>0.2</Z>
+    <Z>0.35</Z>
     <W>0.75</W>
     </FlameFullColor>
-  <FlameLengthMaterial>EngineThrustMiddle</FlameLengthMaterial>
+  <FlameLengthMaterial>SciFiThrusterFlame</FlameLengthMaterial>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockLargeHydrogenThrustIndustrial`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Thruster Effects\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_IndustrialPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -5,15 +5,15 @@
     </Id>
   <FlameIdleColor>
     <X>1</X>
-    <Y>0.85</Y>
-    <Z>0.3</Z>
+    <Y>0.75</Y>
+    <Z>0.35</Z>
     <W>0.75</W>
     </FlameIdleColor>
   <FlameFullColor>
     <X>1</X>
     <Y>0.75</Y>
-    <Z>0.2</Z>
+    <Z>0.35</Z>
     <W>0.75</W>
     </FlameFullColor>
-  <FlameLengthMaterial>EngineThrustMiddle</FlameLengthMaterial>
+  <FlameLengthMaterial>SciFiThrusterFlame</FlameLengthMaterial>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockSmallHydrogenThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Thruster Effects\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -5,15 +5,15 @@
     </Id>
   <FlameIdleColor>
     <X>1</X>
-    <Y>0.85</Y>
-    <Z>0.3</Z>
+    <Y>0.75</Y>
+    <Z>0.35</Z>
     <W>0.75</W>
     </FlameIdleColor>
   <FlameFullColor>
     <X>1</X>
     <Y>0.75</Y>
-    <Z>0.2</Z>
+    <Z>0.35</Z>
     <W>0.75</W>
     </FlameFullColor>
-  <FlameLengthMaterial>EngineThrustMiddle</FlameLengthMaterial>
+  <FlameLengthMaterial>SciFiThrusterFlame</FlameLengthMaterial>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockSmallHydrogenThrustIndustrial`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Thruster Effects\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_IndustrialPack.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -5,15 +5,15 @@
     </Id>
   <FlameIdleColor>
     <X>1</X>
-    <Y>0.85</Y>
-    <Z>0.3</Z>
+    <Y>0.75</Y>
+    <Z>0.35</Z>
     <W>0.75</W>
     </FlameIdleColor>
   <FlameFullColor>
     <X>1</X>
     <Y>0.75</Y>
-    <Z>0.2</Z>
+    <Z>0.35</Z>
     <W>0.75</W>
     </FlameFullColor>
-  <FlameLengthMaterial>EngineThrustMiddle</FlameLengthMaterial>
+  <FlameLengthMaterial>SciFiThrusterFlame</FlameLengthMaterial>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockLargeThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Thruster Effects\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>LargeBlockLargeThrust</SubtypeId>
     </Id>
-  <FlameLengthMaterial>EngineThrustMiddle</FlameLengthMaterial>
+  <FlameLengthMaterial>SciFiEngineThrustMiddle</FlameLengthMaterial>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `LargeBlockSmallThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Thruster Effects\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>LargeBlockSmallThrust</SubtypeId>
     </Id>
-  <FlameLengthMaterial>EngineThrustMiddle</FlameLengthMaterial>
+  <FlameLengthMaterial>SciFiEngineThrustMiddle</FlameLengthMaterial>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockLargeThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Thruster Effects\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockLargeThrust</SubtypeId>
     </Id>
-  <FlameLengthMaterial>EngineThrustMiddle</FlameLengthMaterial>
+  <FlameLengthMaterial>SciFiEngineThrustMiddle</FlameLengthMaterial>
   </Definition>
```

#### [ModAdjuster] Tag: `Definition` | Type: `Thrust` | Subtype: `SmallBlockSmallThrust`
- **Mod File**: `c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Jaes Thruster Effects\Data\ModAdjuster\Thrusters.xml`
- **Target (Base Game)**: `e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Thrusters.sbc`

```diff
--- Base Game
+++ ModAdjuster
@@ -3,5 +3,5 @@
     <TypeId>Thrust</TypeId>
     <SubtypeId>SmallBlockSmallThrust</SubtypeId>
     </Id>
-  <FlameLengthMaterial>EngineThrustMiddle</FlameLengthMaterial>
+  <FlameLengthMaterial>SciFiEngineThrustMiddle</FlameLengthMaterial>
   </Definition>
```

