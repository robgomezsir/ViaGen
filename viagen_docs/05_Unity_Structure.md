# VIA:GEN — Estrutura no Unity

## Motor Gráfico
Unity 6 LTS

## Pipeline de Renderização
URP (Pipeline de Renderização Universal)

---

# Hierarquia oficial (`Assets/`)

```
Assets/
├── Art/
│   ├── Characters/Player/          # astronaught_3_1.fbx + Textures/
│   ├── Environments/
│   │   ├── Menu/                   # blockout VIAGEN_Stylized_Environment.fbx
│   │   ├── Planets/                # Luto, Culpa, Medo, Nostalgia, Esperanca
│   │   └── Ship/                   # interior nave
│   ├── Props/
│   │   ├── Rockets/                 # destroços / foguetes
│   │   └── POI/                    # pontos de interesse
│   ├── Skyboxes/
│   └── VFX/
├── Materials/URP/                  # Lit, Emissive
├── Prefabs/
│   ├── Characters/                 # Player
│   ├── Environments/Planets|Ship/
│   └── UI/
├── Resources/                      # apenas assets carregados em runtime
│   ├── Art/UI/                     # IconSheet, AppIcon, Menu/MenuBackdrop
│   ├── Audio/Menu|Planets|SFX|Narrative/
│   ├── Fonts/
│   ├── Planets/                    # EmotionPlanetData (.asset)
│   ├── Prefabs/Characters|World/
│   └── MainMenuConfig.asset
├── Scenes/                         # Bootstrap, MainMenu, Planet_*, ShipHub
├── Settings/URP/
├── Code/                           # scripts (asmdef ViaGen.Core + Editor)
└── PROJECT_STRUCTURE.txt           # gerado pelo setup
```

**Raiz do repositório (fonte, não duplicar no Unity):**
- `Astronauta/` → sincronizar para `Assets/Art/Characters/Player/`
- `Menu/` → `Assets/Art/Environments/Menu/`
- `menu.png`, `iconografia.png`, `icone inicio.png` → `Assets/Resources/Art/UI/`

---

# Setup no Unity

| Menu | Função |
|------|--------|
| **ViaGen → Setup → Create Project Folder Structure** | Cria todas as pastas via AssetDatabase |
| **ViaGen → Assets → Sync Source Assets From Repo Root** | Copia FBX/PNG da raiz para `Assets/` |
| **ViaGen → Setup → Full Project Setup** | Pastas + ícones + menu + cenas + URP |

**PowerShell (sem abrir Unity):** `tools/Create-UnityFolders.ps1`

**Código:** caminhos em `ViaGenAssetPaths.cs` — não usar strings soltas.

---

# Gerenciadores

## GerenciadorDeJogo
Controla estado global, salvamentos, progressão.

## GerenciadorDePlaneta
Geração procedural, biomas, clima.

## GerenciadorDeMemória
Memórias, narrativa, gatilhos.

## GerenciadorDeÁudio
Música dinâmica, ambiência.

## GerenciadorDaNave
Melhorias, combustível, sistemas da nave.
