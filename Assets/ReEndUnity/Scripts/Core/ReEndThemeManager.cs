using UnityEngine;

namespace ReEndUnity
{
    [DefaultExecutionOrder(-100)]
    public class ReEndThemeManager : MonoBehaviour
    {
        private static ReEndThemeManager _instance;
        public static ReEndThemeManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<ReEndThemeManager>();
                    if (_instance == null)
                    {
                        var go = new GameObject("[ReEnd ThemeManager]");
                        go.hideFlags = HideFlags.HideAndDontSave;
                        _instance = go.AddComponent<ReEndThemeManager>();
                    }
                }
                return _instance;
            }
        }

        [SerializeField] private ReEndTheme _theme;
        public ReEndTheme Theme
        {
            get
            {
                if (_theme == null)
                    _theme = Resources.Load<ReEndTheme>("ReEndTheme-Dark");
                if (_theme == null)
                    Debug.LogError("[ReEndUnity] Failed to load ReEndTheme-Dark from Resources. Ensure the asset exists.");
                return _theme;
            }
        }

        public static ReEndTheme Current => Instance.Theme;

        public event System.Action<ReEndTheme> OnThemeChanged;

        public void SetTheme(ReEndTheme newTheme)
        {
            _theme = newTheme;
            OnThemeChanged?.Invoke(_theme);
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
            OnThemeChanged = null;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutoInit()
        {
            _ = Instance; // 确保单例存在，Theme 懒加载
        }
    }
}
