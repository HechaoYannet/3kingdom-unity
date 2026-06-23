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
        private List<TMP_Text> _labels = new();
        private List<Image> _tabBgs = new();
        private List<Button> _btns = new();
        private Image _underline;
        private bool _firstBuild = true;

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 44);

            // Underline indicator for Underline variant
            var ulGo = CreateChild(transform, "Underline");
            _underline = ulGo.AddComponent<Image>();
            _underline.raycastTarget = false;
            var ulRT = _underline.rectTransform;
            ulRT.anchorMin = new Vector2(0, 0);
            ulRT.anchorMax = new Vector2(0, 0);
            ulRT.pivot = new Vector2(0, 0);
            ulRT.sizeDelta = new Vector2(0, 2);
        }

        public override void ApplyTheme()
        {
            float totalW = RectTransform.rect.width;
            if (totalW <= 0) totalW = 400;
            float tabW = totalW / Mathf.Max(TabLabels.Count, 1);

            // First build: create children. Subsequent: update only
            if (_firstBuild)
            {
                foreach (var old in _btns) Destroy(old.gameObject);
                _btns.Clear();
                _labels.Clear();
                _tabBgs.Clear();

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

                    // Image for raycast target (required for Button to receive clicks)
                    var img = go.AddComponent<Image>();
                    img.color = Color.clear;
                    img.raycastTarget = true;
                    _tabBgs.Add(img);

                    var txt = go.AddComponent<TextMeshProUGUI>();
                    txt.text = TabLabels[i];
                    txt.alignment = TextAlignmentOptions.Center;
                    txt.fontSize = Theme.bodySmSize;
                    txt.color = i == ActiveIndex ? Theme.textPrimary : Theme.textMuted;
                    txt.raycastTarget = false;
                    Stretch(txt.rectTransform);
                    _labels.Add(txt);

                    var btn = go.AddComponent<Button>();
                    btn.onClick.AddListener(() =>
                    {
                        ActiveIndex = idx;
                        UpdateTabColors();
                        UpdateUnderline(tabW);
                        ApplyVariantStyling(tabW);
                        OnTabChanged?.Invoke(idx);
                    });
                    _btns.Add(btn);
                }

                _firstBuild = false;
            }
            else
            {
                // Update existing tab texts
                for (int i = 0; i < _labels.Count && i < TabLabels.Count; i++)
                {
                    _labels[i].text = TabLabels[i];
                }
            }

            UpdateTabColors();
            UpdateUnderline(tabW);
            ApplyVariantStyling(tabW);
        }

        private void UpdateTabColors()
        {
            for (int i = 0; i < _labels.Count; i++)
            {
                _labels[i].color = i == ActiveIndex ? Theme.textPrimary : Theme.textMuted;
            }
        }

        private void UpdateUnderline(float tabW)
        {
            _underline.color = Theme.primary;
            var uRT = _underline.rectTransform;
            uRT.sizeDelta = new Vector2(tabW, 2);
            uRT.anchoredPosition = new Vector2(ActiveIndex * tabW, 0);
        }

        private void ApplyVariantStyling(float tabW)
        {
            _underline.gameObject.SetActive(Variant == ReEndTabsVariant.Underline);

            if (Variant == ReEndTabsVariant.Pill)
            {
                for (int i = 0; i < _tabBgs.Count; i++)
                {
                    _tabBgs[i].color = i == ActiveIndex ? Theme.primary : Color.clear;
                    _tabBgs[i].raycastTarget = true;
                }
            }
            else if (Variant == ReEndTabsVariant.Bordered)
            {
                for (int i = 0; i < _tabBgs.Count; i++)
                {
                    _tabBgs[i].color = i == ActiveIndex ? new Color(1, 1, 1, 0.06f) : Color.clear;
                    _tabBgs[i].raycastTarget = true;
                }
            }
            else
            {
                // Underline / default: transparent backgrounds, still raycastable
                for (int i = 0; i < _tabBgs.Count; i++)
                {
                    _tabBgs[i].color = Color.clear;
                    _tabBgs[i].raycastTarget = true;
                }
            }
        }

        public ReEndTabs SetTabs(List<string> tabs) { TabLabels = tabs; _firstBuild = true; return this; }
        public ReEndTabs SetVariant(ReEndTabsVariant v) { Variant = v; if (_built) ApplyTheme(); return this; }
        public ReEndTabs SetActiveIndex(int i) { ActiveIndex = i; if (_built) ApplyTheme(); return this; }
        public ReEndTabs SetOnTabChanged(System.Action<int> cb) { OnTabChanged = cb; return this; }
    }
}
