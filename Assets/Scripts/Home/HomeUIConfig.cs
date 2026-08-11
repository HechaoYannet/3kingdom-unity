using System;
using UnityEngine;
using DG.Tweening;

namespace ReEndUnity
{
    /// <summary>
    /// 主界面 UI 配置 — 对标 BattleUIConfig，所有布局/动画/色彩参数集中管理。
    /// </summary>
    [CreateAssetMenu(menuName = "ThreeKingdom/Home UI Config")]
    public class HomeUIConfig : ScriptableObject
    {
        // ── Layout: Right Card Stack ──
        [Header("Right Card Stack")]
        public float stackWidth = 360f;
        public float stackRightMargin = 24f;
        public float cardSpacing = 12f;
        public float primaryCardHeight = 200f;   // 作战
        public float standardCardHeight = 120f;  // 剧本/编队/图鉴/商店
        public float smallCardHeight = 80f;      // 设置

        // ── Layout: Left Art Zone ──
        [Header("Left Art Zone")]
        [Range(0.3f, 0.8f)]
        public float artZoneWidthRatio = 0.55f;
        public float titleFontSize = 56f;
        public float subtitleFontSize = 16f;

        // ── Layout: TopBar ──
        [Header("TopBar")]
        public float topBarHeight = 48f;
        public float resourceItemSpacing = 16f;
        public float resourceItemWidth = 90f;

        // ── Layout: Bottom Corners ──
        [Header("Bottom Corners")]
        public Vector2 bottomLeftSize = new(200, 80);
        public Vector2 bottomRightSize = new(200, 80);
        public float cornerMargin = 24f;

        // ── Canvas ──
        [Header("Canvas")]
        public Vector2 referenceResolution = new(1920, 1080);
        public float matchWidthOrHeight = 0.5f;

        // ── Animation ──
        [Header("Animation")]
        public Ease entrySlide = Ease.OutQuart;
        public Ease entryScale = Ease.OutBack;
        public float bgFadeDuration = 0.30f;
        public float titleSlideDuration = 0.50f;
        public float cardSlideDuration = 0.35f;
        public float cardStaggerDelay = 0.10f;
        public float topBarDuration = 0.30f;
        public float cornerFadeDuration = 0.25f;
        public float footerFadeDuration = 0.20f;

        // ── Colors (bridged from ReEndTheme at runtime) ──
        [Header("Colors (auto-bridged from ReEndTheme)")]
        public Color backgroundColor = new(0.031f, 0.031f, 0.063f);  // #080810
        public Color gridAlpha = new(1, 1, 1, 0.04f);
        public Color cardBackground;     // theme.surfaceCanvas
        public Color cardHover;         // theme.surfaceHover
        public Color accentGold;         // theme.primary
        public Color accentRed;         // theme.destructive
        public Color accentCyan;       // theme.efCyan
        public Color textPrimary;      // theme.textPrimary
        public Color textMuted;      // theme.textMuted

        /// <summary>从 ReEndTheme 桥接主题色到本配置。</summary>
        public void ApplyReEndColors(ReEndTheme theme)
        {
            if (theme == null) return;
            cardBackground = theme.surfaceCanvas;
            cardHover = theme.surfaceHover;
            accentGold = theme.primary;
            accentRed = theme.destructive;
            accentCyan = theme.efCyan;
            textPrimary = theme.textPrimary;
            textMuted = theme.textMuted;
        }
    }
}
