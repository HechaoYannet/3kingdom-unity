using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndCheckbox : ReEndBaseComponent
    {
        public bool Checked { get; set; }
        public string LabelText { get; set; } = "";
        public System.Action<bool> OnValueChanged { get; set; }

        public Image CheckImage { get; private set; }
        public TMP_Text Label { get; private set; }
        private Button _btn;

        protected override void BuildInternal()
        {
            // Checkbox square
            var checkGo = CreateChild(transform, "Check");
            CheckImage = checkGo.AddComponent<Image>();
            CheckImage.raycastTarget = true;
            _btn = checkGo.AddComponent<Button>();
            _btn.onClick.AddListener(() =>
            {
                Checked = !Checked;
                ApplyTheme();
                OnValueChanged?.Invoke(Checked);
            });

            var checkRT = CheckImage.rectTransform;
            checkRT.anchorMin = new Vector2(0, 0.5f);
            checkRT.anchorMax = new Vector2(0, 0.5f);
            checkRT.pivot = new Vector2(0, 0.5f);
            checkRT.sizeDelta = new Vector2(20, 20);
            checkRT.anchoredPosition = Vector2.zero;

            // Label
            var labGo = CreateChild(transform, "Label");
            Label = labGo.AddComponent<TextMeshProUGUI>();
            Label.alignment = TextAlignmentOptions.Left;
            Label.raycastTarget = false;
            var labRT = Label.rectTransform;
            labRT.anchorMin = new Vector2(0, 0);
            labRT.anchorMax = new Vector2(1, 1);
            labRT.offsetMin = new Vector2(28, 0);
            labRT.offsetMax = Vector2.zero;

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 24);
        }

        public override void ApplyTheme()
        {
            CheckImage.color = Checked ? Theme.primary : Theme.surface3;
            var mat = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
            mat.SetFloat("_CornerSize", 0.04f);
            CheckImage.material = mat;

            Label.text = Checked ? $"◆ {LabelText}" : $"◇ {LabelText}";
            Label.fontSize = Theme.bodySmSize;
            Label.color = Theme.textPrimary;
        }

        public ReEndCheckbox SetChecked(bool c) { Checked = c; if (_built) ApplyTheme(); return this; }
        public ReEndCheckbox SetLabel(string l) { LabelText = l; if (_built) ApplyTheme(); return this; }
        public ReEndCheckbox SetOnValueChanged(System.Action<bool> cb) { OnValueChanged = cb; return this; }
    }
}
