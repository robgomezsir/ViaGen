#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using ViaGen.Core;

namespace ViaGen.Editor
{
    /// <summary>
    /// Copia assets da raiz do repositório (Astronauta/, Menu/, PNGs) para os caminhos canônicos em Assets/.
    /// </summary>
    public static class ViaGenSyncSourceAssets
    {
        public static void SyncFromRepoRoot()
        {
            ViaGenProjectStructure.CreateAllFolders();
            var projectRoot = Path.GetDirectoryName(Application.dataPath);
            if (projectRoot == null) return;

            CopyIfExists(Path.Combine(projectRoot, "Astronauta", "source", "astronaught_3_1.fbx"), ViaGenAssetPaths.AstronautFbx);
            CopyDirIfExists(Path.Combine(projectRoot, "Astronauta", "textures"), ViaGenAssetPaths.ArtCharactersPlayerTextures);
            CopyIfExists(Path.Combine(projectRoot, "Menu", "VIAGEN_Stylized_Environment.fbx"), ViaGenAssetPaths.MenuEnvironmentFbx);
            CopyIfExists(Path.Combine(projectRoot, "menu.png"), ViaGenAssetPaths.ResourcesArtUiMenuBackdrop + ".png");
            CopyIfExists(Path.Combine(projectRoot, "iconografia.png"), ViaGenAssetPaths.ResourcesArtUiIconSheet + ".png");
            CopyIfExists(Path.Combine(projectRoot, "icone inicio.png"), ViaGenAssetPaths.ResourcesArtUiAppIcon + ".png");

            AssetDatabase.Refresh();
            Debug.Log("[ViaGen] Assets sincronizados da raiz do repo para Assets/.");
        }

        private static void CopyIfExists(string source, string destAssetPath)
        {
            if (!File.Exists(source)) return;
            var destFull = Path.Combine(Path.GetDirectoryName(Application.dataPath)!, destAssetPath);
            Directory.CreateDirectory(Path.GetDirectoryName(destFull)!);
            File.Copy(source, destFull, true);
        }

        private static void CopyDirIfExists(string sourceDir, string destAssetFolder)
        {
            if (!Directory.Exists(sourceDir)) return;
            var destFull = Path.Combine(Path.GetDirectoryName(Application.dataPath)!, destAssetFolder);
            Directory.CreateDirectory(destFull);
            foreach (var file in Directory.GetFiles(sourceDir))
            {
                if (file.EndsWith(".meta")) continue;
                File.Copy(file, Path.Combine(destFull, Path.GetFileName(file)), true);
            }
        }
    }
}
#endif
