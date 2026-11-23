"""Render a [Main | BS1 | BS2 | BS3 | COL] stage contact sheet across scenes + world-space JSON audit.

Usage:
    blender.exe -b "<file.blend>" -P render_stages.py -- <out.png> [out.json] [panel_size] [samples]
    blender.exe -b "<file.blend>" -P render_stages.py -- --output <out.png> [--json <out.json>] [--scene <name>] [--size 300] [--samples 8] [--back]
"""

import argparse
import bmesh
import json
import os
import sys
import bpy
import numpy as np
from mathutils import Vector

SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))
if SCRIPT_DIR not in sys.path:
    sys.path.insert(0, SCRIPT_DIR)
import render_blend as rb


def parse_args():
    argv = sys.argv[sys.argv.index("--") + 1 :] if "--" in sys.argv else []
    # Support legacy positional invocation: <out_png> [out_json] [panel_size] [samples]
    if argv and not argv[0].startswith("-"):
        return argparse.Namespace(
            output=argv[0],
            json=argv[1] if len(argv) > 1 else "",
            size=int(argv[2]) if len(argv) > 2 else 300,
            samples=int(argv[3]) if len(argv) > 3 else 8,
            scene="",
            back=False,
        )
    parser = argparse.ArgumentParser(description="Render SEUT [Main | BS1..3 | COL] contact sheet.")
    parser.add_argument("--output", required=True, help="Output PNG path.")
    parser.add_argument("--json", default="", help="Optional world-space JSON report path.")
    parser.add_argument("--scene", default="", help="Render only this scene (default: all scenes).")
    parser.add_argument("--size", type=int, default=300, help="Square panel size in pixels.")
    parser.add_argument("--samples", type=int, default=8, help="Cycles samples per panel.")
    parser.add_argument("--back", action="store_true", help="Render from opposite (-X, +Y, -Z) angle.")
    return parser.parse_args(argv)


def obj_world_mat(obj):
    m = obj.matrix_basis.copy()
    cur = obj
    while cur.parent is not None:
        m = cur.parent.matrix_basis @ cur.matrix_parent_inverse @ m
        cur = cur.parent
    return m


def world_bbox_of_meshes(meshes):
    min_v = Vector((float("inf"), float("inf"), float("inf")))
    max_v = Vector((float("-inf"), float("-inf"), float("-inf")))
    found = False
    for obj in meshes:
        if obj.type != "MESH" or not obj.data or len(obj.data.vertices) == 0:
            continue
        mat = obj_world_mat(obj)
        for corner in obj.bound_box:
            pt = mat @ Vector(corner)
            min_v.x = min(min_v.x, pt.x)
            min_v.y = min(min_v.y, pt.y)
            min_v.z = min(min_v.z, pt.z)
            max_v.x = max(max_v.x, pt.x)
            max_v.y = max(max_v.y, pt.y)
            max_v.z = max(max_v.z, pt.z)
            found = True
    if not found:
        return None
    return {
        "min": [round(min_v.x, 3), round(min_v.y, 3), round(min_v.z, 3)],
        "max": [round(max_v.x, 3), round(max_v.y, 3), round(max_v.z, 3)],
        "dims": [round(max_v.x - min_v.x, 3), round(max_v.y - min_v.y, 3), round(max_v.z - min_v.z, 3)],
    }


def analyze_world_mesh_issues(obj):
    """Analyze true world-space mesh geometry (unaffected by unapplied scale)."""
    mat = obj_world_mat(obj)
    bm = bmesh.new()
    bm.from_mesh(obj.data)
    bm.transform(mat)
    bm.verts.ensure_lookup_table()
    bm.edges.ensure_lookup_table()
    bm.faces.ensure_lookup_table()

    zero_area = sum(1 for f in bm.faces if f.calc_area() <= 1e-6)
    face_sigs = {}
    dup_faces = 0
    for f in bm.faces:
        sig = tuple(sorted(
            (round(v.co.x * 500), round(v.co.y * 500), round(v.co.z * 500))
            for v in f.verts
        ))
        if sig in face_sigs:
            dup_faces += 1
        else:
            face_sigs[sig] = 1
    loose_v = sum(1 for v in bm.verts if len(v.link_edges) == 0)
    loose_e = sum(1 for e in bm.edges if len(e.link_faces) == 0)
    multi_e = sum(1 for e in bm.edges if len(e.link_faces) > 2)
    tris = sum(max(0, len(f.verts) - 2) for f in bm.faces)
    bm.free()
    return {
        "tris": tris,
        "world_zero_area": zero_area,
        "world_dup_faces": dup_faces,
        "loose_v": loose_v,
        "loose_e": loose_e,
        "multi_e": multi_e,
    }


def get_stage_collections(scn):
    """Return ordered list of (label, col) for Main, BS1, BS2, BS3, and optional HKT."""
    found = {}

    def walk(col):
        ct = str(getattr(col.seut, "col_type", "none")).lower() if hasattr(col, "seut") else "none"
        idx = getattr(col.seut, "type_index", 0) if hasattr(col, "seut") else 0
        if ct == "main":
            found["Main"] = col
        elif ct == "bs":
            found[f"BS{idx}"] = col
        elif ct == "hkt":
            found["COL"] = col
        for c in col.children:
            walk(c)

    walk(scn.collection)
    ordered = []
    for key in ("Main", "BS1", "BS2", "BS3", "COL"):
        if key in found:
            ordered.append((key, found[key]))
    return ordered


def unhide_layer_collection(lc):
    lc.exclude = False
    lc.hide_viewport = False
    for child in lc.children:
        unhide_layer_collection(child)


def unhide_collection_tree(col):
    col.hide_viewport = False
    col.hide_render = False
    for child in col.children:
        unhide_collection_tree(child)


def render_stage_panel(scn, stage_label, meshes, block_size, panel_size, samples, tmp_dir, back_view=False):
    unhide_collection_tree(scn.collection)
    for vl in scn.view_layers:
        vl.use = True
    scn.use_nodes = False
    scn.render.use_compositing = False
    scn.render.use_sequencer = False
    scn.render.use_border = False

    for o in scn.objects:
        o.hide_render = True
    for o in meshes:
        o.hide_viewport = False
        o.hide_render = False

    ov = bpy.data.collections.new("__StageOverlay")
    scn.collection.children.link(ov)

    min_v = Vector((float("inf"), float("inf"), float("inf")))
    max_v = Vector((float("-inf"), float("-inf"), float("-inf")))
    has_geo = False
    for o in meshes:
        if o.type == "MESH" and o.data and len(o.data.vertices) > 0:
            mat = obj_world_mat(o)
            for corner in o.bound_box:
                pt = mat @ Vector(corner)
                min_v.x = min(min_v.x, pt.x)
                min_v.y = min(min_v.y, pt.y)
                min_v.z = min(min_v.z, pt.z)
                max_v.x = max(max_v.x, pt.x)
                max_v.y = max(max_v.y, pt.y)
                max_v.z = max(max_v.z, pt.z)
                has_geo = True

    if not has_geo:
        min_v = Vector((-block_size * 0.5,) * 3)
        max_v = Vector((block_size * 0.5,) * 3)

    dims = max_v - min_v
    center = (min_v + max_v) * 0.5
    max_dim = max(dims.x, dims.y, dims.z, block_size)
    radius = max(dims.length * 0.5, block_size * 0.6)

    rb.build_grid_and_axes_overlay(ov, block_size, max_dim)
    rb.setup_studio_lighting(ov, center, radius)
    rb.setup_render_engine(scn, panel_size, samples)

    cam_data = bpy.data.cameras.new("__StageCam")
    cam_data.type = "PERSP"
    cam_data.lens = 42.0
    cam_data.clip_start = 0.01
    cam_data.clip_end = 500.0
    cam_obj = bpy.data.objects.new("__StageCam", cam_data)
    ov.objects.link(cam_obj)
    scn.camera = cam_obj

    iso_dir = Vector((-1.0, 1.15, -0.85)).normalized() if back_view else Vector((1.0, -1.15, 0.85)).normalized()
    cam_pos = center + iso_dir * (radius * 2.65)
    cam_obj.location = cam_pos
    cam_obj.rotation_euler = (center - cam_pos).to_track_quat("-Z", "Y").to_euler()

    panel_path = os.path.join(tmp_dir, f"__tmp_{scn.name}_{stage_label}.png")
    scn.render.filepath = panel_path
    bpy.ops.render.render(write_still=True, scene=scn.name)
    if not os.path.exists(panel_path):
        raise RuntimeError(f"Render did not produce {panel_path}")
    arr = rb.load_png_topdown(panel_path)
    if os.path.exists(panel_path):
        os.remove(panel_path)

    for o in list(ov.objects):
        bpy.data.objects.remove(o, do_unlink=True)
    bpy.data.collections.remove(ov)
    return arr


def main():
    args = parse_args()
    out_png = args.output
    out_json = args.json
    panel_size = args.size
    samples = args.samples

    rb.sanitize_missing_textures()
    tmp_dir = os.path.dirname(os.path.abspath(out_png))
    os.makedirs(tmp_dir, exist_ok=True)

    if args.scene:
        if args.scene not in bpy.data.scenes:
            raise RuntimeError(f"Scene '{args.scene}' not found in {bpy.data.filepath}")
        target_scenes = [bpy.data.scenes[args.scene]]
    else:
        target_scenes = list(bpy.data.scenes)

    rows = []
    geom_report = {"filepath": bpy.data.filepath, "scenes": []}

    for scn in target_scenes:
        bpy.context.window.scene = scn
        unhide_layer_collection(bpy.context.view_layer.layer_collection)
        bpy.context.view_layer.update()

        grid_scale = str(getattr(scn.seut, "gridScale", "large")).lower() if hasattr(scn, "seut") else "large"
        block_size = rb.GRID_BLOCK_SIZES.get(grid_scale, 2.5)
        stages = get_stage_collections(scn)

        scn_info = {
            "name": scn.name,
            "grid_scale": grid_scale,
            "stages": {},
            "main_empties": [],
        }

        stage_panels = {}
        main_meshes_ref = []
        for label, col in stages:
            col.hide_viewport = False
            col.hide_render = False
            meshes = [o for o in col.objects if o.type == "MESH"]
            empties = [o for o in col.objects if o.type == "EMPTY"]
            if label == "Main":
                main_meshes_ref = meshes
            if label == "COL" and len(meshes) == 0:
                continue
            bbox = world_bbox_of_meshes(meshes)

            tot_tris = 0
            w_zero = 0
            w_dup = 0
            loose_v = 0
            loose_e = 0
            multi_e = 0
            non_const_mats = set()
            for m in meshes:
                st = analyze_world_mesh_issues(m)
                tot_tris += st["tris"]
                w_zero += st["world_zero_area"]
                w_dup += st["world_dup_faces"]
                loose_v += st["loose_v"]
                loose_e += st["loose_e"]
                multi_e += st["multi_e"]
                if label not in ("Main", "COL"):
                    for slot in m.material_slots:
                        if slot.material and slot.material.name != "Construction":
                            non_const_mats.add(slot.material.name)

            scn_info["stages"][label] = {
                "mesh_count": len(meshes),
                "empty_count": len(empties),
                "tris": tot_tris,
                "bbox": bbox,
                "world_zero_area": w_zero,
                "world_dup_faces": w_dup,
                "loose_v": loose_v,
                "loose_e": loose_e,
                "multi_e": multi_e,
                "non_const_mats": sorted(non_const_mats),
            }

            if label == "Main":
                for e in empties:
                    wm = obj_world_mat(e)
                    pos = wm.translation
                    scn_info["main_empties"].append({
                        "name": e.name,
                        "world_pos": [round(pos.x, 3), round(pos.y, 3), round(pos.z, 3)],
                        "scale": [round(e.scale.x, 3), round(e.scale.y, 3), round(e.scale.z, 3)],
                    })

            if label == "COL":
                col_mat = rb.make_collision_material()
                for m in meshes:
                    m.data.materials.clear()
                    m.data.materials.append(col_mat)
                arr = render_stage_panel(
                    scn, label, main_meshes_ref + meshes, block_size, panel_size, samples, tmp_dir, args.back
                )
                hdr_text = f"COL: {len(meshes)} OBJS"
            else:
                arr = render_stage_panel(
                    scn, label, meshes, block_size, panel_size, samples, tmp_dir, args.back
                )
                hdr_text = f"{label}: {tot_tris}T"
                if bbox:
                    d = bbox["dims"]
                    hdr_text += f" ({d[0]:.1f}X{d[1]:.1f}X{d[2]:.1f})"
            stage_panels[label] = (hdr_text, arr)

        geom_report["scenes"].append(scn_info)
        rows.append((scn.name, stage_panels))

    has_bs3 = any("BS3" in sp for _, sp in rows)
    has_col = any("COL" in sp for _, sp in rows)
    cols_order = ["Main", "BS1", "BS2"] + (["BS3"] if has_bs3 else []) + (["COL"] if has_col else [])
    max_cols = len(cols_order)

    hdr_h = 24
    row_lbl_h = 22
    cell_h = panel_size + hdr_h
    row_h = cell_h + row_lbl_h + 4
    sheet_w = panel_size * max_cols + 4 * (max_cols - 1)
    sheet_h = row_h * len(rows)

    sheet = np.zeros((sheet_h, sheet_w, 4), dtype=np.float32)
    sheet[:, :, :] = (0.06, 0.07, 0.09, 1.0)

    for r_idx, (scn_name, sp) in enumerate(rows):
        ry = r_idx * row_h
        sheet[ry : ry + row_lbl_h, :, :] = (0.18, 0.22, 0.28, 1.0)
        rb.draw_text(sheet, scn_name, 8, ry + 4, scale=2, color=(1.0, 0.92, 0.55, 1.0))
        cy = ry + row_lbl_h
        for c_idx, cname in enumerate(cols_order):
            cx = c_idx * (panel_size + 4)
            sheet[cy : cy + hdr_h, cx : cx + panel_size, :] = (0.12, 0.14, 0.18, 1.0)
            if cname in sp:
                lbl, arr = sp[cname]
                rb.draw_text(sheet, lbl, cx + 6, cy + 5, scale=1, color=(0.92, 0.95, 1.0, 1.0))
                sheet[cy + hdr_h : cy + cell_h, cx : cx + panel_size, :] = arr
            else:
                rb.draw_text(sheet, f"{cname}: NONE", cx + 6, cy + 5, scale=1, color=(0.55, 0.55, 0.60, 1.0))

    rb.save_png_topdown(sheet, out_png)
    print("WROTE_STAGES_PNG:", out_png)
    if out_json:
        with open(out_json, "w", encoding="utf-8") as f:
            json.dump(geom_report, f, indent=2)
        print("WROTE_WORLD_JSON:", out_json)


if __name__ == "__main__":
    main()
