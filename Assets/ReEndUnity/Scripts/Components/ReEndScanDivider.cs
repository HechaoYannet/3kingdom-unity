using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndScanDivider : ReEndBaseComponent
    {
        public float ScanDuration { get; set; } = 2f;

        private Image _line;
        private Image _scanDot;
        private float _timer;

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1);

            _line = gameObject.AddComponent<Image>();
            _line.raycastTarget = false;

            // Animated dot that moves across the line
            var dotGo = CreateChild(transform, "ScanDot");
            _scanDot = dotGo.AddComponent<Image>();
            _scanDot.raycastTarget = false;
            var dRT = _scanDot.rectTransform;
            dRT.anchorMin = new Vector2(0, 0.5f);
            dRT.sizeDelta = new Vector2(6, 12);
        }

        public override void ApplyTheme()
        {
            _line.color = Theme.primary;
            _scanDot.color = Theme.primary;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            float pct = (_timer % ScanDuration) / ScanDuration;
            _scanDot.rectTransform.anchoredPosition = new Vector2(pct * RectTransform.rect.width, 0);
        }
    }
}
