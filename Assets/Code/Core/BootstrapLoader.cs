using UnityEngine;

namespace ViaGen.Core
{
    public class BootstrapLoader : MonoBehaviour
    {
        private void Awake()
        {
            ViaGenRuntimeInitializer.EnsureManagers();
            var gm = GameManager.Instance;
            if (gm != null)
                gm.LoadMainMenu();
        }
    }
}
