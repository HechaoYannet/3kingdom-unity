Shader "ReEnd/UI/MatrixDot"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _DotColor ("Dot Color", Color) = (1, 0.83, 0.16, 1)
        _DotInactiveColor ("Inactive Color", Color) = (0.6, 0.6, 0.6, 0.1)
        _DotColumns ("Columns", Range(2, 40)) = 20
        _DotRows ("Rows", Range(2, 40)) = 10
        _DotSize ("Dot Size", Range(0.01, 0.2)) = 0.06
        _DutyCycle ("Duty Cycle", Range(0.05, 0.5)) = 0.05
        [Toggle] _Static ("Static Mode", Float) = 0

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
            Name "MatrixDot"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Assets/ReEndUnity/Shaders/Include/ReEndCommon.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _DotColor;
                float4 _DotInactiveColor;
                float _DotColumns;
                float _DotRows;
                float _DotSize;
                float _DutyCycle;
                float _Static;
            CBUFFER_END

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
                int cols = max(2, (int)_DotColumns);
                int rows = max(2, (int)_DotRows);

                float cellW = 1.0 / cols;
                float cellH = 1.0 / rows;
                float2 cellIdx = float2(floor(uv.x / cellW), floor(uv.y / cellH));
                float2 cellUV = frac(float2(uv.x / cellW, uv.y / cellH));

                float2 cellCenter = cellUV - 0.5;
                float dist = length(cellCenter) * 2.0;

                // ── 时间相干状态 ────────────────────────────
                // 每个单元基于位置哈希有独立的振荡周期和相位
                float cellHash = hashRand(cellIdx);
                float cellPhase = hashRand(cellIdx + float2(0.5, 0.5));
                float cellPeriod = lerp(2.0, 8.0, cellHash); // 2-8 秒周期
                float cellOffset = cellPhase * cellPeriod;

                // 正弦振荡产生平滑的 on/off 过渡
                float timeVal = _Static > 0.5 ? 0.0 : _Time.y;
                float oscillation = sin((timeVal + cellOffset) / cellPeriod * 6.28318) * 0.5 + 0.5;

                // 占空比控制：cell 在 oscillation > (1-_DutyCycle) 时激活
                float threshold = 1.0 - _DutyCycle;
                float activeTarget = smoothstep(threshold - 0.05, threshold + 0.05, oscillation);

                // ── 绘制圆点 ──────────────────────────────────
                float dot = 1.0 - smoothstep(_DotSize - 0.01, _DotSize + 0.01, dist);

                float dotAlpha = dot * lerp(_DotInactiveColor.a, _DotColor.a, activeTarget);
                float3 dotRgb = lerp(_DotInactiveColor.rgb, _DotColor.rgb, activeTarget);

                color.rgb = lerp(color.rgb, dotRgb, dotAlpha);
                color.a = dotAlpha;

                return color;
            }
            ENDHLSL
        }
    }

    Fallback "UI/Default"
}
