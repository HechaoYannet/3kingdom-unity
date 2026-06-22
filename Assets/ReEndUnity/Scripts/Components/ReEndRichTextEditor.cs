using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndRichTextEditor : ReEndBaseComponent
    {
        public string Value { get; set; } = "";
        public int MaxLength { get; set; } = 5000;

        public TMP_InputField InputField { get; private set; }
        private GameObject _toolbar;
        private TMP_Text _charCount;

        protected override void BuildInternal()
        {
            // Toolbar
            _toolbar = CreateChild(transform, "Toolbar");
            var toolbarRT = _toolbar.GetComponent<RectTransform>();
            toolbarRT.anchorMin = new Vector2(0, 1);
            toolbarRT.anchorMax = new Vector2(1, 1);
            toolbarRT.pivot = new Vector2(0, 1);
            toolbarRT.sizeDelta = new Vector2(0, 36);
            toolbarRT.anchoredPosition = Vector2.zero;

            AddToolBtn("B", FontStyles.Bold);
            AddToolBtn("I", FontStyles.Italic);
            AddToolBtn("U", FontStyles.Underline);
            AddToolBtn("S", FontStyles.Strikethrough);

            // Text area
            var textGo = CreateChild(transform, "Text");
            var text = textGo.AddComponent<TextMeshProUGUI>();
            text.alignment = TextAlignmentOptions.TopLeft;
            var tRT = text.rectTransform;
            tRT.offsetMin = new Vector2(8, 44);
            tRT.offsetMax = new Vector2(-8, -28);
            Stretch(tRT);

            InputField = gameObject.AddComponent<TMP_InputField>();
            InputField.textComponent = text;
            InputField.lineType = TMP_InputField.LineType.MultiLineNewline;
            InputField.characterLimit = MaxLength;

            // Char count
            var countGo = CreateChild(transform, "CharCount");
            _charCount = countGo.AddComponent<TextMeshProUGUI>();
            _charCount.alignment = TextAlignmentOptions.Right;
            var cRT = _charCount.rectTransform;
            cRT.anchorMin = new Vector2(1, 0);
            cRT.anchorMax = new Vector2(1, 0);
            cRT.pivot = new Vector2(1, 0);
            cRT.sizeDelta = new Vector2(100, 20);
            cRT.anchoredPosition = new Vector2(-4, 4);

            InputField.onValueChanged.AddListener(v =>
            {
                Value = v;
                _charCount.text = $"{v.Length}/{MaxLength}";
            });
        }

        private void AddToolBtn(string label, TMPro.FontStyles style)
        {
            var go = CreateChild(_toolbar.transform, $"Btn_{label}");
            var img = go.AddComponent<Image>();
            var txt = CreateChild<TextMeshProUGUI>(go.transform, "Label");
            txt.text = label;
            txt.alignment = TextAlignmentOptions.Center;
            txt.fontSize = 14;
            Stretch(txt.rectTransform);
            var btn = go.AddComponent<Button>();
            btn.onClick.AddListener(() => ToggleStyle(style));
            var rt = go.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(32, 32);
        }

        private void ToggleStyle(TMPro.FontStyles style)
        {
            // Simplified: toggle isn't directly supported on TMP_InputField easily
        }

        public override void ApplyTheme()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);

            var bg = gameObject.GetComponent<Image>();
            if (bg == null) bg = gameObject.AddComponent<Image>();
            bg.color = Theme.surface1;

            _charCount.fontSize = Theme.captionSize;
            _charCount.color = Theme.textMuted;
            _charCount.text = $"{Value.Length}/{MaxLength}";
        }

        public ReEndRichTextEditor SetText(string text) { Value = text; if (_built) { InputField.text = text; } return this; }
        public string GetMarkdown() => Value;
    }
}
