using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using ReEndUnity;

/// <summary>
/// 屏幕空间 HP 信息条：显示角色名、HP 数值和血条。
/// 在 Awake 中自动构建视觉子树。
/// </summary>
public class BattlePlayerHUD : MonoBehaviour
{
    [Header("Bindings (auto-built)")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image barFill;
    [SerializeField] private Image barBg;
    [SerializeField] private Image panelBg;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Target")]
    [SerializeField] private Player targetPlayer;
    [SerializeField] private BattleTheme theme;
    [SerializeField] private Color barColor = new Color(0.28f, 0.72f, 0.44f);
    [SerializeField] private Color enemyBarColor = new Color(0.84f, 0.34f, 0.24f);

    private int lastHP = -1;
    private int lastMaxHP = -1;
    private string lastName = "";

    private ReEndTheme reEndTheme;

    /// <summary>
    /// 绑定目标玩家并开始监听。
    /// </summary>
    public void Bind(Player player)
    {
        if (targetPlayer != null)
            targetPlayer.HandChanged -= OnPlayerChanged;
        targetPlayer = player;
        if (targetPlayer != null)
        {
            targetPlayer.HandChanged += OnPlayerChanged;
            barColor = targetPlayer is EnemyAI ? enemyBarColor : barColor;
        }
        FullRefresh();
    }

    private void Awake()
    {
        reEndTheme = ReEndThemeManager.Current;
        EnsureReferences();
    }

    private void OnDestroy()
    {
        if (targetPlayer != null)
            targetPlayer.HandChanged -= OnPlayerChanged;
    }

    private void OnPlayerChanged(Player p)
    {
        FullRefresh();
    }

    private void LateUpdate()
    {
        FullRefresh();
    }

    private void FullRefresh()

    {

        if (targetPlayer == null) return;


        string roleName = targetPlayer.CurrentRole != null ? targetPlayer.CurrentRole.roleName : "未知";

        int hp = targetPlayer.CurrentHP;

        int maxHp = targetPlayer.GetMaxHP();


        bool hpChanged = hp != lastHP;

        if (hp == lastHP && maxHp == lastMaxHP && roleName == lastName) return;

        lastHP = hp;

        lastMaxHP = maxHp;

        lastName = roleName;


        if (nameText != null)

            nameText.text = targetPlayer is EnemyAI ? $"{roleName} (敌方)" : $"{roleName} (你)";

        if (hpText != null)

            hpText.text = $"HP {hp}/{maxHp}";


        // HP 变化时 punch 动画


        if (hpChanged && hpText != null)


        {


            hpText.transform.DOKill();


            hpText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 1, 0.5f).SetUpdate(true);


        }



        if (barFill != null)


        {


            float ratio = maxHp > 0 ? (float)hp / maxHp : 0f;


            barFill.DOKill();


            barFill.DOFillAmount(ratio, 0.3f).SetEase(Ease.OutQuad).SetUpdate(true);

            Color healthyColor = reEndTheme != null ? reEndTheme.efGreen : (theme != null ? theme.playerColor : barColor);

            Color criticalColor = reEndTheme != null ? reEndTheme.efRed : (theme != null ? theme.enemyColor : new Color(0.9f, 0.2f, 0.2f));

            Color midColor = reEndTheme != null ? reEndTheme.efOrange : new Color(1f, 0.7f, 0.2f);

            Color targetBarColor = ratio > 0.5f ? healthyColor :

                            ratio > 0.25f ? midColor :

                            criticalColor;

            barFill.DOColor(targetBarColor, 0.3f).SetUpdate(true);

        }

    }

    private void EnsureReferences()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null) rectTransform = gameObject.AddComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        if (barBg == null || barFill == null || nameText == null || hpText == null)
            BuildVisualTree();
    }

    private void BuildVisualTree()
    {
        // 面板背景 — ReEndUI 暗色风格
        panelBg = gameObject.GetComponent<Image>();
        if (panelBg == null)
            panelBg = gameObject.AddComponent<Image>();

        Color panelColor = reEndTheme != null ? reEndTheme.surface1 : new Color(0.05f, 0.06f, 0.1f, 0.9f);
        panelBg.color = panelColor;

        var clipMat = new Material(Shader.Find("ReEnd/UI/ClipCorner"));
        if (clipMat != null) clipMat.SetFloat("_CornerSize", reEndTheme != null ? reEndTheme.clipCornerSm : 8f);
        panelBg.material = clipMat;

        // Name text
        var nameGo = new GameObject("NameText", typeof(RectTransform), typeof(TextMeshProUGUI));
        nameGo.transform.SetParent(transform, false);
        nameText = nameGo.GetComponent<TextMeshProUGUI>();
        var nameRect = nameGo.GetComponent<RectTransform>();
        nameRect.anchorMin = new Vector2(0f, 0.55f);
        nameRect.anchorMax = new Vector2(1f, 1f);
        nameRect.offsetMin = new Vector2(12f, 0f);
        nameRect.offsetMax = new Vector2(-12f, -4f);
        nameText.font = BattleUIBootstrap.DefaultFont;
        nameText.fontSize = reEndTheme != null ? reEndTheme.bodyLgSize : 18f;
        nameText.color = reEndTheme != null ? reEndTheme.textPrimary : new Color(0.97f, 0.94f, 0.87f);
        nameText.alignment = TextAlignmentOptions.Left;

        // HP text
        var hpGo = new GameObject("HPText", typeof(RectTransform), typeof(TextMeshProUGUI));
        hpGo.transform.SetParent(transform, false);
        hpText = hpGo.GetComponent<TextMeshProUGUI>();
        var hpRect = hpGo.GetComponent<RectTransform>();
        hpRect.anchorMin = new Vector2(0.7f, 0.55f);
        hpRect.anchorMax = new Vector2(1f, 1f);
        hpRect.offsetMin = new Vector2(0f, 0f);
        hpRect.offsetMax = new Vector2(-12f, -4f);
        hpText.font = BattleUIBootstrap.DefaultFont;
        hpText.fontSize = reEndTheme != null ? reEndTheme.h4Size : 22f;
        hpText.color = reEndTheme != null ? reEndTheme.primary : new Color(0.97f, 0.94f, 0.87f);
        hpText.alignment = TextAlignmentOptions.Right;

        // HP bar background
        var bgGo = new GameObject("BarBG", typeof(RectTransform), typeof(Image));
        bgGo.transform.SetParent(transform, false);
        barBg = bgGo.GetComponent<Image>();
        var bgRect = bgGo.GetComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0f, 0.05f);
        bgRect.anchorMax = new Vector2(1f, 0.5f);
        bgRect.offsetMin = new Vector2(10f, 2f);
        bgRect.offsetMax = new Vector2(-10f, -2f);
        barBg.color = reEndTheme != null ? reEndTheme.surface3 : new Color(0.15f, 0.15f, 0.2f, 1f);

        var barClipMat = new Material(Shader.Find("ReEnd/UI/ClipCorner"));
        if (barClipMat != null) barClipMat.SetFloat("_CornerSize", reEndTheme != null ? reEndTheme.clipCornerSm : 8f);
        barBg.material = barClipMat;

        // HP bar fill
        var fillGo = new GameObject("BarFill", typeof(RectTransform), typeof(Image));
        fillGo.transform.SetParent(bgGo.transform, false);
        barFill = fillGo.GetComponent<Image>();
        barFill.type = Image.Type.Filled;
        barFill.fillMethod = Image.FillMethod.Horizontal;
        barFill.fillOrigin = 0;
        barFill.fillAmount = 1f;
        var fillRect = fillGo.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;
    }
}
