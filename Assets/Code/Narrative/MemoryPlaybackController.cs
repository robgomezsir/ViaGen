using System.Collections;
using UnityEngine;
using ViaGen.Core;
using ViaGen.UI;

namespace ViaGen.Narrative
{
    /// <summary>
    /// Playback de memória: câmera/áudio no ambiente; jogador congelado; sem animação de personagem 3D.
    /// </summary>
    public class MemoryPlaybackController : MonoBehaviour
    {
        [SerializeField] private float duration = 16f;
        [SerializeField] private string memoryId;

        public void Play(string id)
        {
            memoryId = id;
            StartCoroutine(PlayRoutine());
        }

        private IEnumerator PlayRoutine()
        {
            var gm = GameManager.Instance;
            if (gm != null) gm.SetState(GameState.MemoryPlayback);

            var player = FindFirstObjectByType<Player.PlayerController>();
            if (player != null) player.CanMove = false;

            var hud = GameObject.Find("HUD");
            if (hud != null) hud.SetActive(false);

            yield return new WaitForSeconds(duration);

            if (hud != null) hud.SetActive(true);
            if (player != null) player.CanMove = true;
            MemoryManager.Instance?.EndPlayback();
        }
    }
}
