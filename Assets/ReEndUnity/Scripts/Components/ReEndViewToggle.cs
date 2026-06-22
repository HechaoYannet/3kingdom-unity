using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndViewToggle : ReEndBaseComponent
    {
        public ReEndViewMode Mode { get; set; } = ReEndViewMode.Grid;
        public System.Action<ReEndViewMode> OnModeChanged { get; set; }

        private Button _gridBtn;
        private Button _listBtn;
        private TMP_Text _gridLabel;
        private TMP_Text _listLabel;

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 80);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 32);

            var gridGo = CreateChild(transform, "Grid");
            var gridImg = gridGo.AddComponent<Image>();
            _gridLabel = CreateChild<TextMeshProUGUI>(gridGo.transform, "Label");
            _gridLabel.text = "▦";
            _gridLabel.alignment = TextAlignmentOptions.Center;
            _gridLabel.fontSize = 16;
            Stretch(_gridLabel.rectTransform);
            _gridBtn = gridGo.AddComponent<Button>();
            _gridBtn.onClick.AddListener(() => SetMode(ReEndViewMode.Grid));
            gridGo.GetComponent<RectTransform>().sizeDelta = new Vector2(36, 32);

            var listGo = CreateChild(transform, "List");
            var listImg = listGo.AddComponent<Image>();
            _listLabel = CreateChild<TextMeshProUGUI>(listGo.transform, "Label");
            _listLabel.text = "≡";
            _listLabel.alignment = TextAlignmentOptions.Center;
            _listLabel.fontSize = 16;
            Stretch(_listLabel.rectTransform);
            _listBtn = listGo.AddComponent<Button>();
            _listBtn.onClick.AddListener(() => SetMode(ReEndViewMode.List));
            var lRT = listGo.GetComponent<RectTransform>();
            lRT.anchorMin = new Vector2(0, 0);
            lRT.sizeDelta = new Vector2(36, 32);
            lRT.anchoredPosition = new Vector2(40, 0);
        }

        private void SetMode(ReEndViewMode m) { Mode = m; ApplyTheme(); OnModeChanged?.Invoke(m); }

        public override void ApplyTheme()
        {
            _gridLabel.color = Mode == ReEndViewMode.Grid ? Theme.primary : Theme.textMuted;
            _listLabel.color = Mode == ReEndViewMode.List ? Theme.primary : Theme.textMuted;
        }

        public ReEndViewToggle SetOnModeChanged(System.Action<ReEndViewMode> cb) { OnModeChanged = cb; return this; }
    }
}
