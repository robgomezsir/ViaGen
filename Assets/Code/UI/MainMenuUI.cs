using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ViaGen.Core;
using ViaGen.Planet;

namespace ViaGen.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private MainMenuConfig config;
        [SerializeField] private Sprite backdropSprite;
        [SerializeField] private CanvasGroup fadeOverlay;

        private readonly List<MainMenuItemView> _items = new();
        private MainMenuAnimator _animator;

        private void Start()
        {
            ViaGenRuntimeInitializer.EnsureManagers();
            UiInputModuleHelper.EnsureCorrectModule();
            if (config == null)
                config = Resources.Load<MainMenuConfig>("MainMenuConfig");
            if (backdropSprite == null)
                backdropSprite = Resources.Load<Sprite>("Art/UI/Menu/MenuBackdrop");
            BuildUi();
        }

        private void BuildUi()
        {
            var canvasGo = new GameObject("MainMenuCanvas");
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
            var scaler = canvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            canvasGo.AddComponent<GraphicRaycaster>();

            CreateBackdrop(canvasGo.transform);
            CreateHeader(canvasGo.transform);
            CreateMenuItems(canvasGo.transform);
            CreateFooter(canvasGo.transform);
            CreateFadeOverlay(canvasGo.transform);

            _animator = canvasGo.AddComponent<MainMenuAnimator>();
            _animator.Configure(_items.ToArray(), config != null ? config.staggerDelay : 0.08f,
                config != null ? config.fadeDuration : 0.35f);
        }

        private void CreateBackdrop(Transform parent)
        {
            var go = new GameObject("Backdrop");
            go.transform.SetParent(parent, false);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            var img = go.AddComponent<Image>();
            if (backdropSprite != null)
            {
                img.sprite = backdropSprite;
                img.preserveAspect = false;
            }
            else
                img.color = new Color(0.02f, 0.04f, 0.1f, 1f);
            img.raycastTarget = false;
        }

        private void CreateHeader(Transform parent)
        {
            var header = CreateRect("Header", parent, new Vector2(0.06f, 0.78f), new Vector2(0.5f, 0.95f));
            var logo = ViaGenIcons.AppIcon;
            if (logo != null)
            {
                var logoGo = new GameObject("Logo");
                logoGo.transform.SetParent(header, false);
                var lr = logoGo.AddComponent<RectTransform>();
                lr.anchorMin = new Vector2(0f, 0.55f);
                lr.sizeDelta = new Vector2(120f, 120f);
                var li = logoGo.AddComponent<Image>();
                li.sprite = logo;
                li.preserveAspect = true;
                li.raycastTarget = false;
            }

            AddText(header, "Title", "VIA:GEN", 42, new Vector2(0f, 0.35f), FontStyle.Bold);
            AddText(header, "Subtitle", Localization.Get("menu_subtitle"), 16, new Vector2(0f, 0.05f), FontStyle.Normal);
        }

        private void CreateMenuItems(Transform parent)
        {
            var list = config != null ? config.entries : GetDefaultEntries();
            var container = CreateRect("MenuItems", parent, new Vector2(0.06f, 0.22f), new Vector2(0.45f, 0.72f));
            var layout = container.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 14f;
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            var save = GameManager.Instance?.GetComponent<SaveSystem>();
            var hasSave = save != null && save.HasSave();

            foreach (var entry in list)
            {
                var row = CreateMenuRow(container, entry);
                _items.Add(row);
                if (entry.requiresSave && !hasSave)
                    row.SetInteractable(false);
            }
        }

        private MainMenuItemView CreateMenuRow(Transform parent, MainMenuEntry entry)
        {
            var rowGo = new GameObject($"Item_{entry.action}");
            rowGo.transform.SetParent(parent, false);
            var rowRect = rowGo.AddComponent<RectTransform>();
            rowRect.sizeDelta = new Vector2(0f, 56f);
            rowGo.AddComponent<CanvasGroup>();

            var iconGo = new GameObject("Icon");
            iconGo.transform.SetParent(rowGo.transform, false);
            var iconRect = iconGo.AddComponent<RectTransform>();
            iconRect.anchorMin = new Vector2(0f, 0.5f);
            iconRect.anchorMax = new Vector2(0f, 0.5f);
            iconRect.sizeDelta = new Vector2(40f, 40f);
            iconRect.anchoredPosition = new Vector2(24f, 0f);
            var iconImg = iconGo.AddComponent<Image>();
            var sprite = ViaGenIcons.Get(entry.iconId);
            if (sprite != null) iconImg.sprite = sprite;
            iconImg.preserveAspect = true;

            var texts = new GameObject("Texts");
            texts.transform.SetParent(rowGo.transform, false);
            var textsRect = texts.AddComponent<RectTransform>();
            textsRect.anchorMin = new Vector2(0f, 0f);
            textsRect.anchorMax = new Vector2(1f, 1f);
            textsRect.offsetMin = new Vector2(72f, 0f);
            textsRect.offsetMax = Vector2.zero;

            var title = AddText(texts.transform, "Title", Localization.Get(entry.titleKey), 20,
                new Vector2(0f, 0.6f), FontStyle.Bold);
            title.alignment = TextAnchor.MiddleLeft;
            var sub = AddText(texts.transform, "Sub", Localization.Get(entry.subtitleKey), 12,
                new Vector2(0f, 0.2f), FontStyle.Normal);
            sub.color = new Color(0.7f, 0.75f, 0.85f, 0.9f);
            sub.alignment = TextAnchor.MiddleLeft;

            var btn = rowGo.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            var colors = btn.colors;
            colors.normalColor = new Color(0f, 0f, 0f, 0f);
            colors.highlightedColor = new Color(0f, 0f, 0f, 0f);
            colors.pressedColor = new Color(0f, 0f, 0f, 0f);
            btn.colors = colors;

            var view = rowGo.AddComponent<MainMenuItemView>();
            view.Setup(entry, config, () => OnMenuAction(entry.action));

            return view;
        }

        private void CreateFooter(Transform parent)
        {
            var quote = AddText(parent, "Quote", Localization.Get("menu_quote"), 14,
                new Vector2(0.5f, 0.08f), FontStyle.Italic);
            quote.alignment = TextAnchor.MiddleCenter;
            var qr = quote.rectTransform;
            qr.anchorMin = new Vector2(0.2f, 0.06f);
            qr.anchorMax = new Vector2(0.8f, 0.1f);
            qr.offsetMin = Vector2.zero;
            qr.offsetMax = Vector2.zero;

            var waveGo = new GameObject("Waveform");
            waveGo.transform.SetParent(parent, false);
            var wr = waveGo.AddComponent<RectTransform>();
            wr.anchorMin = new Vector2(0.35f, 0.03f);
            wr.anchorMax = new Vector2(0.65f, 0.05f);
            wr.offsetMin = Vector2.zero;
            wr.offsetMax = Vector2.zero;
            waveGo.AddComponent<MenuAudioVisualizer>();

            var ver = AddText(parent, "Version", Localization.Get("menu_version"), 11,
                new Vector2(0.92f, 0.04f), FontStyle.Normal);
            ver.alignment = TextAnchor.MiddleRight;
            var vr = ver.rectTransform;
            vr.anchorMin = new Vector2(0.7f, 0.02f);
            vr.anchorMax = new Vector2(0.98f, 0.06f);

            var online = AddText(parent, "Online", Localization.Get("menu_online"), 11,
                new Vector2(0.92f, 0.07f), FontStyle.Normal);
            online.color = new Color(0.3f, 0.95f, 1f);
            online.alignment = TextAnchor.MiddleRight;
            var or = online.rectTransform;
            or.anchorMin = new Vector2(0.7f, 0.06f);
            or.anchorMax = new Vector2(0.98f, 0.09f);
        }

        private void CreateFadeOverlay(Transform parent)
        {
            var go = new GameObject("FadeOverlay");
            go.transform.SetParent(parent, false);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            var img = go.AddComponent<Image>();
            img.color = Color.black;
            img.raycastTarget = false;
            fadeOverlay = go.AddComponent<CanvasGroup>();
            fadeOverlay.alpha = 0f;
            fadeOverlay.blocksRaycasts = false;
        }

        private void OnMenuAction(MainMenuAction action)
        {
            switch (action)
            {
                case MainMenuAction.Continue:
                    StartCoroutine(TransitionAnd(() => GameManager.Instance?.LoadGame()));
                    break;
                case MainMenuAction.NewGame:
                    GameManager.Instance?.GetComponent<SaveSystem>()?.DeleteSave();
                    StartCoroutine(TransitionAnd(() => GameManager.Instance?.LoadPlanet(PlanetEmotion.Luto)));
                    break;
                case MainMenuAction.Ship:
                    StartCoroutine(TransitionAnd(() => GameManager.Instance?.LoadShipHub()));
                    break;
                case MainMenuAction.Memories:
                case MainMenuAction.Options:
                    ShowPlaceholder(action);
                    break;
                case MainMenuAction.Quit:
                    StartCoroutine(TransitionAnd(Quit));
                    break;
            }
        }

        private void ShowPlaceholder(MainMenuAction action)
        {
            Debug.Log($"[ViaGen] {action}: {Localization.Get("menu_coming_soon")}");
        }

        private IEnumerator TransitionAnd(System.Action action)
        {
            if (fadeOverlay != null)
            {
                fadeOverlay.blocksRaycasts = true;
                yield return MainMenuAnimator.FadeScreen(fadeOverlay, 1f, config != null ? config.fadeDuration : 0.35f);
            }
            action?.Invoke();
        }

        private static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private static MainMenuEntry[] GetDefaultEntries() => new[]
        {
            new MainMenuEntry { action = MainMenuAction.Continue, iconId = ViaGenIconId.Journey, titleKey = "menu_continue_title", subtitleKey = "menu_continue_sub", requiresSave = true },
            new MainMenuEntry { action = MainMenuAction.NewGame, iconId = ViaGenIconId.Rocket, titleKey = "menu_new_title", subtitleKey = "menu_new_sub" },
            new MainMenuEntry { action = MainMenuAction.Memories, iconId = ViaGenIconId.Photos, titleKey = "menu_memories_title", subtitleKey = "menu_memories_sub" },
            new MainMenuEntry { action = MainMenuAction.Ship, iconId = ViaGenIconId.RocketRepair, titleKey = "menu_ship_title", subtitleKey = "menu_ship_sub" },
            new MainMenuEntry { action = MainMenuAction.Options, iconId = ViaGenIconId.Gear, titleKey = "menu_options_title", subtitleKey = "menu_options_sub" },
            new MainMenuEntry { action = MainMenuAction.Quit, iconId = ViaGenIconId.Gear, titleKey = "menu_quit_title", subtitleKey = "menu_quit_sub" }
        };

        private static RectTransform CreateRect(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            return rect;
        }

        private static Text AddText(Transform parent, string name, string content, int size, Vector2 anchor, FontStyle style)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = new Vector2(0f, 0.5f);
            rect.sizeDelta = new Vector2(500f, 36f);
            var text = go.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = size;
            text.fontStyle = style;
            text.color = Color.white;
            text.text = content;
            return text;
        }
    }
}
