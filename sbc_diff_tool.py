import os
import xml.etree.ElementTree as ET
import difflib
import copy

MOD_FOLDER = r"c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods"
BASE_GAME_DATA_FOLDER = r"e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data"
OUTPUT_FILE = r"c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\Mod_BaseGame_Diffs.md"

def get_id(elem):
    id_tag = elem.find('Id')
    if id_tag is None:
        return None
    
    type_id_elem = id_tag.find('TypeId')
    subtype_id_elem = id_tag.find('SubtypeId')
    
    type_id = None
    subtype_id = ""
    
    if type_id_elem is not None:
        type_id = type_id_elem.text if type_id_elem.text else ""
    elif 'Type' in id_tag.attrib:
        type_id = id_tag.attrib['Type']
        
    if subtype_id_elem is not None:
        subtype_id = subtype_id_elem.text if subtype_id_elem.text else ""
    elif 'Subtype' in id_tag.attrib:
        subtype_id = id_tag.attrib['Subtype']
        
    type_id = type_id.strip() if type_id is not None else None
    subtype_id = subtype_id.strip() if subtype_id is not None else ""
    
    if type_id is not None:
        return (type_id, subtype_id)
    return None

def extract_definitions_from_folder(folder):
    """Extract definitions from a single folder (non-recursive). Returns {full_key: (filepath, elem)}."""
    definitions = {}
    file_count = 0
    for root_dir, dirs, files in os.walk(folder):
        for file in files:
            if file.lower().endswith('.sbc'):
                filepath = os.path.join(root_dir, file)
                try:
                    it = ET.iterparse(filepath)
                    for _, el in it:
                        if '}' in el.tag:
                            el.tag = el.tag.split('}', 1)[1]
                        for attr_name in list(el.attrib.keys()):
                            if '}' in attr_name:
                                new_name = attr_name.split('}', 1)[1]
                                el.attrib[new_name] = el.attrib.pop(attr_name)
                    root = it.root
                    file_count += 1
                    for elem in root.iter():
                        if elem.tag == 'Id': continue
                        id_tag = elem.find('Id')
                        if id_tag is not None:
                            id_tuple = get_id(elem)
                            if id_tuple is not None:
                                full_key = (elem.tag, id_tuple[0], id_tuple[1])
                                definitions[full_key] = (filepath, elem)
                except Exception as e:
                    pass
    return definitions, file_count

def extract_mods(mods_folder):
    """Returns {mod_name: {full_key: (filepath, elem)}} - one dict per mod subfolder."""
    mods = {}
    total_files = 0
    print(f"Scanning {mods_folder}...")
    for entry in os.scandir(mods_folder):
        if entry.is_dir():
            mod_defs, file_count = extract_definitions_from_folder(entry.path)
            if mod_defs:
                mods[entry.name] = mod_defs
            total_files += file_count
    print(f"Scanned {total_files} .sbc files across {len(mods)} mods in {mods_folder}.")
    return mods

def extract_base_game(folder):
    print(f"Scanning {folder}...")
    defs, file_count = extract_definitions_from_folder(folder)
    print(f"Scanned {file_count} .sbc files in {folder}.")
    return defs

def indent(elem, level=0):
    i = "\n" + level*"  "
    if len(elem):
        if not elem.text or not elem.text.strip():
            elem.text = i + "  "
        if not elem.tail or not elem.tail.strip():
            elem.tail = i
        for elem in elem:
            indent(elem, level+1)
        if not elem.tail or not elem.tail.strip():
            elem.tail = i
    else:
        if level and (not elem.tail or not elem.tail.strip()):
            elem.tail = i

def get_formatted_xml(elem):
    elem_copy = copy.deepcopy(elem)
    for e in elem_copy.iter():
        if e.text: e.text = e.text.strip()
        if e.tail: e.tail = e.tail.strip()
    indent(elem_copy)
    return ET.tostring(elem_copy, encoding='unicode')

def main():
    mods = extract_mods(MOD_FOLDER)
    base_defs = extract_base_game(BASE_GAME_DATA_FOLDER)
    
    total_mod_defs = sum(len(v) for v in mods.values())
    print(f"Found {total_mod_defs} definitions across {len(mods)} mods.")
    print(f"Found {len(base_defs)} definitions in Base Game.")
    
    os.makedirs(os.path.dirname(OUTPUT_FILE), exist_ok=True)
    
    with open(OUTPUT_FILE, 'w', encoding='utf-8') as out:
        out.write("# Space Engineers Mod vs Base Game Diffs\n\n")
        out.write("This document shows the differences between definitions in your `Mods` folder and their original versions in the base game `Data` folder. Whitespace-only differences are ignored.\n\n")
        
        diff_count = 0
        
        for mod_name, mod_defs in sorted(mods.items()):
            mod_diff_texts = []
            
            for full_key, (mod_filepath, mod_elem) in mod_defs.items():
                if full_key not in base_defs:
                    continue
                
                base_filepath, base_elem = base_defs[full_key]
                tag, type_id, subtype_id = full_key
                
                mod_xml = get_formatted_xml(mod_elem).splitlines()
                base_xml = get_formatted_xml(base_elem).splitlines()
                
                if mod_xml == base_xml:
                    continue
                    
                diff = list(difflib.unified_diff(base_xml, mod_xml, fromfile='Base Game', tofile='Mod', lineterm=''))
                
                if type_id == 'GuiBlockCategoryDefinition':
                    diff = [line for line in diff if not (line.startswith('-') and not line.startswith('---'))]
                    if not any(line.startswith('+') and not line.startswith('+++') for line in diff):
                        continue
                
                diff_text = f"#### Tag: `{tag}` | Type: `{type_id}` | Subtype: `{subtype_id}`\n"
                diff_text += f"- **Mod File**: `{mod_filepath}`\n"
                diff_text += f"- **Base File**: `{base_filepath}`\n\n"
                diff_text += "```diff\n"
                diff_text += "\n".join(diff)
                diff_text += "\n```\n\n"
                
                mod_diff_texts.append(diff_text)
                diff_count += 1
            
            if mod_diff_texts:
                out.write(f"## Mod: {mod_name} ({len(mod_diff_texts)} differences)\n\n")
                for diff_text in mod_diff_texts:
                    out.write(diff_text)
                
        print(f"Done! Wrote {diff_count} diffs to {OUTPUT_FILE}")

if __name__ == '__main__':
    main()
