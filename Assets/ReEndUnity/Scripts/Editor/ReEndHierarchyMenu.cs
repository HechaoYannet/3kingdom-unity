using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity.Editor
{
    public static class ReEndHierarchyMenu
    {
        private static Transform GetOrCreateCanvas()
        {
            var canvas = Object.FindAnyObjectByType<Canvas>();
            if (canvas == null)
            {
                var go = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                go.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            }
            return (Object.FindAnyObjectByType<Canvas>() ?? new GameObject("Canvas").AddComponent<Canvas>()).transform;
        }

        [MenuItem("GameObject/ReEnd/Button", false, 10)]
        private static void CreateButton() { ReEndUI.Button(Selection.activeTransform ?? GetOrCreateCanvas(), "Button"); }

        [MenuItem("GameObject/ReEnd/Badge", false, 10)]
        private static void CreateBadge() { ReEndUI.Badge(Selection.activeTransform ?? GetOrCreateCanvas(), "Badge"); }

        [MenuItem("GameObject/ReEnd/Label", false, 10)]
        private static void CreateLabel() { ReEndUI.Label(Selection.activeTransform ?? GetOrCreateCanvas(), "Label"); }

        [MenuItem("GameObject/ReEnd/Input", false, 10)]
        private static void CreateInput() { ReEndUI.Input(Selection.activeTransform ?? GetOrCreateCanvas()); }

        [MenuItem("GameObject/ReEnd/Card", false, 10)]
        private static void CreateCard() { ReEndUI.Card(Selection.activeTransform ?? GetOrCreateCanvas()).SetWidth(300).SetHeight(200); }

        [MenuItem("GameObject/ReEnd/TacticalPanel", false, 10)]
        private static void CreateTacticalPanel() { ReEndUI.TacticalPanel(Selection.activeTransform ?? GetOrCreateCanvas()).SetWidth(400).SetHeight(280); }

        [MenuItem("GameObject/ReEnd/Dialog", false, 10)]
        private static void CreateDialog() { ReEndUI.Dialog(Selection.activeTransform ?? GetOrCreateCanvas(), "Dialog").SetSize(ReEndDialogSize.Md).SetMessage("This is a dialog message."); }

        [MenuItem("GameObject/ReEnd/Progress", false, 10)]
        private static void CreateProgress() { ReEndUI.Progress(Selection.activeTransform ?? GetOrCreateCanvas()).SetWidth(200); }

        [MenuItem("GameObject/ReEnd/Avatar", false, 10)]
        private static void CreateAvatar() { ReEndUI.Avatar(Selection.activeTransform ?? GetOrCreateCanvas()); }

        [MenuItem("GameObject/ReEnd/Switch", false, 10)]
        private static void CreateSwitch() { ReEndUI.Switch(Selection.activeTransform ?? GetOrCreateCanvas()); }

        [MenuItem("GameObject/ReEnd/Checkbox", false, 10)]
        private static void CreateCheckbox() { ReEndUI.Checkbox(Selection.activeTransform ?? GetOrCreateCanvas()); }

        [MenuItem("GameObject/ReEnd/Alert", false, 10)]
        private static void CreateAlert() { ReEndUI.Alert(Selection.activeTransform ?? GetOrCreateCanvas(), "This is an alert message."); }

        [MenuItem("GameObject/ReEnd/Stat", false, 10)]
        private static void CreateStat() { ReEndUI.Stat(Selection.activeTransform ?? GetOrCreateCanvas()); }

        [MenuItem("GameObject/ReEnd/Separator", false, 10)]
        private static void CreateSeparator() { ReEndUI.Separator(Selection.activeTransform ?? GetOrCreateCanvas()); }

        [MenuItem("GameObject/ReEnd/Tabs", false, 10)]
        private static void CreateTabs() { ReEndUI.Tabs(Selection.activeTransform ?? GetOrCreateCanvas()).SetTabs(new() { "Tab 1", "Tab 2", "Tab 3" }); }

        [MenuItem("GameObject/ReEnd/DiamondLoader", false, 10)]
        private static void CreateDiamondLoader() { ReEndUI.DiamondLoader(Selection.activeTransform ?? GetOrCreateCanvas()); }

        [MenuItem("GameObject/ReEnd/GlitchText", false, 10)]
        private static void CreateGlitchText() { ReEndUI.GlitchText(Selection.activeTransform ?? GetOrCreateCanvas(), "GLITCH"); }

        [MenuItem("GameObject/ReEnd/HUDOverlay", false, 10)]
        private static void CreateHUDOverlay() { ReEndUI.HUDOverlay("ENDFIELD::OPS"); }

        [MenuItem("GameObject/ReEnd/Theme", false, 50)]
        private static void CreateTheme()
        {
            var theme = ScriptableObject.CreateInstance<ReEndTheme>();
            AssetDatabase.CreateAsset(theme, "Assets/ReEndTheme.asset");
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = theme;
        }
    }
}
