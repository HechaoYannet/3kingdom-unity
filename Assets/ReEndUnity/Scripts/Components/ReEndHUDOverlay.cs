using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndHUDOverlay : ReEndBaseComponent
    {
        public string SystemLabel { get; set; } = "ENDFIELD::OPS";

        public TMP_Text SystemLabelText { get; private set; }
        public TMP_Text CoordLabel { get; private set; }
        public Image Crosshair { get; private set; }
        private Image _brackets;
        private Image _scanline;

        protected override void BuildInternal()
        {
            // Full-screen overlay
            RectTransform.anchorMin = Vector2.zero;
            RectTransform.anchorMax = Vector2.one;
            RectTransform.offsetMin = Vector2.zero;
            RectTransform.offsetMax = Vector2.zero;

            // Corner brackets
            var bracketGo = CreateChild(transform, "Brackets");
            _brackets = bracketGo.AddComponent<Image>();
            _brackets.material = GetOrCreateMaterial("ReEnd/UI/CornerBracket");
            _brackets.material.SetFloat("_BracketSize", 0.2f);
            _brackets.material.SetFloat("_BracketWidth", 0.01f);
            _brackets.raycastTarget = false;
            Stretch(_brackets.rectTransform);

            // Scanline overlay
            var scanGo = CreateChild(transform, "Scanline");
            _scanline = scanGo.AddComponent<Image>();
            _scanline.material = GetOrCreateMaterial("ReEnd/UI/Scanline");
            _scanline.raycastTarget = false;
            Stretch(_scanline.rectTransform);

            // System label (top-left)
            SystemLabelText = CreateChild<TextMeshProUGUI>(transform, "SystemLabel");
            SystemLabelText.fontSize = 10;
            SystemLabelText.color = Theme.textMuted;
            SystemLabelText.raycastTarget = false;
            var sRT = SystemLabelText.rectTransform;
            sRT.anchorMin = new Vector2(0, 1);
            sRT.pivot = new Vector2(0, 1);
            sRT.anchoredPosition = new Vector2(16, -16);

            // Crosshair (center)
            var crossGo = CreateChild(transform, "Crosshair");
            Crosshair = crossGo.AddComponent<Image>();
            Crosshair.raycastTarget = false;
            Crosshair.color = Theme.primary;
            var cRT = Crosshair.rectTransform;
            cRT.anchorMin = new Vector2(0.5f, 0.5f);
            cRT.anchorMax = new Vector2(0.5f, 0.5f);
            cRT.sizeDelta = new Vector2(24, 1);

            var crossV = CreateChild(transform, "CrosshairV");
            var crossVImg = crossV.AddComponent<Image>();
            crossVImg.color = Theme.primary;
            crossVImg.raycastTarget = false;
            var cvRT = crossVImg.rectTransform;
            cvRT.anchorMin = new Vector2(0.5f, 0.5f);
            cvRT.sizeDelta = new Vector2(1, 24);

            // Bottom coord
            CoordLabel = CreateChild<TextMeshProUGUI>(transform, "CoordLabel");
            CoordLabel.raycastTarget = false;
            CoordLabel.fontSize = 9;
            CoordLabel.color = Theme.textMuted;
            var coRT = CoordLabel.rectTransform;
            coRT.anchorMin = new Vector2(0, 0);
            coRT.pivot = new Vector2(0, 0);
            coRT.anchoredPosition = new Vector2(16, 16);

            // Prevent raycast blocking
            gameObject.AddComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public override void ApplyTheme()
        {
            SystemLabelText.text = SystemLabel;
            SystemLabelText.color = Theme.textMuted;
            Crosshair.color = new Color(1f, 0.83f, 0.16f, 0.15f);
            CoordLabel.text = $"SYS::NOMINAL  //  {Time.time:F1}s";
            CoordLabel.color = Theme.textMuted;
        }

        private void Update()
        {
            CoordLabel.text = $"SYS::NOMINAL  //  {Time.time:F1}s";
        }

        public ReEndHUDOverlay SetSystemLabel(string l) { SystemLabel = l; if (_built) ApplyTheme(); return this; }
    }
}
