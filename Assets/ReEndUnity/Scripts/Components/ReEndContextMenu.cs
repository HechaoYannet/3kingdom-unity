using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndContextMenu : ReEndBaseComponent
    {
        public List<ContextMenuItem> Items { get; private set; } = new();

        [System.Serializable]
        public class ContextMenuItem
        {
            public string Label;
            public Action OnClick;
            public bool Separator;
        }

        protected override void BuildInternal()
        {
            var img = gameObject.AddComponent<Image>();
            img.color = Theme.popover;
            img.raycastTarget = true;
            var mat = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
            mat.SetFloat("_CornerSize", 0.04f);
            img.material = mat;

            gameObject.SetActive(false);
        }

        public override void ApplyTheme()
        {
            foreach (Transform t in transform) Destroy(t.gameObject);

            float y = 4;
            foreach (var item in Items)
            {
                if (item.Separator)
                {
                    var sep = CreateChild(transform, "Sep");
                    var sepImg = sep.AddComponent<Image>();
                    sepImg.color = Theme.borderDefault;
                    sepImg.rectTransform.anchoredPosition = new Vector2(0, -y);
                    sepImg.rectTransform.sizeDelta = new Vector2(180, 1);
                    y += 9;
                    continue;
                }

                var go = CreateChild(transform, item.Label);
                var txt = go.AddComponent<TextMeshProUGUI>();
                txt.text = item.Label;
                txt.fontSize = Theme.bodySmSize;
                txt.color = Theme.textPrimary;
                txt.alignment = TextAlignmentOptions.Left;
                txt.rectTransform.offsetMin = new Vector2(12, 0);
                txt.rectTransform.offsetMax = new Vector2(-12, 0);
                txt.rectTransform.sizeDelta = new Vector2(0, 32);
                txt.rectTransform.anchoredPosition = new Vector2(0, -y);

                var btn = go.AddComponent<Button>();
                btn.onClick.AddListener(() => { item.OnClick?.Invoke(); Hide(); });
                y += 32;
            }

            RectTransform.sizeDelta = new Vector2(200, y + 4);
        }

        public ReEndContextMenu AddItem(string label, System.Action onClick)
        {
            Items.Add(new ContextMenuItem { Label = label, OnClick = onClick });
            return this;
        }

        public ReEndContextMenu AddSeparator() { Items.Add(new ContextMenuItem { Separator = true }); return this; }

        public void Show(Vector2 screenPos)
        {
            gameObject.SetActive(true);
            RectTransform.position = screenPos;
            EnsureBuilt();
            ApplyTheme();
        }

        public void Hide() { gameObject.SetActive(false); }
    }
}
