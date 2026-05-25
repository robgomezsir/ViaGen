#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using ViaGen.Core;
using ViaGen.Narrative;
using ViaGen.Planet;
using ViaGen.Player;
using ViaGen.UI;

namespace ViaGen.Editor
{
    public static class ViaGenSceneSetup
    {
        public static void CreateAllScenes()
        {
            ViaGenProjectStructure.CreateAllFolders();
            var scenesFolder = ViaGenAssetPaths.Scenes;
            CreateBootstrapScene();
            CreateMainMenuScene();
            CreatePlanetScenes();
            CreateShipHubScene();
            CreateEndingScene();
            ConfigureBuildSettings();
            AssetDatabase.SaveAssets();
            Debug.Log("[ViaGen] Cenas criadas em " + ViaGenAssetPaths.Scenes);
        }

        public static void FullSetup()
        {
            ViaGenProjectStructure.CreateProjectFoldersMenu();
            ViaGenMenuSetup.SetupMenuAssets();
            ViaGenIconSetup.SetupIconsInternal();
            ViaGenUrpSetup.ConfigureUrp();
            CreateAllScenes();
        }

        private static void CreateBootstrapScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            var loader = new GameObject("BootstrapLoader");
            loader.AddComponent<BootstrapLoader>();
            SaveScene("Bootstrap.unity");
        }

        private static void CreateMainMenuScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var root = new GameObject("MainMenuRoot");
            root.AddComponent<MainMenuSceneController>();
            SaveScene("MainMenu.unity");
        }

        private static void CreatePlanetScenes()
        {
            foreach (PlanetEmotion emotion in System.Enum.GetValues(typeof(PlanetEmotion)))
            {
                if (emotion == PlanetEmotion.TerraDestruida) continue;
                CreatePlanetScene(emotion);
            }
        }

        private static void CreatePlanetScene(PlanetEmotion emotion)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            var terrainGo = Terrain.CreateTerrainGameObject(new TerrainData());
            terrainGo.name = "PlanetTerrain";
            terrainGo.transform.position = Vector3.zero;
            var data = CreatePlanetData(emotion);
            var gen = terrainGo.AddComponent<PlanetGenerator>();
            var so = new SerializedObject(gen);
            so.FindProperty("planetData").objectReferenceValue = data;
            so.ApplyModifiedPropertiesWithoutUndo();

            var spawn = new GameObject("PlayerSpawn");
            spawn.transform.position = new Vector3(0f, 80f, 0f);
            spawn.AddComponent<PlayerSpawner>();

            var hud = new GameObject("HUD");
            var canvas = hud.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            hud.AddComponent<UnityEngine.UI.CanvasScaler>();
            hud.AddComponent<UnityEngine.UI.GraphicRaycaster>();
            hud.AddComponent<GameHudBootstrap>();

            var pm = new GameObject("PlanetManager");
            pm.AddComponent<PlanetManager>();

            SaveScene($"Planet_{emotion}.unity");
        }

        private static EmotionPlanetData CreatePlanetData(PlanetEmotion emotion)
        {
            var folder = ViaGenAssetPaths.ResourcesPlanets;
            Directory.CreateDirectory(folder);
            var path = $"{folder}/Planet_{emotion}.asset";
            var existing = AssetDatabase.LoadAssetAtPath<EmotionPlanetData>(path);
            if (existing != null) return existing;

            var data = ScriptableObject.CreateInstance<EmotionPlanetData>();
            data.emotion = emotion;
            data.displayName = emotion.ToString();
            data.terrainSeed = 100 + (int)emotion;
            data.skyTint = emotion switch
            {
                PlanetEmotion.Luto => new Color(0.35f, 0.3f, 0.65f),
                PlanetEmotion.Culpa => new Color(0.3f, 0.5f, 0.35f),
                PlanetEmotion.Medo => new Color(0.6f, 0.15f, 0.2f),
                PlanetEmotion.Nostalgia => new Color(0.85f, 0.7f, 0.4f),
                PlanetEmotion.Esperanca => new Color(0.5f, 0.75f, 0.95f),
                _ => new Color(0.4f, 0.4f, 0.5f)
            };
            data.fogColor = data.skyTint * 0.3f;
            AssetDatabase.CreateAsset(data, path);
            return data;
        }

        private static void CreateShipHubScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "ShipFloor";
            floor.transform.localScale = new Vector3(12f, 0.2f, 20f);
            floor.transform.position = new Vector3(0f, 0f, 0f);

            var spawn = new GameObject("PlayerSpawn");
            spawn.transform.position = new Vector3(0f, 1f, -8f);
            spawn.AddComponent<PlayerSpawner>();

            var hud = new GameObject("HUD");
            hud.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            hud.AddComponent<UnityEngine.UI.CanvasScaler>();
            hud.AddComponent<UnityEngine.UI.GraphicRaycaster>();
            hud.AddComponent<GameHudBootstrap>();

            SaveScene("ShipHub.unity");
        }

        private static void CreateEndingScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            var cam = Camera.main;
            if (cam != null)
            {
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = Color.black;
            }
            SaveScene("Ending_Earth.unity");
        }

        private static void SaveScene(string fileName)
        {
            var path = Path.Combine(ViaGenAssetPaths.Scenes, fileName);
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), path);
        }

        private static void ConfigureBuildSettings()
        {
            var scenes = new List<EditorBuildSettingsScene>
            {
                MakeScene("Bootstrap.unity"),
                MakeScene("MainMenu.unity"),
                MakeScene("Planet_Luto.unity"),
                MakeScene("Planet_Culpa.unity"),
                MakeScene("Planet_Medo.unity"),
                MakeScene("Planet_Nostalgia.unity"),
                MakeScene("Planet_Esperanca.unity"),
                MakeScene("ShipHub.unity"),
                MakeScene("Ending_Earth.unity")
            };
            EditorBuildSettings.scenes = scenes.ToArray();
        }

        private static EditorBuildSettingsScene MakeScene(string file) =>
            new EditorBuildSettingsScene(Path.Combine(ViaGenAssetPaths.Scenes, file), true);
    }
}
#endif
