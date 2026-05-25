# VIA:GEN — Unity Assets

## Menu **ViaGen** não aparece?

1. Abra **Window > General > Console** e veja se há erros em vermelho nos scripts.
2. Clique com o botão direito em `Assets/Code` → **Reimport**.
3. Menu **Assets > Refresh** (Ctrl+R).
4. O menu fica na barra superior: **ViaGen** (entre Component e Window).

Se ainda não aparecer: feche e reabra o projeto no Unity Hub.

## Corrigir pastas no Project

Se a hierarquia não aparecer corretamente no Unity:

1. **ViaGen → Setup → Create Project Folder Structure**
2. **ViaGen → Assets → Sync Source Assets From Repo Root**
3. **Assets → Refresh** (Ctrl+R)

Ou no terminal, na raiz do repo:

```powershell
.\tools\Create-UnityFolders.ps1
```

Depois abra o Unity e use **Refresh**.

## Onde colocar cada tipo de arquivo

| Tipo | Pasta |
|------|--------|
| Modelo jogador | `Art/Characters/Player/` |
| Texturas PBR | `Art/Characters/Player/Textures/` |
| Ambiente menu (FBX) | `Art/Environments/Menu/` |
| Arte por planeta | `Art/Environments/Planets/Luto` (etc.) |
| Props / foguetes | `Art/Props/Rockets/` |
| Skybox | `Art/Skyboxes/` |
| UI runtime (ícones, menu.png) | `Resources/Art/UI/` |
| ScriptableObjects planetas | `Resources/Planets/` |
| Cenas | `Scenes/` |
| Scripts | `Code/` (não mover para Resources) |

## Lista completa

Ver `Assets/PROJECT_STRUCTURE.txt` (gerado pelo setup).

## Caminhos no código

Use `ViaGen.Core.ViaGenAssetPaths` — nunca strings `Assets/...` espalhadas.
