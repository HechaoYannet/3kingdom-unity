using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndSwitch : ReEndBaseComponent
    {
        public bool IsOn { get; set; }
        public string LabelText { get; set; } = "";
        public string OnLabel { get; set; }
        public string OffLabel { get; set; }
        public System.Action<bool> OnValueChanged { get; set; }

        public Image Thumb { get; private set; }
        public Image Track { get; private set; }
        public TMP_Text Label { get; private set; }
        public TMP_Text StateLabel { get; private set; }
        private Button _btn;

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 28);

            // Track
            var trackGo = CreateChild(transform, "Track");
            Track = trackGo.AddComponent<Image>();
            _btn = trackGo.AddComponent<Button>();
            _btn.onClick.AddListener(() =>
            {
                IsOn = !IsOn;
                ApplyTheme();
                OnValueChanged?.Invoke(IsOn);
                ReEndAnimationHelper.ScaleIn(Thumb.rectTransform, Theme.durationFast);
            });
            var trRT = Track.rectTransform;
            trRT.anchorMin = new Vector2(1, 0.5f);
            trRT.anchorMax = new Vector2(1, 0.5f);
            trRT.pivot = new Vector2(1, 0.5f);
            trRT.sizeDelta = new Vector2(44, 24);
            trRT.anchoredPosition = Vector2.zero;

            // Thumb (diamond)
            var thumbGo = CreateChild(transform, "Thumb");
            Thumb = thumbGo.AddComponent<Image>();
            Thumb.raycastTarget = false;
            var thRT = Thumb.rectTransform;
            thRT.anchorMin = new Vector2(1, 0.5f);
            thRT.anchorMax = new Vector2(1, 0.5f);
            thRT.pivot = new Vector2(0.5f, 0.5f);
            thRT.sizeDelta = new Vector2(18, 18);

            // Label
            var labGo = CreateChild(transform, "Label");
            Label = labGo.AddComponent<TextMeshProUGUI>();
            Label.alignment = TextAlignmentOptions.Left;
            Label.raycastTarget = false;
            var labRT = Label.rectTransform;
            labRT.anchorMin = new Vector2(0, 0);
            labRT.anchorMax = new Vector2(1, 1);
            labRT.offsetMax = new Vector2(-52, 0);

            // State label
            var stGo = CreateChild(transform, "StateLabel");
            StateLabel = stGo.AddComponent<TextMeshProUGUI>();
            StateLabel.alignment = TextAlignmentOptions.Right;
            StateLabel.raycastTarget = false;
            var stRT = StateLabel.rectTransform;
            stRT.anchorMin = new Vector2(1, 0.5f);
            stRT.anchorMax = new Vector2(1, 0.5f);
            stRT.pivot = new Vector2(1, 0.5f);
            stRT.sizeDelta = new Vector2(60, 20);
            stRT.anchoredPosition = new Vector2(-52, 0);
        }

        public override void ApplyTheme()
        {
            Track.color = IsOn ? Theme.primary : Theme.surface3;
            Thumb.color = IsOn ? Theme.primaryForeground : Theme.efWhiteMuted;

            var thRT = Thumb.rectTransform;
            thRT.anchoredPosition = IsOn ? new Vector2(-6, 0) : new Vector2(-38, 0);

            Label.text = LabelText;
            Label.fontSize = Theme.bodySmSize;
            Label.color = Theme.textPrimary;

            StateLabel.text = IsOn ? (OnLabel ?? "ON") : (OffLabel ?? "OFF");
            StateLabel.fontSize = Theme.captionSize;
            StateLabel.color = IsOn ? Theme.primary : Theme.textMuted;
        }

        public ReEndSwitch SetOn(bool on = true) { IsOn = on; if (_built) ApplyTheme(); return this; }
        public ReEndSwitch SetLabel(string l) { LabelText = l; if (_built) ApplyTheme(); return this; }
        public ReEndSwitch SetLabels(string on, string off) { OnLabel = on; OffLabel = off; if (_built) ApplyTheme(); return this; }
        public ReEndSwitch SetOnValueChanged(System.Action<bool> cb) { OnValueChanged = cb; return this; }
    }
}
