using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndDiamondLoader : ReEndBaseComponent
    {
        public float SpinDuration { get; set; } = 0.8f;

        public Image Diamond { get; private set; }

        protected override void BuildInternal()
        {
            RectTransform.sizeDelta = new Vector2(32, 32);

            Diamond = gameObject.AddComponent<Image>();
            Diamond.raycastTarget = false;
            var mat = GetOrCreateMaterial("ReEnd/UI/Diamond");
            mat.SetFloat("_DiamondCount", 1);
            mat.SetFloat("_DiamondSize", 0.14f);
            Diamond.material = mat;
            Diamond.rectTransform.sizeDelta = new Vector2(32, 32);
        }

        public override void ApplyTheme()
        {
            Diamond.material.SetColor("_DiamondColor", Theme.primary);
            ReEndAnimationHelper.DiamondSpin(Diamond.rectTransform, SpinDuration);
        }

        public ReEndDiamondLoader SetSpinDuration(float d) { SpinDuration = d; if (_built) ApplyTheme(); return this; }
    }
}
