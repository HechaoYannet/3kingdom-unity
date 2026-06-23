Shader "ReEnd/UI/TopoContour"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _ContourColor ("Contour Color", Color) = (1, 1, 1, 0.07)
        _ContourMajorColor ("Major Contour Color", Color) = (1, 1, 1, 0.12)
        _ContourLevels ("Contour Levels", Range(4, 32)) = 16
        _ContourWidth ("Contour Width", Range(0.001, 0.05)) = 0.008
        _ContourScale ("Noise Scale", Range(0.5, 4.0)) = 1.5
        _ContourSpeed ("Scroll Speed", Range(0, 0.5)) = 0.02
        _NoiseSeed ("Noise Seed", Range(0, 100)) = 42

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
            "RenderPipeline"="UniversalPipeline"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "TopoContour"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Assets/ReEndUnity/Shaders/Include/ReEndCommon.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _ContourColor;
                float4 _ContourMajorColor;
                float _ContourLevels;
                float _ContourWidth;
                float _ContourScale;
                float _ContourSpeed;
                float _NoiseSeed;
            CBUFFER_END

            // ── Hash & Noise ──────────────────────────────────────
            float2 hash2D(float2 p)
            {
                p = float2(dot(p, float2(127.1, 311.7)),
                           dot(p, float2(269.5, 183.3)));
                return frac(sin(p) * 43758.5453) * 2.0 - 1.0;
            }

            // 2D value noise
            float noise2D(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                f = f * f * (3.0 - 2.0 * f); // smoothstep

                return lerp(
                    lerp(dot(hash2D(i + float2(0, 0)), f - float2(0, 0)),
                         dot(hash2D(i + float2(1, 0)), f - float2(1, 0)), f.x),
                    lerp(dot(hash2D(i + float2(0, 1)), f - float2(0, 1)),
                         dot(hash2D(i + float2(1, 1)), f - float2(1, 1)), f.x),
                    f.y);
            }

            // 多八度地形噪声 — 模拟自然地形高度场
            float terrainHeight(float2 uv, float time)
            {
                float2 p = uv * _ContourScale;
                float2 seed = float2(_NoiseSeed, _NoiseSeed * 1.7);

                float h = 0.0;
                float amp = 1.0;
                float freq = 1.0;
                float total = 0.0;

                // 6 octaves of noise for organic terrain feel
                for (int i = 0; i < 6; i++)
                {
                    float2 np = p * freq + seed + float2(time * freq * 0.1, -time * freq * 0.07);
                    h += noise2D(np) * amp;
                    total += amp;
                    amp *= 0.5;
                    freq *= 2.0;
                }

                return h / total; // normalize to ~[0,1]
            }

            Varyings Vert(Attributes input)
            {
                return VertDefault(input);
            }

            half4 Frag(Varyings input) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                half4 color = tex * _Color * input.color;

                float2 uv = input.uv;
                float time = _Time.y * _ContourSpeed;

                // ── 生成地形高度 ──────────────────────────────────
                float height = terrainHeight(uv, time);

                // ── 等高线 (isoline) ─────────────────────────────
                // frac(height * levels) → 0..1 重复
                float lines = frac(height * _ContourLevels + 0.5);
                // 三角形波：0 在轮廓线上，1 在线之间
                float tri = abs(lines - 0.5) * 2.0;

                // 细轮廓线
                float contour = 1.0 - smoothstep(0.0, _ContourWidth, tri);

                // 粗轮廓线：每 4 条一条主轮廓
                float majorLines = frac(height * (_ContourLevels / 4.0) + 0.5);
                float majorTri = abs(majorLines - 0.5) * 2.0;
                float majorContour = 1.0 - smoothstep(0.0, _ContourWidth * 2.0, majorTri);

                // ── 合成 ──────────────────────────────────────────
                // 直接输出等高线色 + 透明 alpha：非等高线区域完全透明
                float minorAlpha = contour * (1.0 - majorContour) * _ContourColor.a;
                float majorAlpha = majorContour * _ContourMajorColor.a;

                color.rgb = lerp(_ContourColor.rgb, _ContourMajorColor.rgb, majorAlpha);
                color.a = max(minorAlpha, majorAlpha);

                return color;
            }
            ENDHLSL
        }
    }

    Fallback "UI/Default"
}
