using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndInput : ReEndBaseComponent
    {
        public string Value { get; set; } = "";
        public string Placeholder { get; set; } = "Enter text...";
        public ReEndInputState State { get; set; } = ReEndInputState.Default;
        public ReEndSize Size { get; set; } = ReEndSize.Md;

        public TMP_InputField InputField { get; private set; }
        public TMP_Text TextComponent { get; private set; }
        public TMP_Text PlaceholderText { get; private set; }

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.GetComponent<Image>() ?? gameObject.AddComponent<Image>();

            // Text area
            var textGo = CreateChild(transform, "Text");
            TextComponent = textGo.AddComponent<TextMeshProUGUI>();
            TextComponent.raycastTarget = false;
            TextComponent.alignment = TextAlignmentOptions.Left;

            // Placeholder
            var phGo = CreateChild(transform, "Placeholder");
            PlaceholderText = phGo.AddComponent<TextMeshProUGUI>();
            PlaceholderText.raycastTarget = false;
            PlaceholderText.alignment = TextAlignmentOptions.Left;
            PlaceholderText.fontStyle = FontStyles.Italic;

            // Input field
            InputField = gameObject.AddComponent<TMP_InputField>();
            InputField.textViewport = RectTransform;
            InputField.textComponent = TextComponent;
            InputField.placeholder = PlaceholderText;
            InputField.onValueChanged.AddListener(v => Value = v);

            Stretch(TextComponent.rectTransform);
            Stretch(PlaceholderText.rectTransform);
        }

        public override void ApplyTheme()
        {
            float h = Theme.GetSizeValue(Size);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);

            var pad = Theme.GetSpace(3);
            TextComponent.rectTransform.offsetMin = new Vector2(pad, 0);
            TextComponent.rectTransform.offsetMax = new Vector2(-pad, 0);
            PlaceholderText.rectTransform.offsetMin = new Vector2(pad, 0);
            PlaceholderText.rectTransform.offsetMax = new Vector2(-pad, 0);

            BackgroundImage.color = Theme.surface2;
            TextComponent.fontSize = Theme.GetFontSize(Size);
            TextComponent.color = Theme.textPrimary;
            PlaceholderText.fontSize = Theme.GetFontSize(Size);
            PlaceholderText.text = Placeholder;
            PlaceholderText.color = Theme.textPlaceholder;

            InputField.text = Value;
            InputField.interactable = State != ReEndInputState.Disabled;
        }

        public ReEndInput SetText(string text) { Value = text; if (_built) { InputField.text = text; } return this; }
        public ReEndInput SetPlaceholder(string ph) { Placeholder = ph; if (_built) ApplyTheme(); return this; }
        public ReEndInput SetState(ReEndInputState s) { State = s; if (_built) ApplyTheme(); return this; }
        public ReEndInput SetSize(ReEndSize s) { Size = s; if (_built) ApplyTheme(); return this; }
        public ReEndInput SetWidth(float w) { return SetWidth<ReEndInput>(w); }
    }
}
