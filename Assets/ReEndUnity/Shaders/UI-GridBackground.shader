Shader "ReEnd/UI/GridBackground"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _GridColor ("Grid Color", Color) = (1, 1, 1, 0.03)
        _GridSize ("Grid Size", Range(2, 50)) = 10
        _GridWidth ("Grid Line Width", Range(0.001, 0.05)) = 0.005

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
            CBUFFER_END

            Varyings Vert(Attributes input)
            {
                return VertDefault(input);
            }

            half4 Frag(Varyings input) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                half4 color = tex * _Color * input.color;

                // 使用 UV 空间绘制网格（不依赖屏幕分辨率）
                float2 gridUV = input.uv * _GridSize;
                float2 grid = abs(frac(gridUV) - 0.5) * 2.0;

                float lineX = 1.0 - smoothstep(0.0, _GridWidth, grid.x);
                float lineY = 1.0 - smoothstep(0.0, _GridWidth, grid.y);
                float gridLine = max(lineX, lineY);

                color.rgb = lerp(color.rgb, _GridColor.rgb, gridLine * _GridColor.a);
                color.a = max(color.a, gridLine * _GridColor.a);

                return color;
            }
            ENDHLSL
        }
    }

    Fallback "UI/Default"
}
