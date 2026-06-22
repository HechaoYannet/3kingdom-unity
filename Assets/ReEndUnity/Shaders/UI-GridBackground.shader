Shader "ReEnd/UI/GridBackground"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _GridColor ("Grid Color", Color) = (1, 1, 1, 0.03)
        _GridSize ("Grid Size", Range(10, 200)) = 60
        _GridWidth ("Grid Line Width", Range(0.5, 4)) = 1

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

            Varyings Vert(Attributes input)
            {
                return VertDefault(input);
            }

            half4 Frag(Varyings input) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                half4 color = tex * _Color * input.color;

                float2 screenUV = input.positionCS.xy;
                float2 grid = abs(frac(screenUV / _GridSize) - 0.5) * 2.0;

                float lineX = 1.0 - smoothstep(0, _GridWidth, grid.x * _GridSize);
                float lineY = 1.0 - smoothstep(0, _GridWidth, grid.y * _GridSize);
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
