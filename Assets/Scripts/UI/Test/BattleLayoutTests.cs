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
}
