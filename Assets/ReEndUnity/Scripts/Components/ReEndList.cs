using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndList : ReEndBaseComponent
    {
        public List<string> Items { get; private set; } = new();
        public bool Ordered { get; set; }
        public bool ShowDividers { get; set; }
        public Sprite ItemIcon { get; set; }
        public System.Action<int> OnItemClick { get; set; }

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);
        }

        public override void ApplyTheme()
        {
            foreach (Transform t in transform)
                if (t.name.StartsWith("Item_")) Destroy(t.gameObject);

            float y = 0;
            for (int i = 0; i < Items.Count; i++)
            {
                int idx = i;
                var go = CreateChild(transform, $"Item_{i}");
                var rt = go.GetComponent<RectTransform>();
                rt.anchorMin = new Vector2(0, 1);
                rt.anchorMax = new Vector2(1, 1);
                rt.pivot = new Vector2(0, 1);
                rt.sizeDelta = new Vector2(0, 32);
                rt.anchoredPosition = new Vector2(0, -y);

                string prefix = Ordered ? $"{i + 1}." : ItemIcon == null ? "◆" : "";
                var txt = go.AddComponent<TextMeshProUGUI>();
                txt.text = $"{prefix} {Items[i]}";
                txt.alignment = TextAlignmentOptions.Left;
                txt.fontSize = Theme.bodySize;
                txt.color = Theme.textPrimary;

                var btn = go.AddComponent<Button>();
                btn.onClick.AddListener(() => OnItemClick?.Invoke(idx));

                y += 32;

                if (ShowDividers && i < Items.Count - 1)
                    y += 4; // gap for divider
            }

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y);
        }

        public ReEndList AddItem(string item) { Items.Add(item); return this; }
        public ReEndList SetItems(List<string> items) { Items = items; return this; }
        public ReEndList SetOrdered(bool o = true) { Ordered = o; return this; }
        public ReEndList SetShowDividers(bool s = true) { ShowDividers = s; return this; }
        public ReEndList SetOnItemClick(System.Action<int> cb) { OnItemClick = cb; return this; }
    }
}
