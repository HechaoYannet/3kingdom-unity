using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndSkeleton : ReEndBaseComponent
    {
        public ReEndSkeletonVariant Variant { get; set; } = ReEndSkeletonVariant.Line;

        protected override void BuildInternal()
        {
            switch (Variant)
            {
                case ReEndSkeletonVariant.Line:
                    RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200);
                    RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 16);
                    break;
                case ReEndSkeletonVariant.Text:
                    RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
                    RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 60);
                    break;
                case ReEndSkeletonVariant.Avatar:
                    RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 40);
                    RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 40);
                    break;
                case ReEndSkeletonVariant.Card:
                    RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 280);
                    RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 160);
                    break;
            }
        }

        public override void ApplyTheme()
        {
            foreach (Transform t in transform) Destroy(t.gameObject);

            var bg = gameObject.GetComponent<Image>() ?? gameObject.AddComponent<Image>();

            switch (Variant)
            {
                case ReEndSkeletonVariant.Line:
                    bg.color = Theme.surface2;
                    break;

                case ReEndSkeletonVariant.Text:
                    bg.color = new Color(0, 0, 0, 0); // transparent parent
                    for (int i = 0; i < 3; i++)
                    {
                        var line = CreateChild(transform, $"Line_{i}");
                        var img = line.AddComponent<Image>();
                        img.color = Theme.surface2;
                        float w = i == 2 ? 0.6f : 1f;
                        var rt = img.rectTransform;
                        rt.anchorMin = new Vector2(0, 1);
                        rt.sizeDelta = new Vector2(300 * w, 14);
                        rt.anchoredPosition = new Vector2(0, -(i * 22 + 8));
                    }
                    break;

                case ReEndSkeletonVariant.Avatar:
                    bg.color = Theme.surface2;
                    bg.material = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
                    bg.material.SetFloat("_CornerSize", 0.08f);
                    break;

                case ReEndSkeletonVariant.Card:
                    bg.color = Theme.surface1;
                    bg.material = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
                    bg.material.SetFloat("_CornerSize", 0.12f);
                    // Avatar row
                    var avRow = CreateChild(transform, "Avatar");
                    var avImg = avRow.AddComponent<Image>();
                    avImg.color = Theme.surface2;
                    var avRT = avImg.rectTransform;
                    avRT.anchorMin = new Vector2(0, 1);
                    avRT.sizeDelta = new Vector2(32, 32);
                    avRT.anchoredPosition = new Vector2(12, -12);
                    // Lines
                    for (int i = 0; i < 2; i++)
                    {
                        var l = CreateChild(transform, $"CardLine_{i}");
                        var li = l.AddComponent<Image>();
                        li.color = Theme.surface2;
                        var lr = li.rectTransform;
                        lr.anchorMin = new Vector2(0, 1);
                        lr.sizeDelta = new Vector2(200, 12);
                        lr.anchoredPosition = new Vector2(56, -(i * 20 + 14));
                    }
                    break;
            }
        }

        public ReEndSkeleton SetVariant(ReEndSkeletonVariant v) { Variant = v; return this; }
    }
}
