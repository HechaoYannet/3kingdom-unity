using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndTooltip : ReEndBaseComponent
    {
        public string Text { get; set; } = "";

        public TMP_Text Label { get; private set; }

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.GetComponent<Image>() ?? gameObject.AddComponent<Image>();
            BackgroundImage.raycastTarget = false;
            var mat = new Material(Shader.Find("ReEnd/UI/ClipCorner"));
            mat.SetFloat("_CornerSize", 4);
            BackgroundImage.material = mat;

            var textGo = CreateChild(transform, "Text");
            Label = textGo.AddComponent<TextMeshProUGUI>();
            Label.alignment = TextAlignmentOptions.Center;
            Label.raycastTarget = false;
            var tRT = Label.rectTransform;
            tRT.offsetMin = new Vector2(8, 4);
            tRT.offsetMax = new Vector2(-8, -4);
            Stretch(tRT);

            gameObject.SetActive(false);
        }

        public override void ApplyTheme()
        {
            BackgroundImage.color = Theme.popover;
            Label.text = Text;
            Label.fontSize = Theme.bodySmSize;
            Label.color = Theme.popoverForeground;
        }

        public ReEndTooltip SetText(string t) { Text = t; if (_built) ApplyTheme(); return this; }
        public void ShowAt(Vector2 screenPos) { gameObject.SetActive(true); RectTransform.position = screenPos; ApplyTheme(); }
        public void Hide() { gameObject.SetActive(false); }
    }
}
