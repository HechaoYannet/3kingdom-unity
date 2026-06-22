using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndButton : ReEndBaseComponent, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        public string Text { get; set; } = "Button";
        public ReEndVariant Variant { get; set; } = ReEndVariant.Primary;
        public ReEndSize Size { get; set; } = ReEndSize.Md;
        public Sprite Icon { get; set; }
        public bool Disabled { get; set; }
        public bool Loading { get; set; }
        public Action OnClick { get; set; }

        public TMP_Text Label { get; private set; }
        public Image IconImage { get; private set; }
        private Button _button;

        protected override void BuildInternal()
        {
            // Background
            BackgroundImage = gameObject.GetComponent<Image>() ?? gameObject.AddComponent<Image>();
            BackgroundImage.raycastTarget = true;

            _button = gameObject.GetComponent<Button>() ?? gameObject.AddComponent<Button>();
            _button.onClick.AddListener(() => OnClick?.Invoke());

            // Icon (left)
            var iconGo = CreateChild(transform, "Icon");
            IconImage = iconGo.AddComponent<Image>();
            IconImage.raycastTarget = false;
            var iconRT = IconImage.rectTransform;
            iconRT.anchorMin = new Vector2(0, 0.5f);
            iconRT.anchorMax = new Vector2(0, 0.5f);
            iconRT.pivot = new Vector2(0, 0.5f);
            iconRT.sizeDelta = new Vector2(0, 20);
            iconRT.anchoredPosition = Vector2.zero;
            IconImage.gameObject.SetActive(false);

            // Label
            var labGo = CreateChild(transform, "Label");
            Label = labGo.AddComponent<TextMeshProUGUI>();
            Label.alignment = TextAlignmentOptions.Center;
            Label.raycastTarget = false;
            Stretch(Label.rectTransform);
        }

        public override void ApplyTheme()
        {
            float h = Theme.GetSizeValue(Size);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
            var pad = Theme.GetSpace((int)Size + 3);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Max(h * 2, 120));

            // Clip corner
            var mat = new Material(Shader.Find("ReEnd/UI/ClipCorner"));
            mat.SetFloat("_CornerSize", Theme.clipCornerMd);
            BackgroundImage.material = mat;

            // Colors by variant
            (Color bg, Color fg, Color border) = Variant switch
            {
                ReEndVariant.Primary => (Theme.primary, Theme.primaryForeground, Color.clear),
                ReEndVariant.Secondary => (Theme.secondary, Theme.secondaryForeground, Color.clear),
                ReEndVariant.Destructive => (Theme.destructive, Theme.destructiveForeground, Color.clear),
                ReEndVariant.Outline => (Color.clear, Theme.textPrimary, Theme.borderDefault),
                ReEndVariant.Ghost => (Color.clear, Theme.textPrimary, Color.clear),
                ReEndVariant.Link => (Color.clear, Theme.textLink, Color.clear),
                _ => (Theme.primary, Theme.primaryForeground, Color.clear)
            };

            BackgroundImage.color = bg;
            Label.text = Text;
            Label.fontSize = Theme.GetFontSize(Size);
            Label.color = fg;

            if (Icon != null)
            {
                IconImage.sprite = Icon;
                IconImage.gameObject.SetActive(true);
                var padVal = Theme.GetSpace(3);
                IconImage.rectTransform.anchoredPosition = new Vector2(padVal, 0);
                IconImage.rectTransform.sizeDelta = new Vector2(Theme.iconSm, Theme.iconSm);
                Label.rectTransform.offsetMin = new Vector2(Theme.iconSm + padVal * 2, 0);
            }
            else
            {
                IconImage.gameObject.SetActive(false);
                Label.rectTransform.offsetMin = Vector2.zero;
            }

            _button.interactable = !Disabled && !Loading;
        }

        public ReEndButton SetText(string text) { Text = text; if (_built) ApplyTheme(); return this; }
        public ReEndButton SetVariant(ReEndVariant v) { Variant = v; if (_built) ApplyTheme(); return this; }
        public ReEndButton SetSize(ReEndSize s) { Size = s; if (_built) ApplyTheme(); return this; }
        public ReEndButton SetIcon(Sprite sprite) { Icon = sprite; if (_built) ApplyTheme(); return this; }
        public ReEndButton SetDisabled(bool d = true) { Disabled = d; if (_built) ApplyTheme(); return this; }
        public ReEndButton SetOnClick(Action cb) { OnClick = cb; return this; }

        public void OnPointerEnter(PointerEventData e) { if (!Disabled) BackgroundImage.color = Theme.surfaceHover; }
        public void OnPointerExit(PointerEventData e) { ApplyTheme(); }
        public void OnPointerDown(PointerEventData e) { transform.localScale = new Vector3(0.96f, 0.96f, 1); }
        public void OnPointerUp(PointerEventData e) { transform.localScale = Vector3.one; }
    }
}
