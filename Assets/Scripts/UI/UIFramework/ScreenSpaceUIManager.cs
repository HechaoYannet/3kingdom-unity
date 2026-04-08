using System.Collections.Generic;
using UnityEngine;


public interface IScreenSpaceElement
{
    void UpdateElement(object data);
}

// 屏幕空间UI管理器
public class ScreenSpaceUIManager : MonoBehaviour
{
    private Transform container;
    private Camera uiCamera;

    // UI元素池
    private Dictionary<string, Queue<GameObject>> uiPools = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<GameObject, string> activeUIElements = new Dictionary<GameObject, string>();

    // HUD元素注册表
    private Dictionary<string, IScreenSpaceElement> hudElements = new Dictionary<string, IScreenSpaceElement>();

    public void Initialize(Transform container, Camera camera)
    {
        this.container = container;
        this.uiCamera = camera;

        // 预加载常用UI元素
        PreloadUIElements();
    }

    // 预加载UI元素
    private void PreloadUIElements()
    {
        PreloadUI("TurnIndicator", 1);
        PreloadUI("PlayerStatus", 2);
        PreloadUI("HandArea", 1);
        PreloadUI("ActionButtons", 1);
    }

    // 预加载特定UI
    private void PreloadUI(string uiName, int count)
    {
        if (!uiPools.ContainsKey(uiName))
        {
            uiPools[uiName] = new Queue<GameObject>();
        }

        GameObject prefab = Resources.Load<GameObject>($"UI/ScreenSpace/{uiName}");
        if (prefab == null)
        {
            Debug.LogError($"无法加载屏幕空间UI预制体: {uiName}");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            GameObject uiInstance = Instantiate(prefab, container);
            uiInstance.SetActive(false);
            uiPools[uiName].Enqueue(uiInstance);
        }
    }

    // 创建UI元素
    public GameObject CreateUIElement(string uiName, Vector3 position = default)
    {
        // 从对象池获取或实例化新对象
        if (uiPools.ContainsKey(uiName) && uiPools[uiName].Count > 0)
        {
            GameObject uiInstance = uiPools[uiName].Dequeue();
            uiInstance.transform.SetParent(container, false);

            if (position != default)
            {
                uiInstance.transform.position = position;
            }

            uiInstance.SetActive(true);
            activeUIElements[uiInstance] = uiName;

            return uiInstance;
        }
        else
        {
            // 动态加载
            GameObject prefab = Resources.Load<GameObject>($"UI/ScreenSpace/{uiName}");
            if (prefab != null)
            {
                GameObject uiInstance = Instantiate(prefab, container);

                if (position != default)
                {
                    uiInstance.transform.position = position;
                }

                activeUIElements[uiInstance] = uiName;
                return uiInstance;
            }
            else
            {
                Debug.LogError($"屏幕空间UI预制体不存在: {uiName}");
                return null;
            }
        }
    }

    // 释放UI元素
    public void ReleaseUIElement(GameObject uiElement)
    {
        if (activeUIElements.ContainsKey(uiElement))
        {
            string uiName = activeUIElements[uiElement];
            uiElement.SetActive(false);

            if (!uiPools.ContainsKey(uiName))
            {
                uiPools[uiName] = new Queue<GameObject>();
            }

            uiPools[uiName].Enqueue(uiElement);
            activeUIElements.Remove(uiElement);
        }
    }

    // 注册HUD元素
    public void RegisterHUDElement(string elementId, IScreenSpaceElement element)
    {
        hudElements[elementId] = element;
    }

    // 更新HUD元素
    public void UpdateHUDElement(string elementId, object data)
    {
        if (hudElements.ContainsKey(elementId))
        {
            hudElements[elementId].UpdateElement(data);
        }
    }

    // 屏幕坐标转换
    public Vector3 WorldToScreenPoint(Vector3 worldPosition)
    {
        return uiCamera.WorldToScreenPoint(worldPosition);
    }

    // 屏幕坐标转换（考虑Canvas缩放）
    public Vector2 WorldToCanvasPoint(Vector3 worldPosition, Canvas canvas)
    {
        Vector3 screenPoint = WorldToScreenPoint(worldPosition);
        Vector2 canvasPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPoint,
            uiCamera,
            out canvasPoint
        );

        return canvasPoint;
    }
}