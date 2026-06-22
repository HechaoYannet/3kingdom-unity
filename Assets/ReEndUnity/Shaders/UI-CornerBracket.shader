Shader "ReEnd/UI/CornerBracket"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _BracketColor ("Bracket Color", Color) = (1, 0.83, 0.16, 0.4)
        _BracketSize ("Bracket Size", Range(4, 48)) = 24
        _BracketWidth ("Bracket Width", Range(1, 6)) = 2

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
            Name "CornerBracket"

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

                float2 uv = input.uv;
                float2 size = _MainTex_TexelSize.zw;

                // Normalized bracket size in UV space
                float2 bracketUV = _BracketSize / size.xy;

                // Top-left bracket (L-shape)
                float tlH = step(uv.x, bracketUV.x) * step(1.0 - bracketUV.y, uv.y) * step(abs(uv.y - (1.0 - bracketUV.y * 0.5)), _BracketWidth * 0.5 / size.y);
                float tlV = step(uv.y, 1.0) * step(1.0 - bracketUV.y, uv.y) * step(uv.x, bracketUV.x) * step(abs(uv.x - bracketUV.x * 0.5), _BracketWidth * 0.5 / size.x);
                float tl = max(tlH, tlV);

                // Top-right bracket
                float trH = step(1.0 - bracketUV.x, uv.x) * step(1.0 - bracketUV.y, uv.y) * step(abs(uv.y - (1.0 - bracketUV.y * 0.5)), _BracketWidth * 0.5 / size.y);
                float trV = step(1.0 - bracketUV.y, uv.y) * step(1.0 - bracketUV.x, uv.x) * step(abs(uv.x - (1.0 - bracketUV.x * 0.5)), _BracketWidth * 0.5 / size.x);
                float tr = max(trH, trV);

                // Bottom-left bracket
                float blH = step(uv.x, bracketUV.x) * step(uv.y, bracketUV.y) * step(abs(uv.y - bracketUV.y * 0.5), _BracketWidth * 0.5 / size.y);
                float blV = step(uv.y, bracketUV.y) * step(uv.x, bracketUV.x) * step(abs(uv.x - bracketUV.x * 0.5), _BracketWidth * 0.5 / size.x);
                float bl = max(blH, blV);

                // Bottom-right bracket
                float brH = step(1.0 - bracketUV.x, uv.x) * step(uv.y, bracketUV.y) * step(abs(uv.y - bracketUV.y * 0.5), _BracketWidth * 0.5 / size.y);
                float brV = step(uv.y, bracketUV.y) * step(1.0 - bracketUV.x, uv.x) * step(abs(uv.x - (1.0 - bracketUV.x * 0.5)), _BracketWidth * 0.5 / size.x);
                float br = max(brH, brV);

                float bracket = max(max(tl, tr), max(bl, br));
                color.rgb = lerp(color.rgb, _BracketColor.rgb, bracket * _BracketColor.a);
                color.a = max(color.a, bracket * _BracketColor.a);

                return color;
            }
            ENDHLSL
        }
    }

    Fallback "UI/Default"
}
