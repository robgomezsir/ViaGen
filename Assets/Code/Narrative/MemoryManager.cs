using UnityEngine;
using ViaGen.Core;

namespace ViaGen.Narrative
{
    public class MemoryManager : MonoBehaviour
    {
        public static MemoryManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(this); return; }
            Instance = this;
        }

        public void PlayMemory(string memoryId)
        {
            GameEvents.RaiseMemoryTriggered(memoryId);
            var playback = FindFirstObjectByType<MemoryPlaybackController>();
            if (playback != null)
                playback.Play(memoryId);
            else
            {
                var gm = GameManager.Instance;
                if (gm != null) gm.SetState(GameState.MemoryPlayback);
                Debug.Log($"[ViaGen] Memória: {memoryId}");
            }
        }

        public void EndPlayback()
        {
            var gm = GameManager.Instance;
            if (gm != null) gm.SetState(GameState.Exploring);
        }
    }
}
