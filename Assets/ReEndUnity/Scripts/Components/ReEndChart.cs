using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndChart : ReEndBaseComponent
    {
        public ReEndChartType ChartType { get; set; } = ReEndChartType.Line;
        public List<float> Data { get; private set; } = new();
        public List<string> Labels { get; private set; } = new();
        public List<Color> ColorsOverride { get; set; }

        private Image _plotArea;
        private List<Image> _segments = new();
        private List<TMP_Text> _labelObjs = new();

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 240);

            _plotArea = gameObject.AddComponent<Image>();
            _plotArea.color = new Color(0, 0, 0, 0);
            _plotArea.raycastTarget = false;
        }

        public override void ApplyTheme()
        {
            foreach (var s in _segments) Destroy(s.gameObject);
            _segments.Clear();

            if (Data.Count == 0) return;

            float max = Mathf.Max(Data.ToArray());
            float w = RectTransform.rect.width;
            float h = RectTransform.rect.height;

            switch (ChartType)
            {
                case ReEndChartType.Bar:
                    DrawBars(w, h, max);
                    break;
                case ReEndChartType.Pie:
                    DrawPie(w, h);
                    break;
                default:
                    DrawBars(w, h, max); // fallback
                    break;
            }
        }

        private void DrawBars(float w, float h, float max)
        {
            float barW = (w - (Data.Count - 1) * 4) / Data.Count;
            for (int i = 0; i < Data.Count; i++)
            {
                var go = CreateChild(transform, $"Bar_{i}");
                var img = go.AddComponent<Image>();
                img.raycastTarget = false;
                float barH = (Data[i] / max) * (h - 20);
                var rt = img.rectTransform;
                rt.anchorMin = new Vector2(0, 0);
                rt.anchorMax = new Vector2(0, 0);
                rt.pivot = new Vector2(0, 0);
                rt.sizeDelta = new Vector2(barW, barH);
                rt.anchoredPosition = new Vector2(i * (barW + 4), 0);

                img.color = (ColorsOverride != null && i < ColorsOverride.Count)
                    ? ColorsOverride[i] : Theme.chart1;
                _segments.Add(img);
            }
        }

        private void DrawPie(float w, float h)
        {
            // Simplified pie chart: concentric colored rings or abar segments
            float total = 0;
            foreach (var d in Data) total += d;
            if (total <= 0) return;

            float centerX = w * 0.5f;
            float centerY = h * 0.5f;
            float radius = Mathf.Min(w, h) * 0.4f;
            float angle = 0;

            for (int i = 0; i < Data.Count; i++)
            {
                float slice = (Data[i] / total) * 360f;
                var go = CreateChild(transform, $"Slice_{i}");
                var img = go.AddComponent<Image>();
                img.raycastTarget = false;
                img.fillMethod = Image.FillMethod.Radial360;
                img.fillOrigin = 0; // Top
                img.fillAmount = slice / 360f;
                img.color = (ColorsOverride != null && i < ColorsOverride.Count)
                    ? ColorsOverride[i] : GetChartColor(i);
                var rt = img.rectTransform;
                rt.anchorMin = new Vector2(0.5f, 0.5f);
                rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.sizeDelta = new Vector2(radius * 2, radius * 2);
                rt.localRotation = Quaternion.Euler(0, 0, -angle);
                angle += slice;
                _segments.Add(img);
            }
        }

        private Color GetChartColor(int i)
        {
            return (i % 8) switch
            {
                0 => Theme.chart1, 1 => Theme.chart2, 2 => Theme.chart3, 3 => Theme.chart4,
                4 => Theme.chart5, 5 => Theme.chart6, 6 => Theme.chart7, _ => Theme.chart8
            };
        }

        public ReEndChart SetChartType(ReEndChartType t) { ChartType = t; return this; }
        public ReEndChart SetData(List<float> data) { Data = data; return this; }
        public ReEndChart SetLabels(List<string> labels) { Labels = labels; return this; }
        public ReEndChart AddData(float v, string label = null) { Data.Add(v); if (label != null) Labels.Add(label); return this; }
    }
}
