Shader "ReEnd/UI/Scanline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _ScanlineOpacity ("Scanline Opacity", Range(0, 0.1)) = 0.015
        _ScanlineSpacing ("Scanline Spacing", Range(10, 400)) = 120
        [Toggle] _DarkOverlay ("Dark Overlay (Light Mode)", Float) = 0

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
            Name "Scanline"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Assets/ReEndUnity/Shaders/Include/ReEndCommon.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float _ScanlineOpacity;
                float _ScanlineSpacing;
                float _DarkOverlay;
            CBUFFER_END

            Varyings Vert(Attributes input)
            {
                return VertDefault(input);
            }

            half4 Frag(Varyings input) : SV_Target
            {
                // 细密扫描线：模拟 CRT/终端效果
                float phase = frac(input.uv.y * _ScanlineSpacing);
                float darkLine = (1.0 - smoothstep(0.35, 0.5, phase)) * smoothstep(0.0, 0.15, phase);

                // 暗色模式：白色扫描线（web rgba(255,255,255,0.015)）
                // 亮色模式：黑色扫描线（web rgba(0,0,0,0.04)）
                float3 overlayColor = lerp(float3(1, 1, 1), float3(0, 0, 0), _DarkOverlay);
                float overlayAlpha = _ScanlineOpacity * darkLine;
                return half4(overlayColor, overlayAlpha);
            }
            ENDHLSL
        }
    }

    Fallback "UI/Default"
}
