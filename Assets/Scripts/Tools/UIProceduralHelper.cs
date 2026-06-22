using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 运行时程序化生成 UI Sprite，替代 Tuanjie 中不可用的 Resources.GetBuiltinResource。
/// </summary>
public static class UIProceduralHelper
{
    private static Sprite _whiteSliced4x4;
    private static Sprite _white1x1;

    /// <summary>
    /// 获取 4x4 白色 Sliced Sprite，用于 Image.type=Sliced 的纯色背景。
    /// </summary>
    public static Sprite WhiteSliced4x4
    {
        get
        {
            if (_whiteSliced4x4 == null)
                _whiteSliced4x4 = CreateSolidSprite(4, 4, Color.white, 2f);
            return _whiteSliced4x4;
        }
    }

    /// <summary>
    /// 获取 1x1 白色 Simple Sprite，用于 Image.type=Simple 的纯色填充。
    /// </summary>
    public static Sprite White1x1
    {
        get
        {
            if (_white1x1 == null)
                _white1x1 = CreateSolidSprite(1, 1, Color.white, 100f);
            return _white1x1;
        }
    }

    private static Sprite CreateSolidSprite(int width, int height, Color color, float pixelsPerUnit)
    {
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Bilinear;
        tex.wrapMode = TextureWrapMode.Clamp;
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = color;
        tex.SetPixels(pixels);
        tex.Apply();

        return Sprite.Create(tex, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), pixelsPerUnit, 0, SpriteMeshType.FullRect);
    }
}
