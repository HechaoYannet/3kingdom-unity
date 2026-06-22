using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndPagination : ReEndBaseComponent
    {
        public int Total { get; set; } = 1;
        public int Current { get; set; } = 1;
        public System.Action<int> OnPageChanged { get; set; }

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 36);
        }

        public override void ApplyTheme()
        {
            foreach (Transform t in transform) Destroy(t.gameObject);

            float x = 0;
            // Prev
            var prev = MakePageBtn("PREV", Current > 1, () => GoTo(Current - 1));
            prev.RectTransform.anchoredPosition = new Vector2(x, 0);
            x += prev.RectTransform.rect.width + 4;

            // Pages (smart ellipsis)
            for (int i = 1; i <= Total; i++)
            {
                if (Total > 7 && i > 2 && i < Total - 1 && (i < Current - 1 || i > Current + 1))
                {
                    if (i == 3 || i == Total - 2)
                    {
                        var dots = MakePageBtn("…", false, null);
                        dots.RectTransform.anchoredPosition = new Vector2(x, 0);
                        x += dots.RectTransform.rect.width + 4;
                    }
                    continue;
                }

                var btn = MakePageBtn(i.ToString(), true, () => GoTo(i));
                if (i == Current) btn.SetVariant(ReEndVariant.Primary);
                btn.RectTransform.anchoredPosition = new Vector2(x, 0);
                x += btn.RectTransform.rect.width + 4;
            }

            // Next
            var next = MakePageBtn("NEXT", Current < Total, () => GoTo(Current + 1));
            next.RectTransform.anchoredPosition = new Vector2(x, 0);

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x + next.RectTransform.rect.width);
        }

        private ReEndButton MakePageBtn(string label, bool enabled, System.Action onClick)
        {
            var btn = ReEndUI.Button(transform, label).SetSize(ReEndSize.Xs).SetVariant(ReEndVariant.Outline);
            if (!enabled) btn.SetDisabled();
            if (onClick != null) btn.SetOnClick(onClick);
            return btn;
        }

        private void GoTo(int page)
        {
            if (page < 1 || page > Total) return;
            Current = page;
            ApplyTheme();
            OnPageChanged?.Invoke(Current);
        }

        public ReEndPagination SetTotal(int t) { Total = t; return this; }
        public ReEndPagination SetCurrent(int c) { Current = c; if (_built) ApplyTheme(); return this; }
        public ReEndPagination SetOnPageChanged(System.Action<int> cb) { OnPageChanged = cb; return this; }
    }
}
