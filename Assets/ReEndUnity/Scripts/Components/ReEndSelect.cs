using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndSelect : ReEndBaseComponent
    {
        public string Value { get; set; }
        public string Placeholder { get; set; } = "Select...";
        public List<string> Options { get; private set; } = new();
        public System.Action<string> OnValueChanged { get; set; }

        public TMP_Text DisplayText { get; private set; }
        public Image ArrowIcon { get; private set; }
        private Button _btn;
        private GameObject _dropdownContent;
        private bool _open;

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.GetComponent<Image>() ?? gameObject.AddComponent<Image>();
            _btn = gameObject.AddComponent<Button>();
            _btn.onClick.AddListener(ToggleOpen);

            var textGo = CreateChild(transform, "DisplayText");
            DisplayText = textGo.AddComponent<TextMeshProUGUI>();
            DisplayText.alignment = TextAlignmentOptions.Left;
            DisplayText.raycastTarget = false;
            var tRT = DisplayText.rectTransform;
            tRT.offsetMin = new Vector2(12, 0);
            tRT.offsetMax = new Vector2(-32, 0);
            Stretch(tRT);

            var arrowGo = CreateChild(transform, "Arrow");
            ArrowIcon = arrowGo.AddComponent<Image>();
            ArrowIcon.raycastTarget = false;
            var aRT = ArrowIcon.rectTransform;
            aRT.anchorMin = new Vector2(1, 0.5f);
            aRT.anchorMax = new Vector2(1, 0.5f);
            aRT.pivot = new Vector2(1, 0.5f);
            aRT.sizeDelta = new Vector2(16, 16);
            aRT.anchoredPosition = new Vector2(-8, 0);

            _dropdownContent = CreateChild(transform, "DropdownContent");
            _dropdownContent.SetActive(false);
        }

        private void ToggleOpen()
        {
            _open = !_open;
            _dropdownContent.SetActive(_open);
        }

        public override void ApplyTheme()
        {
            float h = Theme.GetSizeValue(ReEndSize.Md);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);

            BackgroundImage.color = Theme.surface2;
            DisplayText.text = string.IsNullOrEmpty(Value) ? Placeholder : Value;
            DisplayText.fontSize = Theme.bodySize;
            DisplayText.color = string.IsNullOrEmpty(Value) ? Theme.textPlaceholder : Theme.textPrimary;

            // Arrow: rotate down when open
            ArrowIcon.rectTransform.localRotation = Quaternion.Euler(0, 0, _open ? 180 : 0);
        }

        public ReEndSelect AddOption(string option)
        {
            Options.Add(option);
            var opt = ReEndUI.Button(_dropdownContent.transform, option)
                .SetVariant(ReEndVariant.Ghost)
                .SetSize(ReEndSize.Sm);
            opt.SetOnClick(() =>
            {
                Value = option;
                ApplyTheme();
                _open = false;
                _dropdownContent.SetActive(false);
                OnValueChanged?.Invoke(option);
            });
            opt.RectTransform.anchoredPosition = new Vector2(0, -(Options.Count - 1) * 36);
            return this;
        }

        public ReEndSelect SetOptions(List<string> options)
        {
            // 销毁旧的选项按钮 GameObject
            for (int i = _dropdownContent.transform.childCount - 1; i >= 0; i--)
                Destroy(_dropdownContent.transform.GetChild(i).gameObject);
            Options.Clear();
            foreach (var o in options) AddOption(o);
            return this;
        }
        public ReEndSelect SetValue(string v) { Value = v; if (_built) ApplyTheme(); return this; }
        public ReEndSelect SetPlaceholder(string ph) { Placeholder = ph; if (_built) ApplyTheme(); return this; }
        public ReEndSelect SetOnValueChanged(System.Action<string> cb) { OnValueChanged = cb; return this; }
    }
}
