using UnityEngine;

namespace ReEndUnity
{
    public static class ReEndBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            // 仅确保 ThemeManager 单例存在，Theme 资产懒加载
            // 不在此处调用 SetTheme —— 避免无订阅者时浪费地触发 OnThemeChanged
            _ = ReEndThemeManager.Instance;
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        private static void EditorInitialize()
        {
            EnsureDefaultTheme();
        }

        private static void EnsureDefaultTheme()
        {
            const string path = "Assets/ReEndUnity/Resources/ReEndTheme-Dark.asset";

            if (UnityEditor.AssetDatabase.LoadAssetAtPath<ReEndTheme>(path) != null)
                return;

            if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/ReEndUnity"))
            {
                Debug.LogWarning("[ReEndUnity] Assets/ReEndUnity folder not found — skipping default theme creation.");
                return;
            }

            if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/ReEndUnity/Resources"))
                UnityEditor.AssetDatabase.CreateFolder("Assets/ReEndUnity", "Resources");

            var theme = ScriptableObject.CreateInstance<ReEndTheme>();
            UnityEditor.AssetDatabase.CreateAsset(theme, path);
            UnityEditor.AssetDatabase.SaveAssets();
            Debug.Log("[ReEnd] Default dark theme created at " + path);
        }
#endif
    }
}
