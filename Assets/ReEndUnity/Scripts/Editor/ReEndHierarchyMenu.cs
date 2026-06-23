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
            if (canvas != null) return canvas.transform;
            var go = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            go.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            Undo.RegisterCreatedObjectUndo(go, "Create Canvas");
            return go.transform;
        }

        [MenuItem("GameObject/ReEnd/Button", false, 10)]
        private static void CreateButton() { var go = ReEndUI.Button(Selection.activeTransform ?? GetOrCreateCanvas(), "Button").gameObject; Undo.RegisterCreatedObjectUndo(go, "Create ReEnd Button"); }

        [MenuItem("GameObject/ReEnd/Badge", false, 10)]
        private static void CreateBadge() { var go = ReEndUI.Badge(Selection.activeTransform ?? GetOrCreateCanvas(), "Badge").gameObject; Undo.RegisterCreatedObjectUndo(go, "Create ReEnd Badge"); }

        [MenuItem("GameObject/ReEnd/Label", false, 10)]
        private static void CreateLabel() { var go = ReEndUI.Label(Selection.activeTransform ?? GetOrCreateCanvas(), "Label").gameObject; Undo.RegisterCreatedObjectUndo(go, "Create ReEnd Label"); }

        [MenuItem("GameObject/ReEnd/Input", false, 10)]
        private static void CreateInput() { var go = ReEndUI.Input(Selection.activeTransform ?? GetOrCreateCanvas()).gameObject; Undo.RegisterCreatedObjectUndo(go, "Create ReEnd Input"); }

        [MenuItem("GameObject/ReEnd/Card", false, 10)]
        private static void CreateCard() { var go = ReEndUI.Card(Selection.activeTransform ?? GetOrCreateCanvas()).SetWidth(300).SetHeight(200).gameObject; Undo.RegisterCreatedObjectUndo(go, "Create ReEnd Card"); }

        [MenuItem("GameObject/ReEnd/TacticalPanel", false, 10)]
        private static void CreateTacticalPanel() { var go = ReEndUI.TacticalPanel(Selection.activeTransform ?? GetOrCreateCanvas()).SetWidth(400).SetHeight(280).gameObject; Undo.RegisterCreatedObjectUndo(go, "Create ReEnd TacticalPanel"); }

        [MenuItem("GameObject/ReEnd/Dialog", false, 10)]
        private static void CreateDialog() { var go = ReEndUI.Dialog(Selection.activeTransform ?? GetOrCreateCanvas(), "Dialog").SetSize(ReEndDialogSize.Md).SetMessage("This is a dialog message.").gameObject; Undo.RegisterCreatedObjectUndo(go, "Create ReEnd Dialog"); }

        [MenuItem("GameObject/ReEnd/Progress", false, 10)]
        private static void CreateProgress() { var go = ReEndUI.Progress(Selection.activeTransform ?? GetOrCreateCanvas()).SetWidth(200).gameObject; Undo.RegisterCreatedObjectUndo(go, "Create ReEnd Progress"); }

        [MenuItem("GameObject/ReEnd/Avatar", false, 10)]
        private static void CreateAvatar() { var go = ReEndUI.Avatar(Selection.activeTransform ?? GetOrCreateCanvas()).gameObject; Undo.RegisterCreatedObjectUndo(go, "Create ReEnd Avatar"); }

        [MenuItem("GameObject/ReEnd/Switch", false, 10)]
        private static void CreateSwitch() { var go = ReEndUI.Switch(Selection.activeTransform ?? GetOrCreateCanvas()).gameObject; Undo.RegisterCreatedObjectUndo(go, "Create ReEnd Switch"); }

        [MenuItem("GameObject/ReEnd/Checkbox", false, 10)]
        private static void CreateCheckbox() { var go = ReEndUI.Checkbox(Selection.activeTransform ?? GetOrCreateCanvas()).gameObject; Undo.RegisterCreatedObjectUndo(go, "Create ReEnd Checkbox"); }

        [MenuItem("GameObject/ReEnd/Alert", false, 10)]
        private static void CreateAlert() { var go = ReEndUI.Alert(Selection.activeTransform ?? GetOrCreateCanvas(), "This is an alert message.").gameObject; Undo.RegisterCreatedObjectUndo(go, "Create ReEnd Alert"); }

        [MenuItem("GameObject/ReEnd/Stat", false, 10)]
        private static void CreateStat() { var go = ReEndUI.Stat(Selection.activeTransform ?? GetOrCreateCanvas()).gameObject; Undo.RegisterCreatedObjectUndo(go, "Create ReEnd Stat"); }

        [MenuItem("GameObject/ReEnd/Separator", false, 10)]
        private static void CreateSeparator() { var go = ReEndUI.Separator(Selection.activeTransform ?? GetOrCreateCanvas()).gameObject; Undo.RegisterCreatedObjectUndo(go, "Create ReEnd Separator"); }

        [MenuItem("GameObject/ReEnd/Tabs", false, 10)]
        private static void CreateTabs() { var go = ReEndUI.Tabs(Selection.activeTransform ?? GetOrCreateCanvas()).SetTabs(new() { "Tab 1", "Tab 2", "Tab 3" }).gameObject; Undo.RegisterCreatedObjectUndo(go, "Create ReEnd Tabs"); }

        [MenuItem("GameObject/ReEnd/DiamondLoader", false, 10)]
        private static void CreateDiamondLoader() { var go = ReEndUI.DiamondLoader(Selection.activeTransform ?? GetOrCreateCanvas()).gameObject; Undo.RegisterCreatedObjectUndo(go, "Create ReEnd DiamondLoader"); }

        [MenuItem("GameObject/ReEnd/GlitchText", false, 10)]
        private static void CreateGlitchText() { var go = ReEndUI.GlitchText(Selection.activeTransform ?? GetOrCreateCanvas(), "GLITCH").gameObject; Undo.RegisterCreatedObjectUndo(go, "Create ReEnd GlitchText"); }

        [MenuItem("GameObject/ReEnd/HUDOverlay", false, 10)]
        private static void CreateHUDOverlay() { var go = ReEndUI.HUDOverlay("ENDFIELD::OPS").gameObject; Undo.RegisterCreatedObjectUndo(go, "Create ReEnd HUDOverlay"); }

        [MenuItem("GameObject/ReEnd/Theme", false, 50)]
        private static void CreateTheme()
        {
            const string path = "Assets/ReEndUnity/Resources/ReEndTheme-Dark.asset";
            if (AssetDatabase.LoadAssetAtPath<ReEndTheme>(path) != null)
            {
                Debug.LogWarning("[ReEnd] Theme asset already exists at " + path);
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<ReEndTheme>(path);
                return;
            }
            var theme = ScriptableObject.CreateInstance<ReEndTheme>();
            AssetDatabase.CreateAsset(theme, path);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = theme;
        }
    }
}
