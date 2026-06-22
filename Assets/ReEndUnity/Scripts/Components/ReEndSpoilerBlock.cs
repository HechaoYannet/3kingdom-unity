using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndSpoilerBlock : ReEndBaseComponent
    {
        public string Text { get; set; } = "Spoiler Content";
        public bool Revealed { get; set; }

        public TMP_Text ContentText { get; private set; }
        private Button _btn;

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.AddComponent<Image>();

            ContentText = CreateChild<TextMeshProUGUI>(transform, "Content");
            ContentText.alignment = TextAlignmentOptions.Center;
            Stretch(ContentText.rectTransform);

            _btn = gameObject.AddComponent<Button>();
            _btn.onClick.AddListener(() => { Revealed = !Revealed; ApplyTheme(); });

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 40);
        }

        public override void ApplyTheme()
        {
            if (Revealed)
            {
                BackgroundImage.color = new Color(0, 0, 0, 0);
                ContentText.text = Text;
                ContentText.fontSize = Theme.bodySize;
                ContentText.color = Theme.textPrimary;
            }
            else
            {
                BackgroundImage.color = Theme.foreground;
                ContentText.text = "";
            }
        }

        public ReEndSpoilerBlock SetText(string t) { Text = t; if (_built) ApplyTheme(); return this; }
    }
}
