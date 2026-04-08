using UnityEngine;

// 场景和摄像机管理器
public class SceneCameraManager : MonoBehaviour
{
    [Header("摄像机参考")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera uiCamera;

    [Header("场景层级")]
    [SerializeField] private LayerMask defaultLayer;
    [SerializeField] private LayerMask uiLayer;
    [SerializeField] private LayerMask cardLayer;

    [Header("摄像机位置")]
    [SerializeField] private Transform tableTopView;
    [SerializeField] private Transform playerFocusView;
    [SerializeField] private Transform overviewView;

    void Start()
    {
        ConfigureCameras();
        SetInitialView();
    }

    // 配置摄像机参数
    private void ConfigureCameras()
    {
        // 主摄像机 - 渲染游戏场景
        mainCamera.cullingMask = ~uiLayer; // 不渲染UI层
        mainCamera.depth = 0;

        // UI摄像机 - 只渲染UI
        uiCamera.cullingMask = uiLayer;
        uiCamera.depth = 1;
        uiCamera.clearFlags = CameraClearFlags.Depth;

        // 设置卡牌层的渲染顺序
        SetCardRenderingOrder();
    }

    // 设置卡牌渲染顺序
    private void SetCardRenderingOrder()
    {
        // 为卡牌模型设置单独的渲染队列，确保正确显示
        Renderer[] cardRenderers = FindObjectsOfType<Renderer>();
        foreach (Renderer renderer in cardRenderers)
        {
            if (renderer.gameObject.layer == LayerMask.NameToLayer("Cards"))
            {
                renderer.material.renderQueue = 3000; // 透明物体之后渲染
            }
        }
    }

    // 设置初始视角
    private void SetInitialView()
    {
        MoveCameraTo(tableTopView.position, tableTopView.rotation);
    }

    // 移动摄像机到指定位置和旋转
    public void MoveCameraTo(Vector3 position, Quaternion rotation, float duration = 1.0f)
    {
        StartCoroutine(MoveCameraRoutine(position, rotation, duration));
    }

    private System.Collections.IEnumerator MoveCameraRoutine(Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;

        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);

            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.transform.rotation = targetRotation;
    }

    // 聚焦到玩家手牌
    public void FocusOnPlayerHand()
    {
        MoveCameraTo(playerFocusView.position, playerFocusView.rotation, 0.7f);
    }

    // 返回桌面全景
    public void ReturnToTableTop()
    {
        MoveCameraTo(tableTopView.position, tableTopView.rotation, 0.7f);
    }

    // 切换视角到全局概览
    public void SwitchToOverview()
    {
        MoveCameraTo(overviewView.position, overviewView.rotation, 1.0f);
    }
}