"""Save a scratch working copy back over the real .blend, remapping relative paths for the new location.

Usage:
  blender -b <scratch_copy.blend> -P writeback_blend.py -- "<Mods\\Models\\...\\Target.blend>" \
      [--expect-sha256 HEX] [--backup-to "<scratch>\\work"]

Why: a copy edited in <scratch> has its relative library paths (e.g. the indirect SEUT.blend link) rewritten
for the scratch folder. save_as_mainfile(relative_remap=True) re-derives them for the target folder.
SEUT's scn.seut.export_exportPath / mod_path are plain strings and are NOT remapped, so they are only
correct when writing back to the folder the file originally came from (printed below for checking).
--expect-sha256 refuses to overwrite if the target changed since you backed it up (user saved in between).
--backup-to DIR copies the CURRENT target to DIR/<stem>_latest_backup.blend (overwriting the previous rolling
  backup) before saving, so you don't need a separate copy step per iteration.
After saving it prints NEWSHA256 <hash>: pass that as --expect-sha256 on the next write-back of the same file
(the saved file's hash never equals the scratch copy's hash, because paths are remapped on save).
Also check from PowerShell that no Blender window has the target open:
  Get-Process blender | ? { $_.MainWindowTitle -match '<Target>' }
"""
import bpy, sys, os, hashlib, argparse, shutil


def sha256(path):
    h = hashlib.sha256()
    with open(path, "rb") as fh:
        for chunk in iter(lambda: fh.read(1 << 20), b""):
            h.update(chunk)
    return h.hexdigest().upper()


def main():
    argv = sys.argv[sys.argv.index("--") + 1:]
    p = argparse.ArgumentParser()
    p.add_argument("target")
    p.add_argument("--expect-sha256", default=None)
    p.add_argument("--backup-to", default=None, help="folder for a rolling <stem>_latest_backup.blend of the target")
    a = p.parse_args(argv)
    target = os.path.abspath(a.target)
    if a.expect_sha256 and os.path.exists(target):
        h = sha256(target)
        if h != a.expect_sha256.upper():
            raise SystemExit(f"REFUSED: {target} changed since backup (sha256 {h})")
    missing = [l.filepath for l in bpy.data.libraries
               if not os.path.exists(bpy.path.abspath(l.filepath, library=l.library))]
    if missing:
        raise SystemExit(f"REFUSED: missing libraries {missing}")
    if a.backup_to and os.path.exists(target):
        os.makedirs(a.backup_to, exist_ok=True)
        stem = os.path.splitext(os.path.basename(target))[0]
        bak = os.path.join(os.path.abspath(a.backup_to), f"{stem}_latest_backup.blend")
        shutil.copy2(target, bak)
        print("BACKUP", bak, sha256(bak))
    bpy.ops.wm.save_as_mainfile(filepath=target, relative_remap=True)
    for l in bpy.data.libraries:
        print("LIB", l.filepath, os.path.exists(bpy.path.abspath(l.filepath, library=l.library)))
    for s in bpy.data.scenes:
        if hasattr(s, "seut"):
            print("EXPORTPATH", s.name, s.seut.export_exportPath)
    print("SAVEDTO", bpy.data.filepath)
    print("NEWSHA256", sha256(target))


main()
