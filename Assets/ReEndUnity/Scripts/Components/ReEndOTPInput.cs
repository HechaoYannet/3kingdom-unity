using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndOTPInput : ReEndBaseComponent
    {
        public int Length { get; set; } = 6;
        public System.Action<string> OnComplete { get; set; }
        public string Value
        {
            get
            {
                var chars = new char[_digits.Length];
                for (int i = 0; i < _digits.Length; i++)
                    chars[i] = _digits[i].text.Length > 0 ? _digits[i].text[0] : ' ';
                return new string(chars).Trim();
            }
        }

        private TMP_InputField[] _digits;
        private int _currentIndex;

        protected override void BuildInternal()
        {
            _digits = new TMP_InputField[Length];

            float slotWidth = 48;
            float gap = 8;
            float totalWidth = Length * slotWidth + (Length - 1) * gap;
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, totalWidth);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotWidth);

            for (int i = 0; i < Length; i++)
            {
                var slotGo = CreateChild(transform, $"Digit_{i}");
                var slotImg = slotGo.AddComponent<Image>();
                slotImg.color = Theme.surface2;

                var slotRT = slotGo.GetComponent<RectTransform>();
                slotRT.anchorMin = new Vector2(0, 0.5f);
                slotRT.anchorMax = new Vector2(0, 0.5f);
                slotRT.pivot = new Vector2(0, 0.5f);
                slotRT.sizeDelta = new Vector2(slotWidth, slotWidth);
                slotRT.anchoredPosition = new Vector2(i * (slotWidth + gap), 0);

                var textGo = new GameObject("Text", typeof(RectTransform));
                textGo.transform.SetParent(slotGo.transform, false);
                var text = textGo.AddComponent<TextMeshProUGUI>();
                text.alignment = TextAlignmentOptions.Center;
                text.fontSize = 28;
                Stretch(text.rectTransform);

                var input = slotGo.AddComponent<TMP_InputField>();
                input.textComponent = text;
                input.characterLimit = 1;
                input.contentType = TMP_InputField.ContentType.Alphanumeric;
                input.onValueChanged.AddListener(v => OnDigitChanged(i, v));
                input.onSelect.AddListener(_ => _currentIndex = i);

                _digits[i] = input;
            }
        }

        private void OnDigitChanged(int index, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _digits[index].text = value[value.Length - 1].ToString().ToUpper();
                if (index < Length - 1)
                {
                    _digits[index + 1].Select();
                    _digits[index + 1].ActivateInputField();
                }
            }

            if (Value.Length == Length)
                OnComplete?.Invoke(Value);
        }

        public override void ApplyTheme()
        {
            foreach (var d in _digits)
            {
                d.image.color = Theme.surface2;
                var mat = new Material(Shader.Find("ReEnd/UI/ClipCorner"));
                mat.SetFloat("_CornerSize", Theme.clipCornerSm);
                d.image.material = mat;
            }
        }

        public ReEndOTPInput SetLength(int len) { Length = len; return this; }
        public ReEndOTPInput SetOnComplete(System.Action<string> cb) { OnComplete = cb; return this; }
        public void Clear()
        {
            foreach (var d in _digits) d.text = "";
            _currentIndex = 0;
            _digits[0].Select();
        }
    }
}
