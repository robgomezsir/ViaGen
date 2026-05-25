#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using ViaGen.Core;

namespace ViaGen.Editor
{
    /// <summary>
    /// Cria e mantém a hierarquia oficial de pastas do VIA:GEN no Unity (AssetDatabase + placeholders).
    /// </summary>
    public static class ViaGenProjectStructure
    {
        private static readonly string[] FolderTree =
        {
            ViaGenAssetPaths.Art,
            ViaGenAssetPaths.ArtCharacters,
            ViaGenAssetPaths.ArtCharactersPlayer,
            ViaGenAssetPaths.ArtCharactersPlayerTextures,
            ViaGenAssetPaths.ArtEnvironments,
            ViaGenAssetPaths.ArtEnvironmentsMenu,
            ViaGenAssetPaths.ArtEnvironmentsPlanets,
            ViaGenAssetPaths.ArtEnvironmentsPlanets + "/Luto",
            ViaGenAssetPaths.ArtEnvironmentsPlanets + "/Culpa",
            ViaGenAssetPaths.ArtEnvironmentsPlanets + "/Medo",
            ViaGenAssetPaths.ArtEnvironmentsPlanets + "/Nostalgia",
            ViaGenAssetPaths.ArtEnvironmentsPlanets + "/Esperanca",
            ViaGenAssetPaths.ArtEnvironmentsShip,
            ViaGenAssetPaths.ArtProps,
            ViaGenAssetPaths.ArtPropsRockets,
            ViaGenAssetPaths.ArtPropsPoi,
            ViaGenAssetPaths.ArtSkyboxes,
            ViaGenAssetPaths.ArtVfx,
            ViaGenAssetPaths.Materials,
            ViaGenAssetPaths.MaterialsUrp,
            ViaGenAssetPaths.Prefabs,
            ViaGenAssetPaths.PrefabsCharacters,
            ViaGenAssetPaths.PrefabsEnvironments,
            ViaGenAssetPaths.PrefabsEnvironmentsPlanets,
            ViaGenAssetPaths.PrefabsEnvironmentsShip,
            ViaGenAssetPaths.PrefabsUi,
            ViaGenAssetPaths.ResourcesRoot,
            ViaGenAssetPaths.ResourcesArtUi,
            ViaGenAssetPaths.ResourcesArtUiMenu,
            ViaGenAssetPaths.ResourcesArtUiIcons,
            ViaGenAssetPaths.ResourcesPlanets,
            ViaGenAssetPaths.ResourcesFonts,
            ViaGenAssetPaths.ResourcesAudio,
            ViaGenAssetPaths.ResourcesAudioMenu,
            ViaGenAssetPaths.ResourcesAudioPlanets,
            ViaGenAssetPaths.ResourcesAudioSfx,
            ViaGenAssetPaths.ResourcesAudioNarrative,
            ViaGenAssetPaths.ResourcesPrefabs,
            ViaGenAssetPaths.ResourcesPrefabs + "/Characters",
            ViaGenAssetPaths.ResourcesPrefabs + "/World",
            ViaGenAssetPaths.Scenes,
            ViaGenAssetPaths.Settings,
            ViaGenAssetPaths.SettingsUrp,
            ViaGenAssetPaths.Code,
            ViaGenAssetPaths.Code + "/Core",
            ViaGenAssetPaths.Code + "/UI",
            ViaGenAssetPaths.Code + "/Player",
            ViaGenAssetPaths.Code + "/Planet",
            ViaGenAssetPaths.Code + "/Crafting",
            ViaGenAssetPaths.Code + "/Narrative",
            ViaGenAssetPaths.Code + "/Ship",
            ViaGenAssetPaths.Code + "/Scanner",
            ViaGenAssetPaths.CodeEditor
        };

        [MenuItem("ViaGen/Setup/Create Project Folder Structure", priority = 0)]
        public static void CreateProjectFoldersMenu()
        {
            var created = CreateAllFolders();
            EnsurePlaceholders();
            AssetDatabase.Refresh();
            Debug.Log($"[ViaGen] Estrutura de pastas: {created} pasta(s) criada(s). Veja Assets/ e o arquivo PROJECT_STRUCTURE.txt.");
        }

        public static int CreateAllFolders()
        {
            var created = 0;
            foreach (var path in FolderTree)
            {
                if (CreateFolderPath(path))
                    created++;
            }
            return created;
        }

        private static bool CreateFolderPath(string assetPath)
        {
            if (AssetDatabase.IsValidFolder(assetPath))
                return false;

            var parts = assetPath.Split('/');
            if (parts.Length < 2 || parts[0] != "Assets")
                return false;

            var created = false;
            var current = "Assets";
            for (var i = 1; i < parts.Length; i++)
            {
                var next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[i]);
                    created = true;
                }
                current = next;
            }
            return created;
        }

        private static void EnsurePlaceholders()
        {
            var placeholders = new Dictionary<string, string>
            {
                { ViaGenAssetPaths.ArtPropsRockets, "Prefabs de foguetes (Elena, Lucas, etc.)." },
                { ViaGenAssetPaths.ArtPropsPoi, "Pontos de interesse por planeta." },
                { ViaGenAssetPaths.ArtSkyboxes, "Materiais Skybox por emoção." },
                { ViaGenAssetPaths.ArtVfx, "Partículas e VFX." },
                { ViaGenAssetPaths.MaterialsUrp, "Materiais URP Lit / Emissive." },
                { ViaGenAssetPaths.ArtEnvironmentsPlanets + "/Luto", "Arte ambiente — Planeta Luto." },
                { ViaGenAssetPaths.ArtEnvironmentsShip, "Interior da nave." },
                { ViaGenAssetPaths.PrefabsCharacters, "Prefab do jogador (astronauta)." },
                { ViaGenAssetPaths.PrefabsUi, "Prefabs de UI (menu, HUD)." },
                { ViaGenAssetPaths.ResourcesFonts, "Fontes TMP (.ttf/.otf)." },
                { ViaGenAssetPaths.ResourcesAudioMenu, "Música e SFX do menu." },
                { ViaGenAssetPaths.ResourcesAudioPlanets, "Ambiente por planeta." },
                { ViaGenAssetPaths.ResourcesAudioSfx, "Passos, UI, escâner." },
                { ViaGenAssetPaths.ResourcesAudioNarrative, "Vozes e memórias." },
                { ViaGenAssetPaths.ResourcesPrefabs + "/Characters", "Player.prefab para Resources.Load." },
                { ViaGenAssetPaths.Scenes, "Bootstrap, MainMenu, Planet_*, ShipHub." },
                { ViaGenAssetPaths.SettingsUrp, "URP Asset e Renderer." }
            };

            foreach (var kv in placeholders)
            {
                var folder = kv.Key;
                if (!AssetDatabase.IsValidFolder(folder)) continue;
                var marker = folder + "/_VIA_GEN_FOLDER.txt";
                if (File.Exists(marker)) continue;
                File.WriteAllText(marker,
                    "VIA:GEN — pasta reservada\n" +
                    kv.Value + "\n\nNão apague este arquivo; pode substituir por assets reais.\n");
            }

            WriteStructureManifest();
        }

        private static void WriteStructureManifest()
        {
            var lines = new List<string>
            {
                "VIA:GEN — Hierarquia oficial (Assets/)",
                "Gerado por ViaGen > Setup > Create Project Folder Structure",
                ""
            };
            lines.AddRange(FolderTree);
            File.WriteAllLines("Assets/PROJECT_STRUCTURE.txt", lines);
            AssetDatabase.ImportAsset("Assets/PROJECT_STRUCTURE.txt");
        }

        [InitializeOnLoadMethod]
        private static void OnLoad()
        {
            EditorApplication.delayCall += () =>
            {
                if (!SessionState.GetBool("ViaGen_FoldersChecked", false))
                {
                    SessionState.SetBool("ViaGen_FoldersChecked", true);
                    if (!AssetDatabase.IsValidFolder(ViaGenAssetPaths.ArtEnvironmentsPlanets))
                        Debug.Log("[ViaGen] Estrutura incompleta. Use: ViaGen > Setup > Create Project Folder Structure");
                }
            };
        }
    }
}
#endif
