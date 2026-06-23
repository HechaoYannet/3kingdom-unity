using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ReEndUnity.Editor
{
    public class ReEndEffectTunerWindow : EditorWindow
    {
        private GameObject _target;

        // ── 按 shader 分组的 Image 列表 ──
        private readonly Dictionary<string, List<Image>> _shaderGroups = new();
        private readonly List<string> _shaderOrder = new();

        // ── 每组 foldout 状态 ──
        private readonly Dictionary<string, bool> _foldouts = new();

        // ── 参数缓存（按 shader+property 唯一） ──
        private readonly Dictionary<string, object> _paramCache = new();

        private Vector2 _scrollPos;

        // ── Shader 中文标签 ──
        private static readonly Dictionary<string, string> ShaderLabels = new()
        {
            { "ReEnd/UI/ClipCorner",      "切角 ClipCorner" },
            { "ReEnd/UI/Glow",            "发光 Glow" },
            { "ReEnd/UI/CornerBracket",   "四角括号 CornerBracket" },
            { "ReEnd/UI/GridBackground",  "网格背景 GridBackground" },
            { "ReEnd/UI/Scanline",        "扫描线 Scanline" },
            { "ReEnd/UI/MatrixDot",       "矩阵点阵 MatrixDot" },
            { "ReEnd/UI/GradientLine",    "渐变线 GradientLine" },
            { "ReEnd/UI/HoloSweep",       "全息扫光 HoloSweep" },
            { "ReEnd/UI/FrequencyBar",    "频谱柱 FrequencyBar" },
            { "ReEnd/UI/Diamond",         "菱形 Diamond" },
            { "ReEnd/UI/Glass",           "毛玻璃 Glass" },
            { "ReEnd/UI/Noise",           "噪声 Noise" },
            { "ReEnd/UI/Glitch",          "故障 Glitch" },
            { "ReEnd/UI/RadialGlow",      "径向发光 RadialGlow" },
            { "ReEnd/UI/TopoContour",     "等高线 TopoContour" },
        };

        // ── 属性元数据 ──
        private class PropMeta
        {
            public string Name;
            public string Display;
            public string Type; // "Range", "Color", "Float", "Toggle"
            public float Min;
            public float Max;
        }

        // ── 每个 shader 的可调属性列表（排除通用属性） ──
        private static readonly Dictionary<string, PropMeta[]> ShaderProps = new()
        {
            ["ReEnd/UI/ClipCorner"] = new[]
            {
                P("_CornerSize", "Corner Size", "Range", 0f, 0.5f),
                P("_ClipSoftness", "Clip Softness", "Range", 0.001f, 0.05f),
                P("_RTOnly", "RT Only Cut", "Toggle", 0, 1),
            },
            ["ReEnd/UI/Glow"] = new[]
            {
                P("_GlowColor", "Glow Color", "Color"),
                P("_GlowRadius", "Glow Radius", "Range", 0f, 0.5f),
                P("_GlowFalloff", "Glow Falloff", "Range", 0.1f, 2f),
                P("_GlowPulse", "Glow Pulse", "Range", 0f, 0.5f),
                P("_GlowPulseSpeed", "Pulse Speed", "Range", 0.1f, 5f),
            },
            ["ReEnd/UI/CornerBracket"] = new[]
            {
                P("_BracketColor", "Bracket Color", "Color"),
                P("_BracketSize", "Bracket Size", "Range", 0.05f, 0.5f),
                P("_BracketWidth", "Bracket Width", "Range", 0.002f, 0.05f),
                P("_FourCorners", "Four Corners", "Toggle", 0, 1),
            },
            ["ReEnd/UI/GridBackground"] = new[]
            {
                P("_GridColor", "Grid Color", "Color"),
                P("_GridSize", "Grid Size", "Range", 10f, 120f),
                P("_GridWidth", "Grid Line Width", "Range", 0.001f, 0.03f),
                P("_GridMajorScale", "Major Grid Scale", "Range", 2f, 10f),
                P("_DiagonalOpacity", "Diagonal Opacity", "Range", 0f, 0.15f),
                P("_AspectRatio", "Aspect Ratio (W/H)", "Float"),
            },
            ["ReEnd/UI/Scanline"] = new[]
            {
                P("_ScanlineOpacity", "Scanline Opacity", "Range", 0f, 0.1f),
                P("_ScanlineSpacing", "Scanline Spacing", "Range", 10f, 400f),
                P("_DarkOverlay", "Dark Overlay", "Toggle", 0, 1),
            },
            ["ReEnd/UI/MatrixDot"] = new[]
            {
                P("_DotColor", "Dot Color", "Color"),
                P("_DotInactiveColor", "Inactive Color", "Color"),
                P("_DotColumns", "Columns", "Range", 4f, 80f),
                P("_DotRows", "Rows", "Range", 4f, 80f),
                P("_DotSize", "Dot Size", "Range", 0.01f, 0.3f),
                P("_DutyCycle", "Duty Cycle", "Range", 0.01f, 0.5f),
                P("_Static", "Static", "Toggle", 0, 1),
            },
            ["ReEnd/UI/GradientLine"] = new[]
            {
                P("_LineColor", "Line Color", "Color"),
                P("_GradientSharpness", "Gradient Sharpness", "Range", 0.1f, 2f),
                P("_Direction", "Direction (H↔V)", "Range", 0f, 1f),
            },
            ["ReEnd/UI/HoloSweep"] = new[]
            {
                P("_SweepColor", "Sweep Color", "Color"),
                P("_SweepPosition", "Sweep Position", "Range", -0.2f, 1.2f),
                P("_SweepWidth", "Sweep Width", "Range", 0.01f, 0.3f),
                P("_SweepOpacity", "Sweep Opacity", "Range", 0f, 1f),
                P("_ShimmerOpacity", "Shimmer Opacity", "Range", 0f, 1f),
                P("_ShimmerAngle", "Shimmer Angle", "Range", 0f, 360f),
            },
            ["ReEnd/UI/FrequencyBar"] = new[]
            {
                P("_BarColor", "Bar Color", "Color"),
                P("_BarCount", "Bar Count", "Range", 4f, 64f),
                P("_BarWidth", "Bar Width", "Range", 0.1f, 1f),
                P("_BarSpeed", "Bar Speed", "Range", 0.1f, 5f),
                P("_BarMinScale", "Min Scale", "Range", 0f, 0.5f),
                P("_BarMaxScale", "Max Scale", "Range", 0.5f, 1f),
                P("_BarGlow", "Bar Glow", "Range", 0f, 0.3f),
                P("_BarStagger", "Bar Stagger", "Range", 0f, 0.5f),
            },
            ["ReEnd/UI/Diamond"] = new[]
            {
                P("_DiamondColor", "Diamond Color", "Color"),
                P("_DiamondSize", "Diamond Size", "Range", 0.02f, 0.5f),
                P("_DiamondCount", "Diamond Count", "Range", 1f, 20f),
                P("_DiamondSpacing", "Spacing", "Range", 0f, 0.3f),
                P("_DiamondRotate", "Rotation", "Range", 0f, 360f),
                P("_DiamondSoftness", "Edge Softness", "Range", 0.001f, 0.1f),
            },
            ["ReEnd/UI/Glass"] = new[]
            {
                P("_GlassColor", "Glass Color", "Color"),
                P("_GlassNoiseStrength", "Noise Strength", "Range", 0f, 0.1f),
                P("_GlassNoiseScale", "Noise Scale", "Range", 1f, 50f),
                P("_BorderColor", "Border Color", "Color"),
                P("_BorderWidth", "Border Width", "Range", 0f, 0.05f),
                P("_CornerSize", "Corner Size", "Range", 0f, 0.5f),
                P("_EdgeSoftness", "Edge Softness", "Range", 0f, 0.1f),
            },
            ["ReEnd/UI/Noise"] = new[]
            {
                P("_NoiseOpacity", "Noise Opacity", "Range", 0f, 0.15f),
                P("_NoiseScale", "Noise Scale", "Range", 0.2f, 10f),
                P("_NoiseSpeed", "Noise Speed", "Range", 0f, 0.05f),
                P("_NoiseOctaves", "Noise Octaves", "Range", 1f, 5f),
            },
            ["ReEnd/UI/Glitch"] = new[]
            {
                P("_GlitchInterval", "Glitch Interval", "Range", 0.5f, 10f),
                P("_GlitchDuration", "Glitch Duration", "Range", 0.01f, 0.5f),
                P("_GlitchOffsetX", "Offset X", "Range", 0f, 20f),
                P("_GlitchOffsetY", "Offset Y", "Range", 0f, 10f),
                P("_GlitchColor1", "Color 1 (Cyan)", "Color"),
                P("_GlitchColor2", "Color 2 (Red)", "Color"),
            },
            ["ReEnd/UI/RadialGlow"] = new[]
            {
                P("_GlowColor", "Glow Color", "Color"),
                P("_GlowCenterX", "Center X", "Range", 0f, 1f),
                P("_GlowCenterY", "Center Y", "Range", 0f, 1f),
                P("_GlowRadiusX", "Radius X", "Range", 0.05f, 1f),
                P("_GlowRadiusY", "Radius Y", "Range", 0.05f, 1f),
                P("_GlowFalloff", "Falloff", "Range", 0.1f, 3f),
                P("_GlowPulse", "Pulse", "Range", 0f, 0.5f),
            },
            ["ReEnd/UI/TopoContour"] = new[]
            {
                P("_ContourColor", "Contour Color", "Color"),
                P("_ContourMajorColor", "Major Contour Color", "Color"),
                P("_ContourLevels", "Contour Levels", "Range", 4f, 32f),
                P("_ContourWidth", "Contour Width", "Range", 0.001f, 0.05f),
                P("_ContourScale", "Noise Scale", "Range", 0.5f, 4f),
                P("_ContourSpeed", "Scroll Speed", "Range", 0f, 0.5f),
                P("_NoiseSeed", "Noise Seed", "Range", 0f, 100f),
            },
        };

        private static PropMeta P(string name, string display, string type, float min = 0, float max = 1)
            => new() { Name = name, Display = display, Type = type, Min = min, Max = max };

        // ── 属性缓存 key ──
        private static string CKey(string shader, string prop) => $"{shader}::{prop}";

        [MenuItem("Window/ReEnd Effect Tuner")]
        public static void Open()
        {
            var w = GetWindow<ReEndEffectTunerWindow>("ReEnd Effect Tuner");
            w.minSize = new Vector2(340, 480);
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
            _shaderGroups.Clear();
            _shaderOrder.Clear();

            if (_target == null) return;

            var images = _target.GetComponentsInChildren<Image>(true);
            foreach (var img in images)
            {
                if (img.material == null) continue;
                var shaderName = img.material.shader != null ? img.material.shader.name : "";
                if (!ShaderProps.ContainsKey(shaderName)) continue;

                if (!_shaderGroups.ContainsKey(shaderName))
                {
                    _shaderGroups[shaderName] = new List<Image>();
                    _shaderOrder.Add(shaderName);
                }
                _shaderGroups[shaderName].Add(img);

                // 从首个材质读取参数缓存
                if (_shaderGroups[shaderName].Count == 1)
                    ReadParams(shaderName, img.material);
            }
        }

        private void ReadParams(string shaderName, Material mat)
        {
            if (!ShaderProps.TryGetValue(shaderName, out var props)) return;
            foreach (var p in props)
            {
                if (!mat.HasProperty(p.Name)) continue;
                var key = CKey(shaderName, p.Name);
                if (p.Type == "Color")
                    _paramCache[key] = mat.GetColor(p.Name);
                else
                    _paramCache[key] = mat.GetFloat(p.Name);
            }
        }

        private void ApplyAll()
        {
            foreach (var shaderName in _shaderOrder)
            {
                if (!_shaderGroups.TryGetValue(shaderName, out var imgs)) continue;
                if (!ShaderProps.TryGetValue(shaderName, out var props)) continue;

                foreach (var img in imgs)
                {
                    if (img == null || img.material == null) continue;
                    foreach (var p in props)
                    {
                        var key = CKey(shaderName, p.Name);
                        if (!_paramCache.TryGetValue(key, out var val)) continue;
                        if (p.Type == "Color")
                            img.material.SetColor(p.Name, (Color)val);
                        else
                            img.material.SetFloat(p.Name, (float)val);
                    }
                }
            }
        }

        // ── GUI ──

        private void OnGUI()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            EditorGUILayout.LabelField("ReEnd Effect Tuner", EditorStyles.boldLabel);
            EditorGUILayout.Space(4);

            if (_target == null)
            {
                EditorGUILayout.HelpBox("请在 Hierarchy 中选中一个带有 ReEnd 组件的 GameObject。", MessageType.Info);
                EditorGUILayout.EndScrollView();
                return;
            }

            EditorGUILayout.LabelField("选中对象", _target.name, EditorStyles.boldLabel);

            // 统计
            int totalImages = 0;
            foreach (var kvp in _shaderGroups)
                totalImages += kvp.Value.Count;

            if (totalImages == 0)
            {
                EditorGUILayout.HelpBox("未在选中对象上找到 ReEnd shader material。\n请选中包含 ReEnd 组件的 GameObject。", MessageType.Warning);
                EditorGUILayout.EndScrollView();
                return;
            }

            // 摘要
            foreach (var shaderName in _shaderOrder)
            {
                var label = ShaderLabels.TryGetValue(shaderName, out var cn) ? cn : shaderName;
                EditorGUILayout.LabelField(label, $"{_shaderGroups[shaderName].Count} 个");
            }

            EditorGUILayout.Space(8);

            EditorGUI.BeginChangeCheck();

            foreach (var shaderName in _shaderOrder)
            {
                if (!_shaderGroups.TryGetValue(shaderName, out var imgs) || imgs.Count == 0) continue;
                if (!ShaderProps.TryGetValue(shaderName, out var props)) continue;

                var label = ShaderLabels.TryGetValue(shaderName, out var cn) ? cn : shaderName;
                if (!_foldouts.TryGetValue(shaderName, out var show)) show = true;
                show = EditorGUILayout.Foldout(show, $"{label} ({imgs.Count})", true);
                _foldouts[shaderName] = show;

                if (!show) continue;

                EditorGUI.indentLevel++;
                foreach (var p in props)
                {
                    var key = CKey(shaderName, p.Name);
                    if (!_paramCache.TryGetValue(key, out var val))
                    {
                        // 从材质读取默认值
                        if (imgs[0] != null && imgs[0].material != null && imgs[0].material.HasProperty(p.Name))
                        {
                            if (p.Type == "Color")
                                val = imgs[0].material.GetColor(p.Name);
                            else
                                val = imgs[0].material.GetFloat(p.Name);
                            _paramCache[key] = val;
                        }
                        else continue;
                    }

                    if (p.Type == "Color")
                    {
                        var c = (Color)val;
                        c = EditorGUILayout.ColorField(p.Display, c);
                        _paramCache[key] = c;
                    }
                    else if (p.Type == "Range")
                    {
                        var f = (float)val;
                        f = EditorGUILayout.Slider(p.Display, f, p.Min, p.Max);
                        _paramCache[key] = f;
                    }
                    else if (p.Type == "Toggle")
                    {
                        var f = (float)val;
                        f = EditorGUILayout.Toggle(p.Display, f > 0.5f) ? 1f : 0f;
                        _paramCache[key] = f;
                    }
                    else // Float
                    {
                        var f = (float)val;
                        f = EditorGUILayout.FloatField(p.Display, f);
                        _paramCache[key] = f;
                    }
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.Space(4);
            }

            if (EditorGUI.EndChangeCheck())
            {
                ApplyAll();
                EditorUtility.SetDirty(_target);
                SceneView.RepaintAll();
            }

            EditorGUILayout.Space(8);

            // 底部按钮
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
            EditorGUILayout.HelpBox("拖动滑块实时预览效果。\n点击「保存到 Theme」将当前值写入 ReEndTheme 资产。", MessageType.Info);

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

            // ClipCorner
            if (_paramCache.TryGetValue(CKey("ReEnd/UI/ClipCorner", "_CornerSize"), out var cs))
            {
                var v = (float)cs;
                theme.clipCornerSm = v * 0.667f;
                theme.clipCornerMd = v;
                theme.clipCornerLg = v * 1.333f;
            }

            // CornerBracket
            if (_paramCache.TryGetValue(CKey("ReEnd/UI/CornerBracket", "_BracketSize"), out var bs))
                theme.bracketSize = (float)bs;
            if (_paramCache.TryGetValue(CKey("ReEnd/UI/CornerBracket", "_BracketWidth"), out var bw))
                theme.bracketWidth = (float)bw;
            if (_paramCache.TryGetValue(CKey("ReEnd/UI/CornerBracket", "_BracketColor"), out var bc))
                theme.bracketColor = (Color)bc;

            // Glow
            if (_paramCache.TryGetValue(CKey("ReEnd/UI/Glow", "_GlowColor"), out var gc))
            {
                theme.glowPrimary = (Color)gc;
                theme.glowPrimaryStrong = new Color(((Color)gc).r, ((Color)gc).g, ((Color)gc).b, Mathf.Min(((Color)gc).a * 1.5f, 1f));
            }
            if (_paramCache.TryGetValue(CKey("ReEnd/UI/Glow", "_GlowRadius"), out var gr))
                theme.glowRadius = (float)gr;

            // GridBackground
            if (_paramCache.TryGetValue(CKey("ReEnd/UI/GridBackground", "_GridColor"), out var gridc))
                theme.bgGridColor = (Color)gridc;
            if (_paramCache.TryGetValue(CKey("ReEnd/UI/GridBackground", "_GridSize"), out var gs))
                theme.bgGridSize = (float)gs;
            if (_paramCache.TryGetValue(CKey("ReEnd/UI/GridBackground", "_GridWidth"), out var gw))
                theme.bgGridWidth = (float)gw;

            // Scanline
            if (_paramCache.TryGetValue(CKey("ReEnd/UI/Scanline", "_ScanlineOpacity"), out var so))
                theme.scanlineOpacity = (float)so;
            if (_paramCache.TryGetValue(CKey("ReEnd/UI/Scanline", "_ScanlineSpacing"), out var ss))
                theme.scanlineSpacing = (float)ss;

            EditorUtility.SetDirty(theme);
            AssetDatabase.SaveAssets();
            Debug.Log("[ReEndEffectTuner] 参数已保存到 Theme。");
        }
    }
}
