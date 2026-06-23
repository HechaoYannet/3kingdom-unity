using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndHoloCard : ReEndBaseComponent, IPointerEnterHandler, IPointerExitHandler
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public string Subtitle { get; set; }

        public TMP_Text TitleText { get; private set; }
        public TMP_Text ValueText { get; private set; }
        public TMP_Text SubtitleText { get; private set; }
        public Image BackgroundImg { get; private set; }
        private Image _overlay;
        private Material _sweepMat;
        private Image _glowImg;

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 240);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 160);

            BackgroundImg = gameObject.AddComponent<Image>();
            BackgroundImg.material = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
            BackgroundImg.material.SetFloat("_CornerSize", 0.12f);

            _overlay = CreateChild<Image>(transform, "Overlay");
            _overlay.color = new Color(0.8f, 1f, 0.25f, 0.05f);
            _overlay.raycastTarget = false;
            Stretch(_overlay.rectTransform);

            // Glow behind value
            var glowGo = CreateChild(transform, "Glow");
            _glowImg = glowGo.AddComponent<Image>();
            _glowImg.material = GetOrCreateMaterial("ReEnd/UI/Glow");
            _glowImg.color = new Color(1f, 0.83f, 0.16f, 0.1f);
            _glowImg.raycastTarget = false;
            var gRT = _glowImg.rectTransform;
            gRT.anchorMin = new Vector2(0.5f, 0.5f);
            gRT.anchorMax = new Vector2(0.5f, 0.5f);
            gRT.sizeDelta = new Vector2(200, 80);

            // HoloSweep: hover-triggered sweep line + diagonal shimmer
            var sweepGo = CreateChild(transform, "HoloSweep");
            var sweepImg = sweepGo.AddComponent<Image>();
            _sweepMat = GetOrCreateMaterial("ReEnd/UI/HoloSweep");
            _sweepMat.SetFloat("_SweepOpacity", 0);
            _sweepMat.SetFloat("_ShimmerOpacity", 0);
            _sweepMat.SetFloat("_ShimmerAngle", 135);
            _sweepMat.SetFloat("_SweepPosition", 0.5f);
            _sweepMat.SetFloat("_SweepWidth", 0.08f);
            _sweepMat.SetColor("_SweepColor", new Color(1f, 0.83f, 0.16f, 0.6f));
            sweepImg.material = _sweepMat;
            sweepImg.raycastTarget = false;
            Stretch(sweepImg.rectTransform);

            TitleText = CreateChild<TextMeshProUGUI>(transform, "Title");
            TitleText.alignment = TextAlignmentOptions.Left;
            TitleText.raycastTarget = false;
            var tRT = TitleText.rectTransform;
            tRT.anchorMin = new Vector2(0, 1);
            tRT.sizeDelta = new Vector2(-24, 20);
            tRT.anchoredPosition = new Vector2(12, -12);

            ValueText = CreateChild<TextMeshProUGUI>(transform, "Value");
            ValueText.alignment = TextAlignmentOptions.Center;
            ValueText.raycastTarget = false;
            var vRT = ValueText.rectTransform;
            vRT.anchorMin = new Vector2(0.5f, 0.5f);
            vRT.sizeDelta = new Vector2(200, 48);

            SubtitleText = CreateChild<TextMeshProUGUI>(transform, "Subtitle");
            SubtitleText.alignment = TextAlignmentOptions.Right;
            SubtitleText.raycastTarget = false;
            var sRT = SubtitleText.rectTransform;
            sRT.anchorMin = new Vector2(1, 0);
            sRT.sizeDelta = new Vector2(100, 16);
            sRT.anchoredPosition = new Vector2(-12, 12);
        }

        public override void ApplyTheme()
        {
            BackgroundImg.color = Theme.surface1;
            TitleText.text = Title?.ToUpper();
            TitleText.fontSize = Theme.overlineSize;
            TitleText.color = Theme.textMuted;
            ValueText.text = Value;
            ValueText.fontSize = Theme.displayLgSize;
            ValueText.color = Theme.primary;
            SubtitleText.text = Subtitle;
            SubtitleText.fontSize = Theme.captionSize;
            SubtitleText.color = Theme.textSecondary;
        }

        public void OnPointerEnter(PointerEventData e)
        {
            if (_sweepMat != null)
            {
                _sweepMat.SetFloat("_SweepOpacity", 1);
                _sweepMat.SetFloat("_ShimmerOpacity", 1);
            }
            if (_glowImg != null)
                _glowImg.color = new Color(1f, 0.83f, 0.16f, 0.25f);
        }

        public void OnPointerExit(PointerEventData e)
        {
            if (_sweepMat != null)
            {
                _sweepMat.SetFloat("_SweepOpacity", 0);
                _sweepMat.SetFloat("_ShimmerOpacity", 0);
            }
            if (_glowImg != null)
                _glowImg.color = new Color(1f, 0.83f, 0.16f, 0.1f);
        }

        public ReEndHoloCard SetTitle(string t) { Title = t; if (_built) ApplyTheme(); return this; }
        public ReEndHoloCard SetValue(string v) { Value = v; if (_built) ApplyTheme(); return this; }
        public ReEndHoloCard SetSubtitle(string s) { Subtitle = s; if (_built) ApplyTheme(); return this; }
    }
}
