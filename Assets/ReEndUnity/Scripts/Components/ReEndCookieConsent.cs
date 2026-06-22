using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndCookieConsent : ReEndBaseComponent
    {
        public string Message { get; set; } = "This site uses cookies to enhance your experience.";
        public System.Action OnAccept { get; set; }
        public System.Action OnReject { get; set; }
        public System.Action OnCustomize { get; set; }

        public TMP_Text MessageText { get; private set; }

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.AddComponent<Image>();
            BackgroundImage.color = Theme.card;

            RectTransform.anchorMin = new Vector2(0, 0);
            RectTransform.anchorMax = new Vector2(1, 0);
            RectTransform.pivot = new Vector2(0.5f, 0);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 72);
            Stretch(RectTransform);

            MessageText = CreateChild<TextMeshProUGUI>(transform, "Message");
            MessageText.alignment = TextAlignmentOptions.Left;
            var mRT = MessageText.rectTransform;
            mRT.offsetMin = new Vector2(16, 8);
            mRT.offsetMax = new Vector2(-280, -8);
            Stretch(mRT);

            // Buttons
            var accept = ReEndUI.Button(transform, "Accept").SetVariant(ReEndVariant.Primary).SetSize(ReEndSize.Xs);
            accept.SetOnClick(() => { OnAccept?.Invoke(); Destroy(gameObject); });
            accept.RectTransform.anchorMin = new Vector2(1, 0.5f);
            accept.RectTransform.anchorMax = new Vector2(1, 0.5f);
            accept.RectTransform.pivot = new Vector2(1, 0.5f);
            accept.RectTransform.anchoredPosition = new Vector2(-8, 0);

            var reject = ReEndUI.Button(transform, "Reject").SetVariant(ReEndVariant.Outline).SetSize(ReEndSize.Xs);
            reject.SetOnClick(() => { OnReject?.Invoke(); Destroy(gameObject); });
            reject.RectTransform.anchorMin = new Vector2(1, 0.5f);
            reject.RectTransform.anchorMax = new Vector2(1, 0.5f);
            reject.RectTransform.pivot = new Vector2(1, 0.5f);
            reject.RectTransform.anchoredPosition = new Vector2(-accept.RectTransform.rect.width - 16, 0);
        }

        public override void ApplyTheme()
        {
            MessageText.text = Message;
            MessageText.fontSize = Theme.bodySmSize;
            MessageText.color = Theme.textSecondary;
        }
    }
}
