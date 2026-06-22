using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndFooter : ReEndBaseComponent
    {
        public string BrandText { get; set; } = "ENDFIELD SYSTEM";
        public List<FooterColumn> Columns { get; private set; } = new();

        [System.Serializable]
        public class FooterColumn
        {
            public string Title;
            public List<string> Links;
        }

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 800);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 120);
        }

        public override void ApplyTheme()
        {
            foreach (Transform t in transform) Destroy(t.gameObject);

            // Brand
            var brandGo = CreateChild(transform, "Brand");
            var brandTxt = brandGo.AddComponent<TextMeshProUGUI>();
            brandTxt.text = BrandText;
            brandTxt.fontSize = Theme.h4Size;
            brandTxt.color = Theme.textPrimary;
            var bRT = brandTxt.rectTransform;
            bRT.anchorMin = new Vector2(0, 1);
            bRT.sizeDelta = new Vector2(200, 28);

            // Columns
            float colX = 200;
            foreach (var col in Columns)
            {
                var colGo = CreateChild(transform, col.Title);
                float y = 0;
                var titleTxt = CreateChild<TextMeshProUGUI>(colGo.transform, "Title");
                titleTxt.text = col.Title.ToUpperInvariant();
                titleTxt.fontSize = Theme.captionSize;
                titleTxt.color = Theme.textMuted;
                titleTxt.rectTransform.anchoredPosition = new Vector2(colX, -y);
                y += 20;

                foreach (var link in col.Links)
                {
                    var linkGo = CreateChild(colGo.transform, link);
                    var linkTxt = linkGo.AddComponent<TextMeshProUGUI>();
                    linkTxt.text = link;
                    linkTxt.fontSize = Theme.bodySmSize;
                    linkTxt.color = Theme.textSecondary;
                    linkTxt.rectTransform.anchoredPosition = new Vector2(colX, -y);
                    y += 22;
                }

                colX += 160;
            }
        }

        public ReEndFooter SetBrand(string b) { BrandText = b; return this; }
        public ReEndFooter AddColumn(FooterColumn col) { Columns.Add(col); return this; }
    }
}
