using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndCopyClipboard : ReEndBaseComponent
    {
        public string Text { get; set; } = "";
        public string SuccessMessage { get; set; } = "Copied!";

        public TMP_Text Label { get; private set; }
        private Button _btn;
        private float _feedbackTimer;

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.AddComponent<Image>();
            var mat = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
            mat.SetFloat("_CornerSize", 0.04f);
            BackgroundImage.material = mat;

            _btn = gameObject.AddComponent<Button>();
            _btn.onClick.AddListener(Copy);

            var textGo = CreateChild(transform, "Label");
            Label = textGo.AddComponent<TextMeshProUGUI>();
            Label.alignment = TextAlignmentOptions.Center;
            var tRT = Label.rectTransform;
            tRT.offsetMin = new Vector2(8, 4);
            tRT.offsetMax = new Vector2(-8, -4);
            Stretch(tRT);

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 120);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 32);
        }

        private void Copy()
        {
            GUIUtility.systemCopyBuffer = Text;
            Label.text = SuccessMessage;
            _feedbackTimer = 1.5f;
        }

        public override void ApplyTheme()
        {
            BackgroundImage.color = Theme.surface2;
            Label.text = Text;
            Label.fontSize = Theme.bodySmSize;
            Label.color = Theme.textPrimary;
        }

        private void Update()
        {
            if (_feedbackTimer > 0)
            {
                _feedbackTimer -= Time.deltaTime;
                if (_feedbackTimer <= 0)
                    Label.text = Text;
            }
        }

        public ReEndCopyClipboard SetText(string t) { Text = t; if (_built) ApplyTheme(); return this; }
    }
}
