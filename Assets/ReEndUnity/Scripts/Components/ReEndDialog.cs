using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndDialog : ReEndBaseComponent
    {
        public string Title { get; set; } = "Dialog";
        public string Message { get; set; }
        public string ConfirmText { get; set; } = "Confirm";
        public string CancelText { get; set; } = "Cancel";
        public ReEndDialogSize Size { get; set; } = ReEndDialogSize.Md;
        public bool IsOpen { get; set; }
        public System.Action OnConfirm { get; set; }
        public System.Action OnCancel { get; set; }
        public System.Action<bool> OnClose { get; set; }

        public TMP_Text TitleText { get; private set; }
        public TMP_Text MessageText { get; private set; }
        public RectTransform ContentSlot { get; private set; }
        public ReEndButton ConfirmBtn { get; private set; }
        public ReEndButton CancelBtn { get; private set; }
        private Image _overlay;

        protected override void BuildInternal()
        {
            // Overlay (full screen)
            var overlayGo = CreateChild(transform, "Overlay");
            _overlay = overlayGo.AddComponent<Image>();
            _overlay.color = new Color(0, 0, 0, 0.85f);
            _overlay.raycastTarget = true;
            Stretch(_overlay.rectTransform);

            // Panel
            BackgroundImage = gameObject.GetComponent<Image>() ?? gameObject.AddComponent<Image>();
            var mat = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
            mat.SetFloat("_CornerSize", 0.12f);
            BackgroundImage.material = mat;

            float w = Size switch
            {
                ReEndDialogSize.Xs => 320, ReEndDialogSize.Sm => 400,
                ReEndDialogSize.Md => 480, ReEndDialogSize.Lg => 600,
                ReEndDialogSize.Xl => 720, _ => 480
            };
            RectTransform.sizeDelta = new Vector2(w, 300);

            // Title
            var titleGo = CreateChild(transform, "Title");
            TitleText = titleGo.AddComponent<TextMeshProUGUI>();
            TitleText.alignment = TextAlignmentOptions.Left;
            var tRT = TitleText.rectTransform;
            tRT.anchorMin = new Vector2(0, 1);
            tRT.anchorMax = new Vector2(1, 1);
            tRT.pivot = new Vector2(0, 1);
            tRT.sizeDelta = new Vector2(-32, 32);
            tRT.anchoredPosition = new Vector2(16, -16);

            // Message
            var msgGo = CreateChild(transform, "Message");
            MessageText = msgGo.AddComponent<TextMeshProUGUI>();
            MessageText.alignment = TextAlignmentOptions.Left;
            var mRT = MessageText.rectTransform;
            mRT.anchorMin = new Vector2(0, 1);
            mRT.anchorMax = new Vector2(1, 1);
            mRT.pivot = new Vector2(0, 1);
            mRT.sizeDelta = new Vector2(-32, 60);
            mRT.anchoredPosition = new Vector2(16, -52);

            // Content slot
            var slotGo = CreateChild(transform, "ContentSlot");
            ContentSlot = slotGo.GetComponent<RectTransform>();
            ContentSlot.anchorMin = new Vector2(0, 0);
            ContentSlot.anchorMax = new Vector2(1, 1);
            ContentSlot.offsetMin = new Vector2(16, 64);
            ContentSlot.offsetMax = new Vector2(-16, -60);

            // Buttons
            ConfirmBtn = ReEndUI.Button(transform, ConfirmText).SetVariant(ReEndVariant.Primary).SetSize(ReEndSize.Sm);
            ConfirmBtn.SetOnClick(Close);
            var cbRT = ConfirmBtn.RectTransform;
            cbRT.anchorMin = new Vector2(1, 0);
            cbRT.anchorMax = new Vector2(1, 0);
            cbRT.pivot = new Vector2(1, 0);
            cbRT.anchoredPosition = new Vector2(-12, 12);

            CancelBtn = ReEndUI.Button(transform, CancelText).SetVariant(ReEndVariant.Outline).SetSize(ReEndSize.Sm);
            CancelBtn.SetOnClick(() =>
            {
                IsOpen = false;
                gameObject.SetActive(false);
                OnCancel?.Invoke();
                OnClose?.Invoke(false);
            });
            var ccRT = CancelBtn.RectTransform;
            ccRT.anchorMin = new Vector2(1, 0);
            ccRT.anchorMax = new Vector2(1, 0);
            ccRT.pivot = new Vector2(1, 0);
            ccRT.anchoredPosition = new Vector2(-140, 12); // 初始位置，ApplyTheme 中会根据 ConfirmBtn 宽度修正

            gameObject.SetActive(false);
        }

        private void Close()
        {
            IsOpen = false;
            gameObject.SetActive(false);
            OnConfirm?.Invoke();
            OnClose?.Invoke(true);
        }

        public override void ApplyTheme()
        {
            BackgroundImage.color = Theme.card;
            TitleText.text = Title;
            TitleText.fontSize = Theme.h4Size;
            TitleText.color = Theme.textPrimary;
            MessageText.text = Message;
            MessageText.fontSize = Theme.bodySize;
            MessageText.color = Theme.textSecondary;
            ConfirmBtn.SetText(ConfirmText).RefreshTheme();
            CancelBtn.SetText(CancelText).RefreshTheme();
            // 在按钮 ApplyTheme 后修正 CancelBtn 位置（此时 ConfirmBtn 宽度已确定）
            var cbRT = ConfirmBtn.RectTransform;
            CancelBtn.RectTransform.anchoredPosition = new Vector2(-cbRT.rect.width - 20, 12);
        }

        public ReEndDialog SetTitle(string t) { Title = t; if (_built) ApplyTheme(); return this; }
        public ReEndDialog SetMessage(string m) { Message = m; if (_built) ApplyTheme(); return this; }
        public ReEndDialog SetConfirm(string label) { ConfirmText = label; if (_built) ApplyTheme(); return this; }
        public ReEndDialog SetCancel(string label) { CancelText = label; if (_built) ApplyTheme(); return this; }
        public ReEndDialog SetSize(ReEndDialogSize s) { Size = s; return this; }
        public ReEndDialog SetOnConfirm(System.Action cb) { OnConfirm = cb; return this; }
        public ReEndDialog SetOnCancel(System.Action cb) { OnCancel = cb; return this; }
        public ReEndDialog SetOnClose(System.Action<bool> cb) { OnClose = cb; return this; }

        public void Open()
        {
            IsOpen = true;
            gameObject.SetActive(true);
            EnsureBuilt();
            ApplyTheme();
        }
    }
}
