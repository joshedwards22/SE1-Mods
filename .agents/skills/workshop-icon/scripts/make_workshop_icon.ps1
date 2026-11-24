<#
.SYNOPSIS
    Removes a corner watermark (e.g. Gemini 4-pointed diamond star), downscales
    an image to 348x348, and saves it as a PNG workshop icon (Thumb.png).

.EXAMPLE
    powershell -NoProfile -ExecutionPolicy Bypass -File make_workshop_icon.ps1 `
        -InputPath "JS - Lighting Overhaul\source.jpg" `
        -OutputPath "scratch\Thumb.png" `
        -DiagnosticPath "scratch\corner_check.png"
#>
param(
    [Parameter(Mandatory = $true)]
    [string]$InputPath,

    [Parameter(Mandatory = $false)]
    [string]$OutputPath = "",

    [Parameter(Mandatory = $false)]
    [int]$Size = 348,

    [Parameter(Mandatory = $false)]
    [string]$DiagnosticPath = "",

    [Parameter(Mandatory = $false)]
    [ValidateSet("BottomRight", "BottomLeft", "TopRight", "TopLeft")]
    [string]$Corner = "BottomRight",

    [Parameter(Mandatory = $false)]
    [ValidateSet("Auto", "Mirror", "Inpaint", "None")]
    [string]$WatermarkMode = "Auto"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$resolvedInput = (Resolve-Path -LiteralPath $InputPath).Path
if ([string]::IsNullOrWhiteSpace($OutputPath)) {
    $OutputPath = Join-Path (Split-Path -Parent $resolvedInput) "Thumb.png"
}

function BoxBlur2D([double[,]]$src, [int]$w, [int]$h, [int]$rad) {
    $tmp = New-Object 'double[,]' $w, $h
    $dst = New-Object 'double[,]' $w, $h
    $inv = 1.0 / (2 * $rad + 1)
    for ($j = 0; $j -lt $h; $j++) {
        for ($i = 0; $i -lt $w; $i++) {
            $s = 0.0
            for ($k = -$rad; $k -le $rad; $k++) {
                $ii = [Math]::Max(0, [Math]::Min($w - 1, $i + $k))
                $s += $src[$ii,$j]
            }
            $tmp[$i,$j] = $s * $inv
        }
    }
    for ($j = 0; $j -lt $h; $j++) {
        for ($i = 0; $i -lt $w; $i++) {
            $s = 0.0
            for ($k = -$rad; $k -le $rad; $k++) {
                $jj = [Math]::Max(0, [Math]::Min($h - 1, $j + $k))
                $s += $tmp[$i,$jj]
            }
            $dst[$i,$j] = $s * $inv
        }
    }
    return ,$dst
}

function HasTransparency([System.Drawing.Bitmap]$bitmap) {
    if (($bitmap.PixelFormat -band [System.Drawing.Imaging.PixelFormat]::Alpha) -eq 0) {
        return $false
    }
    $stepX = [Math]::Max(1, [int]($bitmap.Width / 64))
    $stepY = [Math]::Max(1, [int]($bitmap.Height / 64))
    for ($y = 0; $y -lt $bitmap.Height; $y += $stepY) {
        for ($x = 0; $x -lt $bitmap.Width; $x += $stepX) {
            if ($bitmap.GetPixel($x, $y).A -lt 255) { return $true }
        }
    }
    return $false
}

$srcBmp = New-Object System.Drawing.Bitmap($resolvedInput)
try {
    $origW = $srcBmp.Width
    $origH = $srcBmp.Height
    Write-Output ("INPUT: {0} ({1}x{2})" -f $resolvedInput, $origW, $origH)

    # Center-crop to square if input is non-square so icons never stretch
    $workBmp = $null
    if ($origW -ne $origH) {
        $side = [Math]::Min($origW, $origH)
        $cropX = [int](($origW - $side) / 2)
        $cropY = [int](($origH - $side) / 2)
        $cropRect = New-Object System.Drawing.Rectangle($cropX, $cropY, $side, $side)
        $workBmp = $srcBmp.Clone($cropRect, $srcBmp.PixelFormat)
        Write-Output ("CROP: Non-square input center-cropped to {0}x{0} at ({1},{2})" -f $side, $cropX, $cropY)
    } else {
        $workBmp = New-Object System.Drawing.Bitmap($srcBmp)
    }

    try {
        $W = $workBmp.Width
        $H = $workBmp.Height
        $repaired = New-Object System.Drawing.Bitmap($workBmp)

        try {
            # Proportional corner search geometry (scales cleanly for 348, 512, 1024, 2048, 4096, etc.)
            $diagSize = [Math]::Max(64, [Math]::Min($W, [int][Math]::Round($W * 0.1465))) # ~300px at 2048
            $roiSize  = [Math]::Max(24, [Math]::Min($W, [int][Math]::Round($W * 0.095)))  # ~195px at 2048
            $edgePad  = [Math]::Max(2, [int][Math]::Round($W * 0.005))                    # ~10px at 2048

            $isRight  = ($Corner -eq "BottomRight" -or $Corner -eq "TopRight")
            $isBottom = ($Corner -eq "BottomRight" -or $Corner -eq "BottomLeft")

            $rx0 = if ($isRight)  { $W - $roiSize } else { 0 }
            $ry0 = if ($isBottom) { $H - $roiSize } else { 0 }
            $rw  = $roiSize - $edgePad
            $rh  = $roiSize - $edgePad
            if (-not $isRight)  { $rx0 = $edgePad }
            if (-not $isBottom) { $ry0 = $edgePad }

            $mask = New-Object 'double[,]' $rw, $rh
            $bestDx = 0
            $bestDy = 0
            $bestErr = 0.0
            $detected = $false
            $usedMode = "None"
            $wmPixels = 0
            $minX = $W; $maxX = 0; $minY = $H; $maxY = 0

            if ($WatermarkMode -ne "None") {
                # 1. Estimate optimal horizontal-mirror alignment (dx, dy) in ring outside watermark core
                $alignRange = [Math]::Max(3, [Math]::Min(8, [int][Math]::Round($W * 0.004)))
                $ringSize   = [Math]::Min($W - 10, [int][Math]::Round($W * 0.14))
                $exclOuter  = [int][Math]::Round($W * 0.105)
                $exclInner  = [int][Math]::Round($W * 0.015)
                $sampleStep = [Math]::Max(2, [int][Math]::Round($W / 512.0))

                $xStart = if ($isRight)  { $W - $ringSize } else { 10 }
                $xEnd   = if ($isRight)  { $W - 10 } else { $ringSize }
                $yStart = if ($isBottom) { $H - $ringSize } else { 10 }
                $yEnd   = if ($isBottom) { $H - 10 } else { $ringSize }

                $bestErr = [double]::MaxValue
                for ($dy = -$alignRange; $dy -le $alignRange; $dy++) {
                    for ($dx = -$alignRange; $dx -le $alignRange; $dx++) {
                        $err = 0.0
                        $cnt = 0
                        for ($y = $yStart; $y -lt $yEnd; $y += $sampleStep) {
                            $distY = if ($isBottom) { $H - $y } else { $y }
                            for ($x = $xStart; $x -lt $xEnd; $x += $sampleStep) {
                                $distX = if ($isRight) { $W - $x } else { $x }
                                if ($distX -ge $exclInner -and $distX -le $exclOuter -and $distY -ge $exclInner -and $distY -le $exclOuter) {
                                    continue
                                }
                                $sx = ($W - 1 - $x) + $dx
                                $sy = $y + $dy
                                if ($sx -lt 0 -or $sx -ge $W -or $sy -lt 0 -or $sy -ge $H) { continue }
                                $c1 = $workBmp.GetPixel($x, $y)
                                $c2 = $workBmp.GetPixel($sx, $sy)
                                $err += [Math]::Abs($c1.R - $c2.R) + [Math]::Abs($c1.G - $c2.G) + [Math]::Abs($c1.B - $c2.B)
                                $cnt++
                            }
                        }
                        if ($cnt -gt 0) {
                            $avg = $err / $cnt
                            if ($avg -lt $bestErr) {
                                $bestErr = $avg
                                $bestDx = $dx
                                $bestDy = $dy
                            }
                        }
                    }
                }
                Write-Output ("ALIGNMENT: dx={0} dy={1} avgErr={2:F2}" -f $bestDx, $bestDy, $bestErr)

                # 2. Detect bright watermark via hysteresis thresholding (high seed + low grow)
                #    against horizontally mirrored corner, then keep connected components from strong seeds.
                $diffMap = New-Object 'int[,]' $rw, $rh
                $visited = New-Object 'bool[,]' $rw, $rh
                $rawMask = New-Object 'double[,]' $rw, $rh
                $queue = New-Object 'System.Collections.Generic.Queue[int]'

                $seedThresh = 65
                $growThresh = 22

                for ($j = 0; $j -lt $rh; $j++) {
                    $y = $ry0 + $j
                    $sy = [Math]::Max(0, [Math]::Min($H - 1, $y + $bestDy))
                    for ($i = 0; $i -lt $rw; $i++) {
                        $x = $rx0 + $i
                        $sx = [Math]::Max(0, [Math]::Min($W - 1, ($W - 1 - $x) + $bestDx))
                        $c1 = $workBmp.GetPixel($x, $y)
                        $c2 = $workBmp.GetPixel($sx, $sy)
                        $dr = $c1.R - $c2.R
                        $dg = $c1.G - $c2.G
                        $db = $c1.B - $c2.B
                        # Require brightening across all 3 RGB channels (white/grey watermark)
                        if ($dr -gt 0 -and $dg -gt 0 -and $db -gt 0) {
                            $diffMap[$i,$j] = $dr + $dg + $db
                        } else {
                            $diffMap[$i,$j] = 0
                        }
                    }
                }

                # Find connected components seeded by diff > seedThresh; keep the largest component
                $bestComp = $null
                $bestCompSize = 0
                for ($j = 1; $j -lt ($rh - 1); $j++) {
                    for ($i = 1; $i -lt ($rw - 1); $i++) {
                        if (-not $visited[$i,$j] -and $diffMap[$i,$j] -ge $seedThresh) {
                            $comp = New-Object 'System.Collections.Generic.List[int]'
                            $visited[$i,$j] = $true
                            $queue.Enqueue($j * $rw + $i)
                            while ($queue.Count -gt 0) {
                                $idx = $queue.Dequeue()
                                $comp.Add($idx)
                                $ci = $idx % $rw
                                $cj = [int](($idx - $ci) / $rw)
                                for ($dj = -1; $dj -le 1; $dj++) {
                                    $nj = $cj + $dj
                                    if ($nj -lt 0 -or $nj -ge $rh) { continue }
                                    for ($di = -1; $di -le 1; $di++) {
                                        if ($di -eq 0 -and $dj -eq 0) { continue }
                                        $ni = $ci + $di
                                        if ($ni -lt 0 -or $ni -ge $rw) { continue }
                                        if (-not $visited[$ni,$nj] -and $diffMap[$ni,$nj] -ge $growThresh) {
                                            $visited[$ni,$nj] = $true
                                            $queue.Enqueue($nj * $rw + $ni)
                                        }
                                    }
                                }
                            }
                            if ($comp.Count -gt $bestCompSize) {
                                $bestCompSize = $comp.Count
                                $bestComp = $comp
                            }
                        }
                    }
                }

                $minPixels = [Math]::Max(25, [int][Math]::Round(($W / 2048.0) * ($W / 2048.0) * 400))
                if ($null -ne $bestComp -and $bestCompSize -ge $minPixels) {
                    $detected = $true
                    $wmPixels = $bestCompSize
                    foreach ($idx in $bestComp) {
                        $ci = $idx % $rw
                        $cj = [int](($idx - $ci) / $rw)
                        $rawMask[$ci,$cj] = 1.0
                        $wx = $rx0 + $ci
                        $wy = $ry0 + $cj
                        if ($wx -lt $minX) { $minX = $wx }
                        if ($wx -gt $maxX) { $maxX = $wx }
                        if ($wy -lt $minY) { $minY = $wy }
                        if ($wy -gt $maxY) { $maxY = $wy }
                    }

                    # 3. Dilate mask to capture soft anti-aliased edge / halo, then smooth-feather
                    $dilR = [Math]::Max(3, [int][Math]::Round($W * 0.0032))
                    $dil = New-Object 'double[,]' $rw, $rh
                    for ($j = 0; $j -lt $rh; $j++) {
                        for ($i = 0; $i -lt $rw; $i++) {
                            if ($rawMask[$i,$j] -gt 0.5) {
                                for ($dj = -$dilR; $dj -le $dilR; $dj++) {
                                    $nj = $j + $dj
                                    if ($nj -lt 0 -or $nj -ge $rh) { continue }
                                    for ($di = -$dilR; $di -le $dilR; $di++) {
                                        if (($di * $di + $dj * $dj) -gt ($dilR * $dilR)) { continue }
                                        $ni = $i + $di
                                        if ($ni -ge 0 -and $ni -lt $rw) { $dil[$ni,$nj] = 1.0 }
                                    }
                                }
                            }
                        }
                    }

                    $blurR = [Math]::Max(2, [int][Math]::Round($W * 0.002))
                    $mask = BoxBlur2D $dil $rw $rh $blurR
                    $mask = BoxBlur2D $mask $rw $rh $blurR

                    # 4. Choose fill strategy
                    if ($WatermarkMode -eq "Mirror" -or ($WatermarkMode -eq "Auto" -and $bestErr -le 45.0)) {
                        $usedMode = "Mirror"
                        for ($j = 0; $j -lt $rh; $j++) {
                            $y = $ry0 + $j
                            $sy = [Math]::Max(0, [Math]::Min($H - 1, $y + $bestDy))
                            for ($i = 0; $i -lt $rw; $i++) {
                                $a = $mask[$i,$j]
                                if ($a -gt 0.001) {
                                    $x = $rx0 + $i
                                    $sx = [Math]::Max(0, [Math]::Min($W - 1, ($W - 1 - $x) + $bestDx))
                                    $c1 = $workBmp.GetPixel($x, $y)
                                    $c2 = $workBmp.GetPixel($sx, $sy)
                                    $r = [int][Math]::Round($c1.R * (1.0 - $a) + $c2.R * $a)
                                    $g = [int][Math]::Round($c1.G * (1.0 - $a) + $c2.G * $a)
                                    $b = [int][Math]::Round($c1.B * (1.0 - $a) + $c2.B * $a)
                                    $al = [int][Math]::Round($c1.A * (1.0 - $a) + $c2.A * $a)
                                    $repaired.SetPixel($x, $y, [System.Drawing.Color]::FromArgb($al, $r, $g, $b))
                                }
                            }
                        }
                    } else {
                        # Harmonic boundary inpainting fallback for asymmetric corners
                        $usedMode = "Inpaint"
                        $rBuf = New-Object 'double[,]' $rw, $rh
                        $gBuf = New-Object 'double[,]' $rw, $rh
                        $bBuf = New-Object 'double[,]' $rw, $rh
                        $isHole = New-Object 'bool[,]' $rw, $rh

                        for ($j = 0; $j -lt $rh; $j++) {
                            $y = $ry0 + $j
                            for ($i = 0; $i -lt $rw; $i++) {
                                $x = $rx0 + $i
                                $c = $workBmp.GetPixel($x, $y)
                                $rBuf[$i,$j] = [double]$c.R
                                $gBuf[$i,$j] = [double]$c.G
                                $bBuf[$i,$j] = [double]$c.B
                                if ($dil[$i,$j] -gt 0.5) { $isHole[$i,$j] = $true }
                            }
                        }

                        # Gauss-Seidel SOR diffusion inside the dilated hole
                        $iters = 60
                        $omega = 1.7
                        for ($it = 0; $it -lt $iters; $it++) {
                            for ($j = 1; $j -lt ($rh - 1); $j++) {
                                for ($i = 1; $i -lt ($rw - 1); $i++) {
                                    if ($isHole[$i,$j]) {
                                        $im = $i - 1; $ip = $i + 1; $jm = $j - 1; $jp = $j + 1
                                        $nr = 0.25 * ($rBuf[$im,$j] + $rBuf[$ip,$j] + $rBuf[$i,$jm] + $rBuf[$i,$jp])
                                        $ng = 0.25 * ($gBuf[$im,$j] + $gBuf[$ip,$j] + $gBuf[$i,$jm] + $gBuf[$i,$jp])
                                        $nb = 0.25 * ($bBuf[$im,$j] + $bBuf[$ip,$j] + $bBuf[$i,$jm] + $bBuf[$i,$jp])
                                        $rBuf[$i,$j] = $rBuf[$i,$j] + $omega * ($nr - $rBuf[$i,$j])
                                        $gBuf[$i,$j] = $gBuf[$i,$j] + $omega * ($ng - $gBuf[$i,$j])
                                        $bBuf[$i,$j] = $bBuf[$i,$j] + $omega * ($nb - $bBuf[$i,$j])
                                    }
                                }
                            }
                        }

                        for ($j = 0; $j -lt $rh; $j++) {
                            $y = $ry0 + $j
                            for ($i = 0; $i -lt $rw; $i++) {
                                $a = $mask[$i,$j]
                                if ($a -gt 0.001) {
                                    $x = $rx0 + $i
                                    $c1 = $workBmp.GetPixel($x, $y)
                                    $rv = $rBuf[$i,$j]
                                    $gv = $gBuf[$i,$j]
                                    $bv = $bBuf[$i,$j]
                                    $r = [Math]::Max(0, [Math]::Min(255, [int][Math]::Round($c1.R * (1.0 - $a) + $rv * $a)))
                                    $g = [Math]::Max(0, [Math]::Min(255, [int][Math]::Round($c1.G * (1.0 - $a) + $gv * $a)))
                                    $b = [Math]::Max(0, [Math]::Min(255, [int][Math]::Round($c1.B * (1.0 - $a) + $bv * $a)))
                                    $repaired.SetPixel($x, $y, [System.Drawing.Color]::FromArgb($c1.A, $r, $g, $b))
                                }
                            }
                        }
                    }
                }
            }

            Write-Output ("WATERMARK: detected={0} pixels={1} bbox=[{2}..{3},{4}..{5}] mode={6}" -f $detected, $wmPixels, $minX, $maxX, $minY, $maxY, $usedMode)

            # Optional 3-panel diagnostic strip: Original | Mask | Repaired
            if (-not [string]::IsNullOrWhiteSpace($DiagnosticPath)) {
                $diagParent = Split-Path -Parent $DiagnosticPath
                if (-not [string]::IsNullOrWhiteSpace($diagParent)) {
                    New-Item -ItemType Directory -Force -Path $diagParent | Out-Null
                }
                $dx0 = if ($isRight)  { $W - $diagSize } else { 0 }
                $dy0 = if ($isBottom) { $H - $diagSize } else { 0 }
                $diagBmp = New-Object System.Drawing.Bitmap(($diagSize * 3), $diagSize)
                try {
                    for ($py = 0; $py -lt $diagSize; $py++) {
                        $y = $dy0 + $py
                        for ($px = 0; $px -lt $diagSize; $px++) {
                            $x = $dx0 + $px
                            $diagBmp.SetPixel($px, $py, $workBmp.GetPixel($x, $y))
                            $mi = $x - $rx0
                            $mj = $y - $ry0
                            $mv = 0
                            if ($mi -ge 0 -and $mi -lt $rw -and $mj -ge 0 -and $mj -lt $rh) {
                                $mval = $mask[$mi,$mj]
                                $mv = [int][Math]::Round($mval * 255.0)
                            }
                            $diagBmp.SetPixel($diagSize + $px, $py, [System.Drawing.Color]::FromArgb(255, $mv, $mv, $mv))
                            $diagBmp.SetPixel($diagSize * 2 + $px, $py, $repaired.GetPixel($x, $y))
                        }
                    }
                    $diagBmp.Save($DiagnosticPath, [System.Drawing.Imaging.ImageFormat]::Png)
                    Write-Output ("DIAGNOSTIC: {0}" -f $DiagnosticPath)
                } finally {
                    $diagBmp.Dispose()
                }
            }

            # Downscale to target square size and save PNG
            $outParent = Split-Path -Parent $OutputPath
            if (-not [string]::IsNullOrWhiteSpace($outParent)) {
                New-Item -ItemType Directory -Force -Path $outParent | Out-Null
            }

            $outFmt = if (HasTransparency $repaired) {
                [System.Drawing.Imaging.PixelFormat]::Format32bppArgb
            } else {
                [System.Drawing.Imaging.PixelFormat]::Format24bppRgb
            }

            $thumb = New-Object System.Drawing.Bitmap($Size, $Size, $outFmt)
            try {
                $gfx = [System.Drawing.Graphics]::FromImage($thumb)
                $wrap = New-Object System.Drawing.Imaging.ImageAttributes
                try {
                    $gfx.CompositingQuality = [System.Drawing.Drawing2D.CompositingQuality]::HighQuality
                    $gfx.InterpolationMode  = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
                    $gfx.SmoothingMode      = [System.Drawing.Drawing2D.SmoothingMode]::HighQuality
                    $gfx.PixelOffsetMode    = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
                    $wrap.SetWrapMode([System.Drawing.Drawing2D.WrapMode]::TileFlipXY)
                    $destRect = New-Object System.Drawing.Rectangle(0, 0, $Size, $Size)
                    $gfx.DrawImage($repaired, $destRect, 0, 0, $W, $H, [System.Drawing.GraphicsUnit]::Pixel, $wrap)
                } finally {
                    $wrap.Dispose()
                    $gfx.Dispose()
                }

                $ms = New-Object System.IO.MemoryStream
                try {
                    $thumb.Save($ms, [System.Drawing.Imaging.ImageFormat]::Png)
                    [System.IO.File]::WriteAllBytes($OutputPath, $ms.ToArray())
                } finally {
                    $ms.Dispose()
                }
            } finally {
                $thumb.Dispose()
            }

            $fi = Get-Item -LiteralPath $OutputPath
            Write-Output ("OUTPUT: {0} ({1}x{1}, PNG, {2}, {3} bytes)" -f $OutputPath, $Size, $outFmt, $fi.Length)
        } finally {
            $repaired.Dispose()
        }
    } finally {
        $workBmp.Dispose()
    }
} finally {
    $srcBmp.Dispose()
}
