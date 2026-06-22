using System;
using UnityEngine;

namespace ReEndUnity
{
    [CreateAssetMenu(menuName = "ReEnd/Theme", fileName = "ReEndTheme")]
    public class ReEndTheme : ScriptableObject
    {
        [Header("Base")]
        public Color efBlack          = Hex(0x0A0A0A);
        public Color efBlackSoft      = Hex(0x111111);
        public Color efBlackMuted     = Hex(0x1A1A1A);
        public Color efDarkGray       = Hex(0x222222);
        public Color efGray           = Hex(0x333333);
        public Color efGrayMid        = Hex(0x666666);
        public Color efGrayLight      = Hex(0x999999);
        public Color efWhiteMuted     = Hex(0xCCCCCC);
        public Color efWhiteSoft      = Hex(0xE0E0E0);
        public Color efWhite          = Hex(0xF0F0F0);
        public Color efPureWhite      = Color.white;

        [Header("Accent Colors")]
        public Color efYellow         = Hex(0xFFD429);
        public Color efYellowDark     = Hex(0xC28C00);
        public Color efBlue           = Hex(0x4DA8DA);
        public Color efBlueLight      = Hex(0x6BBFE8);
        public Color efBlueDark       = Hex(0x2A6A8F);
        public Color efCyan           = Hex(0x00E5FF);
        public Color efRed            = Hex(0xFF4757);
        public Color efGreen          = Hex(0x2ED573);
        public Color efOrange         = Hex(0xFFA502);
        public Color efPurple         = Hex(0xA55EEA);
        public Color efLime           = Hex(0xCBFF40);
        public Color efLimeDark       = Hex(0x72C41A);

        [Header("Semantic")]
        public Color background       = Hex(0x0A0A0A);
        public Color foreground       = Hex(0xF0F0F0);
        public Color card             = Hex(0x141414);
        public Color cardForeground   = Hex(0xE0E0E0);
        public Color popover          = Hex(0x1A1A1A);
        public Color popoverForeground= Hex(0xE0E0E0);
        public Color primary          = Hex(0xFFD429);
        public Color primaryForeground= Hex(0x0A0A0A);
        public Color secondary        = Hex(0x1A1A1A);
        public Color secondaryForeground = Hex(0xE0E0E0);
        public Color muted            = Hex(0x222222);
        public Color mutedForeground  = Hex(0x999999);
        public Color accent           = Hex(0x1A1A1A);
        public Color accentForeground = Hex(0xFFD429);
        public Color destructive      = Hex(0xFF4757);
        public Color destructiveForeground = Color.white;

        [Header("Surface")]
        public Color surfaceCanvas    = Hex(0x0A0A0A);
        public Color surface0         = Hex(0x0F0F0F);
        public Color surface1         = Hex(0x141414);
        public Color surface2         = Hex(0x1A1A1A);
        public Color surface3         = Hex(0x222222);
        public Color surfaceHover     = Hex(0x1E1E1E);
        public Color surfaceActive    = Hex(0x252525);

        [Header("Border")]
        public Color borderSubtle     = new Color(1, 1, 1, 0.06f);
        public Color borderDefault    = new Color(1, 1, 1, 0.10f);
        public Color borderStrong     = new Color(1, 1, 1, 0.12f);
        public Color borderAccent     = new Color(1f, 0.83f, 0.16f, 0.4f);
        public Color inputBorder      = new Color(1, 1, 1, 0.10f);
        public Color ring             = Hex(0xFFD429);

        [Header("Text")]
        public Color textPrimary      = Hex(0xF0F0F0);
        public Color textSecondary    = Hex(0xE0E0E0);
        public Color textTertiary     = Hex(0xCCCCCC);
        public Color textMuted        = Hex(0x999999);
        public Color textPlaceholder  = Hex(0x666666);
        public Color textDisabled     = Hex(0x444444);
        public Color textLink         = Hex(0x4DA8DA);
        public Color textAccent       = Hex(0xFFD429);
        public Color textError        = Hex(0xFF4757);
        public Color textSuccess      = Hex(0x2ED573);

        [Header("Shadow & Glow")]
        public Color shadowSm         = new Color(0, 0, 0, 0.3f);
        public Color shadowMd         = new Color(0, 0, 0, 0.4f);
        public Color shadowLg         = new Color(0, 0, 0, 0.5f);
        public Color glowPrimary      = new Color(1f, 0.83f, 0.16f, 0.2f);
        public Color glowPrimaryStrong= new Color(1f, 0.83f, 0.16f, 0.3f);
        public Color glowLime         = new Color(0.8f, 1f, 0.25f, 0.2f);

        [Header("Soft Colors")]
        public Color redSoft          = new Color(1, 0.28f, 0.34f, 0.1f);
        public Color greenSoft        = new Color(0.18f, 0.84f, 0.45f, 0.1f);
        public Color orangeSoft       = new Color(1, 0.65f, 0.01f, 0.1f);

        [Header("Spacing")]
        public float space1  = 4;
        public float space2  = 8;
        public float space3  = 12;
        public float space4  = 16;
        public float space5  = 20;
        public float space6  = 24;
        public float space8  = 32;
        public float space10 = 40;
        public float space12 = 48;
        public float space16 = 64;
        public float space20 = 80;
        public float space24 = 96;
        public float space32 = 128;

        [Header("Corner")]
        public float clipCornerSm = 8;
        public float clipCornerMd = 12;
        public float clipCornerLg = 16;
        public float bracketSize  = 24;
        public float bracketWidth = 2;
        public Color bracketColor = new Color(1f, 0.83f, 0.16f, 0.4f);

        [Header("Radius")]
        public float radiusNone = 0;
        public float radiusSm   = 2;
        public float radiusMd   = 4;

        [Header("Animation")]
        public float durationInstant = 0.1f;
        public float durationFast    = 0.15f;
        public float durationNormal  = 0.3f;
        public float durationSlow    = 0.5f;
        public float durationSlower  = 0.8f;

        [Header("Opacity")]
        public float opacityOverlay  = 0.85f;
        public float opacityHover    = 0.08f;
        public float opacityDisabled = 0.38f;
        public float opacityBorder   = 0.12f;
        public float opacityFocus    = 0.24f;
        public float opacityMuted    = 0.5f;

        [Header("Layout")]
        public float headerHeight    = 64;
        public float sidebarWidth    = 280;
        public float containerMax    = 1280;
        public float containerPadding = 24;

        [Header("Icon Sizes")]
        public float iconXs  = 16;
        public float iconSm  = 20;
        public float iconMd  = 24;
        public float iconLg  = 32;
        public float iconXl  = 48;
        public float iconXxl = 64;

        [Header("Type Scale")]
        public float displayXlSize = 72;
        public float displayLgSize = 56;
        public float h1Size  = 48;
        public float h2Size  = 36;
        public float h3Size  = 28;
        public float h4Size  = 22;
        public float bodyLgSize = 18;
        public float bodySize   = 16;
        public float bodySmSize = 14;
        public float captionSize = 12;
        public float overlineSize = 11;

        [Header("Chart Colors")]
        public Color chart1 = Hex(0xFFD429);
        public Color chart2 = Hex(0x00E5FF);
        public Color chart3 = Hex(0xA55EEA);
        public Color chart4 = Hex(0x2ED573);
        public Color chart5 = Hex(0xFFA502);
        public Color chart6 = Hex(0x4DA8DA);
        public Color chart7 = Hex(0xFF4757);
        public Color chart8 = Hex(0x7EC8E3);

        [Header("Background")]
        public float bgGridSize = 60;
        public Color bgGridColor = new Color(1, 1, 1, 0.03f);

        public float GetSpace(int level)
        {
            return level switch
            {
                1 => space1, 2 => space2, 3 => space3, 4 => space4,
                5 => space5, 6 => space6, 8 => space8, 10 => space10,
                12 => space12, 16 => space16, 20 => space20, 24 => space24,
                32 => space32, _ => level
            };
        }

        public float GetCornerSize(ReEndCornerSize size) => size switch
        {
            ReEndCornerSize.Sm => clipCornerSm,
            ReEndCornerSize.Md => clipCornerMd,
            ReEndCornerSize.Lg => clipCornerLg,
            _ => 0
        };

        public Color GetStatusColor(ReEndStatus status) => status switch
        {
            ReEndStatus.Success => efGreen,
            ReEndStatus.Warning => efOrange,
            ReEndStatus.Danger => efRed,
            ReEndStatus.Info => efBlue,
            ReEndStatus.Online => efGreen,
            ReEndStatus.Offline => efGrayMid,
            _ => efYellow
        };

        public Color GetSeverityColor(ReEndSeverity severity) => severity switch
        {
            ReEndSeverity.Info => efBlue,
            ReEndSeverity.Success => efGreen,
            ReEndSeverity.Warning => efOrange,
            ReEndSeverity.Error => efRed,
            ReEndSeverity.Caution => efYellow,
            ReEndSeverity.Alert => efOrange,
            ReEndSeverity.Critical => efRed,
            _ => efYellow
        };

        public Color GetAccentColor(ReEndAccentColor accent) => accent switch
        {
            ReEndAccentColor.Yellow => efYellow,
            ReEndAccentColor.Lime => efLime,
            ReEndAccentColor.Blue => efBlue,
            ReEndAccentColor.Cyan => efCyan,
            ReEndAccentColor.Red => efRed,
            ReEndAccentColor.Green => efGreen,
            ReEndAccentColor.Orange => efOrange,
            ReEndAccentColor.Purple => efPurple,
            _ => efYellow
        };

        public float GetSizeValue(ReEndSize size) => size switch
        {
            ReEndSize.Xs => 24,
            ReEndSize.Sm => 32,
            ReEndSize.Md => 40,
            ReEndSize.Lg => 48,
            ReEndSize.Xl => 56,
            _ => 40
        };

        public float GetFontSize(ReEndSize size) => size switch
        {
            ReEndSize.Xs => captionSize,
            ReEndSize.Sm => bodySmSize,
            ReEndSize.Md => bodySize,
            ReEndSize.Lg => bodyLgSize,
            ReEndSize.Xl => h4Size,
            _ => bodySize
        };

        private static Color Hex(uint hex)
        {
            float r = ((hex >> 16) & 0xFF) / 255f;
            float g = ((hex >> 8) & 0xFF) / 255f;
            float b = (hex & 0xFF) / 255f;
            return new Color(r, g, b, 1f);
        }
    }
}
