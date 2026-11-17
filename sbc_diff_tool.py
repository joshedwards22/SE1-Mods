import os
import re
import sys
import xml.etree.ElementTree as ET
import difflib
import copy

MOD_FOLDER = r"c:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods"
BASE_GAME_DATA_FOLDER = r"e:\SteamLibrary\steamapps\common\SpaceEngineers\Content\Data"
WORKSHOP_MODS_FOLDER = r"e:\SteamLibrary\steamapps\workshop\content\244850"
SE_USER_DATA_FOLDER = os.path.dirname(MOD_FOLDER)
STEAM_APPMANIFEST_FILE = os.path.abspath(
    os.path.join(BASE_GAME_DATA_FOLDER, "..", "..", "..", "..", "appmanifest_244850.acf")
)
OUTPUT_PREFIX = "Mod_BaseGame_Diffs"


def read_steam_manifest(manifest_path):
    """Read BetaKey, buildid, and LastUpdated from Steam's appmanifest_244850.acf."""
    info = {}
    if not os.path.isfile(manifest_path):
        return info
    try:
        with open(manifest_path, "r", encoding="utf-8", errors="ignore") as f:
            content = f.read()
        for key in ("BetaKey", "buildid", "LastUpdated"):
            match = re.search(rf'"{key}"\s+"([^"]+)"', content)
            if match:
                info[key] = match.group(1).strip()
    except OSError:
        pass
    return info


def get_newest_log_version(user_data_folder):
    """Return (version_str, mtime) from the newest Space Engineers log, or (None, 0)."""
    if not os.path.isdir(user_data_folder):
        return None, 0

    log_files = [
        os.path.join(user_data_folder, name)
        for name in os.listdir(user_data_folder)
        if name == "SpaceEngineers.log"
        or (name.startswith("SpaceEngineers_") and name.endswith(".log"))
    ]
    log_files.sort(key=os.path.getmtime, reverse=True)

    for log_path in log_files:
        try:
            mtime = os.path.getmtime(log_path)
            with open(log_path, "r", encoding="utf-8", errors="ignore") as f:
                for _ in range(30):
                    line = f.readline()
                    if not line:
                        break
                    match = re.search(r"App Version:\s*([^\s]+)", line)
                    if match:
                        raw_version = match.group(1).strip()
                        parts = raw_version.split("_")
                        if len(parts) == 3 and parts[0].isdigit():
                            return f"{int(parts[0])}.{parts[1]}.{parts[2]}", mtime
                        return re.sub(r'[<>:"/\\|?*]+', "_", raw_version), mtime
        except OSError:
            continue

    return None, 0


def get_game_version(user_data_folder, manifest_path):
    """Determine installed game version from CLI arg, Steam manifest, or game log."""
    if len(sys.argv) > 1 and sys.argv[1].strip():
        return re.sub(r'[<>:"/\\|?*]+', "_", sys.argv[1].strip())

    manifest = read_steam_manifest(manifest_path)
    beta_key = manifest.get("BetaKey", "")
    if beta_key and beta_key.lower() != "public":
        return re.sub(r'[<>:"/\\|?*]+', "_", beta_key)

    log_version, log_mtime = get_newest_log_version(user_data_folder)
    last_updated = int(manifest["LastUpdated"]) if manifest.get("LastUpdated", "").isdigit() else 0
    if log_version and log_mtime >= last_updated:
        return log_version

    if manifest.get("buildid"):
        return f"build_{manifest['buildid']}"

    return log_version or "unknown"


def get_id(elem):
    """Extract (type_id, subtype_id) from an element's <Id> child, or None."""
    id_tag = elem.find("Id")
    if id_tag is None:
        return None

    type_id_elem = id_tag.find("TypeId")
    subtype_id_elem = id_tag.find("SubtypeId")

    type_id = None
    subtype_id = ""

    if type_id_elem is not None:
        type_id = type_id_elem.text if type_id_elem.text else ""
    elif "Type" in id_tag.attrib:
        type_id = id_tag.attrib["Type"]

    if subtype_id_elem is not None:
        subtype_id = subtype_id_elem.text if subtype_id_elem.text else ""
    elif "Subtype" in id_tag.attrib:
        subtype_id = id_tag.attrib["Subtype"]

    type_id = type_id.strip() if type_id is not None else None
    subtype_id = subtype_id.strip() if subtype_id is not None else ""

    if type_id is not None:
        return (type_id, subtype_id)
    return None


def get_definition_key(elem):
    """Return a unique definition key, matching GuiBlockCategoryDefinition by <Name>."""
    id_tuple = get_id(elem)
    if id_tuple is None:
        return None

    type_id, subtype_id = id_tuple
    if type_id == "GuiBlockCategoryDefinition":
        name_elem = elem.find("Name")
        if name_elem is not None and name_elem.text and name_elem.text.strip():
            return (elem.tag, type_id, name_elem.text.strip())

    return (elem.tag, type_id, subtype_id)


def parse_xml_stripped(filepath):
    """Parse an XML file and strip XML namespaces from tags and attributes."""
    it = ET.iterparse(filepath)
    for _, el in it:
        if "}" in el.tag:
            el.tag = el.tag.split("}", 1)[1]
        for attr_name in list(el.attrib.keys()):
            if "}" in attr_name:
                new_name = attr_name.split("}", 1)[1]
                el.attrib[new_name] = el.attrib.pop(attr_name)
    return it.root


def collect_top_definitions(elem, filepath, out_list):
    """Collect top-level <Id>-bearing definition elements without descending into child items."""
    if elem.tag != "Id" and elem.find("Id") is not None:
        key = get_definition_key(elem)
        if key is not None:
            out_list.append((key, filepath, elem))
        return

    for child in elem:
        collect_top_definitions(child, filepath, out_list)


def collect_blueprint_class_entries(root, filepath, out_set):
    """Collect (Class, BlueprintSubtypeId) pairs from <BlueprintClassEntries>."""
    for entries_elem in root.findall(".//BlueprintClassEntries"):
        for entry in entries_elem.findall("Entry"):
            cls_name = (entry.attrib.get("Class") or "").strip()
            bp_sub = (entry.attrib.get("BlueprintSubtypeId") or "").strip()
            if cls_name or bp_sub:
                out_set.add((cls_name, bp_sub))


def extract_sbc_definitions_from_folder(folder, skip_modadjuster=True):
    """Extract .sbc definitions and blueprint class entries from a folder tree."""
    definitions = {}
    bp_entries = set()
    file_count = 0

    for root_dir, dirs, files in os.walk(folder):
        if skip_modadjuster and os.path.basename(root_dir).lower() == "modadjuster":
            continue
        for file in files:
            if not file.lower().endswith(".sbc"):
                continue
            filepath = os.path.join(root_dir, file)
            try:
                root = parse_xml_stripped(filepath)
                file_count += 1
                found = []
                collect_top_definitions(root, filepath, found)
                for full_key, fp, elem in found:
                    definitions[full_key] = (fp, elem)
                collect_blueprint_class_entries(root, filepath, bp_entries)
            except Exception:
                pass

    return definitions, bp_entries, file_count


def get_modadjuster_files(mod_folder):
    """Return list of XML file paths configured in Data/ModAdjuster."""
    ma_dir = os.path.join(mod_folder, "Data", "ModAdjuster")
    if not os.path.isdir(ma_dir):
        return []

    list_file = os.path.join(ma_dir, "ModAdjusterFiles.txt")
    xml_paths = []
    if os.path.isfile(list_file):
        try:
            with open(list_file, "r", encoding="utf-8", errors="ignore") as f:
                for line in f:
                    name = line.strip()
                    if not name or name.startswith("#") or name.startswith("//"):
                        continue
                    candidate = os.path.join(ma_dir, name)
                    if os.path.isfile(candidate) and candidate.lower().endswith(".xml"):
                        xml_paths.append(candidate)
        except OSError:
            pass

    if not xml_paths:
        for name in sorted(os.listdir(ma_dir)):
            if name.lower().endswith(".xml"):
                xml_paths.append(os.path.join(ma_dir, name))

    return xml_paths


def extract_modadjuster_from_mod(mod_folder):
    """Extract ModAdjuster definition edits and BlueprintClassEntries from a mod."""
    defs = []
    bp_entries = []
    xml_paths = get_modadjuster_files(mod_folder)

    for filepath in xml_paths:
        try:
            root = parse_xml_stripped(filepath)
            found = []
            collect_top_definitions(root, filepath, found)
            defs.extend(found)
            for entries_elem in root.findall(".//BlueprintClassEntries"):
                for entry in entries_elem.findall("Entry"):
                    cls_name = (entry.attrib.get("Class") or "").strip()
                    bp_sub = (entry.attrib.get("BlueprintSubtypeId") or "").strip()
                    if cls_name or bp_sub:
                        bp_entries.append((cls_name, bp_sub, filepath, entry))
        except Exception:
            pass

    return defs, bp_entries, len(xml_paths)


def extract_mods(mods_folder):
    """Scan all user mods for both .sbc definitions and ModAdjuster XML edits."""
    mods = {}
    total_sbc = 0
    total_xml = 0
    print(f"Scanning {mods_folder}...")

    for entry in sorted(os.scandir(mods_folder), key=lambda e: e.name.lower()):
        if not entry.is_dir() or entry.name.startswith(".") or entry.name == "244850":
            continue
        sbc_defs, sbc_bp_entries, sbc_count = extract_sbc_definitions_from_folder(entry.path)
        ma_defs, ma_bp_entries, xml_count = extract_modadjuster_from_mod(entry.path)
        if sbc_defs or ma_defs or ma_bp_entries:
            mods[entry.name] = {
                "sbc_defs": sbc_defs,
                "sbc_bp_entries": sbc_bp_entries,
                "ma_defs": ma_defs,
                "ma_bp_entries": ma_bp_entries,
            }
        total_sbc += sbc_count
        total_xml += xml_count

    print(f"Scanned {total_sbc} .sbc files and {total_xml} ModAdjuster .xml files across {len(mods)} mods in {mods_folder}.")
    return mods


def extract_base_game(folder):
    """Extract all base game .sbc definitions and blueprint class entries."""
    print(f"Scanning {folder}...")
    defs, bp_entries, file_count = extract_sbc_definitions_from_folder(folder, skip_modadjuster=False)
    print(f"Scanned {file_count} .sbc files in {folder}.")
    return defs, bp_entries


def extract_workshop_mods(workshop_folder):
    """Extract .sbc definitions and blueprint class entries from Steam Workshop mods."""
    workshop_defs = {}
    workshop_bp_entries = set()
    total_files = 0
    if not os.path.isdir(workshop_folder):
        return workshop_defs, workshop_bp_entries

    print(f"Scanning workshop mods in {workshop_folder}...")
    for entry in os.scandir(workshop_folder):
        if not entry.is_dir():
            continue
        defs, bp_entries, file_count = extract_sbc_definitions_from_folder(entry.path)
        total_files += file_count
        for k, (fp, elem) in defs.items():
            workshop_defs[k] = (entry.name, fp, elem)
        workshop_bp_entries.update(bp_entries)

    print(f"Scanned {total_files} .sbc files in {workshop_folder} ({len(workshop_defs)} definitions).")
    return workshop_defs, workshop_bp_entries


def indent(elem, level=0):
    """Pretty-indent XML ElementTree in place."""
    i = "\n" + level * "  "
    if len(elem):
        if not elem.text or not elem.text.strip():
            elem.text = i + "  "
        if not elem.tail or not elem.tail.strip():
            elem.tail = i
        for child in elem:
            indent(child, level + 1)
        if not elem.tail or not elem.tail.strip():
            elem.tail = i
    else:
        if level and (not elem.tail or not elem.tail.strip()):
            elem.tail = i


def get_formatted_xml(elem):
    """Return consistently formatted XML string for an element."""
    elem_copy = copy.deepcopy(elem)
    for e in elem_copy.iter():
        if e.text:
            e.text = e.text.strip()
        if e.tail:
            e.tail = e.tail.strip()
    indent(elem_copy)
    return ET.tostring(elem_copy, encoding="unicode")


def build_partial_target_element(target_elem, adjuster_elem):
    """Filter target_elem down to <Id> plus only child tags present in adjuster_elem."""
    filtered = ET.Element(target_elem.tag, dict(target_elem.attrib))
    override_tags = [child.tag for child in adjuster_elem if child.tag != "Id"]
    seen_tags = set()

    # Keep the adjuster's <Id> representation when present so attribute vs tag style doesn't noise the diff
    id_child = adjuster_elem.find("Id")
    if id_child is None:
        id_child = target_elem.find("Id")
    if id_child is not None:
        filtered.append(copy.deepcopy(id_child))

    for tag in override_tags:
        if tag in seen_tags:
            continue
        seen_tags.add(tag)
        for match in target_elem.findall(tag):
            filtered.append(copy.deepcopy(match))

    return filtered


def resolve_target(full_key, current_mod_name, base_defs, user_sbc_index, get_workshop_fn):
    """Find the target definition for a ModAdjuster override across base, user mods, and workshop."""
    if full_key in base_defs:
        fp, elem = base_defs[full_key]
        other_user_mods = [
            mod_name for mod_name, _, _ in user_sbc_index.get(full_key, [])
            if mod_name != current_mod_name
        ]
        source_label = "Base Game"
        if other_user_mods:
            source_label += f" (also overridden in: {', '.join(other_user_mods)})"
        return source_label, fp, elem

    user_hits = user_sbc_index.get(full_key, [])
    if user_hits:
        # Prefer another user mod's definition, else fall back to same mod's .sbc
        chosen = next((h for h in user_hits if h[0] != current_mod_name), user_hits[0])
        return f"User Mod ({chosen[0]})", chosen[1], chosen[2]

    workshop_defs, _ = get_workshop_fn()
    if full_key in workshop_defs:
        ws_id, fp, elem = workshop_defs[full_key]
        return f"Workshop Mod ({ws_id})", fp, elem

    return None, None, None


def format_diff_block(title, mod_filepath, target_label, target_filepath, diff_lines):
    """Format a markdown diff section for a single definition."""
    lines = [
        f"#### {title}\n",
        f"- **Mod File**: `{mod_filepath}`\n",
    ]
    if target_filepath:
        lines.append(f"- **Target ({target_label})**: `{target_filepath}`\n\n")
    else:
        lines.append(f"- **Target**: `{target_label}`\n\n")
    lines.append("```diff\n")
    lines.append("\n".join(diff_lines))
    lines.append("\n```\n\n")
    return "".join(lines)


def main():
    game_version = get_game_version(SE_USER_DATA_FOLDER, STEAM_APPMANIFEST_FILE)
    output_file = os.path.join(MOD_FOLDER, f"{OUTPUT_PREFIX}_{game_version}.md")

    mods = extract_mods(MOD_FOLDER)
    base_defs, base_bp_entries = extract_base_game(BASE_GAME_DATA_FOLDER)

    user_sbc_index = {}
    all_user_bp_entries = set(base_bp_entries)
    for mod_name, data in sorted(mods.items()):
        for k, (fp, elem) in data["sbc_defs"].items():
            user_sbc_index.setdefault(k, []).append((mod_name, fp, elem))
        all_user_bp_entries.update(data["sbc_bp_entries"])

    workshop_cache = {}

    def get_workshop():
        if "data" not in workshop_cache:
            workshop_cache["data"] = extract_workshop_mods(WORKSHOP_MODS_FOLDER)
        return workshop_cache["data"]

    total_mod_defs = sum(len(d["sbc_defs"]) + len(d["ma_defs"]) for d in mods.values())
    print(f"Detected Space Engineers version: {game_version}")
    print(f"Found {total_mod_defs} definitions/adjustments across {len(mods)} mods.")
    print(f"Found {len(base_defs)} definitions in Base Game.")

    os.makedirs(os.path.dirname(output_file), exist_ok=True)

    with open(output_file, "w", encoding="utf-8") as out:
        out.write(f"# Space Engineers Mod vs Base Game Diffs ({game_version})\n\n")
        out.write(
            "This document shows the differences between `.sbc` definitions and `ModAdjuster` `.xml` edits "
            "in your `Mods` folder and their targets in Base Game, your other mods, or Steam Workshop mods. "
            "Whitespace-only differences are ignored.\n\n"
        )

        diff_count = 0

        for mod_name, data in sorted(mods.items()):
            mod_diff_texts = []

            # 1. Full .sbc overrides against Base Game
            for full_key, (mod_filepath, mod_elem) in data["sbc_defs"].items():
                if full_key not in base_defs:
                    continue

                base_filepath, base_elem = base_defs[full_key]
                tag, type_id, subtype_id = full_key

                mod_xml = get_formatted_xml(mod_elem).splitlines()
                base_xml = get_formatted_xml(base_elem).splitlines()

                if mod_xml == base_xml:
                    continue

                diff = list(difflib.unified_diff(base_xml, mod_xml, fromfile="Base Game", tofile="Mod", lineterm=""))

                if type_id == "GuiBlockCategoryDefinition":
                    diff = [line for line in diff if not (line.startswith("-") and not line.startswith("---"))]
                    if not any(line.startswith("+") and not line.startswith("+++") for line in diff):
                        continue

                title = f"Tag: `{tag}` | Type: `{type_id}` | Subtype: `{subtype_id}`"
                mod_diff_texts.append(format_diff_block(title, mod_filepath, "Base Game", base_filepath, diff))
                diff_count += 1

            # 2. ModAdjuster .xml partial overrides against Base Game / User Mods / Workshop Mods
            for full_key, mod_filepath, ma_elem in data["ma_defs"]:
                tag, type_id, subtype_id = full_key
                target_label, target_fp, target_elem = resolve_target(
                    full_key, mod_name, base_defs, user_sbc_index, get_workshop
                )

                if target_elem is None:
                    title = f"[ModAdjuster - MISSING TARGET] Tag: `{tag}` | Type: `{type_id}` | Subtype: `{subtype_id}`"
                    ma_xml = get_formatted_xml(ma_elem).splitlines()
                    diff = list(difflib.unified_diff([], ma_xml, fromfile="Missing Target", tofile="ModAdjuster", lineterm=""))
                    mod_diff_texts.append(format_diff_block(title, mod_filepath, "Not found in Base/Mods/Workshop", None, diff))
                    diff_count += 1
                    continue

                partial_target = build_partial_target_element(target_elem, ma_elem)
                target_xml = get_formatted_xml(partial_target).splitlines()
                ma_xml = get_formatted_xml(ma_elem).splitlines()

                if target_xml == ma_xml:
                    continue

                diff = list(difflib.unified_diff(target_xml, ma_xml, fromfile=target_label, tofile="ModAdjuster", lineterm=""))
                if type_id == "GuiBlockCategoryDefinition":
                    diff = [line for line in diff if not (line.startswith("-") and not line.startswith("---"))]
                    if not any(line.startswith("+") and not line.startswith("+++") for line in diff):
                        continue

                title = f"[ModAdjuster] Tag: `{tag}` | Type: `{type_id}` | Subtype: `{subtype_id}`"
                mod_diff_texts.append(format_diff_block(title, mod_filepath, target_label, target_fp, diff))
                diff_count += 1

            # 3. ModAdjuster <BlueprintClassEntries> additions
            if data["ma_bp_entries"]:
                _, workshop_bp_entries = get_workshop() if workshop_cache else ({}, set())
                known_entries = all_user_bp_entries | workshop_bp_entries
                new_entries = [e for e in data["ma_bp_entries"] if (e[0], e[1]) not in known_entries]
                if new_entries:
                    by_file = {}
                    for cls_name, bp_sub, fp, entry_el in new_entries:
                        by_file.setdefault(fp, []).append(ET.tostring(entry_el, encoding="unicode").strip())
                    for fp, entry_lines in by_file.items():
                        diff = ["--- Base/Existing BlueprintClassEntries", "+++ ModAdjuster"] + [f"+  {ln}" for ln in entry_lines]
                        title = "[ModAdjuster] `BlueprintClassEntries` additions"
                        mod_diff_texts.append(format_diff_block(title, fp, "BlueprintClasses.sbc", None, diff))
                        diff_count += 1

            if mod_diff_texts:
                out.write(f"## Mod: {mod_name} ({len(mod_diff_texts)} differences)\n\n")
                for diff_text in mod_diff_texts:
                    out.write(diff_text)

        print(f"Done! Wrote {diff_count} diffs to {output_file}")


if __name__ == "__main__":
    main()
