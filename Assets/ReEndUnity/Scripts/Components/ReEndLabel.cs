using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndLabel : ReEndBaseComponent
    {
        public string Text { get; set; }
        public ReEndSize Size { get; set; } = ReEndSize.Md;
        public Color TextColor { get; set; } = Color.clear; // clear = use theme
        public bool IsMuted { get; set; }

        public TMP_Text Label { get; private set; }

        protected override void BuildInternal()
        {
            Label = gameObject.AddComponent<TextMeshProUGUI>();
            Label.alignment = TextAlignmentOptions.Left;
            Label.raycastTarget = false;
        }

        public override void ApplyTheme()
        {
            Label.text = Text;
            Label.fontSize = Theme.GetFontSize(Size);
            Label.color = TextColor != Color.clear ? TextColor
                : IsMuted ? Theme.textMuted : Theme.textPrimary;
        }

        public ReEndLabel SetText(string text) { Text = text; if (_built) ApplyTheme(); return this; }
        public ReEndLabel SetSize(ReEndSize size) { Size = size; if (_built) ApplyTheme(); return this; }
        public ReEndLabel SetColor(Color c) { TextColor = c; if (_built) ApplyTheme(); return this; }
        public ReEndLabel SetMuted(bool muted = true) { IsMuted = muted; if (_built) ApplyTheme(); return this; }
    }
}
