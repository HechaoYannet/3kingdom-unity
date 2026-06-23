using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndTacticalBadge : ReEndBaseComponent
    {
        public string Text { get; set; } = "STATUS";
        public ReEndStatus Status { get; set; } = ReEndStatus.Default;

        public TMP_Text Label { get; private set; }
        public Image Dot { get; private set; }

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.AddComponent<Image>();
            var mat = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
            mat.SetFloat("_CornerSize", 0.04f);
            BackgroundImage.material = mat;

            // Status dot
            var dotGo = CreateChild(transform, "Dot");
            Dot = dotGo.AddComponent<Image>();
            var dRT = Dot.rectTransform;
            dRT.anchorMin = new Vector2(0, 0.5f);
            dRT.anchorMax = new Vector2(0, 0.5f);
            dRT.pivot = new Vector2(0, 0.5f);
            dRT.sizeDelta = new Vector2(8, 8);
            dRT.anchoredPosition = new Vector2(8, 0);

            Label = CreateChild<TextMeshProUGUI>(transform, "Label");
            Label.alignment = TextAlignmentOptions.Left;
            Label.raycastTarget = false;
            var lRT = Label.rectTransform;
            Stretch(lRT);
            lRT.offsetMin = new Vector2(22, 0);
            lRT.offsetMax = new Vector2(-8, 0);

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 120);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 28);
        }

        public override void ApplyTheme()
        {
            Color c = Status switch
            {
                ReEndStatus.Success => Theme.efGreen,
                ReEndStatus.Warning => Theme.efOrange,
                ReEndStatus.Danger => Theme.efRed,
                ReEndStatus.Info => Theme.efBlue,
                ReEndStatus.Online => Theme.efGreen,
                ReEndStatus.Offline => Theme.efGrayMid,
                _ => Theme.primary
            };

            BackgroundImage.color = new Color(c.r, c.g, c.b, 0.1f);
            Dot.color = c;
            Label.text = Text.ToUpper();
            Label.fontSize = Theme.captionSize;
            Label.color = c;
        }

        public ReEndTacticalBadge SetText(string t) { Text = t; if (_built) ApplyTheme(); return this; }
        public ReEndTacticalBadge SetStatus(ReEndStatus s) { Status = s; if (_built) ApplyTheme(); return this; }
    }
}
