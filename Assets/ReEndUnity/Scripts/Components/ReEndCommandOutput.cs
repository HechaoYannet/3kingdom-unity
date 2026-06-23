using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndCommandOutput : ReEndBaseComponent
    {
        public string Prompt { get; set; } = "$>";
        public List<string> Lines { get; private set; } = new();
        public bool AutoScroll { get; set; } = true;

        public TMP_Text Output { get; private set; }

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.AddComponent<Image>();
            BackgroundImage.color = Theme.efBlack;

            var scanGo = CreateChild(transform, "Scanline");
            var scanImg = scanGo.AddComponent<Image>();
            scanImg.material = GetOrCreateMaterial("ReEnd/UI/Scanline");
            scanImg.material.SetFloat("_ScanlineOpacity", 0.015f);
            scanImg.material.SetFloat("_ScanlineSpacing", 120);
            scanImg.raycastTarget = false;
            Stretch(scanImg.rectTransform);

            Output = CreateChild<TextMeshProUGUI>(transform, "Output");
            Output.alignment = TextAlignmentOptions.TopLeft;
            Output.raycastTarget = false;
            Stretch(Output.rectTransform);
            Output.rectTransform.offsetMin = new Vector2(10, 10);
            Output.rectTransform.offsetMax = new Vector2(-10, -10);

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 300);
        }

        public override void ApplyTheme()
        {
            Output.fontSize = Theme.bodySmSize;
            Output.color = Theme.efLime;
            RefreshOutput();
        }

        private void RefreshOutput()
        {
            Output.text = string.Join("\n", Lines);
        }

        public ReEndCommandOutput Execute(string command, string result = "")
        {
            Lines.Add($"{Prompt} {command}");
            if (!string.IsNullOrEmpty(result))
                Lines.Add($"  {result}");
            if (AutoScroll && Lines.Count > 50)
                Lines.RemoveAt(0);
            if (_built)
            {
                RefreshOutput();
                // Scroll to bottom
                // Canvas.ForceUpdateCanvases();
                // scrollRect.verticalNormalizedPosition = 0;
            }
            return this;
        }

        public ReEndCommandOutput Clear() { Lines.Clear(); if (_built) RefreshOutput(); return this; }
    }
}
