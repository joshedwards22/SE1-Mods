"""Shift the gatling barrel subpart origin so the barrel tip sits at TARGET_TIP (m forward of the subpart origin).

Turret weapon effects (Weapons.sbc <Effect OffsetZ>) spawn |OffsetZ| m forward of the barrel subpart ORIGIN, not the
muzzle dummy, so pass the vanilla |OffsetZ| as <target_tip> (LargeGatlingTurret 1.9, SmallGatlingTurret 0.9).

Usage: blender -b file.blend -P shift_barrel_origin.py -- <target_tip> [--save]
Geometry/dummy move +dy inside the Barrel scene, subpart_GatlingBarrel empties move -dy, so world placement is unchanged.
"""
import bpy, os, sys
from mathutils import Matrix, Vector

argv = sys.argv[sys.argv.index("--") + 1:]
TARGET_TIP = float(argv[0])
SAVE = "--save" in argv


def world_matrix(o):
    chain = []
    while o:
        chain.append(o)
        o = o.parent
    m = Matrix.Identity(4)
    for ob in reversed(chain):
        if ob.parent:
            m = m @ ob.matrix_parent_inverse @ ob.matrix_basis
        else:
            m = ob.matrix_basis.copy()
    return m


def world_bbox(o):
    mw = world_matrix(o)
    pts = [mw @ v.co for v in o.data.vertices]
    return tuple(round(min(p[i] for p in pts), 4) for i in range(3)), tuple(round(max(p[i] for p in pts), 4) for i in range(3))


def is_barrel_mesh(o):
    return o.type == 'MESH' and o.name.startswith("GatlingBarrel")


def is_muzzle(o):
    return o.type == 'EMPTY' and o.name.startswith("muzzle_projectile")


def is_barrel_subpart(o):
    return o.type == 'EMPTY' and o.name.startswith("subpart_GatlingBarrel")


real_barrel = bpy.data.objects["GatlingBarrel"]
assert real_barrel.parent is None and real_barrel.matrix_basis.is_identity
tip = max(v.co.y for v in real_barrel.data.vertices)
dy = TARGET_TIP - tip
print("TIP=%.4f TARGET=%.4f DY=%+.4f" % (tip, TARGET_TIP, dy))

main_scene = next(s for s in bpy.data.scenes if s.seut.sceneType == 'mainScene')
main_barrel = next(o for o in main_scene.objects if is_barrel_mesh(o))
muzzles = [o for o in bpy.data.objects if is_muzzle(o)]
before_bbox = world_bbox(main_barrel)
before_muzzle = {o.name: tuple(round(c, 4) for c in world_matrix(o).translation) for o in muzzles}
print("BEFORE main-scene barrel world bbox:", before_bbox)

shift = Matrix.Translation(Vector((0.0, dy, 0.0)))
barrel_meshes = [o for o in bpy.data.objects if is_barrel_mesh(o)]
subparts = [o for o in bpy.data.objects if is_barrel_subpart(o)]

for o in barrel_meshes:
    assert o.data.users == 1, o.name
    assert o.matrix_basis.is_identity, o.name
    o.data.transform(shift)
    o.data.update()
for o in muzzles:
    assert o.parent and (is_barrel_mesh(o.parent) or is_barrel_subpart(o.parent)) and o.matrix_parent_inverse.is_identity, o.name
    o.location.y += dy
for o in subparts:
    assert o.matrix_parent_inverse.is_identity, o.name
    o.location.y -= dy

after_bbox = world_bbox(main_barrel)
after_muzzle = {o.name: tuple(round(c, 4) for c in world_matrix(o).translation) for o in muzzles}
print("AFTER  main-scene barrel world bbox:", after_bbox)
for o in barrel_meshes:
    print("MESH", o.name, "local ymin=%.4f ymax=%.4f" % (min(v.co.y for v in o.data.vertices), max(v.co.y for v in o.data.vertices)))
for o in muzzles:
    print("MUZZLE", o.name, "local y=%.4f" % o.location.y, "world", before_muzzle[o.name], "->", after_muzzle[o.name])
for o in subparts:
    print("SUBPART", o.name, "local y=%.4f" % o.location.y)

for a, b in zip(before_bbox, after_bbox):
    assert all(abs(x - y) < 1e-3 for x, y in zip(a, b)), "world bbox changed"
for n in before_muzzle:
    if "(L)" not in n:
        continue  # the real dummy lives in the Barrel scene's own space and is meant to move with the geometry
    assert all(abs(x - y) < 1e-3 for x, y in zip(before_muzzle[n], after_muzzle[n])), "muzzle world pos changed " + n

for lib in bpy.data.libraries:
    p = bpy.path.abspath(lib.filepath)
    assert os.path.exists(p), "missing library " + p

if SAVE:
    bpy.ops.wm.save_mainfile()
    print("SAVED", bpy.data.filepath)
else:
    print("DRY RUN (not saved)")
