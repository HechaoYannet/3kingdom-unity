using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity
{
    public abstract class ReEndBaseComponent : MonoBehaviour
    {
        protected ReEndTheme _theme;
        protected bool _built;

        public ReEndTheme Theme => _theme != null ? _theme : (_theme = ReEndThemeManager.Current);

        public RectTransform RectTransform
        {
            get
            {
                EnsureBuilt();
                return (RectTransform)transform;
            }
        }

        public Image BackgroundImage { get; protected set; }

        // ── Per-instance material cache (每个组件实例缓存自己的 Material，避免 ApplyTheme 反复 new 导致泄漏) ──
        private readonly Dictionary<string, Material> _instanceMaterials = new();

        protected Material GetOrCreateMaterial(string shaderName)
        {
            if (!_instanceMaterials.TryGetValue(shaderName, out var mat))
            {
                var shader = Shader.Find(shaderName);
                if (shader == null)
                {
                    Debug.LogError($"[ReEndUnity] Shader not found: {shaderName}");
                    return null;
                }
                mat = new Material(shader);
                _instanceMaterials[shaderName] = mat;
            }
            return mat;
        }

        // ── 事件清理注册 ──
        private readonly List<Action> _cleanupActions = new();
        protected void RegisterCleanup(Action action) => _cleanupActions.Add(action);

        protected virtual void Awake()
        {
            EnsureBuilt();
        }

        protected virtual void OnDestroy()
        {
            foreach (var action in _cleanupActions)
            {
                try { action?.Invoke(); } catch { /* 忽略清理异常 */ }
            }
            _cleanupActions.Clear();
            // 清理本实例缓存的 Material
            foreach (var kvp in _instanceMaterials)
            {
                if (kvp.Value != null) Destroy(kvp.Value);
            }
            _instanceMaterials.Clear();
        }

        public void EnsureBuilt()
        {
            if (!_built)
            {
                _theme = ReEndThemeManager.Current;
                if (_theme == null)
                {
                    Debug.LogError("[ReEndUnity] Theme is null — ensure ReEndTheme-Dark.asset exists in Resources.");
                    _built = true;
                    BuildInternal();
                    return;
                }
                _built = true;
                BuildInternal();
                ApplyTheme();
            }
        }

        public void Rebuild()
        {
            // 先销毁所有子物体，避免重复创建
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            _theme = ReEndThemeManager.Current;
            _built = true;
            BuildInternal();
            if (_theme != null) ApplyTheme();
        }

        public void RefreshTheme()
        {
            _theme = ReEndThemeManager.Current;
            if (_theme != null) ApplyTheme();
        }

        /// <summary>Create child objects and components. Do NOT apply theme here.</summary>
        protected abstract void BuildInternal();

        /// <summary>Apply theme colors, fonts, sizes to already-built children.</summary>
        public abstract void ApplyTheme();

        // ── Chain helpers (return this for fluent API) ──

        public T SetName<T>(string name) where T : ReEndBaseComponent
        {
            gameObject.name = name;
            return (T)this;
        }

        public T SetWidth<T>(float w) where T : ReEndBaseComponent
        {
            EnsureBuilt();
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
            return (T)this;
        }

        public T SetHeight<T>(float h) where T : ReEndBaseComponent
        {
            EnsureBuilt();
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
            return (T)this;
        }

        public T SetSize<T>(float w, float h) where T : ReEndBaseComponent
        {
            EnsureBuilt();
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
            return (T)this;
        }

        public T SetAnchoredPos<T>(float x, float y) where T : ReEndBaseComponent
        {
            EnsureBuilt();
            RectTransform.anchoredPosition = new Vector2(x, y);
            return (T)this;
        }

        public T SetPivot<T>(float x, float y) where T : ReEndBaseComponent
        {
            EnsureBuilt();
            RectTransform.pivot = new Vector2(x, y);
            return (T)this;
        }

        public T SetAnchor<T>(Vector2 min, Vector2 max) where T : ReEndBaseComponent
        {
            EnsureBuilt();
            RectTransform.anchorMin = min;
            RectTransform.anchorMax = max;
            return (T)this;
        }

        public T SetParent<T>(Transform parent) where T : ReEndBaseComponent
        {
            transform.SetParent(parent, false);
            return (T)this;
        }

        // ── Static creation helpers ──

        public static T Create<T>(Transform parent, string name) where T : ReEndBaseComponent
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var comp = go.AddComponent<T>();
            return comp;
        }

        public static T CreateWithImage<T>(Transform parent, string name) where T : ReEndBaseComponent
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            go.transform.SetParent(parent, false);
            var comp = go.AddComponent<T>();
            comp.BackgroundImage = go.GetComponent<Image>();
            return comp;
        }

        protected static GameObject CreateChild(Transform parent, string name)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            return go;
        }

        protected static T CreateChild<T>(Transform parent, string name) where T : Component
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            return go.AddComponent<T>();
        }

        protected static RectTransform Stretch(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            return rt;
        }

        protected static RectTransform CenterAt(RectTransform rt, float width, float height)
        {
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(width, height);
            rt.anchoredPosition = Vector2.zero;
            return rt;
        }
    }
}
