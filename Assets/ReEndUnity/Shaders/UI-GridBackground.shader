Shader "ReEnd/UI/GridBackground"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _GridColor ("Grid Color", Color) = (1, 1, 1, 0.03)
        _GridSize ("Grid Size", Range(10, 120)) = 60
        _GridWidth ("Grid Line Width", Range(0.001, 0.03)) = 0.005
        _GridMajorScale ("Major Grid Scale", Range(2, 10)) = 5

        _DiagonalOpacity ("Diagonal Opacity", Range(0, 0.15)) = 0.02
        _AspectRatio ("Aspect Ratio (W/H)", Float) = 1.777

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
            Name "GridBackground"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Assets/ReEndUnity/Shaders/Include/ReEndCommon.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _GridColor;
                float _GridSize;
                float _GridWidth;
                float _GridMajorScale;
                float _DiagonalOpacity;
                float _AspectRatio;
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

                // ── 宽高比校正 ────────────────────────────
                float aspect = max(_AspectRatio, 0.1);
                float2 aspectUV = float2(uv.x * aspect, uv.y);

                // ── 主网格 (Major + Minor) ─────────────────
                float2 gridUV = aspectUV * _GridSize;
                float2 gridFrac = frac(gridUV);

                float2 minorLine;
                minorLine.x = 1.0 - smoothstep(0.0, _GridWidth, min(gridFrac.x, 1.0 - gridFrac.x));
                minorLine.y = 1.0 - smoothstep(0.0, _GridWidth, min(gridFrac.y, 1.0 - gridFrac.y));
                float minorGrid = max(minorLine.x, minorLine.y);

                float2 majorFrac = frac(gridUV / _GridMajorScale);
                float2 majorLine;
                majorLine.x = 1.0 - smoothstep(0.0, _GridWidth * 2.0, min(majorFrac.x, 1.0 - majorFrac.x));
                majorLine.y = 1.0 - smoothstep(0.0, _GridWidth * 2.0, min(majorFrac.y, 1.0 - majorFrac.y));
                float majorGrid = max(majorLine.x, majorLine.y);

                float gridLine = max(minorGrid * 0.6, majorGrid);

                // ── -45° 对角线 ──────────────────────────
                float diag = 0.0;
                if (_DiagonalOpacity > 0.001)
                {
                    float diagPhase = frac((aspectUV.x - aspectUV.y) * _GridSize * 0.707);
                    float diagLine = 1.0 - smoothstep(0.0, _GridWidth * 0.5, min(diagPhase, 1.0 - diagPhase));
                    diag = diagLine * _DiagonalOpacity;
                }

                // ── 合成 ──────────────────────────────────────────
                // 直接输出网格色 + 透明 alpha：非网格区域完全透明，仅网格线可见
                float composite = saturate(gridLine * _GridColor.a + diag);
                color.rgb = _GridColor.rgb;
                color.a = composite;

                return color;
            }
            ENDHLSL
        }
    }

    Fallback "UI/Default"
}
