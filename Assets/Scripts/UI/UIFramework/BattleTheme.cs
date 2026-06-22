using UnityEngine;

/// <summary>
/// 战斗视觉主题，统一配色、Sprite 资源和视觉风格。
/// </summary>
[CreateAssetMenu(fileName = "BattleTheme", menuName = "ThreeKingdom/Battle Theme")]
public class BattleTheme : ScriptableObject
{
    [Header("背景")]
    public Sprite backgroundSprite;
    public Color backgroundColor = new Color(0.08f, 0.07f, 0.06f, 1f);

    [Header("卡牌")]
    public Sprite cardFrameSprite;
    public Sprite cardBackSprite;
    public Sprite cardArtMaskSprite;

    [Header("HUD")]
    public Sprite hudPanelSprite;
    public Sprite hpBarBackgroundSprite;
    public Sprite hpBarFillSprite;

    [Header("按钮")]
    public Sprite buttonNormalSprite;
    public Sprite buttonHoverSprite;
    public Sprite buttonPressedSprite;

    [Header("配色方案")]
    public Color primaryColor = new Color(0.96f, 0.78f, 0.28f, 1f);
    public Color secondaryColor = new Color(0.88f, 0.82f, 0.64f, 1f);
    public Color backgroundColor2 = new Color(0.08f, 0.07f, 0.06f, 1f);
    public Color playerColor = new Color(0.28f, 0.72f, 0.44f, 1f);
    public Color enemyColor = new Color(0.84f, 0.34f, 0.24f, 1f);
    public Color warningColor = new Color(0.92f, 0.46f, 0.16f, 1f);
    public Color successColor = new Color(0.28f, 0.72f, 0.44f, 1f);
    public Color textPrimaryColor = Color.white;
    public Color textSecondaryColor = new Color(0.7f, 0.7f, 0.7f, 1f);

    [Header("卡牌类型配色")]
    public Color shaAccent = new Color(0.84f, 0.34f, 0.24f, 1f);
    public Color shanAccent = new Color(0.24f, 0.52f, 0.84f, 1f);
    public Color taoAccent = new Color(0.28f, 0.72f, 0.44f, 1f);

    /// <summary>
    /// 从 ReEndTheme 设计令牌更新配色方案。
    /// </summary>
    public void ApplyReEndTheme(ReEndUnity.ReEndTheme reEnd)
    {
        if (reEnd == null) return;

        primaryColor = reEnd.primary;
        secondaryColor = reEnd.efYellowDark;
        backgroundColor = reEnd.background;
        backgroundColor2 = reEnd.surface0;
        playerColor = reEnd.efGreen;
        enemyColor = reEnd.efRed;
        warningColor = reEnd.efOrange;
        successColor = reEnd.efGreen;
        textPrimaryColor = reEnd.textPrimary;
        textSecondaryColor = reEnd.textSecondary;

        shaAccent = reEnd.efRed;
        shanAccent = reEnd.efBlue;
        taoAccent = reEnd.efGreen;
    }
}
