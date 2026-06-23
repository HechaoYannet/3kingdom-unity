using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndFrequencyBars : ReEndBaseComponent
    {
        public int BarCount { get; set; } = 16;
        public float UpdateSpeed { get; set; } = 0.8f;
        public float MaxHeight { get; set; } = 60;

        public Image BarImage { get; private set; }

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, BarCount * 8);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, MaxHeight);

            BarImage = gameObject.AddComponent<Image>();
            BarImage.raycastTarget = false;
            var mat = GetOrCreateMaterial("ReEnd/UI/FrequencyBar");
            mat.SetFloat("_BarCount", BarCount);
            mat.SetFloat("_BarWidth", 0.7f);
            mat.SetFloat("_BarSpeed", UpdateSpeed);
            mat.SetFloat("_BarMinScale", 0.15f);
            mat.SetFloat("_BarMaxScale", 1f);
            mat.SetFloat("_BarGlow", 0.05f);
            mat.SetFloat("_BarStagger", 0.13f);
            BarImage.material = mat;
        }

        public override void ApplyTheme()
        {
            BarImage.material.SetColor("_BarColor", Theme.primary);
        }

        public ReEndFrequencyBars SetBarCount(int count) { BarCount = count; if (_built) { BarImage.material.SetFloat("_BarCount", BarCount); ApplyTheme(); } return this; }
    }
}
