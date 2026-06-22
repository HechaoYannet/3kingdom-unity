using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndRadarChart : ReEndBaseComponent
    {
        public List<RadarAxis> Axes { get; private set; } = new();

        [System.Serializable]
        public class RadarAxis
        {
            public string Label;
            public float Value; // 0-100
        }

        private Image _plotArea;
        private List<Image> _axisLines = new();
        private List<TMP_Text> _labels = new();

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);

            _plotArea = gameObject.AddComponent<Image>();
            _plotArea.color = new Color(0, 0, 0, 0);
            _plotArea.raycastTarget = false;
        }

        public override void ApplyTheme()
        {
            foreach (var l in _axisLines) Destroy(l.gameObject);
            foreach (var l in _labels) Destroy(l.gameObject);
            _axisLines.Clear();
            _labels.Clear();

            int n = Axes.Count;
            if (n < 3) return;

            float cx = RectTransform.rect.width * 0.5f;
            float cy = RectTransform.rect.height * 0.5f;
            float r = Mathf.Min(cx, cy) - 30;

            // Draw axis lines from center
            for (int i = 0; i < n; i++)
            {
                float angle = Mathf.PI * 0.5f - (i * 2f * Mathf.PI / n);
                float ex = cx + r * Mathf.Cos(angle);
                float ey = cy + r * Mathf.Sin(angle);

                var lineGo = CreateChild(transform, $"Axis_{i}");
                var lineImg = lineGo.AddComponent<Image>();
                lineImg.color = Theme.borderDefault;
                lineImg.raycastTarget = false;

                // Place and rotate a thin rect as axis line
                var lRT = lineImg.rectTransform;
                lRT.anchorMin = new Vector2(0.5f, 0.5f);
                lRT.sizeDelta = new Vector2(r, 1);
                lRT.anchoredPosition = Vector2.zero;
                lRT.localRotation = Quaternion.Euler(0, 0, -i * 360f / n);
                lRT.pivot = new Vector2(0, 0.5f);

                _axisLines.Add(lineImg);

                // Value point
                float val = Mathf.Clamp01(Axes[i].Value / 100f);
                float px = cx + r * val * Mathf.Cos(angle);
                float py = cy + r * val * Mathf.Sin(angle);

                var dotGo = CreateChild(transform, $"Dot_{i}");
                var dotImg = dotGo.AddComponent<Image>();
                dotImg.color = Theme.primary;
                dotImg.raycastTarget = false;
                dotImg.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                dotImg.rectTransform.sizeDelta = new Vector2(6, 6);
                dotImg.rectTransform.anchoredPosition = new Vector2(px - cx, py - cy);

                // Label
                float lx = cx + (r + 20) * Mathf.Cos(angle);
                float ly = cy + (r + 20) * Mathf.Sin(angle);

                var labGo = CreateChild(transform, $"Label_{i}");
                var labTxt = labGo.AddComponent<TextMeshProUGUI>();
                labTxt.text = Axes[i].Label;
                labTxt.fontSize = Theme.captionSize;
                labTxt.color = Theme.textMuted;
                labTxt.alignment = TextAlignmentOptions.Center;
                labTxt.raycastTarget = false;
                labTxt.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                labTxt.rectTransform.sizeDelta = new Vector2(60, 18);
                labTxt.rectTransform.anchoredPosition = new Vector2(lx - cx, ly - cy);
                _labels.Add(labTxt);
            }
        }

        public ReEndRadarChart AddAxis(string label, float value) { Axes.Add(new RadarAxis { Label = label, Value = value }); return this; }
        public ReEndRadarChart SetSize(float w, float h) { return SetSize<ReEndRadarChart>(w, h); }
    }
}
