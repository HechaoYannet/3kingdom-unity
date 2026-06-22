using UnityEngine;

namespace ReEndUnity
{
    public static class ReEndBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            var theme = Resources.Load<ReEndTheme>("ReEndTheme-Dark");
            if (theme != null)
                ReEndThemeManager.Instance.SetTheme(theme);
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
