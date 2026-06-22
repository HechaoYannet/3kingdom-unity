using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 屏幕空间手牌表现控制器，负责布局、拖拽提交、响应输入和目标选择。
/// </summary>
public class BattleHandPresenter : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private BattleUIConfig config;

    [Header("Hierarchy")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform handAnchor;
    [SerializeField] private RectTransform guideRoot;
    [SerializeField] private RectTransform targetRoot;
    [SerializeField] private RectTransform targetButtonsRoot;
    [SerializeField] private RectTransform permanentHintRoot;
    [SerializeField] private TextMeshProUGUI guideText;
    [SerializeField] private TextMeshProUGUI targetTitleText;
    [SerializeField] private TextMeshProUGUI targetHintText;
    [SerializeField] private TextMeshProUGUI permanentHintText;
    [SerializeField] private Image guideLine;

    [Header("Prefabs")]
    [SerializeField] private BattleHandCardView handCardViewPrefab;
    [SerializeField] private BattleTargetButtonView targetButtonViewPrefab;

    [Header("Pooling")]
    [SerializeField] private BattleCardViewPool cardViewPool;

    private readonly Dictionary<Card, BattleHandCardView> cardViews = new Dictionary<Card, BattleHandCardView>();
    private readonly Dictionary<Player, BattleTargetButtonView> targetViews = new Dictionary<Player, BattleTargetButtonView>();

    private Player player;
    private BattleHandCardView hoveredCard;
    private BattleHandCardView selectedCard;
    private BattleHandCardView draggingCard;
    private BattleTargetButtonView hoveredTarget;
    private BattleTargetButtonView selectedTarget;
    private BattleHandCardView lastHoveredCard;
    private bool layoutDirty = true;

    private Canvas rootCanvas;
    private readonly Dictionary<Card, Rect> cardGroundRects = new Dictionary<Card, Rect>();

    /// <summary>
    /// 校验运行时所需引用，自动查找未绑定的层级节点。
    /// </summary>
    public void Initialize()
    {
        rectTransform = rectTransform != null ? rectTransform : GetComponent<RectTransform>();

        if (handAnchor == null) handAnchor = FindInParent("HandArea") ?? rectTransform;
        if (guideRoot == null) guideRoot = FindInParent("GuideRoot");
        if (targetRoot == null) targetRoot = FindInParent("TargetArea");
        if (targetButtonsRoot == null) targetButtonsRoot = FindInParent("TargetButtonsRoot");
        if (guideText == null && guideRoot != null) guideText = guideRoot.GetComponentInChildren<TextMeshProUGUI>(true);
        if (guideLine == null && guideRoot != null) guideLine = guideRoot.Find("GuideLine")?.GetComponent<Image>();
        if (targetTitleText == null && targetRoot != null) targetTitleText = FindTMPByName(targetRoot, "TargetTitleText");
        if (targetHintText == null && targetRoot != null) targetHintText = FindTMPByName(targetRoot, "TargetHintText");

        if (permanentHintRoot == null) permanentHintRoot = FindInParent("PermanentHintText");
        if (permanentHintText == null && permanentHintRoot != null)
            permanentHintText = permanentHintRoot.GetComponent<TextMeshProUGUI>();
        if (permanentHintText != null)
            permanentHintText.font = BattleUIBootstrap.DefaultFont;

        rootCanvas = GetComponentInParent<Canvas>();
    }

    private RectTransform FindInParent(string name)
    {
        Transform current = transform.parent;
        while (current != null)
        {
            Transform found = current.Find(name);
            if (found != null) return found as RectTransform ?? found.GetComponent<RectTransform>();
            current = current.parent;
        }
        return null;
    }

    private static TextMeshProUGUI FindTMPByName(RectTransform root, string name)
    {
        Transform t = root.Find(name);
        if (t != null) return t.GetComponent<TextMeshProUGUI>();
        return root.GetComponentInChildren<TextMeshProUGUI>(true);
    }

    /// <summary>
    /// 绑定一个玩家作为当前手牌来源。
    /// </summary>
    /// <param name="targetPlayer">玩家对象。</param>
    public void BindPlayer(Player targetPlayer)
    {
        if (player != null)
        {
            player.HandChanged -= HandleHandChanged;
        }

        player = targetPlayer;
        if (player != null)
        {
            player.HandChanged += HandleHandChanged;
            // 统一启用陀螺仪，避免每张卡牌各自启用
            if (SystemInfo.supportsGyroscope)
            {
                Input.gyro.enabled = true;
            }
        }

        RebuildHand();
        RebuildTargets();
        RefreshOverlayVisibility();
    }

    /// <summary>
    /// 判断一张手牌当前是否允许开始交互。
    /// </summary>
    /// <param name="cardView">手牌视图。</param>
    /// <returns>是否可交互。</returns>
    public bool CanInteractWithCard(BattleHandCardView cardView)
    {
        return CanInteract() && cardView != null && IsCardPlayable(cardView.BoundCard);
    }

    /// <summary>
    /// 判断一张手牌当前是否允许开始拖拽。
    /// </summary>
    /// <param name="cardView">手牌视图。</param>
    /// <returns>是否可拖拽。</returns>
    public bool CanBeginDragCard(BattleHandCardView cardView)
    {
        return CanInteractWithCard(cardView) && player != null && player.IsInitiativeInputActive;
    }

    /// <summary>
    /// 通知某张牌被按下。
    /// </summary>
    public void NotifyCardPressed(BattleHandCardView cardView)
    {
        if (!CanInteractWithCard(cardView))
        {
            return;
        }

        // Discard mode: click to discard
        if (player != null && player.IsRefusingInputActive)
        {
            int cardIndex = player.currentCards.IndexOf(cardView.BoundCard);
            if (cardIndex >= 0)
            {
                player.DiscardCardFromHand(cardIndex);
                hoveredCard = null;
                selectedCard = null;
                RefreshOverlayVisibility();
            }
            return;
        }

        if (player != null && player.IsResponseInputActive)
        {
            CommitCard(cardView, null);
            return;
        }

        selectedCard = selectedCard == cardView ? null : cardView;
        draggingCard = null;
        SyncTargetSelectionFromCard();
        RefreshOverlayVisibility();
        layoutDirty = true;
    }

    /// <summary>
    /// 通知拖拽状态变化。
    /// </summary>
    public void NotifyCardDragState(BattleHandCardView cardView, bool isDragging, Vector2 screenPosition)
    {
        if (isDragging && !CanBeginDragCard(cardView))
        {
            return;
        }

        draggingCard = isDragging ? cardView : null;
        selectedCard = isDragging ? cardView : selectedCard;
        if (isDragging)
        {
            SyncTargetSelectionFromCard();
            UpdateTargetHoverFromScreenPosition(screenPosition);
        }
        else
        {
            hoveredTarget = null;
            layoutDirty = true;
        }

        UpdateGuideState(isDragging, screenPosition);
        RefreshOverlayVisibility();
    }

    /// <summary>
    /// 拖拽过程中更新提示状态。
    /// </summary>
    public void NotifyCardDragged(BattleHandCardView cardView, Vector2 screenPosition)
    {
        if (draggingCard != cardView)
        {
            return;
        }

        UpdateTargetHoverFromScreenPosition(screenPosition);
        UpdateGuideState(true, screenPosition);
    }

    /// <summary>
    /// 尝试提交拖拽出牌。
    /// </summary>
    public void TryCommitDraggedCard(BattleHandCardView cardView, Vector2 screenPosition)
    {
        UpdateGuideState(false, screenPosition);

        if (!CanBeginDragCard(cardView))
        {
            RebuildHand();
            RefreshOverlayVisibility();
            return;
        }

        if (screenPosition.y < GetCommitScreenHeight())
        {
            layoutDirty = true;
            RefreshOverlayVisibility();
            return;
        }

        Player target = ResolveCommitTarget(cardView);
        CommitCard(cardView, target);
    }

    /// <summary>
    /// 通知目标按钮被悬停或离开。
    /// </summary>
    public void NotifyTargetHover(BattleTargetButtonView targetView, bool isHovered)
    {
        hoveredTarget = isHovered ? targetView : hoveredTarget == targetView ? null : hoveredTarget;
    }

    /// <summary>
    /// 通知目标按钮被点击。
    /// </summary>
    public void NotifyTargetClicked(BattleTargetButtonView targetView)
    {
        if (!CanInteract() || selectedCard == null || targetView == null || targetView.BoundPlayer == null)
        {
            return;
        }

        selectedTarget = targetView;
        CommitCard(selectedCard, targetView.BoundPlayer);
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.HandChanged -= HandleHandChanged;
        }
    }

    private void LateUpdate()
    {
        if (player == null) return;

        // 拖拽过程中每帧更新位置
        if (draggingCard != null)
        {
            LayoutCards();
            LayoutTargets();
            layoutDirty = false;
            return;
        }

        // 基于地面矩形的指针轮询检测，不依赖会移动的视觉矩形
        UpdateHoverFromPointer();

        // 脏标记模式：仅在需要时重新布局
        if (layoutDirty)
        {
            LayoutCards();
            LayoutTargets();
            RefreshOverlayVisibility();
            layoutDirty = false;
        }
        else
        {
            // 非脏帧仍需更新提示文字（阶段/状态可能变化）
            UpdatePermanentHint();
        }
    }

    private void HandleHandChanged(Player changedPlayer)
    {
        if (changedPlayer != player)
        {
            return;
        }

        DiffUpdateHand();
        layoutDirty = true;
    }

    /// <summary>
    /// 基于地面矩形（rest position）轮询检测指针悬停，不依赖会移动的视觉矩形。
    /// 地面矩形在 LayoutCards 中缓存，每帧仅做 Contains 检测，开销极低。
    /// </summary>
    private void UpdateHoverFromPointer()
    {
        if (draggingCard != null || player == null || handAnchor == null || cardGroundRects.Count == 0)
            return;

        if (rootCanvas == null)
            rootCanvas = GetComponentInParent<Canvas>();
        Camera cam = rootCanvas != null && rootCanvas.renderMode != RenderMode.ScreenSpaceOverlay
            ? rootCanvas.worldCamera : null;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            handAnchor, Input.mousePosition, cam, out Vector2 localPoint))
            return;

        BattleHandCardView newHovered = null;
        foreach (KeyValuePair<Card, Rect> pair in cardGroundRects)
        {
            if (!cardViews.TryGetValue(pair.Key, out BattleHandCardView view) || view == null)
                continue;
            if (view.IsDragging)
                continue;

            if (pair.Value.Contains(localPoint))
            {
                newHovered = view;
                break;
            }
        }

        if (newHovered != hoveredCard)
        {
            if (newHovered != null && AudioManage.Instance != null)
                AudioManage.Instance.PlayBattleSfx(AudioManage.BattleSfx.CardHover);
            hoveredCard = newHovered;
            lastHoveredCard = null;
            layoutDirty = true;
        }
    }

    /// <summary>
    /// 差量更新手牌视图：保留仍在手中的卡牌视图，移除已不在手中的，新增新摸到的。
    /// </summary>
    private void DiffUpdateHand()
    {
        if (player == null || handAnchor == null || handCardViewPrefab == null)
        {
            return;
        }

        // 找出已不在手中的卡牌视图
        var toRemove = new List<Card>();
        foreach (var pair in cardViews)
        {
            if (!player.currentCards.Contains(pair.Key))
            {
                toRemove.Add(pair.Key);
            }
        }

        foreach (Card removed in toRemove)
        {
            if (cardViews.TryGetValue(removed, out BattleHandCardView view))
            {
                if (cardViewPool != null)
                {
                    cardViewPool.Return(view);
                }
                else
                {
                    Destroy(view.gameObject);
                }
                cardViews.Remove(removed);
                cardGroundRects.Remove(removed);
            }
        }

        if (hoveredCard != null && (!cardViews.ContainsValue(hoveredCard) || !hoveredCard.gameObject.activeInHierarchy))
        {
            hoveredCard = null;
        }
        if (selectedCard != null && (!cardViews.ContainsValue(selectedCard) || !selectedCard.gameObject.activeInHierarchy))
        {
            selectedCard = null;
        }

        // 若拖拽中的卡牌视图已不在手中，清理引用防止 LateUpdate 持续操作回池对象
        if (draggingCard != null && (!cardViews.ContainsValue(draggingCard) || !draggingCard.gameObject.activeInHierarchy))
        {
            draggingCard = null;
        }

        // 新增新摸到的卡牌
        foreach (Card card in player.currentCards)
        {
            if (!cardViews.ContainsKey(card))
            {
                BattleHandCardView view;
                if (cardViewPool != null)
                {
                    view = cardViewPool.Get(handAnchor);
                }
                else
                {
                    view = BattleHandCardView.Create(handCardViewPrefab, handAnchor, this, card);
                }

                if (cardViewPool != null)
                {
                    // 池化路径需要手动初始化
                    view.name = $"HandCard_{card.cardName}";
                    view.SetPresenter(this);
                    view.Bind(card);
                    view.gameObject.SetActive(true);
                }
                cardViews[card] = view;
            }
        }
    }

    private void RebuildHand()
    {
        foreach (BattleHandCardView view in cardViews.Values)
        {
            if (view != null)
            {
                if (cardViewPool != null)
                {
                    cardViewPool.Return(view);
                }
                else
                {
                    Destroy(view.gameObject);
                }
            }
        }

        cardViews.Clear();
        cardGroundRects.Clear();
        hoveredCard = null;
        selectedCard = null;
        draggingCard = null;

        if (player == null || handAnchor == null || handCardViewPrefab == null)
        {
            return;
        }

        // 初始化池（首次）
        if (cardViewPool == null && handCardViewPrefab != null)
        {
            GameObject poolObj = new GameObject("CardViewPool");
            poolObj.transform.SetParent(transform, false);
            cardViewPool = poolObj.AddComponent<BattleCardViewPool>();
            cardViewPool.Initialize(handCardViewPrefab, 8);
        }

        foreach (Card card in player.currentCards)
        {
            BattleHandCardView view = cardViewPool != null
                ? cardViewPool.Get(handAnchor)
                : BattleHandCardView.Create(handCardViewPrefab, handAnchor, this, card);

            if (view != null)
            {
                if (cardViewPool != null)
                {
                    view.name = $"HandCard_{card.cardName}";
                    view.SetPresenter(this);
                    view.Bind(card);
                    view.gameObject.SetActive(true);
                }
                cardViews[card] = view;
            }
        }
    }

    private void RebuildTargets()
    {
        foreach (BattleTargetButtonView view in targetViews.Values)
        {
            if (view != null)
            {
                Destroy(view.gameObject);
            }
        }

        targetViews.Clear();
        hoveredTarget = null;
        selectedTarget = null;

        if (player == null || PlayerManager.Instance == null || targetButtonsRoot == null || targetButtonViewPrefab == null)
        {
            return;
        }

        foreach (Player opponent in GetAvailableTargets())
        {
            BattleTargetButtonView view = BattleTargetButtonView.Create(targetButtonViewPrefab, targetButtonsRoot, this);
            if (view != null)
            {
                view.Bind(opponent);
                targetViews[opponent] = view;
            }
        }
    }

    private void LayoutCards()
    {
        int cardCount = player.currentCards.Count;
        if (cardCount <= 0)
        {
            if (guideRoot != null)
            {
                guideRoot.gameObject.SetActive(false);
            }

            return;
        }

        // 自适应间距：基于配置最大宽度而非硬编码系数
        float canvasWidth = rectTransform != null ? rectTransform.rect.width : 1920f;
        float maxWidth = config != null ? config.maxSpacingWidth : canvasWidth * 0.85f;
        float baseSpacing = config != null ? config.baseCardSpacing : 165f;
        float spacing = Mathf.Min(baseSpacing, maxWidth / Mathf.Max(cardCount - 1, 1));
        float width = spacing * Mathf.Max(cardCount - 1, 0);

        // 预计算 hover 索引和推开参数，避免 O(n²)
        int hoverIndex = -1;
        float pushAmount = config != null ? config.pushAmount : 72f;
        float pushDecay = config != null ? config.pushDecayFactor : 0.35f;
        if (hoveredCard != null)
        {
            for (int j = 0; j < cardCount; j++)
            {
                Card hc = player.currentCards[j];
                if (cardViews.TryGetValue(hc, out BattleHandCardView hv) && hv == hoveredCard)
                {
                    hoverIndex = j;
                    break;
                }
            }
        }

        float hoverLift = config != null ? config.hoverLift : 88f;
        float hoverScale = config != null ? config.hoverScale : 1.08f;
        float selectedScale = config != null ? config.selectedScale : 1.04f;
        float cardW = config != null ? config.cardWidth : 220f;
        float cardH = config != null ? config.cardHeight : 300f;

        for (int i = 0; i < cardCount; i++)
        {
            Card card = player.currentCards[i];
            if (!cardViews.TryGetValue(card, out BattleHandCardView view) || view == null)
            {
                continue;
            }

            // 使用 presenter 统一的可打判定（覆盖弃牌阶段等边缘情况）
            bool isPlayable = IsCardPlayable(card);
            bool allowDrag = isPlayable && player.IsInitiativeInputActive;
            view.SetInteractionState(isPlayable, allowDrag, player.IsResponseInputActive);

            if (view.IsDragging)
            {
                view.transform.SetAsLastSibling();
                continue;
            }

            float normalized = cardCount <= 1 ? 0f : (i / (float)(cardCount - 1)) * 2f - 1f;
            float baseX = -width * 0.5f + spacing * i;
            float yOffset = config != null ? config.baseYOffset : -36f;
            float arcDrop = config != null ? config.arcDropFactor : 24f;
            float baseY = yOffset - Mathf.Abs(normalized) * arcDrop;
            float fanAngle = config != null ? config.fanAngleRange : 16f;
            float rotation = -normalized * fanAngle;
            float scale = 1f;
            bool dimmed = !isPlayable;

            // 缓存地面矩形，供 presenter 轮询检测使用
            Vector2 groundPos = new Vector2(baseX, baseY);
            cardGroundRects[card] = new Rect(
                groundPos.x - cardW * 0.5f, groundPos.y - cardH * 0.5f,
                cardW, cardH);

            // 推开偏移：悬停卡两侧的卡牌向外推开，距离随索引差衰减
            float pushOffset = 0f;
            if (hoverIndex >= 0 && view != hoveredCard && view != selectedCard)
            {
                int distance = Mathf.Abs(i - hoverIndex);
                if (distance > 0)
                {
                    float decay = Mathf.Pow(pushDecay, distance - 1);
                    float direction = (i < hoverIndex) ? -1f : 1f;
                    pushOffset = direction * pushAmount * decay;
                }
            }

            float x = baseX + pushOffset;
            float y = baseY;

            if (view == hoveredCard && isPlayable)
            {
                y += hoverLift;
                scale = hoverScale;
                rotation = 0f;
                dimmed = false;
                // 仅在 hover 目标首次变化时重新排序 sibling，避免每 frame 触发 sibling 重排
                if (lastHoveredCard != hoveredCard)
                {
                    view.transform.SetAsLastSibling();
                    lastHoveredCard = hoveredCard;
                }
            }
            else if (view == selectedCard && isPlayable)
            {
                y += hoverLift * 0.6f;
                scale = selectedScale;
                rotation *= 0.35f;
                dimmed = false;
                view.transform.SetAsLastSibling();
            }
            else if ((hoveredCard != null || selectedCard != null) && isPlayable)
            {
                dimmed = true;
            }

            view.SetHovered(view == hoveredCard && isPlayable);
            view.UpdatePresentation(new Vector2(x, y), rotation, scale, dimmed);
        }

        UpdatePermanentHint();
    }

    private void UpdatePermanentHint()
    {
        if (permanentHintText == null || player == null) return;

        Color discardColor = config != null ? config.discardHintColor : new Color(1f, 0.6f, 0.3f, 0.9f);
        Color responseColor = config != null ? config.responseHintColor : new Color(1f, 0.35f, 0.35f, 0.9f);
        Color normalColor = config != null ? config.normalHintColor : new Color(0.88f, 0.82f, 0.64f, 0.72f);
        Color waitingColor = config != null ? config.waitingHintColor : new Color(0.5f, 0.5f, 0.5f, 0.5f);

        if (player.IsRefusingInputActive)
        {
            int excess = player.currentCards.Count - player.GetMaxHP();
            permanentHintText.text = excess > 0
                ? $"弃牌阶段 — 点击手牌弃置（需弃 {excess} 张）"
                : "弃牌阶段";
            permanentHintText.color = discardColor;
        }
        else if (player.IsResponseInputActive)
        {
            permanentHintText.text = "⚠ 响应阶段 — 点击闪/防御牌来响应";
            permanentHintText.color = responseColor;
        }
        else if (CanInteract())
        {
            permanentHintText.text = "选择一张牌出牌  ·  或向上拖拽";
            permanentHintText.color = normalColor;
        }
        else
        {
            permanentHintText.text = "等待中…";
            permanentHintText.color = waitingColor;
        }
    }

    private void LayoutTargets()
    {
        EnsureTargetViews();

        List<Player> targets = GetAvailableTargets();
        bool showTargets = ShouldShowTargetSelection();
        int count = targets.Count;

        for (int i = 0; i < count; i++)
        {
            Player target = targets[i];
            if (!targetViews.TryGetValue(target, out BattleTargetButtonView view) || view == null)
            {
                continue;
            }

            float baseTargetSpacing = config != null ? config.targetButtonSpacing : 248f;
            float containerWidth = targetButtonsRoot != null ? targetButtonsRoot.rect.width : 860f;
            float targetSpacing = Mathf.Min(baseTargetSpacing, containerWidth / Mathf.Max(count, 1));
            float x = count <= 1 ? 0f : (i - (count - 1) * 0.5f) * targetSpacing;
            bool isSelected = selectedTarget != null && selectedTarget.BoundPlayer == target;
            bool isHovered = hoveredTarget != null && hoveredTarget.BoundPlayer == target;
            view.UpdatePresentation(new Vector2(x, 0f), isSelected, isHovered, showTargets && target.IsAlive);
        }

        foreach (KeyValuePair<Player, BattleTargetButtonView> pair in targetViews)
        {
            if (targets.Contains(pair.Key) || pair.Value == null)
            {
                continue;
            }

            pair.Value.UpdatePresentation(pair.Value.RectTransform.anchoredPosition, false, false, false);
        }

        if (targetTitleText != null)
        {
            targetTitleText.text = player != null && player.IsResponseInputActive ? "响应阶段" : "选择目标";
        }

        if (targetHintText != null)
        {
            if (player != null && player.IsResponseInputActive)
            {
                targetHintText.text = "点击可响应手牌";
            }
            else
            {
                targetHintText.text = draggingCard != null ? "拖到阈值线以上，目标可在此切换" : "点一张牌后，再点击目标出牌";
            }
        }
    }

    private void EnsureTargetViews()
    {
        if (player == null || targetButtonsRoot == null || targetButtonViewPrefab == null)
        {
            return;
        }

        foreach (Player target in GetAvailableTargets())
        {
            if (targetViews.ContainsKey(target))
            {
                continue;
            }

            BattleTargetButtonView view = BattleTargetButtonView.Create(targetButtonViewPrefab, targetButtonsRoot, this);
            if (view != null)
            {
                view.Bind(target);
                targetViews[target] = view;
            }
        }
    }

    private void CommitCard(BattleHandCardView cardView, Player explicitTarget)
    {
        if (!CanInteractWithCard(cardView) || player == null)
        {
            RebuildHand();
            RefreshOverlayVisibility();
            return;
        }

        int cardIndex = player.currentCards.IndexOf(cardView.BoundCard);
        if (cardIndex < 0)
        {
            RebuildHand();
            RefreshOverlayVisibility();
            return;
        }

        player.currSelectedCardID = cardIndex;

        Player resolvedTarget = null;
        if (CardRequiresTarget(cardView.BoundCard))
        {
            resolvedTarget = explicitTarget != null && explicitTarget.IsAlive
                ? explicitTarget
                : ResolveCommitTarget(cardView);
        }

        if (resolvedTarget != null)
        {
            player.TargetPlayerEnemyID = resolvedTarget.PlayerID;
        }

        // 仅锁定卡牌视图（不可交互），由 DiffUpdateHand 在 UseCard 后自然回收
        // 避免 RetireCommittedCardView 提前回收导致 UseCard 失败时视图永久丢失
        cardView.SetInteractionState(false, false, false);
        player.Confirm_ButtonClick();

        if (AudioManage.Instance != null)
            AudioManage.Instance.PlayBattleSfx(AudioManage.BattleSfx.CardPlay);

        selectedCard = null;
        hoveredCard = null;
        draggingCard = null;
        hoveredTarget = null;
        RefreshSelectedTargetFromPlayer();
        RefreshOverlayVisibility();
    }

    private void SyncTargetSelectionFromCard()
    {
        if (!ShouldShowTargetSelection())
        {
            hoveredTarget = null;
            selectedTarget = null;
            return;
        }

        RefreshSelectedTargetFromPlayer();
    }

    private void RefreshSelectedTargetFromPlayer()
    {
        selectedTarget = null;
        if (PlayerManager.Instance == null)
        {
            return;
        }

        Player explicitTarget = PlayerManager.Instance.GetPlayerInstanceByID(player != null ? player.TargetPlayerEnemyID : 0);
        if (explicitTarget != null && targetViews.TryGetValue(explicitTarget, out BattleTargetButtonView selectedView))
        {
            selectedTarget = selectedView;
            return;
        }

        Player fallback = player != null ? PlayerManager.Instance.GetFirstLivingOpponent(player) : null;
        if (fallback != null && targetViews.TryGetValue(fallback, out BattleTargetButtonView fallbackView))
        {
            selectedTarget = fallbackView;
        }
    }

    private void UpdateTargetHoverFromScreenPosition(Vector2 screenPosition)
    {
        if (!ShouldShowTargetSelection())
        {
            hoveredTarget = null;
            return;
        }

        hoveredTarget = null;
        foreach (BattleTargetButtonView targetView in targetViews.Values)
        {
            if (targetView == null || !targetView.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (RectTransformUtility.RectangleContainsScreenPoint(targetView.RectTransform, screenPosition, null))
            {
                hoveredTarget = targetView;
                break;
            }
        }
    }

    private Player ResolveCommitTarget(BattleHandCardView cardView)
    {
        if (!CardRequiresTarget(cardView != null ? cardView.BoundCard : null))
        {
            return null;
        }

        if (hoveredTarget != null && hoveredTarget.BoundPlayer != null && hoveredTarget.BoundPlayer.IsAlive)
        {
            selectedTarget = hoveredTarget;
            return hoveredTarget.BoundPlayer;
        }

        if (selectedTarget != null && selectedTarget.BoundPlayer != null && selectedTarget.BoundPlayer.IsAlive)
        {
            return selectedTarget.BoundPlayer;
        }

        return PlayerManager.Instance != null ? PlayerManager.Instance.GetFirstLivingOpponent(player) : null;
    }

    private bool CardRequiresTarget(Card card)
    {
        return card != null && card.IsCardResponsible();
    }

    private bool IsCardPlayable(Card card)
    {
        if (!CanInteract() || card == null || player == null)
        {
            return false;
        }

        // During discard phase, all hand cards are playable (click to discard)
        if (player.IsRefusingInputActive)
            return true;

        int cardIndex = player.currentCards.IndexOf(card);
        return cardIndex >= 0 && player.CanPlayCardAt(cardIndex);
    }

    private bool ShouldShowTargetSelection()
    {
        if (!CanInteract() || player == null || player.IsResponseInputActive)
        {
            return false;
        }

        BattleHandCardView activeCard = draggingCard != null ? draggingCard : selectedCard;
        return activeCard != null && CardRequiresTarget(activeCard.BoundCard) && GetAvailableTargets().Count > 0;
    }

    private bool CanInteract()
    {
        return player != null &&
               player.CanReceiveManualInput &&
               RoundManager.instance != null &&
               (RoundManager.instance.RoundState == GRoundState.Battling ||
                RoundManager.instance.RoundState == GRoundState.RefusingCard);
    }

    private List<Player> GetAvailableTargets()
    {
        List<Player> result = new List<Player>();
        if (player == null || PlayerManager.Instance == null)
        {
            return result;
        }

        foreach (Player candidate in PlayerManager.Instance.players)
        {
            if (candidate == null || candidate == player || !candidate.IsAlive)
            {
                continue;
            }

            result.Add(candidate);
        }

        return result;
    }

    private void UpdateGuideState(bool isDragging, Vector2 screenPosition)
    {
        if (guideRoot == null)
        {
            return;
        }

        guideRoot.gameObject.SetActive(isDragging && player != null && player.IsInitiativeInputActive);
        if (!guideRoot.gameObject.activeSelf)
        {
            return;
        }

        bool canCommit = screenPosition.y >= GetCommitScreenHeight();
        Color canCommitColor = config != null ? config.guideLineCanCommitColor : new Color(0.98f, 0.88f, 0.32f, 0.72f);
        Color defaultColor = config != null ? config.guideLineDefaultColor : new Color(0.96f, 0.78f, 0.28f, 0.16f);
        if (guideText != null)
        {
            guideText.text = canCommit ? "松手即可出牌" : "继续向上拖拽";
        }

        if (guideLine != null)
        {
            guideLine.color = canCommit ? canCommitColor : defaultColor;
        }
    }

    private void RefreshOverlayVisibility()
    {
        if (targetRoot != null)
        {
            targetRoot.gameObject.SetActive(true);
        }

        if (guideRoot != null && draggingCard == null)
        {
            guideRoot.gameObject.SetActive(false);
        }
    }

    private float GetCommitScreenHeight()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        float canvasHeight = canvas != null && canvas.rootCanvas != null
            ? ((RectTransform)canvas.rootCanvas.transform).rect.height
            : Screen.height;
        float ratio = config != null ? config.commitHeightRatio : 0.5f;
        float threshold = config != null ? config.commitThreshold : 220f;
        return canvasHeight * ratio + threshold;
    }

    private void RetireCommittedCardView(BattleHandCardView cardView)
    {
        if (cardView == null || cardView.BoundCard == null)
        {
            return;
        }

        cardViews.Remove(cardView.BoundCard);
        if (cardViewPool != null)
        {
            cardViewPool.Return(cardView);
        }
        else
        {
            Destroy(cardView.gameObject);
        }
    }
}
