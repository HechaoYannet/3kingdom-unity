using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndMatrixGrid : ReEndBaseComponent
    {
        public int Columns { get; set; } = 8;
        public int Rows { get; set; } = 8;
        public float UpdateInterval { get; set; } = 0.1f;

        private Image[] _cells;
        private float _timer;

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);

            BackgroundImage = gameObject.AddComponent<Image>();
            BackgroundImage.color = Theme.efBlack;

            _cells = new Image[Columns * Rows];
            float cellW = 200f / Columns;
            float cellH = 200f / Rows;

            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    var cellGo = CreateChild(transform, $"C_{x}_{y}");
                    var cell = cellGo.AddComponent<Image>();
                    cell.raycastTarget = false;
                    cell.color = new Color(1f, 0.83f, 0.16f, 0.05f);
                    cell.rectTransform.anchorMin = new Vector2(0, 1);
                    cell.rectTransform.pivot = new Vector2(0, 1);
                    cell.rectTransform.sizeDelta = new Vector2(cellW - 1, cellH - 1);
                    cell.rectTransform.anchoredPosition = new Vector2(x * cellW, -(y * cellH));

                    _cells[y * Columns + x] = cell;
                }
            }
        }

        public override void ApplyTheme()
        {
            // Randomize once on theme change
            foreach (var c in _cells)
                c.color = new Color(1f, 0.83f, 0.16f, Random.Range(0.02f, 0.15f));
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > UpdateInterval)
            {
                _timer = 0;
                // Randomly update a few cells
                for (int i = 0; i < 5; i++)
                {
                    int idx = Random.Range(0, _cells.Length);
                    _cells[idx].color = new Color(1f, 0.83f, 0.16f, Random.Range(0.02f, 0.3f));
                }
            }
        }

        public ReEndMatrixGrid SetDimensions(int cols, int rows) { Columns = cols; Rows = rows; return this; }
    }
}
