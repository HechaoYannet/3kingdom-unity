using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

// 伤害数字控制器
public class DamageTextController : MonoBehaviour
{
    [Header("UI组件")]
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("动画参数")]
    [SerializeField] private float floatHeight = 2f;
    [SerializeField] private float duration = 1f;
    [SerializeField] private AnimationCurve floatCurve;
    [SerializeField] private AnimationCurve fadeCurve;

    private Vector3 startPosition;
    private Color originalColor;

    void Awake()
    {
        if (damageText == null)
            damageText = GetComponent<TextMeshProUGUI>();

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        originalColor = damageText.color;
        startPosition = transform.position;
    }

    public void Initialize(int damage, bool isCritical = false)
    {
        // 设置伤害文本
        damageText.text = damage.ToString();

        // 暴击效果
        if (isCritical)
        {
            damageText.color = Color.red;
            damageText.fontSize += 10;
        }

        // 开始动画
        StartCoroutine(AnimateDamageText());
    }

    private IEnumerator AnimateDamageText()
    {
        float elapsed = 0f;
        Vector3 targetPosition = startPosition + Vector3.up * floatHeight;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // 位置动画
            transform.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                floatCurve.Evaluate(t)
            );

            // 透明度动画
            if (canvasGroup != null)
            {
                canvasGroup.alpha = fadeCurve.Evaluate(t);
            }

            yield return null;
        }

        // 动画结束后回收
        UIManager.Instance.GetWorldSpaceManager().ReleaseUIElement(gameObject);
    }
}

