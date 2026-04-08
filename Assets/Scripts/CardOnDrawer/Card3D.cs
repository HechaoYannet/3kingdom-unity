using UnityEngine;

/// <summary>
/// 3D 卡牌核心逻辑 —— 使用 OnMouse 系列事件实现拖拽
/// 要求卡牌预制体上必须带有 Collider（推荐 Box Collider）
/// Prefab实例化前请确保已正确设置卡牌数据（Card）
/// </summary>
[RequireComponent(typeof(Collider))]
public class Card3D : MonoBehaviour
{
    [Header("卡牌数据")]
    public Card cardData;               // 卡牌脚本模型，包含卡牌技能、信息和效果描述

    [Header("拖拽设置")]
    public float dragHeightOffset = 1.0f;   // 拖拽时卡牌提升的高度（世界单位）
    public float dragScaleMultiplier = 1.2f;// 拖拽时放大倍数
    public LayerMask playZoneLayer;         // 打出区域的 Layer，用于检测

    // 内部状态
    [HideInInspector] public bool isDragging = false;
    private Vector3 originalPosition;       // 拖拽前的原始位置（手牌位置）
    private Quaternion originalRotation;    // 原始旋转（手牌扇形朝向）
    private Vector3 originalScale;          // 原始缩放
    private float originalY;               // 原始 Y 轴高度

    // 组件缓存
    private Collider cardCollider;
    private HandManager3D handManager;

    // 事件：当卡牌数据变更时通知视图刷新
    public event System.Action OnCardDataChanged;

    private void Awake()
    {
        cardData = GetComponent<Card>();
        cardCollider = GetComponent<Collider>();
        handManager = FindObjectOfType<HandManager3D>(); // 或使用单例
        originalScale = transform.localScale;
    }

    private void Start()
    {
        // 首次刷新视图
        GetComponent<CardView3D>()?.RefreshUI();
    }

    /// <summary>
    /// 初始化卡牌（从卡组生成时调用）
    /// </summary>
    public void Initialize(Card card)
    {
        card.transform.SetParent(transform, false); // 将卡牌数据对象作为子对象，方便管理
        cardData = card;
        OnCardDataChanged?.Invoke();
    }

    // -------------------- 鼠标交互（需 Collider）--------------------
    private void OnMouseDown()
    {
        if (!CanDrag()) return;

        isDragging = true;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalY = transform.position.y;

        // 视觉反馈：提升高度 + 放大
        transform.position += Vector3.up * dragHeightOffset;
        transform.localScale = originalScale * dragScaleMultiplier;

        // 通知手牌管理器：开始拖拽，暂时从手牌布局中移除
        handManager?.OnCardDragStart(this);
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;

        // 将鼠标屏幕坐标转换为 3D 世界坐标，保持 Y 轴为拖拽高度
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.WorldToScreenPoint(originalPosition).z; // 保持原始深度
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.y = originalY + dragHeightOffset; // 固定拖拽高度
        transform.position = worldPos;
    }

    private void OnMouseUp()
    {
        if (!isDragging) return;

        isDragging = false;

        // 还原缩放和旋转（位置稍后决定）
        transform.localScale = originalScale;
        transform.rotation = originalRotation;

        // 检测是否在打出区域内
        bool played = IsOverPlayZone(out PlayZone3D playZone);

        if (played && CanPlayCard())
        {
            //PlayCard(playZone.ZoneID);
            Debug.Log($"打出卡牌：{cardData.cardName} 到区域 {playZone.ZoneID}");
        }
        else
        {
            // 未打出：回归原位，并通知手牌管理器恢复布局
            transform.position = originalPosition;
            handManager?.OnCardDragEnd(this, false);
        }
    }

    // -------------------- 辅助判定 --------------------
    private bool CanDrag()
    {
        // 示例条件：己方回合且法力足够
        //return TurnManager.Instance != null &&
        //       TurnManager.Instance.IsPlayerTurn &&
        //       ManaManager.Instance.CurrentMana >= cardData.manaCost;
        return true; // 先放行，后续根据游戏逻辑完善
    }

    private bool CanPlayCard()
    {
        //return ManaManager.Instance.CurrentMana >= cardData.manaCost;
        return true; // 先放行，后续根据游戏逻辑完善
    }

    /// <summary>
    /// 检测当前卡牌是否位于打出区域内
    /// </summary>
    private bool IsOverPlayZone(out PlayZone3D playZone)
    {
        // 在卡牌位置发射一个向下的射线，检测是否碰到 PlayZone 的碰撞体
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2f, playZoneLayer))
        {
            PlayZone3D zone = hit.collider.GetComponent<PlayZone3D>();
            playZone = zone;
            return zone != null;
        }
        playZone = null;
        return false;
    }

    /// <summary>
    /// 打出卡牌 —— 消耗法力、触发效果、销毁对象
    /// </summary>
    //public void PlayCard(int target)
    //{
    //    // 消耗法力
    //    //ManaManager.Instance?.SpendMana(cardData.manaCost);

    //    // 触发全局事件（供技能系统监听）
    //    EventManager.TriggerCardPlayed(cardData, target);

    //    // 从手牌移除并销毁
    //    handManager?.RemoveCard(this);
    //    Destroy(gameObject);

    //    Debug.Log($"打出卡牌：{cardData.cardName}");
    //}

    // 供外部调用的数据更新接口
    public void SetAttack(int newAttack) { /* 修改运行时数据，并触发事件 */ OnCardDataChanged?.Invoke(); }
}