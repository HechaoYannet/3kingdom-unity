using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ReEndUnity;

/// <summary>
/// 顶部 HUD：显示当前回合归属、阶段名称和响应提示。
/// 挂到 Canvas(Screen) 下的 PhaseHUD GameObject 上。
/// </summary>
public class BattlePhaseHUD : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI turnLabel;
    [SerializeField] private TextMeshProUGUI phaseLabel;
    [SerializeField] private TextMeshProUGUI responseLabel;
    [SerializeField] private CanvasGroup responseGroup;
    [SerializeField] private Image panelBackground;

    [Header("Settings")]
    [SerializeField] private float responseBlinkSpeed = 2.5f;

    [SerializeField] private BattleTheme theme;

    private Player watchedPlayer;
    private string playerRoleName = "";
    private string enemyRoleName = "";

    private ReEndTheme reEndTheme;

    private void Awake()
    {
        reEndTheme = ReEndThemeManager.Current;
        EnsureReferences();
    }

    private void EnsureReferences()
    {
        // 背景面板 — ReEndUI 暗色风格
        if (panelBackground == null)
        {
            var bgGo = new GameObject("PanelBG", typeof(RectTransform), typeof(Image));
            bgGo.transform.SetParent(transform, false);
            panelBackground = bgGo.GetComponent<Image>();
            var bgRect = bgGo.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;
            bgRect.SetAsFirstSibling();

            Color bg = reEndTheme != null ? reEndTheme.surface2 : new Color(0.1f, 0.1f, 0.1f, 0.9f);
            panelBackground.color = bg;

            var clipMat = new Material(Shader.Find("ReEnd/UI/ClipCorner"));
            if (clipMat != null) clipMat.SetFloat("_CornerSize", reEndTheme != null ? reEndTheme.clipCornerMd : 12f);
            panelBackground.material = clipMat;
        }

        // 扫描线叠加
        var existingScan = transform.Find("Scanline");
        if (existingScan == null && reEndTheme != null)
        {
            var scanGo = new GameObject("Scanline", typeof(RectTransform), typeof(Image));
            scanGo.transform.SetParent(transform, false);
            var scanImg = scanGo.GetComponent<Image>();
            var scanMat = new Material(Shader.Find("ReEnd/UI/Scanline"));
            scanImg.material = scanMat;
            scanImg.raycastTarget = false;
            var scanRect = scanGo.GetComponent<RectTransform>();
            scanRect.anchorMin = Vector2.zero;
            scanRect.anchorMax = Vector2.one;
            scanRect.offsetMin = Vector2.zero;
            scanRect.offsetMax = Vector2.zero;
        }

        if (turnLabel == null) turnLabel = BuildLabel("TurnLabel",
            reEndTheme != null ? reEndTheme.h4Size : 22f,
            new Vector2(0f, 0.55f), new Vector2(1f, 1f));
        if (phaseLabel == null) phaseLabel = BuildLabel("PhaseLabel",
            reEndTheme != null ? reEndTheme.bodyLgSize : 18f,
            new Vector2(0f, 0.05f), new Vector2(1f, 0.5f));

        if (responseLabel == null)
        {
            var respGo = new GameObject("ResponseLabel", typeof(RectTransform), typeof(TextMeshProUGUI));
            respGo.transform.SetParent(transform, false);
            responseLabel = respGo.GetComponent<TextMeshProUGUI>();
            var rr = respGo.GetComponent<RectTransform>();
            rr.anchorMin = new Vector2(0f, -1.5f);
            rr.anchorMax = new Vector2(1f, -0.2f);
            rr.offsetMin = Vector2.zero;
            rr.offsetMax = Vector2.zero;
            responseLabel.font = BattleUIBootstrap.DefaultFont;
            responseLabel.fontSize = reEndTheme != null ? reEndTheme.h3Size : 28f;
            responseLabel.alignment = TextAlignmentOptions.Center;
            responseGroup = respGo.AddComponent<CanvasGroup>();
        }
    }

    private TextMeshProUGUI BuildLabel(string name, float fontSize, Vector2 anchorMin, Vector2 anchorMax)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
        go.transform.SetParent(transform, false);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        var tmp = go.GetComponent<TextMeshProUGUI>();
        tmp.font = BattleUIBootstrap.DefaultFont;
        tmp.fontSize = fontSize;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = reEndTheme != null ? reEndTheme.textPrimary : Color.white;
        return tmp;
    }

    /// <summary>
    /// 绑定要观察的玩家（人类玩家）。
    /// </summary>
    public void BindPlayer(Player player)
    {
        watchedPlayer = player;
        RefreshRoleNames();
    }

    /// <summary>
    /// 外部更新角色名（在角色分配后调用）。
    /// </summary>
    public void RefreshRoleNames()
    {
        foreach (Player p in PlayerManager.Instance?.players ?? new System.Collections.Generic.List<Player>())
        {
            if (p == null) continue;
            string roleName = p.CurrentRole != null ? p.CurrentRole.roleName : "未知";
            if (p is EnemyAI)
                enemyRoleName = roleName;
            else
                playerRoleName = roleName;
        }
    }

    private void Update()
    {
        if (RoundManager.instance == null) return;

        GRoundState state = RoundManager.instance.RoundState;
        Player currentPlayer = RoundManager.instance.CurrentTurnPlayer;
        bool isPlayerTurn = currentPlayer != null && !(currentPlayer is EnemyAI);
        bool isResponseActive = watchedPlayer != null && watchedPlayer.IsResponseInputActive;

        // ── 回合归属 ──
        Color playerTurnColor = reEndTheme != null ? reEndTheme.primary : (theme != null ? theme.primaryColor : new Color(0.98f, 0.94f, 0.56f));
        Color enemyTurnColor = reEndTheme != null ? reEndTheme.efRed : (theme != null ? theme.enemyColor : new Color(0.84f, 0.34f, 0.24f));
        Color responseColor = reEndTheme != null ? reEndTheme.efOrange : (theme != null ? theme.warningColor : new Color(1f, 0.35f, 0.35f));

        if (turnLabel != null)
        {
            if (currentPlayer == null)
            {
                turnLabel.text = "准备中…";
                turnLabel.color = Color.white;
            }
            else if (isPlayerTurn)
            {
                turnLabel.text = $"<b>你的回合</b>  ·  {playerRoleName}";
                turnLabel.color = playerTurnColor;
            }
            else
            {
                turnLabel.text = $"敌方回合  ·  {enemyRoleName}";
                turnLabel.color = enemyTurnColor;
            }
        }

        // ── 阶段名 ──
        if (phaseLabel != null)
        {
            phaseLabel.text = GetPhaseDisplayName(state);
        }

        // ── 响应提示 ──
        if (responseGroup != null && responseLabel != null)
        {
            bool show = isResponseActive;
            responseGroup.alpha = show ? 0.5f + Mathf.Abs(Mathf.Sin(Time.unscaledTime * responseBlinkSpeed)) * 0.5f : 0f;
            responseGroup.gameObject.SetActive(show);

            if (show)
            {
                string respCardType = watchedPlayer?.ResponsingCard?.CardType ?? "";
                responseLabel.text = respCardType == "Sha"
                    ? "⚠ 请出 <b>闪</b> 响应 ⚠"
                    : $"⚠ 需要响应: <b>{respCardType}</b>";
                responseLabel.color = responseColor;
            }
        }
    }

    private string GetPhaseDisplayName(GRoundState state)
    {
        return state switch
        {
            GRoundState.Preparing => "准备阶段",
            GRoundState.Checking => "判定阶段",
            GRoundState.GettingCard => "摸牌阶段",
            GRoundState.Battling => "出牌阶段",
            GRoundState.RefusingCard => "弃牌阶段",
            GRoundState.Ending => "结束阶段",
            _ => ""
        };
    }
}
