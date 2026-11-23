"""Periodic audit of the whole Mods folder: every Models\\**\\*.blend (via audit_blend.py) plus .mwm / .sbc
cross-checks. Report-only: it never modifies .blend, .mwm or .sbc files.

Run with Blender's bundled Python (the Windows 'python' alias is blocked in the sandbox); it launches
blender.exe once per .blend, and SEUT's own checks need the sandbox bypass:
  & "C:\\Program Files\\Blender Foundation\\Blender 4.5\\4.5\\python\\bin\\python.exe" `
    "<Mods>\\.agents\\skills\\seut-blend\\scripts\\audit_mod.py" --out "<scratch>\\audit"

Options: --only <substring> (repeatable, filter .blend paths), --jobs 3, --skip-blend (sbc/mwm checks only, using
cached per-blend JSON when present), --reuse (reuse a cached per-blend JSON when the .blend is unchanged), --verbose
(list INFO findings too). Output: <out>\\audit_report.md (read this), <out>\\audit_report.json, <out>\\blend\\*.json.
"""
import argparse, concurrent.futures, glob, json, os, re, subprocess, sys, time
import xml.etree.ElementTree as ET
from collections import defaultdict

HERE = os.path.dirname(os.path.abspath(__file__))
DEFAULT_MODS = os.path.normpath(os.path.join(HERE, "..", "..", "..", ".."))
DEFAULT_BLENDER = r"C:\Program Files\Blender Foundation\Blender 4.5\blender.exe"
DEFAULT_CONTENT = r"E:\SteamLibrary\steamapps\common\SpaceEngineers\Content"
STALE_SECONDS = 120
PARTIAL_EXPORT_SECONDS = 600
DEFAULT_PAIRING = {
    "LargeGatlingTurret": ["GatlingTurretBase1", "GatlingTurretBase2", "GatlingBarrel"],
    "LargeMissileTurret": ["MissileTurretBase1", "MissileTurretBarrels"],
}
SEV_ORDER = {"ERROR": 0, "WARN": 1, "INFO": 2}


def nc(p):
    return os.path.normcase(os.path.normpath(p))


def parse_args():
    p = argparse.ArgumentParser()
    p.add_argument("--mods", default=DEFAULT_MODS)
    p.add_argument("--blender", default=DEFAULT_BLENDER)
    p.add_argument("--content", default=DEFAULT_CONTENT, help="vanilla SpaceEngineers\\Content folder")
    p.add_argument("--out", required=True)
    p.add_argument("--only", action="append", default=[])
    p.add_argument("--jobs", type=int, default=3)
    p.add_argument("--skip-blend", action="store_true")
    p.add_argument("--reuse", action="store_true")
    p.add_argument("--verbose", action="store_true")
    return p.parse_args()


# ------------------------------------------------------------------------------------------ blend audits
def run_blend_audit(a, blend, out_json):
    mtime = os.path.getmtime(blend)
    if (a.reuse or a.skip_blend) and os.path.exists(out_json):
        try:
            with open(out_json, encoding="utf-8") as fh:
                cached = json.load(fh)
            if abs(cached.get("mtime", 0) - mtime) < 1e-3 or a.skip_blend:
                cached["cached"] = True
                return cached
        except Exception:
            pass
    if a.skip_blend:
        return None
    if os.path.exists(out_json):
        os.remove(out_json)                       # so a crash can't leave an older result looking current
    cmd = [a.blender, "-b", blend, "-P", os.path.join(HERE, "audit_blend.py"), "--", "--json", out_json, "--quiet"]
    try:
        r = subprocess.run(cmd, capture_output=True, text=True, timeout=900)
    except subprocess.TimeoutExpired:
        return {"file": blend, "failed": "timeout", "findings": [], "scenes": [], "counts": {}}
    if not os.path.exists(out_json):
        tail = "\n".join((r.stdout + r.stderr).splitlines()[-15:])
        return {"file": blend, "failed": tail, "findings": [], "scenes": [], "counts": {}}
    with open(out_json, encoding="utf-8") as fh:
        return json.load(fh)


# ------------------------------------------------------------------------------------------ sbc parsing
def text(el, tag):
    c = el.find(tag)
    return c.text.strip() if c is not None and c.text else None


def def_id(el):
    idn = el.find("Id")
    if idn is None:
        return None
    t = idn.get("Type") or text(idn, "TypeId")
    s = idn.get("Subtype") if idn.get("Subtype") is not None else (text(idn, "SubtypeId") or "")
    if not t:
        return None
    return (t.replace("MyObjectBuilder_", ""), s or "")


def parse_cubeblocks(path, findings, source):
    try:
        root = ET.parse(path).getroot()
    except Exception as exc:
        if findings is not None:
            findings.append(F("ERROR", "SBC_PARSE", f"cannot parse: {exc}", path))
        return [], set(), set()
    blocks = []
    for cb in root.iter("CubeBlocks"):
        for d in cb.findall("Definition"):
            i = def_id(d)
            if i is None:
                continue
            bpm = d.find("BuildProgressModels")
            pairing = []
            sp = d.find("SubpartPairing")
            if sp is not None:
                for item in sp.iter("item"):
                    k, v = text(item, "Key"), text(item, "Value")
                    if v:
                        pairing.append((k, v))
            blocks.append({
                "id": i, "file": path, "source": source,
                "cube_size": text(d, "CubeSize"), "pair": text(d, "BlockPairName"),
                "model": text(d, "Model"),
                "icons": [ic.text.strip() for ic in d.findall("Icon") if ic.text],
                "bs_models": [m.get("File") for m in bpm.findall("Model")] if bpm is not None else [],
                "pairing": pairing,
                "muzzle": text(d, "MuzzleProjectileDummyName"), "camera": text(d, "CameraDummyName"),
                "size": tuple(float((d.find("Size").get(k) if d.find("Size") is not None else None) or 1)
                              for k in ("x", "y", "z")),
                "model_offset": tuple(float((d.find("ModelOffset").get(k) if d.find("ModelOffset") is not None
                                             else None) or 0) for k in ("x", "y", "z")),
            })
    rblocks, rgroups = set(), set()
    for rb in root.iter("ResearchBlock"):
        i = def_id(rb)
        if i:
            rblocks.add(i)
    for b in root.iter("BlockId"):
        t, s = b.get("Type"), b.get("Subtype")
        if t is not None:
            rgroups.add((t.replace("MyObjectBuilder_", ""), s or ""))
    return blocks, rblocks, rgroups


def F(sev, code, msg, where, obj="", fix=""):
    return {"sev": sev, "code": code, "msg": msg, "where": where, "object": obj, "fix": fix}


def resolve_asset(rel, mod_root, content, other_roots):
    """Find a mod-relative asset (Models\\..., Textures\\...) in this mod, vanilla Content, or another mod."""
    if not rel:
        return None, None
    parts = re.split(r"[\\/]+", rel)
    for where, root in [("mod", mod_root), ("vanilla", content)] + [("other mod", r) for r in other_roots]:
        if root and os.path.exists(os.path.join(root, *parts)):
            return where, os.path.join(root, *parts)
    return None, None


# ------------------------------------------------------------------------------------------ main
def main():
    a = parse_args()
    t0 = time.time()
    os.makedirs(os.path.join(a.out, "blend"), exist_ok=True)
    models_dir = os.path.join(a.mods, "Models")
    blends = sorted(glob.glob(os.path.join(models_dir, "**", "*.blend"), recursive=True))
    if a.only:
        blends = [b for b in blends if any(o.lower() in b.lower() for o in a.only)]
    print(f"AUDIT {len(blends)} .blend file(s), jobs={a.jobs}", flush=True)

    results = {}

    def job(b):
        rel = os.path.relpath(b, models_dir)
        out_json = os.path.join(a.out, "blend", re.sub(r"[\\/]", "__", rel) + ".json")
        return b, run_blend_audit(a, b, out_json)
    with concurrent.futures.ThreadPoolExecutor(max_workers=max(1, a.jobs)) as ex:
        for b, res in ex.map(job, blends):
            results[b] = res
            if res is None:
                print(f"  skip {os.path.relpath(b, models_dir)} (no cached audit)", flush=True)
                continue
            c = res.get("counts", {})
            tag = "FAILED" if res.get("failed") else ("cached" if res.get("cached") else "ok")
            print(f"  {tag:6} {os.path.relpath(b, models_dir):45} E{c.get('ERROR', 0)} W{c.get('WARN', 0)} "
                  f"I{c.get('INFO', 0)}", flush=True)

    findings = []          # cross-check findings
    for b, res in results.items():
        if res and res.get("failed"):
            findings.append(F("ERROR", "AUDIT_FAILED", f"audit_blend.py failed: {res['failed'][-400:]}", b,
                              fix="run audit_blend.py on it directly (sandbox bypass needed for SEUT)"))

    # scene index: expected mwm path -> (blend, scene)
    scene_by_mwm, scenes_by_blend = {}, {}
    for b, res in results.items():
        if not res:
            continue
        idx = {s["name"]: s for s in res.get("scenes", [])}
        scenes_by_blend[b] = idx
        for s in res.get("scenes", []):
            if s.get("export_path") and s.get("subtypeId"):
                scene_by_mwm[nc(os.path.join(s["export_path"], s["subtypeId"] + ".mwm"))] = (b, s)

    def chain(b, s, seen=None):
        seen = seen or set()
        if s["name"] in seen:
            return []
        seen.add(s["name"])
        out = [s]
        for sp in s.get("subparts", []):
            sub = scenes_by_blend.get(b, {}).get(sp.get("linked"))
            if sub:
                out += chain(b, sub, seen)
        return out

    # sbc
    mod_roots = sorted(d for d in glob.glob(os.path.join(a.mods, "*")) if os.path.isdir(os.path.join(d, "Data")))
    mod_blocks, rblocks, rgroups = [], set(), set()
    for root in mod_roots:
        for path in glob.glob(os.path.join(root, "Data", "**", "*.sbc"), recursive=True):
            bl, rb, rg = parse_cubeblocks(path, findings, os.path.basename(root))
            for x in bl:
                x["mod_root"] = root
            mod_blocks += bl
            rblocks |= rb
            rgroups |= rg
    van_blocks, van_rb, van_rg = [], set(), set()
    vdata = os.path.join(a.content, "Data")
    if os.path.isdir(vdata):
        for path in glob.glob(os.path.join(vdata, "**", "*.sbc"), recursive=True):
            name = os.path.basename(path).lower()
            if not (name.startswith("cubeblocks") or name.startswith("research")):
                continue
            bl, rb, rg = parse_cubeblocks(path, None, "vanilla")
            van_blocks += bl
            van_rb |= rb
            van_rg |= rg
    else:
        findings.append(F("WARN", "VANILLA_MISSING", f"vanilla Content not found: {a.content}", a.content,
                          fix="pass --content"))
    vanilla_ids = {x["id"] for x in van_blocks}

    referenced_mwm = set()
    defs_by_id = defaultdict(list)
    for blk in mod_blocks:
        defs_by_id[blk["id"]].append(blk)
        others = [r for r in mod_roots if r != blk["mod_root"]]
        name = f"{blk['id'][0]}/{blk['id'][1]}"
        for kind, rel in [("Model", blk["model"])] + [("BuildProgressModel", f) for f in blk["bs_models"]]:
            if not rel:
                continue
            where, path = resolve_asset(rel, blk["mod_root"], a.content, others)
            if where is None:
                findings.append(F("ERROR", "MODEL_MISSING", f"{kind} not found: {rel}", blk["file"], name,
                                  "export the model or fix the path"))
            else:
                referenced_mwm.add(nc(path))
                if where == "other mod":
                    findings.append(F("WARN", "MODEL_OTHER_MOD", f"{kind} only exists in another mod: {path}",
                                      blk["file"], name))
        for ic in blk["icons"]:
            if resolve_asset(ic, blk["mod_root"], a.content, others)[0] is None:
                findings.append(F("WARN", "ICON_MISSING", f"Icon not found: {ic}", blk["file"], name))
        # link to the .blend that exports this model
        if not blk["model"]:
            continue
        where, mpath = resolve_asset(blk["model"], blk["mod_root"], a.content, [])
        key = nc(mpath) if mpath else nc(os.path.join(blk["mod_root"], *re.split(r"[\\/]+", blk["model"])))
        hit = scene_by_mwm.get(key)
        if not hit:
            continue
        b, s = hit
        ch = chain(b, s)
        empties = {e for sc in ch for e in sc.get("empties", [])}
        for sc in ch:
            if sc.get("export_path") and sc.get("subtypeId"):
                referenced_mwm.add(nc(os.path.join(sc["export_path"], sc["subtypeId"] + ".mwm")))
        if blk["muzzle"] and blk["muzzle"] not in empties:
            findings.append(F("ERROR", "MUZZLE_DUMMY_MISSING",
                              f"MuzzleProjectileDummyName '{blk['muzzle']}' not found in {os.path.basename(b)}",
                              blk["file"], name, "rename the dummy or fix the .sbc"))
        if blk["camera"] and blk["camera"] not in empties:
            findings.append(F("WARN", "CAMERA_DUMMY_MISSING",
                              f"CameraDummyName '{blk['camera']}' not found in {os.path.basename(b)}", blk["file"], name))
        wanted = []
        if blk["pairing"]:
            for _, v in blk["pairing"]:
                wanted += [seg for seg in v.split("/") if seg]
        else:
            wanted = DEFAULT_PAIRING.get(blk["id"][0], [])
        for seg in sorted(set(wanted)):
            if f"subpart_{seg}" not in empties:
                findings.append(F("ERROR", "SUBPART_PAIRING_MISSING",
                                  f"needs empty 'subpart_{seg}' (SubpartPairing/default) in {os.path.basename(b)}",
                                  blk["file"], name, "rename the subpart empty or add <SubpartPairing>"))
        cf = s.get("collision_fit") or {}
        if cf.get("collision_bbox") and blk["cube_size"] in ("Large", "Small") and not any(blk["model_offset"]):
            g = 2.5 if blk["cube_size"] == "Large" else 0.5
            sx, sy, sz = blk["size"]                       # SE Y is up = Blender Z; SE Z = Blender Y
            half = (sx * g / 2, sz * g / 2, sy * g / 2)
            (x0, y0, z0), (x1, y1, z1) = cf["collision_bbox"]
            out = {ax: round(max(abs(lo), abs(hi)) - h, 2) for ax, lo, hi, h in
                   (("X", x0, x1, half[0]), ("Y", y0, y1, half[1]), ("Z", z0, z1, half[2]))
                   if max(abs(lo), abs(hi)) > h + 0.05}
            if out:
                findings.append(F("WARN", "COL_OUTSIDE_BLOCK", f"collision reaches past the {int(sx)}x{int(sy)}x{int(sz)} "
                                  f"block bounds by {out} m", b, s["name"],
                                  "keep collision inside the block's cells (it blocks neighbours)"))
        if blk["bs_models"] and s.get("bs_count") is not None and len(blk["bs_models"]) != s["bs_count"]:
            findings.append(F("INFO", "BS_COUNT_MISMATCH",
                              f"{len(blk['bs_models'])} BuildProgressModels but {s['bs_count']} BS collections in "
                              f"{os.path.basename(b)}:{s['name']}", blk["file"], name))

    # mwm freshness / completeness per blend
    for b, idx in scenes_by_blend.items():
        bm = os.path.getmtime(b)
        for s in idx.values():
            if not s.get("export_path") or not s.get("subtypeId") or s.get("sceneType") not in ("mainScene", "subpart"):
                continue
            mwm = os.path.join(s["export_path"], s["subtypeId"] + ".mwm")
            if not os.path.exists(mwm):
                sev = "WARN" if nc(mwm) in referenced_mwm or s.get("sceneType") == "subpart" else "INFO"
                findings.append(F(sev, "MWM_MISSING", f"{s['name']} never exported ({mwm})", b, s["name"],
                                  "Export All Scenes"))
                continue
            if bm > os.path.getmtime(mwm) + STALE_SECONDS:
                findings.append(F("INFO", "MWM_STALE", f"{s['name']}: .blend edited after last export "
                                  f"({time.strftime('%Y-%m-%d %H:%M', time.localtime(os.path.getmtime(mwm)))})",
                                  b, s["name"], "Export All Scenes when the change is final"))
        for s in idx.values():
            if s.get("sceneType") != "mainScene" or not s.get("export_path"):
                continue
            times = {}
            for sc in chain(b, s):
                p = os.path.join(sc.get("export_path", ""), (sc.get("subtypeId") or "") + ".mwm")
                if os.path.exists(p):
                    times[sc["name"]] = os.path.getmtime(p)
            if len(times) > 1 and max(times.values()) - min(times.values()) > PARTIAL_EXPORT_SECONDS:
                old = min(times, key=times.get)
                findings.append(F("WARN", "MWM_PARTIAL_EXPORT",
                                  f"exports of {s['name']}'s scenes differ by "
                                  f"{(max(times.values()) - min(times.values())) / 60:.0f} min (oldest: {old})",
                                  b, s["name"], "'Export Current Scene' was probably used: Export All Scenes"))

    # visual review status (review_pack.py --mark writes Models\review_log.json)
    import hashlib
    log_path = os.path.join(models_dir, "review_log.json")
    rlog = json.load(open(log_path, encoding="utf-8")) if os.path.exists(log_path) else {}
    for b in blends:
        key = os.path.relpath(b, models_dir).replace("/", "\\")
        e = rlog.get(key)
        h = hashlib.sha256(open(b, "rb").read()).hexdigest()
        if not e:
            findings.append(F("INFO", "REVIEW_NEEDED", "never visually reviewed (collision fit + build stages)", b,
                              fix="review_pack.py --model ... then --mark"))
        elif e.get("status") == "issues":
            findings.append(F("WARN", "REVIEW_OPEN_ISSUES", f"review on {e.get('date')} found issues: {e.get('notes')}", b,
                              fix="fix them, re-run review_pack.py, then --mark ... --status ok"))
        elif e.get("sha256") != h:
            findings.append(F("INFO", "REVIEW_STALE", f"changed since its visual review on {e.get('date')}", b,
                              fix="review_pack.py --needs-review"))

    # stray / unreferenced mwm files in mod Models folders
    for root in mod_roots:
        for p in glob.glob(os.path.join(root, "Models", "**", "*.mwm"), recursive=True):
            base = os.path.basename(p)
            if re.match(r"^(LG|SG)_", base):
                findings.append(F("WARN", "MWM_DUAL_GRID_COPY", f"dual-grid export leftover {base}", p,
                                  fix="turn off the wrong export_*Grid flag and delete the LG_/SG_ file"))
            elif nc(p) not in referenced_mwm and not re.search(r"_(BS\d+|LOD\d+)\.mwm$", base, re.I):
                findings.append(F("INFO", "MWM_UNREFERENCED", f"not referenced by any .sbc or turret chain: {base}", p))

    # block pair / duplicate definitions / research
    pairs = defaultdict(set)
    mod_ids = {blk["id"] for blk in mod_blocks}
    effective_vanilla = [blk for blk in van_blocks if blk["id"] not in mod_ids]   # mod overrides replace vanilla
    for blk in mod_blocks + effective_vanilla:
        if blk["cube_size"] and blk["pair"]:
            pairs[(blk["cube_size"], blk["pair"])].add((blk["id"], blk["source"]))
    for (size, pair), ids in pairs.items():
        distinct = {i for i, _ in ids}
        if len(distinct) > 1 and any(src != "vanilla" for _, src in ids):
            mine = sorted({f"{i[0]}/{i[1]}" for i, src in ids if src != "vanilla"})
            others = sorted({f"{i[0]}/{i[1]}" for i in distinct} - set(mine))
            findings.append(F("WARN", "PAIR_CONFLICT", f"CubeSize={size} BlockPairName='{pair}' shared by "
                              f"{mine} and {others}", "sbc", pair,
                              "BlockPairName+CubeSize must be unique (AGENTS.md)"))
    for i, defs in defs_by_id.items():
        files = sorted({d["file"] for d in defs})
        if len(files) > 1:
            findings.append(F("WARN", "DEF_DUPLICATE", f"{i[0]}/{i[1]} defined in {len(files)} files: "
                              + "; ".join(os.path.relpath(x, a.mods) for x in files), files[0], f"{i[0]}/{i[1]}"))
        if i in vanilla_ids:
            continue                                    # override of a vanilla block
        if i not in rblocks and i not in van_rb:
            findings.append(F("WARN", "RESEARCH_BLOCK_MISSING", f"{i[0]}/{i[1]} has no ResearchBlocks entry",
                              defs[0]["file"], f"{i[0]}/{i[1]}", "add to the mod's ResearchBlocks.sbc"))
        if i not in rgroups and i not in van_rg:
            findings.append(F("INFO", "RESEARCH_GROUP_NONMEMBER",
                              f"{i[0]}/{i[1]} is in no ResearchGroup (building it unlocks nothing; normal for "
                              "decorative blocks, e.g. vanilla corridors)", defs[0]["file"], f"{i[0]}/{i[1]}",
                              "only add membership if building it should unlock other blocks; a mod ResearchGroup "
                              "replaces the whole vanilla group, so copy all vanilla members"))

    # ------------------------------------------------------------------ report
    blend_findings = []
    accepted = {}                       # blend -> review date, when a visual review marked this exact file 'ok'
    for b in blends:
        e = rlog.get(os.path.relpath(b, models_dir).replace("/", "\\"))
        if e and e.get("status") == "ok" and e.get("sha256") == hashlib.sha256(open(b, "rb").read()).hexdigest():
            accepted[b] = e.get("date")
    for b, res in results.items():
        for f in (res or {}).get("findings", []):
            if b in accepted and f["sev"] == "WARN" and f["code"].startswith(("COL_", "BS_")):
                f = dict(f, sev="INFO", msg=f"(accepted in review {accepted[b]}) " + f["msg"])
            blend_findings.append({"sev": f["sev"], "code": f["code"], "msg": f["msg"], "where": b,
                                   "object": (f.get("scene", "") + " | " + f.get("object", "")).strip(" |"),
                                   "fix": f.get("fix", "")})
    allf = blend_findings + findings
    counts = {k: sum(1 for f in allf if f["sev"] == k) for k in SEV_ORDER}
    report = {"generated": time.strftime("%Y-%m-%d %H:%M"), "mods": a.mods, "counts": counts,
              "blends": {b: (r or {}).get("counts") for b, r in results.items()}, "findings": allf}
    with open(os.path.join(a.out, "audit_report.json"), "w", encoding="utf-8") as fh:
        json.dump(report, fh, indent=1)

    def short(p):
        try:
            return os.path.relpath(p, a.mods)
        except ValueError:
            return p
    lines = [f"# Mod audit {report['generated']}", "",
             f"**{counts['ERROR']} errors, {counts['WARN']} warnings, {counts['INFO']} info** across "
             f"{len(blends)} .blend files and {len(mod_blocks)} block definitions in {len(mod_roots)} mods "
             f"({time.time() - t0:.0f}s).", "", "## Per model", "", "| Model | E | W | I |", "|---|---|---|---|"]
    per_where = defaultdict(lambda: defaultdict(int))
    for f in allf:
        per_where[f["where"]][f["sev"]] += 1
    for b in blends:
        c = per_where.get(b, {})
        lines.append(f"| {short(b)} | {c.get('ERROR', 0)} | {c.get('WARN', 0)} | {c.get('INFO', 0)} |")
    for sev in ("ERROR", "WARN") + (("INFO",) if a.verbose else ()):
        group = defaultdict(list)
        for f in allf:
            if f["sev"] == sev:
                group[(f["where"], f["code"])].append(f)
        lines += ["", f"## {sev} ({counts[sev]})", ""]
        for (where, code), fs in sorted(group.items()):
            fix = fs[0]["fix"]
            lines.append(f"- **{code}** `{short(where)}` ({len(fs)}){': ' + fix if fix else ''}")
            for f in fs[:6]:
                lines.append(f"  - {f['object'] + ': ' if f['object'] else ''}{f['msg']}")
            if len(fs) > 6:
                lines.append(f"  - ... {len(fs) - 6} more")
    if not a.verbose:
        codes = defaultdict(int)
        for f in allf:
            if f["sev"] == "INFO":
                codes[f["code"]] += 1
        lines += ["", f"## INFO ({counts['INFO']}, use --verbose to list)", "",
                  ", ".join(f"{k} {v}" for k, v in sorted(codes.items(), key=lambda kv: -kv[1]))]
    md = os.path.join(a.out, "audit_report.md")
    with open(md, "w", encoding="utf-8") as fh:
        fh.write("\n".join(lines) + "\n")
    print(f"AUDIT_DONE errors={counts['ERROR']} warnings={counts['WARN']} info={counts['INFO']} report={md}")


if __name__ == "__main__":
    main()
