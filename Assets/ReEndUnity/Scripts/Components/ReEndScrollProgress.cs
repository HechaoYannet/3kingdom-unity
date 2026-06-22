using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndScrollProgress : ReEndBaseComponent
    {
        public ScrollRect ScrollRect { get; set; }

        public Image Fill { get; private set; }

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 3);
            RectTransform.anchorMin = new Vector2(0, 1);
            RectTransform.anchorMax = new Vector2(1, 1);
            RectTransform.pivot = new Vector2(0, 1);
            RectTransform.anchoredPosition = Vector2.zero;

            var bg = gameObject.AddComponent<Image>();
            bg.color = new Color(0, 0, 0, 0);

            var fillGo = CreateChild(transform, "Fill");
            Fill = fillGo.AddComponent<Image>();
            Fill.raycastTarget = false;
            var fRT = Fill.rectTransform;
            fRT.anchorMin = new Vector2(0, 0.5f);
            fRT.anchorMax = new Vector2(0, 0.5f);
            fRT.pivot = new Vector2(0, 0.5f);
            fRT.sizeDelta = new Vector2(0, 3);
        }

        public override void ApplyTheme()
        {
            Fill.color = Theme.primary;
        }

        private void Update()
        {
            if (ScrollRect != null)
            {
                float pct = ScrollRect.verticalNormalizedPosition;
                Fill.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pct * RectTransform.rect.width);
            }
        }
    }
}
