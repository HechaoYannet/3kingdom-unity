using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndThemeSwitcher : ReEndBaseComponent
    {
        public bool IsDark { get; set; } = true;

        private Button _btn;
        private TMP_Text _label;

        protected override void BuildInternal()
        {
            _btn = gameObject.AddComponent<Button>();
            _btn.onClick.AddListener(Toggle);

            _label = CreateChild<TextMeshProUGUI>(transform, "Label");
            _label.alignment = TextAlignmentOptions.Center;
            _label.raycastTarget = false;
            Stretch(_label.rectTransform);

            RectTransform.sizeDelta = new Vector2(40, 40);
        }

        private void Toggle()
        {
            IsDark = !IsDark;
            ApplyTheme();
            // Load appropriate theme
            var themeName = IsDark ? "ReEndTheme-Dark" : "ReEndTheme-Light";
            var theme = Resources.Load<ReEndTheme>(themeName);
            if (theme != null) ReEndThemeManager.Instance.SetTheme(theme);
        }

        public override void ApplyTheme()
        {
            _label.text = IsDark ? "☽" : "☀";
            _label.fontSize = 20;
            _label.color = Theme.efYellow;
        }
    }
}
