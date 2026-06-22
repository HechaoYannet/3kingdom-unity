using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndPullToRefresh : ReEndBaseComponent, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public ScrollRect ScrollRect { get; set; }
        public float Threshold { get; set; } = 80;
        public System.Action OnRefresh { get; set; }

        public Image Spinner { get; private set; }
        private bool _refreshing;
        private float _pullDistance;

        protected override void BuildInternal()
        {
            Spinner = gameObject.AddComponent<Image>();
            Spinner.raycastTarget = false;

            RectTransform.anchorMin = new Vector2(0.5f, 1);
            RectTransform.anchorMax = new Vector2(0.5f, 1);
            RectTransform.sizeDelta = new Vector2(28, 28);
            RectTransform.anchoredPosition = new Vector2(0, -40);
        }

        public override void ApplyTheme()
        {
            Spinner.color = _refreshing ? Theme.primary : Theme.textMuted;
        }

        public void OnBeginDrag(PointerEventData e) { _pullDistance = 0; }
        public void OnDrag(PointerEventData e)
        {
            if (_refreshing) return;
            _pullDistance += e.delta.y * 0.5f;
            _pullDistance = Mathf.Max(0, _pullDistance);
            RectTransform.anchoredPosition = new Vector2(0, -40 + _pullDistance);
            float pct = Mathf.Clamp01(_pullDistance / Threshold);
            Spinner.rectTransform.localRotation = Quaternion.Euler(0, 0, -pct * 360);
            Spinner.color = Color.Lerp(Theme.textMuted, Theme.primary, pct);
        }

        public void OnEndDrag(PointerEventData e)
        {
            if (_pullDistance >= Threshold)
            {
                _refreshing = true;
                ApplyTheme();
                OnRefresh?.Invoke();
                // Simulate refresh for 1.5s
                Invoke(nameof(ResetRefresh), 1.5f);
            }
            else
            {
                RectTransform.anchoredPosition = new Vector2(0, -40);
            }
        }

        private void ResetRefresh()
        {
            _refreshing = false;
            RectTransform.anchoredPosition = new Vector2(0, -40);
            ApplyTheme();
        }
    }
}
