using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using TMPro;
using ReEndUnity;

/// <summary>
/// 主界面场景引导器 — 对标 BattleUIBootstrap。
/// 负责: ReEnd 主题加载 → EventSystem 配置 → Canvas 创建 → HomePresenter 委托 → SafeArea 适配。
/// </summary>
[DefaultExecutionOrder(-50)]
public class HomeBootstrap : MonoBehaviour
{
    private HomeUIConfig config;
    private HomePresenter presenter;
    private TMP_FontAsset _cjkFont;

    private void Awake()
    {
        config = Resources.Load<HomeUIConfig>("HomeUIConfig");
        if (config == null)
        {
            Debug.LogError("[HomeBootstrap] HomeUIConfig not found in Resources!");
            return;
        }

        EnsureReEndTheme();
        EnsureTMPFont();
        EnsureEventSystem();
        var canvas = EnsureHomeCanvas();
        presenter = new HomePresenter(config);
        presenter.Build(canvas.transform);
        ApplyFontToAllTMP(canvas);
        ApplySafeArea(canvas);
    }

    /// <summary>确保 ReEnd 主题已加载并桥接到 HomeUIConfig。</summary>
    private void EnsureReEndTheme()
    {
        var theme = ReEndThemeManager.Current;
        if (theme != null)
            config.ApplyReEndColors(theme);
    }

    /// <summary>加载 SourceHanSansSC SDF 字体并设为 TMP 默认字体。</summary>
    private void EnsureTMPFont()
    {
        var font = Resources.Load<TMP_FontAsset>("Fonts/SourceHanSansSC SDF");
        if (font != null)
        {
            _cjkFont = font;
            Debug.Log("[HomeBootstrap] SourceHanSansSC SDF loaded");
        }
        else
        {
            Debug.LogWarning("[HomeBootstrap] SourceHanSansSC SDF not found in Resources!");
        }
    }

    /// <summary>为 Canvas 下所有 TMP_Text 设置中文字体。</summary>
    private void ApplyFontToAllTMP(Canvas canvas)
    {
        if (_cjkFont == null) return;
        foreach (var tmp in canvas.GetComponentsInChildren<TMP_Text>(true))
            tmp.font = _cjkFont;
    }

    /// <summary>确保场景中存在 EventSystem 且使用 InputSystemUIInputModule。</summary>
    private void EnsureEventSystem()
    {
        var es = FindAnyObjectByType<EventSystem>();
        if (es == null)
        {
            var go = new GameObject("EventSystem");
            es = go.AddComponent<EventSystem>();
        }

        // 移除旧版 StandaloneInputModule
        var standalone = es.GetComponent<StandaloneInputModule>();
        if (standalone != null) Destroy(standalone);

        // 添加 InputSystemUIInputModule
        if (es.GetComponent<InputSystemUIInputModule>() == null)
            es.gameObject.AddComponent<InputSystemUIInputModule>();
    }

    /// <summary>创建 Screen Space Overlay Canvas + CanvasScaler。</summary>
    private Canvas EnsureHomeCanvas()
    {
        var go = new GameObject("MainHomeCanvas",
            typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));

        var canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 0;

        var scaler = go.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = config.referenceResolution;
        scaler.matchWidthOrHeight = config.matchWidthOrHeight;

        return canvas;
    }

    /// <summary>添加 SafeAreaFitter 适配刘海屏。</summary>
    private void ApplySafeArea(Canvas canvas)
    {
        canvas.gameObject.AddComponent<SafeAreaFitter>();
    }
}
