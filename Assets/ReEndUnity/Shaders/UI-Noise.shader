Shader "ReEnd/UI/Noise"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _NoiseOpacity ("Noise Opacity", Range(0, 0.15)) = 0.025
        _NoiseScale ("Noise Scale", Range(0.2, 10)) = 2
        _NoiseSpeed ("Noise Speed", Range(0, 0.05)) = 0.0
        _NoiseOctaves ("Noise Octaves", Range(1, 5)) = 4

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
            Name "Noise"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Assets/ReEndUnity/Shaders/Include/ReEndCommon.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float _NoiseOpacity;
                float _NoiseScale;
                float _NoiseSpeed;
                float _NoiseOctaves;
            CBUFFER_END

            // ── Hash & fractal noise ──────────────────────────
            float hash1D(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }

            // 2D value noise with bilinear interpolation
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

            // 模拟 feTurbulence fractalNoise
            float fractalNoise(float2 uv, float time)
            {
                float v = 0.0;
                float amp = 1.0;
                float freq = 1.0;
                float total = 0.0;
                int octaves = (int)_NoiseOctaves;

                for (int i = 0; i < 5; i++)
                {
                    if (i >= octaves) break;
                    float2 p = uv * _NoiseScale * freq + time * (i + 1) * 0.3;
                    v += valueNoise(p) * amp;
                    total += amp;
                    amp *= 0.5;
                    freq *= 2.0;
                }
                return v / total;
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
                float time = _Time.y * _NoiseSpeed;

                // 生成噪点纹理（模拟 SVG feTurbulence）
                float grain = fractalNoise(uv, time);
                // 让噪点均值偏移到 0.5 附近，叠加时不会过分改变颜色
                float overlay = (grain - 0.5) * _NoiseOpacity;

                color.rgb = saturate(color.rgb + overlay);

                clip(color.a - 0.001);
                return color;
            }
            ENDHLSL
        }
    }

    Fallback "UI/Default"
}
