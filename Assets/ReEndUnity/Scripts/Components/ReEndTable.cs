using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndTable : ReEndBaseComponent
    {
        public List<string> Headers { get; private set; } = new();
        public List<List<string>> Rows { get; private set; } = new();
        public bool Sortable { get; set; }
        public bool Striped { get; set; } = true;
        public int SortColumn { get; set; } = -1;
        public bool SortAscending { get; set; } = true;
        public System.Action<int, bool> OnSort { get; set; }

        private GameObject _headerRow;
        private GameObject _bodyContainer;

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 600);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 300);

            _headerRow = CreateChild(transform, "Header");
            var hRT = _headerRow.GetComponent<RectTransform>();
            hRT.anchorMin = new Vector2(0, 1);
            hRT.anchorMax = new Vector2(1, 1);
            hRT.pivot = new Vector2(0, 1);
            hRT.sizeDelta = new Vector2(0, 40);

            _bodyContainer = CreateChild(transform, "Body");
            var bRT = _bodyContainer.GetComponent<RectTransform>();
            bRT.anchorMin = new Vector2(0, 0);
            bRT.anchorMax = new Vector2(1, 1);
            bRT.offsetMin = new Vector2(0, 0);
            bRT.offsetMax = new Vector2(0, -44);
        }

        public override void ApplyTheme()
        {
            // Headers
            foreach (Transform t in _headerRow.transform) Destroy(t.gameObject);
            float colW = RectTransform.rect.width / Mathf.Max(Headers.Count, 1);
            for (int i = 0; i < Headers.Count; i++)
            {
                int idx = i;
                var go = CreateChild(_headerRow.transform, $"H_{i}");
                var txt = go.AddComponent<TextMeshProUGUI>();
                txt.text = Headers[i];
                txt.alignment = TextAlignmentOptions.Left;
                txt.fontSize = Theme.captionSize;
                txt.color = Theme.textMuted;
                txt.fontStyle = FontStyles.UpperCase;
                var rt = go.GetComponent<RectTransform>();
                rt.anchorMin = new Vector2(0, 0);
                rt.anchorMax = new Vector2(0, 1);
                rt.pivot = new Vector2(0, 0);
                rt.sizeDelta = new Vector2(colW, 0);
                rt.anchoredPosition = new Vector2(i * colW + 8, 0);

                if (Sortable)
                {
                    var btn = go.AddComponent<Button>();
                    btn.onClick.AddListener(() =>
                    {
                        if (SortColumn == idx) SortAscending = !SortAscending;
                        else { SortColumn = idx; SortAscending = true; }
                        ApplyTheme();
                        OnSort?.Invoke(SortColumn, SortAscending);
                    });
                }
            }

            // Rows
            foreach (Transform t in _bodyContainer.transform) Destroy(t.gameObject);
            for (int r = 0; r < Rows.Count; r++)
            {
                var rowGo = CreateChild(_bodyContainer.transform, $"Row_{r}");
                var rowRT = rowGo.GetComponent<RectTransform>();
                rowRT.anchorMin = new Vector2(0, 1);
                rowRT.anchorMax = new Vector2(1, 1);
                rowRT.pivot = new Vector2(0, 1);
                rowRT.sizeDelta = new Vector2(0, 32);
                rowRT.anchoredPosition = new Vector2(0, -(r * 32 + 4));

                if (Striped && r % 2 == 1)
                {
                    var bg = rowGo.AddComponent<Image>();
                    bg.color = Theme.surface1;
                }

                for (int c = 0; c < Mathf.Min(Rows[r].Count, Headers.Count); c++)
                {
                    var cellGo = CreateChild(rowGo.transform, $"C_{c}");
                    var txt = cellGo.AddComponent<TextMeshProUGUI>();
                    txt.text = Rows[r][c];
                    txt.alignment = TextAlignmentOptions.Left;
                    txt.fontSize = Theme.bodySmSize;
                    txt.color = Theme.textPrimary;
                    var rt = cellGo.GetComponent<RectTransform>();
                    rt.anchorMin = new Vector2(0, 0);
                    rt.anchorMax = new Vector2(0, 1);
                    rt.pivot = new Vector2(0, 0);
                    rt.sizeDelta = new Vector2(colW, 0);
                    rt.anchoredPosition = new Vector2(c * colW + 8, 0);
                }
            }
        }

        public ReEndTable SetHeaders(List<string> h) { Headers = h; return this; }
        public ReEndTable AddRow(List<string> row) { Rows.Add(row); return this; }
        public ReEndTable SetRows(List<List<string>> rows) { Rows = rows; return this; }
        public ReEndTable SetSortable(bool s = true) { Sortable = s; return this; }
        public ReEndTable SetOnSort(System.Action<int, bool> cb) { OnSort = cb; return this; }
    }
}
