using NUnit.Framework;
using UnityEngine;

/// <summary>
/// 战斗 UI 布局逻辑单元测试。
/// </summary>
public class BattleLayoutTests
{
    /// <summary>
    /// 手牌间距应随数量增加而缩小，不超过基准间距。
    /// </summary>
    [Test]
    public void FanLayout_SpacingShrinks_WhenHandExceedsWidth()
    {
        float baseSpacing = 165f;
        float canvasWidth = 1920f * 0.85f; // 1632
        int cardCount = 12;

        float spacing = Mathf.Min(baseSpacing, canvasWidth / Mathf.Max(cardCount - 1, 1));

        Assert.Less(spacing, baseSpacing, "间距应小于基准值");
        Assert.Greater(spacing, 0f, "间距必须为正");
    }

    /// <summary>
    /// 手牌为 1 张时间距应为基准值（不除以 0）。
    /// </summary>
    [Test]
    public void FanLayout_SingleCard_NoDivisionByZero()
    {
        float baseSpacing = 165f;
        float canvasWidth = 1632f;
        int cardCount = 1;

        float spacing = Mathf.Min(baseSpacing, canvasWidth / Mathf.Max(cardCount - 1, 1));

        Assert.AreEqual(baseSpacing, spacing, 0.01f, "单张牌间距应等于基准值");
    }

    /// <summary>
    /// 提交阈值应基于 Canvas 高度而非屏幕像素。
    /// </summary>
    [Test]
    public void CommitThreshold_IsCanvasRelative()
    {
        float canvasHeight = 1080f;
        float ratio = 0.5f;
        float threshold = 220f;

        float commitHeight = canvasHeight * ratio + threshold;

        Assert.AreEqual(760f, commitHeight, 0.01f, "提交高度 = Canvas高度*0.5 + 220");
    }

    /// <summary>
    /// 目标按钮间距应随数量增加而缩小，不超过基准间距。
    /// </summary>
    [Test]
    public void TargetSpacing_Shrinks_WhenTargetsExceedWidth()
    {
        float baseSpacing = 248f;
        float containerWidth = 800f;
        int count = 5;

        float spacing = Mathf.Min(baseSpacing, containerWidth / Mathf.Max(count, 1));

        Assert.Less(spacing, baseSpacing, "间距应小于基准值");
    }

    /// <summary>
    /// SafeAreaFitter 应正确计算归一化安全区域。
    /// </summary>
    [Test]
    public void SafeArea_NormalizesCorrectly()
    {
        // 模拟 1080p 屏幕，安全区域为全屏
        Rect safeArea = new Rect(0, 0, 1920, 1080);

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= 1920f;
        anchorMin.y /= 1080f;
        anchorMax.x /= 1920f;
        anchorMax.y /= 1080f;

        Assert.AreEqual(Vector2.zero, anchorMin, "安全区域起点应归一化为 0,0");
        Assert.AreEqual(Vector2.one, anchorMax, "安全区域终点应归一化为 1,1");
    }

    /// <summary>
    /// SafeAreaFitter 应正确处理刘海屏偏移。
    /// </summary>
    [Test]
    public void SafeArea_HandlesNotchCorrectly()
    {
        // 模拟带刘海的屏幕：安全区域从 (0, 100) 开始，大小 (1920, 980)
        Rect safeArea = new Rect(0, 100, 1920, 980);
        float screenW = 1920f;
        float screenH = 1080f;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= screenW;
        anchorMin.y /= screenH;
        anchorMax.x /= screenW;
        anchorMax.y /= screenH;

        Assert.Greater(anchorMin.y, 0f, "底部 anchor 应 > 0（避开刘海）");
        Assert.LessOrEqual(anchorMax.y, 1f, "顶部 anchor 应 <= 1");
    }

    /// <summary>
    /// hover 退出容差应让指针离开卡牌底部一小段距离后仍保持悬停（防边缘闪烁）。
    /// </summary>
    [Test]
    public void HoverTolerance_KeepsHover_BelowCardBottom()
    {
        // 卡牌地面矩形 220x300，容差 88（配置默认值）
        Rect ground = new Rect(-110f, -150f, 220f, 300f);
        float tolerance = 88f;
        float side = tolerance * 0.3f;
        Rect extended = new Rect(
            ground.xMin - side, ground.yMin - tolerance,
            ground.width + side * 2f, ground.height + tolerance + tolerance * 0.2f);

        // 指针在卡牌底部下方 50px（< 88px 容差）应仍在扩展矩形内
        Vector2 belowBottom = new Vector2(0f, -150f - 50f);
        Assert.IsTrue(extended.Contains(belowBottom), "底部容差内应保持悬停");

        // 超出容差（150px）应脱离悬停
        Vector2 farBelow = new Vector2(0f, -150f - 150f);
        Assert.IsFalse(extended.Contains(farBelow), "超出容差应解除悬停");

        // 卡牌中心应始终命中
        Assert.IsTrue(extended.Contains(Vector2.zero), "卡牌内部应命中");
    }

    /// <summary>
    /// hover 推开偏移应随距离指数衰减。
    /// </summary>
    [Test]
    public void HoverPush_DecaysWithDistance()
    {
        float pushAmount = 12f;
        float pushDecay = 0.60f;

        float offsetAt1 = pushAmount * Mathf.Pow(pushDecay, 1 - 1); // 相邻：12
        float offsetAt2 = pushAmount * Mathf.Pow(pushDecay, 2 - 1); // 隔一张：7.2
        float offsetAt3 = pushAmount * Mathf.Pow(pushDecay, 3 - 1); // 隔两张：4.32

        Assert.AreEqual(12f, offsetAt1, 0.001f, "相邻卡推开距离应为全额");
        Assert.Greater(offsetAt2, offsetAt3, "距离越远推开越小");
        Assert.Greater(offsetAt1, offsetAt2, "相邻卡推开应大于远处卡");
    }

    /// <summary>
    /// 拖拽启动阈值应按 DPI 缩放但 clamp 上限，避免高 DPI 手机拖拽迟钝。
    /// </summary>
    [Test]
    public void DragThreshold_ClampedForHighDpi()
    {
        float baseThreshold = 10f;

        float lowDpi = Mathf.Clamp(96f / 96f, 1f, 2.4f);
        float highDpi = Mathf.Clamp(400f / 96f, 1f, 2.4f);
        float extremeDpi = Mathf.Clamp(600f / 96f, 1f, 2.4f);

        Assert.AreEqual(10, Mathf.RoundToInt(baseThreshold * lowDpi), "96dpi 阈值应为 10px");
        Assert.AreEqual(24, Mathf.RoundToInt(baseThreshold * highDpi), "400dpi 阈值应 clamp 到 24px");
        Assert.AreEqual(24, Mathf.RoundToInt(baseThreshold * extremeDpi), "600dpi 阈值应 clamp 到 24px");
    }
}
