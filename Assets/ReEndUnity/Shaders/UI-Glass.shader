Shader "ReEnd/UI/Glass"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _GlassColor ("Glass Color", Color) = (0.078, 0.078, 0.078, 0.55)
        _GlassNoiseStrength ("Noise Strength", Range(0, 0.1)) = 0.015
        _GlassNoiseScale ("Noise Scale", Range(1, 50)) = 12
        _BorderColor ("Border Color", Color) = (1, 1, 1, 0.1)
        _BorderWidth ("Border Width", Range(0, 0.05)) = 0.005
        _CornerSize ("Corner Size", Range(0, 0.5)) = 0.12
        _EdgeSoftness ("Edge Softness", Range(0, 0.1)) = 0.02

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
            Name "Glass"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Assets/ReEndUnity/Shaders/Include/ReEndCommon.hlsl"
            #include "Assets/ReEndUnity/Shaders/Include/ReEndSDF.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _GlassColor;
                float _GlassNoiseStrength;
                float _GlassNoiseScale;
                float4 _BorderColor;
                float _BorderWidth;
                float _CornerSize;
                float _EdgeSoftness;
            CBUFFER_END

            // 简易 2D 哈希噪声
            float hash1D(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }

            float valueNoise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                f = f * f * (3.0 - 2.0 * f);
                return lerp(
                    lerp(hash1D(i), hash1D(i + float2(1, 0)), f.x),
                    lerp(hash1D(i + float2(0, 1)), hash1D(i + float2(1, 1)), f.x),
                    f.y);
            }

            float fbmNoise(float2 p)
            {
                float v = 0.0;
                float amp = 1.0;
                float freq = 1.0;
                for (int i = 0; i < 3; i++)
                {
                    v += valueNoise(p * freq) * amp;
                    amp *= 0.5;
                    freq *= 2.0;
                }
                return v / 1.75;
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

                // ── 毛玻璃底纹 ──────────────────────────────────
                float noise = fbmNoise(uv * _GlassNoiseScale + _Time.y * 0.02);
                float glassAlpha = _GlassColor.a + noise * _GlassNoiseStrength;
                color.rgb = lerp(color.rgb, _GlassColor.rgb, glassAlpha);
                color.a = max(color.a, glassAlpha);

                // ── 定向切角 (RT + LB) ─────────────────────────
                float ar = ComputeAspectRatio(input.uv);
                float d = sdCutCornerRT_LB(uv, _CornerSize, ar);
                float cornerAlpha = smoothstep(0.0, _EdgeSoftness, d);
                color.a *= cornerAlpha;

                // ── 边框 ──────────────────────────────────────
                float borderDist = abs(d);
                float border = 0.0;
                if (d > 0.0)
                {
                    border = 1.0 - smoothstep(0.0, _BorderWidth, borderDist);
                }
                color.rgb = lerp(color.rgb, _BorderColor.rgb, border * _BorderColor.a);
                color.a = max(color.a, border * _BorderColor.a);

                clip(color.a - 0.001);
                return color;
            }
            ENDHLSL
        }
    }

    Fallback "UI/Default"
}
