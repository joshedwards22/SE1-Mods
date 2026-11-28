"""Headless SEUT exporter with path, dual-grid, and SBC safety guards.

Modes:
    1) In-file (one .blend opened by Blender):
        blender.exe -b "<file.blend>" -P export_blend.py -- [options]

    2) Batch (a .blend file OR a directory of .blend files; one Blender process per file):
        blender.exe -b -P export_blend.py -- --path "<file.blend | directory>" [options]
       (also runs under plain Python: python export_blend.py --path ... [--blender <blender.exe>])
       Step 1: read-only check (dry run) of every file in parallel, which gives each scene's export folder.
       Step 2: files that write into the same folder are grouped, because SEUT's MwmBuilder call
               (/m:<SubtypeId>*.fbx) and loose-file cleanup would clash if two exports shared a folder.
       Step 3: groups export in parallel (--jobs); files inside a group export one after another.
       Files with nothing to export are not opened again after step 1.

Every SEUT scene in each file is checked and exported on its own:
    - Scenes with no export_exportPath are SKIPPED (intentionally not exported).
    - Scenes failing a guard or whose SEUT export errors are FAILED; the other scenes still export.
At the end a report lists only the scenes/files that were NOT exported, with the reason.
Exit code: 0 when nothing FAILED (skips are fine), 1 otherwise.

Options:
    --path <file_or_dir>         Batch mode: a .blend file or a directory to scan for .blend files.
    --no-recursive               Batch mode: only scan the top level of --path (default: recurse).
    --blender <exe>              Batch mode: Blender executable (default: the running Blender, else BLENDER_EXE).
    --timeout <seconds>          Batch mode: per-file timeout (default 1800).
    --jobs <n>                   Batch mode: Blender processes at once (default 4; 1 = one file at a time).
    --expect-mod <name_or_path>  Fail any scene whose export path is outside this mod folder.
    --scene <name>               Export only a single scene (in-file mode or --path to a single .blend).
    --allow-dual-grid            Allow both export_largeGrid and export_smallGrid enabled.
    --allow-sbc                  Keep scene export_sbc_type (default forces 'none' in memory).
    --dry-run                    Validate scenes and export paths without running export.
"""

import argparse
import concurrent.futures
import json
import os
import subprocess
import sys
import tempfile
import threading

try:
    import bpy
except ImportError:  # batch driver under plain Python
    bpy = None


DEFAULT_MODS_ROOT = r"C:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods"
BLENDER_EXE = r"C:\Program Files\Blender Foundation\Blender 4.5\blender.exe"
EXPORTABLE_SCENE_TYPES = ("mainScene", "subpart", "character", "character_animation", "item")
OUTPUT_TAIL_LINES = 15
DEFAULT_JOBS = 4

STATUS_EXPORTED = "EXPORTED"
STATUS_DRY_RUN_OK = "DRY_RUN_OK"
STATUS_SKIPPED = "SKIPPED"
STATUS_FAILED = "FAILED"


def script_argv():
    if bpy is not None:
        return sys.argv[sys.argv.index("--") + 1 :] if "--" in sys.argv else []
    return sys.argv[1:]


def parse_args():
    parser = argparse.ArgumentParser(description="Headless SEUT exporter with safety guards.")
    parser.add_argument("--path", default="", help="Batch mode: .blend file or directory of .blend files.")
    parser.add_argument("--no-recursive", action="store_true", help="Batch mode: do not recurse into subfolders.")
    parser.add_argument("--blender", default="", help="Batch mode: Blender executable.")
    parser.add_argument("--timeout", type=int, default=1800, help="Batch mode: per-file timeout in seconds.")
    parser.add_argument("--jobs", type=int, default=DEFAULT_JOBS, help="Batch mode: Blender processes at once.")
    parser.add_argument("--expect-mod", default="", help="Expected mod folder name or absolute path.")
    parser.add_argument("--scene", default="", help="Export only this scene name (default: all).")
    parser.add_argument("--allow-dual-grid", action="store_true", help="Allow both largeGrid and smallGrid enabled.")
    parser.add_argument("--allow-sbc", action="store_true", help="Do not force export_sbc_type='none'.")
    parser.add_argument("--dry-run", action="store_true", help="Validate export settings without exporting.")
    parser.add_argument("--worker-json", default="", help=argparse.SUPPRESS)
    return parser.parse_args(script_argv())


# ---------------------------------------------------------------- path helpers

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


# ---------------------------------------------------------------- in-file mode (runs inside Blender with a .blend)

def get_view3d_area():
    for win in bpy.context.window_manager.windows:
        for area in win.screen.areas:
            if area.type == "VIEW_3D":
                return area
    area = bpy.context.window_manager.windows[0].screen.areas[0]
    area.type = "VIEW_3D"
    return area


def is_seut_scene(scn):
    return "SEUT" in scn.view_layers


def preflight_scene(scn, args, expected_root):
    """Returns (status, reason, resolved_path). status is None when the scene is ready to export."""
    seut = scn.seut
    scene_type = str(getattr(seut, "sceneType", ""))
    grid_scale = str(getattr(seut, "gridScale", "")).lower()
    large_grid = bool(getattr(seut, "export_largeGrid", False))
    small_grid = bool(getattr(seut, "export_smallGrid", False))
    raw_path = str(getattr(seut, "export_exportPath", "")).strip()
    resolved_path = resolve_export_path(raw_path)

    print(
        f"  {scn.name}: type={scene_type} grid={grid_scale} "
        f"large={large_grid} small={small_grid} path='{resolved_path}'"
    )

    if not resolved_path:
        return STATUS_SKIPPED, "no export path set", ""
    if scene_type not in EXPORTABLE_SCENE_TYPES:
        return STATUS_SKIPPED, f"sceneType '{scene_type}' is not exportable", resolved_path

    errors = []
    # Mod folder = mod root (Data\, Textures\); export path = where .mwm go, must be inside <Mod>\Models.
    # SEUT only checks export_path.startswith(mod_path), which passes when the Mod folder is empty.
    mod_root = resolve_export_path(str(getattr(seut, "mod_path", "")).strip())
    if not mod_root:
        errors.append("export path is set but the Mod folder is empty")
    elif not os.path.isdir(mod_root):
        errors.append(f"Mod folder '{mod_root}' does not exist")
    elif not is_subpath(resolved_path, os.path.join(mod_root, "Models")):
        errors.append(f"export path '{resolved_path}' is not inside the Mod folder's Models ('{mod_root}\\Models')")

    if expected_root and not is_subpath(resolved_path, expected_root):
        errors.append(f"export path '{resolved_path}' is outside expected mod '{expected_root}'")

    # Never silently write to a bogus drive/folder: the path or at least its parent (e.g. Models\Cubes) must exist
    if not os.path.isdir(resolved_path) and not os.path.isdir(os.path.dirname(resolved_path)):
        errors.append(
            f"export path '{resolved_path}' (and parent) do not exist "
            f"(nearest existing: '{find_existing_ancestor(resolved_path)}')"
        )

    # Dual-grid checkbox trap (writes LG_/SG_ prefixed files instead of <Scene>.mwm)
    if large_grid and small_grid and not args.allow_dual_grid:
        errors.append(
            "BOTH export_largeGrid and export_smallGrid enabled (would write LG_/SG_ prefixed files); "
            "fix the flags or pass --allow-dual-grid"
        )
    elif not large_grid and not small_grid:
        errors.append("NEITHER export_largeGrid nor export_smallGrid enabled")
    elif not args.allow_dual_grid:
        if grid_scale == "large" and not large_grid:
            errors.append("gridScale='large' but export_largeGrid=False")
        elif grid_scale == "small" and not small_grid:
            errors.append("gridScale='small' but export_smallGrid=False")

    if errors:
        return STATUS_FAILED, "; ".join(errors), resolved_path
    return None, "", resolved_path


def export_scene(scn, view3d_area, popups):
    """Exports one scene; returns (status, reason)."""
    first_popup = len(popups)
    try:
        with bpy.context.temp_override(area=view3d_area):
            bpy.context.window.scene = scn
            res = bpy.ops.scene.export()
    except Exception as ex:  # operator poll failure or exception inside SEUT
        return STATUS_FAILED, f"export raised {type(ex).__name__}: {ex}"

    error_texts = [text for title, text in popups[first_popup:] if "error" in title.lower()]
    if res != {"FINISHED"} or error_texts:
        reason = f"SEUT export returned {sorted(res)}"
        if error_texts:
            reason += ": " + " | ".join(error_texts)
        return STATUS_FAILED, reason
    return STATUS_EXPORTED, ""


def run_in_file(args):
    blend_path = bpy.data.filepath
    results = []
    file_error = ""
    try:
        results = export_open_file(args)
    except Exception as ex:
        file_error = f"{type(ex).__name__}: {ex}"
        print(f"SEUT_FILE_ERROR: {file_error}")

    if args.worker_json:
        with open(args.worker_json, "w", encoding="utf-8") as f:
            json.dump({"file": blend_path, "file_error": file_error, "scenes": results}, f, indent=2)
    else:
        print_report([{"file": blend_path, "display": os.path.basename(blend_path),
                       "file_error": file_error, "scenes": results}], args.dry_run)

    if file_error or any(r["status"] == STATUS_FAILED for r in results):
        sys.exit(1)


def export_open_file(args):
    seut_mod = next((m.split(".")[0] for m in sys.modules if "space-engineers-utilities" in m), None)
    if not seut_mod:
        raise RuntimeError("SEUT addon not loaded (run with BypassSandbox: true if user addons are blocked)")
    seut_errors = sys.modules[f"{seut_mod}.seut_errors"]

    # Stub popup reports so headless -b mode never hits UI_popup_menu_end crash
    popups = []
    seut_errors.show_popup_report = lambda context, title, text: popups.append((str(title), str(text)))

    if args.scene:
        if args.scene not in bpy.data.scenes:
            raise RuntimeError(f"Scene '{args.scene}' not found")
        target_scenes = [bpy.data.scenes[args.scene]]
    else:
        target_scenes = [s for s in bpy.data.scenes if is_seut_scene(s)]

    expected_root = resolve_expected_mod_root(args.expect_mod)
    if expected_root and not os.path.isdir(expected_root):
        raise RuntimeError(f"--expect-mod directory does not exist: {expected_root}")

    print(f"=== SEUT EXPORT PLAN: {bpy.data.filepath} ===")
    results = []
    ready = []
    for scn in target_scenes:
        if not is_seut_scene(scn):
            results.append({"scene": scn.name, "status": STATUS_SKIPPED, "reason": "not a SEUT scene", "path": ""})
            continue
        status, reason, path = preflight_scene(scn, args, expected_root)
        if status is None:
            ready.append((scn, path))
        else:
            results.append({"scene": scn.name, "status": status, "reason": reason, "path": path})

    if not target_scenes:
        results.append({"scene": "", "status": STATUS_SKIPPED, "reason": "file has no SEUT scenes", "path": ""})

    if args.dry_run:
        results += [{"scene": s.name, "status": STATUS_DRY_RUN_OK, "reason": "", "path": p} for s, p in ready]
        return results

    if ready:
        # Prevent overwriting hand-edited .sbc files unless explicitly requested
        if not args.allow_sbc:
            for scn in bpy.data.scenes:
                if is_seut_scene(scn):
                    scn.seut.export_sbc_type = "none"

        view3d_area = get_view3d_area()
        original_scene = bpy.context.window.scene
        for scn, path in ready:
            status, reason = export_scene(scn, view3d_area, popups)
            print(f"SEUT_SCENE_{status}: {scn.name}" + (f" -- {reason}" if reason else ""))
            results.append({"scene": scn.name, "status": status, "reason": reason, "path": path})
        bpy.context.window.scene = original_scene

    if popups:
        print("SEUT_POPUPS:")
        for title, text in popups:
            print(f"  [{title}] {text}")
    return results


# ---------------------------------------------------------------- batch mode (one Blender process per .blend)

def find_blends(root, recursive):
    if os.path.isfile(root):
        return [root]
    if not recursive:
        return sorted(os.path.join(root, f) for f in os.listdir(root) if f.lower().endswith(".blend"))
    blends = []
    for dirpath, dirnames, filenames in os.walk(root):
        dirnames[:] = sorted(d for d in dirnames if not d.startswith(".") and d != "__pycache__")
        blends += [os.path.join(dirpath, f) for f in sorted(filenames) if f.lower().endswith(".blend")]
    return blends


def output_tail(text):
    lines = [l for l in text.splitlines() if l.strip() and "Cannot relocate indirectly linked library" not in l]
    return "\n".join(lines[-OUTPUT_TAIL_LINES:])


def run_batch(args):
    root = os.path.normpath(os.path.abspath(args.path))
    if not os.path.exists(root):
        print(f"SEUT_BATCH_ERROR: --path does not exist: {root}")
        sys.exit(1)
    if os.path.isfile(root) and not root.lower().endswith(".blend"):
        print(f"SEUT_BATCH_ERROR: --path is not a .blend file: {root}")
        sys.exit(1)
    if args.scene and not os.path.isfile(root):
        print("SEUT_BATCH_ERROR: --scene can only be used with a single .blend file")
        sys.exit(1)

    if args.jobs < 1:
        print("SEUT_BATCH_ERROR: --jobs must be at least 1")
        sys.exit(1)

    blender = args.blender or (bpy.app.binary_path if bpy is not None else BLENDER_EXE)
    blends = find_blends(root, not args.no_recursive)
    base_dir = root if os.path.isdir(root) else os.path.dirname(root)
    work_dir = tempfile.mkdtemp(prefix="seut_export_")

    passthrough = []
    if args.expect_mod:
        passthrough += ["--expect-mod", args.expect_mod]
    if args.scene:
        passthrough += ["--scene", args.scene]
    for flag in ("allow_dual_grid", "allow_sbc"):
        if getattr(args, flag):
            passthrough.append("--" + flag.replace("_", "-"))

    runner = BatchRunner(blender, blends, base_dir, work_dir, passthrough, args.timeout)
    print(f"=== SEUT BATCH EXPORT: {len(blends)} file(s) under {root}, jobs={args.jobs} ===")

    # Step 1: read-only check pass on every file, in parallel (finds each scene's export folder)
    print(f"--- Step 1/3: checking {len(blends)} file(s) ---", flush=True)
    plan = runner.run_many(range(len(blends)), dry_run=True, jobs=args.jobs)

    if args.dry_run:
        results = [plan[i] for i in range(len(blends))]
    else:
        # Step 2: group files that write into the same folder (they must export one after another)
        to_export = [i for i in range(len(blends)) if not plan[i]["file_error"]
                     and any(s["status"] == STATUS_DRY_RUN_OK for s in plan[i]["scenes"])]
        unplanned = [i for i in range(len(blends)) if plan[i]["file_error"]]
        groups = group_by_export_dirs(to_export, {i: export_dirs(plan[i], args.allow_dual_grid) for i in to_export})
        print(f"--- Step 2/3: {len(to_export)} file(s) to export in {len(groups)} folder group(s) ---")
        for group in groups:
            if len(group) > 1:
                print("    sequential: " + ", ".join(runner.display(i) for i in group))

        # Step 3: export groups in parallel, files inside a group one at a time
        print("--- Step 3/3: exporting ---", flush=True)
        exported = runner.run_groups(groups, jobs=args.jobs)
        # Files whose check pass crashed have unknown folders: export them alone, after everything else
        if unplanned:
            print(f"    {len(unplanned)} file(s) failed the check pass; exporting them one at a time", flush=True)
            exported.update(runner.run_groups([[i] for i in unplanned], jobs=1))
        # Files with nothing to export keep their check-pass results (skips / failures)
        results = [exported.get(i, plan[i]) for i in range(len(blends))]

    any_failed = print_report(results, args.dry_run)
    if any(r.get("log") for r in results):
        print(f"Full Blender logs for failed files: {work_dir}")
    else:
        for name in os.listdir(work_dir):
            os.remove(os.path.join(work_dir, name))
        os.rmdir(work_dir)
    sys.exit(1 if any_failed else 0)


def export_dirs(entry, dual_grid):
    """Folders a file's exportable scenes write into.

    With dual_grid, also the \\large<->\\small swap SEUT's export() applies when it exports the other grid size
    (without --allow-dual-grid, dual-grid scenes fail the check and never export)."""
    dirs = set()
    for s in entry["scenes"]:
        if s["status"] != STATUS_DRY_RUN_OK or not s["path"]:
            continue
        path = os.path.normcase(os.path.normpath(s["path"]))
        dirs.add(path)
        if dual_grid:
            dirs.update((path.replace("\\small", "\\large"), path.replace("\\large", "\\small")))
    return dirs


def group_by_export_dirs(indices, dirs_by_index):
    """Union files that share any export folder; returns groups (lists of indices, original order kept)."""
    parent = {i: i for i in indices}

    def find(i):
        while parent[i] != i:
            parent[i] = parent[parent[i]]
            i = parent[i]
        return i

    owner = {}
    for i in indices:
        for d in dirs_by_index[i]:
            if d in owner:
                parent[find(i)] = find(owner[d])
            else:
                owner[d] = i
    groups = {}
    for i in indices:
        groups.setdefault(find(i), []).append(i)
    return list(groups.values())


class BatchRunner:
    """Launches one Blender worker per .blend and collects its JSON result."""

    def __init__(self, blender, blends, base_dir, work_dir, passthrough, timeout):
        self.blender_ = blender
        self.blends_ = blends
        self.base_dir_ = base_dir
        self.work_dir_ = work_dir
        self.passthrough_ = passthrough
        self.timeout_ = timeout
        self.print_lock_ = threading.Lock()
        self.done_ = 0
        self.total_ = 0

    def display(self, i):
        return os.path.relpath(self.blends_[i], self.base_dir_)

    def run_many(self, indices, dry_run, jobs):
        return self.run_groups([[i] for i in indices], jobs, dry_run)

    def run_groups(self, groups, jobs, dry_run=False):
        """Groups run in parallel (up to jobs); files inside a group run one after another."""
        self.done_ = 0
        self.total_ = sum(len(g) for g in groups)
        results = {}

        def run_group(group):
            return {i: self.run_one(i, dry_run) for i in group}

        with concurrent.futures.ThreadPoolExecutor(max_workers=jobs) as pool:
            for group_results in pool.map(run_group, groups):
                results.update(group_results)
        return results

    def run_one(self, i, dry_run):
        blend = self.blends_[i]
        stem = f"{i + 1:03d}_{os.path.splitext(os.path.basename(blend))[0]}_{'check' if dry_run else 'export'}"
        json_path = os.path.join(self.work_dir_, stem + ".json")
        log_path = os.path.join(self.work_dir_, stem + ".log")
        cmd = [self.blender_, "-b", blend, "--python-exit-code", "3", "-P", os.path.abspath(__file__),
               "--", "--worker-json", json_path] + self.passthrough_ + (["--dry-run"] if dry_run else [])

        entry = {"file": blend, "display": self.display(i), "file_error": "", "scenes": []}
        try:
            proc = subprocess.run(cmd, capture_output=True, text=True, encoding="utf-8",
                                  errors="replace", timeout=self.timeout_)
            output = (proc.stdout or "") + (proc.stderr or "")
            returncode = proc.returncode
        except subprocess.TimeoutExpired as ex:
            output = ex.stdout if isinstance(ex.stdout, str) else (ex.stdout or b"").decode("utf-8", "replace")
            returncode = None
            entry["file_error"] = f"timed out after {self.timeout_}s"

        with open(log_path, "w", encoding="utf-8") as f:
            f.write(output)

        if os.path.isfile(json_path):
            with open(json_path, encoding="utf-8") as f:
                worker = json.load(f)
            entry["scenes"] = worker.get("scenes", [])
            entry["file_error"] = entry["file_error"] or worker.get("file_error", "")
        elif not entry["file_error"]:
            entry["file_error"] = f"Blender exited with code {returncode} without a result"

        failed = bool(entry["file_error"]) or any(s["status"] == STATUS_FAILED for s in entry["scenes"])
        if failed:
            entry["log"] = log_path
            if entry["file_error"]:
                entry["tail"] = output_tail(output)

        counts = {}
        for s in entry["scenes"]:
            counts[s["status"]] = counts.get(s["status"], 0) + 1
        summary = ", ".join(f"{n} {k.lower()}" for k, n in sorted(counts.items()))
        with self.print_lock_:
            self.done_ += 1
            print(f"  [{self.done_}/{self.total_}] {entry['display']} -> {'FAILED' if failed else 'ok'}"
                  + (f" ({summary})" if summary else ""), flush=True)
        return entry


# ---------------------------------------------------------------- report

def print_report(file_results, dry_run):
    """Prints only what was NOT exported. Returns True if anything FAILED."""
    ok_status = STATUS_DRY_RUN_OK if dry_run else STATUS_EXPORTED
    ok_scenes = sum(1 for e in file_results for s in e["scenes"] if s["status"] == ok_status)
    ok_files = sum(1 for e in file_results if any(s["status"] == ok_status for s in e["scenes"]))

    failed, skipped = [], []
    for e in file_results:
        if e["file_error"]:
            line = f"{e['display']} -- {e['file_error']}"
            if e.get("log"):
                line += f"\n        log: {e['log']}"
            if e.get("tail"):
                line += "\n        " + e["tail"].replace("\n", "\n        ")
            failed.append(line)
        for s in e["scenes"]:
            target = f"{e['display']} :: {s['scene']}" if s["scene"] else e["display"]
            if s["status"] == STATUS_FAILED:
                failed.append(f"{target} -- {s['reason']}")
            elif s["status"] == STATUS_SKIPPED:
                skipped.append(f"{target} -- {s['reason']}")

    verb = "Passed dry run" if dry_run else "Exported"
    print("")
    print("=== SEUT EXPORT REPORT ===")
    print(f"{verb}: {ok_scenes} scene(s) in {ok_files} file(s)")
    print(f"Not exported: {len(failed) + len(skipped)}")
    if failed:
        print(f"  FAILED ({len(failed)}):")
        for line in failed:
            print(f"    {line}")
    if skipped:
        print(f"  SKIPPED ({len(skipped)}):")
        for line in skipped:
            print(f"    {line}")
    print("SEUT_EXPORT_FAILED" if failed else ("SEUT_DRY_RUN_OK" if dry_run else "SEUT_EXPORT_OK"))
    return bool(failed)


def main():
    args = parse_args()
    if args.path:
        run_batch(args)
    elif bpy is not None and bpy.data.filepath:
        run_in_file(args)
    else:
        print("SEUT_BATCH_ERROR: open a .blend with Blender (-b file.blend) or pass --path <file_or_dir>")
        sys.exit(1)


if __name__ == "__main__":
    main()
