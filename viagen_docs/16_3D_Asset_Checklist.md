# VIA:GEN — Checklist de assets 3D

## Entregues no repositório

- [x] `Astronauta/source/astronaught_3_1.fbx` → `Assets/Art/Characters/Player/`
- [x] `Astronauta/textures/bake_*_4k.png` → `Assets/Art/Characters/Player/Textures/`
- [x] `Menu/VIAGEN_Stylized_Environment.fbx` → `Assets/Art/Environments/Menu/` (opcional)
- [x] `menu.png` → `Assets/Resources/Art/UI/Menu/MenuBackdrop.png` (estático)
- [x] `iconografia.png` → `Assets/Resources/Art/UI/IconSheet.png`
- [x] `icone inicio.png` → `Assets/Resources/Art/UI/AppIcon.png`

## Unity (após abrir o projeto)

1. **ViaGen → Setup → Create Project Folder Structure** (corrige hierarquia no Project)
2. **ViaGen → Assets → Sync Source Assets From Repo Root** (copia Astronauta/Menu/PNG)
3. **ViaGen → Setup → Full Project Setup**
2. Ou passo a passo: **Setup Menu Assets**, **Setup Icons**, **Create All Scenes**, **Configure URP Pipeline**
3. Play em `Bootstrap` ou `MainMenu`

## Regras de animação

| Com animação | Estático |
|--------------|----------|
| Itens de menu (hover, entrada) | `MenuBackdrop.png` |
| Waveform UI | Astronauta (bind pose) |
| Menus de pausa/listas | Planetas, props, terreno |

## Pendente (arte futura)

- [ ] Skybox por planeta
- [ ] Terrain layers por bioma
- [ ] POI prefabs (foguetes, cristais) por emoção
- [ ] Interior nave detalhado
- [ ] Áudio ambiente e SFX
