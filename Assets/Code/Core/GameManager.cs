using UnityEngine;
using UnityEngine.SceneManagement;
using ViaGen.Planet;

namespace ViaGen.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private PlanetEmotion currentPlanet = PlanetEmotion.Luto;
        [SerializeField] private GameState state = GameState.MainMenu;

        public PlanetEmotion CurrentPlanet => currentPlanet;
        public GameState State => state;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SetState(GameState newState)
        {
            if (state == newState) return;
            var prev = state;
            state = newState;
            GameEvents.RaiseStateChanged(prev, newState);
        }

        public void SetCurrentPlanet(PlanetEmotion emotion) => currentPlanet = emotion;

        public void LoadMainMenu()
        {
            SetState(GameState.MainMenu);
            SceneManager.LoadScene("MainMenu");
        }

        public void LoadPlanet(PlanetEmotion emotion)
        {
            currentPlanet = emotion;
            SetState(GameState.Exploring);
            SceneManager.LoadScene($"Planet_{emotion}");
        }

        public void LoadShipHub()
        {
            SetState(GameState.InShip);
            SceneManager.LoadScene("ShipHub");
        }

        public void LoadEnding()
        {
            currentPlanet = PlanetEmotion.TerraDestruida;
            SetState(GameState.Ending);
            SceneManager.LoadScene("Ending_Earth");
        }

        public void LoadGame()
        {
            var save = GetComponent<SaveSystem>();
            if (save == null || !save.HasSave()) return;
            save.Load();
            LoadPlanet(currentPlanet);
        }
    }
}
