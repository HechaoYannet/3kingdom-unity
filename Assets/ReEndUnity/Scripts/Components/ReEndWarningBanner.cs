using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndWarningBanner : ReEndBaseComponent
    {
        public string Message { get; set; } = "WARNING";
        public ReEndSeverity Severity { get; set; } = ReEndSeverity.Warning;
        public System.Action OnAction { get; set; }
        public string ActionLabel { get; set; }

        public TMP_Text MessageText { get; private set; }

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.AddComponent<Image>();
            BackgroundImage.color = Theme.orangeSoft;

            MessageText = CreateChild<TextMeshProUGUI>(transform, "Message");
            MessageText.alignment = TextAlignmentOptions.Left;
            MessageText.raycastTarget = false;
            var mRT = MessageText.rectTransform;
            mRT.offsetMin = new Vector2(16, 0);
            mRT.offsetMax = new Vector2(-16, 0);
            Stretch(mRT);

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 600);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 44);
        }

        public override void ApplyTheme()
        {
            (Color bg, Color fg) = Severity switch
            {
                ReEndSeverity.Caution => (new Color(1f, 0.83f, 0.16f, 0.1f), Theme.efYellow),
                ReEndSeverity.Alert => (Theme.orangeSoft, Theme.efOrange),
                ReEndSeverity.Critical => (Theme.redSoft, Theme.efRed),
                _ => (Theme.orangeSoft, Theme.efOrange)
            };

            BackgroundImage.color = bg;
            MessageText.text = $"⚠ {Message}";
            MessageText.fontSize = Theme.bodySmSize;
            MessageText.color = fg;

            if (!string.IsNullOrEmpty(ActionLabel) && OnAction != null)
            {
                var existing = transform.Find("ActionBtn");
                if (existing == null)
                {
                    var btn = ReEndUI.Button(transform, ActionLabel).SetVariant(ReEndVariant.Outline).SetSize(ReEndSize.Xs).SetOnClick(OnAction);
                    btn.RectTransform.anchorMin = new Vector2(1, 0.5f);
                    btn.RectTransform.anchorMax = new Vector2(1, 0.5f);
                    btn.RectTransform.pivot = new Vector2(1, 0.5f);
                    btn.RectTransform.anchoredPosition = new Vector2(-8, 0);
                }
            }
        }

        public ReEndWarningBanner SetMessage(string m) { Message = m; if (_built) ApplyTheme(); return this; }
        public ReEndWarningBanner SetSeverity(ReEndSeverity s) { Severity = s; if (_built) ApplyTheme(); return this; }
        public ReEndWarningBanner SetAction(string label, System.Action cb) { ActionLabel = label; OnAction = cb; return this; }
    }
}
