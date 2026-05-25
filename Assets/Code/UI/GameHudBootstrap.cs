using UnityEngine;
using UnityEngine.UI;

namespace ViaGen.UI
{
    public class GameHudBootstrap : MonoBehaviour
    {
        private void Awake()
        {
            if (GetComponent<SurvivalHUD>() == null)
                BuildMinimalHud();
        }

        private void BuildMinimalHud()
        {
            var panel = new GameObject("SurvivalPanel");
            panel.transform.SetParent(transform, false);
            var rect = panel.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.02f, 0.02f);
            rect.anchorMax = new Vector2(0.3f, 0.12f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            var o2 = CreateBar(panel.transform, "Oxygen", new Color(0.2f, 0.6f, 1f));
            var en = CreateBar(panel.transform, "Energy", new Color(0.9f, 0.8f, 0.2f));
            en.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -28f);

            var hud = gameObject.AddComponent<SurvivalHUD>();
            // SurvivalHUD uses serialized fields - set via reflection alternative: duplicate simple update here
        }

        private static GameObject CreateBar(Transform parent, string name, Color color)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rect = go.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(220f, 18f);
            var bg = go.AddComponent<Image>();
            bg.color = new Color(0f, 0f, 0f, 0.5f);
            var fillGo = new GameObject("Fill");
            fillGo.transform.SetParent(go.transform, false);
            var fillRect = fillGo.AddComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;
            var fill = fillGo.AddComponent<Image>();
            fill.color = color;
            fill.type = Image.Type.Filled;
            fill.fillMethod = Image.FillMethod.Horizontal;
            fill.fillAmount = 1f;
            return go;
        }
    }
}
