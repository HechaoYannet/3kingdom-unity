using TMPro;
using UnityEngine;

namespace ReEndUnity
{
    public class ReEndStat : ReEndBaseComponent
    {
        public string LabelText { get; set; } = "Stat";
        public string ValueText { get; set; } = "0";
        public string DeltaText { get; set; }
        public bool Positive { get; set; } = true;
        public System.Action OnClickAction { get; set; }

        public TMP_Text Label { get; private set; }
        public TMP_Text Value { get; private set; }
        public TMP_Text Delta { get; private set; }

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 72);

            // Label (overline)
            var labGo = CreateChild(transform, "Label");
            Label = labGo.AddComponent<TextMeshProUGUI>();
            Label.alignment = TextAlignmentOptions.Left;
            Label.raycastTarget = false;
            var lRT = Label.rectTransform;
            lRT.anchorMin = new Vector2(0, 1);
            lRT.anchorMax = new Vector2(1, 1);
            lRT.pivot = new Vector2(0, 1);
            lRT.sizeDelta = new Vector2(0, 18);
            lRT.anchoredPosition = Vector2.zero;

            // Value
            var valGo = CreateChild(transform, "Value");
            Value = valGo.AddComponent<TextMeshProUGUI>();
            Value.alignment = TextAlignmentOptions.Left;
            Value.raycastTarget = false;
            var vRT = Value.rectTransform;
            vRT.anchorMin = new Vector2(0, 0.65f);
            vRT.anchorMax = new Vector2(1, 0.65f);
            vRT.pivot = new Vector2(0, 0.5f);
            vRT.sizeDelta = new Vector2(0, 32);
            vRT.anchoredPosition = Vector2.zero;

            // Delta
            var deltaGo = CreateChild(transform, "Delta");
            Delta = deltaGo.AddComponent<TextMeshProUGUI>();
            Delta.alignment = TextAlignmentOptions.Left;
            Delta.raycastTarget = false;
            var dRT = Delta.rectTransform;
            dRT.anchorMin = new Vector2(0, 0);
            dRT.anchorMax = new Vector2(1, 0);
            dRT.pivot = new Vector2(0, 0);
            dRT.sizeDelta = new Vector2(0, 18);
            dRT.anchoredPosition = Vector2.zero;
        }

        public override void ApplyTheme()
        {
            Label.text = LabelText.ToUpperInvariant();
            Label.fontSize = Theme.overlineSize;
            Label.color = Theme.textMuted;

            Value.text = ValueText;
            Value.fontSize = Theme.h3Size;
            Value.color = Theme.textPrimary;

            if (!string.IsNullOrEmpty(DeltaText))
            {
                Delta.text = $"{(Positive ? "▲" : "▼")} {DeltaText}";
                Delta.fontSize = Theme.bodySmSize;
                Delta.color = Positive ? Theme.efGreen : Theme.efRed;
                Delta.gameObject.SetActive(true);
            }
            else
            {
                Delta.gameObject.SetActive(false);
            }
        }

        public ReEndStat SetLabel(string l) { LabelText = l; if (_built) ApplyTheme(); return this; }
        public ReEndStat SetValue(string v) { ValueText = v; if (_built) ApplyTheme(); return this; }
        public ReEndStat SetDelta(string d, bool positive = true) { DeltaText = d; Positive = positive; if (_built) ApplyTheme(); return this; }
        public ReEndStat SetPositive(bool p) { Positive = p; if (_built) ApplyTheme(); return this; }
    }
}
