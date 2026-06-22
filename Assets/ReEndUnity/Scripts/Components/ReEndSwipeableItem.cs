using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndSwipeableItem : ReEndBaseComponent, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public string LeftActionLabel { get; set; } = "Edit";
        public string RightActionLabel { get; set; } = "Delete";
        public System.Action OnLeftAction { get; set; }
        public System.Action OnRightAction { get; set; }

        public TMP_Text ContentText { get; private set; }
        private RectTransform _contentRT;
        private float _swipeThreshold = 60;
        private float _startX;

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 56);

            var bg = gameObject.AddComponent<Image>();
            bg.color = Theme.surface1;

            // Left action (behind content)
            var leftGo = CreateChild(transform, "LeftAction");
            var leftTxt = leftGo.AddComponent<TextMeshProUGUI>();
            leftTxt.text = LeftActionLabel;
            leftTxt.fontSize = 14;
            leftTxt.color = Theme.efGreen;
            leftTxt.rectTransform.anchorMin = new Vector2(0, 0.5f);
            leftTxt.rectTransform.sizeDelta = new Vector2(80, 56);
            leftTxt.rectTransform.anchoredPosition = new Vector2(8, 0);
            leftTxt.alignment = TextAlignmentOptions.Left;

            // Right action (behind content)
            var rightGo = CreateChild(transform, "RightAction");
            var rightTxt = rightGo.AddComponent<TextMeshProUGUI>();
            rightTxt.text = RightActionLabel;
            rightTxt.fontSize = 14;
            rightTxt.color = Theme.efRed;
            rightTxt.rectTransform.anchorMin = new Vector2(1, 0.5f);
            rightTxt.rectTransform.sizeDelta = new Vector2(80, 56);
            rightTxt.rectTransform.anchoredPosition = new Vector2(-8, 0);
            rightTxt.alignment = TextAlignmentOptions.Right;

            // Content
            var contentGo = CreateChild(transform, "Content");
            _contentRT = contentGo.GetComponent<RectTransform>();
            var contentBg = contentGo.AddComponent<Image>();
            contentBg.color = Theme.surface2;

            ContentText = CreateChild<TextMeshProUGUI>(contentGo.transform, "Text");
            ContentText.fontSize = 16;
            ContentText.color = Theme.textPrimary;
            ContentText.alignment = TextAlignmentOptions.Left;
            ContentText.rectTransform.offsetMin = new Vector2(12, 0);
            ContentText.rectTransform.offsetMax = new Vector2(-12, 0);
            Stretch(ContentText.rectTransform);
            Stretch(_contentRT);
        }

        public override void ApplyTheme()
        {
            ContentText.fontSize = Theme.bodySize;
            ContentText.color = Theme.textPrimary;
        }

        public void OnBeginDrag(PointerEventData e) { _startX = _contentRT.anchoredPosition.x; }
        public void OnDrag(PointerEventData e)
        {
            float x = Mathf.Clamp(_startX + e.delta.x, -160, 160);
            _contentRT.anchoredPosition = new Vector2(x, 0);
        }
        public void OnEndDrag(PointerEventData e)
        {
            float x = _contentRT.anchoredPosition.x;
            if (x > _swipeThreshold) { OnLeftAction?.Invoke(); }
            else if (x < -_swipeThreshold) { OnRightAction?.Invoke(); }
            _contentRT.anchoredPosition = Vector2.zero;
        }

        public ReEndSwipeableItem SetText(string t) { ContentText.text = t; return this; }
        public ReEndSwipeableItem SetActions(string left, System.Action onLeft, string right, System.Action onRight)
        {
            LeftActionLabel = left; OnLeftAction = onLeft;
            RightActionLabel = right; OnRightAction = onRight;
            return this;
        }
    }
}
