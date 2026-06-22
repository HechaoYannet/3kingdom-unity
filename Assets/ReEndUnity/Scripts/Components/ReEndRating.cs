using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndRating : ReEndBaseComponent
    {
        public int Max { get; set; } = 5;
        public float Value { get; set; }
        public bool ReadOnly { get; set; }
        public System.Action<float> OnValueChanged { get; set; }

        private Image[] _stars;
        private int _fullStars;
        private bool _hasHalf;

        protected override void BuildInternal()
        {
            _stars = new Image[Max];
            float starSize = 24;
            float gap = 4;
            for (int i = 0; i < Max; i++)
            {
                var go = CreateChild(transform, $"Star_{i}");
                _stars[i] = go.AddComponent<Image>();
                _stars[i].raycastTarget = !ReadOnly;
                var rt = _stars[i].rectTransform;
                rt.anchorMin = new Vector2(0, 0.5f);
                rt.anchorMax = new Vector2(0, 0.5f);
                rt.pivot = new Vector2(0, 0.5f);
                rt.sizeDelta = new Vector2(starSize, starSize);
                rt.anchoredPosition = new Vector2(i * (starSize + gap), 0);

                int idx = i;
                var btn = go.AddComponent<Button>();
                btn.onClick.AddListener(() =>
                {
                    Value = idx + 1;
                    UpdateStars();
                    OnValueChanged?.Invoke(Value);
                });
            }

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Max * (starSize + gap));
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, starSize);
        }

        private void UpdateStars()
        {
            _fullStars = Mathf.FloorToInt(Value);
            _hasHalf = Value - _fullStars >= 0.25f;

            for (int i = 0; i < Max; i++)
            {
                if (i < _fullStars)
                    _stars[i].color = Theme.efYellow;
                else if (i == _fullStars && _hasHalf)
                    _stars[i].color = new Color(1f, 0.83f, 0.16f, 0.5f);
                else
                    _stars[i].color = Theme.surface3;
            }
        }

        public override void ApplyTheme()
        {
            UpdateStars();
        }

        public ReEndRating SetMax(int max) { Max = max; return this; }
        public ReEndRating SetValue(float v) { Value = Mathf.Clamp(v, 0, Max); if (_built) UpdateStars(); return this; }
        public ReEndRating SetReadOnly(bool ro = true) { ReadOnly = ro; return this; }
        public ReEndRating SetOnValueChanged(System.Action<float> cb) { OnValueChanged = cb; return this; }
    }
}
