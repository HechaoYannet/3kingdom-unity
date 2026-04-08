using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 治疗数字控制器
public class HealTextController : MonoBehaviour
{
    [Header("UI组件")]
    [SerializeField] private TextMeshProUGUI healText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("动画参数")]
    [SerializeField] private float floatHeight = 1.5f;
    [SerializeField] private float duration = 1f;

    public void Initialize(int healAmount)
    {
        healText.text = $"+{healAmount}";
        healText.color = Color.green;

        StartCoroutine(AnimateHealText());
    }

    private IEnumerator AnimateHealText()
    {
        float elapsed = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * floatHeight;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // 位置动画
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            // 透明度动画
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1 - t;
            }

            yield return null;
        }

        // 动画结束后回收
        UIManager.Instance.GetWorldSpaceManager().ReleaseUIElement(gameObject);
    }
}
