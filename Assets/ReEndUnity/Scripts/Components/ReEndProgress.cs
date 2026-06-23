using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndProgress : ReEndBaseComponent
    {
        public float Value { get; set; }
        public ReEndProgressColor ColorVariant { get; set; } = ReEndProgressColor.Primary;
        public bool Indeterminate { get; set; }

        public Image FillImage { get; private set; }

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 8);

            // Track (self)
            BackgroundImage = gameObject.GetComponent<Image>() ?? gameObject.AddComponent<Image>();
            BackgroundImage.color = Theme.surface3;

            // Fill
            var fillGo = CreateChild(transform, "Fill");
            FillImage = fillGo.AddComponent<Image>();
            FillImage.raycastTarget = false;
            var fRT = FillImage.rectTransform;
            fRT.anchorMin = Vector2.zero;
            fRT.anchorMax = Vector2.one;
            fRT.pivot = new Vector2(0, 0.5f);
            fRT.offsetMin = Vector2.zero;
            fRT.offsetMax = Vector2.zero;
        }

        public override void ApplyTheme()
        {
            Color c = ColorVariant switch
            {
                ReEndProgressColor.Success => Theme.efGreen,
                ReEndProgressColor.Warning => Theme.efOrange,
                ReEndProgressColor.Danger => Theme.efRed,
                _ => Theme.primary
            };

            FillImage.color = c;
            float pct = Mathf.Clamp01(Value);
            FillImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pct * RectTransform.rect.width);

            if (Indeterminate)
                ReEndAnimationHelper.PulseGlow(FillImage);
            else
                ReEndAnimationHelper.StopPulseGlow(FillImage);
        }

        public ReEndProgress SetValue(float v) { Value = v; if (_built) ApplyTheme(); return this; }
        public ReEndProgress SetColor(ReEndProgressColor c) { ColorVariant = c; if (_built) ApplyTheme(); return this; }
        public ReEndProgress SetIndeterminate(bool ind = true) { Indeterminate = ind; if (_built) ApplyTheme(); return this; }
        public ReEndProgress SetWidth(float w) { return SetWidth<ReEndProgress>(w); }
    }
}
