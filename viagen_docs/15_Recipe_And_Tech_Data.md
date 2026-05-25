# VIA:GEN — Recipes & Tech Data

## Recursos
| ID | Categoria |
|----|-----------|
| titanio | Mineral |
| cobalto_ionizado | Mineral |
| carbono_cristalino | Mineral |
| fungos_neurais | Biológico |
| algas_termicas | Biológico |
| nucleo_quantico | Tecnológico |
| chip_neural | Tecnológico |

## Receitas
| ID | Estação | Ingredientes | Saída |
|----|---------|--------------|-------|
| oxygen_tank | Portátil | 2 titânio, 1 carbono | oxigenio |
| med_kit | Portátil | 2 algas, 1 fungos | kit_medico |
| fuel_cell | Oficina | 3 cobalto, 2 carbono | combustivel |
| tech_jetpack | Oficina | 5 titânio, 1 núcleo | tech_jetpack |

## Tech Tree
### Nv.1 — Sobrevivência
- scanner_basic, flashlight, simple_repair

### Nv.2 — Exploração
- jetpack ← scanner_basic
- cave_radar ← jetpack
- submarine_module ← jetpack

### Nv.3 — Científico
- quantum_analysis ← cave_radar
- gravity_stabilizer ← quantum_analysis

### Nv.4 — Interestelar
- warp_drive ← gravity_stabilizer
- advanced_ai ← warp_drive
- colony_systems ← warp_drive

Implementação: `GameContentDatabase.cs` + `CraftingSystem.cs`
