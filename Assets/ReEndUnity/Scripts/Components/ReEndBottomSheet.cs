using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndBottomSheet : ReEndBaseComponent
    {
        public string Title { get; set; } = "";
        public ReEndSnapPoint SnapPoint { get; set; } = ReEndSnapPoint.Half;
        public System.Action OnDismiss { get; set; }

        public TMP_Text TitleText { get; private set; }
        public RectTransform ContentSlot { get; private set; }
        private Image _handle;

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.AddComponent<Image>();
            BackgroundImage.color = Theme.card;
            var mat = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
            mat.SetFloat("_CornerSize", 0.12f);
            BackgroundImage.material = mat;

            RectTransform.anchorMin = new Vector2(0, 0);
            RectTransform.anchorMax = new Vector2(1, 0);
            RectTransform.pivot = new Vector2(0.5f, 0);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 300);
            Stretch(RectTransform);

            // Handle
            var handleGo = CreateChild(transform, "Handle");
            _handle = handleGo.AddComponent<Image>();
            _handle.color = Theme.borderDefault;
            var hRT = _handle.rectTransform;
            hRT.anchorMin = new Vector2(0.5f, 1);
            hRT.anchorMax = new Vector2(0.5f, 1);
            hRT.sizeDelta = new Vector2(40, 4);
            hRT.anchoredPosition = new Vector2(0, -8);

            // Title
            TitleText = CreateChild<TextMeshProUGUI>(transform, "Title");
            TitleText.alignment = TextAlignmentOptions.Center;
            var tRT = TitleText.rectTransform;
            tRT.anchorMin = new Vector2(0, 1);
            tRT.anchorMax = new Vector2(1, 1);
            tRT.pivot = new Vector2(0.5f, 1);
            tRT.sizeDelta = new Vector2(-32, 28);
            tRT.anchoredPosition = new Vector2(0, -16);

            // Content
            var slotGo = CreateChild(transform, "Content");
            ContentSlot = slotGo.GetComponent<RectTransform>();
            ContentSlot.anchorMin = new Vector2(0, 0);
            ContentSlot.anchorMax = new Vector2(1, 1);
            ContentSlot.offsetMin = new Vector2(16, 16);
            ContentSlot.offsetMax = new Vector2(-16, -56);

            gameObject.SetActive(false);
        }

        public override void ApplyTheme()
        {
            TitleText.text = Title;
            TitleText.fontSize = Theme.h4Size;
            TitleText.color = Theme.textPrimary;

            float h = SnapPoint switch
            {
                ReEndSnapPoint.Hidden => 0,
                ReEndSnapPoint.Quarter => Screen.height * 0.25f,
                ReEndSnapPoint.Half => Screen.height * 0.5f,
                ReEndSnapPoint.Full => Screen.height * 0.9f,
                _ => Screen.height * 0.5f
            };
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
        }

        public ReEndBottomSheet SetTitle(string t) { Title = t; if (_built) ApplyTheme(); return this; }
        public ReEndBottomSheet SetSnapPoint(ReEndSnapPoint s) { SnapPoint = s; if (_built) ApplyTheme(); return this; }

        public void Show() { gameObject.SetActive(true); EnsureBuilt(); ApplyTheme(); }
        public void Hide() { gameObject.SetActive(false); OnDismiss?.Invoke(); }
    }
}
