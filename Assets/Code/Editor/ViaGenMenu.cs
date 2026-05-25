#if UNITY_EDITOR
using UnityEditor;

namespace ViaGen.Editor
{
    /// <summary>
    /// Ponto de entrada do menu ViaGen no topo da Unity (garante itens visíveis).
    /// </summary>
    public static class ViaGenMenu
    {
        [MenuItem("ViaGen/Setup/Full Project Setup", false, 0)]
        public static void FullSetup() => ViaGenSceneSetup.FullSetup();

        [MenuItem("ViaGen/Setup/Create Project Folder Structure", false, 1)]
        public static void CreateFolders() => ViaGenProjectStructure.CreateProjectFoldersMenu();

        [MenuItem("ViaGen/Setup/Create All Scenes", false, 2)]
        public static void CreateScenes() => ViaGenSceneSetup.CreateAllScenes();

        [MenuItem("ViaGen/Setup/Configure URP Pipeline", false, 3)]
        public static void ConfigureUrp() => ViaGenUrpSetup.ConfigureUrp();

        [MenuItem("ViaGen/Assets/Setup Icons And App Icon", false, 20)]
        public static void SetupIcons() => ViaGenIconSetup.SetupIconsMenu();

        [MenuItem("ViaGen/Assets/Setup Menu Assets", false, 21)]
        public static void SetupMenuAssets() => ViaGenMenuSetup.SetupMenuAssets();

        [MenuItem("ViaGen/Assets/Sync Source Assets From Repo Root", false, 22)]
        public static void SyncAssets() => ViaGenSyncSourceAssets.SyncFromRepoRoot();

        [MenuItem("ViaGen/Help/Open README", false, 100)]
        public static void OpenReadme()
        {
            var path = "Assets/README_ViaGen.md";
            if (!System.IO.File.Exists(path))
                path = "Assets/README_VIA_GEN.md";
            var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            if (asset != null)
                AssetDatabase.OpenAsset(asset);
            else
                EditorUtility.DisplayDialog("ViaGen", "Arquivo não encontrado: " + path, "OK");
        }
    }
}
#endif
