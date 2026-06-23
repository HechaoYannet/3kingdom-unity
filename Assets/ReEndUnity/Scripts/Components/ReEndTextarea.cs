using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndTextarea : ReEndBaseComponent
    {
        public string Value { get; set; } = "";
        public string Placeholder { get; set; } = "Enter text...";
        public ReEndInputState State { get; set; } = ReEndInputState.Default;
        public int MaxLength { get; set; } = 500;
        public bool ShowCharCount { get; set; } = true;

        public TMP_InputField InputField { get; private set; }
        public TMP_Text TextComponent { get; private set; }
        public TMP_Text PlaceholderText { get; private set; }
        public TMP_Text CharCountText { get; private set; }

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.GetComponent<Image>() ?? gameObject.AddComponent<Image>();
            var mat = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
            mat.SetFloat("_CornerSize", 0.06f);
            BackgroundImage.material = mat;

            var textGo = CreateChild(transform, "Text");
            TextComponent = textGo.AddComponent<TextMeshProUGUI>();
            TextComponent.alignment = TextAlignmentOptions.TopLeft;
            TextComponent.raycastTarget = false;
            Stretch(TextComponent.rectTransform);

            var phGo = CreateChild(transform, "Placeholder");
            PlaceholderText = phGo.AddComponent<TextMeshProUGUI>();
            PlaceholderText.alignment = TextAlignmentOptions.TopLeft;
            PlaceholderText.fontStyle = FontStyles.Italic;
            PlaceholderText.raycastTarget = false;
            Stretch(PlaceholderText.rectTransform);

            var countGo = CreateChild(transform, "CharCount");
            CharCountText = countGo.AddComponent<TextMeshProUGUI>();
            CharCountText.alignment = TextAlignmentOptions.Right;
            var cRT = CharCountText.rectTransform;
            cRT.anchorMin = new Vector2(1, 0);
            cRT.anchorMax = new Vector2(1, 0);
            cRT.pivot = new Vector2(1, 0);
            cRT.sizeDelta = new Vector2(80, 20);
            cRT.anchoredPosition = new Vector2(-4, 4);

            InputField = gameObject.AddComponent<TMP_InputField>();
            InputField.textComponent = TextComponent;
            InputField.placeholder = PlaceholderText;
            InputField.characterLimit = MaxLength;
            InputField.lineType = TMP_InputField.LineType.MultiLineNewline;
            InputField.onValueChanged.AddListener(v => { Value = v; UpdateCharCount(); });
        }

        private void UpdateCharCount()
        {
            CharCountText.text = $"{Value.Length}/{MaxLength}";
        }

        public override void ApplyTheme()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 120f);

            BackgroundImage.color = Theme.surface2;
            var pad = Theme.GetSpace(3);
            TextComponent.rectTransform.offsetMin = new Vector2(pad, pad + 20);
            TextComponent.rectTransform.offsetMax = new Vector2(-pad, -pad);
            PlaceholderText.rectTransform.offsetMin = new Vector2(pad, pad + 20);
            PlaceholderText.rectTransform.offsetMax = new Vector2(-pad, -pad);

            TextComponent.fontSize = Theme.bodySize;
            TextComponent.color = Theme.textPrimary;
            PlaceholderText.fontSize = Theme.bodySize;
            PlaceholderText.text = Placeholder;
            PlaceholderText.color = Theme.textPlaceholder;

            CharCountText.fontSize = Theme.captionSize;
            CharCountText.color = Theme.textMuted;
            UpdateCharCount();

            InputField.interactable = State != ReEndInputState.Disabled;
        }

        public ReEndTextarea SetText(string text) { Value = text; if (_built) { InputField.text = text; UpdateCharCount(); } return this; }
        public ReEndTextarea SetPlaceholder(string ph) { Placeholder = ph; if (_built) ApplyTheme(); return this; }
        public ReEndTextarea SetState(ReEndInputState s) { State = s; if (_built) ApplyTheme(); return this; }
        public ReEndTextarea SetMaxLength(int max) { MaxLength = max; if (_built) { InputField.characterLimit = max; } return this; }
    }
}
