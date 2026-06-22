using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndDropdown : ReEndBaseComponent
    {
        public string Label { get; set; } = "Dropdown";
        public string Value { get; set; }
        public List<string> Options { get; private set; } = new();
        public System.Action<string> OnValueChanged { get; set; }

        public TMP_Text LabelText { get; private set; }
        public TMP_Text ValueText { get; private set; }
        private Button _btn;
        private GameObject _menu;
        private bool _open;

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.GetComponent<Image>() ?? gameObject.AddComponent<Image>();
            _btn = gameObject.AddComponent<Button>();
            _btn.onClick.AddListener(Toggle);

            LabelText = CreateChild<TextMeshProUGUI>(transform, "Label");
            LabelText.alignment = TextAlignmentOptions.Left;
            var lRT = LabelText.rectTransform;
            lRT.offsetMin = new Vector2(12, 0);
            lRT.offsetMax = new Vector2(-32, 0);
            Stretch(lRT);

            ValueText = CreateChild<TextMeshProUGUI>(transform, "Value");
            ValueText.alignment = TextAlignmentOptions.Right;
            ValueText.raycastTarget = false;
            var vRT = ValueText.rectTransform;
            vRT.anchorMin = new Vector2(1, 0.5f);
            vRT.anchorMax = new Vector2(1, 0.5f);
            vRT.pivot = new Vector2(1, 0.5f);
            vRT.sizeDelta = new Vector2(120, 20);
            vRT.anchoredPosition = new Vector2(-32, 0);

            _menu = CreateChild(transform, "Menu");
            _menu.SetActive(false);
        }

        private void Toggle() { _open = !_open; _menu.SetActive(_open); }

        public override void ApplyTheme()
        {
            float h = Theme.GetSizeValue(ReEndSize.Md);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
            BackgroundImage.color = Theme.surface2;

            LabelText.text = Label;
            LabelText.fontSize = Theme.bodySmSize;
            LabelText.color = Theme.textMuted;
            ValueText.text = Value ?? Options[0];
            ValueText.fontSize = Theme.bodySize;
            ValueText.color = Theme.textPrimary;

            foreach (Transform t in _menu.transform) Destroy(t.gameObject);
            float y = 0;
            foreach (var o in Options)
            {
                var item = ReEndUI.Button(_menu.transform, o).SetVariant(ReEndVariant.Ghost).SetSize(ReEndSize.Xs);
                item.SetOnClick(() => { Value = o; ApplyTheme(); _open = false; _menu.SetActive(false); OnValueChanged?.Invoke(o); });
                item.RectTransform.anchoredPosition = new Vector2(0, -y);
                y += 36;
            }
            var menuRT = _menu.GetComponent<RectTransform>();
            menuRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y);
            menuRT.anchoredPosition = new Vector2(0, -h);
        }

        public ReEndDropdown SetLabel(string l) { Label = l; if (_built) ApplyTheme(); return this; }
        public ReEndDropdown AddOption(string o) { Options.Add(o); return this; }
        public ReEndDropdown SetOptions(List<string> opts) { Options = opts; return this; }
        public ReEndDropdown SetOnValueChanged(System.Action<string> cb) { OnValueChanged = cb; return this; }
    }
}
