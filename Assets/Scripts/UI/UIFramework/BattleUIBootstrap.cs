using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 绑定场景内既有 HUD 根节点、事件系统和屏幕空间手牌表现。
/// </summary>
public class BattleUIBootstrap : MonoBehaviour
{
    private static BattleUIBootstrap instance;

    /// <summary>
    /// 全局字体资源，消除各文件重复硬编码的字体路径。
    /// </summary>
    public static TMP_FontAsset DefaultFont =>
        Resources.Load<TMP_FontAsset>("Fonts/SourceHanSansSC SDF") ?? TMP_Settings.defaultFontAsset;

    [Header("Scene References")]
    [SerializeField] private RectTransform presenterParent;
    [SerializeField] private BattleHandPresenter handPresenterPrefab;
    [SerializeField] private BattleHandPresenter handPresenterInstance;
    [SerializeField] private Player boundPlayerOverride;

    [Header("HUD Components")]
    [SerializeField] private BattlePhaseHUD phaseHUD;
    [SerializeField] private BattlePlayerHUD playerHUD;
    [SerializeField] private BattlePlayerHUD enemyHUD;
    [SerializeField] private BattleResultPanel resultPanel;

    /// <summary>
    /// 确保运行时战斗 HUD 可用，优先使用场景中已有实例。
    /// </summary>
    /// <returns>战斗 HUD 引导组件。</returns>
    public static BattleUIBootstrap EnsureRuntimeUI()
    {
        if (instance != null)
        {
            return instance;
        }

        BattleUIBootstrap[] bootstraps = Resources.FindObjectsOfTypeAll<BattleUIBootstrap>();
        foreach (BattleUIBootstrap bootstrap in bootstraps)
        {
            if (bootstrap != null && bootstrap.gameObject.scene.IsValid())
            {
                instance = bootstrap;
                return instance;
            }
        }

        GameObject fallbackObject = new GameObject("BattleUIBootstrap(Fallback)");
        instance = fallbackObject.AddComponent<BattleUIBootstrap>();
        Debug.LogWarning("BattleUIBootstrap fallback instance created. Please bind scene references in the editor.");
        return instance;
    }

    /// <summary>
    /// 绑定战斗开始后的运行时对象。
    /// </summary>
    public void BindBattle()
    {
        ApplyReEndThemeBridge();
        EnsureEventSystem();
        EnsureHandPresenter();

        if (handPresenterInstance != null)
        {
            handPresenterInstance.BindPlayer(GetBoundPlayer());
        }

        BindHUDComponents();
    }

    /// <summary>
    /// 将 ReEndTheme 设计令牌桥接到 BattleTheme / BattleUIConfig，
    /// 并确保 BattleCardSpriteLibrary 获得主题引用。
    /// </summary>
    private void ApplyReEndThemeBridge()
    {
        var reEndTheme = ReEndUnity.ReEndThemeManager.Current;
        if (reEndTheme == null)
        {
            Debug.LogWarning("[BattleUIBootstrap] ReEndTheme not available, skipping theme bridge.");
            return;
        }

        // 加载或复用 BattleTheme
        BattleTheme battleTheme = Resources.Load<BattleTheme>("BattleTheme");
        if (battleTheme != null)
        {
            battleTheme.ApplyReEndTheme(reEndTheme);
            BattleCardSpriteLibrary.SetTheme(battleTheme);
        }

        // 加载或复用 BattleUIConfig
        BattleUIConfig uiConfig = Resources.Load<BattleUIConfig>("BattleUIConfig");
        if (uiConfig != null)
        {
            uiConfig.ApplyReEndColors(reEndTheme);
        }

        EnsureSceneBackground(reEndTheme);
    }

    /// <summary>
    /// 在 Canvas 底层添加 ReEndUI GridBackground shader 全屏背景。
    /// </summary>
    private void EnsureSceneBackground(ReEndUnity.ReEndTheme reEndTheme)
    {
        var canvas = GetComponentInParent<Canvas>();
        if (canvas == null) return;

        Transform existing = canvas.transform.Find("ReEndGridBG");
        if (existing != null) return;

        var bgGo = new GameObject("ReEndGridBG", typeof(RectTransform), typeof(Image));
        bgGo.transform.SetParent(canvas.transform, false);
        bgGo.transform.SetAsFirstSibling();

        var img = bgGo.GetComponent<Image>();
        img.color = Color.clear;
        img.raycastTarget = false;

        var gridMat = new Material(Shader.Find("ReEnd/UI/GridBackground"));
        if (gridMat != null)
        {
            gridMat.SetFloat("_GridSize", reEndTheme.bgGridSize);
            gridMat.SetColor("_GridColor", reEndTheme.bgGridColor);
        }
        img.material = gridMat;

        var rt = bgGo.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    private void BindHUDComponents()
    {
        Player boundPlayer = GetBoundPlayer();
        Player enemyPlayer = PlayerManager.Instance?.RuntimeEnemyPlayer;

        // PhaseHUD
        if (phaseHUD == null)
        {
            phaseHUD = FindHUDInCanvas<BattlePhaseHUD>("PhaseHUD");
        }
        if (phaseHUD != null && boundPlayer != null)
        {
            phaseHUD.BindPlayer(boundPlayer);
            phaseHUD.RefreshRoleNames();
        }

        // PlayerHUD
        if (playerHUD == null)
        {
            playerHUD = FindHUDInCanvas<BattlePlayerHUD>("PlayerHUD");
        }
        if (playerHUD != null && boundPlayer != null)
        {
            playerHUD.Bind(boundPlayer);
        }

        // EnemyHUD
        if (enemyHUD == null)
        {
            enemyHUD = FindHUDInCanvas<BattlePlayerHUD>("EnemyHUD");
        }
        if (enemyHUD != null && enemyPlayer != null)
        {
            enemyHUD.Bind(enemyPlayer);
        }

        // ResultPanel
        if (resultPanel == null)
        {
            resultPanel = FindHUDInCanvas<BattleResultPanel>("ResultPanel");
        }

        // Hook GameOver — 先取消旧注册防止重复绑定
        if (resultPanel != null && GamePlaying.instance != null)
        {
            GamePlaying.instance.OnGameResult -= resultPanel.Show;
            GamePlaying.instance.OnGameResult += resultPanel.Show;
        }
        else if (resultPanel != null)
        {
            StartCoroutine(HookGameOverDelayed());
        }
    }

    private System.Collections.IEnumerator HookGameOverDelayed()
    {
        yield return new WaitUntil(() => GamePlaying.instance != null);
        GamePlaying.instance.OnGameResult -= resultPanel.Show;
        GamePlaying.instance.OnGameResult += resultPanel.Show;
    }

    private static T FindHUDInCanvas<T>(string name) where T : Component
    {
        var canvas = FindObjectOfType<Canvas>();
        if (canvas == null) return null;
        var t = canvas.transform.Find(name);
        return t != null ? t.GetComponent<T>() : null;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void EnsureEventSystem()
    {
        EventSystem existing = FindObjectOfType<EventSystem>();
        if (existing != null)
        {
            AdjustDragThreshold(existing);
            return;
        }

        GameObject eventSystemObject = new GameObject("EventSystem", typeof(EventSystem));
        Type inputSystemModuleType = Type.GetType("UnityEngine.InputSystem.UI.InputSystemUIInputModule, Unity.InputSystem");
        if (inputSystemModuleType != null)
        {
            eventSystemObject.AddComponent(inputSystemModuleType);
        }
        else
        {
            eventSystemObject.AddComponent<StandaloneInputModule>();
        }

        AdjustDragThreshold(eventSystemObject.GetComponent<EventSystem>());
    }

    /// <summary>
    /// 根据 DPI 调整拖拽阈值，确保移动端触控手感一致。
    /// 阈值 clamp 在 10-24px：高 DPI 手机按比例放大过大会让拖拽启动迟钝。
    /// </summary>
    private static void AdjustDragThreshold(EventSystem eventSystem)
    {
        if (eventSystem == null) return;
        float baseThreshold = 10f;
        float dpiScale = Screen.dpi > 0 ? Mathf.Clamp(Screen.dpi / 96f, 1f, 2.4f) : 1f;
        eventSystem.pixelDragThreshold = Mathf.RoundToInt(baseThreshold * dpiScale);
    }

    private void EnsureHandPresenter()
    {
        if (handPresenterInstance != null)
        {
            return;
        }

        RectTransform parent = presenterParent != null ? presenterParent : transform as RectTransform;
        if (parent == null)
        {
            Debug.LogError("BattleUIBootstrap requires a presenter parent under an existing scene Canvas.");
            return;
        }

        handPresenterInstance = GetComponentInChildren<BattleHandPresenter>(true);
        if (handPresenterInstance == null)
        {
            if (handPresenterPrefab == null)
            {
                Debug.LogError("BattleUIBootstrap requires a BattleHandPresenter prefab.");
                return;
            }

            handPresenterInstance = Instantiate(handPresenterPrefab, parent);
            handPresenterInstance.name = handPresenterPrefab.name;
        }

        handPresenterInstance.Initialize();
    }

    private Player GetBoundPlayer()
    {
        return boundPlayerOverride != null
            ? boundPlayerOverride
            : PlayerManager.Instance != null
                ? PlayerManager.Instance.TEMP_PLAYER
                : null;
    }
}
