using System.Collections.Generic;
using UnityEngine;

// UI空间类型
public enum UISpaceType
{
    ScreenSpace,    // 屏幕空间UI (Canvas - Screen Space)
    WorldSpace      // 世界空间UI (Canvas - World Space)
}

// 改进的UI管理器
public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance => _instance;

    [Header("UI空间配置")]
    [SerializeField] private Transform screenSpaceContainer;
    [SerializeField] private Transform worldSpaceContainer;

    [Header("UI摄像机")]
    [SerializeField] private Camera screenSpaceCamera;
    [SerializeField] private Camera worldSpaceCamera;

    // 管理器实例
    private ScreenSpaceUIManager screenSpaceManager;
    private WorldSpaceUIManager worldSpaceManager;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // 初始化子管理器
        InitializeSubManagers();
    }

    // 初始化子管理器
    private void InitializeSubManagers()
    {
        screenSpaceManager = gameObject.AddComponent<ScreenSpaceUIManager>();
        screenSpaceManager.Initialize(screenSpaceContainer, screenSpaceCamera);

        worldSpaceManager = gameObject.AddComponent<WorldSpaceUIManager>();
        worldSpaceManager.Initialize(worldSpaceContainer, worldSpaceCamera);
    }

    // 获取屏幕空间UI管理器
    public ScreenSpaceUIManager GetScreenSpaceManager()
    {
        return screenSpaceManager;
    }

    // 获取世界空间UI管理器
    public WorldSpaceUIManager GetWorldSpaceManager()
    {
        return worldSpaceManager;
    }

    // 创建UI元素
    public GameObject CreateUI(string uiName, UISpaceType spaceType, Vector3 position = default)
    {
        switch (spaceType)
        {
            case UISpaceType.ScreenSpace:
                return screenSpaceManager.CreateUIElement(uiName, position);
            case UISpaceType.WorldSpace:
                return worldSpaceManager.CreateUIElement(uiName, position);
            default:
                return null;
        }
    }

    // 释放UI元素
    public void ReleaseUI(GameObject uiElement, UISpaceType spaceType)
    {
        switch (spaceType)
        {
            case UISpaceType.ScreenSpace:
                screenSpaceManager.ReleaseUIElement(uiElement);
                break;
            case UISpaceType.WorldSpace:
                worldSpaceManager.ReleaseUIElement(uiElement);
                break;
        }
    }
}