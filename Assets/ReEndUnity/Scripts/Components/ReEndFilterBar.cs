using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndFilterBar : ReEndBaseComponent
    {
        public List<string> Filters { get; private set; } = new();
        public List<string> ActiveFilters { get; private set; } = new();
        public System.Action<List<string>> OnFiltersChanged { get; set; }

        private List<ReEndBadge> _badges = new();

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 32);
        }

        public override void ApplyTheme()
        {
            foreach (var b in _badges)
                Destroy(b.gameObject);
            _badges.Clear();

            float x = 0;
            foreach (var f in Filters)
            {
                bool active = ActiveFilters.Contains(f);
                var badge = ReEndUI.Badge(transform, f)
                    .SetVariant(active ? ReEndTagVariant.Accent : ReEndTagVariant.Default)
                    .SetRemovable(active);
                badge.SetOnRemove(() =>
                {
                    ActiveFilters.Remove(f);
                    ApplyTheme();
                    OnFiltersChanged?.Invoke(ActiveFilters);
                });

                var btn = badge.BackgroundImage.gameObject.AddComponent<Button>();
                btn.onClick.AddListener(() =>
                {
                    if (ActiveFilters.Contains(f)) ActiveFilters.Remove(f);
                    else ActiveFilters.Add(f);
                    ApplyTheme();
                    OnFiltersChanged?.Invoke(ActiveFilters);
                });

                badge.RectTransform.anchoredPosition = new Vector2(x, 0);
                x += badge.RectTransform.rect.width + Theme.space2;
                _badges.Add(badge);
            }
        }

        public ReEndFilterBar AddFilter(string name) { Filters.Add(name); return this; }
        public ReEndFilterBar SetFilters(List<string> filters) { Filters = filters; return this; }
        public ReEndFilterBar SetOnFiltersChanged(System.Action<List<string>> cb) { OnFiltersChanged = cb; return this; }
    }
}
