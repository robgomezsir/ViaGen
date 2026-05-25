#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using ViaGen.Core;

namespace ViaGen.Editor
{
    public static class ViaGenMenuSetup
    {
        private static string BackdropPath => ViaGenAssetPaths.ResourcesArtUiMenuBackdrop + ".png";
        private static string ConfigPath => ViaGenAssetPaths.ResourcesMainMenuConfig + ".asset";

        public static void SetupMenuAssets()
        {
            ViaGenProjectStructure.CreateAllFolders();
            ConfigureBackdrop();
            EnsureMainMenuConfig();
            ConfigureAstronautFbx();
            ConfigureEnvironmentFbx();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[ViaGen] Menu assets configurados.");
        }

        private static void ConfigureBackdrop()
        {
            var importer = AssetImporter.GetAtPath(BackdropPath) as TextureImporter;
            if (importer == null) return;
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.alphaIsTransparency = false;
            importer.mipmapEnabled = false;
            importer.maxTextureSize = 4096;
            importer.SaveAndReimport();
        }

        private static void EnsureMainMenuConfig()
        {
            var existing = AssetDatabase.LoadAssetAtPath<MainMenuConfig>(ConfigPath);
            if (existing != null) return;
            var config = ScriptableObject.CreateInstance<MainMenuConfig>();
            AssetDatabase.CreateAsset(config, ConfigPath);
        }

        private static void ConfigureAstronautFbx()
        {
            var path = ViaGenAssetPaths.AstronautFbx;
            var importer = AssetImporter.GetAtPath(path) as ModelImporter;
            if (importer == null) return;
            importer.importAnimation = false;
            importer.animationType = ModelImporterAnimationType.None;
            importer.materialImportMode = ModelImporterMaterialImportMode.ImportViaMaterialDescription;
            importer.SaveAndReimport();
        }

        private static void ConfigureEnvironmentFbx()
        {
            var path = ViaGenAssetPaths.MenuEnvironmentFbx;
            var importer = AssetImporter.GetAtPath(path) as ModelImporter;
            if (importer == null) return;
            importer.importAnimation = false;
            importer.globalScale = 1f;
            importer.SaveAndReimport();
        }
    }
}
#endif
