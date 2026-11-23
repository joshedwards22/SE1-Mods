"""Headless SEUT exporter with path, dual-grid, and SBC safety guards.

Usage:
    blender.exe -b "<file.blend>" -P export_blend.py -- [options]

Options:
    --expect-mod <name_or_path>  Assert every exported scene writes inside this mod folder.
    --scene <name>               Export only a single scene (default: all exportable scenes).
    --allow-dual-grid            Allow both export_largeGrid and export_smallGrid enabled.
    --allow-sbc                  Keep scene export_sbc_type (default forces 'none' in memory).
    --dry-run                    Validate scenes and export paths without running export.
"""

import argparse
import os
import sys
import bpy


DEFAULT_MODS_ROOT = r"C:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods"


def parse_args():
    argv = sys.argv[sys.argv.index("--") + 1 :] if "--" in sys.argv else []
    parser = argparse.ArgumentParser(description="Headless SEUT exporter with safety guards.")
    parser.add_argument("--expect-mod", default="", help="Expected mod folder name or absolute path.")
    parser.add_argument("--scene", default="", help="Export only this scene name (default: all).")
    parser.add_argument("--allow-dual-grid", action="store_true", help="Allow both largeGrid and smallGrid enabled.")
    parser.add_argument("--allow-sbc", action="store_true", help="Do not force export_sbc_type='none'.")
    parser.add_argument("--dry-run", action="store_true", help="Validate export settings without exporting.")
    return parser.parse_args(argv)


def resolve_export_path(raw_path):
    if not raw_path:
        return ""
    return os.path.normpath(os.path.abspath(bpy.path.abspath(raw_path)))


def resolve_expected_mod_root(expect_mod):
    if not expect_mod:
        return ""
    if os.path.isabs(expect_mod):
        return os.path.normpath(os.path.abspath(expect_mod))
    return os.path.normpath(os.path.abspath(os.path.join(DEFAULT_MODS_ROOT, expect_mod)))


def is_subpath(path, root):
    try:
        return os.path.commonpath([os.path.normcase(path), os.path.normcase(root)]) == os.path.normcase(root)
    except ValueError:
        return False


def find_existing_ancestor(path):
    cur = path
    while cur and cur != os.path.dirname(cur):
        if os.path.exists(cur):
            return cur
        cur = os.path.dirname(cur)
    return cur


def get_view3d_area():
    for win in bpy.context.window_manager.windows:
        for area in win.screen.areas:
            if area.type == "VIEW_3D":
                return area
    area = bpy.context.window_manager.windows[0].screen.areas[0]
    area.type = "VIEW_3D"
    return area


def main():
    args = parse_args()

    seut_mod = next((m.split(".")[0] for m in sys.modules if "space-engineers-utilities" in m), None)
    assert seut_mod, "SEUT addon not loaded! (Run with BypassSandbox: true if user addons are blocked.)"
    seut_errors = sys.modules[f"{seut_mod}.seut_errors"]

    # Stub popup reports so headless -b mode never hits UI_popup_menu_end crash
    popups = []
    seut_errors.show_popup_report = lambda context, title, text: popups.append((str(title), str(text)))

    if args.scene:
        if args.scene not in bpy.data.scenes:
            raise RuntimeError(f"Scene '{args.scene}' not found in {bpy.data.filepath}")
        target_scenes = [bpy.data.scenes[args.scene]]
    else:
        target_scenes = [s for s in bpy.data.scenes if hasattr(s, "seut")]

    expected_root = resolve_expected_mod_root(args.expect_mod)
    if expected_root and not os.path.isdir(expected_root):
        raise RuntimeError(f"--expect-mod directory does not exist: {expected_root}")

    errors = []
    print(f"=== SEUT EXPORT PLAN: {bpy.data.filepath} ===")
    for scn in target_scenes:
        seut = scn.seut
        scene_type = str(getattr(seut, "sceneType", ""))
        grid_scale = str(getattr(seut, "gridScale", "")).lower()
        large_grid = bool(getattr(seut, "export_largeGrid", False))
        small_grid = bool(getattr(seut, "export_smallGrid", False))
        raw_path = str(getattr(seut, "export_exportPath", ""))
        resolved_path = resolve_export_path(raw_path)

        print(
            f"  {scn.name}: type={scene_type} grid={grid_scale} "
            f"large={large_grid} small={small_grid} path='{resolved_path}'"
        )

        if not resolved_path:
            errors.append(f"Scene '{scn.name}' has empty export_exportPath.")
            continue

        if expected_root and not is_subpath(resolved_path, expected_root):
            errors.append(
                f"Scene '{scn.name}' export path '{resolved_path}' is outside expected mod '{expected_root}'!"
            )

        # Ensure at least the mod folder (or Models dir) exists so we never silently write to a bogus drive/folder
        ancestor = find_existing_ancestor(resolved_path)
        if not os.path.isdir(resolved_path):
            # Allow missing Cubes/large leaf only if its parent Models folder exists
            parent_dir = os.path.dirname(resolved_path)
            if not os.path.isdir(parent_dir):
                errors.append(
                    f"Scene '{scn.name}' export path '{resolved_path}' (and parent) do not exist (nearest existing: '{ancestor}')."
                )

        # Guard against dual-grid checkbox trap (writes LG_/SG_ prefixed files instead of <Scene>.mwm)
        if large_grid and small_grid and not args.allow_dual_grid:
            errors.append(
                f"Scene '{scn.name}' has BOTH export_largeGrid and export_smallGrid enabled "
                f"(will write LG_/SG_ prefixed files instead of '{scn.name}.mwm'). "
                f"Fix the flags or pass --allow-dual-grid."
            )
        elif not large_grid and not small_grid:
            errors.append(f"Scene '{scn.name}' has NEITHER export_largeGrid nor export_smallGrid enabled.")
        elif not args.allow_dual_grid:
            if grid_scale == "large" and not large_grid:
                errors.append(f"Scene '{scn.name}' gridScale='large' but export_largeGrid=False.")
            elif grid_scale == "small" and not small_grid:
                errors.append(f"Scene '{scn.name}' gridScale='small' but export_smallGrid=False.")

    if errors:
        print("SEUT_PREFLIGHT_FAILED:")
        for e in errors:
            print(f"  ERROR: {e}")
        sys.exit(2)

    if args.dry_run:
        print("SEUT_DRY_RUN_OK")
        return

    # Prevent overwriting hand-edited .sbc files unless explicitly requested
    if not args.allow_sbc:
        for scn in bpy.data.scenes:
            if hasattr(scn, "seut"):
                scn.seut.export_sbc_type = "none"

    view3d_area = get_view3d_area()
    with bpy.context.temp_override(area=view3d_area):
        if args.scene:
            bpy.context.window.scene = target_scenes[0]
            res = bpy.ops.scene.export()
        else:
            res = bpy.ops.scene.export_all_scenes()

    print("EXPORT_RESULT:", res)
    error_popups = []
    if popups:
        print("SEUT_POPUPS:")
        for title, text in popups:
            print(f"  [{title}] {text}")
            if "error" in title.lower():
                error_popups.append((title, text))

    assert res == {"FINISHED"}, f"SEUT export returned {res}"
    if error_popups:
        raise RuntimeError(f"SEUT reported {len(error_popups)} error popup(s): {error_popups}")


if __name__ == "__main__":
    main()
