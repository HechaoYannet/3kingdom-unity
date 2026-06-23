using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndDataStream : ReEndBaseComponent
    {
        public int MaxLines { get; set; } = 20;
        public float ScrollSpeed { get; set; } = 1f;

        public TMP_Text TerminalText { get; private set; }
        private List<string> _lines = new();

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.AddComponent<Image>();
            BackgroundImage.color = Theme.efBlack;
            var mat = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
            mat.SetFloat("_CornerSize", 0.04f);
            BackgroundImage.material = mat;

            var scanGo = CreateChild(transform, "Scanline");
            var scanImg = scanGo.AddComponent<Image>();
            scanImg.material = GetOrCreateMaterial("ReEnd/UI/Scanline");
            scanImg.raycastTarget = false;
            Stretch(scanImg.rectTransform);

            TerminalText = CreateChild<TextMeshProUGUI>(transform, "Text");
            TerminalText.alignment = TextAlignmentOptions.TopLeft;
            TerminalText.raycastTarget = false;
            TerminalText.rectTransform.offsetMin = new Vector2(8, 8);
            TerminalText.rectTransform.offsetMax = new Vector2(-8, -8);
            Stretch(TerminalText.rectTransform);

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 240);
        }

        public void AddLine(string line)
        {
            _lines.Add($"> {line}");
            if (_lines.Count > MaxLines) _lines.RemoveAt(0);
            Render();
        }

        private void Render()
        {
            TerminalText.text = string.Join("\n", _lines);
        }

        public override void ApplyTheme()
        {
            BackgroundImage.color = Theme.efBlack;
            TerminalText.fontSize = Theme.bodySmSize;
            TerminalText.color = Theme.efLime;
        }

        public ReEndDataStream AddLines(List<string> lines) { foreach (var l in lines) AddLine(l); return this; }
    }
}
