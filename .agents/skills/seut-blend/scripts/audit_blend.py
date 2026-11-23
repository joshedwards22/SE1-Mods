"""Read-only audit of one SEUT .blend: finds the common mistakes listed in SKILL.md. Never saves.

Usage:
  blender -b <file.blend> -P audit_blend.py -- --json "<out.json>" [--quiet]
Needs the SEUT addon for SEUT's own checks (retry with sandbox bypass if seut_validation is unavailable).
audit_mod.py runs this for every Models\\**\\*.blend and adds .mwm / .sbc cross-checks.

Output: prints 'FINDING <SEV> <code> <scene> | <object> | <message>' lines and 'AUDIT_SUMMARY {...}', and writes
JSON {file, findings:[{sev, code, scene, object, msg, fix}], scenes:[...]} (scenes feed audit_mod's cross-checks).
Severities: ERROR = breaks export/game or SEUT refuses; WARN = known pitfall / likely wrong; INFO = hygiene.
"""
import bpy, sys, os, json, argparse, math
from collections import defaultdict

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import inspect_blend  # noqa: E402  (reuses run_seut_validation)

SUBPART_PREFIX = "subpart_"
UV_ZERO_LIMIT = 0.005
COLLISION_SHAPES = {"BOX", "SPHERE", "CYLINDER", "CONVEX_HULL"}


class Audit:
    def __init__(self, path):
        self.path_ = path
        self.findings_ = []

    def add(self, sev, code, msg, scene="", obj="", fix=""):
        self.findings_.append({"sev": sev, "code": code, "scene": scene, "object": obj, "msg": msg, "fix": fix})


def seut_col_type(col):
    return getattr(getattr(col, "seut", None), "col_type", "none")


def scene_cols(scn):
    out = []

    def walk(c):
        for ch in c.children:
            out.append(ch)
            walk(ch)
    walk(scn.collection)
    return out


def abs_export_path(scn):
    p = getattr(scn.seut, "export_exportPath", "") or ""
    return os.path.normpath(bpy.path.abspath(p)) if p else ""


VANILLA_LIBS = {"materials.blend", "items.blend", "armors.blend", "character.blend", "foliage.blend", "lods.blend",
                "playeritems.blend", "voxels.blend", "seut.blend"}     # SEUT's vanilla material libraries


def check_file_level(A):
    mat_dirs = {os.path.normcase(os.path.dirname(os.path.normpath(bpy.path.abspath(l.filepath, library=l.library))))
                for l in bpy.data.libraries if os.path.basename(l.filepath).lower() == "materials.blend"}
    for lib in bpy.data.libraries:
        ap = os.path.normpath(bpy.path.abspath(lib.filepath, library=lib.library))
        base = os.path.basename(ap).lower()
        in_seut_folder = not mat_dirs or os.path.normcase(os.path.dirname(ap)) in mat_dirs
        if not os.path.exists(ap):
            A.add("ERROR", "LIB_MISSING", f"library not found: {lib.filepath}", fix="relink Materials.blend / SEUT.blend")
        elif base in VANILLA_LIBS and in_seut_folder:
            continue
        elif in_seut_folder:
            A.add("WARN", "LIB_NONVANILLA", f"uses SEUT material library {base}, which is not vanilla game content",
                  fix="remap to Materials.blend / Items.blend materials (W012-style: may not exist in game)")
        else:
            users = [i.name for c in (bpy.data.meshes, bpy.data.materials, bpy.data.objects, bpy.data.images)
                     for i in c if i.library == lib]
            if users:
                A.add("WARN", "LIB_FOREIGN", f"linked data from {base}: {users[:5]}",
                      fix="append + remap to Materials.blend (SKILL 3.2)")
            else:
                A.add("INFO", "LIB_LEFTOVER", f"unused library entry {base} (left by a cross-file append)",
                      fix="bpy.data.libraries.remove(lib) (SKILL 3.10)")
    used = set()
    for o in bpy.data.objects:
        if o.type == "MESH" and o.data:
            for s in o.material_slots:
                if s.material:
                    used.add(s.material)
    for m in sorted(used, key=lambda m: m.name):
        if m.library is None and not m.name.startswith("__"):
            A.add("WARN", "MAT_LOCAL", f"material '{m.name}' is local, not linked from Materials.blend",
                  fix="remap slots to the linked vanilla material")
        stem, _, tail = m.name.rpartition(".")
        if stem and tail.isdigit() and len(tail) == 3:
            A.add("WARN", "MAT_DUPLICATE", f"duplicate material '{m.name}'", fix=f"remap to '{stem}'")
    scene_objs = {o for s in bpy.data.scenes for o in s.objects}
    stray = [me.name for me in bpy.data.meshes if me.users and not any(o.data == me for o in scene_objs)]
    if stray:
        A.add("INFO", "MESH_UNUSED", f"{len(stray)} mesh(es) not used by any scene object: {stray[:6]}",
              fix="delete orphans (they bloat the file)")


def check_mesh(A, scn, o, role):
    me = o.data
    if any(abs(s - 1.0) > 1e-4 for s in o.scale):
        sc = tuple(round(s, 4) for s in o.scale)
        child_empties = [c.name for c in o.children if c.type == "EMPTY" and "(L)" not in c.name]
        if role == "collision" or any(s < 0 for s in o.scale) or child_empties:
            why = ("collision meshes need applied scale" if role == "collision" else
                   "negative scale flips faces" if any(s < 0 for s in o.scale) else
                   f"child empties inherit it: {child_empties[:4]}")
            A.add("WARN", "OBJ_SCALE", f"unapplied scale {sc} ({why})", scn.name, o.name,
                  "apply scale, then check child empties are back at 1.0 (SKILL 2 Detectors & Empties)")
        else:
            A.add("INFO", "OBJ_SCALE", f"unapplied scale {sc} (baked on export, harmless by itself)", scn.name, o.name)
    if len(me.uv_layers) == 0:
        A.add("ERROR", "UV_NONE", "mesh has no UV map", scn.name, o.name, "add 'UVMap' (MwmBuilder E044)")
    elif len(me.uv_layers) > 1:
        A.add("INFO", "UV_MULTI", f"{len(me.uv_layers)} UV layers {[u.name for u in me.uv_layers]}", scn.name, o.name,
              "only the first layer is exported; harmless on vanilla imports, but after a join part of the UVs "
              "can sit in the 2nd layer -> merge into one layer named UVMap")
    elif me.uv_layers[0].name != "UVMap":
        A.add("INFO", "UV_NAME", f"UV layer named '{me.uv_layers[0].name}'", scn.name, o.name, "rename to UVMap")
    if me.uv_layers and len(me.loops):
        uv = me.uv_layers.active.data
        zero = sum(1 for d in uv if abs(d.uv.x) < 1e-6 and abs(d.uv.y) < 1e-6)
        if zero / len(me.loops) > UV_ZERO_LIMIT:
            A.add("WARN", "UV_ZERO", f"{100 * zero / len(me.loops):.1f}% of UVs at (0,0)", scn.name, o.name,
                  "re-project UVs (SEUT W002/E042)")
    if role != "collision":
        if not o.material_slots or any(s.material is None for s in o.material_slots):
            A.add("ERROR", "MAT_EMPTY_SLOT", "mesh has no material or an empty slot", scn.name, o.name,
                  "assign a vanilla material")
        zero_area = sum(1 for p in me.polygons if p.area <= 1e-9)
        if zero_area:
            A.add("INFO", "MESH_ZERO_AREA", f"{zero_area} zero-area face(s)", scn.name, o.name, "dissolve degenerate")
        used = set()
        for p in me.polygons:
            used.update(p.vertices)
        loose = len(me.vertices) - len(used)
        if loose:
            A.add("INFO", "MESH_LOOSE", f"{loose} loose vertex/vertices", scn.name, o.name, "delete loose")
    if role == "main" and o.modifiers:
        A.add("INFO", "OBJ_MODIFIERS", f"modifiers {[m.type for m in o.modifiers]}", scn.name, o.name,
              "apply before export if they matter")


def check_collision(A, scn, col):
    objs = [o for o in col.all_objects if o.type == "MESH"]
    hkt = getattr(col.seut, "hkt_file", "") or ""
    if hkt:
        if not os.path.exists(bpy.path.abspath(hkt)):
            A.add("ERROR", "HKT_MISSING", f"external hkt_file not found: {hkt}", scn.name, col.name,
                  "clear col.seut.hkt_file or fix the path (E055)")
        elif objs:
            A.add("WARN", "HKT_AND_OBJECTS", "collision has both an external hkt_file and objects", scn.name, col.name,
                  "clear col.seut.hkt_file when using collision objects (E055)")
    if len(objs) > 10:
        A.add("ERROR", "COL_TOO_MANY", f"{len(objs)} collision objects (max 10)", scn.name, col.name, "merge (E022)")
    for o in objs:
        check_mesh(A, scn, o, "collision")
        if o.modifiers:
            A.add("ERROR", "COL_MODIFIERS", "collision mesh has modifiers", scn.name, o.name, "apply (E034)")
        rb = o.rigid_body
        if rb is None:
            A.add("WARN", "COL_NO_RIGIDBODY", "collision mesh has no rigid body", scn.name, o.name, "add a rigid body")
        elif rb.collision_shape == "COMPOUND":
            A.add("ERROR", "COL_COMPOUND", "rigid body shape COMPOUND", scn.name, o.name, "use BOX/CONVEX_HULL (E054)")
        elif rb.collision_shape not in COLLISION_SHAPES:
            A.add("WARN", "COL_SHAPE", f"rigid body shape {rb.collision_shape}", scn.name, o.name,
                  "use BOX/SPHERE/CYLINDER/CONVEX_HULL")
        if o.display_type != "WIRE":
            A.add("INFO", "COL_DISPLAY", f"display_type {o.display_type}", scn.name, o.name, "set WIRE")


# ------------------------------------------------------------------------------------ geometry fit checks
FIT_SAMPLES = 3000
COL_TOL = 0.03               # a visual point counts as covered within 3 cm of a collision hull
COL_COVER_MIN = 0.85
BS_NEAR = 0.08               # final BS surface point "matches" if within 8 cm of the finished model
BS_COVER = 0.15              # finished-model point is "built" if within 15 cm of the final BS
BS_COVER_MIN = 0.60
BS_OBJ_MATCH_MIN = 0.93      # each final-stage object: share of it on/inside the finished model
BS_FAR = 0.20               # unmatched BS point further than this from the model = extra / stale geometry


def basis_world(o):
    if o.parent is None:
        return o.matrix_basis.copy()
    return basis_world(o.parent) @ o.matrix_parent_inverse @ o.matrix_basis


def is_main_obj(o):
    return any(seut_col_type(c) == "main" for c in o.users_collection)


def gather_tris(scn, M, subparts, depth=0, out=None):
    """World triangles of a scene's real Main meshes (no '(L)', no detector helpers); with subparts=True the
    linked subpart scenes are added at their rest pose (assembled block)."""
    out = out if out is not None else []
    for o in scn.objects:
        if o.type == "MESH" and "(L)" not in o.name and is_main_obj(o) and o.data \
                and not o.name.lower().startswith(("detector_", "dummy_")):
            mw = M @ basis_world(o)
            me = o.data
            me.calc_loop_triangles()
            vs = [mw @ v.co for v in me.vertices]
            out += [(vs[t.vertices[0]], vs[t.vertices[1]], vs[t.vertices[2]]) for t in me.loop_triangles]
    if subparts and depth < 5:
        for e in scn.objects:
            ls = getattr(getattr(e, "seut", None), "linkedScene", None)
            if e.type == "EMPTY" and "(L)" not in e.name and e.name.lower().startswith("subpart_") and ls:
                gather_tris(ls, M @ basis_world(e), True, depth + 1, out)
    return out


def is_helper(o):
    return o.name.lower().startswith(("detector_", "dummy_"))


def tris_of_objects(objs):
    out = []
    for o in objs:
        if o.type != "MESH" or not o.data or is_helper(o):
            continue
        mw = basis_world(o)
        o.data.calc_loop_triangles()
        vs = [mw @ v.co for v in o.data.vertices]
        out += [(vs[t.vertices[0]], vs[t.vertices[1]], vs[t.vertices[2]]) for t in o.data.loop_triangles]
    return out


def sample_tris(tris, n, seed=1):
    """Area-weighted surface samples (deterministic)."""
    import random
    if not tris:
        return []
    areas = [((b - a).cross(c - a)).length * 0.5 for a, b, c in tris]
    total = sum(areas)
    if total <= 0:
        return [a for a, b, c in tris[:n]]
    rng = random.Random(seed)
    cum, acc = [], 0.0
    for ar in areas:
        acc += ar
        cum.append(acc)
    import bisect
    pts = []
    for _ in range(n):
        i = min(bisect.bisect_left(cum, rng.random() * total), len(tris) - 1)
        a, b, c = tris[i]
        r1, r2 = rng.random(), rng.random()
        if r1 + r2 > 1:
            r1, r2 = 1 - r1, 1 - r2
        pts.append(a + (b - a) * r1 + (c - a) * r2)
    return pts


def bbox_of(points):
    from mathutils import Vector
    return (Vector([min(p[i] for p in points) for i in range(3)]), Vector([max(p[i] for p in points) for i in range(3)]))


def hull_planes(o):
    """Outward planes (n, d) of the shape Havok builds from a collision object (BOX = local bbox, otherwise the
    convex hull of its vertices; SPHERE/CYLINDER approximated by the hull)."""
    import bmesh
    from mathutils import Vector
    mw = basis_world(o)
    vs = [v.co for v in o.data.vertices]
    if not vs:
        return []
    shape = o.rigid_body.collision_shape if o.rigid_body else "CONVEX_HULL"
    if shape == "BOX":
        mn = Vector([min(v[i] for v in vs) for i in range(3)])
        mx = Vector([max(v[i] for v in vs) for i in range(3)])
        vs = [Vector((x, y, z)) for x in (mn.x, mx.x) for y in (mn.y, mx.y) for z in (mn.z, mx.z)]
    bm = bmesh.new()
    for v in vs:
        bm.verts.new(mw @ v)
    try:
        bmesh.ops.convex_hull(bm, input=bm.verts)
        bm.normal_update()
        centre = sum((v.co for v in bm.verts), Vector()) / max(1, len(bm.verts))
        planes = []
        for f in bm.faces:
            n = f.normal.copy()
            if n.length < 1e-9:
                continue
            p0 = f.verts[0].co
            if n.dot(centre - p0) > 0:
                n = -n
            planes.append((n, n.dot(p0)))
        return planes
    finally:
        bm.free()


RAY_DIRS = None


def grid_half(scn):
    return 0.25 if str(getattr(scn.seut, "gridScale", "large")).lower() == "small" else 1.25


def split_block_face(over, mn, mx, half):
    """Split {'+Z': m, ...} overreach into (ends exactly on a block face within half a cell -> fine, rest)."""
    face, rest = {}, {}
    for k, v in over.items():
        i = "XYZ".index(k[1])
        edge = mx[i] if k[0] == "+" else mn[i]
        on_face = abs(edge / half - round(edge / half)) * half < 0.02
        (face if on_face and v <= half + 0.02 else rest)[k] = v
    return face, rest


def matches_model(tree, p):
    """A build-stage point 'matches' the finished model if it lies on/near its surface or is enclosed by it
    (rays along all 6 axes hit the model): interior scaffolding passes, geometry floating in empty space fails."""
    global RAY_DIRS
    from mathutils import Vector
    if tree.find_nearest(p)[3] <= BS_NEAR:
        return True
    if RAY_DIRS is None:
        RAY_DIRS = [Vector(d) for d in ((1, 0, 0), (-1, 0, 0), (0, 1, 0), (0, -1, 0), (0, 0, 1), (0, 0, -1))]
    return all(tree.ray_cast(p, d, 20.0)[0] is not None for d in RAY_DIRS)


def fit_checks(A, scn, info, cols):
    """Collision coverage/overreach and build-stage match for a main (block) scene."""
    from mathutils import Matrix
    from mathutils.bvhtree import BVHTree
    own = gather_tris(scn, Matrix.Identity(4), False)
    if not own:
        return
    full = gather_tris(scn, Matrix.Identity(4), True)
    full_pts = sample_tris(full, FIT_SAMPLES)
    own_pts = sample_tris(own, FIT_SAMPLES)
    fmn, fmx = bbox_of([p for t in full for p in t])
    info["assembled_bbox"] = [list(fmn), list(fmx)]

    # ---- collision
    hkt_cols = [c for c in cols if seut_col_type(c) == "hkt"]
    col_objs = [o for c in hkt_cols for o in c.all_objects if o.type == "MESH" and o.data and len(o.data.vertices)]
    ext_hkt = any(getattr(c.seut, "hkt_file", "") for c in hkt_cols)
    fit = {"objects": len(col_objs), "external_hkt": bool(ext_hkt)}
    if not col_objs:
        if not ext_hkt:
            A.add("WARN", "COL_MISSING", "block has no collision objects and no external .hkt", scn.name,
                  fix="add a 'Collision - Main' collection with convex boxes/hulls around the model")
    else:
        hulls = [hull_planes(o) for o in col_objs]
        inside = sum(1 for p in own_pts if any(h and all(n.dot(p) - d <= COL_TOL for n, d in h) for h in hulls))
        cov = inside / len(own_pts)
        cpts = [basis_world(o) @ v.co for o in col_objs for v in o.data.vertices]
        cmn, cmx = bbox_of(cpts)
        over = {f"{'-' if s == 0 else '+'}{'XYZ'[i]}": round((fmn[i] - cmn[i]) if s == 0 else (cmx[i] - fmx[i]), 3)
                for i in range(3) for s in (0, 1)}
        fit.update({"coverage": round(cov, 3), "collision_bbox": [list(cmn), list(cmx)], "overreach": over})
        if cov < COL_COVER_MIN:
            miss = [p for p in own_pts if not any(h and all(n.dot(p) - d <= COL_TOL for n, d in h) for h in hulls)]
            mmn, mmx = bbox_of(miss)
            A.add("WARN", "COL_COVERAGE", f"collision covers only {100 * cov:.0f}% of the visible block; uncovered "
                  f"region x {mmn.x:+.2f}..{mmx.x:+.2f} y {mmn.y:+.2f}..{mmx.y:+.2f} z {mmn.z:+.2f}..{mmx.z:+.2f}",
                  scn.name, fix="extend/add collision shapes over that region")
        big = {k: v for k, v in over.items()
               if v > max(0.25, 0.1 * (fmx["XYZ".index(k[1])] - fmn["XYZ".index(k[1])]))}
        face, wall = split_block_face(big, cmn, cmx, grid_half(scn))
        if wall:
            A.add("WARN", "COL_OVERREACH", f"collision extends past the assembled model by {wall} (m): invisible walls",
                  scn.name, fix="shrink the collision to the model (turret heads: up to the head's top at rest)")
        if face:
            A.add("INFO", "COL_TO_BLOCK_FACE", f"collision reaches the block face past the visible model by {face} (m) "
                  "(normal for conveyor ports)", scn.name)
    info["collision_fit"] = fit

    # ---- build stages
    full_tree = BVHTree.FromPolygons([v for t in full for v in t], [(3 * i, 3 * i + 1, 3 * i + 2) for i in range(len(full))],
                                     all_triangles=True)
    stages = sorted([c for c in cols if seut_col_type(c) == "bs"], key=lambda c: c.seut.type_index)
    res = []
    for i, c in enumerate(stages):
        tris = tris_of_objects(list(c.all_objects))
        if not tris:
            continue
        pts = sample_tris(tris, 2000, seed=7 + i)
        near = sum(1 for p in pts if matches_model(full_tree, p)) / len(pts)
        per_obj = {}
        for o in [x for x in c.all_objects if not is_helper(x)]:   # per object: a stale head isn't hidden by a pedestal
            ot = tris_of_objects([o])
            if ot:
                op = sample_tris(ot, 1200, seed=11)
                miss = [p for p in op if not matches_model(full_tree, p)]
                farp = [p for p in miss if full_tree.find_nearest(p)[3] > BS_FAR]
                rec = {"match": round(1 - len(miss) / len(op), 3), "far": round(len(farp) / len(op), 3)}
                if farp:
                    fmn_, fmx_ = bbox_of(farp)
                    rec["far_bbox"] = [[round(x, 2) for x in fmn_], [round(x, 2) for x in fmx_]]
                per_obj[o.name] = rec
        bmn, bmx = bbox_of([p for t in tris for p in t])
        outside = {f"{'-' if s == 0 else '+'}{'XYZ'[k]}": round((fmn[k] - bmn[k]) if s == 0 else (bmx[k] - fmx[k]), 3)
                   for k in range(3) for s in (0, 1)}
        outside = {k: v for k, v in outside.items() if v > 0.15}
        face_only, outside = split_block_face(outside, bmn, bmx, grid_half(scn))
        row = {"collection": c.name, "index": c.seut.type_index, "faces": len(tris), "near_model": round(near, 3),
               "objects": per_obj}
        if outside:
            A.add("WARN", "BS_OUTSIDE", f"build stage sticks out past the finished model by {outside} (m)", scn.name,
                  c.name, "stale geometry from an older design? rebuild this stage")
        if i == len(stages) - 1:
            bs_tree = BVHTree.FromPolygons([v for t in tris for v in t],
                                           [(3 * j, 3 * j + 1, 3 * j + 2) for j in range(len(tris))], all_triangles=True)
            cover = sum(1 for p in full_pts if bs_tree.find_nearest(p)[3] <= BS_COVER) / len(full_pts)
            row["covers_model"] = round(cover, 3)
            for oname, rec in per_obj.items():
                if rec["match"] < BS_OBJ_MATCH_MIN:
                    where = ""
                    if "far_bbox" in rec:
                        (x0, y0, z0), (x1, y1, z1) = rec["far_bbox"]
                        where = f"; off-model geometry around x {x0:+.2f}..{x1:+.2f} y {y0:+.2f}..{y1:+.2f} z {z0:+.2f}..{z1:+.2f}"
                    A.add("WARN", "BS_MISMATCH", f"final-stage mesh matches the finished model only {100 * rec['match']:.0f}% "
                          f"({100 * rec['far']:.0f}% more than {BS_FAR:.2f} m off){where}", scn.name, oname,
                          "check the stage close-ups; rebuild it from the current model if it's an old design")
            if cover < BS_COVER_MIN:
                A.add("WARN", "BS_INCOMPLETE", f"final build stage reaches only {100 * cover:.0f}% of the finished "
                      "model's surface (parts missing)", scn.name, c.name,
                      "add the missing parts to the last stage")
        res.append(row)
    info["bs_fit"] = res


def check_scene(A, scn, out_scenes):
    s = scn.seut
    info = {"name": scn.name, "sceneType": getattr(s, "sceneType", ""), "subtypeId": getattr(s, "subtypeId", ""),
            "gridScale": getattr(s, "gridScale", ""), "export_path": abs_export_path(scn),
            "export_large": bool(getattr(s, "export_largeGrid", False)),
            "export_small": bool(getattr(s, "export_smallGrid", False)),
            "empties": [], "subparts": [], "bs_count": 0, "lod_count": 0}
    if "SEUT" not in [vl.name for vl in scn.view_layers]:
        A.add("WARN", "SCENE_NO_SEUT_LAYER", "scene has no 'SEUT' view layer (not a SEUT scene?)", scn.name)
        out_scenes.append(info)
        return
    if not info["subtypeId"]:
        A.add("ERROR", "SCENE_NO_SUBTYPE", "empty subtypeId", scn.name, fix="set the scene's SubtypeId")
    if info["export_large"] and info["export_small"]:
        A.add("ERROR", "EXPORT_DUAL_GRID", "both export_largeGrid and export_smallGrid are on", scn.name,
              fix="keep only the flag matching gridScale (writes LG_/SG_ copies otherwise)")
    ep = info["export_path"]
    if not ep:
        A.add("ERROR", "EXPORT_NO_PATH", "export_exportPath is empty", scn.name)
    else:
        parts = [p.lower() for p in ep.replace("/", "\\").split("\\")]
        if info["gridScale"] in ("large", "small") and ("large" in parts or "small" in parts) \
                and info["gridScale"] not in parts:
            A.add("WARN", "EXPORT_GRID_FOLDER", f"gridScale={info['gridScale']} but export path is {ep}", scn.name)
        if not os.path.isdir(ep):
            A.add("WARN", "EXPORT_PATH_MISSING", f"export folder does not exist: {ep}", scn.name)
        mods_root = os.path.normcase(os.path.dirname(os.path.dirname(os.path.dirname(bpy.data.filepath))))
        if not os.path.normcase(ep).startswith(mods_root):
            A.add("WARN", "EXPORT_OUTSIDE_MODS", f"export path outside the Mods folder: {ep}", scn.name)
    if getattr(s, "export_sbc_type", "none") != "none":
        A.add("INFO", "EXPORT_SBC_ON", f"export_sbc_type={s.export_sbc_type} (SEUT will write .sbc)", scn.name,
              fix="usually 'none'; .sbc files are maintained by hand")

    v = inspect_blend.run_seut_validation(scn)
    if v.get("available"):
        if v.get("needs_seut_patch"):
            A.add("WARN", "SEUT_PATCH", "file needs SEUT 'Patch BLEND File'", scn.name, fix="bpy.ops.wm.patch_blend()")
        for key in ("check_export", "check_main_collection"):
            if v.get(key) not in (None, "{'CONTINUE'}"):
                A.add("ERROR", "SEUT_" + key.upper(), f"SEUT {key} -> {v.get(key)}", scn.name)
        for obj, res in v.get("check_uvms", {}).items():
            if res != "{'CONTINUE'}":
                A.add("ERROR", "SEUT_CHECK_UVMS", f"SEUT check_uvms -> {res}", scn.name, obj)
        for r in v.get("reports", []):
            sev = "ERROR" if any(t in ("ERROR", "POPUP") for t in r.get("types", [])) else "WARN"
            A.add(sev, "SEUT_REPORT", str(r.get("message") or r.get("title")), scn.name)
    else:
        A.add("INFO", "SEUT_UNAVAILABLE", "SEUT addon not loaded: SEUT checks skipped", scn.name,
              fix="rerun with sandbox bypass")

    cols = scene_cols(scn)
    bs_idx = {}
    for col in cols:
        ct = seut_col_type(col)
        if ct == "lod":
            info["lod_count"] += 1
            A.add("WARN", "LOD_PRESENT", "LOD collection present (user does not want LODs)", scn.name, col.name,
                  "delete the LOD collection")
        elif ct == "bs":
            info["bs_count"] += 1
            key = (getattr(col.seut, "ref_col", None) and col.seut.ref_col.name, col.seut.type_index)
            if key in bs_idx:
                A.add("ERROR", "BS_DUP_INDEX", f"BS type_index {col.seut.type_index} used by '{bs_idx[key]}' too",
                      scn.name, col.name, "set ref_col BEFORE type_index/name (SKILL 2 Build Stage)")
            bs_idx[key] = col.name
            if not any(o.type == "MESH" for o in col.all_objects):
                A.add("WARN", "BS_EMPTY", "build-stage collection has no mesh", scn.name, col.name)
            other = defaultdict(int)
            for o in col.all_objects:
                if o.type == "MESH":
                    check_mesh(A, scn, o, "bs")
                    for s in o.material_slots:
                        if s.material and s.material.name != "Construction":
                            other[s.material.name] += 1
            if other:
                A.add("INFO", "BS_MATERIAL", f"non-Construction materials in build stage: {dict(other)}", scn.name,
                      col.name, "fine for vanilla-style last stages with decals; new BS heads should be all "
                      "'Construction' (SKILL 2 Build Stage)")
        elif ct == "hkt":
            check_collision(A, scn, col)
        elif ct == "main":
            for o in col.all_objects:
                if "(L)" in o.name:
                    continue
                if o.type == "MESH":
                    check_mesh(A, scn, o, "main")
                elif o.type == "EMPTY":
                    info["empties"].append(o.name)
                    low = o.name.lower()
                    if any(abs(sc - 1.0) > 1e-4 for sc in o.scale):
                        sc = tuple(round(x, 4) for x in o.scale)
                        if low.startswith(SUBPART_PREFIX):
                            A.add("ERROR", "EMPTY_SCALE_SUBPART", f"subpart empty scaled {sc}", scn.name, o.name,
                                  "must be 1.0: a scaled subpart empty rescales the whole subpart in game")
                        elif not low.startswith(("detector_", "dummy_detector_")):
                            A.add("INFO", "EMPTY_SCALE", f"empty scaled {sc} (only position/orientation are used; "
                                  "common on vanilla imports)", scn.name, o.name)
                    if low.startswith("subpart_"):
                        ls = getattr(o.seut, "linkedScene", None)
                        rec = {"empty": o.name, "linked": ls.name if ls else None,
                               "linked_subtype": ls.seut.subtypeId if ls else None,
                               "file": str(o["file"]) if "file" in o else None}
                        info["subparts"].append(rec)
                        if ls is None:
                            A.add("ERROR", "SUBPART_UNLINKED", "subpart empty has no linkedScene", scn.name, o.name,
                                  "set seut.linkedScene")
                        elif rec["file"] is not None and rec["file"] != ls.seut.subtypeId:
                            A.add("WARN", "SUBPART_FILE_MISMATCH",
                                  f"'file'={rec['file']} but linked subtype is {ls.seut.subtypeId}", scn.name, o.name,
                                  "relink the subpart so SEUT rewrites 'file'")
                        if ls is not None and getattr(scn.seut, "linkSubpartInstances", False):
                            real = sorted(len(m.data.polygons) for m in ls.objects
                                          if m.type == "MESH" and "(L)" not in m.name and m.data
                                          and any(seut_col_type(c) == "main" for c in m.users_collection))
                            prev = sorted(len(c.data.polygons) for c in o.children
                                          if c.type == "MESH" and "(L)" in c.name and c.data)
                            if real != prev:
                                A.add("INFO", "PREVIEW_STALE",
                                      f"(L) previews {prev} != linked scene meshes {real}", scn.name, o.name,
                                      "run update_subparts.py (or SEUT Update Subparts)")
    if info["sceneType"] in ("mainScene", "subpart") and not any(seut_col_type(c) == "main" for c in cols):
        A.add("ERROR", "NO_MAIN_COLLECTION", "no SEUT Main collection", scn.name)
    if info["sceneType"] == "mainScene":
        try:
            fit_checks(A, scn, info, cols)
        except Exception as exc:
            A.add("INFO", "FIT_CHECK_FAILED", f"{type(exc).__name__}: {exc}", scn.name)
    else:
        info["collision_fit"] = {"objects": sum(1 for c in cols if seut_col_type(c) == "hkt"
                                                for o in c.all_objects if o.type == "MESH")}
    out_scenes.append(info)


def main():
    argv = sys.argv[sys.argv.index("--") + 1:] if "--" in sys.argv else []
    p = argparse.ArgumentParser()
    p.add_argument("--json", required=True)
    p.add_argument("--quiet", action="store_true", help="only print the summary line")
    a = p.parse_args(argv)
    A = Audit(bpy.data.filepath)
    scenes = []
    check_file_level(A)
    for scn in bpy.data.scenes:
        try:
            check_scene(A, scn, scenes)
        except Exception as exc:                      # keep auditing the other scenes
            A.add("ERROR", "AUDIT_CRASH", f"{type(exc).__name__}: {exc}", scn.name)
    counts = {k: sum(1 for f in A.findings_ if f["sev"] == k) for k in ("ERROR", "WARN", "INFO")}
    out = {"file": bpy.data.filepath, "mtime": os.path.getmtime(bpy.data.filepath), "counts": counts,
           "findings": A.findings_, "scenes": scenes}
    os.makedirs(os.path.dirname(os.path.abspath(a.json)), exist_ok=True)
    with open(a.json, "w", encoding="utf-8") as fh:
        json.dump(out, fh, indent=1)
    if not a.quiet:
        for f in A.findings_:
            print(f"FINDING {f['sev']} {f['code']} {f['scene']} | {f['object']} | {f['msg']}")
    print("AUDIT_SUMMARY", json.dumps({"file": bpy.data.filepath, **counts}))


main()
