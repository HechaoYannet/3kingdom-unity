using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndResizable : ReEndBaseComponent, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public float MinWidth { get; set; } = 100;
        public float MinHeight { get; set; } = 100;
        public bool ResizeHorizontal { get; set; } = true;
        public bool ResizeVertical { get; set; } = true;

        private Image _handle;
        private Vector2 _dragStart;
        private Vector2 _sizeStart;

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);

            var bg = gameObject.AddComponent<Image>();
            bg.color = Theme.surface1;

            _handle = CreateChild<Image>(transform, "Handle");
            _handle.color = Theme.borderDefault;
            _handle.raycastTarget = true;
            var hRT = _handle.rectTransform;
            hRT.anchorMin = new Vector2(1, 0);
            hRT.anchorMax = new Vector2(1, 0);
            hRT.pivot = new Vector2(1, 0);
            hRT.sizeDelta = new Vector2(12, 12);

            var evt = _handle.gameObject.AddComponent<EventTrigger>();
            var entry = new EventTrigger.Entry { eventID = EventTriggerType.Drag };
            evt.triggers.Add(entry);
        }

        public override void ApplyTheme() { }

        public void OnBeginDrag(PointerEventData e) { _dragStart = e.position; _sizeStart = RectTransform.sizeDelta; }
        public void OnDrag(PointerEventData e)
        {
            var delta = e.position - _dragStart;
            var newSize = _sizeStart;
            if (ResizeHorizontal) newSize.x = Mathf.Max(MinWidth, _sizeStart.x + delta.x);
            if (ResizeVertical) newSize.y = Mathf.Max(MinHeight, _sizeStart.y - delta.y);
            RectTransform.sizeDelta = newSize;
        }
        public void OnEndDrag(PointerEventData e) { }
    }
}
