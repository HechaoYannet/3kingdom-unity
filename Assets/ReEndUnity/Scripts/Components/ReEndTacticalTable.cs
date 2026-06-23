using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndTacticalTable : ReEndBaseComponent
    {
        public List<string> Headers { get; private set; } = new();
        public List<List<string>> Rows { get; private set; } = new();
        public int SelectedRow { get; set; } = -1;
        public System.Action<int> OnRowSelect { get; set; }
        public System.Action<int> OnRowDoubleClick { get; set; }

        private float _headerHeight = 36;
        private float _rowHeight = 32;

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.AddComponent<Image>();
            BackgroundImage.material = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
            BackgroundImage.material.SetFloat("_CornerSize", 0.04f);

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 600);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 300);
        }

        public override void ApplyTheme()
        {
            foreach (Transform t in transform) Destroy(t.gameObject);

            BackgroundImage.color = Theme.surface0;

            // Column widths
            float totalW = RectTransform.rect.width;
            float colW = totalW / Mathf.Max(Headers.Count, 1);

            // Header row
            var hGo = CreateChild(transform, "Header");
            var hBg = hGo.AddComponent<Image>();
            hBg.color = Theme.surface1;
            var hRT = hBg.rectTransform;
            hRT.anchorMin = new Vector2(0, 1);
            hRT.anchorMax = new Vector2(1, 1);
            hRT.pivot = new Vector2(0, 1);
            hRT.sizeDelta = new Vector2(0, _headerHeight);

            for (int c = 0; c < Headers.Count; c++)
            {
                var cell = CreateChild(hGo.transform, $"H_{c}");
                var txt = cell.AddComponent<TextMeshProUGUI>();
                txt.text = Headers[c];
                txt.fontSize = Theme.captionSize;
                txt.color = Theme.textMuted;
                txt.fontStyle = FontStyles.UpperCase;
                txt.alignment = TextAlignmentOptions.Left;
                txt.rectTransform.anchorMin = new Vector2(0, 0);
                txt.rectTransform.sizeDelta = new Vector2(colW, _headerHeight);
                txt.rectTransform.anchoredPosition = new Vector2(c * colW + 12, 0);
            }

            // Rows
            for (int r = 0; r < Rows.Count; r++)
            {
                int rowIdx = r;
                var rGo = CreateChild(transform, $"Row_{r}");
                bool selected = r == SelectedRow;

                var rBg = rGo.AddComponent<Image>();
                rBg.color = selected ? Theme.surfaceHover : (r % 2 == 0 ? Theme.surface0 : Theme.surface1);
                rBg.raycastTarget = true;

                var btn = rGo.AddComponent<Button>();
                btn.onClick.AddListener(() =>
                {
                    SelectedRow = rowIdx;
                    ApplyTheme();
                    OnRowSelect?.Invoke(rowIdx);
                });

                var rRT = rBg.rectTransform;
                rRT.anchorMin = new Vector2(0, 1);
                rRT.anchorMax = new Vector2(1, 1);
                rRT.pivot = new Vector2(0, 1);
                rRT.sizeDelta = new Vector2(0, _rowHeight);
                rRT.anchoredPosition = new Vector2(0, -(_headerHeight + r * _rowHeight));

                for (int c = 0; c < Mathf.Min(Rows[r].Count, Headers.Count); c++)
                {
                    var cell = CreateChild(rGo.transform, $"C_{c}");
                    var txt = cell.AddComponent<TextMeshProUGUI>();
                    txt.text = Rows[r][c];
                    txt.fontSize = Theme.bodySmSize;
                    txt.color = selected ? Theme.textPrimary : Theme.textSecondary;
                    txt.alignment = TextAlignmentOptions.Left;
                    txt.rectTransform.anchorMin = new Vector2(0, 0);
                    txt.rectTransform.sizeDelta = new Vector2(colW, _rowHeight);
                    txt.rectTransform.anchoredPosition = new Vector2(c * colW + 12, 0);
                }
            }
        }

        public ReEndTacticalTable SetHeaders(List<string> h) { Headers = h; return this; }
        public ReEndTacticalTable AddRow(List<string> row) { Rows.Add(row); return this; }
        public ReEndTacticalTable SetRows(List<List<string>> rows) { Rows = rows; return this; }
        public ReEndTacticalTable SetOnRowSelect(System.Action<int> cb) { OnRowSelect = cb; return this; }
    }
}
