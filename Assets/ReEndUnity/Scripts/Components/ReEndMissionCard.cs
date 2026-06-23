using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndMissionCard : ReEndBaseComponent
    {
        public string Title { get; set; } = "MISSION";
        public string Description { get; set; }
        public float Progress { get; set; }
        public string StatusText { get; set; }
        public string RewardText { get; set; }
        public System.Action OnClick { get; set; }

        public TMP_Text TitleText { get; private set; }
        public TMP_Text DescText { get; private set; }
        public TMP_Text StatusLabel { get; private set; }
        public Image ProgressBar { get; private set; }
        private Image _progressFill;
        private Button _btn;

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.AddComponent<Image>();
            BackgroundImage.material = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
            BackgroundImage.material.SetFloat("_CornerSize", 0.08f);

            _btn = gameObject.AddComponent<Button>();
            _btn.onClick.AddListener(() => OnClick?.Invoke());

            TitleText = CreateChild<TextMeshProUGUI>(transform, "Title");
            TitleText.alignment = TextAlignmentOptions.Left;
            TitleText.raycastTarget = false;
            var tRT = TitleText.rectTransform;
            tRT.anchorMin = new Vector2(0, 1);
            tRT.sizeDelta = new Vector2(-24, 22);
            tRT.anchoredPosition = new Vector2(12, -12);

            DescText = CreateChild<TextMeshProUGUI>(transform, "Description");
            DescText.alignment = TextAlignmentOptions.Left;
            DescText.raycastTarget = false;
            var dRT = DescText.rectTransform;
            dRT.anchorMin = new Vector2(0, 1);
            dRT.sizeDelta = new Vector2(-24, 16);
            dRT.anchoredPosition = new Vector2(12, -36);

            // Progress bar
            ProgressBar = CreateChild<Image>(transform, "ProgressBg");
            ProgressBar.color = Theme.surface3;
            ProgressBar.raycastTarget = false;
            var pRT = ProgressBar.rectTransform;
            pRT.anchorMin = new Vector2(0, 0);
            pRT.sizeDelta = new Vector2(-24, 6);
            pRT.anchoredPosition = new Vector2(12, 44);

            _progressFill = CreateChild<Image>(ProgressBar.transform, "Fill");
            _progressFill.color = Theme.primary;
            _progressFill.raycastTarget = false;
            var fRT = _progressFill.rectTransform;
            fRT.anchorMin = Vector2.zero;
            fRT.anchorMax = Vector2.one;
            fRT.pivot = new Vector2(0, 0.5f);
            fRT.sizeDelta = Vector2.zero;

            StatusLabel = CreateChild<TextMeshProUGUI>(transform, "Status");
            StatusLabel.alignment = TextAlignmentOptions.Right;
            StatusLabel.raycastTarget = false;
            var sRT = StatusLabel.rectTransform;
            sRT.anchorMin = new Vector2(1, 0);
            sRT.sizeDelta = new Vector2(100, 16);
            sRT.anchoredPosition = new Vector2(-12, 44);

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 120);
        }

        public override void ApplyTheme()
        {
            BackgroundImage.color = Theme.card;
            TitleText.text = Title.ToUpper();
            TitleText.fontSize = Theme.bodySize;
            TitleText.color = Theme.textPrimary;
            DescText.text = Description;
            DescText.fontSize = Theme.bodySmSize;
            DescText.color = Theme.textSecondary;
            StatusLabel.text = StatusText;
            StatusLabel.fontSize = Theme.captionSize;
            StatusLabel.color = Theme.textMuted;

            float pct = Mathf.Clamp01(Progress);
            _progressFill.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pct * (RectTransform.rect.width - 24));
        }

        public ReEndMissionCard SetTitle(string t) { Title = t; if (_built) ApplyTheme(); return this; }
        public ReEndMissionCard SetDescription(string d) { Description = d; if (_built) ApplyTheme(); return this; }
        public ReEndMissionCard SetProgress(float p) { Progress = p; if (_built) ApplyTheme(); return this; }
        public ReEndMissionCard SetOnClick(System.Action cb) { OnClick = cb; return this; }
    }
}
