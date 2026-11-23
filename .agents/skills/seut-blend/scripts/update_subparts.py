"""Headless SEUT 'Update Subparts': rebuild the '(L)' subpart preview instances, drop orphaned copies.

Usage:
  blender -b <file.blend> -P update_subparts.py -- [--scene MissileTurret] [--save]
Without --scene, every scene that has subpart_* empties with a linkedScene is refreshed (main scenes first).
Requires the SEUT addon (run with sandbox bypass if user addons are blocked).
"""
import bpy, sys, argparse


def parse_args():
    argv = sys.argv[sys.argv.index("--") + 1:] if "--" in sys.argv else []
    p = argparse.ArgumentParser()
    p.add_argument("--scene", action="append", default=[])
    p.add_argument("--save", action="store_true")
    return p.parse_args(argv)


def get_view3d_area():
    for win in bpy.context.window_manager.windows:
        for area in win.screen.areas:
            if area.type == "VIEW_3D":
                return area
    area = bpy.context.window_manager.windows[0].screen.areas[0]
    area.type = "VIEW_3D"
    return area


def preview_children(scn):
    out = []
    for o in scn.objects:
        if o.type == 'EMPTY' and o.name.startswith("subpart_") and "(L)" not in o.name:
            out += [c.name for c in o.children_recursive if c.name in scn.objects]
    return out


def main():
    a = parse_args()
    seut_mod = next((m.split(".")[0] for m in sys.modules if "space-engineers-utilities" in m), None)
    assert seut_mod, "SEUT addon not loaded (retry with sandbox bypass)"
    seut_errors = sys.modules[f"{seut_mod}.seut_errors"]
    popups = []
    seut_errors.show_popup_report = lambda context, title, text: popups.append((str(title), str(text)))

    if a.scene:
        scenes = [bpy.data.scenes[n] for n in a.scene]
    else:
        scenes = [s for s in bpy.data.scenes
                  if any(o.type == 'EMPTY' and o.name.startswith("subpart_") and "(L)" not in o.name
                         and getattr(o.seut, "linkedScene", None) for o in s.objects)]
        scenes.sort(key=lambda s: s.seut.sceneType != 'mainScene')
    area = get_view3d_area()
    for scn in scenes:
        before = preview_children(scn)
        bpy.context.window.scene = scn
        with bpy.context.temp_override(area=area):
            scn.seut.linkSubpartInstances = False
            scn.seut.linkSubpartInstances = True
        after = preview_children(scn)
        print(f"UPDATED {scn.name}: previews {len(before)} -> {len(after)}")
    removed = []
    for m in list(bpy.data.meshes):
        if m.users == 0 and m.library is None:
            removed.append(m.name)
            bpy.data.meshes.remove(m)
    print("ORPHAN_MESHES_REMOVED", len(removed), removed[:20])
    print("POPUPS", popups)
    if a.save:
        bpy.ops.wm.save_mainfile()
        print("SAVED", bpy.data.filepath)


main()
