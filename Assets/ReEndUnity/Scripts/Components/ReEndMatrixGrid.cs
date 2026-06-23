using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndMatrixGrid : ReEndBaseComponent
    {
        public int Columns { get; set; } = 20;
        public int Rows { get; set; } = 10;
        public float UpdateInterval { get; set; } = 0.5f;

        public Image GridImage { get; private set; }

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);

            BackgroundImage = gameObject.AddComponent<Image>();
            BackgroundImage.color = Theme.efBlack;

            var dotGo = CreateChild(transform, "Dots");
            GridImage = dotGo.AddComponent<Image>();
            GridImage.raycastTarget = false;
            var mat = GetOrCreateMaterial("ReEnd/UI/MatrixDot");
            mat.SetFloat("_DotColumns", Columns);
            mat.SetFloat("_DotRows", Rows);
            mat.SetFloat("_DotSize", 0.06f);
            mat.SetFloat("_DutyCycle", 0.05f);
            mat.SetFloat("_Static", 0);
            GridImage.material = mat;
            Stretch(GridImage.rectTransform);
        }

        public override void ApplyTheme()
        {
            BackgroundImage.color = Theme.efBlack;
            GridImage.material.SetColor("_DotColor", Theme.primary);
            GridImage.material.SetColor("_DotInactiveColor", new Color(0.6f, 0.6f, 0.6f, 0.1f));
        }

        public ReEndMatrixGrid SetDimensions(int cols, int rows) { Columns = cols; Rows = rows; if (_built) { GridImage.material.SetFloat("_DotColumns", Columns); GridImage.material.SetFloat("_DotRows", Rows); } return this; }
    }
}
