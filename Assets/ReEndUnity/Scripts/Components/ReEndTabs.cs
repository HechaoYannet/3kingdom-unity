using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndTabs : ReEndBaseComponent
    {
        public ReEndTabsVariant Variant { get; set; } = ReEndTabsVariant.Underline;
        public int ActiveIndex { get; set; }
        public System.Action<int> OnTabChanged { get; set; }

        public List<string> TabLabels { get; private set; } = new();
        private List<Button> _btns = new();
        private Image _underline;

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 44);
        }

        public override void ApplyTheme()
        {
            foreach (var b in _btns) Destroy(b.gameObject);
            _btns.Clear();

            float totalW = RectTransform.rect.width;
            if (totalW <= 0) totalW = 400; // 在首次构建时 rect.width 可能为 0，使用默认宽度
            float tabW = totalW / Mathf.Max(TabLabels.Count, 1);

            for (int i = 0; i < TabLabels.Count; i++)
            {
                int idx = i;
                var go = CreateChild(transform, $"Tab_{i}");
                var rt = go.GetComponent<RectTransform>();
                rt.anchorMin = new Vector2(0, 0);
                rt.anchorMax = new Vector2(0, 1);
                rt.pivot = new Vector2(0, 0);
                rt.sizeDelta = new Vector2(tabW, 0);
                rt.anchoredPosition = new Vector2(i * tabW, 0);

                var txt = go.AddComponent<TextMeshProUGUI>();
                txt.text = TabLabels[i];
                txt.alignment = TextAlignmentOptions.Center;
                txt.fontSize = Theme.bodySmSize;
                txt.color = i == ActiveIndex ? Theme.textPrimary : Theme.textMuted;
                txt.raycastTarget = false;
                Stretch(txt.rectTransform);

                var btn = go.AddComponent<Button>();
                btn.onClick.AddListener(() =>
                {
                    ActiveIndex = idx;
                    ApplyTheme();
                    OnTabChanged?.Invoke(idx);
                });
                _btns.Add(btn);
            }
        }

        public ReEndTabs SetTabs(List<string> tabs) { TabLabels = tabs; return this; }
        public ReEndTabs SetVariant(ReEndTabsVariant v) { Variant = v; return this; }
        public ReEndTabs SetActiveIndex(int i) { ActiveIndex = i; if (_built) ApplyTheme(); return this; }
        public ReEndTabs SetOnTabChanged(System.Action<int> cb) { OnTabChanged = cb; return this; }
    }
}
