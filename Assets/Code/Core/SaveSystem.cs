using UnityEngine;
using ViaGen.Planet;

namespace ViaGen.Core
{
    public class SaveSystem : MonoBehaviour
    {
        private const string KeyPlanet = "viagen_planet";
        private const string KeyHasSave = "viagen_has_save";

        public bool HasSave() => PlayerPrefs.GetInt(KeyHasSave, 0) == 1;

        public void Save()
        {
            var gm = GameManager.Instance;
            if (gm == null) return;
            PlayerPrefs.SetString(KeyPlanet, gm.CurrentPlanet.ToString());
            PlayerPrefs.SetInt(KeyHasSave, 1);
            PlayerPrefs.Save();
        }

        public void Load()
        {
            if (!HasSave()) return;
            var planetStr = PlayerPrefs.GetString(KeyPlanet, PlanetEmotion.Luto.ToString());
            if (System.Enum.TryParse<PlanetEmotion>(planetStr, out var emotion) && GameManager.Instance != null)
                GameManager.Instance.SetCurrentPlanet(emotion);
        }

        public void DeleteSave()
        {
            PlayerPrefs.DeleteKey(KeyPlanet);
            PlayerPrefs.DeleteKey(KeyHasSave);
            PlayerPrefs.Save();
        }
    }
}
