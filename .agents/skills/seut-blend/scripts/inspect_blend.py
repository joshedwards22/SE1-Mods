import argparse
import json
import os
import sys
import bmesh
import bpy
from mathutils import Vector

RAD_TO_DEG = 57.2957795
DEFAULT_ISLAND_TOL = 0.001


class CaptureOp:
    """Minimal operator stub to capture SEUT validation reports."""

    def __init__(self):
        self.reports_ = []

    def report(self, report_types, message):
        self.reports_.append({"types": list(report_types), "message": message})


def safe_val(val):
    """Convert Blender/RNA values into JSON-serializable primitives."""
    if isinstance(val, (int, float, str, bool)) or val is None:
        return val
    if hasattr(val, "to_list"):
        return val.to_list()
    if hasattr(val, "__len__") and not isinstance(val, (str, bytes)):
        try:
            return [safe_val(x) for x in val]
        except Exception:
            return str(val)
    return str(val)


def dump_id_props(id_obj):
    """Extract custom ID properties excluding Blender RNA metadata."""
    return {k: safe_val(id_obj[k]) for k in id_obj.keys() if k != "_RNA_UI"}


def dump_seut_props(obj):
    """Extract SEUT property groups attached to a Blender datablock."""
    seut_data = {}
    for attr in dir(obj):
        if not attr.startswith("seut"):
            continue
        try:
            val = getattr(obj, attr)
            if hasattr(val, "bl_rna"):
                sub = {}
                for prop in val.bl_rna.properties:
                    pid = prop.identifier
                    if pid == "rna_type":
                        continue
                    try:
                        sub[pid] = safe_val(getattr(val, pid))
                    except Exception as exc:
                        sub[pid] = f"<err: {exc}>"
                seut_data[attr] = sub
            else:
                seut_data[attr] = safe_val(val)
        except Exception as exc:
            seut_data[attr] = f"<err: {exc}>"
    return seut_data


def dump_collection_tree(col):
    """Recursively serialize a Blender collection hierarchy."""
    return {
        "name": col.name,
        "id_props": dump_id_props(col),
        "seut": dump_seut_props(col),
        "objects": [o.name for o in col.objects],
        "children": [dump_collection_tree(c) for c in col.children],
    }


def world_bbox(obj):
    """Compute world-space axis-aligned bounding box for a mesh object."""
    pts = [obj.matrix_world @ Vector(corner) for corner in obj.bound_box]
    xs = [p.x for p in pts]
    ys = [p.y for p in pts]
    zs = [p.z for p in pts]
    return {
        "x": [round(min(xs), 4), round(max(xs), 4)],
        "y": [round(min(ys), 4), round(max(ys), 4)],
        "z": [round(min(zs), 4), round(max(zs), 4)],
    }


def compute_spatial_islands(mesh, tol=DEFAULT_ISLAND_TOL):
    """Cluster faces into spatial islands on a copy bmesh without altering the live mesh."""
    bm = bmesh.new()
    bm.from_mesh(mesh)
    bmesh.ops.remove_doubles(bm, verts=bm.verts, dist=tol)
    bm.verts.ensure_lookup_table()
    bm.faces.ensure_lookup_table()

    visited = set()
    islands = []
    for vert in bm.verts:
        if vert.index in visited:
            continue
        stack = [vert]
        visited.add(vert.index)
        comp_verts = []
        comp_faces = set()
        while stack:
            cur = stack.pop()
            comp_verts.append(cur)
            for edge in cur.link_edges:
                other = edge.other_vert(cur)
                if other.index not in visited:
                    visited.add(other.index)
                    stack.append(other)
            for face in cur.link_faces:
                comp_faces.add(face)
        if not comp_faces:
            continue
        xs = [cv.co.x for cv in comp_verts]
        ys = [cv.co.y for cv in comp_verts]
        zs = [cv.co.z for cv in comp_verts]
        mats = {}
        for face in comp_faces:
            mname = (
                mesh.materials[face.material_index].name
                if face.material_index < len(mesh.materials) and mesh.materials[face.material_index]
                else "None"
            )
            mats[mname] = mats.get(mname, 0) + 1
        islands.append({
            "verts": len(comp_verts),
            "faces": len(comp_faces),
            "min": [round(min(xs), 4), round(min(ys), 4), round(min(zs), 4)],
            "max": [round(max(xs), 4), round(max(ys), 4), round(max(zs), 4)],
            "size": [
                round(max(xs) - min(xs), 4),
                round(max(ys) - min(ys), 4),
                round(max(zs) - min(zs), 4),
            ],
            "center": [
                round((min(xs) + max(xs)) * 0.5, 4),
                round((min(ys) + max(ys)) * 0.5, 4),
                round((min(zs) + max(zs)) * 0.5, 4),
            ],
            "materials": mats,
        })
    bm.free()
    islands.sort(key=lambda item: (-item["center"][2], -item["faces"]))
    return islands


def dump_object(obj, include_islands=False, island_meshes=None, island_tol=DEFAULT_ISLAND_TOL):
    """Serialize transform, hierarchy, mesh, and SEUT metadata for an object."""
    info = {
        "name": obj.name,
        "type": obj.type,
        "parent": obj.parent.name if obj.parent else None,
        "collections": [c.name for c in obj.users_collection],
        "location": [round(x, 6) for x in obj.location],
        "rotation_euler_deg": [round(x * RAD_TO_DEG, 3) for x in obj.rotation_euler],
        "scale": [round(x, 6) for x in obj.scale],
        "dimensions": [round(x, 6) for x in obj.dimensions],
        "hide_viewport": obj.hide_viewport,
        "hide_render": obj.hide_render,
        "id_props": dump_id_props(obj),
        "seut": dump_seut_props(obj),
        "modifiers": [{"name": m.name, "type": m.type} for m in obj.modifiers],
    }
    if obj.type == "MESH" and obj.data:
        mesh = obj.data
        zero_area = sum(1 for p in mesh.polygons if p.area <= 1e-8)
        info["world_bbox"] = world_bbox(obj)
        mesh_info = {
            "vertices": len(mesh.vertices),
            "polygons": len(mesh.polygons),
            "zero_area_polys": zero_area,
            "uv_layers": [uv.name for uv in mesh.uv_layers],
            "materials": [s.material.name if s.material else None for s in obj.material_slots],
            "has_custom_normals": mesh.has_custom_normals,
        }
        is_linked_preview = "(L)" in obj.name or (hasattr(obj, "seut") and getattr(obj.seut, "linked", False))
        if include_islands and not is_linked_preview:
            if not island_meshes or obj.name in island_meshes:
                mesh_info["spatial_islands"] = compute_spatial_islands(mesh, tol=island_tol)
        info["mesh"] = mesh_info
    return info


def run_seut_validation(scn):
    """Run SEUT's built-in export/collection/UV checks for a scene if available."""
    seut_mod = next((m.split(".")[0] for m in sys.modules if "space-engineers-utilities" in m), None)
    if not seut_mod:
        return {"available": False}

    seut_errors = sys.modules.get(f"{seut_mod}.seut_errors")
    seut_cols = sys.modules.get(f"{seut_mod}.seut_collections")
    seut_patch = sys.modules.get(f"{seut_mod}.utils.seut_patch_blend")
    if not seut_errors or not seut_cols:
        return {"available": False}

    op = CaptureOp()
    bpy.context.window.scene = scn
    bpy.context.view_layer.update()

    orig_popup = getattr(seut_errors, "show_popup_report", None)
    if orig_popup is not None:
        seut_errors.show_popup_report = lambda context, title, text: op.reports_.append(
            {"types": ["POPUP"], "title": str(title), "message": str(text)}
        )

    try:
        cols = seut_cols.get_collections(scn)
        main_cols = cols.get("main") or []
        main_check = None
        unparented = []
        uv_checks = {}
        if main_cols:
            main_col = main_cols[0]
            main_check = str(seut_errors.check_collection(op, bpy.context, scn, main_col, False))
            unparented = [
                o.name for o in main_col.objects
                if o.parent is None and o.type not in ("LIGHT", "CAMERA")
            ]
            for obj in main_col.objects:
                if obj.type == "MESH" and not obj.seut.linked:
                    uv_checks[obj.name] = str(seut_errors.check_uvms(op, bpy.context, obj))

        result = {
            "available": True,
            "needs_seut_patch": bool(seut_patch.check_patch_needed()) if seut_patch else None,
            "check_export": str(seut_errors.check_export(op, bpy.context)),
            "check_main_collection": main_check,
            "main_unparented": unparented,
            "check_uvms": uv_checks,
            "reports": op.reports_,
        }
    finally:
        if orig_popup is not None:
            seut_errors.show_popup_report = orig_popup

    return result


def parse_args():
    """Parse CLI args after '--', preserving legacy positional output path."""
    argv = sys.argv[sys.argv.index("--") + 1 :] if "--" in sys.argv else []
    parser = argparse.ArgumentParser(description="Inspect a SEUT .blend file and emit JSON report.")
    parser.add_argument(
        "output",
        nargs="?",
        default=os.path.join(os.environ.get("TEMP", "."), "seut_blend_report.json"),
        help="Output JSON report path.",
    )
    parser.add_argument(
        "--islands",
        action="store_true",
        help="Include spatial-merged mesh island summaries (handles edge-split meshes cleanly).",
    )
    parser.add_argument(
        "--island-mesh",
        action="append",
        default=[],
        help="Restrict --islands output to specific mesh object name(s) (repeatable).",
    )
    parser.add_argument(
        "--island-tol",
        type=float,
        default=DEFAULT_ISLAND_TOL,
        help="Vertex merge distance in meters on copy bmesh for --islands (default 0.001).",
    )
    parser.add_argument(
        "--object",
        action="append",
        default=[],
        help="Only dump these object name(s) in 'objects' (repeatable). Keeps the JSON small.",
    )
    parser.add_argument(
        "--print-islands",
        action="store_true",
        help="Also print a compact 'ISLAND <obj> faces min max mats' line per island to stdout (implies --islands).",
    )
    parser.add_argument(
        "--min-faces",
        type=int,
        default=0,
        help="Skip islands with fewer faces in --print-islands output.",
    )
    parser.add_argument(
        "--profile",
        action="append",
        default=[],
        help="Mesh object name to height-profile (repeatable): prints 'PROFILE <obj> <axis> a..b up_min up_max' "
             "per slice, mesh-local coords. Use it to fit covers/roofs over existing geometry.",
    )
    parser.add_argument("--profile-axis", default="Y", choices=["X", "Y", "Z"], help="Slice along this axis (default Y).")
    parser.add_argument("--profile-up", default="Z", choices=["X", "Y", "Z"], help="Report min/max of this axis (default Z).")
    parser.add_argument("--profile-slice", type=float, default=0.05, help="Slice width in meters (default 0.05).")
    parser.add_argument(
        "--profile-limit",
        action="append",
        default=[],
        help="Only count verts with |axis| <= value, e.g. 'X=0.46' (repeatable).",
    )
    return parser.parse_args(argv)


AXIS_INDEX = {"X": 0, "Y": 1, "Z": 2}


def print_profile(obj, args):
    """Print min/max of --profile-up per slice along --profile-axis for one mesh object (mesh-local)."""
    if obj.type != "MESH":
        print("PROFILE", obj.name, "not a mesh")
        return
    a, u = AXIS_INDEX[args.profile_axis], AXIS_INDEX[args.profile_up]
    limits = []
    for spec in args.profile_limit:
        k, v = spec.split("=")
        limits.append((AXIS_INDEX[k.strip().upper()], float(v)))
    bands = {}
    for v in obj.data.vertices:
        c = v.co
        if any(abs(c[i]) > lim for i, lim in limits):
            continue
        s = int((c[a] // args.profile_slice))
        lo, hi = bands.get(s, (1e9, -1e9))
        bands[s] = (min(lo, c[u]), max(hi, c[u]))
    for s in sorted(bands):
        lo, hi = bands[s]
        print(f"PROFILE {obj.name} {args.profile_axis} {s * args.profile_slice:+.3f}..{(s + 1) * args.profile_slice:+.3f} "
              f"{args.profile_up}min {lo:+.4f} {args.profile_up}max {hi:+.4f}")


def print_islands(info, min_faces):
    """Compact one-line-per-island stdout summary (numbers are plain floats, no JSON digging)."""
    for isl in info.get("mesh", {}).get("spatial_islands", []):
        if isl["faces"] < min_faces:
            continue
        mats = ",".join(f"{k}:{v}" for k, v in isl["materials"].items())
        mn = " ".join(f"{c:+.3f}" for c in isl["min"])
        mx = " ".join(f"{c:+.3f}" for c in isl["max"])
        print(f"ISLAND {info['name']} f={isl['faces']} min {mn} max {mx} {mats}")


def main():
    args = parse_args()
    island_meshes = set(args.island_mesh) if args.island_mesh else None
    include_islands = bool(args.islands or island_meshes or args.print_islands)
    wanted = set(args.object)
    missing = wanted - {o.name for o in bpy.data.objects}
    if missing:
        print("WARNING: --object not found:", sorted(missing))
    objects = [o for o in bpy.data.objects if not wanted or o.name in wanted]
    for name in args.profile:
        obj = bpy.data.objects.get(name)
        if obj is None:
            print("PROFILE", name, "not found")
        else:
            print_profile(obj, args)

    report = {
        "blender_version": bpy.app.version_string,
        "filepath": bpy.data.filepath,
        "libraries": [
            {
                "filepath": lib.filepath,
                "abs_path": bpy.path.abspath(lib.filepath),
                "exists": os.path.exists(bpy.path.abspath(lib.filepath)),
            }
            for lib in bpy.data.libraries
        ],
        "scenes": [],
        "objects": [
            dump_object(
                o,
                include_islands=include_islands,
                island_meshes=island_meshes,
                island_tol=args.island_tol,
            )
            for o in objects
        ],
        "materials": [
            {
                "name": m.name,
                "users": m.users,
                "images": [
                    {
                        "node": n.name,
                        "image": n.image.name if n.image else None,
                        "filepath": n.image.filepath if n.image else None,
                    }
                    for n in (m.node_tree.nodes if m.use_nodes and m.node_tree else [])
                    if n.type == "TEX_IMAGE"
                ],
            }
            for m in bpy.data.materials
        ],
        "images": [
            {
                "name": img.name,
                "filepath": img.filepath,
                "packed": img.packed_file is not None,
                "size": list(img.size),
            }
            for img in bpy.data.images
        ],
    }

    for scn in bpy.data.scenes:
        bpy.context.window.scene = scn
        bpy.context.view_layer.update()
        report["scenes"].append({
            "name": scn.name,
            "view_layers": [vl.name for vl in scn.view_layers],
            "seut": dump_seut_props(scn),
            "collection_tree": dump_collection_tree(scn.collection),
            "seut_validation": run_seut_validation(scn),
        })

    if args.print_islands:
        for info in report["objects"]:
            print_islands(info, args.min_faces)

    out_path = args.output
    os.makedirs(os.path.dirname(os.path.abspath(out_path)), exist_ok=True)
    with open(out_path, "w", encoding="utf-8") as handle:
        json.dump(report, handle, indent=2)
    print("WROTE_BLEND_REPORT:", out_path)


if __name__ == "__main__":
    main()
