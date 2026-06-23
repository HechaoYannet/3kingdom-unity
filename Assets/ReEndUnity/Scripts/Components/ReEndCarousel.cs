using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public class ReEndCarousel : ReEndBaseComponent
    {
        public List<Sprite> Slides { get; private set; } = new();
        public int CurrentIndex { get; set; }
        public bool AutoPlay { get; set; }
        public float AutoPlayInterval { get; set; } = 3f;
        public System.Action<int> OnSlideChanged { get; set; }

        public Image CurrentImage { get; private set; }
        private Button _prev, _next;
        private GameObject _dotsContainer;
        private float _timer;

        protected override void BuildInternal()
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 240);

            CurrentImage = gameObject.AddComponent<Image>();
            CurrentImage.raycastTarget = false;

            // Prev/Next buttons
            _prev = CreateChild<Button>(transform, "Prev");
            _prev.onClick.AddListener(() => GoTo(CurrentIndex - 1));
            var pImg = _prev.gameObject.AddComponent<Image>();
            pImg.color = new Color(0, 0, 0, 0.3f);
            _prev.GetComponent<RectTransform>().sizeDelta = new Vector2(32, 240);

            _next = CreateChild<Button>(transform, "Next");
            _next.onClick.AddListener(() => GoTo(CurrentIndex + 1));
            var nImg = _next.gameObject.AddComponent<Image>();
            nImg.color = new Color(0, 0, 0, 0.3f);
            _next.GetComponent<RectTransform>().sizeDelta = new Vector2(32, 240);

            // Dots
            _dotsContainer = CreateChild(transform, "Dots");
            var dRT = _dotsContainer.GetComponent<RectTransform>();
            dRT.anchorMin = new Vector2(0.5f, 0);
            dRT.anchorMax = new Vector2(0.5f, 0);
            dRT.sizeDelta = new Vector2(200, 16);
            dRT.anchoredPosition = new Vector2(0, 12);
        }

        private void GoTo(int idx)
        {
            if (Slides.Count == 0) return;
            CurrentIndex = (idx + Slides.Count) % Slides.Count;
            ApplyTheme();
            OnSlideChanged?.Invoke(CurrentIndex);
            _timer = 0;
        }

        public override void ApplyTheme()
        {
            if (Slides.Count > 0 && CurrentIndex < Slides.Count)
                CurrentImage.sprite = Slides[CurrentIndex];

            foreach (Transform t in _dotsContainer.transform) Destroy(t.gameObject);
            for (int i = 0; i < Slides.Count; i++)
            {
                var dot = CreateChild(_dotsContainer.transform, $"Dot_{i}");
                var dotImg = dot.AddComponent<Image>();
                dotImg.color = i == CurrentIndex ? Theme.primary : Theme.surface3;
                dotImg.rectTransform.sizeDelta = new Vector2(8, 8);
                dotImg.rectTransform.anchoredPosition = new Vector2(i * 16 - Slides.Count * 8, 0);
            }
        }

        private void Update()
        {
            if (AutoPlay && Slides.Count > 1)
            {
                _timer += Time.deltaTime;
                if (_timer >= AutoPlayInterval)
                    GoTo(CurrentIndex + 1);
            }
        }

        public ReEndCarousel AddSlide(Sprite s) { Slides.Add(s); return this; }
        public ReEndCarousel SetAutoPlay(bool ap = true) { AutoPlay = ap; return this; }
        public ReEndCarousel SetOnSlideChanged(System.Action<int> cb) { OnSlideChanged = cb; return this; }
    }
}
