using System.Collections.Generic;
using UnityEngine;
using ReEndUnity;

/// <summary>
/// 为屏幕空间手牌提供卡图与配色映射。
/// </summary>
public static class BattleCardSpriteLibrary
{
    private static readonly Dictionary<string, string> SpritePathByType = new Dictionary<string, string>
    {
        { "Sha", "UI/Card/杀" },
        { "Shan", "UI/Card/闪" },
        { "Tao", "UI/Card/桃" },
        { "NanManRuQin", "UI/Card/南蛮入侵" },
        { "WanJianQiFa", "UI/Card/万箭齐发" },
        { "WuZhongShengYou", "UI/Card/无中生有" }
    };

    private static readonly Dictionary<string, string> EffectSpritePathByType = new Dictionary<string, string>
    {
        { "Sha", "UI/Card/杀_effect" },
        { "Shan", "UI/Card/闪_effect" }
    };

    private static readonly Dictionary<string, Sprite> SpriteCache = new Dictionary<string, Sprite>();

    private static BattleTheme theme;

    /// <summary>
    /// 注入主题配置，用于获取配色。
    /// </summary>
    public static void SetTheme(BattleTheme t) => theme = t;

    /// <summary>
    /// 获取卡牌主图。
    /// </summary>
    public static Sprite GetMainSprite(Card card)
    {
        if (card == null) return null;

        if (card.cardImage != null) return card.cardImage;

        if (!SpritePathByType.TryGetValue(card.CardType, out string path)) return null;

        if (!SpriteCache.TryGetValue(path, out Sprite sprite))
        {
            sprite = Resources.Load<Sprite>(path);
            SpriteCache[path] = sprite;
        }

        return sprite;
    }

    /// <summary>
    /// 获取卡牌特效图。
    /// </summary>
    public static Sprite GetEffectSprite(Card card)
    {
        if (card == null) return null;

        if (!EffectSpritePathByType.TryGetValue(card.CardType, out string path)) return null;

        if (!SpriteCache.TryGetValue(path, out Sprite sprite))
        {
            sprite = Resources.Load<Sprite>(path);
            SpriteCache[path] = sprite;
        }

        return sprite;
    }

    /// <summary>
    /// 获取卡背图。
    /// </summary>
    public static Sprite GetCardBack()
    {
        const string path = "UI/Card/CardBack";
        if (!SpriteCache.TryGetValue(path, out Sprite sprite))
        {
            sprite = Resources.Load<Sprite>(path);
            SpriteCache[path] = sprite;
        }
        return sprite;
    }

    /// <summary>
    /// 获取卡牌主题色。
    /// </summary>
    public static Color GetAccentColor(Card card)
    {
        if (card == null) return new Color(0.85f, 0.75f, 0.45f, 1f);

        // 优先使用 ReEndTheme 配色
        var reEnd = ReEndThemeManager.Current;
        if (reEnd != null)
        {
            switch (card.CardType)
            {
                case "Sha": return reEnd.efRed;
                case "Shan": return reEnd.efBlue;
                case "Tao": return reEnd.efGreen;
                default: return reEnd.primary;
            }
        }

        if (theme != null)
        {
            switch (card.CardType)
            {
                case "Sha": return theme.shaAccent;
                case "Shan": return theme.shanAccent;
                case "Tao": return theme.taoAccent;
                default: return theme.primaryColor;
            }
        }

        // 无主题时的默认配色
        switch (card.CardType)
        {
            case "Sha": return new Color(0.84f, 0.23f, 0.24f, 1f);
            case "Shan": return new Color(0.28f, 0.63f, 0.97f, 1f);
            case "Tao": return new Color(0.34f, 0.76f, 0.44f, 1f);
            default: return new Color(0.85f, 0.75f, 0.45f, 1f);
        }
    }
}
