using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndFrequencyBars : ReEndBaseComponent
    {
        public int BarCount { get; set; } = 16;
        public float UpdateSpeed { get; set; } = 8f;
        public float MaxHeight { get; set; } = 60;

        private Image[] _bars;
        private float[] _targets;
        private float[] _current;

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, BarCount * 8);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, MaxHeight);

            _bars = new Image[BarCount];
            _targets = new float[BarCount];
            _current = new float[BarCount];

            for (int i = 0; i < BarCount; i++)
            {
                var go = CreateChild(transform, $"Bar_{i}");
                _bars[i] = go.AddComponent<Image>();
                _bars[i].raycastTarget = false;
                _bars[i].rectTransform.anchorMin = new Vector2(0, 0);
                _bars[i].rectTransform.anchorMax = new Vector2(0, 0);
                _bars[i].rectTransform.pivot = new Vector2(0, 0);
                _bars[i].rectTransform.sizeDelta = new Vector2(6, 10);
                _bars[i].rectTransform.anchoredPosition = new Vector2(i * 8, 0);

                _targets[i] = Random.Range(0.1f, 1f);
                _current[i] = 0.5f;
            }
        }

        public override void ApplyTheme()
        {
            for (int i = 0; i < BarCount; i++)
            {
                float gradient = (float)i / BarCount;
                _bars[i].color = Color.Lerp(Theme.efGreen, Theme.efYellow, gradient);
            }
        }

        private void Update()
        {
            for (int i = 0; i < BarCount; i++)
            {
                _current[i] = Mathf.Lerp(_current[i], _targets[i], Time.deltaTime * UpdateSpeed);

                float h = _current[i] * MaxHeight;
                _bars[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);

                if (Mathf.Abs(_current[i] - _targets[i]) < 0.02f)
                    _targets[i] = Random.Range(0.1f, 1f);
            }
        }

        public ReEndFrequencyBars SetBarCount(int count) { BarCount = count; return this; }
    }
}
