using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using ReEndUnity;

/// <summary>
/// 屏幕空间目标按钮，负责显示敌方状态并回传点击与悬停。
/// </summary>
public class BattleTargetButtonView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Runtime References")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI detailText;
    [SerializeField] private BattleTheme theme;

    private BattleHandPresenter presenter;
    private ReEndTheme reEndTheme;
    private string lastTitleText;
    private string lastDetailText;

    /// <summary>
    /// 绑定的目标玩家。
    /// </summary>
    public Player BoundPlayer { get; private set; }

    /// <summary>
    /// 目标按钮矩形。
    /// </summary>
    public RectTransform RectTransform => rectTransform;

    /// <summary>
    /// 通过 prefab 创建一个目标按钮。
    /// </summary>
    /// <param name="prefab">目标按钮 prefab。</param>
    /// <param name="parent">父节点。</param>
    /// <param name="owner">所属 presenter。</param>
    /// <returns>创建好的按钮视图。</returns>
    public static BattleTargetButtonView Create(BattleTargetButtonView prefab, Transform parent, BattleHandPresenter owner)
    {
        if (prefab == null)
        {
            Debug.LogError("BattleTargetButtonView prefab is not assigned.");
            return null;
        }

        BattleTargetButtonView view = Instantiate(prefab, parent);
        view.presenter = owner;
        view.EnsureRuntimeReferences();
        view.gameObject.SetActive(true);
        return view;
    }

    /// <summary>
    /// 绑定目标玩家。
    /// </summary>
    /// <param name="targetPlayer">目标玩家。</param>
    public void Bind(Player targetPlayer)
    {
        BoundPlayer = targetPlayer;
        RefreshText();
    }

    /// <summary>
    /// 更新目标按钮表现。
    /// </summary>
    /// <param name="anchoredPosition">目标位置。</param>
    /// <param name="isSelected">是否已选中。</param>
    /// <param name="isHovered">是否悬停。</param>
    /// <param name="isVisible">是否显示。</param>
    public void UpdatePresentation(Vector2 anchoredPosition, bool isSelected, bool isHovered, bool isVisible)
    {
        gameObject.SetActive(isVisible);
        if (!isVisible)
        {
            return;
        }

        float animDuration = 0.12f;
        rectTransform.DOAnchorPos(anchoredPosition, animDuration).SetEase(Ease.OutQuad).SetUpdate(true);
        rectTransform.DOScale(Vector3.one * (isHovered ? 1.06f : isSelected ? 1.03f : 1f), animDuration).SetEase(Ease.OutQuad).SetUpdate(true);

        Color baseColor = isSelected
            ? (reEndTheme != null ? reEndTheme.efRed : (theme != null ? theme.enemyColor : new Color(0.62f, 0.18f, 0.18f, 0.94f)))
            : (reEndTheme != null ? reEndTheme.surface2 : new Color(0.10f, 0.12f, 0.18f, 0.88f));
        Color hoverColor = reEndTheme != null ? reEndTheme.surfaceHover : (theme != null ? theme.warningColor : new Color(0.84f, 0.34f, 0.24f, 0.98f));
        backgroundImage.DOColor(isHovered ? hoverColor : baseColor, 0.1f).SetUpdate(true);
        RefreshText();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        presenter.NotifyTargetClicked(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        presenter.NotifyTargetHover(this, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        presenter.NotifyTargetHover(this, false);
    }

    private void Awake()
    {
        reEndTheme = ReEndThemeManager.Current;
        EnsureRuntimeReferences();
    }

    private void EnsureRuntimeReferences()
    {
        rectTransform = rectTransform != null ? rectTransform : GetComponent<RectTransform>();
        backgroundImage = backgroundImage != null ? backgroundImage : GetComponent<Image>();

        if (rectTransform == null)
        {
            rectTransform = gameObject.AddComponent<RectTransform>();
        }

        if (backgroundImage == null)
        {
            backgroundImage = gameObject.AddComponent<Image>();
        }

        rectTransform.sizeDelta = new Vector2(220f, 84f);
        backgroundImage.sprite = UIProceduralHelper.WhiteSliced4x4;
        backgroundImage.type = Image.Type.Sliced;
        backgroundImage.color = reEndTheme != null ? reEndTheme.surface2 : new Color(0.10f, 0.12f, 0.18f, 0.88f);

        // ReEndUI ClipCorner shader
        var clipMat = new Material(Shader.Find("ReEnd/UI/ClipCorner"));
        if (clipMat != null) clipMat.SetFloat("_CornerSize", reEndTheme != null ? reEndTheme.clipCornerSm : 8f);
        backgroundImage.material = clipMat;

        if (titleText == null)
        {
            titleText = CreateText("Title", new Vector2(0.5f, 0.68f), new Vector2(188f, 28f), reEndTheme != null ? reEndTheme.h4Size : 26f, FontStyles.Bold);
        }

        if (detailText == null)
        {
            detailText = CreateText("Detail", new Vector2(0.5f, 0.28f), new Vector2(188f, 22f), reEndTheme != null ? reEndTheme.bodySmSize : 18f, FontStyles.Normal);
        }
    }

    private TextMeshProUGUI CreateText(string objectName, Vector2 anchor, Vector2 size, float fontSize, FontStyles fontStyle)
    {
        GameObject textObject = new GameObject(objectName, typeof(RectTransform), typeof(TextMeshProUGUI));
        textObject.transform.SetParent(transform, false);

        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = anchor;
        textRect.anchorMax = anchor;
        textRect.pivot = new Vector2(0.5f, 0.5f);
        textRect.anchoredPosition = Vector2.zero;
        textRect.sizeDelta = size;

        TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
        text.font = BattleUIBootstrap.DefaultFont;
        text.fontSize = fontSize;
        text.fontStyle = fontStyle;
        text.alignment = TextAlignmentOptions.Center;
        text.color = reEndTheme != null ? reEndTheme.textPrimary : new Color(0.97f, 0.94f, 0.87f, 1f);
        text.raycastTarget = false;
        return text;
    }

    private void RefreshText()
    {
        if (titleText == null || detailText == null)
        {
            return;
        }

        string title;
        string detail;
        if (BoundPlayer == null)
        {
            title = "Unknown";
            detail = string.Empty;
        }
        else
        {
            string roleName = BoundPlayer.CurrentRole != null ? BoundPlayer.CurrentRole.GetType().Name : "Enemy";
            title = string.Format("{0}  #{1}", roleName, BoundPlayer.PlayerID);
            detail = string.Format("HP {0}/{1}", BoundPlayer.CurrentHP, BoundPlayer.GetMaxHP());
        }

        // 去重：拖动时 LayoutTargets 可能频繁触发，避免重复设置相同文本
        if (title != lastTitleText)
        {
            titleText.text = title;
            lastTitleText = title;
        }

        if (detail != lastDetailText)
        {
            detailText.text = detail;
            lastDetailText = detail;
        }
    }
}
