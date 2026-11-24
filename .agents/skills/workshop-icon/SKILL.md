---
name: workshop-icon
description: >-
  Creates 348x348 PNG Steam Workshop thumbnails (Thumb.png) for Space Engineers
  mods by automatically detecting and removing corner watermarks (such as the
  Gemini 4-pointed diamond watermark), downscaling cleanly to 348x348, and
  saving as PNG. Use whenever the user asks to turn an image into a mod icon,
  workshop icon, or Thumb.png, remove a corner watermark from a mod icon, or
  resize/convert a mod thumbnail.
---

# Workshop Icon Workflow (`348x348` PNG)

Space Engineers Steam Workshop thumbnails (`Thumb.png`) must always be:
- **Format:** PNG (`.png`)
- **Dimensions:** `348x348` pixels
- **Clean:** No AI/generator corner watermark (e.g., Gemini bottom-right diamond star)

## Helper Script

Use [`scripts/make_workshop_icon.ps1`](./scripts/make_workshop_icon.ps1) (pure Windows PowerShell + .NET `System.Drawing`; no Python or external packages needed).

### Parameters
- `-InputPath <path>` *(required)* — Source image (`.jpg`, `.jpeg`, `.png`, `.bmp`, etc.).
- `-OutputPath <path>` *(optional)* — Output PNG path. Defaults to `Thumb.png` in the same folder as `-InputPath`.
- `-Size <int>` *(optional, default `348`)* — Square output size in pixels. Keep `348` for Steam Workshop icons.
- `-DiagnosticPath <path>` *(optional)* — Saves a 3-panel full-resolution corner comparison PNG (`Original | Mask | Repaired`) for visual verification.
- `-Corner <BottomRight|BottomLeft|TopRight|TopLeft>` *(optional, default `BottomRight`)* — Corner to scan for the watermark.
- `-WatermarkMode <Auto|Mirror|Inpaint|None>` *(optional, default `Auto`)* — How to fill the detected watermark mask:
  - `Auto`: Aligns and mirrors the horizontally opposite corner when symmetry matches (`avgErr <= 45`, ideal for the symmetric rounded metallic mod-icon frames); otherwise falls back to harmonic boundary inpainting.
  - `Mirror`: Forces horizontally mirrored corner patching.
  - `Inpaint`: Forces harmonic diffusion inpainting from surrounding pixels (use for asymmetric corners where mirrored content differs).
  - `None`: Skips watermark removal (only use if the source has no watermark).

## Recommended Steps

1. **Generate in scratch (sandboxed) with a diagnostic strip:**
   ```powershell
   powershell -NoProfile -ExecutionPolicy Bypass -File "C:\Users\joshe\AppData\Roaming\SpaceEngineers\Mods\.agents\skills\workshop-icon\scripts\make_workshop_icon.ps1" -InputPath "<ModFolder>\<source_image>" -OutputPath "<scratch>\Thumb.png" -DiagnosticPath "<scratch>\corner_check.png"
   ```
   Check stdout for:
   - `WATERMARK: detected=True ...` (if `detected=False`, inspect the source image—either it has no watermark or it sits in another `-Corner`).
   - `OUTPUT: ... (348x348, PNG, ... bytes)`.

2. **Visually inspect both outputs with `read_file`:**
   - Read `<scratch>\corner_check.png` (confirm the mask tightly covers the diamond watermark and the repaired corner is seamless with no cut-off background lines or grey halo).
   - Read `<scratch>\Thumb.png` (confirm framing, sharpness, and clean corners).
   - If the corner is asymmetric and mirror patching introduced mismatched background art, re-run with `-WatermarkMode Inpaint`.

3. **Copy to the mod folder as `Thumb.png`:**
   ```powershell
   Copy-Item -Path "<scratch>\Thumb.png" -Destination "<ModFolder>\Thumb.png" -Force
   ```
   > **Sandbox note:** Writing directly into a mod folder under `Mods\` may fail in the sandbox with `UnauthorizedAccessException` (`Access to the path ... is denied`). Try `Copy-Item` sandboxed first; if denied, retry only `Copy-Item` with `BypassSandbox: true` using the exact same `toolSummary`.

4. **Verify the final file:**
   Read `<ModFolder>\Thumb.png` with `read_file` (or check dimensions/PNG format) to confirm it landed cleanly in the target folder.
