using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndCard : ReEndBaseComponent
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Hoverable { get; set; }
        public bool Selected { get; set; }

        public TMP_Text TitleText { get; private set; }
        public TMP_Text DescText { get; private set; }
        public RectTransform ContentArea { get; private set; }

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.GetComponent<Image>() ?? gameObject.AddComponent<Image>();
            var mat = new Material(Shader.Find("ReEnd/UI/ClipCorner"));
            mat.SetFloat("_CornerSize", 12);
            BackgroundImage.material = mat;

            // Corner brackets (4 child images with CornerBracket shader)
            var bracketGo = CreateChild(transform, "Brackets");
            var bracketImg = bracketGo.AddComponent<Image>();
            bracketImg.material = new Material(Shader.Find("ReEnd/UI/CornerBracket"));
            bracketImg.raycastTarget = false;
            Stretch(bracketImg.rectTransform);

            // Title
            var titleGo = CreateChild(transform, "Title");
            TitleText = titleGo.AddComponent<TextMeshProUGUI>();
            TitleText.alignment = TextAlignmentOptions.Left;
            TitleText.raycastTarget = false;
            var tRT = TitleText.rectTransform;
            tRT.anchorMin = new Vector2(0, 1);
            tRT.anchorMax = new Vector2(1, 1);
            tRT.pivot = new Vector2(0, 1);
            tRT.sizeDelta = new Vector2(-32, 28);
            tRT.anchoredPosition = new Vector2(16, -16);

            // Description
            var descGo = CreateChild(transform, "Description");
            DescText = descGo.AddComponent<TextMeshProUGUI>();
            DescText.alignment = TextAlignmentOptions.Left;
            DescText.raycastTarget = false;
            var dRT = DescText.rectTransform;
            dRT.anchorMin = new Vector2(0, 1);
            dRT.anchorMax = new Vector2(1, 1);
            dRT.pivot = new Vector2(0, 1);
            dRT.sizeDelta = new Vector2(-32, 20);
            dRT.anchoredPosition = new Vector2(16, -48);

            // Content area (for child elements)
            var contentGo = CreateChild(transform, "Content");
            ContentArea = contentGo.GetComponent<RectTransform>();
            ContentArea.anchorMin = new Vector2(0, 0);
            ContentArea.anchorMax = new Vector2(1, 1);
            ContentArea.offsetMin = new Vector2(16, 16);
            ContentArea.offsetMax = new Vector2(-16, -8);
        }

        public override void ApplyTheme()
        {
            BackgroundImage.color = Selected ? Theme.surfaceHover : Theme.card;

            TitleText.text = Title ?? "";
            TitleText.color = Theme.textPrimary;
            TitleText.fontSize = Theme.h4Size;

            DescText.text = Description ?? "";
            DescText.color = Theme.textMuted;
            DescText.fontSize = Theme.bodySmSize;
        }

        public ReEndCard SetTitle(string t) { Title = t; if (_built) ApplyTheme(); return this; }
        public ReEndCard SetDescription(string d) { Description = d; if (_built) ApplyTheme(); return this; }
        public ReEndCard SetHoverable(bool h = true) { Hoverable = h; return this; }
        public ReEndCard SetSelected(bool s = true) { Selected = s; if (_built) ApplyTheme(); return this; }
        public ReEndCard SetWidth(float w) { return SetWidth<ReEndCard>(w); }
        public ReEndCard SetHeight(float h) { return SetHeight<ReEndCard>(h); }
    }
}
