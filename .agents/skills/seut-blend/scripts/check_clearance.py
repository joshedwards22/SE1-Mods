"""Sweep a rotating subpart through its rotation range and report where it hits the static part.

Usage:
  blender -b <file.blend> -P check_clearance.py -- --static-scene MissileTurretBase1 \
      --pivot-empty subpart_MissileTurretBarrels --min -58 --max 90 [--axis X] [--step 2] [--json out.json]

How it works:
  * Moving geometry = every real (non-'(L)') mesh of the scene linked to --pivot-empty (or --moving-scene),
    split into spatial-merged islands; each island's AABB (shrunk by --shrink) is the collision proxy.
  * Static geometry = every real mesh of --static-scene except --ignore-mesh; sampled at vertices,
    face centres and edge midpoints.
  * For each angle the static samples are rotated into the moving frame and tested against the AABBs.
AABBs are conservative (a hit is "possible contact", not proof). Hits on islands that the static part is
meant to enclose (e.g. an axle hub buried inside an armoured block) are expected; read the per-island report.
Curved islands (a tub/shroud around the pivot) give AABB false positives at their box corners: confirm with
--exact before changing geometry.
Use the .sbc <MinElevationDegrees>/<MaxElevationDegrees> (or azimuth limits with --axis Z) for --min/--max.

Extra modes (can be combined with the default AABB sweep; --exact replaces it):
  --exact          Real triangle-vs-triangle overlap per angle (mathutils BVHTree.overlap). Reports only actual
                   surface intersections ("EXACT {...}" lines). It does not see a part that is fully buried
                   inside another without the surfaces touching (that is never visible anyway).
  --radius-report  Angle-independent check for parts that rotate about the pivot axis: per slice along the axis,
                   the moving geometry's max distance from the axis vs. the static geometry's min distance.
                   margin >= 0 in a slice means nothing can ever touch there at ANY angle ("RADIUS {...}").
"""
import bpy, bmesh, sys, math, json, argparse
from mathutils import Vector, Matrix
from mathutils.bvhtree import BVHTree
from collections import defaultdict


def parse_args():
    argv = sys.argv[sys.argv.index("--") + 1:] if "--" in sys.argv else []
    p = argparse.ArgumentParser()
    p.add_argument("--static-scene", required=True)
    p.add_argument("--pivot-empty", required=True, help="subpart_* empty in the static scene that carries the moving part")
    p.add_argument("--moving-scene", default=None, help="defaults to the pivot empty's seut.linkedScene")
    p.add_argument("--axis", default="X", choices=["X", "Y", "Z"], help="rotation axis in the pivot empty's local frame")
    p.add_argument("--min", type=float, default=-90.0)
    p.add_argument("--max", type=float, default=90.0)
    p.add_argument("--step", type=float, default=2.0)
    p.add_argument("--shrink", type=float, default=0.003, help="shrink AABBs by this much (m) to ignore touching faces")
    p.add_argument("--ignore-mesh", action="append", default=[], help="static mesh object name to skip (repeatable)")
    p.add_argument("--json", default=None)
    p.add_argument("--exact", action="store_true", help="triangle overlap test instead of island AABBs")
    p.add_argument("--radius-report", action="store_true", help="per-slice max/min distance from the pivot axis")
    p.add_argument("--slice", type=float, default=0.05, help="slice width (m) along the axis for --radius-report")
    return p.parse_args(argv)


def tri_data(o, mat):
    """(verts, tris) of a mesh's loop triangles with verts transformed by mat."""
    me = o.data
    me.calc_loop_triangles()
    return [mat @ v.co for v in me.vertices], [tuple(t.vertices) for t in me.loop_triangles]


def exact_sweep(a, st, mv, P):
    """Per angle, BVH-overlap every moving mesh (rotated about the pivot) against every static mesh."""
    statics = []
    for o in real_meshes(st):
        if o.name in a.ignore_mesh:
            continue
        vs, ts = tri_data(o, basis_world(o))
        if ts:
            statics.append((o.name, BVHTree.FromPolygons(vs, ts, all_triangles=True)))
    movers = []
    for o in real_meshes(mv):
        vs, ts = tri_data(o, basis_world(o))
        if ts:
            movers.append((o.name, vs, ts))
    print(f"EXACT static={st.name} moving={mv.name} static_meshes={len(statics)} moving_meshes={len(movers)}")
    hits = defaultdict(lambda: {"degs": set(), "pairs": 0, "lo": [1e9] * 3, "hi": [-1e9] * 3})
    n = int(math.floor((a.max - a.min) / a.step)) + 1
    for i in range(n):
        deg = a.min + i * a.step
        M = P @ Matrix.Rotation(math.radians(deg), 4, a.axis)      # moving local -> static frame at this angle
        for mname, vs, ts in movers:
            tree = BVHTree.FromPolygons([M @ v for v in vs], ts, all_triangles=True)
            for sname, stree in statics:
                pairs = tree.overlap(stree)
                if not pairs:
                    continue
                h = hits[(mname, sname)]
                h["degs"].add(round(deg, 1))
                h["pairs"] += len(pairs)
                for mi, _ in pairs:                                # where on the MOVING part (its local frame)
                    for vi in ts[mi]:
                        c = vs[vi]
                        for k in range(3):
                            h["lo"][k] = min(h["lo"][k], c[k]); h["hi"][k] = max(h["hi"][k], c[k])
    report = []
    for (mname, sname), h in sorted(hits.items(), key=lambda kv: -kv[1]["pairs"]):
        degs = sorted(h["degs"])
        r = {"moving_mesh": mname, "static_mesh": sname, "tri_pairs": h["pairs"], "deg_range": [degs[0], degs[-1]],
             "deg_count": len(degs), "moving_local_min": [round(c, 3) for c in h["lo"]],
             "moving_local_max": [round(c, 3) for c in h["hi"]]}
        report.append(r)
        print("EXACT", json.dumps(r))
    if not report:
        print("EXACT none")
    return report


def radius_report(a, st, mv, P, pts):
    """Slices along the pivot axis: moving max distance from the axis vs static min distance (any angle)."""
    ax = {"X": 0, "Y": 1, "Z": 2}[a.axis]
    o1, o2 = [k for k in range(3) if k != ax]
    Pinv = P.inverted()
    mv_r = defaultdict(float)
    for o in real_meshes(mv):
        mw = basis_world(o)
        for v in o.data.vertices:
            c = mw @ v.co
            s = math.floor(c[ax] / a.slice)
            mv_r[s] = max(mv_r[s], math.hypot(c[o1], c[o2]))
    st_r = defaultdict(lambda: (1e9, None))
    for src, p in pts:
        q = Pinv @ p
        s = math.floor(q[ax] / a.slice)
        r = math.hypot(q[o1], q[o2])
        if r < st_r[s][0]:
            st_r[s] = (r, src)
    rows, worst = [], None
    for s in sorted(mv_r):
        rs, src = st_r.get(s, (1e9, None))
        row = {"slice": [round(s * a.slice, 3), round((s + 1) * a.slice, 3)], "moving_r_max": round(mv_r[s], 3),
               "static_r_min": round(rs, 3) if rs < 1e8 else None, "static_mesh": src,
               "margin": round(rs - mv_r[s], 3) if rs < 1e8 else None}
        rows.append(row)
        if row["margin"] is not None and (worst is None or row["margin"] < worst["margin"]):
            worst = row
        if row["margin"] is not None and row["margin"] < 0:
            print("RADIUS", json.dumps(row))
    print("RADIUS_WORST", json.dumps(worst))
    return rows


def basis_world(o):
    """World matrix composed from matrix_basis (matrix_world is stale in inactive scenes)."""
    if o.parent is None:
        return o.matrix_basis.copy()
    return basis_world(o.parent) @ o.matrix_parent_inverse @ o.matrix_basis


def real_meshes(scn):
    return [o for o in scn.objects if o.type == 'MESH' and "(L)" not in o.name
            and not any(c.name.startswith(("Collision", "BS", "LOD")) for c in o.users_collection)]


def islands(o, shrink):
    bm = bmesh.new()
    bm.from_mesh(o.data)
    bm.verts.ensure_lookup_table()
    par = list(range(len(bm.verts)))

    def f(a):
        while par[a] != a:
            par[a] = par[par[a]]
            a = par[a]
        return a
    key = {}
    for v in bm.verts:
        k = tuple(round(c * 1e4) for c in v.co)
        if k in key:
            par[f(v.index)] = f(key[k])
        else:
            key[k] = v.index
    for e in bm.edges:
        a, b = f(e.verts[0].index), f(e.verts[1].index)
        if a != b:
            par[a] = b
    mw = basis_world(o)
    g = defaultdict(lambda: [[1e9] * 3, [-1e9] * 3])
    for v in bm.verts:
        w = mw @ v.co
        mn, mx = g[f(v.index)]
        for i in range(3):
            mn[i] = min(mn[i], w[i]); mx[i] = max(mx[i], w[i])
    bm.free()
    return [(o.name, [c + shrink for c in mn], [c - shrink for c in mx]) for mn, mx in g.values()]


def main():
    a = parse_args()
    st = bpy.data.scenes[a.static_scene]
    pivot = bpy.data.objects[a.pivot_empty]
    assert pivot.name in st.objects, f"{a.pivot_empty} is not in scene {a.static_scene}"
    mv_name = a.moving_scene or (pivot.seut.linkedScene.name if getattr(pivot, "seut", None) and pivot.seut.linkedScene else None)
    assert mv_name, "pass --moving-scene (pivot empty has no seut.linkedScene)"
    mv = bpy.data.scenes[mv_name]

    P = basis_world(pivot)                       # moving frame -> static frame at angle 0
    Pinv = P.inverted()

    pts = []
    for o in real_meshes(st):
        if o.name in a.ignore_mesh:
            continue
        mw = basis_world(o)
        me = o.data
        pts += [(o.name, mw @ v.co) for v in me.vertices]
        pts += [(o.name, mw @ p.center) for p in me.polygons]
        pts += [(o.name, mw @ ((me.vertices[e.vertices[0]].co + me.vertices[e.vertices[1]].co) / 2)) for e in me.edges]

    extra = {}
    if a.radius_report:
        extra["radius_report"] = radius_report(a, st, mv, P, pts)
    if a.exact:
        extra["exact"] = exact_sweep(a, st, mv, P)
        if a.json:
            with open(a.json, "w") as fh:
                json.dump(extra, fh, indent=1)
            print("WROTE_CLEARANCE_JSON", a.json)
        return

    boxes = []
    for o in real_meshes(mv):
        boxes += islands(o, a.shrink)
    print(f"CLEARANCE static={st.name} moving={mv.name} islands={len(boxes)} samples={len(pts)} "
          f"axis={a.axis} range={a.min}..{a.max} step={a.step}")

    hits = defaultdict(list)
    n = int(math.floor((a.max - a.min) / a.step)) + 1
    for i in range(n):
        deg = a.min + i * a.step
        R = Matrix.Rotation(math.radians(-deg), 4, a.axis) @ Pinv   # static -> rotated moving frame
        for src, p in pts:
            q = R @ p
            for name, mn, mx in boxes:
                if mn[0] < q.x < mx[0] and mn[1] < q.y < mx[1] and mn[2] < q.z < mx[2]:
                    hits[(name, tuple(round(c, 2) for c in mn), tuple(round(c, 2) for c in mx))].append(
                        (round(deg, 1), src, round(p.x, 3), round(p.y, 3), round(p.z, 3)))
                    break
    report = []
    for (name, mn, mx), v in sorted(hits.items(), key=lambda kv: -len(kv[1])):
        degs = sorted({h[0] for h in v})
        zs = [h[4] for h in v]
        r = {"moving_mesh": name, "island_min": mn, "island_max": mx, "hits": len(v),
             "deg_range": [degs[0], degs[-1]], "static_z_range": [min(zs), max(zs)],
             "static_meshes": sorted({h[1] for h in v}), "example": v[0]}
        report.append(r)
        print("CLASH", json.dumps(r))
    if not report:
        print("CLASH none")
    if a.json:
        with open(a.json, "w") as fh:
            json.dump(dict(extra, aabb=report) if extra else report, fh, indent=1)
        print("WROTE_CLEARANCE_JSON", a.json)


main()
