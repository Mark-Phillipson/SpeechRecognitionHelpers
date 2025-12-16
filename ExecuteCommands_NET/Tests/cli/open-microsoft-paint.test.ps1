# CLI test: open Microsoft Paint
# Usage: Open PowerShell and run this script. It builds the project, runs the app with a natural command,
# waits briefly, and checks the generated app.log for evidence we attempted to launch mspaint.

$ErrorActionPreference = "Stop"
$proj = "ExecuteCommands.csproj"
Write-Host "Building project..."
dotnet build $proj -c Release | Write-Host

Write-Host "Running: dotnet run -- natural \"open microsoft paint\""
dotnet run -p $proj -c Release -- natural "open microsoft paint" 2>&1 | Write-Host

# Give app some time to write logs and for apps to start
Start-Sleep -Seconds 2

# log path relative to project root (Program.cs writes to bin\Release\net10.0-windows\app.log)
$logPath = "bin\Release\net10.0-windows\app.log"
if (-Not (Test-Path $logPath)) {
    Write-Error "Log file not found: $logPath"
    exit 2
}

$log = Get-Content $logPath -Raw
Write-Host "--- Log excerpt ---"
$log | Select-String -Pattern "mspaint|microsoft paint|Launched|Started|Fallback explorer" -AllMatches | ForEach-Object { Write-Host $_.Line }
Write-Host "--- End excerpt ---"

if ($log -match "mspaint" -or $log -match "microsoft paint") {
    Write-Host "PASS: mspaint reference found in log."
    exit 0
} else {
    Write-Error "FAIL: no mspaint reference found in log."
    exit 1
}



















