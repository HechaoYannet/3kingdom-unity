Shader "ReEnd/UI/FrequencyBar"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _BarColor ("Bar Color", Color) = (1, 0.83, 0.16, 1)
        _BarCount ("Bar Count", Range(2, 32)) = 12
        _BarWidth ("Bar Width", Range(0.3, 1)) = 0.7
        _BarSpeed ("Bar Speed", Range(0.1, 3)) = 0.8
        _BarMinScale ("Min Scale", Range(0.05, 0.5)) = 0.15
        _BarMaxScale ("Max Scale", Range(0.5, 1)) = 1
        _BarGlow ("Bar Glow", Range(0, 0.3)) = 0.05
        _BarStagger ("Bar Stagger", Range(0.05, 0.5)) = 0.13

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
            Name "FrequencyBar"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Assets/ReEndUnity/Shaders/Include/ReEndCommon.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _BarColor;
                float _BarCount;
                float _BarWidth;
                float _BarSpeed;
                float _BarMinScale;
                float _BarMaxScale;
                float _BarGlow;
                float _BarStagger;
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
                int barCount = max(2, (int)_BarCount);

                // ── 列索引 ────────────────────────────────────
                float barIndex = floor(uv.x * barCount);
                float barCenter = (barIndex + 0.5) / barCount;
                float barHalfWidth = _BarWidth / (barCount * 2.0);

                // 当前 UV 是否在柱子范围内
                float inBar = step(abs(uv.x - barCenter), barHalfWidth);

                // ── 高度动画 ──────────────────────────────────
                // 每个柱子独立的相位偏移（交错运动）
                float phase = frac(barIndex * _BarStagger + _Time.y * (1.0 / _BarSpeed));
                // 正弦波模拟缩放 (0 → 1 → 0 → ...)
                float wave = sin(phase * 3.14159);
                float scale = lerp(_BarMinScale, _BarMaxScale, wave);

                // 柱子从底部 (uv.y = 0) 向上延伸
                float barTop = scale;
                float inHeight = step(uv.y, barTop);

                // ── 合并 ──────────────────────────────────────
                float barMask = inBar * inHeight;

                // 顶部发光
                float topGlow = inBar * smoothstep(barTop - _BarGlow, barTop, uv.y) * _BarGlow * 10.0;

                float alpha = max(barMask, topGlow) * _BarColor.a;
                color.rgb = lerp(color.rgb, _BarColor.rgb, alpha);
                color.a = alpha;

                return color;
            }
            ENDHLSL
        }
    }

    Fallback "UI/Default"
}
