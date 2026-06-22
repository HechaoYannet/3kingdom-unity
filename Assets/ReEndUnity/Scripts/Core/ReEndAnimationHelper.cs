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

        // ── Fade ──

        public static void FadeIn(CanvasGroup cg, float duration = -1)
        {
            if (cg == null) return;
            if (duration < 0) duration = ReEndThemeManager.Current.durationNormal;
            cg.alpha = 0;
            if (DOTweenAvailable)
            {
                cg.DOFade(1, duration);
            }
            else
            {
                cg.alpha = 1;
            }
        }

        public static void FadeOut(CanvasGroup cg, float duration = -1, Action onComplete = null)
        {
            if (cg == null) return;
            if (duration < 0) duration = ReEndThemeManager.Current.durationNormal;
            if (DOTweenAvailable)
            {
                cg.DOFade(0, duration).OnComplete(() => onComplete?.Invoke());
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
            if (duration < 0) duration = ReEndThemeManager.Current.durationNormal;
            var start = rt.anchoredPosition + new Vector2(0, -16);
            if (DOTweenAvailable)
            {
                rt.anchoredPosition = start;
                rt.DOAnchorPos(start + new Vector2(0, 16), duration).SetEase(Ease.OutCubic);
            }
        }

        public static void SlideInRight(RectTransform rt, float duration = -1)
        {
            if (rt == null) return;
            if (duration < 0) duration = ReEndThemeManager.Current.durationNormal;
            var start = rt.anchoredPosition + new Vector2(-24, 0);
            if (DOTweenAvailable)
            {
                rt.anchoredPosition = start;
                rt.DOAnchorPos(start + new Vector2(24, 0), duration).SetEase(Ease.OutCubic);
            }
        }

        public static void SlideInDown(RectTransform rt, float duration = -1)
        {
            if (rt == null) return;
            if (duration < 0) duration = ReEndThemeManager.Current.durationNormal;
            var start = rt.anchoredPosition + new Vector2(0, 16);
            if (DOTweenAvailable)
            {
                rt.anchoredPosition = start;
                rt.DOAnchorPos(start + new Vector2(0, -16), duration).SetEase(Ease.OutCubic);
            }
        }

        public static void ScaleIn(RectTransform rt, float duration = -1)
        {
            if (rt == null) return;
            if (duration < 0) duration = ReEndThemeManager.Current.durationNormal;
            rt.localScale = new Vector3(0.92f, 0.92f, 1);
            if (DOTweenAvailable)
            {
                rt.DOScale(1, duration).SetEase(Ease.OutBack);
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
            var orig = rt.anchoredPosition;
            rt.DOShakeAnchorPos(0.4f, intensity, 20, 90, false, true);
        }

        // ── Diamond Spin ──

        public static void DiamondSpin(RectTransform rt, float durationPerRotation = 0.8f)
        {
            if (rt == null || !DOTweenAvailable) return;
            rt.DORotate(new Vector3(0, 0, -360), durationPerRotation, RotateMode.FastBeyond360)
              .SetEase(Ease.Linear).SetLoops(-1);
        }

        // ── Pulse Glow ──

        public static void PulseGlow(Graphic graphic, float duration = 2f)
        {
            if (graphic == null || !DOTweenAvailable) return;
            // Approximate with alpha pulse on a glow child
            var c = graphic.color;
            graphic.DOColor(new Color(c.r, c.g, c.b, 0.6f), duration * 0.5f)
                   .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }

        // ── Shake ──

        public static void Shake(RectTransform rt, float duration = 0.4f, float strength = 4)
        {
            if (rt == null || !DOTweenAvailable) return;
            rt.DOShakeAnchorPos(duration, strength, 20, 90, false, true);
        }

        // ── Count Up ──

        public static void CountUp(Text text, int from, int to, float duration = -1)
        {
            if (text == null) return;
            if (duration < 0) duration = ReEndThemeManager.Current.durationNormal;
            if (DOTweenAvailable)
            {
                var current = from;
                DG.Tweening.DOTween.To(() => current, v =>
                {
                    current = v;
                    text.text = Mathf.RoundToInt(v).ToString();
                }, to, duration);
            }
            else
            {
                text.text = to.ToString();
            }
        }
    }
}
