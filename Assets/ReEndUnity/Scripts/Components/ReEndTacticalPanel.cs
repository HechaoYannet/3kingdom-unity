using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndTacticalPanel : ReEndBaseComponent
    {
        public string Title { get; set; } = "PANEL";
        public ReEndStatus Status { get; set; } = ReEndStatus.Online;

        public TMP_Text TitleText { get; private set; }
        public Image StatusDot { get; private set; }
        public RectTransform ContentSlot { get; private set; }
        private Image _brackets;
        private Image _scanline;

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.GetComponent<Image>() ?? gameObject.AddComponent<Image>();
            BackgroundImage.color = Theme.card;
            var mat = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
            mat.SetFloat("_CornerSize", 0.12f);
            BackgroundImage.material = mat;

            // Corner brackets
            var bracketGo = CreateChild(transform, "Brackets");
            _brackets = bracketGo.AddComponent<Image>();
            _brackets.material = GetOrCreateMaterial("ReEnd/UI/CornerBracket");
            _brackets.material.SetFloat("_BracketSize", 0.15f);
            _brackets.material.SetFloat("_BracketWidth", 0.01f);
            _brackets.material.SetColor("_BracketColor", Theme.bracketColor);
            _brackets.raycastTarget = false;
            Stretch(_brackets.rectTransform);

            // Scanline
            var scanGo = CreateChild(transform, "Scanline");
            _scanline = scanGo.AddComponent<Image>();
            _scanline.material = GetOrCreateMaterial("ReEnd/UI/Scanline");
            _scanline.material.SetFloat("_ScanlineOpacity", 0.015f);
            _scanline.material.SetFloat("_ScanlineSpacing", 120);
            _scanline.raycastTarget = false;
            Stretch(_scanline.rectTransform);

            // Header
            var headerGo = CreateChild(transform, "Header");
            var hRT = headerGo.GetComponent<RectTransform>();
            hRT.anchorMin = new Vector2(0, 1);
            hRT.anchorMax = new Vector2(1, 1);
            hRT.pivot = new Vector2(0, 1);
            hRT.sizeDelta = new Vector2(0, 36);
            hRT.anchoredPosition = Vector2.zero;

            // Title
            TitleText = CreateChild<TextMeshProUGUI>(headerGo.transform, "Title");
            TitleText.alignment = TextAlignmentOptions.Left;
            TitleText.raycastTarget = false;
            var tRT = TitleText.rectTransform;
            Stretch(tRT);
            tRT.offsetMin = new Vector2(16, 0);
            tRT.offsetMax = new Vector2(-40, 0);

            // Status dot
            var dotGo = CreateChild(headerGo.transform, "StatusDot");
            StatusDot = dotGo.AddComponent<Image>();
            var dRT = StatusDot.rectTransform;
            dRT.anchorMin = new Vector2(1, 0.5f);
            dRT.anchorMax = new Vector2(1, 0.5f);
            dRT.pivot = new Vector2(1, 0.5f);
            dRT.sizeDelta = new Vector2(8, 8);
            dRT.anchoredPosition = new Vector2(-12, 0);

            // Content
            var contentGo = CreateChild(transform, "Content");
            ContentSlot = contentGo.GetComponent<RectTransform>();
            ContentSlot.anchorMin = new Vector2(0, 0);
            ContentSlot.anchorMax = new Vector2(1, 1);
            ContentSlot.offsetMin = new Vector2(12, 8);
            ContentSlot.offsetMax = new Vector2(-12, -40);

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 280);
        }

        public override void ApplyTheme()
        {
            BackgroundImage.color = Theme.card;
            TitleText.text = Title.ToUpper();
            TitleText.fontSize = Theme.overlineSize;
            TitleText.color = Theme.textMuted;

            StatusDot.color = Status switch
            {
                ReEndStatus.Online => Theme.efGreen,
                ReEndStatus.Offline => Theme.efRed,
                ReEndStatus.Warning => Theme.efYellow,
                ReEndStatus.Danger => Theme.efRed,
                ReEndStatus.Scanning => Theme.efCyan,
                _ => Theme.primary
            };
        }

        public ReEndTacticalPanel SetTitle(string t) { Title = t; if (_built) ApplyTheme(); return this; }
        public ReEndTacticalPanel SetStatus(ReEndStatus s) { Status = s; if (_built) ApplyTheme(); return this; }
        public ReEndTacticalPanel SetWidth(float w) { return SetWidth<ReEndTacticalPanel>(w); }
        public ReEndTacticalPanel SetHeight(float h) { return SetHeight<ReEndTacticalPanel>(h); }
    }
}
