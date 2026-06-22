using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndAccordion : ReEndBaseComponent
    {
        public string Title { get; set; } = "Accordion";
        public bool Expanded { get; set; }
        public System.Action<bool> OnToggle { get; set; }

        public TMP_Text TitleText { get; private set; }
        public TMP_Text Indicator { get; private set; }
        public RectTransform ContentArea { get; private set; }
        private Button _btn;
        private CanvasGroup _contentGroup;
        private float _contentHeight;

        protected override void BuildInternal()
        {
            // Header
            var headerGo = CreateChild(transform, "Header");
            var headerRT = headerGo.GetComponent<RectTransform>();
            headerRT.anchorMin = new Vector2(0, 1);
            headerRT.anchorMax = new Vector2(1, 1);
            headerRT.pivot = new Vector2(0, 1);
            headerRT.sizeDelta = new Vector2(0, 44);
            headerRT.anchoredPosition = Vector2.zero;

            var headerImg = headerGo.AddComponent<Image>();
            headerImg.color = Theme.surface1;
            _btn = headerGo.AddComponent<Button>();
            _btn.onClick.AddListener(Toggle);

            TitleText = CreateChild<TextMeshProUGUI>(headerGo.transform, "Title");
            TitleText.alignment = TextAlignmentOptions.Left;
            TitleText.raycastTarget = false;
            var tRT = TitleText.rectTransform;
            tRT.offsetMin = new Vector2(12, 0);
            tRT.offsetMax = new Vector2(-48, 0);
            Stretch(tRT);

            Indicator = CreateChild<TextMeshProUGUI>(headerGo.transform, "Indicator");
            Indicator.text = "+";
            Indicator.alignment = TextAlignmentOptions.Center;
            Indicator.fontSize = 20;
            Indicator.raycastTarget = false;
            var iRT = Indicator.rectTransform;
            iRT.anchorMin = new Vector2(1, 0.5f);
            iRT.anchorMax = new Vector2(1, 0.5f);
            iRT.pivot = new Vector2(1, 0.5f);
            iRT.sizeDelta = new Vector2(24, 24);
            iRT.anchoredPosition = new Vector2(-12, 0);

            // Content
            var contentGo = CreateChild(transform, "Content");
            ContentArea = contentGo.GetComponent<RectTransform>();
            ContentArea.anchorMin = new Vector2(0, 1);
            ContentArea.anchorMax = new Vector2(1, 1);
            ContentArea.pivot = new Vector2(0, 1);
            ContentArea.anchoredPosition = new Vector2(0, -44);
            ContentArea.sizeDelta = new Vector2(0, 100);
            _contentGroup = contentGo.AddComponent<CanvasGroup>();
            _contentHeight = 100;
        }

        private void Toggle()
        {
            Expanded = !Expanded;
            ApplyTheme();
            OnToggle?.Invoke(Expanded);
        }

        public override void ApplyTheme()
        {
            TitleText.text = Title;
            TitleText.fontSize = Theme.bodySize;
            TitleText.color = Theme.textPrimary;
            Indicator.text = Expanded ? "−" : "+";
            Indicator.color = Theme.primary;

            if (_contentGroup != null)
            {
                _contentGroup.alpha = Expanded ? 1 : 0;
                _contentGroup.interactable = Expanded;
                _contentGroup.blocksRaycasts = Expanded;
            }

            ContentArea.sizeDelta = new Vector2(0, Expanded ? _contentHeight : 0);
            Indicator.rectTransform.localRotation = Quaternion.Euler(0, 0, Expanded ? 0 : 0);
        }

        public ReEndAccordion SetTitle(string t) { Title = t; if (_built) ApplyTheme(); return this; }
        public ReEndAccordion SetExpanded(bool e) { Expanded = e; if (_built) ApplyTheme(); return this; }
        public ReEndAccordion SetContentHeight(float h) { _contentHeight = h; if (_built) ApplyTheme(); return this; }
        public ReEndAccordion SetOnToggle(System.Action<bool> cb) { OnToggle = cb; return this; }
        public ReEndAccordion AddChild(Transform child) { child.SetParent(ContentArea, false); return this; }
    }
}
