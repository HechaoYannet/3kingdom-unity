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
            var mat = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
            Diamond.material = mat;

            // Create diamond shape by rotating a square
            Diamond.rectTransform.localRotation = Quaternion.Euler(0, 0, 45);
            Diamond.rectTransform.sizeDelta = new Vector2(24, 24);
        }

        public override void ApplyTheme()
        {
            Diamond.color = Theme.primary;
        }

        private void Update()
        {
            Diamond.rectTransform.Rotate(0, 0, -360f / SpinDuration * Time.deltaTime);
        }
    }
}
