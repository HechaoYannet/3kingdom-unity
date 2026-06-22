using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndStatusBar : ReEndBaseComponent
    {
        public List<StatusIndicator> Indicators { get; private set; } = new();

        [System.Serializable]
        public class StatusIndicator
        {
            public string Label;
            public ReEndStatus Status;
        }

        protected override void BuildInternal()
        {
            var bg = gameObject.AddComponent<Image>();
            bg.color = Theme.efBlack;
            bg.raycastTarget = false;

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 24);
        }

        public override void ApplyTheme()
        {
            foreach (Transform t in transform) Destroy(t.gameObject);

            float x = 8;
            for (int i = 0; i < Indicators.Count; i++)
            {
                var ind = Indicators[i];
                Color c = ind.Status switch
                {
                    ReEndStatus.Online => Theme.efGreen,
                    ReEndStatus.Offline => Theme.efGrayMid,
                    ReEndStatus.Warning => Theme.efOrange,
                    _ => Theme.textMuted
                };

                // Dot
                var dotGo = CreateChild(transform, $"Dot_{i}");
                var dot = dotGo.AddComponent<Image>();
                dot.color = c;
                dot.rectTransform.anchorMin = new Vector2(0, 0.5f);
                dot.rectTransform.sizeDelta = new Vector2(6, 6);
                dot.rectTransform.anchoredPosition = new Vector2(x, 0);
                x += 10;

                // Label
                var labGo = CreateChild(transform, $"Label_{i}");
                var lab = labGo.AddComponent<TextMeshProUGUI>();
                lab.text = ind.Label.ToUpper();
                lab.fontSize = 8;
                lab.color = Theme.textMuted;
                lab.alignment = TextAlignmentOptions.Left;
                lab.rectTransform.anchorMin = new Vector2(0, 0.5f);
                lab.rectTransform.sizeDelta = new Vector2(60, 20);
                lab.rectTransform.anchoredPosition = new Vector2(x, 0);
                x += 64;
            }
        }

        public ReEndStatusBar AddIndicator(StatusIndicator i) { Indicators.Add(i); return this; }
        public ReEndStatusBar AddIndicator(string label, ReEndStatus status)
        {
            Indicators.Add(new StatusIndicator { Label = label, Status = status });
            return this;
        }
    }
}
