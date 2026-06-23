Shader "ReEnd/UI/Glitch"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _GlitchInterval ("Glitch Interval", Range(0.5, 10)) = 3
        _GlitchDuration ("Glitch Duration", Range(0.01, 0.5)) = 0.1
        _GlitchOffsetX ("Glitch Offset X", Range(0, 20)) = 3
        _GlitchOffsetY ("Glitch Offset Y", Range(0, 10)) = 1
        _GlitchColor1 ("Glitch Color 1 (Top)", Color) = (0, 0.9, 1, 0.4)
        _GlitchColor2 ("Glitch Color 2 (Bottom)", Color) = (1, 0.28, 0.34, 0.3)

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
            Name "Glitch"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Assets/ReEndUnity/Shaders/Include/ReEndCommon.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float _GlitchInterval;
                float _GlitchDuration;
                float _GlitchOffsetX;
                float _GlitchOffsetY;
                float4 _GlitchColor1;
                float4 _GlitchColor2;
            CBUFFER_END

            // ── 简易随机噪声 ──────────────────────────────────
            float hashRand(float2 seed)
            {
                return frac(sin(dot(seed, float2(12.9898, 78.233))) * 43758.5453);
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
                float time = _Time.y;

                // ── 故障时序 ──────────────────────────────────
                // 使用 hash 生成随机故障触发（基于时间 + UV）
                float intervalTime = floor(time / _GlitchInterval);
                float phaseTime = frac(time / _GlitchInterval);
                float glitchActive = step(phaseTime, _GlitchDuration);

                // 随机 UV 偏移
                float offsetX = (hashRand(float2(intervalTime, 0.0)) - 0.5) * _GlitchOffsetX * 0.01;
                float offsetY = (hashRand(float2(intervalTime, 1.0)) - 0.5) * _GlitchOffsetY * 0.01;

                // 仅故障激活时偏移
                float2 glitchUV = uv + float2(offsetX, offsetY) * glitchActive;

                // ── 分层切片 ──────────────────────────────────
                // CSS inset(20% 0 40% 0) → y in [0.2, 0.6] = middle 40%
                float cyanSlice = step(uv.y, 0.6) * step(0.2, uv.y) * glitchActive;
                // CSS inset(40% 0 20% 0) → y in [0.4, 0.8]
                float redSlice = step(uv.y, 0.8) * step(0.4, uv.y) * glitchActive;

                // 固定方向偏移：青左，红右
                float2 cyanUV = uv - float2(_GlitchOffsetX * 0.008 * glitchActive, 0);
                float2 redUV = uv + float2(_GlitchOffsetX * 0.006 * glitchActive, 0);
                half4 cyanTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, cyanUV);
                half4 redTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, redUV);

                color.rgb = lerp(color.rgb, cyanTex.rgb * _GlitchColor1.rgb, cyanSlice * _GlitchColor1.a);
                color.rgb = lerp(color.rgb, redTex.rgb * _GlitchColor2.rgb, redSlice * _GlitchColor2.a);

                // 故障激活时轻微增加整体偏移
                half4 glitchTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, glitchUV);
                color.rgb = lerp(color.rgb, glitchTex.rgb, glitchActive * 0.3);

                return color;
            }
            ENDHLSL
        }
    }

    Fallback "UI/Default"
}
