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
                    Line.material = Direction == ReEndDirection.Horizontal
                        ? new Material(Shader.Find("ReEnd/UI/Glow"))
                        : new Material(Shader.Find("ReEnd/UI/Glow"));
                    Line.color = Theme.primary;
                    break;
                case ReEndSeparatorStyle.Glow:
                    Line.color = Theme.primary;
                    Line.material = new Material(Shader.Find("ReEnd/UI/Glow"));
                    break;
                case ReEndSeparatorStyle.Dashed:
                    Line.color = Theme.borderDefault;
                    break;
                case ReEndSeparatorStyle.Diamond:
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
