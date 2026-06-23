using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndCoordinateTag : ReEndBaseComponent
    {
        public string Label { get; set; } = "SECT";
        public string X { get; set; } = "000";
        public string Y { get; set; } = "000";

        public TMP_Text LabelText { get; private set; }
        public TMP_Text CoordText { get; private set; }

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.AddComponent<Image>();
            BackgroundImage.color = new Color(0, 0, 0, 0.4f);
            var mat = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
            mat.SetFloat("_CornerSize", 0.04f);
            BackgroundImage.material = mat;
            BackgroundImage.raycastTarget = false;

            // Label (top small)
            LabelText = CreateChild<TextMeshProUGUI>(transform, "Label");
            LabelText.alignment = TextAlignmentOptions.Left;
            LabelText.raycastTarget = false;
            var lRT = LabelText.rectTransform;
            lRT.anchorMin = new Vector2(0, 1);
            lRT.sizeDelta = new Vector2(80, 14);
            lRT.anchoredPosition = new Vector2(8, -4);

            // Coord (bottom)
            CoordText = CreateChild<TextMeshProUGUI>(transform, "Coord");
            CoordText.alignment = TextAlignmentOptions.Left;
            CoordText.raycastTarget = false;
            var cRT = CoordText.rectTransform;
            cRT.anchorMin = new Vector2(0, 0);
            cRT.sizeDelta = new Vector2(120, 18);
            cRT.anchoredPosition = new Vector2(8, 6);

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 140);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 44);
        }

        public override void ApplyTheme()
        {
            LabelText.text = Label.ToUpper();
            LabelText.fontSize = 8;
            LabelText.color = Theme.textMuted;

            CoordText.text = $"X:{X} Y:{Y}";
            CoordText.fontSize = 10;
            CoordText.color = Theme.textPrimary;
        }

        public ReEndCoordinateTag SetLabel(string l) { Label = l; if (_built) ApplyTheme(); return this; }
        public ReEndCoordinateTag SetCoords(string x, string y) { X = x; Y = y; if (_built) ApplyTheme(); return this; }
    }
}
