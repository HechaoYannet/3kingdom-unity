using TMPro;
using UnityEngine;
using DG.Tweening;
using ReEndUnity;

/// <summary>
/// 战斗反馈文字池，统一管理伤害/治疗飘字的对象池与动画。
/// 实现 IFeedbackProvider 接口供规则层通过 BattleFeedback 静态门面调用。
/// </summary>
[RequireComponent(typeof(Canvas))]
public class FeedbackTextPool : MonoBehaviour, IFeedbackProvider
{
    [SerializeField] private TextMeshProUGUI floatingTextPrefab;
    [SerializeField] private int poolSize = 20;
    [SerializeField] private float floatHeight = 60f;
    [SerializeField] private float duration = 1f;
    [SerializeField] private Color damageColor = new Color(1f, 0.35f, 0.25f, 1f);
    [SerializeField] private Color healColor = new Color(0.28f, 0.72f, 0.44f, 1f);
    [SerializeField] private Color criticalColor = new Color(1f, 0.82f, 0.24f, 1f);

    private readonly System.Collections.Generic.Queue<TextMeshProUGUI> pool = new System.Collections.Generic.Queue<TextMeshProUGUI>();
    private Camera worldCamera;
    private RectTransform canvasRect;
    private ReEndTheme reEndTheme;

    private void Awake()
    {
        reEndTheme = ReEndThemeManager.Current;
        Canvas canvas = GetComponent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
        worldCamera = canvas.worldCamera;

        if (floatingTextPrefab == null)
        {
            floatingTextPrefab = CreateDefaultPrefab();
        }

        for (int i = 0; i < poolSize; i++)
        {
            TextMeshProUGUI text = Instantiate(floatingTextPrefab, transform);
            text.gameObject.SetActive(false);
            pool.Enqueue(text);
        }

        BattleFeedback.SetProvider(this);
    }

    private void OnDestroy()
    {
        BattleFeedback.SetProvider(null);
    }

    /// <summary>
    /// 显示伤害飘字。
    /// </summary>
    /// <param name="amount">伤害数值。</param>
    /// <param name="worldPos">世界坐标位置。</param>
    /// <param name="isCritical">是否为暴击。</param>
    public void ShowDamage(int amount, Vector3 worldPos, bool isCritical = false)
    {
        TextMeshProUGUI text = GetFromPool();
        if (text == null) return;

        text.text = isCritical ? $"{amount}!" : amount.ToString();
        text.color = isCritical
            ? (reEndTheme != null ? reEndTheme.efYellow : criticalColor)
            : (reEndTheme != null ? reEndTheme.efRed : damageColor);
        text.fontSize = isCritical
            ? (reEndTheme != null ? reEndTheme.h2Size : 42f)
            : (reEndTheme != null ? reEndTheme.h3Size : 32f);
        PositionAndAnimate(text, worldPos);
        if (AudioManage.Instance != null)
            AudioManage.Instance.PlayBattleSfx(AudioManage.BattleSfx.Damage);
    }

    /// <summary>
    /// 显示治疗飘字。
    /// </summary>
    /// <param name="amount">治疗数值。</param>
    /// <param name="worldPos">世界坐标位置。</param>
    public void ShowHeal(int amount, Vector3 worldPos)
    {
        TextMeshProUGUI text = GetFromPool();
        if (text == null) return;

        text.text = $"+{amount}";
        text.color = reEndTheme != null ? reEndTheme.efGreen : healColor;
        text.fontSize = reEndTheme != null ? reEndTheme.h3Size : 32f;
        PositionAndAnimate(text, worldPos);
        if (AudioManage.Instance != null)
            AudioManage.Instance.PlayBattleSfx(AudioManage.BattleSfx.Heal);
    }

    private TextMeshProUGUI GetFromPool()
    {
        TextMeshProUGUI text = pool.Count > 0 ? pool.Dequeue() : Instantiate(floatingTextPrefab, transform);
        // 杀死残留 Tween，防止旧动画在新文字上继续运行
        text.rectTransform.DOKill();
        text.DOKill();
        text.gameObject.SetActive(true);
        return text;
    }

    private void ReturnToPool(TextMeshProUGUI text)
    {
        if (text == null) return;
        text.rectTransform.DOKill();
        text.DOKill();
        text.gameObject.SetActive(false);
        pool.Enqueue(text);
    }

    private void PositionAndAnimate(TextMeshProUGUI text, Vector3 worldPos)
    {
        Vector2 screenPos = worldCamera != null
            ? worldCamera.WorldToScreenPoint(worldPos)
            : RectTransformUtility.WorldToScreenPoint(null, worldPos);

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out localPos);
        text.rectTransform.anchoredPosition = localPos;
        text.alpha = 1f;

        // DOTween 动画：上浮 + 淡出，完成后回池
        text.rectTransform.DOAnchorPos(localPos + Vector2.up * floatHeight, duration)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true)
            .OnComplete(() => ReturnToPool(text));
        text.DOFade(0f, duration)
            .SetEase(Ease.InQuad)
            .SetUpdate(true);
    }

    private TextMeshProUGUI CreateDefaultPrefab()
    {
        GameObject go = new GameObject("FloatingText", typeof(RectTransform), typeof(TextMeshProUGUI));
        go.transform.SetParent(transform, false);
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(200f, 50f);
        TextMeshProUGUI text = go.GetComponent<TextMeshProUGUI>();
        text.font = BattleUIBootstrap.DefaultFont;
        text.alignment = TextAlignmentOptions.Center;
        text.raycastTarget = false;
        go.SetActive(false);
        return text;
    }
}
