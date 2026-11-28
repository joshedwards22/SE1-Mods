@echo off
setlocal

rem Usage: export_all.bat <file.blend | folder> [extra export_blend.py options, e.g. --dry-run --jobs 2]
if "%~1"=="" (
    echo Usage: %~nx0 ^<file.blend ^| folder^> [--dry-run] [--jobs N] [--expect-mod NAME] ...
    exit /b 1
)

set "TARGET=%~1"
shift

rem Collect any remaining options to pass through
set "EXTRA="
:collect
if "%~1"=="" goto run
set "EXTRA=%EXTRA% %1"
shift
goto collect

:run
"C:\Program Files\Blender Foundation\Blender 4.5\blender.exe" -b -P "%~dp0.agents\skills\seut-blend\scripts\export_blend.py" -- --path "%TARGET%"%EXTRA%
exit /b %ERRORLEVEL%
