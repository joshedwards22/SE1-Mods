import argparse
import json
import math
import os
import sys

import bmesh
import bpy
import numpy as np
from mathutils import Euler, Matrix, Vector

DEFAULT_PANEL_SIZE = 640
DEFAULT_SAMPLES = 16
HEADER_HEIGHT = 28
BG_COLOR = (0.11, 0.12, 0.14, 1.0)
GRID_BLOCK_SIZES = {"large": 2.5, "small": 0.5}

# Minimal 5x7 uppercase/ASCII bitmap font for panel headers (no PIL required).
FONT_5X7 = {
    "A": ["01110", "10001", "10001", "11111", "10001", "10001", "10001"],
    "B": ["11110", "10001", "10001", "11110", "10001", "10001", "11110"],
    "C": ["01110", "10001", "10000", "10000", "10000", "10001", "01110"],
    "D": ["11100", "10010", "10001", "10001", "10001", "10010", "11100"],
    "E": ["11111", "10000", "10000", "11110", "10000", "10000", "11111"],
    "F": ["11111", "10000", "10000", "11110", "10000", "10000", "10000"],
    "G": ["01110", "10001", "10000", "10111", "10001", "10001", "01110"],
    "H": ["10001", "10001", "10001", "11111", "10001", "10001", "10001"],
    "I": ["01110", "00100", "00100", "00100", "00100", "00100", "01110"],
    "J": ["00111", "00010", "00010", "00010", "10010", "10010", "01100"],
    "K": ["10001", "10010", "10100", "11000", "10100", "10010", "10001"],
    "L": ["10000", "10000", "10000", "10000", "10000", "10000", "11111"],
    "M": ["10001", "11011", "10101", "10101", "10001", "10001", "10001"],
    "N": ["10001", "11001", "10101", "10011", "10001", "10001", "10001"],
    "O": ["01110", "10001", "10001", "10001", "10001", "10001", "01110"],
    "P": ["11110", "10001", "10001", "11110", "10000", "10000", "10000"],
    "Q": ["01110", "10001", "10001", "10001", "10101", "10010", "01101"],
    "R": ["11110", "10001", "10001", "11110", "10100", "10010", "10001"],
    "S": ["01111", "10000", "10000", "01110", "00001", "00001", "11110"],
    "T": ["11111", "00100", "00100", "00100", "00100", "00100", "00100"],
    "U": ["10001", "10001", "10001", "10001", "10001", "10001", "01110"],
    "V": ["10001", "10001", "10001", "10001", "10001", "01010", "00100"],
    "W": ["10001", "10001", "10001", "10101", "10101", "11011", "10001"],
    "X": ["10001", "10001", "01010", "00100", "01010", "10001", "10001"],
    "Y": ["10001", "10001", "01010", "00100", "00100", "00100", "00100"],
    "Z": ["11111", "00001", "00010", "00100", "01000", "10000", "11111"],
    "0": ["01110", "10011", "10101", "10101", "11001", "10001", "01110"],
    "1": ["00100", "01100", "00100", "00100", "00100", "00100", "01110"],
    "2": ["01110", "10001", "00001", "00110", "01000", "10000", "11111"],
    "3": ["11110", "00001", "00001", "01110", "00001", "00001", "11110"],
    "4": ["00010", "00110", "01010", "10010", "11111", "00010", "00010"],
    "5": ["11111", "10000", "11110", "00001", "00001", "10001", "01110"],
    "6": ["01110", "10000", "11110", "10001", "10001", "10001", "01110"],
    "7": ["11111", "00001", "00010", "00100", "01000", "01000", "01000"],
    "8": ["01110", "10001", "10001", "01110", "10001", "10001", "01110"],
    "9": ["01110", "10001", "10001", "01111", "00001", "00001", "01110"],
    "+": ["00000", "00100", "00100", "11111", "00100", "00100", "00000"],
    "-": ["00000", "00000", "00000", "11111", "00000", "00000", "00000"],
    "(": ["00010", "00100", "01000", "01000", "01000", "00100", "00010"],
    ")": ["01000", "00100", "00010", "00010", "00010", "00100", "01000"],
    ":": ["00000", "00100", "00100", "00000", "00100", "00100", "00000"],
    ".": ["00000", "00000", "00000", "00000", "00000", "01100", "01100"],
    ",": ["00000", "00000", "00000", "00000", "00100", "00100", "01000"],
    "/": ["00001", "00010", "00010", "00100", "01000", "01000", "10000"],
    "_": ["00000", "00000", "00000", "00000", "00000", "00000", "11111"],
    "|": ["00100", "00100", "00100", "00100", "00100", "00100", "00100"],
    " ": ["00000", "00000", "00000", "00000", "00000", "00000", "00000"],
}

EMPTY_COLORS = {
    "subpart": (0.15, 0.85, 1.0, 1.0),
    "muzzle": (1.0, 0.25, 0.15, 1.0),
    "camera": (1.0, 0.9, 0.15, 1.0),
    "detector": (0.3, 1.0, 0.4, 1.0),
    "default": (0.95, 0.45, 1.0, 1.0),
}


def parse_args():
    argv = []
    if "--" in sys.argv:
        argv = sys.argv[sys.argv.index("--") + 1 :]
    # '--camera -1.6,1.7,0.9' would be read as an option by argparse: glue x,y,z values to their flag.
    glued = []
    i = 0
    while i < len(argv):
        if argv[i] in ("--camera", "--target") and i + 1 < len(argv):
            glued.append(f"{argv[i]}={argv[i + 1]}")
            i += 2
        else:
            glued.append(argv[i])
            i += 1
    argv = glued
    parser = argparse.ArgumentParser(description="Render SEUT .blend scenes to PNG contact sheets.")
    parser.add_argument("--output", required=True, help="Output PNG path (or directory when --all-scenes).")
    parser.add_argument("--scene", default=None, help="Specific scene name to render.")
    parser.add_argument("--collection", default=None, help="Render only meshes from a specific collection (e.g. 'BS1 (Turret)').")
    parser.add_argument("--all-scenes", action="store_true", help="Render every scene in the .blend file.")
    parser.add_argument("--size", type=int, default=DEFAULT_PANEL_SIZE, help="Per-panel resolution in pixels.")
    parser.add_argument("--samples", type=int, default=DEFAULT_SAMPLES, help="Cycles CPU sample count.")
    parser.add_argument("--clay", action="store_true", help="Override all mesh materials with neutral gray clay.")
    parser.add_argument("--show-collision", action="store_true", help="Overlay collision meshes in translucent amber.")
    parser.add_argument("--no-empties", action="store_true", help="Disable visual markers for SEUT empties/dummies.")
    parser.add_argument("--no-grid", action="store_true", help="Disable SE block wireframe box & origin axes.")
    parser.add_argument("--bottom", action="store_true", help="Render underside views (ISO-Bottom, Back, Left, Bottom) instead of top views.")
    parser.add_argument("--assemble", action="store_true", help="Recursively instance subpart scenes onto subpart_* empties for a full assembled preview.")
    parser.add_argument("--pitch", type=float, default=0.0, help="Elevation pitch angle in degrees applied to Base2 or elevation Barrels subpart when using --assemble.")
    parser.add_argument("--save-individual", action="store_true", help="Also keep individual view PNG files.")
    parser.add_argument("--hide-linked", action="store_true", help="Hide SEUT '(L)' subpart previews (render a subpart scene's own geometry only).")
    parser.add_argument("--camera", default=None, help="Close-up mode: single perspective render from camera position 'x,y,z' (scene world coords) instead of the 2x2 sheet.")
    parser.add_argument("--target", default=None, help="Close-up look-at point 'x,y,z' (default: model bbox centre).")
    parser.add_argument("--lens", type=float, default=40.0, help="Close-up focal length in mm (default 40).")
    parser.add_argument("--width", type=int, default=1000, help="Close-up image width in pixels (default 1000).")
    parser.add_argument("--height", type=int, default=750, help="Close-up image height in pixels (default 750).")
    args = parser.parse_args(argv)

    def vec3(s, name):
        if s is None:
            return None
        parts = [float(x) for x in s.replace(" ", "").split(",")]
        if len(parts) != 3:
            parser.error(f"{name} needs 'x,y,z'")
        return Vector(parts)
    args.camera = vec3(args.camera, "--camera")
    args.target = vec3(args.target, "--target")
    if args.target is not None and args.camera is None:
        parser.error("--target needs --camera")
    return args


def draw_text(img_rgba, text, x0, y0, scale=2, color=(0.95, 0.96, 0.98, 1.0)):
    """Draw simple bitmap text onto a top-down float32 RGBA numpy image."""
    h, w, _ = img_rgba.shape
    cursor_x = x0
    for ch in text.upper():
        glyph = FONT_5X7.get(ch, FONT_5X7[" "])
        for row_idx, row in enumerate(glyph):
            for col_idx, bit in enumerate(row):
                if bit != "1":
                    continue
                py0 = y0 + row_idx * scale
                px0 = cursor_x + col_idx * scale
                py1 = min(h, py0 + scale)
                px1 = min(w, px0 + scale)
                if py0 < h and px0 < w and py1 > 0 and px1 > 0:
                    img_rgba[max(0, py0) : py1, max(0, px0) : px1, :] = color
        cursor_x += 6 * scale


def make_emission_material(name, rgba, strength=1.5):
    mat = bpy.data.materials.new(name=name)
    mat.use_nodes = True
    nodes = mat.node_tree.nodes
    links = mat.node_tree.links
    nodes.clear()
    out = nodes.new("ShaderNodeOutputMaterial")
    emit = nodes.new("ShaderNodeEmission")
    emit.inputs["Color"].default_value = rgba
    emit.inputs["Strength"].default_value = strength
    links.new(emit.outputs["Emission"], out.inputs["Surface"])
    return mat


def make_clay_material(name="__RenderClay", base_color=(0.62, 0.65, 0.70, 1.0)):
    mat = bpy.data.materials.new(name=name)
    mat.use_nodes = True
    bsdf = mat.node_tree.nodes.get("Principled BSDF")
    if bsdf:
        bsdf.inputs["Base Color"].default_value = base_color
        bsdf.inputs["Metallic"].default_value = 0.15
        bsdf.inputs["Roughness"].default_value = 0.45
    return mat


def make_collision_material(name="__RenderCollision"):
    mat = bpy.data.materials.new(name=name)
    mat.use_nodes = True
    nodes = mat.node_tree.nodes
    links = mat.node_tree.links
    nodes.clear()
    out = nodes.new("ShaderNodeOutputMaterial")
    mix = nodes.new("ShaderNodeMixShader")
    mix.inputs["Fac"].default_value = 0.35
    transp = nodes.new("ShaderNodeBsdfTransparent")
    bsdf = nodes.new("ShaderNodeBsdfPrincipled")
    bsdf.inputs["Base Color"].default_value = (1.0, 0.55, 0.05, 1.0)
    bsdf.inputs["Roughness"].default_value = 0.4
    links.new(transp.outputs["BSDF"], mix.inputs[1])
    links.new(bsdf.outputs["BSDF"], mix.inputs[2])
    links.new(mix.outputs["Shader"], out.inputs["Surface"])
    return mat


def sanitize_missing_textures():
    """Replace missing/unloaded image texture links so Cycles never renders magenta."""
    replaced = []
    for mat in bpy.data.materials:
        if not mat.use_nodes or not mat.node_tree:
            continue
        for node in mat.node_tree.nodes:
            if node.type != "TEX_IMAGE":
                continue
            img = node.image
            missing = False
            if img is None:
                missing = True
            elif img.packed_file is None:
                abs_path = bpy.path.abspath(img.filepath, library=img.library)
                if not abs_path or not os.path.exists(abs_path):
                    missing = True
            if missing:
                replaced.append(f"{mat.name}:{node.name}")
                for link in list(node.outputs["Color"].links):
                    to_socket = link.to_socket
                    mat.node_tree.links.remove(link)
                    if hasattr(to_socket, "default_value") and len(to_socket.default_value) == 4:
                        to_socket.default_value = (0.45, 0.48, 0.52, 1.0)
    return replaced


def get_seut_collection_tag(col):
    name_low = col.name.lower()
    if hasattr(col, "seut") and hasattr(col.seut, "col_type"):
        return str(col.seut.col_type).lower()
    for tag in ("collision", "mountpoints", "mirroring", "bs", "lod", "main", "hkt"):
        if name_low.startswith(tag):
            return tag
    return "other"


def classify_scene_objects(scn):
    """Partition scene objects into main meshes, collision meshes, empties, and ignored."""
    main_meshes = []
    collision_meshes = []
    empties = []
    hidden_others = []

    def walk(col, inherited_tag=None):
        tag = get_seut_collection_tag(col)
        if tag == "other" and inherited_tag:
            tag = inherited_tag
        for obj in col.objects:
            if obj.type == "MESH":
                if tag in ("collision", "hkt"):
                    collision_meshes.append(obj)
                elif tag in ("mountpoints", "mirroring", "bs", "lod"):
                    hidden_others.append(obj)
                else:
                    main_meshes.append(obj)
            elif obj.type == "EMPTY":
                if tag not in ("mountpoints", "mirroring", "bs", "lod"):
                    empties.append(obj)
        for child in col.children:
            walk(child, tag)

    walk(scn.collection)
    # Fallback: if no main meshes found, include everything mesh-like.
    if not main_meshes:
        main_meshes = [o for o in scn.objects if o.type == "MESH" and o not in collision_meshes]
    return main_meshes, collision_meshes, empties, hidden_others


def compute_world_bounds(objects):
    min_v = Vector((float("inf"), float("inf"), float("inf")))
    max_v = Vector((float("-inf"), float("-inf"), float("-inf")))
    found = False
    for obj in objects:
        if obj.type == "MESH" and obj.data and len(obj.data.vertices) > 0:
            for corner in obj.bound_box:
                pt = obj.matrix_world @ Vector(corner)
                min_v.x = min(min_v.x, pt.x)
                min_v.y = min(min_v.y, pt.y)
                min_v.z = min(min_v.z, pt.z)
                max_v.x = max(max_v.x, pt.x)
                max_v.y = max(max_v.y, pt.y)
                max_v.z = max(max_v.z, pt.z)
                found = True
        elif obj.type == "EMPTY":
            pt = obj.matrix_world.translation
            min_v.x = min(min_v.x, pt.x)
            min_v.y = min(min_v.y, pt.y)
            min_v.z = min(min_v.z, pt.z)
            max_v.x = max(max_v.x, pt.x)
            max_v.y = max(max_v.y, pt.y)
            max_v.z = max(max_v.z, pt.z)
            found = True
    if not found:
        return Vector((-1.0, -1.0, -1.0)), Vector((1.0, 1.0, 1.0))
    return min_v, max_v


def add_cylinder_segment(bm, p0, p1, radius, segments=8):
    vec = p1 - p0
    length = vec.length
    if length < 1e-6:
        return
    mid = (p0 + p1) * 0.5
    rot = vec.to_track_quat("Z", "Y").to_matrix().to_4x4()
    mat = Matrix.Translation(mid) @ rot
    bmesh.ops.create_cone(
        bm,
        cap_ends=True,
        cap_tris=False,
        segments=segments,
        radius1=radius,
        radius2=radius,
        depth=length,
        matrix=mat,
    )


def create_mesh_object(name, bm, material, collection):
    mesh = bpy.data.meshes.new(name)
    bm.to_mesh(mesh)
    bm.free()
    obj = bpy.data.objects.new(name, mesh)
    obj.data.materials.append(material)
    collection.objects.link(obj)
    return obj


def build_grid_and_axes_overlay(overlay_col, block_size, extent_scale):
    """Add SE block wireframe cube centered at origin plus RGB world-axis rods."""
    wire_radius = max(0.008, extent_scale * 0.004)
    half = block_size * 0.5
    corners = [
        Vector((sx * half, sy * half, sz * half))
        for sx in (-1, 1)
        for sy in (-1, 1)
        for sz in (-1, 1)
    ]
    edges = [
        (0, 1), (0, 2), (0, 4), (7, 6), (7, 5), (7, 3),
        (1, 3), (1, 5), (2, 3), (2, 6), (4, 5), (4, 6),
    ]
    bm = bmesh.new()
    for a, b in edges:
        add_cylinder_segment(bm, corners[a], corners[b], wire_radius, segments=6)
    grid_mat = make_emission_material("__OverlayGrid", (0.45, 0.50, 0.58, 1.0), strength=0.8)
    create_mesh_object("__SE_BlockBox", bm, grid_mat, overlay_col)

    axis_len = max(block_size * 0.65, extent_scale * 0.55)
    axis_rad = max(0.015, extent_scale * 0.008)
    axes = [
        ("__AxisX", Vector((axis_len, 0.0, 0.0)), (0.95, 0.15, 0.15, 1.0)),
        ("__AxisY", Vector((0.0, axis_len, 0.0)), (0.15, 0.90, 0.25, 1.0)),
        ("__AxisZ", Vector((0.0, 0.0, axis_len)), (0.20, 0.45, 1.0, 1.0)),
    ]
    for name, tip, color in axes:
        bm_axis = bmesh.new()
        add_cylinder_segment(bm_axis, Vector((0.0, 0.0, 0.0)), tip, axis_rad, segments=8)
        mat = make_emission_material(f"__Mat{name}", color, strength=1.8)
        create_mesh_object(name, bm_axis, mat, overlay_col)


def build_empties_overlay(overlay_col, empties, extent_scale):
    """Create colored marker spheres & forward-direction spikes for SEUT empties."""
    marker_rad = max(0.04, extent_scale * 0.022)
    spike_len = marker_rad * 3.5
    mats = {
        key: make_emission_material(f"__EmptyMat_{key}", rgba, strength=2.2)
        for key, rgba in EMPTY_COLORS.items()
    }
    legend = []
    for emp in empties:
        low = emp.name.lower()
        cat = "default"
        for key in ("subpart", "muzzle", "camera", "detector"):
            if key in low:
                cat = key
                break
        pos = emp.matrix_world.translation
        # Local -Z and -Y indicator; in SEUT empties, +Z is up and -Y or -Z often indicates forward.
        fwd = (emp.matrix_world.to_3x3() @ Vector((0.0, -1.0, 0.0))).normalized()
        bm = bmesh.new()
        bmesh.ops.create_icosphere(
            bm,
            subdivisions=1,
            radius=marker_rad,
            matrix=Matrix.Translation(pos),
        )
        add_cylinder_segment(bm, pos, pos + fwd * spike_len, marker_rad * 0.35, segments=6)
        create_mesh_object(f"__EmptyMarker_{emp.name}", bm, mats[cat], overlay_col)
        legend.append({
            "name": emp.name,
            "category": cat,
            "location": [round(v, 4) for v in pos],
        })
    return legend


def setup_studio_lighting(overlay_col, center, radius):
    """Add neutral 3-point lighting so every angle is evenly legible."""
    dist = max(4.0, radius * 4.0)
    light_specs = [
        ("__KeyLight", center + Vector((dist, -dist, dist * 1.1)), 3.0),
        ("__FillLight", center + Vector((-dist, -dist * 0.5, dist * 0.4)), 1.6),
        ("__RimLight", center + Vector((-dist * 0.5, dist, dist * 0.8)), 1.8),
        ("__BottomFill", center + Vector((0.0, 0.0, -dist)), 0.7),
    ]
    for name, loc, energy in light_specs:
        ldata = bpy.data.lights.new(name=name, type="SUN")
        ldata.energy = energy
        ldata.angle = math.radians(18.0)
        lobj = bpy.data.objects.new(name=name, object_data=ldata)
        lobj.location = loc
        direction = center - loc
        lobj.rotation_euler = direction.to_track_quat("-Z", "Y").to_euler()
        overlay_col.objects.link(lobj)


def setup_render_engine(scn, size, samples):
    scn.render.engine = "CYCLES"
    scn.cycles.device = "CPU"
    scn.cycles.samples = samples
    scn.cycles.use_adaptive_sampling = True
    scn.cycles.max_bounces = 3
    scn.cycles.diffuse_bounces = 1
    scn.cycles.glossy_bounces = 1
    scn.cycles.transmission_bounces = 2
    scn.cycles.transparent_max_bounces = 4
    try:
        scn.cycles.use_denoising = True
    except Exception:
        pass

    scn.render.resolution_x = size
    scn.render.resolution_y = size
    scn.render.resolution_percentage = 100
    scn.render.image_settings.file_format = "PNG"
    scn.render.image_settings.color_mode = "RGBA"
    scn.render.film_transparent = False

    if not scn.world:
        scn.world = bpy.data.worlds.new("__RenderWorld")
    scn.world.use_nodes = True
    bg = scn.world.node_tree.nodes.get("Background")
    if bg:
        bg.inputs["Color"].default_value = BG_COLOR
        bg.inputs["Strength"].default_value = 0.75


def load_png_topdown(filepath):
    img = bpy.data.images.load(filepath, check_existing=False)
    w, h = img.size
    arr = np.array(img.pixels[:], dtype=np.float32).reshape((h, w, 4))
    bpy.data.images.remove(img)
    # Blender stores pixels bottom-to-top; flip vertically for intuitive top-down compositing.
    return np.flipud(arr)


def save_png_topdown(arr_rgba, filepath):
    h, w, _ = arr_rgba.shape
    flipped = np.flipud(np.clip(arr_rgba, 0.0, 1.0))
    out_img = bpy.data.images.new("__ContactSheet", width=w, height=h, alpha=True)
    out_img.pixels.foreach_set(flipped.ravel())
    out_img.filepath_raw = filepath
    out_img.file_format = "PNG"
    out_img.save()
    bpy.data.images.remove(out_img)


def render_scene_contact_sheet(scn, out_png, args):
    bpy.context.window.scene = scn
    bpy.context.view_layer.update()

    main_meshes, collision_meshes, empties, hidden_others = classify_scene_objects(scn)
    if args.hide_linked and not args.assemble:
        # Show only this scene's own geometry: hide SEUT '(L)' previews of child subparts (e.g. the pods
        # hanging off a Base1 scene) so they don't hide what you're inspecting.
        for obj in main_meshes:
            if "(L)" in obj.name:
                obj.hide_render = True
        main_meshes = [o for o in main_meshes if "(L)" not in o.name]
        empties = [o for o in empties if "(L)" not in o.name]
    if args.collection:
        target_col = bpy.data.collections.get(args.collection)
        if not target_col:
            raise ValueError(f"Collection '{args.collection}' not found.")
        main_meshes = [o for o in target_col.objects if o.type == "MESH"]
        collision_meshes = []
        empties = []
        hidden_others = [o for o in scn.objects if o.type == "MESH" and o not in main_meshes]
        # Ensure target collection is not excluded/hidden for render.
        target_col.hide_render = False
        for obj in main_meshes:
            obj.hide_viewport = False
            obj.hide_render = False
    grid_scale_key = "large"
    if hasattr(scn, "seut") and hasattr(scn.seut, "gridScale"):
        grid_scale_key = str(scn.seut.gridScale).lower()
    block_size = GRID_BLOCK_SIZES.get(grid_scale_key, 2.5)

    # Hide non-main SEUT helper collections (mountpoints, LODs, BS) and existing scene lights.
    for obj in scn.objects:
        if obj.type in ("LIGHT", "CAMERA"):
            obj.hide_render = True
    for obj in hidden_others:
        obj.hide_render = True
    for obj in main_meshes:
        obj.hide_render = False

    clay_mat = make_clay_material() if args.clay else None
    if clay_mat:
        for obj in main_meshes:
            obj.data.materials.clear()
            obj.data.materials.append(clay_mat)

    if args.show_collision and collision_meshes:
        col_mat = make_collision_material()
        for obj in collision_meshes:
            obj.hide_render = False
            obj.data.materials.clear()
            obj.data.materials.append(col_mat)
    else:
        for obj in collision_meshes:
            obj.hide_render = True

    overlay_col = bpy.data.collections.new("__RenderOverlay")
    scn.collection.children.link(overlay_col)

    assembled_meshes = []
    if args.assemble:
        # Hide SEUT's existing '(L)' viewport preview instances so they never Z-fight with assembled copies.
        kept_main = []
        for obj in main_meshes:
            if "(L)" in obj.name:
                obj.hide_render = True
            else:
                kept_main.append(obj)
        main_meshes = kept_main

        def obj_local_world_mat(obj):
            """Compute world matrix safely even for objects in inactive scenes."""
            m = obj.matrix_basis.copy()
            cur = obj
            while cur.parent is not None:
                m = cur.parent.matrix_basis @ cur.matrix_parent_inverse @ m
                cur = cur.parent
            return m

        def mount_subparts(src_scn, parent_mat, depth=0):
            if depth > 5:
                return
            _, _, src_empties, _ = classify_scene_objects(src_scn)
            for emp in src_empties:
                if "(L)" in emp.name or not emp.name.lower().startswith("subpart_"):
                    continue
                target_scn = None
                if hasattr(emp, "seut") and getattr(emp.seut, "linkedScene", None):
                    target_scn = emp.seut.linkedScene
                if target_scn is None and "file" in emp:
                    target_scn = bpy.data.scenes.get(str(emp["file"]))
                if target_scn is None or target_scn == src_scn:
                    continue
                emp_mat = parent_mat @ obj_local_world_mat(emp)
                emp_low = emp.name.lower()
                is_elevation_subpart = ("base2" in emp_low) or (
                    "base1" in src_scn.name.lower() and ("barrel" in emp_low or "elevation" in emp_low)
                )
                if args.pitch and is_elevation_subpart:
                    emp_mat = emp_mat @ Euler((math.radians(args.pitch), 0.0, 0.0), "XYZ").to_matrix().to_4x4()
                sub_meshes, _, _, _ = classify_scene_objects(target_scn)
                for sm in sub_meshes:
                    if "(L)" in sm.name:
                        continue
                    dup = sm.copy()
                    dup.data = sm.data.copy()
                    dup.parent = None
                    dup.matrix_world = emp_mat @ obj_local_world_mat(sm)
                    dup.hide_render = False
                    if clay_mat:
                        dup.data.materials.clear()
                        dup.data.materials.append(clay_mat)
                    overlay_col.objects.link(dup)
                    assembled_meshes.append(dup)
                mount_subparts(target_scn, emp_mat, depth + 1)

        mount_subparts(scn, Matrix.Identity(4))
        bpy.context.view_layer.update()

    bounds_targets = list(main_meshes) + list(assembled_meshes) + (list(empties) if not args.no_empties else [])
    if args.show_collision:
        bounds_targets += list(collision_meshes)
    min_v, max_v = compute_world_bounds(bounds_targets)
    dims = max_v - min_v
    center = (min_v + max_v) * 0.5
    max_dim = max(dims.x, dims.y, dims.z, block_size * 0.5, 0.5)
    radius = max(dims.length * 0.5, block_size * 0.5, 0.5)

    if not args.no_grid:
        build_grid_and_axes_overlay(overlay_col, block_size, max_dim)

    empty_legend = []
    if not args.no_empties and empties:
        empty_legend = build_empties_overlay(overlay_col, empties, max_dim)

    setup_studio_lighting(overlay_col, center, radius)
    setup_render_engine(scn, args.size, args.samples)

    cam_data = bpy.data.cameras.new("__RenderCam")
    cam_obj = bpy.data.objects.new("__RenderCam", cam_data)
    overlay_col.objects.link(cam_obj)
    scn.camera = cam_obj

    if args.camera is not None:
        # Close-up mode: one perspective shot from an explicit camera position, for judging greebles/details.
        target = args.target if args.target is not None else center
        scn.render.resolution_x = args.width
        scn.render.resolution_y = args.height
        cam_data.type = "PERSP"
        cam_data.lens = args.lens
        cam_data.clip_start = 0.01
        cam_data.clip_end = max(200.0, (args.camera - target).length * 20.0)
        cam_obj.location = args.camera
        cam_obj.rotation_euler = (target - args.camera).to_track_quat("-Z", "Y").to_euler()
        os.makedirs(os.path.dirname(os.path.abspath(out_png)), exist_ok=True)
        scn.render.filepath = out_png
        bpy.ops.render.render(write_still=True)
        return {
            "scene": scn.name,
            "output_png": out_png,
            "mode": "closeup",
            "camera": [round(v, 4) for v in args.camera],
            "target": [round(v, 4) for v in target],
            "lens_mm": args.lens,
            "bbox_min": [round(v, 4) for v in min_v],
            "bbox_max": [round(v, 4) for v in max_v],
        }

    ortho_scale = max(max_dim, block_size if not args.no_grid else 0.0) * 1.45
    dist = max(radius * 3.2, ortho_scale * 2.5)
    iso_dir = Vector((1.0, -1.15, 0.85)).normalized()

    # Space Engineers / SEUT conventions: -Y = Front, +X = Right, +Z = Up.
    if args.bottom:
        iso_bot_dir = Vector((-1.0, 1.15, -0.85)).normalized()
        views = [
            ("iso_bottom", "ISO BOTTOM (-X,+Y,-Z)", "PERSP", center + iso_bot_dir * (radius * 2.75), None),
            ("back", "BACK (+Y VIEW)", "ORTHO", center + Vector((0.0, dist, 0.0)), ortho_scale),
            ("left", "LEFT (-X VIEW)", "ORTHO", center + Vector((-dist, 0.0, 0.0)), ortho_scale),
            ("bottom", "BOTTOM (-Z VIEW)", "ORTHO", center + Vector((0.0, 0.0, -dist)), ortho_scale),
        ]
    else:
        views = [
            ("iso", "ISO (+X,-Y,+Z)", "PERSP", center + iso_dir * (radius * 2.75), None),
            ("front", "FRONT (-Y VIEW)", "ORTHO", center + Vector((0.0, -dist, 0.0)), ortho_scale),
            ("right", "RIGHT (+X VIEW)", "ORTHO", center + Vector((dist, 0.0, 0.0)), ortho_scale),
            ("top", "TOP (+Z VIEW)", "ORTHO", center + Vector((0.0, 0.0, dist)), ortho_scale),
        ]

    os.makedirs(os.path.dirname(os.path.abspath(out_png)), exist_ok=True)
    stem, _ = os.path.splitext(out_png)
    panels = []

    for slug, label, cam_type, cam_pos, o_scale in views:
        cam_data.type = cam_type
        if cam_type == "PERSP":
            cam_data.lens = 45.0
        else:
            cam_data.ortho_scale = o_scale
        cam_data.clip_start = 0.01
        cam_data.clip_end = max(200.0, dist * 10.0)

        cam_obj.location = cam_pos
        direction = center - cam_pos
        if slug == "top":
            # Keep +Y up and +X right in top-down plan view.
            cam_obj.rotation_euler = Euler((0.0, 0.0, 0.0), "XYZ")
        elif slug == "bottom":
            # Look straight up from -Z toward +Z.
            cam_obj.rotation_euler = Euler((math.pi, 0.0, 0.0), "XYZ")
        else:
            cam_obj.rotation_euler = direction.to_track_quat("-Z", "Y").to_euler()

        panel_path = f"{stem}_{slug}.png"
        scn.render.filepath = panel_path
        bpy.ops.render.render(write_still=True)
        arr = load_png_topdown(panel_path)
        if not args.save_individual and os.path.exists(panel_path):
            os.remove(panel_path)
        panels.append((label, arr))

    # Stitch 2x2 contact sheet with header banners.
    p_size = args.size
    cell_h = p_size + HEADER_HEIGHT
    footer_h = 34
    sheet_w = p_size * 2 + 4
    sheet_h = cell_h * 2 + 4 + footer_h
    sheet = np.zeros((sheet_h, sheet_w, 4), dtype=np.float32)
    sheet[:, :, :] = (0.06, 0.07, 0.09, 1.0)

    positions = [(0, 0), (p_size + 4, 0), (0, cell_h + 4), (p_size + 4, cell_h + 4)]
    for (label, arr), (ox, oy) in zip(panels, positions):
        sheet[oy : oy + HEADER_HEIGHT, ox : ox + p_size, :] = (0.14, 0.16, 0.20, 1.0)
        draw_text(sheet, label, ox + 10, oy + 7, scale=2)
        sheet[oy + HEADER_HEIGHT : oy + cell_h, ox : ox + p_size, :] = arr

    footer_y = cell_h * 2 + 4
    sheet[footer_y:, :, :] = (0.10, 0.11, 0.14, 1.0)
    footer_text = (
        f"SCENE: {scn.name} | DIMS: {dims.x:.2f}X {dims.y:.2f}Y {dims.z:.2f}Z M | "
        f"GRID: {block_size:.1f}M | AXES: R+X G+Y B+Z"
    )
    draw_text(sheet, footer_text, 10, footer_y + 10, scale=2, color=(0.82, 0.88, 0.96, 1.0))

    save_png_topdown(sheet, out_png)

    return {
        "scene": scn.name,
        "output_png": out_png,
        "grid_scale": grid_scale_key,
        "block_size_m": block_size,
        "bbox_min": [round(v, 4) for v in min_v],
        "bbox_max": [round(v, 4) for v in max_v],
        "dimensions_m": [round(v, 4) for v in dims],
        "main_meshes": [o.name for o in main_meshes],
        "collision_meshes": [o.name for o in collision_meshes],
        "empties": empty_legend,
    }


def choose_scenes(args):
    if args.scene:
        scn = bpy.data.scenes.get(args.scene)
        if not scn:
            raise ValueError(f"Scene '{args.scene}' not found. Available: {[s.name for s in bpy.data.scenes]}")
        return [scn]
    if args.all_scenes:
        return list(bpy.data.scenes)
    # Prefer the SEUT mainScene if present, else the active scene.
    for scn in bpy.data.scenes:
        if hasattr(scn, "seut") and getattr(scn.seut, "sceneType", "") == "mainScene":
            return [scn]
    return [bpy.context.scene]


def main():
    args = parse_args()
    replaced_textures = sanitize_missing_textures()
    scenes = choose_scenes(args)
    results = []

    is_dir_target = args.all_scenes or os.path.isdir(args.output) or not args.output.lower().endswith(".png")
    if len(scenes) == 1 and not is_dir_target:
        results.append(render_scene_contact_sheet(scenes[0], args.output, args))
        meta_path = os.path.splitext(args.output)[0] + "_render_meta.json"
    else:
        os.makedirs(args.output, exist_ok=True)
        for scn in scenes:
            safe_name = "".join(c if c.isalnum() or c in ("-", "_") else "_" for c in scn.name)
            out_png = os.path.join(args.output, f"{safe_name}.png")
            results.append(render_scene_contact_sheet(scn, out_png, args))
        meta_path = os.path.join(args.output, "render_meta.json")

    meta = {
        "filepath": bpy.data.filepath,
        "blender_version": bpy.app.version_string,
        "replaced_missing_textures": replaced_textures,
        "renders": results,
    }
    with open(meta_path, "w", encoding="utf-8") as handle:
        json.dump(meta, handle, indent=2)
    print("WROTE_RENDER_META:", meta_path)
    for item in results:
        print("WROTE_RENDER_PNG:", item["output_png"])


if __name__ == "__main__":
    main()
