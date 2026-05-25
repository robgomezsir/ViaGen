# Cria hierarquia de pastas + .meta para o Unity reconhecer (sem abrir o Editor)
$projectRoot = Split-Path -Parent $PSScriptRoot
$assets = Join-Path $projectRoot "Assets"

$folders = @(
    "Art/Characters/Player/Textures",
    "Art/Environments/Menu",
    "Art/Environments/Planets/Luto",
    "Art/Environments/Planets/Culpa",
    "Art/Environments/Planets/Medo",
    "Art/Environments/Planets/Nostalgia",
    "Art/Environments/Planets/Esperanca",
    "Art/Environments/Ship",
    "Art/Props/Rockets",
    "Art/Props/POI",
    "Art/Skyboxes",
    "Art/VFX",
    "Materials/URP",
    "Prefabs/Characters",
    "Prefabs/Environments/Planets",
    "Prefabs/Environments/Ship",
    "Prefabs/UI",
    "Resources/Art/UI/Menu",
    "Resources/Art/UI/Icons",
    "Resources/Planets",
    "Resources/Fonts",
    "Resources/Audio/Menu",
    "Resources/Audio/Planets",
    "Resources/Audio/SFX",
    "Resources/Audio/Narrative",
    "Resources/Prefabs/Characters",
    "Resources/Prefabs/World",
    "Scenes",
    "Settings/URP",
    "Code/Core",
    "Code/UI",
    "Code/Player",
    "Code/Planet",
    "Code/Crafting",
    "Code/Narrative",
    "Code/Ship",
    "Code/Scanner",
    "Code/Editor"
)

function New-FolderMeta($path) {
    $meta = "$path.meta"
    if (Test-Path $meta) { return }
    $guid = [guid]::NewGuid().ToString("N")
    @"
fileFormatVersion: 2
guid: $guid
folderAsset: yes
DefaultImporter:
  externalObjects: {}
  userData: 
  assetBundleName: 
  assetBundleVariant: 
"@ | Set-Content -Path $meta -Encoding UTF8
}

foreach ($rel in $folders) {
    $full = Join-Path $assets $rel
    New-Item -ItemType Directory -Force -Path $full | Out-Null
    # meta para cada segmento do caminho
    $parts = $rel -split '/'
    $acc = $assets
    foreach ($p in $parts) {
        $acc = Join-Path $acc $p
        New-FolderMeta $acc
    }
    $marker = Join-Path $full "_VIA_GEN_FOLDER.txt"
    if (-not (Test-Path $marker)) {
        Set-Content $marker "VIA:GEN - pasta reservada: $rel"
    }
}

Write-Host "Pastas criadas em $assets"
