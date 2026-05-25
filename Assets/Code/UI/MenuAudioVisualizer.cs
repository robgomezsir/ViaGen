using UnityEngine;
using UnityEngine.UI;
using ViaGen.Core;

namespace ViaGen.UI
{
    public class MenuAudioVisualizer : MonoBehaviour
    {
        [SerializeField] private Image[] bars;
        [SerializeField] private int barCount = 24;
        [SerializeField] private float sensitivity = 120f;

        private float[] _samples = new float[64];

        private void Awake()
        {
            if (bars != null && bars.Length > 0) return;
            var parent = transform;
            bars = new Image[barCount];
            for (var i = 0; i < barCount; i++)
            {
                var go = new GameObject($"Bar_{i}");
                go.transform.SetParent(parent, false);
                var rect = go.AddComponent<RectTransform>();
                rect.sizeDelta = new Vector2(4f, 8f);
                rect.anchoredPosition = new Vector2(i * 6f - barCount * 3f, 0f);
                bars[i] = go.AddComponent<Image>();
                bars[i].color = new Color(0.4f, 0.85f, 1f, 0.7f);
            }
        }

        private void Update()
        {
            var source = AudioManager.Instance != null ? AudioManager.Instance.MusicSource : null;
            if (source == null || !source.isPlaying)
            {
                AnimateIdle();
                return;
            }

            source.GetOutputData(_samples, 0);
            for (var i = 0; i < bars.Length; i++)
            {
                var idx = Mathf.Clamp(i * 2, 0, _samples.Length - 1);
                var h = 8f + Mathf.Abs(_samples[idx]) * sensitivity;
                bars[i].rectTransform.sizeDelta = new Vector2(4f, h);
            }
        }

        private void AnimateIdle()
        {
            for (var i = 0; i < bars.Length; i++)
            {
                var h = 6f + Mathf.Sin(Time.time * 3f + i * 0.35f) * 4f;
                bars[i].rectTransform.sizeDelta = new Vector2(4f, h);
            }
        }
    }
}
