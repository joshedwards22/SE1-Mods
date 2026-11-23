The mods folder contains my mods that I am working on. This is the code I want to be edited. The E:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data folder is the game files for the game, use it as a reference for the mods, do not edit the files there. The E:\SteamLibrary\steamapps\common\SpaceEngineersModSDK\OriginalContent folder is the read-only vanilla ModSDK content (including precompiled vanilla .hkt collision files), use it as a reference as well but do not edit files there. The E:\SteamLibrary\steamapps\workshop\content\244850 folder is where other mods from other developers are, use these as reference as well but do not edit files there. Do not add a compile step to your verification because this project is not compilable. Use the following links for references:

https://spaceengineers.wiki.gg/wiki/Modding/Reference
https://keensoftwarehouse.github.io/SpaceEngineersModAPI/api/index.html
https://github.com/KeenSoftwareHouse/SpaceEngineers
https://github.com/THDigi/SE-ModScript-Examples

To help debug issues the game logs can be found here C:\Users\joshe\AppData\Roaming\SpaceEngineers. They are named like this SpaceEngineers_20261116_180739821.log with the middle field being the date. You can tell the latest by the number in the name and the date modified. Ask the user to start the game with a mod to get a log generated.

When adding or editing blocks in .sbc files, verify unique CubeSize|BlockPairName pairings, mirroring tags (<MirroringX/Y/Z>) for asymmetric shapes, and progression entries (ResearchBlocks.sbc / ResearchGroups.sbc).

Source Blender (.blend) models live in the Models folder (do not edit unless asked), and Blender is installed at C:\Program Files\Blender Foundation\Blender 4.5\blender.exe. For parsing, visualizing, creating, modifying, or debugging .blend/SEUT files, use the seut-blend skill. I do not want LODs in the models.