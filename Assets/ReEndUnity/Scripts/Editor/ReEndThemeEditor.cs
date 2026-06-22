using UnityEditor;
using UnityEngine;

namespace ReEndUnity.Editor
{
    [CustomEditor(typeof(ReEndTheme))]
    public class ReEndThemeEditor : UnityEditor.Editor
    {
        private bool _showBase;
        private bool _showAccent;
        private bool _showSemantic;
        private bool _showSurface;
        private bool _showBorder;
        private bool _showText;
        private bool _showShadow;
        private bool _showSpacing;
        private bool _showAnimation;
        private bool _showType;

        public override void OnInspectorGUI()
        {
            var theme = (ReEndTheme)target;
            serializedObject.Update();

            EditorGUILayout.LabelField("ReEnd Theme Editor", EditorStyles.boldLabel);
            EditorGUILayout.Space(4);

            DrawSection("Base Colors", ref _showBase, "efBlack", "efBlackSoft", "efBlackMuted", "efDarkGray", "efGray", "efGrayMid", "efGrayLight", "efWhiteMuted", "efWhiteSoft", "efWhite", "efPureWhite");
            DrawSection("Accent Colors", ref _showAccent, "efYellow", "efYellowDark", "efBlue", "efBlueLight", "efBlueDark", "efCyan", "efRed", "efGreen", "efOrange", "efPurple", "efLime", "efLimeDark");
            DrawSection("Semantic Colors", ref _showSemantic, "background", "foreground", "card", "cardForeground", "popover", "popoverForeground", "primary", "primaryForeground", "secondary", "secondaryForeground", "muted", "mutedForeground", "accent", "accentForeground", "destructive", "destructiveForeground");
            DrawSection("Surface Colors", ref _showSurface, "surfaceCanvas", "surface0", "surface1", "surface2", "surface3", "surfaceHover", "surfaceActive");
            DrawSection("Border Colors", ref _showBorder, "borderSubtle", "borderDefault", "borderStrong", "borderAccent", "inputBorder", "ring");
            DrawSection("Text Colors", ref _showText, "textPrimary", "textSecondary", "textTertiary", "textMuted", "textPlaceholder", "textDisabled", "textLink", "textAccent", "textError", "textSuccess");
            DrawSection("Shadow & Glow", ref _showShadow, "shadowSm", "shadowMd", "shadowLg", "glowPrimary", "glowPrimaryStrong", "glowLime");
            DrawSection("Spacing", ref _showSpacing, "space1", "space2", "space3", "space4", "space5", "space6", "space8", "space10", "space12", "space16", "space20", "space24", "space32");
            DrawSection("Animation", ref _showAnimation, "durationInstant", "durationFast", "durationNormal", "durationSlow", "durationSlower");
            DrawSection("Type Scale", ref _showType, "displayXlSize", "displayLgSize", "h1Size", "h2Size", "h3Size", "h4Size", "bodyLgSize", "bodySize", "bodySmSize", "captionSize", "overlineSize");

            if (GUI.changed)
            {
                EditorUtility.SetDirty(theme);
                serializedObject.ApplyModifiedProperties();
            }

            EditorGUILayout.Space(12);
            if (GUILayout.Button("Apply Theme to Scene", GUILayout.Height(30)))
            {
                ReEndThemeManager.Instance.SetTheme(theme);
            }
        }

        private void DrawSection(string title, ref bool show, params string[] propNames)
        {
            show = EditorGUILayout.Foldout(show, title, true);
            if (show)
            {
                EditorGUI.indentLevel++;
                foreach (var name in propNames)
                {
                    var prop = serializedObject.FindProperty(name);
                    if (prop != null)
                        EditorGUILayout.PropertyField(prop);
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}
