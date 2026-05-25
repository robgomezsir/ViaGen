using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ViaGen.Core;

namespace ViaGen.UI
{
    /// <summary>
    /// Menu de seleção (pausa) — mesmas animações de hover/entrada que o menu principal; fundo estático.
    /// </summary>
    public class PauseMenuUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup panelGroup;
        private readonly List<MainMenuItemView> _items = new();

        public void Show()
        {
            gameObject.SetActive(true);
            GameManager.Instance?.SetState(GameState.Paused);
            Time.timeScale = 0f;
            if (panelGroup != null)
                StartCoroutine(MainMenuAnimator.FadeScreen(panelGroup, 1f, 0.2f));
        }

        public void Hide()
        {
            Time.timeScale = 1f;
            GameManager.Instance?.SetState(GameState.Exploring);
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (UnityEngine.InputSystem.Keyboard.current?.escapeKey.wasPressedThisFrame == true)
                Hide();
        }
    }
}
