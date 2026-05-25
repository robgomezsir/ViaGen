using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ViaGen.Core;

namespace ViaGen.UI
{
    public class MainMenuItemView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private Text titleText;
        [SerializeField] private Text subtitleText;
        [SerializeField] private Button button;
        [SerializeField] private CanvasGroup canvasGroup;

        private Color _iconNormal = Color.white;
        private Color _hoverColor = new(0f, 0.9f, 1f, 1f);
        private Vector2 _titleBasePos;
        private bool _interactable = true;

        public MainMenuEntry Entry { get; private set; }
        public Button Button => button;

        public void Setup(MainMenuEntry entry, MainMenuConfig config, UnityEngine.Events.UnityAction onClick)
        {
            Entry = entry;
            _hoverColor = config != null ? config.hoverColor : _hoverColor;
            iconImage ??= transform.Find("Icon")?.GetComponent<Image>();
            titleText ??= transform.Find("Texts/Title")?.GetComponent<Text>();
            subtitleText ??= transform.Find("Texts/Sub")?.GetComponent<Text>();
            button ??= GetComponent<Button>();
            canvasGroup ??= GetComponent<CanvasGroup>();
            if (titleText != null)
            {
                titleText.text = Localization.Get(entry.titleKey);
                _titleBasePos = titleText.rectTransform.anchoredPosition;
            }
            if (subtitleText != null)
                subtitleText.text = Localization.Get(entry.subtitleKey);
            if (iconImage != null)
            {
                var sprite = ViaGenIcons.Get(entry.iconId);
                if (sprite != null) iconImage.sprite = sprite;
                _iconNormal = iconImage.color;
            }
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(onClick);
            }
        }

        public void SetInteractable(bool value)
        {
            _interactable = value;
            if (button != null) button.interactable = value;
            if (canvasGroup != null) canvasGroup.alpha = value ? 1f : 0.35f;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_interactable) return;
            if (iconImage != null)
            {
                iconImage.color = _hoverColor;
                iconImage.rectTransform.localScale = Vector3.one * 1.1f;
            }
            if (titleText != null)
                titleText.rectTransform.anchoredPosition = _titleBasePos + new Vector2(12f, 0f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (iconImage != null)
            {
                iconImage.color = _iconNormal;
                iconImage.rectTransform.localScale = Vector3.one;
            }
            if (titleText != null)
                titleText.rectTransform.anchoredPosition = _titleBasePos;
        }
    }
}
