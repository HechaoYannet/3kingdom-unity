using UnityEngine;
using DG.Tweening;

/// <summary>
/// 战斗 UI 集中配置，消除散落在各脚本中的硬编码常量。
/// </summary>
[CreateAssetMenu(fileName = "BattleUIConfig", menuName = "ThreeKingdom/Battle UI Config")]
public class BattleUIConfig : ScriptableObject
{
    [Header("手牌扇形布局")]
    [Tooltip("卡牌间距基准值")]
    public float baseCardSpacing = 240f;
    [Tooltip("卡牌间距最大可用宽度")]
    public float maxSpacingWidth = 1600f;
    [Tooltip("扇形角度范围（度）")]
    public float fanAngleRange = 16f;
    [Tooltip("悬停抬升高度")]
    public float hoverLift = 60f;
    [Tooltip("悬停缩放")]
    public float hoverScale = 1.08f;
    [Tooltip("选中缩放")]
    public float selectedScale = 1.04f;
    [Tooltip("基础 Y 偏移")]
    public float baseYOffset = 20f;
    [Tooltip("弧形 Y 沉降系数")]
    public float arcDropFactor = 24f;

    [Header("悬停推开")]
    [Tooltip("推开相邻卡牌的距离（px）")]
    public float pushAmount = 12f;
    [Tooltip("推开衰减系数（每远离 1 张，影响乘以该系数）")]
    public float pushDecayFactor = 0.60f;

    [Header("悬停抖动修复")]
    [Tooltip("悬停退出容差：指针超出卡牌底部多少 px 仍视为悬停中")]
    public float hoverExitTolerance = 88f;

    [Header("拖拽提交")]
    [Tooltip("提交阈值（Canvas 局部坐标）")]
    public float commitThreshold = 220f;
    [Tooltip("提交高度比例（占 Canvas 高度）")]
    public float commitHeightRatio = 0.5f;

    [Header("卡牌尺寸")]
    public float cardWidth = 220f;
    public float cardHeight = 300f;

    [Header("卡牌视觉层比例")]
    public float backLayerRatio = 0.88f;
    public float artLayerRatio = 0.94f;
    public float frontLayerRatio = 0.98f;

    [Header("卡牌透明度")]
    public float dimmedAlpha = 0.55f;
    public float nonInteractableAlpha = 0.34f;
    public float frameAlpha = 0.001f;
    public float cardBackAlpha = 0.96f;

    [Header("目标按钮")]
    public float targetButtonSpacing = 248f;

    [Header("动画插值速度")]
    public float positionLerpSpeed = 16f;
    public float rotationLerpSpeed = 18f;
    public float alphaLerpSpeed = 18f;
    public float parallaxLerpSpeed = 18f;

    [Header("动画缓动")]
    [Tooltip("位移动画缓动，OutQuart=快速启动+均匀柔和减速，长时长下不拖尾")]
    public Ease positionEase = Ease.OutQuart;
    [Tooltip("旋转动画缓动")]
    public Ease rotationEase = Ease.OutQuart;
    [Tooltip("缩放动画缓动，OutBack=轻微过冲回弹，赋予弹出弹性")]
    public Ease scaleEase = Ease.OutBack;
    [Tooltip("透明度动画缓动")]
    public Ease fadeEase = Ease.OutQuart;

    [Header("动画持续时间")]
    [Tooltip("悬停/推开动画持续时长（秒），0 表示使用插值速度反算")]
    public float animationDuration = 0.30f;

    [Header("视差")]
    public float parallaxHoverAmount = 16f;
    public float parallaxIdleAmount = 6f;
    public float parallaxGyroMultiplier = 6f;
    public float backLayerParallax = -0.35f;
    public float artLayerParallax = 0.18f;
    public float frontLayerParallax = 0.42f;

    [Header("配色 - 提示文字")]
    public Color normalHintColor = new Color(0.88f, 0.82f, 0.64f, 0.72f);
    public Color discardHintColor = new Color(1f, 0.6f, 0.3f, 0.9f);
    public Color responseHintColor = new Color(1f, 0.35f, 0.35f, 0.9f);
    public Color waitingHintColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    [Header("配色 - 拖拽引导")]
    public Color guideLineCanCommitColor = new Color(0.98f, 0.88f, 0.32f, 0.72f);
    public Color guideLineDefaultColor = new Color(0.96f, 0.78f, 0.28f, 0.16f);

    [Header("配色 - 卡牌")]
    public Color cardBackgroundColor = new Color(0.08f, 0.09f, 0.13f, 0.96f);
    public Color cardArtFallbackColor = new Color(0.23f, 0.25f, 0.31f, 1f);
    public Color cardTextColor = new Color(0.97f, 0.94f, 0.87f, 1f);
    public Color dragHintColor = new Color(0.96f, 0.91f, 0.72f, 0.82f);

    /// <summary>
    /// 从 ReEndTheme 设计令牌更新配色字段。
    /// </summary>
    public void ApplyReEndColors(ReEndUnity.ReEndTheme reEnd)
    {
        if (reEnd == null) return;

        normalHintColor = new Color(reEnd.textTertiary.r, reEnd.textTertiary.g, reEnd.textTertiary.b, 0.72f);
        discardHintColor = new Color(reEnd.efOrange.r, reEnd.efOrange.g, reEnd.efOrange.b, 0.9f);
        responseHintColor = new Color(reEnd.efRed.r, reEnd.efRed.g, reEnd.efRed.b, 0.9f);
        waitingHintColor = new Color(reEnd.textMuted.r, reEnd.textMuted.g, reEnd.textMuted.b, 0.5f);

        guideLineCanCommitColor = new Color(reEnd.primary.r, reEnd.primary.g, reEnd.primary.b, 0.72f);
        guideLineDefaultColor = new Color(reEnd.primary.r, reEnd.primary.g, reEnd.primary.b, 0.16f);

        cardBackgroundColor = new Color(reEnd.card.r, reEnd.card.g, reEnd.card.b, 0.96f);
        cardArtFallbackColor = reEnd.surface3;
        cardTextColor = reEnd.textPrimary;
        dragHintColor = new Color(reEnd.textAccent.r, reEnd.textAccent.g, reEnd.textAccent.b, 0.82f);
    }
}
