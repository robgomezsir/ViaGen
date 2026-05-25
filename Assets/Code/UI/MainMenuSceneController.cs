using UnityEngine;
using ViaGen.Core;

namespace ViaGen.UI
{
    public class MainMenuSceneController : MonoBehaviour
    {
        private void Awake()
        {
            ViaGenRuntimeInitializer.EnsureManagers();
            UiInputModuleHelper.EnsureCorrectModule();
            if (FindFirstObjectByType<MainMenuUI>() == null)
            {
                var go = new GameObject("MainMenuUI");
                go.AddComponent<MainMenuUI>();
            }
        }
    }
}
