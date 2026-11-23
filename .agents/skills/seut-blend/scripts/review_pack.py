"""Visual review pack for the checks that need eyes: collision fit and build-stage look, per model.

Run with Blender's bundled Python (like audit_mod.py):
  PY="C:\\Program Files\\Blender Foundation\\Blender 4.5\\4.5\\python\\bin\\python.exe"
  & $PY review_pack.py --model AutocannonTurret --model large\\Gatling --out "<scratch>\\review"   # specific models
  & $PY review_pack.py --needs-review --out "<scratch>\\review"                                    # never reviewed / changed
  & $PY review_pack.py --mark large\\AutocannonTurret --status ok --notes "collision trimmed, BS ok"  # log the verdict

Per model it writes <out>\\<model>\\:
  stages_front.png / stages_back.png   [Main | BS1 | BS2 | BS3 | COL] rows for every scene (render_stages.py)
  <scene>_collision.png                assembled block with the collision in amber (2x2)
  <scene>_final_bs.png / _bottom.png   last build stage alone, top and underside (holes show up here)
  review.md                            audit numbers (coverage, overreach, per-object BS match) + checklist
The review log is Models\\review_log.json: {model: {sha256, date, status, notes}}. audit_mod.py reports models
that were never reviewed, changed since their review, or have open issues.
Read every PNG and go through the checklist in review.md (SKILL.md section "Manual review") before marking.
"""
import argparse, concurrent.futures, glob, hashlib, json, os, re, subprocess, sys, time

HERE = os.path.dirname(os.path.abspath(__file__))
MODS = os.path.normpath(os.path.join(HERE, "..", "..", "..", ".."))
MODELS = os.path.join(MODS, "Models")
LOG = os.path.join(MODELS, "review_log.json")
BLENDER = r"C:\Program Files\Blender Foundation\Blender 4.5\blender.exe"

CHECKLIST = """## Checklist (answer every line from the images, then --mark)
Collision (`*_collision.png`, amber = collision):
- [ ] every solid part of the base is inside amber (no gaps you could walk or shoot through)
- [ ] no amber where there is no model: no invisible walls/pillars above or beside it
- [ ] amber stays inside the block's grid box (doesn't block neighbouring cells)
- [ ] turrets: pedestal box + a round prism over the head's swept volume, up to the head top at rest (barrel outside)
Build stages (`stages_front/back.png`, `*_final_bs*.png`):
- [ ] stages progress sensibly: BS1 frame -> BS2 solid core -> BS3 full shape minus details
- [ ] every stage's silhouette fits the finished model (no parts of an old design, nothing sticking out)
- [ ] all stage geometry is Construction grey (no painted/textured parts in early stages)
- [ ] no see-through holes or missing faces (check back and bottom views)
- [ ] turrets: the head in each stage sits where the real head is at rest, facing the same way
"""


def sha256(p):
    h = hashlib.sha256()
    with open(p, "rb") as fh:
        for chunk in iter(lambda: fh.read(1 << 20), b""):
            h.update(chunk)
    return h.hexdigest()


def load_log():
    if os.path.exists(LOG):
        with open(LOG, encoding="utf-8") as fh:
            return json.load(fh)
    return {}


def rel(b):
    return os.path.relpath(b, MODELS).replace("/", "\\")


def find_blends(subs):
    blends = sorted(glob.glob(os.path.join(MODELS, "**", "*.blend"), recursive=True))
    out = []
    for s in subs:
        hit = [b for b in blends if s.lower().replace("/", "\\") in rel(b).lower()]
        if not hit:
            print("NO MATCH for", s)
        out += [b for b in hit if b not in out]
    return out


def run(cmd, log):
    r = subprocess.run(cmd, capture_output=True, text=True, timeout=1800)
    if r.returncode != 0 or "Traceback" in r.stdout + r.stderr:
        log.append(" ".join(os.path.basename(c) for c in cmd[:4]) + " FAILED:\n" +
                   "\n".join((r.stdout + r.stderr).splitlines()[-8:]))
    return r


def review(a, b):
    name = re.sub(r"[\\/]", "__", os.path.splitext(rel(b))[0])
    out = os.path.join(a.out, name)
    os.makedirs(out, exist_ok=True)
    errs = []
    aj = os.path.join(out, "audit.json")
    run([a.blender, "-b", b, "-P", os.path.join(HERE, "audit_blend.py"), "--", "--json", aj, "--quiet"], errs)
    audit = json.load(open(aj, encoding="utf-8")) if os.path.exists(aj) else {"scenes": [], "findings": []}
    jobs = [[a.blender, "-b", b, "-P", os.path.join(HERE, "render_stages.py"), "--", "--output",
             os.path.join(out, "stages_front.png"), "--size", "300", "--samples", "10"],
            [a.blender, "-b", b, "-P", os.path.join(HERE, "render_stages.py"), "--", "--output",
             os.path.join(out, "stages_back.png"), "--size", "300", "--samples", "10", "--back"]]
    mains = [s for s in audit["scenes"] if s.get("sceneType") == "mainScene"]
    for s in mains:
        safe = re.sub(r"[^A-Za-z0-9_-]", "_", s["name"])
        jobs.append([a.blender, "-b", b, "-P", os.path.join(HERE, "render_blend.py"), "--", "--scene", s["name"],
                     "--assemble", "--show-collision", "--no-empties", "--samples", "12", "--size", "480",
                     "--output", os.path.join(out, f"{safe}_collision.png")])
        bsf = s.get("bs_fit") or []
        if bsf:
            last = bsf[-1]["collection"]
            for extra, tag in (([], "final_bs"), (["--bottom"], "final_bs_bottom")):
                jobs.append([a.blender, "-b", b, "-P", os.path.join(HERE, "render_blend.py"), "--", "--scene",
                             s["name"], "--collection", last, "--no-empties", "--samples", "12", "--size", "480",
                             "--output", os.path.join(out, f"{safe}_{tag}.png")] + extra)
    for j in jobs:                 # sequential: parallel renders of one file fight over temp files
        run(j, errs)

    md = [f"# Review: {rel(b)}", "", f"sha256 `{sha256(b)[:16]}`, generated {time.strftime('%Y-%m-%d %H:%M')}", ""]
    for s in mains:
        cf = s.get("collision_fit") or {}
        md += [f"## Scene {s['name']}", ""]
        if cf.get("objects") or cf.get("external_hkt"):
            md.append(f"- collision: {cf.get('objects', 0)} object(s){', external .hkt' if cf.get('external_hkt') else ''}"
                      + (f", covers {100 * cf['coverage']:.0f}% of the visible block" if "coverage" in cf else ""))
            if cf.get("overreach"):
                big = {k: v for k, v in cf["overreach"].items() if v > 0.05}
                md.append(f"- collision past the assembled model (m): {big or 'none'}")
        else:
            md.append("- collision: **none**")
        for row in s.get("bs_fit") or []:
            objs = ", ".join(f"{k} {100 * v['match']:.0f}%" for k, v in row["objects"].items())
            cov = f", reaches {100 * row['covers_model']:.0f}% of the model" if "covers_model" in row else ""
            md.append(f"- {row['collection']}: {row['faces']} tris{cov}; on/inside model: {objs}")
        md.append("")
    fits = [f for f in audit.get("findings", []) if f["code"].startswith(("COL_", "BS_"))]
    if fits:
        md += ["## Audit findings", ""] + [f"- {f['sev']} {f['code']} {f['scene']} {f['object']}: {f['msg']}" for f in fits] + [""]
    md += ["## Images", ""] + [f"- `{os.path.join(out, p)}`" for p in sorted(os.listdir(out)) if p.endswith(".png")] + [""]
    md += [CHECKLIST]
    if errs:
        md += ["## Render problems", ""] + [f"```\n{e}\n```" for e in errs]
    with open(os.path.join(out, "review.md"), "w", encoding="utf-8") as fh:
        fh.write("\n".join(md) + "\n")
    return out, len(errs)


def main():
    p = argparse.ArgumentParser()
    p.add_argument("--model", action="append", default=[], help="substring of Models\\... path (repeatable)")
    p.add_argument("--needs-review", action="store_true", help="every model never reviewed or changed since review")
    p.add_argument("--out", default=None)
    p.add_argument("--blender", default=BLENDER)
    p.add_argument("--jobs", type=int, default=2, help="models rendered in parallel")
    p.add_argument("--mark", default=None, help="record a review verdict for this model (substring)")
    p.add_argument("--status", choices=["ok", "issues"], default="ok")
    p.add_argument("--notes", default="")
    a = p.parse_args()

    log = load_log()
    if a.mark:
        hits = find_blends([a.mark])
        assert len(hits) == 1, f"--mark must match exactly one model, got {[rel(h) for h in hits]}"
        b = hits[0]
        log[rel(b)] = {"sha256": sha256(b), "date": time.strftime("%Y-%m-%d"), "status": a.status, "notes": a.notes}
        with open(LOG, "w", encoding="utf-8") as fh:
            json.dump(dict(sorted(log.items())), fh, indent=1)
        print(f"MARKED {rel(b)} {a.status} -> {LOG}")
        return
    assert a.out, "--out is required"
    blends = find_blends(a.model)
    if a.needs_review:
        for b in sorted(glob.glob(os.path.join(MODELS, "**", "*.blend"), recursive=True)):
            e = log.get(rel(b))
            if (not e or e.get("sha256") != sha256(b) or e.get("status") == "issues") and b not in blends:
                blends.append(b)
    print(f"REVIEW {len(blends)} model(s)")
    with concurrent.futures.ThreadPoolExecutor(max_workers=max(1, a.jobs)) as ex:
        for out, nerr in ex.map(lambda b: review(a, b), blends):
            print(f"  {'ok' if not nerr else f'{nerr} render problem(s)':22} {os.path.join(out, 'review.md')}", flush=True)


if __name__ == "__main__":
    main()
