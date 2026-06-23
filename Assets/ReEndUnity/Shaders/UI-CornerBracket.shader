Shader "ReEnd/UI/CornerBracket"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _BracketColor ("Bracket Color", Color) = (1, 0.83, 0.16, 0.4)
        _BracketSize ("Bracket Size", Range(0.05, 0.5)) = 0.15
        _BracketWidth ("Bracket Width", Range(0.002, 0.05)) = 0.01
        [Toggle] _FourCorners ("Four Corners", Float) = 0

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
            Name "CornerBracket"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Assets/ReEndUnity/Shaders/Include/ReEndCommon.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _BracketColor;
                float _BracketSize;
                float _BracketWidth;
                float _FourCorners;
            CBUFFER_END

            Varyings Vert(Attributes input)
            {
                return VertDefault(input);
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float2 uv = input.uv;

                // 按宽高比调整，使括号在非正方形矩形上物理尺寸一致
                float ar = ComputeAspectRatio(input.uv);
                float bsX = ar >= 1.0 ? _BracketSize / ar : _BracketSize;
                float bsY = ar >= 1.0 ? _BracketSize : _BracketSize * ar;
                float bwX = ar >= 1.0 ? _BracketWidth / ar : _BracketWidth;
                float bwY = ar >= 1.0 ? _BracketWidth : _BracketWidth * ar;

                // ── L 形括号：线在矩形边缘，不在括号区域中心 ──
                // smoothstep(0, bw, edge - uv) → 1 at edge, 0 at bw distance from edge

                // 左上角 (TL) ⌜
                float tlH = step(uv.x, bsX) * (1.0 - smoothstep(0.0, bwY, 1.0 - uv.y));
                float tlV = step(1.0 - bsY, uv.y) * (1.0 - smoothstep(0.0, bwX, uv.x));
                float tl = max(tlH, tlV);

                // 右下角 (BR) ⌟
                float brH = step(1.0 - bsX, uv.x) * (1.0 - smoothstep(0.0, bwY, uv.y));
                float brV = step(uv.y, bsY) * (1.0 - smoothstep(0.0, bwX, 1.0 - uv.x));
                float br = max(brH, brV);

                // 右上角 (TR) ⌝ — 4 角模式
                float trH = step(1.0 - bsX, uv.x) * (1.0 - smoothstep(0.0, bwY, 1.0 - uv.y));
                float trV = step(1.0 - bsY, uv.y) * (1.0 - smoothstep(0.0, bwX, 1.0 - uv.x));
                float trC = max(trH, trV) * _FourCorners;

                // 左下角 (BL) ⌞ — 4 角模式
                float blH = step(uv.x, bsX) * (1.0 - smoothstep(0.0, bwY, uv.y));
                float blV = step(uv.y, bsY) * (1.0 - smoothstep(0.0, bwX, uv.x));
                float blC = max(blH, blV) * _FourCorners;

                // Overlay-only: 只渲染括号，不渲染背景纹理
                float bracket = max(max(tl, br), max(trC, blC));
                half4 color;
                color.rgb = _BracketColor.rgb;
                color.a = bracket * _BracketColor.a;

                clip(color.a - 0.001);
                return color;
            }
            ENDHLSL
        }
    }

    Fallback "UI/Default"
}
