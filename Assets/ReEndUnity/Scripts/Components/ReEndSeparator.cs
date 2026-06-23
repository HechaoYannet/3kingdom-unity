using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndSeparator : ReEndBaseComponent
    {
        public ReEndDirection Direction { get; set; } = ReEndDirection.Horizontal;
        public ReEndSeparatorStyle Style { get; set; } = ReEndSeparatorStyle.Solid;

        public Image Line { get; private set; }

        protected override void BuildInternal()
        {
            Line = gameObject.AddComponent<Image>();
            Line.raycastTarget = false;

            if (Direction == ReEndDirection.Horizontal)
            {
                RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200);
                RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1);
            }
            else
            {
                RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1);
                RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);
            }
        }

        public override void ApplyTheme()
        {
            switch (Style)
            {
                case ReEndSeparatorStyle.Solid:
                    Line.color = Theme.borderDefault;
                    Line.material = null;
                    break;
                case ReEndSeparatorStyle.Gradient:
                    Line.material = GetOrCreateMaterial("ReEnd/UI/GradientLine");
                    Line.color = Theme.primary;
                    Line.material.SetFloat("_Direction", Direction == ReEndDirection.Horizontal ? 0 : 1);
                    break;
                case ReEndSeparatorStyle.Glow:
                    Line.material = GetOrCreateMaterial("ReEnd/UI/Glow");
                    Line.color = Theme.primary;
                    break;
                case ReEndSeparatorStyle.Dashed:
                    Line.color = Theme.borderDefault;
                    Line.material = null;
                    break;
                case ReEndSeparatorStyle.Diamond:
                    Line.material = GetOrCreateMaterial("ReEnd/UI/Diamond");
                    Line.material.SetFloat("_DiamondCount", Direction == ReEndDirection.Horizontal ? 10 : 3);
                    Line.material.SetFloat("_DiamondSize", 0.04f);
                    Line.material.SetFloat("_DiamondSpacing", 0.12f);
                    Line.material.SetFloat("_DiamondRotate", 0);
                    Line.color = Theme.primary;
                    break;
            }
        }

        public ReEndSeparator SetDirection(ReEndDirection d) { Direction = d; return this; }
        public ReEndSeparator SetStyle(ReEndSeparatorStyle s) { Style = s; if (_built) ApplyTheme(); return this; }
        public ReEndSeparator SetLength(float len)
        {
            if (Direction == ReEndDirection.Horizontal)
                return SetWidth<ReEndSeparator>(len);
            return SetHeight<ReEndSeparator>(len);
        }
    }
}
