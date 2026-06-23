using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndToast : ReEndBaseComponent
    {
        public string Message { get; set; } = "";
        public ReEndStatus Status { get; set; } = ReEndStatus.Default;
        public float Duration { get; set; } = 3f;
        public ReEndToastPosition Position { get; set; } = ReEndToastPosition.TopRight;

        public TMP_Text MessageText { get; private set; }
        private float _timer;

        private static List<ReEndToast> _activeToasts = new();
        private static GameObject _toastCanvas;

        public static ReEndToast Show(string message, ReEndStatus status = ReEndStatus.Default, float duration = 3f)
        {
            if (_toastCanvas == null)
            {
                _toastCanvas = new GameObject("ReEndToastCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                var canvas = _toastCanvas.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 3000;
                DontDestroyOnLoad(_toastCanvas);
            }

            var go = new GameObject($"Toast_{message}", typeof(RectTransform));
            go.transform.SetParent(_toastCanvas.transform, false);
            var toast = go.AddComponent<ReEndToast>();
            toast.Message = message;
            toast.Status = status;
            toast.Duration = duration;
            toast.EnsureBuilt();
            toast.ApplyTheme();

            _activeToasts.Add(toast);
            UpdatePositions();
            return toast;
        }

        private static void UpdatePositions()
        {
            // 移除已销毁的 toast 条目
            for (int i = _activeToasts.Count - 1; i >= 0; i--)
            {
                if (_activeToasts[i] == null)
                    _activeToasts.RemoveAt(i);
            }
            // 重新排列剩余 toast
            for (int i = 0; i < _activeToasts.Count; i++)
            {
                _activeToasts[i].RectTransform.anchoredPosition = new Vector2(0, -(i * 52));
            }
        }

        protected override void BuildInternal()
        {
            BackgroundImage = gameObject.AddComponent<Image>();
            var mat = GetOrCreateMaterial("ReEnd/UI/ClipCorner");
            mat.SetFloat("_CornerSize", 0.04f);
            BackgroundImage.material = mat;

            var textGo = CreateChild(transform, "Message");
            MessageText = textGo.AddComponent<TextMeshProUGUI>();
            MessageText.alignment = TextAlignmentOptions.Left;
            var tRT = MessageText.rectTransform;
            tRT.offsetMin = new Vector2(12, 4);
            tRT.offsetMax = new Vector2(-12, -4);
            Stretch(tRT);

            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 320);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 44);
        }

        public override void ApplyTheme()
        {
            BackgroundImage.color = Theme.card;
            MessageText.text = Message;
            MessageText.fontSize = Theme.bodySmSize;
            MessageText.color = Theme.textPrimary;
            _timer = Duration;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                Destroy(gameObject);
            }
        }

        protected override void OnDestroy()
        {
            _activeToasts.Remove(this);
            UpdatePositions();
            base.OnDestroy();
        }

        public static void DismissAll()
        {
            foreach (var t in _activeToasts)
                if (t != null) Destroy(t.gameObject);
            _activeToasts.Clear();
        }
    }
}
