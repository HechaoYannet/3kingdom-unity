using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndSortControl : ReEndBaseComponent
    {
        public string Label { get; set; } = "Sort";
        public ReEndSortDirection Direction { get; set; } = ReEndSortDirection.Ascending;
        public System.Action<ReEndSortDirection> OnDirectionChanged { get; set; }

        private Button _btn;
        private TMP_Text _labelText;
        private TMP_Text _arrowText;

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 28);

            _btn = gameObject.AddComponent<Button>();
            _btn.onClick.AddListener(Toggle);

            _labelText = CreateChild<TextMeshProUGUI>(transform, "Label");
            _labelText.alignment = TextAlignmentOptions.Left;
            _labelText.raycastTarget = false;
            _labelText.rectTransform.offsetMin = new Vector2(4, 0);
            Stretch(_labelText.rectTransform);

            _arrowText = CreateChild<TextMeshProUGUI>(transform, "Arrow");
            _arrowText.raycastTarget = false;
            _arrowText.alignment = TextAlignmentOptions.Center;
            var aRT = _arrowText.rectTransform;
            aRT.anchorMin = new Vector2(1, 0.5f);
            aRT.anchorMax = new Vector2(1, 0.5f);
            aRT.pivot = new Vector2(1, 0.5f);
            aRT.sizeDelta = new Vector2(20, 20);
            aRT.anchoredPosition = new Vector2(0, 0);
        }

        private void Toggle()
        {
            Direction = Direction == ReEndSortDirection.Ascending ? ReEndSortDirection.Descending : ReEndSortDirection.Ascending;
            ApplyTheme();
            OnDirectionChanged?.Invoke(Direction);
        }

        public override void ApplyTheme()
        {
            _labelText.text = Label;
            _labelText.fontSize = Theme.bodySmSize;
            _labelText.color = Theme.textMuted;
            _arrowText.text = Direction == ReEndSortDirection.Ascending ? "▲" : "▼";
            _arrowText.fontSize = 10;
            _arrowText.color = Theme.primary;
        }

        public ReEndSortControl SetLabel(string l) { Label = l; if (_built) ApplyTheme(); return this; }
        public ReEndSortControl SetDirection(ReEndSortDirection d) { Direction = d; if (_built) ApplyTheme(); return this; }
        public ReEndSortControl SetOnDirectionChanged(System.Action<ReEndSortDirection> cb) { OnDirectionChanged = cb; return this; }
    }
}
