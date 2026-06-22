using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndTimeline : ReEndBaseComponent
    {
        public List<TimelineEntry> Entries { get; private set; } = new();

        [System.Serializable]
        public class TimelineEntry
        {
            public string Date;
            public string Title;
            public string Description;
            public ReEndStatus Status;
        }

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);
        }

        public override void ApplyTheme()
        {
            foreach (Transform t in transform) Destroy(t.gameObject);

            float y = 0;
            for (int i = 0; i < Entries.Count; i++)
            {
                var e = Entries[i];
                float itemH = 64;

                // Line
                if (i < Entries.Count - 1)
                {
                    var lineGo = CreateChild(transform, $"Line_{i}");
                    var lineImg = lineGo.AddComponent<Image>();
                    lineImg.color = Theme.borderDefault;
                    var lRT = lineImg.rectTransform;
                    lRT.anchorMin = new Vector2(0, 1);
                    lRT.pivot = new Vector2(0, 1);
                    lRT.sizeDelta = new Vector2(1, itemH);
                    lRT.anchoredPosition = new Vector2(7, -y - 16);
                }

                // Node
                var nodeGo = CreateChild(transform, $"Node_{i}");
                var nodeImg = nodeGo.AddComponent<Image>();
                nodeImg.color = e.Status == ReEndStatus.Success ? Theme.efGreen
                    : e.Status == ReEndStatus.Warning ? Theme.efOrange
                    : e.Status == ReEndStatus.Online ? Theme.primary
                    : Theme.textMuted;
                var nRT = nodeImg.rectTransform;
                nRT.anchorMin = new Vector2(0, 1);
                nRT.sizeDelta = new Vector2(8, 8);
                nRT.anchoredPosition = new Vector2(3, -y - 4);

                // Date
                var dateGo = CreateChild(transform, $"Date_{i}");
                var dateTxt = dateGo.AddComponent<TextMeshProUGUI>();
                dateTxt.text = e.Date;
                dateTxt.fontSize = Theme.captionSize;
                dateTxt.color = Theme.textMuted;
                var dRT = dateTxt.rectTransform;
                dRT.anchorMin = new Vector2(0, 1);
                dRT.sizeDelta = new Vector2(380, 16);
                dRT.anchoredPosition = new Vector2(24, -y - 4);

                // Title
                var titleGo = CreateChild(transform, $"Title_{i}");
                var titleTxt = titleGo.AddComponent<TextMeshProUGUI>();
                titleTxt.text = e.Title;
                titleTxt.fontSize = Theme.bodySize;
                titleTxt.color = Theme.textPrimary;
                var tRT = titleTxt.rectTransform;
                tRT.anchorMin = new Vector2(0, 1);
                tRT.sizeDelta = new Vector2(380, 20);
                tRT.anchoredPosition = new Vector2(24, -y - 22);

                // Description
                var descGo = CreateChild(transform, $"Desc_{i}");
                var descTxt = descGo.AddComponent<TextMeshProUGUI>();
                descTxt.text = e.Description;
                descTxt.fontSize = Theme.bodySmSize;
                descTxt.color = Theme.textSecondary;
                var descRT = descTxt.rectTransform;
                descRT.anchorMin = new Vector2(0, 1);
                descRT.sizeDelta = new Vector2(380, 16);
                descRT.anchoredPosition = new Vector2(24, -y - 44);

                y += itemH;
            }

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y);
        }

        public ReEndTimeline AddEntry(TimelineEntry e) { Entries.Add(e); return this; }
    }
}
