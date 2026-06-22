using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndDatePicker : ReEndBaseComponent
    {
        public DateTime Value { get; set; } = DateTime.Today;
        public DateTime MinDate { get; set; } = DateTime.MinValue;
        public DateTime MaxDate { get; set; } = DateTime.MaxValue;
        public string DisplayFormat { get; set; } = "yyyy.MM.dd";
        public System.Action<DateTime> OnValueChanged { get; set; }

        public TMP_Text DisplayText { get; private set; }
        public Image CalendarIcon { get; private set; }
        private Button _btn;
        private GameObject _calendarPanel;

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.GetComponent<Image>() ?? gameObject.AddComponent<Image>();
            _btn = gameObject.AddComponent<Button>();
            _btn.onClick.AddListener(ToggleCalendar);

            var textGo = CreateChild(transform, "DisplayText");
            DisplayText = textGo.AddComponent<TextMeshProUGUI>();
            DisplayText.alignment = TextAlignmentOptions.Left;
            DisplayText.raycastTarget = false;
            var tRT = DisplayText.rectTransform;
            tRT.offsetMin = new Vector2(12, 0);
            tRT.offsetMax = new Vector2(-36, 0);
            Stretch(tRT);

            var iconGo = CreateChild(transform, "Icon");
            CalendarIcon = iconGo.AddComponent<Image>();
            CalendarIcon.raycastTarget = false;
            var iRT = CalendarIcon.rectTransform;
            iRT.anchorMin = new Vector2(1, 0.5f);
            iRT.anchorMax = new Vector2(1, 0.5f);
            iRT.pivot = new Vector2(1, 0.5f);
            iRT.sizeDelta = new Vector2(20, 20);
            iRT.anchoredPosition = new Vector2(-8, 0);

            _calendarPanel = CreateChild(transform, "Calendar");
            _calendarPanel.SetActive(false);
            BuildCalendar();
        }

        private void BuildCalendar()
        {
            // Simplified: just a set of quick-pick buttons
            var today = DateTime.Today;
            AddQuickDate("Today", today);
            AddQuickDate("Tomorrow", today.AddDays(1));
            AddQuickDate("+3 Days", today.AddDays(3));
            AddQuickDate("+1 Week", today.AddDays(7));
        }

        private void AddQuickDate(string label, DateTime date)
        {
            var btn = ReEndUI.Button(_calendarPanel.transform, label)
                .SetVariant(ReEndVariant.Ghost)
                .SetSize(ReEndSize.Sm);
            btn.SetOnClick(() =>
            {
                Value = date;
                ApplyTheme();
                _calendarPanel.SetActive(false);
                OnValueChanged?.Invoke(Value);
            });
            btn.RectTransform.anchoredPosition = new Vector2(0, -60);
        }

        private void ToggleCalendar() { _calendarPanel.SetActive(!_calendarPanel.activeSelf); }

        public override void ApplyTheme()
        {
            float h = Theme.GetSizeValue(ReEndSize.Md);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);

            BackgroundImage.color = Theme.surface2;
            DisplayText.text = Value.ToString(DisplayFormat);
            DisplayText.fontSize = Theme.bodySize;
            DisplayText.color = Theme.textPrimary;
        }

        public ReEndDatePicker SetValue(DateTime v) { Value = v; if (_built) ApplyTheme(); return this; }
        public ReEndDatePicker SetRange(DateTime min, DateTime max) { MinDate = min; MaxDate = max; return this; }
        public ReEndDatePicker SetOnValueChanged(System.Action<DateTime> cb) { OnValueChanged = cb; return this; }
    }
}
