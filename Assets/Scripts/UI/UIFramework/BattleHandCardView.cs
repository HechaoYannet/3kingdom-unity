using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using ReEndUnity;

/// <summary>
/// 单张屏幕空间手牌的交互与分层表现。
/// </summary>
public class BattleHandCardView : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Config")]
    [SerializeField] private BattleUIConfig config;

    [Header("Runtime References")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform backLayerRect;
    [SerializeField] private RectTransform artLayerRect;
    [SerializeField] private RectTransform frontLayerRect;
    [SerializeField] private Image backLayerImage;
    [SerializeField] private Image artLayerImage;
    [SerializeField] private Image frontLayerImage;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI bodyText;
    [SerializeField] private TextMeshProUGUI dragHintText;

    private BattleHandPresenter presenter;
    private Card card;
    private bool isHovered;
    private bool isDragging;
    private bool isInteractable = true;
    private bool dragEnabled = true;
    private bool gyroEnabled;
    private Vector2 localPointerNormalized;
    private string lastDragHintText;
    private ReEndTheme reEndTheme;


    public Card BoundCard => card;
    public bool IsDragging => isDragging;
    public bool IsHovered => isHovered;
    public RectTransform RectTransform => rectTransform;
    public Vector2 DragScreenPosition { get; private set; }

    /// <summary>
    /// 对象池复用前重置交互状态与变换，防止残留状态跨生命周期污染。
    /// </summary>
    public void ResetForPool()
    {
        isDragging = false;
        isHovered = false;
        isInteractable = true;
        dragEnabled = true;
        localPointerNormalized = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localScale = Vector3.one;
        if (canvasGroup != null)
        {
            // 离场动画会把 alpha 渐隐到 0，回池必须还原，否则复用后卡牌不可见
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
    }

    /// <summary>
    /// 通过 prefab 创建并绑定一张手牌视图。
    /// </summary>
    /// <param name="prefab">手牌视图 prefab。</param>
    /// <param name="parent">父节点。</param>
    /// <param name="owner">所属 presenter。</param>
    /// <param name="boundCard">绑定的逻辑卡牌。</param>
    /// <returns>创建好的视图组件。</returns>
    public static BattleHandCardView Create(BattleHandCardView prefab, Transform parent, BattleHandPresenter owner, Card boundCard)
    {
        if (prefab == null)
        {
            Debug.LogError("BattleHandCardView prefab is not assigned.");
            return null;
        }

        BattleHandCardView view = Instantiate(prefab, parent);
        view.name = $"HandCard_{boundCard.cardName}";
        view.presenter = owner;
        view.EnsureRuntimeReferences();
        view.Bind(boundCard);
        view.gameObject.SetActive(true);
        return view;
    }

    /// <summary>
    /// 绑定逻辑卡牌。
    /// </summary>
    /// <param name="boundCard">逻辑卡牌。</param>
    public void Bind(Card boundCard)
    {
        card = boundCard;
        RefreshVisuals();
    }

    /// <summary>
    /// 设置 presenter 引用（池化路径使用）。
    /// </summary>
    public void SetPresenter(BattleHandPresenter owner)
    {
        presenter = owner;
    }

    /// <summary>
    /// 由 presenter 通过地面矩形轮询检测设置悬停状态。
    /// </summary>
    public void SetHovered(bool value)
    {
        isHovered = value;
    }

    /// <summary>
    /// 由 presenter 更新目标插值位置。
    /// </summary>
    /// <param name="anchoredPosition">目标位置。</param>
    /// <param name="rotationZ">目标角度。</param>
    /// <param name="scale">目标缩放。</param>
    /// <param name="dimmed">是否压暗。</param>
    public void UpdatePresentation(Vector2 anchoredPosition, float rotationZ, float scale, bool dimmed)
    {
        float posSpeed = config != null ? config.positionLerpSpeed : 16f;
        float rotSpeed = config != null ? config.rotationLerpSpeed : 18f;
        float alphaSpeed = config != null ? config.alphaLerpSpeed : 18f;
        float dimmedA = config != null ? config.dimmedAlpha : 0.55f;
        float nonInteractA = config != null ? config.nonInteractableAlpha : 0.34f;
        float configDuration = config != null ? config.animationDuration : 0f;
        float animDuration = configDuration > 0f ? configDuration : 1f / Mathf.Max(posSpeed, 1f);

        // 从配置读取缓动曲线；OutQuart=快速响应+均匀减速，OutBack=弹性收尾
        Ease posEase = config != null ? config.positionEase : Ease.OutQuart;
        Ease rotEase = config != null ? config.rotationEase : Ease.OutQuart;
        Ease scaEase = config != null ? config.scaleEase : Ease.OutBack;
        Ease fadEase = config != null ? config.fadeEase : Ease.OutQuart;
        float rotDuration = configDuration > 0f ? configDuration : 1f / Mathf.Max(rotSpeed, 1f);
        float alphaDuration = configDuration > 0f ? configDuration : 1f / Mathf.Max(alphaSpeed, 1f);

        // 每帧调用前杀死残留 Tween，防止重叠动画导致属性振荡
        rectTransform.DOKill();
        canvasGroup.DOKill();

        if (!isDragging)
        {
            rectTransform.DOAnchorPos(anchoredPosition, animDuration).SetEase(posEase).SetUpdate(true);
            rectTransform.DOLocalRotate(new Vector3(0f, 0f, rotationZ), rotDuration).SetEase(rotEase).SetUpdate(true);
        }

        Vector3 targetScale = Vector3.one * scale;
        rectTransform.DOScale(targetScale, animDuration).SetEase(scaEase).SetUpdate(true);
        float targetAlpha = dimmed ? dimmedA : isInteractable ? 1f : nonInteractA;
        canvasGroup.DOFade(targetAlpha, alphaDuration).SetEase(fadEase).SetUpdate(true);
    }

    /// <summary>
    /// 出牌/弃牌离场动画：上浮 + 淡出 + 微放大，完成后回调回收视图。
    /// </summary>
    /// <param name="onComplete">动画完成回调（回池或销毁）。</param>
    public void PlayExitAnimation(System.Action onComplete)
    {
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
        }

        isInteractable = false;
        isHovered = false;
        isDragging = false;

        rectTransform.DOKill();
        if (canvasGroup != null)
        {
            canvasGroup.DOKill();
        }

        rectTransform.SetAsLastSibling();
        const float exitDuration = 0.22f;
        rectTransform.DOAnchorPos(rectTransform.anchoredPosition + new Vector2(0f, 130f), exitDuration)
            .SetEase(Ease.OutQuad).SetUpdate(true);
        rectTransform.DOScale(Vector3.one * 1.12f, exitDuration)
            .SetEase(Ease.OutQuad).SetUpdate(true);
        if (canvasGroup != null)
        {
            canvasGroup.DOFade(0f, exitDuration)
                .SetEase(Ease.InQuad).SetUpdate(true)
                .OnComplete(() => onComplete?.Invoke());
        }
        else
        {
            onComplete?.Invoke();
        }
    }

    /// <summary>
    /// 摸牌入场预置：从手牌区底部飞入扇形位，由下一次布局动画过渡到目标位置。
    /// </summary>
    public void PrepareEntryAnimation()
    {
        rectTransform.DOKill();
        if (canvasGroup != null)
        {
            canvasGroup.DOKill();
        }

        rectTransform.anchoredPosition = new Vector2(0f, -160f);
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localScale = new Vector3(0.92f, 0.92f, 1f);
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0.4f;
        }
    }

    /// <summary>
    /// 更新当前卡牌的交互模式提示。
    /// </summary>
    /// <param name="canInteract">是否允许交互。</param>
    /// <param name="allowDrag">是否允许拖拽。</param>
    /// <param name="isResponseMode">是否处于响应模式。</param>
    public void SetInteractionState(bool canInteract, bool allowDrag, bool isResponseMode)
    {
        isInteractable = canInteract;
        dragEnabled = allowDrag;

        if (dragHintText == null)
        {
            return;
        }

        string hint;
        if (!canInteract)
        {
            hint = "当前不可用";
        }
        else if (isResponseMode)
        {
            hint = "点击打出响应牌";
        }
        else if (allowDrag)
        {
            hint = "向上拖拽出牌";
        }
        else
        {
            hint = "点击选择目标";
        }

        // 去重：布局可能随脏标记频繁执行，避免重复设置相同文本触发 TMP 重建
        if (hint != lastDragHintText)
        {
            dragHintText.text = hint;
            lastDragHintText = hint;
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        UpdatePointerParallax(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isInteractable)
        {
            return;
        }

        presenter.NotifyCardPressed(this);
        UpdatePointerParallax(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isInteractable || !dragEnabled || !presenter.CanBeginDragCard(this))
        {
            return;
        }

        isDragging = true;
        canvasGroup.blocksRaycasts = false;

        // 杀死 UpdatePresentation 创建的残留位置/旋转/缩放动画，防止与拖拽位置赋值冲突
        rectTransform.DOKill();
        canvasGroup.DOKill();

        // 拖拽"拿起"反馈：摆正旋转并轻微放大，与位置直接赋值不冲突
        rectTransform.DOLocalRotate(Vector3.zero, 0.12f).SetEase(Ease.OutQuad).SetUpdate(true);
        rectTransform.DOScale(Vector3.one * 1.08f, 0.12f).SetEase(Ease.OutQuad).SetUpdate(true);

        presenter.NotifyCardDragState(this, true, eventData.position);
        UpdateDragPosition(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging)
        {
            return;
        }

        UpdatePointerParallax(eventData);
        UpdateDragPosition(eventData.position);
        presenter.NotifyCardDragged(this, eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging)
        {
            return;
        }

        canvasGroup.blocksRaycasts = true;
        isDragging = false;
        localPointerNormalized = Vector2.zero;

        presenter.NotifyCardDragState(this, false, eventData.position);
        presenter.TryCommitDraggedCard(this, eventData.position);
    }

    private void Awake()
    {
        reEndTheme = ReEndThemeManager.Current;
        EnsureRuntimeReferences();
        gyroEnabled = SystemInfo.supportsGyroscope;
        // 陀螺仪由 BattleHandPresenter 统一启用，此处不再重复设置
    }

    private void LateUpdate()
    {
        UpdateLayerParallax();
    }

    private void EnsureRuntimeReferences()
    {
        float cardW = config != null ? config.cardWidth : 220f;
        float cardH = config != null ? config.cardHeight : 300f;
        float frameA = config != null ? config.frameAlpha : 0.001f;
        Color cardBg = config != null ? config.cardBackgroundColor : new Color(0.08f, 0.09f, 0.13f, 0.96f);

        rectTransform = rectTransform != null ? rectTransform : GetComponent<RectTransform>();
        canvasGroup = canvasGroup != null ? canvasGroup : GetComponent<CanvasGroup>();

        if (rectTransform == null)
        {
            rectTransform = gameObject.AddComponent<RectTransform>();
        }

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        Image frame = GetComponent<Image>();
        if (frame == null)
        {
            frame = gameObject.AddComponent<Image>();
        }

        rectTransform.sizeDelta = new Vector2(cardW, cardH);
        frame.color = new Color(1f, 1f, 1f, frameA);

        if (backLayerRect == null || artLayerRect == null || frontLayerRect == null ||
            backLayerImage == null || artLayerImage == null || frontLayerImage == null ||
            titleText == null || bodyText == null || dragHintText == null)
        {
            BuildVisualTree(frame, cardW, cardH, cardBg);
        }
    }

    private void BuildVisualTree(Image frame, float cardW, float cardH, Color cardBg)
    {
        // ReEndUI ClipCorner shader on card frame
        var clipMat = new Material(Shader.Find("ReEnd/UI/ClipCorner"));
        if (clipMat != null) clipMat.SetFloat("_CornerSize", reEndTheme != null ? reEndTheme.clipCornerMd : 12f);
        frame.material = clipMat;
        frame.sprite = UIProceduralHelper.WhiteSliced4x4;
        frame.type = Image.Type.Sliced;
        Color bgColor = reEndTheme != null ? reEndTheme.card : cardBg;
        frame.color = bgColor;

        float backRatio = config != null ? config.backLayerRatio : 0.88f;
        float artRatio = config != null ? config.artLayerRatio : 0.94f;
        float frontRatio = config != null ? config.frontLayerRatio : 0.98f;

        backLayerRect = CreateLayer("BackLayer", out backLayerImage, cardW, cardH, backRatio);
        artLayerRect = CreateLayer("ArtLayer", out artLayerImage, cardW, cardH, artRatio);
        frontLayerRect = CreateLayer("FrontLayer", out frontLayerImage, cardW, cardH, frontRatio);

        // ReEndUI CornerBracket shader on front layer
        var bracketMat = new Material(Shader.Find("ReEnd/UI/CornerBracket"));
        if (bracketMat != null)
        {
            bracketMat.SetFloat("_BracketSize", reEndTheme != null ? reEndTheme.bracketSize : 24f);
            bracketMat.SetFloat("_BracketWidth", reEndTheme != null ? reEndTheme.bracketWidth : 2f);
            bracketMat.SetColor("_BracketColor", reEndTheme != null ? reEndTheme.bracketColor : new Color(1f, 0.83f, 0.16f, 0.4f));
        }
        frontLayerImage.material = bracketMat;

        Color textColor = reEndTheme != null ? reEndTheme.textPrimary : (config != null ? config.cardTextColor : new Color(0.97f, 0.94f, 0.87f, 1f));
        Color hintColor = reEndTheme != null ? reEndTheme.textAccent : (config != null ? config.dragHintColor : new Color(0.96f, 0.91f, 0.72f, 0.82f));

        float titleSize = reEndTheme != null ? reEndTheme.h4Size : 30f;
        float bodySize = reEndTheme != null ? reEndTheme.bodySmSize : 20f;
        float hintSize = reEndTheme != null ? reEndTheme.captionSize : 16f;

        titleText = CreateText("Title", new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(-85f, -16f), new Vector2(170f, 34f), titleSize, FontStyles.Bold, textColor);
        bodyText = CreateText("Body", new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 46f), new Vector2(170f, 64f), bodySize, FontStyles.Normal, textColor);
        dragHintText = CreateText("DragHint", new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 18f), new Vector2(180f, 24f), hintSize, FontStyles.Italic, textColor);
        dragHintText.color = hintColor;
        dragHintText.text = "向上拖拽出牌";
    }

    private void RefreshVisuals()
    {
        Color accentColor = BattleCardSpriteLibrary.GetAccentColor(card);
        Sprite cardSprite = BattleCardSpriteLibrary.GetMainSprite(card);
        Color artFallback = reEndTheme != null ? reEndTheme.surface3 : (config != null ? config.cardArtFallbackColor : new Color(0.23f, 0.25f, 0.31f, 1f));

        if (backLayerImage != null)
        {
            backLayerImage.color = new Color(accentColor.r, accentColor.g, accentColor.b, 0.28f);
        }

        if (artLayerImage != null)
        {
            artLayerImage.sprite = cardSprite;
            artLayerImage.color = cardSprite != null ? Color.white : artFallback;
            artLayerImage.type = cardSprite != null ? Image.Type.Simple : Image.Type.Sliced;
            if (cardSprite == null)
            {
                artLayerImage.sprite = UIProceduralHelper.WhiteSliced4x4;
            }
        }

        if (frontLayerImage != null)
        {
            frontLayerImage.color = new Color(accentColor.r, accentColor.g, accentColor.b, 0.18f);
        }

        if (titleText != null)
        {
            titleText.text = card != null ? card.cardName : "Card";
        }

        if (bodyText != null)
        {
            bodyText.text = card != null ? card.description : string.Empty;
        }
    }

    private RectTransform CreateLayer(string objectName, out Image image, float cardW, float cardH, float fillRatio)
    {
        GameObject layer = new GameObject(objectName, typeof(RectTransform), typeof(Image));
        layer.transform.SetParent(transform, false);
        RectTransform layerRect = layer.GetComponent<RectTransform>();
        layerRect.anchorMin = new Vector2(0.5f, 0.5f);
        layerRect.anchorMax = new Vector2(0.5f, 0.5f);
        layerRect.sizeDelta = new Vector2(cardW * fillRatio, cardH * fillRatio);
        layerRect.anchoredPosition = Vector2.zero;
        image = layer.GetComponent<Image>();
        image.raycastTarget = false;
        image.sprite = UIProceduralHelper.WhiteSliced4x4;
        image.type = Image.Type.Sliced;
        return layerRect;
    }

    private TextMeshProUGUI CreateText(string objectName, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 sizeDelta, float fontSize, FontStyles fontStyle, Color textColor)
    {
        GameObject textObject = new GameObject(objectName, typeof(RectTransform), typeof(TextMeshProUGUI));
        textObject.transform.SetParent(transform, false);
        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = anchorMin;
        textRect.anchorMax = anchorMax;
        textRect.pivot = new Vector2(0.5f, 0.5f);
        textRect.anchoredPosition = anchoredPosition;
        textRect.sizeDelta = sizeDelta;

        TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
        text.font = BattleUIBootstrap.DefaultFont;
        text.fontSize = fontSize;
        text.fontStyle = fontStyle;
        text.alignment = TextAlignmentOptions.Center;
        text.enableWordWrapping = true;
        text.color = textColor;
        text.raycastTarget = false;
        return text;
    }

    private void UpdateDragPosition(Vector2 screenPosition)
    {
        DragScreenPosition = screenPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, screenPosition, null, out Vector2 localPoint))
        {
            rectTransform.anchoredPosition = localPoint;
        }
    }

    private void UpdatePointerParallax(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
        Vector2 halfSize = rectTransform.rect.size * 0.5f;
        localPointerNormalized = new Vector2(
            Mathf.Clamp(localPoint.x / Mathf.Max(halfSize.x, 1f), -1f, 1f),
            Mathf.Clamp(localPoint.y / Mathf.Max(halfSize.y, 1f), -1f, 1f));
    }

    private void UpdateLayerParallax()
    {
        if (backLayerRect == null || artLayerRect == null || frontLayerRect == null)
        {
            return;
        }

        float gyroMult = config != null ? config.parallaxGyroMultiplier : 6f;
        float hoverAmount = config != null ? config.parallaxHoverAmount : 16f;
        float idleAmount = config != null ? config.parallaxIdleAmount : 6f;
        float lerpSpeed = config != null ? config.parallaxLerpSpeed : 18f;
        float backFactor = config != null ? config.backLayerParallax : -0.35f;
        float artFactor = config != null ? config.artLayerParallax : 0.18f;
        float frontFactor = config != null ? config.frontLayerParallax : 0.42f;
        float parallaxDuration = 1f / Mathf.Max(lerpSpeed, 1f);

        Vector2 gyroParallax = Vector2.zero;
        if (gyroEnabled)
        {
            gyroParallax = GetGyroParallax() * gyroMult;
        }

        Vector2 pointerParallax = localPointerNormalized * (isHovered || isDragging ? hoverAmount : idleAmount);
        Vector2 totalParallax = pointerParallax + gyroParallax;

        backLayerRect.anchoredPosition = Vector2.Lerp(backLayerRect.anchoredPosition, totalParallax * backFactor, lerpSpeed * Time.unscaledDeltaTime);
        artLayerRect.anchoredPosition = Vector2.Lerp(artLayerRect.anchoredPosition, totalParallax * artFactor, lerpSpeed * Time.unscaledDeltaTime);
        frontLayerRect.anchoredPosition = Vector2.Lerp(frontLayerRect.anchoredPosition, totalParallax * frontFactor, lerpSpeed * Time.unscaledDeltaTime);
    }

    // 陀螺仪姿态每帧仅读取一次，避免每张卡牌重复原生调用（多卡时开销明显）
    private static Vector2 cachedGyroParallax;
    private static int cachedGyroFrame = -1;

    private static Vector2 GetGyroParallax()
    {
        if (cachedGyroFrame != Time.frameCount)
        {
            cachedGyroFrame = Time.frameCount;
            if (SystemInfo.supportsGyroscope && Input.gyro.enabled)
            {
                Vector3 forward = Input.gyro.attitude * Vector3.forward;
                cachedGyroParallax = new Vector2(forward.x, forward.y);
            }
            else
            {
                cachedGyroParallax = Vector2.zero;
            }
        }

        return cachedGyroParallax;
    }
}
