using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public static class ReEndAnimationHelper
    {
        public static bool DOTweenAvailable { get; private set; }

        static ReEndAnimationHelper()
        {
            DOTweenAvailable = Type.GetType("DG.Tweening.DOTween, DOTween") != null;
        }

        private static float ResolveDuration(float duration)
        {
            if (duration < 0)
            {
                var theme = ReEndThemeManager.Current;
                return theme != null ? theme.durationNormal : 0.3f;
            }
            return duration;
        }

        // ── Fade ──

        public static void FadeIn(CanvasGroup cg, float duration = -1)
        {
            if (cg == null) return;
            duration = ResolveDuration(duration);
            cg.alpha = 0;
            if (DOTweenAvailable)
            {
                DOTween.Kill(cg);
                cg.DOFade(1, duration).SetLink(cg.gameObject);
            }
            else
            {
                cg.alpha = 1;
            }
        }

        public static void FadeOut(CanvasGroup cg, float duration = -1, Action onComplete = null)
        {
            if (cg == null) return;
            duration = ResolveDuration(duration);
            if (DOTweenAvailable)
            {
                DOTween.Kill(cg);
                cg.DOFade(0, duration).SetLink(cg.gameObject).OnComplete(() => onComplete?.Invoke());
            }
            else
            {
                cg.alpha = 0;
                onComplete?.Invoke();
            }
        }

        // ── Slide ──

        public static void SlideInUp(RectTransform rt, float duration = -1)
        {
            if (rt == null) return;
            duration = ResolveDuration(duration);
            var start = rt.anchoredPosition + new Vector2(0, -16);
            if (DOTweenAvailable)
            {
                DOTween.Kill(rt);
                rt.anchoredPosition = start;
                rt.DOAnchorPos(start + new Vector2(0, 16), duration).SetEase(Ease.OutCubic).SetLink(rt.gameObject);
            }
        }

        public static void SlideInRight(RectTransform rt, float duration = -1)
        {
            if (rt == null) return;
            duration = ResolveDuration(duration);
            var start = rt.anchoredPosition + new Vector2(-24, 0);
            if (DOTweenAvailable)
            {
                DOTween.Kill(rt);
                rt.anchoredPosition = start;
                rt.DOAnchorPos(start + new Vector2(24, 0), duration).SetEase(Ease.OutCubic).SetLink(rt.gameObject);
            }
        }

        public static void SlideInDown(RectTransform rt, float duration = -1)
        {
            if (rt == null) return;
            duration = ResolveDuration(duration);
            var start = rt.anchoredPosition + new Vector2(0, 16);
            if (DOTweenAvailable)
            {
                DOTween.Kill(rt);
                rt.anchoredPosition = start;
                rt.DOAnchorPos(start + new Vector2(0, -16), duration).SetEase(Ease.OutCubic).SetLink(rt.gameObject);
            }
        }

        public static void ScaleIn(RectTransform rt, float duration = -1)
        {
            if (rt == null) return;
            duration = ResolveDuration(duration);
            rt.localScale = new Vector3(0.92f, 0.92f, 1);
            if (DOTweenAvailable)
            {
                DOTween.Kill(rt);
                rt.DOScale(1, duration).SetEase(Ease.OutBack).SetLink(rt.gameObject);
            }
            else
            {
                rt.localScale = Vector3.one;
            }
        }

        // ── Glitch ──

        public static void PlayGlitch(RectTransform rt, float intensity = 3)
        {
            if (rt == null || !DOTweenAvailable) return;
            DOTween.Kill(rt);
            rt.DOShakeAnchorPos(0.4f, intensity, 20, 90, false, true).SetLink(rt.gameObject);
        }

        // ── Diamond Spin (infinite loop — caller must call StopDiamondSpin to clean up) ──

        public static void DiamondSpin(RectTransform rt, float durationPerRotation = 0.8f)
        {
            if (rt == null || !DOTweenAvailable) return;
            DOTween.Kill(rt);
            rt.DORotate(new Vector3(0, 0, -360), durationPerRotation, RotateMode.FastBeyond360)
              .SetEase(Ease.Linear).SetLoops(-1).SetLink(rt.gameObject);
        }

        public static void StopDiamondSpin(RectTransform rt)
        {
            if (rt == null) return;
            DOTween.Kill(rt);
        }

        // ── Pulse Glow (infinite loop — caller must call StopPulseGlow to clean up) ──

        public static void PulseGlow(Graphic graphic, float duration = 2f)
        {
            if (graphic == null || !DOTweenAvailable) return;
            DOTween.Kill(graphic);
            var c = graphic.color;
            graphic.DOColor(new Color(c.r, c.g, c.b, 0.6f), duration * 0.5f)
                   .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetLink(graphic.gameObject);
        }

        public static void StopPulseGlow(Graphic graphic)
        {
            if (graphic == null) return;
            DOTween.Kill(graphic);
        }

        // ── Shake ──

        public static void Shake(RectTransform rt, float duration = 0.4f, float strength = 4)
        {
            if (rt == null || !DOTweenAvailable) return;
            DOTween.Kill(rt);
            rt.DOShakeAnchorPos(duration, strength, 20, 90, false, true).SetLink(rt.gameObject);
        }

        // ── Count Up ──

        public static void CountUp(Text text, int from, int to, float duration = -1)
        {
            if (text == null) return;
            duration = ResolveDuration(duration);
            if (DOTweenAvailable)
            {
                DOTween.Kill(text);
                var current = from;
                DG.Tweening.DOTween.To(() => current, v =>
                {
                    current = v;
                    text.text = Mathf.RoundToInt(v).ToString();
                }, to, duration).SetLink(text.gameObject);
            }
            else
            {
                text.text = to.ToString();
            }
        }
    }
}
