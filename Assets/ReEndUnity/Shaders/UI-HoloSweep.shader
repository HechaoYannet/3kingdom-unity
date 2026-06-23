Shader "ReEnd/UI/HoloSweep"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _SweepColor ("Sweep Color", Color) = (1, 0.83, 0.16, 0.6)
        _SweepPosition ("Sweep Position", Range(-0.2, 1.2)) = -0.1
        _SweepWidth ("Sweep Width", Range(0.01, 0.3)) = 0.08
        _SweepOpacity ("Sweep Opacity", Range(0, 1)) = 0
        _ShimmerOpacity ("Shimmer Opacity", Range(0, 1)) = 0
        _ShimmerAngle ("Shimmer Angle", Range(0, 360)) = 135

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
            Name "HoloSweep"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Assets/ReEndUnity/Shaders/Include/ReEndCommon.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _SweepColor;
                float _SweepPosition;
                float _SweepWidth;
                float _SweepOpacity;
                float _ShimmerOpacity;
                float _ShimmerAngle;
            CBUFFER_END

            Varyings Vert(Attributes input)
            {
                return VertDefault(input);
            }

            half4 Frag(Varyings input) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                half4 color = tex * _Color * input.color;

                float2 uv = input.uv;

                // ── 水平扫光线 ──────────────────────────────
                float sweepDist = abs(uv.y - _SweepPosition);
                float sweep = 1.0 - smoothstep(0.0, _SweepWidth, sweepDist);
                // 水平渐变淡出 (bg-gradient-to-r: transparent → color → transparent)
                float xFade = 1.0 - abs(uv.x - 0.5) * 2.0;
                sweep *= smoothstep(0.0, 0.3, xFade) * _SweepOpacity;

                // ── 135° 对角微光 ───────────────────────────
                float shimmerRad = _ShimmerAngle * 3.14159 / 180.0;
                float diagProj = dot(uv, float2(cos(shimmerRad), sin(shimmerRad)));
                // 平滑渐变：0%透明 → 40%峰值(0.03) → 60%透明
                float shimmer = 1.0 - smoothstep(0.35, 0.45, abs(diagProj - 0.45));
                shimmer *= _ShimmerOpacity * 0.03;

                // ── 合成 ──────────────────────────────────────
                float totalAlpha = max(sweep, shimmer);
                color.rgb = lerp(color.rgb, _SweepColor.rgb, totalAlpha);
                color.a = totalAlpha * _SweepColor.a;

                return color;
            }
            ENDHLSL
        }
    }

    Fallback "UI/Default"
}
