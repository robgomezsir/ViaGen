# ViaGen — setup local (requer Unity 6 no PATH ou abrir editor)
param(
    [switch]$SetupIcons,
    [switch]$SetupMenu,
    [switch]$CreateScenes
)

$unity = $env:UNITY_PATH
if (-not $unity) { $unity = "C:\Program Files\Unity\Hub\Editor\6000.4.8f1\Editor\Unity.exe" }

$project = Split-Path -Parent $MyInvocation.MyCommand.Path
$method = @()
if ($SetupIcons) { $method += "ViaGen.Editor.ViaGenIconSetup.SetupIconsInternal" }
if ($SetupMenu) { $method += "ViaGen.Editor.ViaGenMenuSetup.SetupMenuAssets" }
if ($CreateScenes) { $method += "ViaGen.Editor.ViaGenSceneSetup.CreateAllScenes" }

if ($method.Count -eq 0) {
    Write-Host "Uso: .\build.ps1 -SetupIcons -SetupMenu -CreateScenes"
    Write-Host "Ou no Unity: ViaGen > Setup > Full Project Setup"
    exit 0
}

foreach ($m in $method) {
    & $unity -batchmode -quit -projectPath $project -executeMethod $m
}
