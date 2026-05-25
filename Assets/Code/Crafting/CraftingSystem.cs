using UnityEngine;

namespace ViaGen.Crafting
{
    public class CraftingSystem : MonoBehaviour
    {
        public static CraftingSystem Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(this); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
