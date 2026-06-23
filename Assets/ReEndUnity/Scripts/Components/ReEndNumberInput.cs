using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndNumberInput : ReEndBaseComponent
    {
        public float Value { get; set; }
        public float Min { get; set; } = 0;
        public float Max { get; set; } = 100;
        public float Step { get; set; } = 1;
        public ReEndSize Size { get; set; } = ReEndSize.Md;
        public System.Action<float> OnValueChanged { get; set; }

        public TMP_InputField InputField { get; private set; }
        private Button _minusBtn;
        private Button _plusBtn;

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.GetComponent<Image>() ?? gameObject.AddComponent<Image>();

            var inputRT = RectTransform;
            inputRT.sizeDelta = new Vector2(140, Theme.GetSizeValue(Size));

            // Minus button
            var minusGo = CreateChild(transform, "Minus");
            var minusImg = minusGo.AddComponent<Image>();
            var minusLab = CreateChild<TextMeshProUGUI>(minusGo.transform, "Label");
            minusLab.text = "−";
            minusLab.alignment = TextAlignmentOptions.Center;
            minusLab.fontSize = 18;
            Stretch(minusLab.rectTransform);
            _minusBtn = minusGo.AddComponent<Button>();
            _minusBtn.onClick.AddListener(() => { Value = Mathf.Clamp(Value - Step, Min, Max); UpdateDisplay(); OnValueChanged?.Invoke(Value); });
            var mRT = minusGo.GetComponent<RectTransform>();
            mRT.anchorMin = new Vector2(0, 0);
            mRT.anchorMax = new Vector2(0, 1);
            mRT.pivot = new Vector2(0, 0.5f);
            mRT.sizeDelta = new Vector2(36, 0);

            // Plus button
            var plusGo = CreateChild(transform, "Plus");
            var plusImg = plusGo.AddComponent<Image>();
            var plusLab = CreateChild<TextMeshProUGUI>(plusGo.transform, "Label");
            plusLab.text = "+";
            plusLab.alignment = TextAlignmentOptions.Center;
            plusLab.fontSize = 18;
            Stretch(plusLab.rectTransform);
            _plusBtn = plusGo.AddComponent<Button>();
            _plusBtn.onClick.AddListener(() => { Value = Mathf.Clamp(Value + Step, Min, Max); UpdateDisplay(); OnValueChanged?.Invoke(Value); });
            var pRT = plusGo.GetComponent<RectTransform>();
            pRT.anchorMin = new Vector2(1, 0);
            pRT.anchorMax = new Vector2(1, 1);
            pRT.pivot = new Vector2(1, 0.5f);
            pRT.sizeDelta = new Vector2(36, 0);

            // Input
            var textGo = CreateChild(transform, "Text");
            var text = textGo.AddComponent<TextMeshProUGUI>();
            text.alignment = TextAlignmentOptions.Center;
            var tRT = text.rectTransform;
            tRT.offsetMin = new Vector2(36, 0);
            tRT.offsetMax = new Vector2(-36, 0);
            Stretch(tRT);
            InputField = gameObject.AddComponent<TMP_InputField>();
            InputField.textComponent = text;
            InputField.textViewport = text.rectTransform;
            InputField.contentType = TMP_InputField.ContentType.DecimalNumber;
            InputField.onEndEdit.AddListener(v =>
            {
                Value = float.TryParse(v, out var f) ? Mathf.Clamp(f, Min, Max) : Value;
                UpdateDisplay();
                OnValueChanged?.Invoke(Value);
            });
        }

        private void UpdateDisplay() { InputField.text = Value.ToString("0.##"); }

        public override void ApplyTheme()
        {
            BackgroundImage.color = Theme.surface2;
            UpdateDisplay();
        }

        public ReEndNumberInput SetValue(float v) { Value = Mathf.Clamp(v, Min, Max); if (_built) UpdateDisplay(); return this; }
        public ReEndNumberInput SetRange(float min, float max) { Min = min; Max = max; return this; }
        public ReEndNumberInput SetStep(float s) { Step = s; return this; }
        public ReEndNumberInput SetSize(ReEndSize s) { Size = s; if (_built) ApplyTheme(); return this; }
        public ReEndNumberInput SetOnValueChanged(System.Action<float> cb) { OnValueChanged = cb; return this; }
    }
}
