using UnityEngine;

/// <summary>
/// 3D 打出区域 —— 只需挂载在带有 Collider 的对象上
/// 推荐使用 Box Collider，标记为 Is Trigger
/// </summary>
[RequireComponent(typeof(Collider))]
public class PlayZone3D : MonoBehaviour
{
    public static PlayZone3D Instance { get; private set; }
    public int ZoneID; // 区域 ID，对应PlayerID，方便卡牌检测。0是公共区域。

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        // 确保 Collider 是 Trigger
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }
}