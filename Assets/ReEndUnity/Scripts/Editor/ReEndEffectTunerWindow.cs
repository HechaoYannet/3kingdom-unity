using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity.Editor
{
    public class ReEndEffectTunerWindow : EditorWindow
    {
        private GameObject _target;
        private readonly List<Image> _clipCornerImages = new();
        private readonly List<Image> _glowImages = new();
        private readonly List<Image> _bracketImages = new();
        private readonly List<Image> _gridImages = new();
        private readonly List<Image> _scanlineImages = new();

        private bool _showClip = true;
        private bool _showGlow = true;
        private bool _showBracket = true;
        private bool _showGrid = true;
        private bool _showScanline = true;

        // 缓存当前参数值
        private float _cornerSize = 0.12f;
        private float _clipSoftness = 0.01f;
        private Color _glowColor = new(1f, 0.83f, 0.16f, 0.3f);
        private float _glowRadius = 0.1f;
        private Color _bracketColor = new(1f, 0.83f, 0.16f, 0.4f);
        private float _bracketSize = 0.15f;
        private float _bracketWidth = 0.01f;
        private Color _gridColor = new(1f, 1f, 1f, 0.03f);
        private float _gridSize = 10f;
        private float _gridWidth = 0.005f;
        private float _scanlineOpacity = 0.1f;
        private float _scanlineSpacing = 20f;

        private Vector2 _scrollPos;

        [MenuItem("Window/ReEnd Effect Tuner")]
        public static void Open()
        {
            var w = GetWindow<ReEndEffectTunerWindow>("ReEnd Effect Tuner");
            w.minSize = new Vector2(320, 480);
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;
            OnSelectionChanged();
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= OnSelectionChanged;
        }

        private void OnSelectionChanged()
        {
            _target = Selection.activeGameObject;
            ScanForShaders();
            Repaint();
        }

        private void ScanForShaders()
        {
            _clipCornerImages.Clear();
            _glowImages.Clear();
            _bracketImages.Clear();
            _gridImages.Clear();
            _scanlineImages.Clear();

            if (_target == null) return;

            var images = _target.GetComponentsInChildren<Image>(true);
            foreach (var img in images)
            {
                if (img.material == null) continue;
                var shaderName = img.material.shader != null ? img.material.shader.name : "";
                switch (shaderName)
                {
                    case "ReEnd/UI/ClipCorner":
                        _clipCornerImages.Add(img);
                        ReadClipCornerParams(img.material);
                        break;
                    case "ReEnd/UI/Glow":
                        _glowImages.Add(img);
                        ReadGlowParams(img.material);
                        break;
                    case "ReEnd/UI/CornerBracket":
                        _bracketImages.Add(img);
                        ReadBracketParams(img.material);
                        break;
                    case "ReEnd/UI/GridBackground":
                        _gridImages.Add(img);
                        ReadGridParams(img.material);
                        break;
                    case "ReEnd/UI/Scanline":
                        _scanlineImages.Add(img);
                        ReadScanlineParams(img.material);
                        break;
                }
            }
        }

        // ── 读取当前 material 参数 ──

        private void ReadClipCornerParams(Material mat)
        {
            if (mat.HasProperty("_CornerSize")) _cornerSize = mat.GetFloat("_CornerSize");
            if (mat.HasProperty("_ClipSoftness")) _clipSoftness = mat.GetFloat("_ClipSoftness");
        }
        private void ReadGlowParams(Material mat)
        {
            if (mat.HasProperty("_GlowColor")) _glowColor = mat.GetColor("_GlowColor");
            if (mat.HasProperty("_GlowRadius")) _glowRadius = mat.GetFloat("_GlowRadius");
        }
        private void ReadBracketParams(Material mat)
        {
            if (mat.HasProperty("_BracketColor")) _bracketColor = mat.GetColor("_BracketColor");
            if (mat.HasProperty("_BracketSize")) _bracketSize = mat.GetFloat("_BracketSize");
            if (mat.HasProperty("_BracketWidth")) _bracketWidth = mat.GetFloat("_BracketWidth");
        }
        private void ReadGridParams(Material mat)
        {
            if (mat.HasProperty("_GridColor")) _gridColor = mat.GetColor("_GridColor");
            if (mat.HasProperty("_GridSize")) _gridSize = mat.GetFloat("_GridSize");
            if (mat.HasProperty("_GridWidth")) _gridWidth = mat.GetFloat("_GridWidth");
        }
        private void ReadScanlineParams(Material mat)
        {
            if (mat.HasProperty("_ScanlineOpacity")) _scanlineOpacity = mat.GetFloat("_ScanlineOpacity");
            if (mat.HasProperty("_ScanlineSpacing")) _scanlineSpacing = mat.GetFloat("_ScanlineSpacing");
        }

        // ── 批量更新 material ──

        private void ApplyClipCorner()
        {
            foreach (var img in _clipCornerImages)
            {
                if (img == null || img.material == null) continue;
                img.material.SetFloat("_CornerSize", _cornerSize);
                img.material.SetFloat("_ClipSoftness", _clipSoftness);
            }
        }
        private void ApplyGlow()
        {
            foreach (var img in _glowImages)
            {
                if (img == null || img.material == null) continue;
                img.material.SetColor("_GlowColor", _glowColor);
                img.material.SetFloat("_GlowRadius", _glowRadius);
            }
        }
        private void ApplyBracket()
        {
            foreach (var img in _bracketImages)
            {
                if (img == null || img.material == null) continue;
                img.material.SetColor("_BracketColor", _bracketColor);
                img.material.SetFloat("_BracketSize", _bracketSize);
                img.material.SetFloat("_BracketWidth", _bracketWidth);
            }
        }
        private void ApplyGrid()
        {
            foreach (var img in _gridImages)
            {
                if (img == null || img.material == null) continue;
                img.material.SetColor("_GridColor", _gridColor);
                img.material.SetFloat("_GridSize", _gridSize);
                img.material.SetFloat("_GridWidth", _gridWidth);
            }
        }
        private void ApplyScanline()
        {
            foreach (var img in _scanlineImages)
            {
                if (img == null || img.material == null) continue;
                img.material.SetFloat("_ScanlineOpacity", _scanlineOpacity);
                img.material.SetFloat("_ScanlineSpacing", _scanlineSpacing);
            }
        }

        // ── GUI ──

        private void OnGUI()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            // 目标信息
            EditorGUILayout.LabelField("ReEnd Effect Tuner", EditorStyles.boldLabel);
            EditorGUILayout.Space(4);

            if (_target == null)
            {
                EditorGUILayout.HelpBox("请在 Hierarchy 中选中一个带有 ReEnd 组件的 GameObject。", MessageType.Info);
                EditorGUILayout.EndScrollView();
                return;
            }

            EditorGUILayout.LabelField("选中对象", _target.name, EditorStyles.boldLabel);
            EditorGUILayout.LabelField("ClipCorner", $"{_clipCornerImages.Count} 个");
            EditorGUILayout.LabelField("Glow", $"{_glowImages.Count} 个");
            EditorGUILayout.LabelField("CornerBracket", $"{_bracketImages.Count} 个");
            EditorGUILayout.LabelField("GridBackground", $"{_gridImages.Count} 个");
            EditorGUILayout.LabelField("Scanline", $"{_scanlineImages.Count} 个");

            if (_clipCornerImages.Count == 0 && _glowImages.Count == 0 &&
                _bracketImages.Count == 0 && _gridImages.Count == 0 && _scanlineImages.Count == 0)
            {
                EditorGUILayout.HelpBox("未在选中对象上找到 ReEnd shader material。\n请选中包含 ReEnd 组件（Button、Card、HUDOverlay 等）的 GameObject。", MessageType.Warning);
                EditorGUILayout.EndScrollView();
                return;
            }

            EditorGUILayout.Space(8);

            EditorGUI.BeginChangeCheck();

            // ClipCorner
            if (_clipCornerImages.Count > 0)
            {
                _showClip = EditorGUILayout.Foldout(_showClip, $"切角 ClipCorner ({_clipCornerImages.Count})", true);
                if (_showClip)
                {
                    EditorGUI.indentLevel++;
                    _cornerSize = EditorGUILayout.Slider("Corner Size", _cornerSize, 0f, 0.5f);
                    _clipSoftness = EditorGUILayout.Slider("Clip Softness", _clipSoftness, 0.001f, 0.05f);
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space(4);
                }
            }

            // Glow
            if (_glowImages.Count > 0)
            {
                _showGlow = EditorGUILayout.Foldout(_showGlow, $"发光 Glow ({_glowImages.Count})", true);
                if (_showGlow)
                {
                    EditorGUI.indentLevel++;
                    _glowColor = EditorGUILayout.ColorField("Glow Color", _glowColor);
                    _glowRadius = EditorGUILayout.Slider("Glow Radius", _glowRadius, 0f, 0.5f);
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space(4);
                }
            }

            // CornerBracket
            if (_bracketImages.Count > 0)
            {
                _showBracket = EditorGUILayout.Foldout(_showBracket, $"四角括号 CornerBracket ({_bracketImages.Count})", true);
                if (_showBracket)
                {
                    EditorGUI.indentLevel++;
                    _bracketColor = EditorGUILayout.ColorField("Bracket Color", _bracketColor);
                    _bracketSize = EditorGUILayout.Slider("Bracket Size", _bracketSize, 0.05f, 0.5f);
                    _bracketWidth = EditorGUILayout.Slider("Bracket Width", _bracketWidth, 0.002f, 0.05f);
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space(4);
                }
            }

            // GridBackground
            if (_gridImages.Count > 0)
            {
                _showGrid = EditorGUILayout.Foldout(_showGrid, $"网格背景 GridBackground ({_gridImages.Count})", true);
                if (_showGrid)
                {
                    EditorGUI.indentLevel++;
                    _gridColor = EditorGUILayout.ColorField("Grid Color", _gridColor);
                    _gridSize = EditorGUILayout.Slider("Grid Size", _gridSize, 2f, 50f);
                    _gridWidth = EditorGUILayout.Slider("Grid Width", _gridWidth, 0.001f, 0.05f);
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space(4);
                }
            }

            // Scanline
            if (_scanlineImages.Count > 0)
            {
                _showScanline = EditorGUILayout.Foldout(_showScanline, $"扫描线 Scanline ({_scanlineImages.Count})", true);
                if (_showScanline)
                {
                    EditorGUI.indentLevel++;
                    _scanlineOpacity = EditorGUILayout.Slider("Scanline Opacity", _scanlineOpacity, 0f, 0.5f);
                    _scanlineSpacing = EditorGUILayout.Slider("Scanline Spacing", _scanlineSpacing, 2f, 100f);
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space(4);
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                ApplyClipCorner();
                ApplyGlow();
                ApplyBracket();
                ApplyGrid();
                ApplyScanline();
                EditorUtility.SetDirty(_target);
                SceneView.RepaintAll();
            }

            EditorGUILayout.Space(8);

            // 保存到 Theme
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("保存到 Theme", GUILayout.Height(28)))
            {
                SaveToTheme();
            }
            if (GUILayout.Button("重新扫描", GUILayout.Height(28)))
            {
                ScanForShaders();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(4);
            EditorGUILayout.HelpBox("拖动滑块实时预览效果。\n点击「保存到 Theme」将当前值写入 ReEndTheme 资产，之后所有新创建的组件都会使用这些值。", MessageType.Info);

            EditorGUILayout.EndScrollView();
        }

        private void SaveToTheme()
        {
            var theme = ReEndThemeManager.Current;
            if (theme == null)
            {
                EditorUtility.DisplayDialog("保存失败", "无法加载 ReEndTheme，请确保 Resources/ReEndTheme-Dark.asset 存在。", "OK");
                return;
            }

            Undo.RecordObject(theme, "Save ReEnd Effect Params to Theme");

            theme.clipCornerSm = _cornerSize * 0.667f;
            theme.clipCornerMd = _cornerSize;
            theme.clipCornerLg = _cornerSize * 1.333f;
            theme.bracketSize = _bracketSize;
            theme.bracketWidth = _bracketWidth;
            theme.bracketColor = _bracketColor;

            EditorUtility.SetDirty(theme);
            AssetDatabase.SaveAssets();
            Debug.Log("[ReEndEffectTuner] 参数已保存到 Theme。clipCornerMd=" + theme.clipCornerMd + ", bracketSize=" + theme.bracketSize);
        }
    }
}
