using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 世界空间UI管理器
public class WorldSpaceUIManager : MonoBehaviour
{
    private Transform container;
    private Camera uiCamera;

    // UI元素池
    private Dictionary<string, Queue<GameObject>> uiPools = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<GameObject, string> activeUIElements = new Dictionary<GameObject, string>();

    // 世界UI元素跟踪
    private Dictionary<GameObject, Transform> worldUITargets = new Dictionary<GameObject, Transform>();
    private Dictionary<GameObject, Vector3> worldUIOffsets = new Dictionary<GameObject, Vector3>();

    public void Initialize(Transform container, Camera camera)
    {
        this.container = container;
        this.uiCamera = camera;

        // 预加载常用世界空间UI元素
        PreloadUIElements();
    }

    // 预加载UI元素
    private void PreloadUIElements()
    {
        PreloadUI("DamageText", 10);
        PreloadUI("HealText", 10);
        PreloadUI("CharacterHealthBar", 4);
        PreloadUI("StatusEffectIcon", 10);
    }

    // 预加载特定UI
    private void PreloadUI(string uiName, int count)
    {
        if (!uiPools.ContainsKey(uiName))
        {
            uiPools[uiName] = new Queue<GameObject>();
        }

        GameObject prefab = Resources.Load<GameObject>($"UI/WorldSpace/{uiName}");
        if (prefab == null)
        {
            Debug.LogError($"无法加载世界空间UI预制体: {uiName}");
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
    public GameObject CreateUIElement(string uiName, Vector3 position)
    {
        // 从对象池获取或实例化新对象
        if (uiPools.ContainsKey(uiName) && uiPools[uiName].Count > 0)
        {
            GameObject uiInstance = uiPools[uiName].Dequeue();
            uiInstance.transform.SetParent(container, false);
            uiInstance.transform.position = position;
            uiInstance.SetActive(true);
            activeUIElements[uiInstance] = uiName;

            return uiInstance;
        }
        else
        {
            // 动态加载
            GameObject prefab = Resources.Load<GameObject>($"UI/WorldSpace/{uiName}");
            if (prefab != null)
            {
                GameObject uiInstance = Instantiate(prefab, container);
                uiInstance.transform.position = position;
                activeUIElements[uiInstance] = uiName;

                return uiInstance;
            }
            else
            {
                Debug.LogError($"世界空间UI预制体不存在: {uiName}");
                return null;
            }
        }
    }

    // 创建附着于对象的UI元素
    public GameObject CreateAttachedUIElement(string uiName, Transform target, Vector3 offset)
    {
        GameObject uiElement = CreateUIElement(uiName, target.position + offset);

        if (uiElement != null)
        {
            // 记录附着关系
            worldUITargets[uiElement] = target;
            worldUIOffsets[uiElement] = offset;

            // 启动更新协程
            StartCoroutine(UpdateAttachedUI(uiElement));
        }

        return uiElement;
    }

    // 更新附着UI的位置
    private IEnumerator UpdateAttachedUI(GameObject uiElement)
    {
        while (uiElement.activeInHierarchy && worldUITargets.ContainsKey(uiElement))
        {
            Transform target = worldUITargets[uiElement];
            Vector3 offset = worldUIOffsets[uiElement];

            if (target != null)
            {
                uiElement.transform.position = target.position + offset;

                // 确保UI面向摄像机
                uiElement.transform.rotation = Quaternion.LookRotation(
                    uiElement.transform.position - uiCamera.transform.position
                );
            }

            yield return null;
        }
    }

    // 释放UI元素
    public void ReleaseUIElement(GameObject uiElement)
    {
        if (activeUIElements.ContainsKey(uiElement))
        {
            string uiName = activeUIElements[uiElement];
            uiElement.SetActive(false);

            // 移除附着关系
            if (worldUITargets.ContainsKey(uiElement))
            {
                worldUITargets.Remove(uiElement);
                worldUIOffsets.Remove(uiElement);
            }

            if (!uiPools.ContainsKey(uiName))
            {
                uiPools[uiName] = new Queue<GameObject>();
            }

            uiPools[uiName].Enqueue(uiElement);
            activeUIElements.Remove(uiElement);
        }
    }

    // 创建伤害数字
    public void CreateDamageText(int damage, Vector3 position, bool isCritical = false)
    {
        GameObject damageText = CreateUIElement("DamageText", position);

        if (damageText != null)
        {
            DamageTextController controller = damageText.GetComponent<DamageTextController>();
            if (controller != null)
            {
                controller.Initialize(damage, isCritical);
            }
            else
            {
                // 如果没有控制器，添加一个默认的
                controller = damageText.AddComponent<DamageTextController>();
                controller.Initialize(damage, isCritical);
            }
        }
    }

    // 创建治疗数字
    public void CreateHealText(int healAmount, Vector3 position)
    {
        GameObject healText = CreateUIElement("HealText", position);

        if (healText != null)
        {
            HealTextController controller = healText.GetComponent<HealTextController>();
            if (controller != null)
            {
                controller.Initialize(healAmount);
            }
        }
    }
}