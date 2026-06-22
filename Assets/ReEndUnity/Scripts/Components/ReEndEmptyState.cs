using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndEmptyState : ReEndBaseComponent
    {
        public ReEndEmptyStatePreset Preset { get; set; } = ReEndEmptyStatePreset.NoData;
        public string Title { get; set; }
        public string Description { get; set; }
        public string ActionLabel { get; set; }
        public System.Action OnAction { get; set; }

        public TMP_Text TitleText { get; private set; }
        public TMP_Text DescText { get; private set; }
        public TMP_Text IconText { get; private set; }

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);

            // Icon
            var iconGo = CreateChild(transform, "Icon");
            IconText = iconGo.AddComponent<TextMeshProUGUI>();
            IconText.alignment = TextAlignmentOptions.Center;
            IconText.fontSize = 48;
            var iRT = IconText.rectTransform;
            iRT.anchorMin = new Vector2(0.5f, 1);
            iRT.anchorMax = new Vector2(0.5f, 1);
            iRT.sizeDelta = new Vector2(80, 56);
            iRT.anchoredPosition = new Vector2(0, -28);

            // Title
            var titleGo = CreateChild(transform, "Title");
            TitleText = titleGo.AddComponent<TextMeshProUGUI>();
            TitleText.alignment = TextAlignmentOptions.Center;
            var tRT = TitleText.rectTransform;
            tRT.anchorMin = new Vector2(0.5f, 1);
            tRT.anchorMax = new Vector2(0.5f, 1);
            tRT.sizeDelta = new Vector2(260, 28);
            tRT.anchoredPosition = new Vector2(0, -84);

            // Description
            var descGo = CreateChild(transform, "Description");
            DescText = descGo.AddComponent<TextMeshProUGUI>();
            DescText.alignment = TextAlignmentOptions.Center;
            var dRT = DescText.rectTransform;
            dRT.anchorMin = new Vector2(0.5f, 1);
            dRT.anchorMax = new Vector2(0.5f, 1);
            dRT.sizeDelta = new Vector2(260, 40);
            dRT.anchoredPosition = new Vector2(0, -116);

            // Action button built in ApplyTheme if needed
            EnsureBuilt();
            ApplyTheme();
        }

        public override void ApplyTheme()
        {
            (string icon, string title, string desc) = Preset switch
            {
                ReEndEmptyStatePreset.NoData => ("◇", "No Data", "There's nothing here yet."),
                ReEndEmptyStatePreset.NoResults => ("○", "No Results", "Try adjusting your search or filters."),
                ReEndEmptyStatePreset.Error => ("⚠", "Something Went Wrong", "Please try again later."),
                ReEndEmptyStatePreset.Maintenance => ("◆", "Under Maintenance", "We'll be back shortly."),
                ReEndEmptyStatePreset.Empty => ("◇", "Empty", "Nothing to display."),
                _ => ("◇", "", "")
            };

            IconText.text = icon;
            IconText.color = Theme.textMuted;

            TitleText.text = Title ?? title;
            TitleText.fontSize = Theme.h4Size;
            TitleText.color = Theme.textPrimary;

            DescText.text = Description ?? desc;
            DescText.fontSize = Theme.bodySmSize;
            DescText.color = Theme.textMuted;

            if (!string.IsNullOrEmpty(ActionLabel) && OnAction != null)
            {
                var existing = transform.Find("ActionBtn");
                if (existing != null) Destroy(existing.gameObject);
                var btn = ReEndUI.Button(transform, ActionLabel).SetVariant(ReEndVariant.Outline).SetSize(ReEndSize.Sm).SetOnClick(OnAction);
                btn.RectTransform.anchorMin = new Vector2(0.5f, 1);
                btn.RectTransform.anchorMax = new Vector2(0.5f, 1);
                btn.RectTransform.anchoredPosition = new Vector2(0, -170);
            }
        }

        public ReEndEmptyState SetPreset(ReEndEmptyStatePreset p) { Preset = p; if (_built) ApplyTheme(); return this; }
        public ReEndEmptyState SetTitle(string t) { Title = t; if (_built) ApplyTheme(); return this; }
        public ReEndEmptyState SetDescription(string d) { Description = d; if (_built) ApplyTheme(); return this; }
        public ReEndEmptyState SetAction(string label, System.Action cb) { ActionLabel = label; OnAction = cb; if (_built) ApplyTheme(); return this; }
    }
}
