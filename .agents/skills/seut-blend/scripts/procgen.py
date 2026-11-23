"""Procedural hard-surface helpers for building SE block/subpart meshes in headless Blender.

Import from a build script run with blender -b ... -P build.py:
    import sys; sys.path.insert(0, r"C:\\Users\\joshe\\AppData\\Roaming\\SpaceEngineers\\Mods\\.agents\\skills\\seut-blend\\scripts")
    from procgen import MeshBuilder, panel_outline
    B = MeshBuilder(["PaintedMetal_Colorable", "PaintedMetal_VeryDark", "Chrome"])
    COL, VDARK, CHROME = range(3)
    B.box((-0.4, -0.7, 0.0), (0.4, 0.7, 1.0), COL, ch=0.03, segs=3)
    B.prism(panel_outline(-0.6, 0.6, 0.1, 0.9, 0.12, 0.03), 'X', 0.40, 0.44, VDARK, ch=0.012, segs=2)
    me = B.to_mesh("MyPart")          # materials linked from bpy.data by name, UVMap, sharp edges

Rules baked in (each one cost a debugging round):
  * material_index is assigned BEFORE bmesh.ops.bevel: bevel rebuilds the original faces and copies their
    attributes, so assigning only to the faces bevel returns leaves the big faces on material 0.
  * Every primitive collects its faces as "all faces not present before" (bevel output alone misses faces).
  * Materials are appended to the mesh BEFORE bm.to_mesh (materials.clear() after would zero indices).
  * One UV layer named "UVMap", box-projected with an offset so almost no verts sit at (0,0) (SEUT W002/E042).
  * Edges sharper than sharp_deg are marked sharp; faces from add_mesh() keep their own UVs and edge flags.
  * Primitives that touch should overlap by a few mm instead of sharing a plane (z-fighting).

Also here (see SKILL.md "procgen helpers"):
  * MeshBuilder.loft(outline_a, a0, inset(outline_a, d), a1, mat)  - armour plate with angled edges
  * MeshBuilder.riveted_plate(matrix, hx, hy, plate_mat, rivet_mat) - plate + rivet ring on any face
  * arc(), ngon(), inset(), slope_frame(), FACE_FRONT / FACE_REAR     - outline / placement helpers
  * spatial_islands(), island_bbox(), island_faces_within()           - pick whole parts out of SE meshes
  * append_mesh(blend, obj_name)                                      - cross-file append + library cleanup
  * scale_mesh_keep_texel(me, k, skip_faces=...)                      - scale up without blurry textures
"""
import bpy, bmesh, math, os
from mathutils import Vector, Matrix

AXIS_ROT = {
    'X': Matrix.Rotation(math.radians(90), 4, 'Y'),
    'Y': Matrix.Rotation(math.radians(-90), 4, 'X'),
    'Z': Matrix.Identity(4),
}


def panel_outline(u0, u1, v0, v1, ct, cb):
    """CCW 2D outline of a panel with chamfered top corners (ct) and small bottom chamfers (cb)."""
    return [(u0 + cb, v0), (u1 - cb, v0), (u1, v0 + cb), (u1, v1 - ct), (u1 - ct, v1),
            (u0 + ct, v1), (u0, v1 - ct), (u0, v0 + cb)]


def bezier(a, c1, c2, b, n):
    a, c1, c2, b = map(Vector, (a, c1, c2, b))
    return [(1 - t) ** 3 * a + 3 * (1 - t) ** 2 * t * c1 + 3 * (1 - t) * t ** 2 * c2 + t ** 3 * b
            for t in (i / n for i in range(n + 1))]


class MeshBuilder:
    def __init__(self, material_names, uv_tile=1.25, sharp_deg=25.0):
        self.mats = list(material_names)
        self.bm = bmesh.new()
        self.keep = set()            # faces whose UVs / edge flags come from a source mesh
        self.uv_tile = uv_tile
        self.sharp_deg = sharp_deg

    # ---------------------------------------------------------------- internals
    def _snap(self):
        return set(self.bm.faces)

    def _new(self, before):
        return [f for f in self.bm.faces if f not in before]

    def _set(self, faces, mat):
        for f in faces:
            f.material_index = mat
        return faces

    def _bevel(self, edges, ch, segs):
        if ch > 0 and edges:
            bmesh.ops.bevel(self.bm, geom=list(edges), offset=ch, offset_type='OFFSET', segments=segs,
                            profile=0.5, affect='EDGES', clamp_overlap=True)

    # ---------------------------------------------------------------- primitives
    def box(self, mn, mx, mat, ch=0.0, segs=1, matrix=None):
        """Axis-aligned box mn..mx (optionally placed by matrix), all edges bevelled by ch."""
        before = self._snap()
        c = [(a + b) / 2 for a, b in zip(mn, mx)]
        s = [(b - a) for a, b in zip(mn, mx)]
        m = Matrix.Translation(c) @ Matrix.Diagonal((*s, 1.0))
        bmesh.ops.create_cube(self.bm, size=1.0, matrix=(matrix @ m) if matrix is not None else m)
        faces = self._set(self._new(before), mat)
        self._bevel({e for f in faces for e in f.edges}, ch, segs)
        return self._set(self._new(before), mat)

    def cyl(self, center, axis, r, length, mat, seg=24, ch=0.0, segs=1, matrix=None):
        """Capped cylinder along 'X'/'Y'/'Z' centred at center; cap rims bevelled by ch."""
        before = self._snap()
        m = Matrix.Translation(center) @ AXIS_ROT[axis]
        bmesh.ops.create_cone(self.bm, cap_ends=True, cap_tris=False, segments=seg, radius1=r, radius2=r,
                              depth=length, matrix=(matrix @ m) if matrix is not None else m)
        faces = self._set(self._new(before), mat)
        caps = [f for f in faces if len(f.verts) > 4]
        self._bevel({e for f in caps for e in f.edges}, ch, segs)
        return self._set(self._new(before), mat)

    def ring(self, center, axis, r_in, r_out, length, mat, seg=32, matrix=None):
        """Annulus / tube wall (hollow cylinder)."""
        before = self._snap()
        m = Matrix.Translation(center) @ AXIS_ROT[axis]
        if matrix is not None:
            m = matrix @ m

        def circ(r, z):
            return [self.bm.verts.new(m @ Vector((r * math.cos(2 * math.pi * k / seg),
                                                   r * math.sin(2 * math.pi * k / seg), z))) for k in range(seg)]
        h = length / 2
        o0, o1, i0, i1 = circ(r_out, -h), circ(r_out, h), circ(r_in, -h), circ(r_in, h)
        for k in range(seg):
            j = (k + 1) % seg
            self.bm.faces.new((o0[k], o0[j], o1[j], o1[k]))
            self.bm.faces.new((i0[j], i0[k], i1[k], i1[j]))
            self.bm.faces.new((o1[k], o1[j], i1[j], i1[k]))
            self.bm.faces.new((i0[k], i0[j], o0[j], o0[k]))
        bmesh.ops.recalc_face_normals(self.bm, faces=self._new(before))
        return self._set(self._new(before), mat)

    def prism(self, outline, axis, a0, a1, mat, ch=0.0, segs=1, matrix=None):
        """Extrude a 2D outline from a0 to a1 along 'X' (outline in Y,Z) or 'Y' (outline in X,Z) or 'Z' (X,Y).
        Only the two outline loops are bevelled, so curved outlines stay smooth."""
        before = self._snap()
        mk = {'X': lambda a, u, v: (a, u, v), 'Y': lambda a, u, v: (u, a, v), 'Z': lambda a, u, v: (u, v, a)}[axis]
        tr = matrix if matrix is not None else Matrix.Identity(4)
        A = [self.bm.verts.new(tr @ Vector(mk(a0, u, v))) for u, v in outline]
        B = [self.bm.verts.new(tr @ Vector(mk(a1, u, v))) for u, v in outline]
        cA = self.bm.faces.new(A)
        cB = self.bm.faces.new(list(reversed(B)))
        n = len(outline)
        for i in range(n):
            j = (i + 1) % n
            self.bm.faces.new((A[i], B[i], B[j], A[j]))
        bmesh.ops.recalc_face_normals(self.bm, faces=self._new(before))
        self._set(self._new(before), mat)
        self._bevel(set(cA.edges) | set(cB.edges), ch, segs)
        return self._set(self._new(before), mat)

    def tube(self, points, radius, mat, seg=16, up=(1, 0, 0)):
        """Swept tube along a polyline; radius may be a float or f(i) (e.g. ribbed hose)."""
        before = self._snap()
        pts = [Vector(p) for p in points]
        rf = radius if callable(radius) else (lambda i: radius)
        upv = Vector(up)
        rings = []
        for i, p in enumerate(pts):
            t = (pts[min(i + 1, len(pts) - 1)] - pts[max(i - 1, 0)]).normalized()
            u = upv - t * upv.dot(t)
            if u.length < 1e-6:
                u = Vector((0, 1, 0)) - t * t.y
            u.normalize()
            v = t.cross(u)
            r = rf(i)
            rings.append([self.bm.verts.new(p + (u * math.cos(2 * math.pi * k / seg) +
                                                 v * math.sin(2 * math.pi * k / seg)) * r) for k in range(seg)])
        for a, b in zip(rings, rings[1:]):
            for k in range(seg):
                j = (k + 1) % seg
                self.bm.faces.new((a[k], a[j], b[j], b[k]))
        self.bm.faces.new(list(reversed(rings[0])))
        self.bm.faces.new(rings[-1])
        bmesh.ops.recalc_face_normals(self.bm, faces=self._new(before))
        return self._set(self._new(before), mat)

    def beam(self, p, q, mat, w=0.05, matrix=None):
        """Square beam between two points (construction frames / trusses)."""
        tr = matrix if matrix is not None else Matrix.Identity(4)
        p, q = tr @ Vector(p), tr @ Vector(q)
        d = q - p
        if d.length < 1e-5:
            return []
        before = self._snap()
        rot = d.to_track_quat('Z', 'Y').to_matrix().to_4x4()
        bmesh.ops.create_cube(self.bm, size=1.0, matrix=Matrix.Translation((p + q) / 2) @ rot @
                              Matrix.Diagonal((w, w, d.length + w * 0.6, 1.0)))
        return self._set(self._new(before), mat)

    def add_mesh(self, me, matrix=None, face_filter=None, mat=None, keep_uv=True):
        """Append another mesh's faces (optionally filtered by face_filter(BMFace) on the SOURCE bmesh),
        transformed by matrix. mat=None keeps source material names mapped onto this builder's list."""
        src_names = [m.name if m else "" for m in me.materials]
        tmp = bmesh.new()
        tmp.from_mesh(me)
        if face_filter is not None:
            bmesh.ops.delete(tmp, geom=[f for f in tmp.faces if not face_filter(f)], context='FACES')
            bmesh.ops.delete(tmp, geom=[v for v in tmp.verts if not v.link_faces], context='VERTS')
        if matrix is not None:
            bmesh.ops.transform(tmp, matrix=matrix, verts=tmp.verts)
            if matrix.determinant() < 0:
                bmesh.ops.reverse_faces(tmp, faces=tmp.faces)
        m = bpy.data.meshes.new("_procgen_tmp")
        for mm in me.materials:
            m.materials.append(mm)
        tmp.to_mesh(m)
        tmp.free()
        before = self._snap()
        self.bm.from_mesh(m)
        bpy.data.meshes.remove(m)
        faces = self._new(before)
        for f in faces:
            if mat is not None:
                f.material_index = mat
            else:
                n = src_names[f.material_index] if f.material_index < len(src_names) else ""
                if n not in self.mats:
                    self.mats.append(n)
                f.material_index = self.mats.index(n)
        if keep_uv:
            self.keep.update(faces)
        return faces

    def loft(self, outline_a, a0, outline_b, a1, mat, axis='X', matrix=None):
        """Solid between two 2D outlines with the same vertex count, outline_a at a0 and outline_b at a1
        along 'X' (outline in Y,Z) / 'Y' (X,Z) / 'Z' (X,Y). With outline_b = inset(outline_a, d) this gives an
        armour plate with angled edges on the a1 side (much less boxy than a bevelled prism)."""
        assert len(outline_a) == len(outline_b), "loft outlines need the same vertex count"
        before = self._snap()
        mk = {'X': lambda a, u, v: (a, u, v), 'Y': lambda a, u, v: (u, a, v), 'Z': lambda a, u, v: (u, v, a)}[axis]
        tr = matrix if matrix is not None else Matrix.Identity(4)
        A = [self.bm.verts.new(tr @ Vector(mk(a0, u, v))) for u, v in outline_a]
        C = [self.bm.verts.new(tr @ Vector(mk(a1, u, v))) for u, v in outline_b]
        self.bm.faces.new(A)
        self.bm.faces.new(list(reversed(C)))
        n = len(A)
        for i in range(n):
            j = (i + 1) % n
            self.bm.faces.new((A[i], C[i], C[j], A[j]))
        bmesh.ops.recalc_face_normals(self.bm, faces=self._new(before))
        return self._set(self._new(before), mat)

    def riveted_plate(self, matrix, hx, hy, plate_mat, rivet_mat, pitch=0.045, t=0.012, margin=0.022,
                      rivet_r=0.0065):
        """Armour plate (half-size hx, hy, thickness t) with a ring of rivets, placed by matrix whose local +Z is
        the surface normal (Matrix.Translation(p) for a flat top; use slope_frame() or a rotation for slopes and
        vertical faces). The plate sinks 4 mm into the surface so it never floats on a bevel."""
        faces = self.box((-hx, -hy, -0.004), (hx, hy, t), plate_mat, ch=0.004, segs=1, matrix=matrix)
        nx = max(2, int(round(2 * (hx - margin) / pitch)))
        ny = max(2, int(round(2 * (hy - margin) / pitch)))
        pts = [(-hx + margin + 2 * (hx - margin) * i / nx, sy * (hy - margin)) for i in range(nx + 1) for sy in (-1, 1)]
        pts += [(sx * (hx - margin), -hy + margin + 2 * (hy - margin) * j / ny) for j in range(1, ny) for sx in (-1, 1)]
        for px, py in pts:
            faces += self.cyl((px, py, t + 0.003), 'Z', rivet_r, 0.008, rivet_mat, seg=8, matrix=matrix)
        return faces

    # ---------------------------------------------------------------- output
    def to_mesh(self, name):
        bm = self.bm
        bm.normal_update()
        for f in bm.faces:
            f.smooth = True
        lim = math.radians(self.sharp_deg)
        for e in bm.edges:
            lf = e.link_faces
            if len(lf) != 2:
                e.smooth = False
            elif not (lf[0] in self.keep and lf[1] in self.keep) and lf[0].normal.angle(lf[1].normal, 0) > lim:
                e.smooth = False
        uv = bm.loops.layers.uv.active or bm.loops.layers.uv.new("UVMap")
        for f in bm.faces:
            if f in self.keep:
                continue
            ax = max(range(3), key=lambda i: abs(f.normal[i]))
            for l in f.loops:
                c = l.vert.co
                u, v = ((c.y, c.z), (c.x, c.z), (c.x, c.y))[ax]
                l[uv].uv = (u / self.uv_tile + 0.37, v / self.uv_tile + 0.41)
        me = bpy.data.meshes.new(name)
        for n in self.mats:                                   # BEFORE to_mesh
            mat = bpy.data.materials.get(n)
            assert mat is not None, f"material {n!r} missing - link it from Materials.blend first"
            me.materials.append(mat)
        bm.to_mesh(me)
        bm.free()
        assert len(me.uv_layers) == 1, [u.name for u in me.uv_layers]
        me.uv_layers[0].name = "UVMap"
        return me

    @staticmethod
    def material_counts(me):
        out = {}
        for p in me.polygons:
            n = me.materials[p.material_index].name
            out[n] = out.get(n, 0) + 1
        return out


# ==================================================================== 2D outline helpers (Y,Z / X,Z / X,Y)
def arc(r, a0, a1, n, cu=0.0, cv=0.0):
    """n+1 points on a circle of radius r from angle a0 to a1 (radians, measured from +u towards +v)."""
    return [(cu + r * math.cos(a0 + (a1 - a0) * i / n), cv + r * math.sin(a0 + (a1 - a0) * i / n)) for i in range(n + 1)]


def ngon(apothem, sides=8, cu=0.0, cv=0.0):
    """Flat-topped regular polygon outline by apothem (sides=8 -> octagonal armoured housing)."""
    R = apothem / math.cos(math.pi / sides)
    return [(cu + R * math.cos(math.pi / sides + k * 2 * math.pi / sides),
             cv + R * math.sin(math.pi / sides + k * 2 * math.pi / sides)) for k in range(sides)]


def inset(poly, d):
    """Offset a CONVEX outline inward by d (each edge moved in by d, neighbours intersected).
    Asserts if an edge shorter than the offset would fold over (shows up as a dark sliver/spike in renders):
    lengthen that edge or use a smaller d."""
    n = len(poly)
    area = sum(poly[i][0] * poly[(i + 1) % n][1] - poly[(i + 1) % n][0] * poly[i][1] for i in range(n))
    sgn = 1.0 if area > 0 else -1.0
    lines = []
    for i in range(n):
        (u0, v0), (u1, v1) = poly[i], poly[(i + 1) % n]
        eu, ev = u1 - u0, v1 - v0
        L = math.hypot(eu, ev)
        nu, nv = -ev / L * sgn, eu / L * sgn
        lines.append(((u0 + nu * d, v0 + nv * d), (eu, ev)))
    out = []
    for i in range(n):
        (p, a), (q, b) = lines[i - 1], lines[i]
        den = a[0] * b[1] - a[1] * b[0]
        if abs(den) < 1e-9:
            out.append(q)
            continue
        t = ((q[0] - p[0]) * b[1] - (q[1] - p[1]) * b[0]) / den
        out.append((p[0] + a[0] * t, p[1] + a[1] * t))
    for i in range(n):
        a0, a1, b0, b1 = poly[i], poly[(i + 1) % n], out[i], out[(i + 1) % n]
        assert (a1[0] - a0[0]) * (b1[0] - b0[0]) + (a1[1] - a0[1]) * (b1[1] - b0[1]) > 0, \
            f"inset({d}) folds edge {i} {a0}->{a1}; edge too short for this offset"
    return out


def slope_frame(p_hi, p_lo):
    """(matrix, length) for detail lying on a sloped face of an X-extruded prism/loft, between two (y,z)
    outline points. Local +Y runs p_lo->p_hi, local +Z is the outward (upward) normal."""
    du, dv = p_hi[0] - p_lo[0], p_hi[1] - p_lo[1]
    phi = math.atan2(dv, du)
    rot = Matrix.Rotation(phi, 4, 'X')
    if (rot @ Vector((0, 0, 1))).z < 0:
        rot = Matrix.Rotation(phi + math.pi, 4, 'X')
    mid = Vector((0.0, (p_hi[0] + p_lo[0]) / 2, (p_hi[1] + p_lo[1]) / 2))
    return Matrix.Translation(mid) @ rot, math.hypot(du, dv)


# Local +Z onto a vertical face: FACE_FRONT for a +Y facing face, FACE_REAR for -Y (use with riveted_plate):
#   B.riveted_plate(Matrix.Translation((0, y_face, z_mid)) @ FACE_FRONT, hx, hz, VDARK, CHROME)
FACE_FRONT = Matrix.Rotation(math.radians(-90), 4, 'X')
FACE_REAR = Matrix.Rotation(math.radians(90), 4, 'X')


# ==================================================================== existing-mesh helpers
def spatial_islands(me, tol=1e-4):
    """Face-index islands of a mesh, uniting faces that share a vertex POSITION (SE meshes are edge-split, so
    topology alone splits every hard edge). Read-only; returns a list of lists of polygon indices."""
    parent = list(range(len(me.polygons)))

    def find(a):
        while parent[a] != a:
            parent[a] = parent[parent[a]]
            a = parent[a]
        return a
    owner = {}
    for p in me.polygons:
        for vi in p.vertices:
            c = me.vertices[vi].co
            key = (round(c.x / tol), round(c.y / tol), round(c.z / tol))
            if key in owner:
                ra, rb = find(owner[key]), find(p.index)
                if ra != rb:
                    parent[ra] = rb
            else:
                owner[key] = p.index
    groups = {}
    for p in me.polygons:
        groups.setdefault(find(p.index), []).append(p.index)
    return list(groups.values())


def island_bbox(me, island):
    """(min, max) Vectors of a spatial island (list of polygon indices), mesh-local."""
    cs = [me.vertices[vi].co for fi in island for vi in me.polygons[fi].vertices]
    return (Vector([min(c[i] for c in cs) for i in range(3)]), Vector([max(c[i] for c in cs) for i in range(3)]))


def island_faces_within(me, lo, hi):
    """Set of polygon indices of every spatial island whose WHOLE bbox lies inside lo..hi (mesh local).
    Use this, not a per-face centre test, to lift a part out of another mesh: per-face tests pick up stray
    faces of neighbouring parts (bevel strips etc.). Pass it to MeshBuilder.add_mesh as
    face_filter=lambda f: f.index in keep."""
    keep = set()
    for isl in spatial_islands(me):
        mn, mx = island_bbox(me, isl)
        if all(lo[i] <= mn[i] and mx[i] <= hi[i] for i in range(3)):
            keep.update(isl)
    return keep


VANILLA_MATERIAL_LIBS = {"materials.blend", "items.blend", "armors.blend", "character.blend", "foliage.blend",
                         "playeritems.blend", "voxels.blend"}   # SEUT vanilla libs (not enenra.blend)


def append_mesh(blend_path, obj_name):
    """Append an object's mesh from another .blend and return the mesh (the appended object is removed).
    Asserts every material on it is linked from a Materials.blend library (vanilla SE materials, no local .001
    copies) and removes the source .blend's Library entry when nothing references it any more (SKILL 3.2/3.10).
    Object and mesh names usually differ, so pass the OBJECT name."""
    with bpy.data.libraries.load(blend_path, link=False) as (src, dst):
        assert obj_name in src.objects, f"{obj_name!r} not in {blend_path}"
        dst.objects = [obj_name]
    obj = dst.objects[0]
    me = obj.data
    bpy.data.objects.remove(obj)
    for m in me.materials:
        assert m is not None and m.library is not None and \
            os.path.basename(bpy.path.abspath(m.library.filepath)).lower() in VANILLA_MATERIAL_LIBS, \
            f"material {getattr(m, 'name', None)!r} is not linked from a vanilla SEUT library - remap it"
    src_abs = os.path.normcase(os.path.abspath(blend_path))
    for lib in list(bpy.data.libraries):
        if os.path.normcase(os.path.abspath(bpy.path.abspath(lib.filepath))) == src_abs:
            users = [i for c in (bpy.data.meshes, bpy.data.materials, bpy.data.objects, bpy.data.images)
                     for i in c if i.library == lib]
            if not users:
                bpy.data.libraries.remove(lib)
    return me


TILING_MATERIALS = {
    'PaintedMetal_Colorable', 'PaintedMetal_Darker', 'PaintedMetal_VeryDark', 'PaintedMetal_Yellow',
    'Chrome', 'Metal_Dull', 'MetalMilitary', 'BlackMattePlastic', 'Rubber', 'PanelingColorable', 'PanelingDarker',
}


def scale_mesh_keep_texel(me, k, tiling=TILING_MATERIALS, skip_faces=frozenset(), pivot=(0.0, 0.0, 0.0)):
    """Scale mesh data by k about pivot AND multiply the UVs of tiling-material faces by k, so painted metal,
    chrome etc. keep their texel density instead of going blurry/toy-like. Atlas*/Decals/Emissive faces keep
    their UVs (their baked details just get bigger). skip_faces: polygon indices left completely untouched
    (positions and UVs), e.g. a barrel you keep at original size and move separately (it must be its own
    spatial island(s), see island_faces_within). Uniform scaling keeps custom split normals valid.
    Returns the number of faces whose UVs were scaled."""
    pv = Vector(pivot)
    skip_verts = {vi for fi in skip_faces for vi in me.polygons[fi].vertices}
    for v in me.vertices:
        if v.index not in skip_verts:
            v.co = pv + (v.co - pv) * k
    uv = me.uv_layers.active
    n = 0
    for p in me.polygons:
        if p.index in skip_faces:
            continue
        m = me.materials[p.material_index]
        if m is not None and m.name in tiling:
            for li in p.loop_indices:
                uv.data[li].uv *= k
            n += 1
    me.update()
    return n
