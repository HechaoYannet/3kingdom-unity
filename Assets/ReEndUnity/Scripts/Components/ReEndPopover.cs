using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndPopover : ReEndBaseComponent
    {
        public string Content { get; set; }
        public bool Show { get; set; }

        public TMP_Text ContentText { get; private set; }

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.GetComponent<Image>() ?? gameObject.AddComponent<Image>();
            var mat = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
            mat.SetFloat("_CornerSize", 0.04f);
            BackgroundImage.material = mat;

            var textGo = CreateChild(transform, "Content");
            ContentText = textGo.AddComponent<TextMeshProUGUI>();
            ContentText.alignment = TextAlignmentOptions.Left;
            ContentText.raycastTarget = false;
            var tRT = ContentText.rectTransform;
            tRT.offsetMin = new Vector2(12, 8);
            tRT.offsetMax = new Vector2(-12, -8);
            Stretch(tRT);

            gameObject.SetActive(false);
        }

        public override void ApplyTheme()
        {
            BackgroundImage.color = Theme.popover;
            ContentText.text = Content;
            ContentText.fontSize = Theme.bodySmSize;
            ContentText.color = Theme.popoverForeground;

            gameObject.SetActive(Show);
        }

        public ReEndPopover SetContent(string c) { Content = c; if (_built) ApplyTheme(); return this; }
        public void ShowAt(Vector2 screenPos)
        {
            Show = true;
            RectTransform.position = screenPos;
            ApplyTheme();
        }
        public void Hide() { Show = false; ApplyTheme(); }
    }
}
