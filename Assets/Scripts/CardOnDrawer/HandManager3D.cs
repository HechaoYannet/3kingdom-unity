using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 3D 手牌管理器 —— 控制卡牌在三维空间中的扇形布局
/// </summary>
public class HandManager3D : MonoBehaviour
{
    [Header("手牌预制体")]
    //public GameObject cardPrefab;           // 必须包含 Card3D 和 CardView3D
    Vector3 position;
    Quaternion rotation;

    [Header("手牌布局参数")]
    public Transform handCenter;           // 手牌扇形圆心（世界坐标）
    public float radius = 5f;             // 扇形半径
    public float angleRange = 60f;        // 总角度范围（度）
    public float verticalOffset = 0f;     // 垂直偏移（Y轴）
    public float cardSpacing = 1.2f;      // 卡牌间距系数（用于动态计算）

    [Header("限制")]
    public int maxHandSize = 10;

    public static HandManager3D Instance { get; private set; }

    public Player player; // 当前玩家引用（可通过 PlayerManager 获取）

    //private List<Card3D> cardsInHand = new List<Card3D>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void Start()
    {
        player = PlayerManager.Instance.TEMP_PLAYER;
    }
    void Update()
    {
        if (transform.position != position || transform.rotation != rotation)
        {
            position = transform.position;
            rotation = transform.rotation;
            RefreshHandLayout();
        }
        // 这里可以监听玩家手牌变化的事件，调用 RefreshHandLayout()
        // 例如：Player.OnHandChanged += RefreshHandLayout;
    }

    /// <summary>
    /// 抽牌：实例化卡牌并添加到手牌
    /// </summary>
    public void DrawCard(Card cardData)
    {
        //if (cardsInHand.Count >= maxHandSize)
        //{
        //    Debug.Log("手牌已满");
        //    return;
        //}

        //GameObject cardObj = Instantiate(cardPrefab, handCenter.position, Quaternion.identity);
        //Card3D card = cardObj.GetComponent<Card3D>();
        //card.Initialize(cardData);

        //cardsInHand.Add(card);
        //RefreshHandLayout();
    }

    /// <summary>
    /// 从手牌移除卡牌（打出或弃置后调用）
    /// </summary>
    //public void RemoveCard(Card3D card)
    //{
    //    if (cardsInHand.Contains(card))
    //    {
    //        cardsInHand.Remove(card);
    //        RefreshHandLayout();
    //    }
    //}

    /// <summary>
    /// 刷新手牌扇形布局
    /// </summary>
    public void RefreshHandLayout()
    {
        int count = player.currentCards.Count;
        if (count == 0) return;

        // 计算起始角度和步长
        float startAngle = -angleRange * 0.5f;
        float angleStep = count > 1 ? angleRange / (count - 1) : 0;

        for (int i = 0; i < count; i++)
        {
            Card3D card = player.currentCards[i].GetComponent<Card3D>();
            if (card.isDragging) continue; // 拖拽中的卡牌不由布局控制

            // 扇形位置：极坐标转直角坐标
            float angle = startAngle + i * angleStep;
            float rad = angle * Mathf.Deg2Rad;

            Vector3 pos = handCenter.position + new Vector3(
                Mathf.Sin(rad) * radius,
                verticalOffset,
                Mathf.Cos(rad) * radius
            );
            card.transform.position = pos;

            // 旋转：使卡牌面向圆心 + 略微上扬（可根据需求定制）
            Vector3 directionToCenter = (handCenter.position - pos).normalized;
            card.transform.rotation = Quaternion.LookRotation(directionToCenter, Vector3.up);
        }
    }

    // -------------------- 拖拽回调 --------------------
    public void OnCardDragStart(Card3D card)
    {
        // 拖拽开始：暂时从布局列表中排除，无需立即刷新布局
        // 但可以做一些额外处理，如将该卡牌的碰撞体暂时忽略其他卡牌
    }

    public void OnCardDragEnd(Card3D card, bool wasPlayed)
    {
        if (!wasPlayed)
        {
            // 未打出：卡牌已回到原位，但需确保它在布局中的位置正确
            RefreshHandLayout();
        }
    }
}