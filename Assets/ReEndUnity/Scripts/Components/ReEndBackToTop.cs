using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndBackToTop : ReEndBaseComponent
    {
        public float Threshold { get; set; } = 200;
        public ScrollRect ScrollRect { get; set; }

        private Button _btn;
        private Image _bg;

        protected override void BuildInternal()
        {
            _bg = gameObject.AddComponent<Image>();
            _bg.raycastTarget = true;
            _bg.color = Theme.primary;
            var mat = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
            mat.SetFloat("_CornerSize", 0.04f);
            _bg.material = mat;

            var txt = CreateChild<TextMeshProUGUI>(transform, "Arrow");
            txt.text = "▲";
            txt.alignment = TextAlignmentOptions.Center;
            txt.fontSize = 16;
            txt.color = Theme.primaryForeground;
            Stretch(txt.rectTransform);

            _btn = gameObject.AddComponent<Button>();
            _btn.onClick.AddListener(() => { if (ScrollRect != null) ScrollRect.verticalNormalizedPosition = 1; });

            var rt = (RectTransform)transform;
            rt.sizeDelta = new Vector2(40, 40);
            gameObject.SetActive(false);
        }

        public override void ApplyTheme()
        {
            _bg.color = Theme.primary;
        }

        private void Update()
        {
            if (ScrollRect != null)
                gameObject.SetActive(ScrollRect.verticalNormalizedPosition < Threshold / (ScrollRect.content.rect.height - ScrollRect.viewport.rect.height));
        }
    }
}
