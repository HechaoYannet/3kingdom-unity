using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndAlert : ReEndBaseComponent
    {
        public string Message { get; set; } = "";
        public ReEndSeverity Severity { get; set; } = ReEndSeverity.Info;
        public bool Dismissible { get; set; }
        public System.Action OnDismiss { get; set; }

        public TMP_Text MessageText { get; private set; }
        private Button _dismissBtn;

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.GetComponent<Image>() ?? gameObject.AddComponent<Image>();
            var mat = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
            mat.SetFloat("_CornerSize", 0.04f);
            BackgroundImage.material = mat;

            // Message
            var msgGo = CreateChild(transform, "Message");
            MessageText = msgGo.AddComponent<TextMeshProUGUI>();
            MessageText.alignment = TextAlignmentOptions.Left;
            var mRT = MessageText.rectTransform;
            mRT.offsetMin = new Vector2(12, 8);
            mRT.offsetMax = new Vector2(-40, -8);
            Stretch(mRT);

            // Dismiss
            var dismissGo = CreateChild(transform, "Dismiss");
            var dismissImg = dismissGo.AddComponent<Image>();
            var dismissTxt = CreateChild<TextMeshProUGUI>(dismissGo.transform, "X");
            dismissTxt.text = "×";
            dismissTxt.fontSize = 16;
            dismissTxt.alignment = TextAlignmentOptions.Center;
            Stretch(dismissTxt.rectTransform);
            _dismissBtn = dismissGo.AddComponent<Button>();
            _dismissBtn.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                OnDismiss?.Invoke();
            });
            var dRT = dismissImg.rectTransform;
            dRT.anchorMin = new Vector2(1, 0.5f);
            dRT.anchorMax = new Vector2(1, 0.5f);
            dRT.pivot = new Vector2(1, 0.5f);
            dRT.sizeDelta = new Vector2(24, 24);
            dRT.anchoredPosition = new Vector2(-8, 0);
            dismissGo.SetActive(Dismissible);

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 44);
        }

        public override void ApplyTheme()
        {
            (Color bg, Color fg, Color accent) = Severity switch
            {
                ReEndSeverity.Success => (Theme.greenSoft, Theme.efGreen, Theme.efGreen),
                ReEndSeverity.Warning => (Theme.orangeSoft, Theme.efOrange, Theme.efOrange),
                ReEndSeverity.Error => (Theme.redSoft, Theme.efRed, Theme.efRed),
                ReEndSeverity.Info => (new Color(0.3f, 0.67f, 0.85f, 0.1f), Theme.efBlue, Theme.efBlue),
                ReEndSeverity.Caution => (new Color(1f, 0.83f, 0.16f, 0.1f), Theme.efYellow, Theme.efYellow),
                ReEndSeverity.Critical => (Theme.redSoft, Theme.efRed, Theme.efRed),
                _ => (Theme.surface1, Theme.textPrimary, Theme.primary)
            };

            BackgroundImage.color = bg;
            MessageText.text = $"◆ {Message}";
            MessageText.fontSize = Theme.bodySmSize;
            MessageText.color = fg;
            _dismissBtn.gameObject.SetActive(Dismissible);
        }

        public ReEndAlert SetMessage(string m) { Message = m; if (_built) ApplyTheme(); return this; }
        public ReEndAlert SetSeverity(ReEndSeverity s) { Severity = s; if (_built) ApplyTheme(); return this; }
        public ReEndAlert SetDismissible(bool d = true) { Dismissible = d; if (_built) ApplyTheme(); return this; }
        public ReEndAlert SetOnDismiss(System.Action cb) { OnDismiss = cb; return this; }
    }
}
