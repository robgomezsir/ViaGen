using System.Collections;
using UnityEngine;

namespace ViaGen.UI
{
    public class MainMenuAnimator : MonoBehaviour
    {
        [SerializeField] private MainMenuItemView[] items;
        [SerializeField] private float staggerDelay = 0.08f;
        [SerializeField] private float fadeDuration = 0.35f;

        public void Configure(MainMenuItemView[] menuItems, float stagger, float fade)
        {
            items = menuItems;
            staggerDelay = stagger;
            fadeDuration = fade;
        }

        private void Start() => StartCoroutine(PlayEntrance());

        public IEnumerator PlayEntrance()
        {
            if (items == null) yield break;
            foreach (var item in items)
            {
                if (item == null) continue;
                var cg = item.GetComponent<CanvasGroup>();
                if (cg == null) cg = item.gameObject.AddComponent<CanvasGroup>();
                cg.alpha = 0f;
            }

            foreach (var item in items)
            {
                if (item == null) continue;
                var cg = item.GetComponent<CanvasGroup>();
                if (cg != null)
                    StartCoroutine(FadeIn(cg));
                yield return new WaitForSeconds(staggerDelay);
            }
        }

        private IEnumerator FadeIn(CanvasGroup cg)
        {
            var t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                cg.alpha = Mathf.Clamp01(t / fadeDuration);
                yield return null;
            }
            cg.alpha = 1f;
        }

        public static IEnumerator FadeScreen(CanvasGroup group, float target, float duration)
        {
            if (group == null) yield break;
            var start = group.alpha;
            var t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                group.alpha = Mathf.Lerp(start, target, t / duration);
                yield return null;
            }
            group.alpha = target;
        }
    }
}
