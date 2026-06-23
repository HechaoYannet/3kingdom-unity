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

        public Image BackgroundImage { get; private set; }
        private List<Image> _dots = new();
        private List<TMP_Text> _labels = new();

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.GetComponent<Image>() ?? gameObject.AddComponent<Image>();
            BackgroundImage.color = Theme.efBlack;
            BackgroundImage.raycastTarget = false;

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 24);
        }

        public override void ApplyTheme()
        {
            BackgroundImage.color = Theme.efBlack;

            // Ensure child count matches indicator count (no destroy-all-recreate)
            while (_dots.Count < Indicators.Count)
            {
                var dotGo = CreateChild(transform, $"Dot_{_dots.Count}");
                var dot = dotGo.AddComponent<Image>();
                dot.rectTransform.anchorMin = new Vector2(0, 0.5f);
                dot.rectTransform.sizeDelta = new Vector2(6, 6);
                _dots.Add(dot);

                var labGo = CreateChild(transform, $"Label_{_labels.Count}");
                var lab = labGo.AddComponent<TextMeshProUGUI>();
                lab.fontSize = 8;
                lab.alignment = TextAlignmentOptions.Left;
                lab.rectTransform.anchorMin = new Vector2(0, 0.5f);
                lab.rectTransform.sizeDelta = new Vector2(60, 20);
                _labels.Add(lab);
            }
            // Remove extras
            while (_dots.Count > Indicators.Count)
            {
                Destroy(_dots[_dots.Count - 1].gameObject);
                _dots.RemoveAt(_dots.Count - 1);
            }
            while (_labels.Count > Indicators.Count)
            {
                Destroy(_labels[_labels.Count - 1].gameObject);
                _labels.RemoveAt(_labels.Count - 1);
            }

            // Update positions and colors
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

                _dots[i].color = c;
                _dots[i].rectTransform.anchoredPosition = new Vector2(x, 0);
                x += 10;

                _labels[i].text = ind.Label.ToUpper();
                _labels[i].color = Theme.textMuted;
                _labels[i].rectTransform.anchoredPosition = new Vector2(x, 0);
                x += 64;
            }
        }

        public ReEndStatusBar AddIndicator(StatusIndicator i) { Indicators.Add(i); if (_built) ApplyTheme(); return this; }
        public ReEndStatusBar AddIndicator(string label, ReEndStatus status)
        {
            Indicators.Add(new StatusIndicator { Label = label, Status = status });
            if (_built) ApplyTheme();
            return this;
        }
    }
}
