using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndBadge : ReEndBaseComponent
    {
        public string Text { get; set; } = "Badge";
        public ReEndTagVariant Variant { get; set; } = ReEndTagVariant.Default;
        public bool Removable { get; set; }
        public Action OnRemove { get; set; }

        public TMP_Text Label { get; private set; }
        public Image RemoveButton { get; private set; }
        private Button _removeBtn;

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.GetComponent<Image>() ?? gameObject.AddComponent<Image>();
            BackgroundImage.raycastTarget = false;
            var mat = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
            mat.SetFloat("_CornerSize", Theme.clipCornerSm);
            BackgroundImage.material = mat;

            var labGo = CreateChild(transform, "Label");
            Label = labGo.AddComponent<TextMeshProUGUI>();
            Label.alignment = TextAlignmentOptions.Center;
            Label.raycastTarget = false;
            Stretch(Label.rectTransform);

            // Remove button
            var rmGo = CreateChild(transform, "Remove");
            RemoveButton = rmGo.AddComponent<Image>();
            RemoveButton.raycastTarget = true;
            _removeBtn = rmGo.AddComponent<Button>();
            _removeBtn.onClick.AddListener(() => OnRemove?.Invoke());
            var rmRT = RemoveButton.rectTransform;
            rmRT.anchorMin = new Vector2(1, 0.5f);
            rmRT.anchorMax = new Vector2(1, 0.5f);
            rmRT.pivot = new Vector2(1, 0.5f);
            rmRT.sizeDelta = new Vector2(16, 16);
            rmRT.anchoredPosition = new Vector2(-4, 0);
            var rmTxt = CreateChild<TextMeshProUGUI>(rmGo.transform, "X");
            rmTxt.text = "×";
            rmTxt.fontSize = 12;
            rmTxt.alignment = TextAlignmentOptions.Center;
            rmTxt.color = Theme.textMuted;
            rmTxt.raycastTarget = false;
            Stretch(rmTxt.rectTransform);
            RemoveButton.gameObject.SetActive(false);
        }

        public override void ApplyTheme()
        {
            var h = Theme.space6;
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
            var padH = Theme.space2;
            var padV = Theme.space1;

            // Colors
            (Color bg, Color fg) = Variant switch
            {
                ReEndTagVariant.Success => (Theme.greenSoft, Theme.efGreen),
                ReEndTagVariant.Warning => (Theme.orangeSoft, Theme.efOrange),
                ReEndTagVariant.Danger => (Theme.redSoft, Theme.efRed),
                ReEndTagVariant.Info => (new Color(0.3f, 0.67f, 0.85f, 0.1f), Theme.efBlue),
                ReEndTagVariant.Accent => (new Color(1f, 0.83f, 0.16f, 0.1f), Theme.efYellow),
                ReEndTagVariant.Lime => (new Color(0.8f, 1f, 0.25f, 0.1f), Theme.efLime),
                _ => (Theme.surface2, Theme.textPrimary)
            };

            BackgroundImage.color = bg;

            Label.text = Text;
            Label.fontSize = Theme.captionSize;
            Label.color = fg;
            Label.rectTransform.offsetMin = new Vector2(padH, padV);
            Label.rectTransform.offsetMax = new Vector2(-padH, -padV);

            if (Removable)
            {
                RemoveButton.gameObject.SetActive(true);
                Label.rectTransform.offsetMax = new Vector2(-20, -padV);
            }
            else
            {
                RemoveButton.gameObject.SetActive(false);
            }
        }

        public ReEndBadge SetText(string text) { Text = text; if (_built) ApplyTheme(); return this; }
        public ReEndBadge SetVariant(ReEndTagVariant v) { Variant = v; if (_built) ApplyTheme(); return this; }
        public ReEndBadge SetRemovable(bool r = true) { Removable = r; if (_built) ApplyTheme(); return this; }
        public ReEndBadge SetOnRemove(Action cb) { OnRemove = cb; return this; }
    }
}
