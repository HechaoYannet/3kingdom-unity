using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using ReEndUnity;

/// <summary>
/// 战斗结束面板：遮罩 + 胜/负大字 + 重新开始。
/// </summary>
public class BattleResultPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI resultLabel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Image background;
    [SerializeField] private BattleTheme theme;

    private ReEndTheme reEndTheme;

    private void Awake()
    {
        reEndTheme = ReEndThemeManager.Current;
        EnsureReferences();
        Hide();
    }

    /// <summary>
    /// 显示胜负结果。
    /// </summary>
    public void Show(GameResult result)
    {
        // 杀死可能正在执行的 Hide Tween，防止 Show→Hide→Show 竞态
        canvasGroup.DOKill();
        if (resultLabel != null)
            resultLabel.transform.DOKill();

        if (resultLabel != null)
        {
            resultLabel.text = result == GameResult.Win ? "胜 利" : "败 北";
            resultLabel.color = result == GameResult.Win
                ? (reEndTheme != null ? reEndTheme.efGreen : (theme != null ? theme.primaryColor : new Color(0.98f, 0.88f, 0.32f)))
                : (reEndTheme != null ? reEndTheme.efRed : (theme != null ? theme.enemyColor : new Color(0.84f, 0.28f, 0.28f)));
        }

        gameObject.SetActive(true);
        canvasGroup.DOFade(1f, 0.3f).SetEase(Ease.OutQuad).SetUpdate(true);
        canvasGroup.blocksRaycasts = true;

        if (resultLabel != null)
        {
            resultLabel.transform.localScale = new Vector3(0.92f, 0.92f, 1f);
            resultLabel.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack).SetUpdate(true).SetDelay(0.1f);
        }

        if (AudioManage.Instance != null)
            AudioManage.Instance.PlayBattleSfx(result == GameResult.Win ? AudioManage.BattleSfx.Victory : AudioManage.BattleSfx.Defeat);
    }

    /// <summary>
    /// 隐藏面板。
    /// </summary>
    public void Hide()
    {
        canvasGroup.DOKill();
        canvasGroup.DOFade(0f, 0.2f).SetEase(Ease.OutQuad).SetUpdate(true).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
        canvasGroup.blocksRaycasts = false;
    }

    private void OnRestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void EnsureReferences()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;

        var rt = GetComponent<RectTransform>();
        if (rt == null) rt = gameObject.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        if (background == null)
        {
            background = gameObject.AddComponent<Image>();
            float overlayAlpha = reEndTheme != null ? reEndTheme.opacityOverlay : 0.85f;
            Color bg = reEndTheme != null ? reEndTheme.background : new Color(0f, 0f, 0f);
            background.color = new Color(bg.r, bg.g, bg.b, overlayAlpha);
        }

        // 扫描线叠加层
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

        if (resultLabel == null)
        {
            var labelGo = new GameObject("ResultLabel", typeof(RectTransform), typeof(TextMeshProUGUI));
            labelGo.transform.SetParent(transform, false);
            resultLabel = labelGo.GetComponent<TextMeshProUGUI>();
            var lblRect = labelGo.GetComponent<RectTransform>();
            lblRect.anchorMin = new Vector2(0.5f, 0.55f);
            lblRect.anchorMax = new Vector2(0.5f, 0.8f);
            lblRect.sizeDelta = new Vector2(400f, 120f);
            lblRect.anchoredPosition = Vector2.zero;
            resultLabel.font = BattleUIBootstrap.DefaultFont;
            resultLabel.fontSize = reEndTheme != null ? reEndTheme.displayXlSize : 72f;
            resultLabel.alignment = TextAlignmentOptions.Center;
            resultLabel.fontStyle = FontStyles.Bold;
        }

        if (restartButton == null)
        {
            var btnGo = new GameObject("RestartButton", typeof(RectTransform), typeof(Image), typeof(Button));
            btnGo.transform.SetParent(transform, false);
            restartButton = btnGo.GetComponent<Button>();
            var btnRect = btnGo.GetComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(0.5f, 0.3f);
            btnRect.anchorMax = new Vector2(0.5f, 0.45f);
            btnRect.sizeDelta = new Vector2(240f, 64f);
            btnRect.anchoredPosition = Vector2.zero;

            // ReEndUI Primary 风格按钮
            var btnImg = btnGo.GetComponent<Image>();
            btnImg.color = reEndTheme != null ? reEndTheme.primary : new Color(1f, 0.83f, 0.16f, 1f);
            var clipMat = new Material(Shader.Find("ReEnd/UI/ClipCorner"));
            if (clipMat != null) clipMat.SetFloat("_CornerSize", reEndTheme != null ? reEndTheme.clipCornerMd : 12f);
            btnImg.material = clipMat;

            var btnLabel = new GameObject("BtnText", typeof(RectTransform), typeof(TextMeshProUGUI));
            btnLabel.transform.SetParent(btnGo.transform, false);
            var btnTmp = btnLabel.GetComponent<TextMeshProUGUI>();
            btnTmp.text = "重新开始";
            btnTmp.font = BattleUIBootstrap.DefaultFont;
            btnTmp.fontSize = reEndTheme != null ? reEndTheme.h4Size : 22f;
            btnTmp.alignment = TextAlignmentOptions.Center;
            btnTmp.color = reEndTheme != null ? reEndTheme.primaryForeground : new Color(0.1f, 0.1f, 0.1f, 1f);
            btnTmp.fontStyle = FontStyles.Bold;
            var btnLblRect = btnLabel.GetComponent<RectTransform>();
            btnLblRect.anchorMin = Vector2.zero;
            btnLblRect.anchorMax = Vector2.one;
            btnLblRect.offsetMin = Vector2.zero;
            btnLblRect.offsetMax = Vector2.zero;

            restartButton.onClick.AddListener(OnRestartClicked);
        }
    }
}
