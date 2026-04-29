using UnityEngine;

/// <summary>
/// 3D 卡牌核心逻辑，负责拖拽和出牌区投放。
/// </summary>
[RequireComponent(typeof(Collider))]
public class Card3D : MonoBehaviour
{
    [Header("卡牌数据")]
    public Card cardData;

    [Header("拖拽设置")]
    public float dragHeightOffset = 1.0f;
    public float dragScaleMultiplier = 1.2f;
    public LayerMask playZoneLayer;

    [HideInInspector] public bool isDragging;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;
    private float originalY;
    private HandManager3D handManager;

    public event System.Action OnCardDataChanged;

    private void Awake()
    {
        cardData = GetComponent<Card>();
        handManager = FindObjectOfType<HandManager3D>();
        originalScale = transform.localScale;
    }

    private void Start()
    {
        GetComponent<CardView3D>()?.RefreshUI();
    }

    /// <summary>
    /// 初始化卡牌展示数据。
    /// </summary>
    /// <param name="card">逻辑卡牌对象。</param>
    public void Initialize(Card card)
    {
        card.transform.SetParent(transform, false);
        cardData = card;
        OnCardDataChanged?.Invoke();
    }

    private void OnMouseDown()
    {
        if (!CanDrag())
        {
            return;
        }

        isDragging = true;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalY = transform.position.y;

        transform.position += Vector3.up * dragHeightOffset;
        transform.localScale = originalScale * dragScaleMultiplier;
        handManager?.OnCardDragStart(this);
    }

    private void OnMouseDrag()
    {
        if (!isDragging)
        {
            return;
        }

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.WorldToScreenPoint(originalPosition).z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.y = originalY + dragHeightOffset;
        transform.position = worldPos;
    }

    private void OnMouseUp()
    {
        if (!isDragging)
        {
            return;
        }

        isDragging = false;
        transform.localScale = originalScale;
        transform.rotation = originalRotation;

        bool played = IsOverPlayZone(out PlayZone3D playZone);
        if (played && CanPlayCard() && TryQueuePlayerCardPlay(playZone))
        {
            handManager?.OnCardDragEnd(this, true);
            return;
        }

        transform.position = originalPosition;
        handManager?.OnCardDragEnd(this, false);
    }

    private bool CanDrag()
    {
        Player owner = PlayerManager.Instance != null ? PlayerManager.Instance.GetPlayerInstanceByID(cardData != null ? cardData.OwnerID : -1) : null;
        return owner != null &&
               owner.UsesHumanInput &&
               RoundManager.instance != null &&
               RoundManager.instance.CurrentTurnPlayer == owner &&
               RoundManager.instance.RoundState == GRoundState.Battling;
    }

    private bool CanPlayCard()
    {
        return cardData != null;
    }

    private bool IsOverPlayZone(out PlayZone3D playZone)
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f, playZoneLayer))
        {
            playZone = hit.collider.GetComponent<PlayZone3D>();
            return playZone != null;
        }

        playZone = null;
        return false;
    }

    /// <summary>
    /// 刷新卡牌展示数据。
    /// </summary>
    /// <param name="newAttack">保留的外部刷新接口。</param>
    public void SetAttack(int newAttack)
    {
        OnCardDataChanged?.Invoke();
    }

    private bool TryQueuePlayerCardPlay(PlayZone3D playZone)
    {
        if (cardData == null || PlayerManager.Instance == null)
        {
            return false;
        }

        Player owner = PlayerManager.Instance.GetPlayerInstanceByID(cardData.OwnerID);
        if (owner == null || !owner.UsesHumanInput)
        {
            return false;
        }

        int cardIndex = owner.currentCards.IndexOf(cardData);
        if (cardIndex < 0)
        {
            return false;
        }

        owner.currSelectedCardID = cardIndex;
        owner.TargetPlayerEnemyID = playZone != null && playZone.ZoneID != 0
            ? playZone.ZoneID
            : PlayerManager.Instance.GetFirstLivingOpponent(owner)?.PlayerID ?? 0;
        owner.Confirm_ButtonClick();
        return true;
    }
}
