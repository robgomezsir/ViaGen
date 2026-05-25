using UnityEngine;
using UnityEngine.SceneManagement;
using ViaGen.Crafting;
using ViaGen.Narrative;
using ViaGen.Planet;
using ViaGen.Ship;

namespace ViaGen.Core
{
    public static class ViaGenRuntimeInitializer
    {
        private static bool _managersCreated;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnAfterSceneLoad()
        {
            EnsureManagers();
            ApplySceneContext(SceneManager.GetActiveScene().name);
            UiInputModuleHelper.EnsureCorrectModule();

            if (SceneManager.GetActiveScene().name == "Bootstrap")
            {
                var bootstrap = Object.FindFirstObjectByType<BootstrapLoader>();
                if (bootstrap == null)
                {
                    var go = new GameObject("BootstrapLoader");
                    go.AddComponent<BootstrapLoader>();
                }
            }
        }

        public static void EnsureManagers()
        {
            if (_managersCreated && GameManager.Instance != null) return;

            if (GameManager.Instance == null)
            {
                var root = new GameObject("[ViaGen_Managers]");
                Object.DontDestroyOnLoad(root);
                root.AddComponent<GameManager>();
                root.AddComponent<SaveSystem>();
                root.AddComponent<AudioManager>();
                root.AddComponent<MemoryManager>();
                root.AddComponent<ShipManager>();
            }

            if (CraftingSystem.Instance == null)
            {
                var craftGo = new GameObject("CraftingSystem");
                Object.DontDestroyOnLoad(craftGo);
                craftGo.AddComponent<CraftingSystem>();
            }

            _managersCreated = true;
        }

        public static void ApplySceneContext(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName)) return;
            var gm = GameManager.Instance;
            if (gm == null) return;

            if (sceneName.StartsWith("Planet_"))
            {
                var emotionName = sceneName.Replace("Planet_", "");
                if (System.Enum.TryParse<PlanetEmotion>(emotionName, out var emotion))
                {
                    gm.SetCurrentPlanet(emotion);
                    gm.SetState(GameState.Exploring);
                    GameEvents.RaisePlanetLoaded(emotion);
                }
            }
            else if (sceneName == "ShipHub")
                gm.SetState(GameState.InShip);
            else if (sceneName == "MainMenu")
                gm.SetState(GameState.MainMenu);
            else if (sceneName == "Ending_Earth")
            {
                gm.SetCurrentPlanet(PlanetEmotion.TerraDestruida);
                gm.SetState(GameState.Ending);
            }
        }
    }
}
