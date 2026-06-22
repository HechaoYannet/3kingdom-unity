using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndBreadcrumb : ReEndBaseComponent
    {
        public List<string> Segments { get; private set; } = new();
        public string Separator { get; set; } = "/";
        public System.Action<int> OnSegmentClick { get; set; }

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 24);
        }

        public override void ApplyTheme()
        {
            foreach (Transform t in transform) Destroy(t.gameObject);

            float x = 0;
            for (int i = 0; i < Segments.Count; i++)
            {
                int idx = i;
                bool isLast = i == Segments.Count - 1;

                var go = CreateChild(transform, $"Seg_{i}");
                var txt = go.AddComponent<TextMeshProUGUI>();
                txt.text = Segments[i];
                txt.fontSize = Theme.bodySmSize;
                txt.color = isLast ? Theme.textPrimary : Theme.textMuted;
                txt.alignment = TextAlignmentOptions.Left;

                if (!isLast)
                {
                    var btn = go.AddComponent<Button>();
                    btn.onClick.AddListener(() => OnSegmentClick?.Invoke(idx));
                }

                var rt = go.GetComponent<RectTransform>();
                rt.anchorMin = new Vector2(0, 0.5f);
                rt.sizeDelta = new Vector2(80, 20);
                rt.anchoredPosition = new Vector2(x, 0);
                x += txt.preferredWidth + 4;

                if (!isLast)
                {
                    var sepGo = CreateChild(transform, $"Sep_{i}");
                    var sepTxt = sepGo.AddComponent<TextMeshProUGUI>();
                    sepTxt.text = Separator;
                    sepTxt.fontSize = Theme.bodySmSize;
                    sepTxt.color = Theme.textMuted;
                    var sRT = sepTxt.rectTransform;
                    sRT.anchorMin = new Vector2(0, 0.5f);
                    sRT.sizeDelta = new Vector2(12, 20);
                    sRT.anchoredPosition = new Vector2(x, 0);
                    x += 16;
                }
            }
        }

        public ReEndBreadcrumb SetSegments(List<string> s) { Segments = s; return this; }
        public ReEndBreadcrumb AddSegment(string s) { Segments.Add(s); return this; }
        public ReEndBreadcrumb SetOnSegmentClick(System.Action<int> cb) { OnSegmentClick = cb; return this; }
    }
}
