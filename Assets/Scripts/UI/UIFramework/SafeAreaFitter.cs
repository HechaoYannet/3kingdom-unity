using UnityEngine;

/// <summary>
/// 将 RectTransform 适配到屏幕安全区域，避免刘海/圆角遮挡。
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class SafeAreaFitter : MonoBehaviour
{
    private RectTransform rt;
    private Rect lastSafeArea;

    private void Awake()
    {
        rt = (RectTransform)transform;
    }

    private void OnEnable()
    {
        ApplySafeArea();
    }

    private void OnRectTransformDimensionsChange()
    {
        ApplySafeArea();
    }

    private void ApplySafeArea()
    {
        if (rt == null) rt = (RectTransform)transform;

        Rect safeArea = Screen.safeArea;
        if (safeArea == lastSafeArea) return;
        lastSafeArea = safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}
