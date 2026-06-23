Shader "ReEnd/UI/CornerBracket"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _BracketColor ("Bracket Color", Color) = (1, 0.83, 0.16, 0.4)
        _BracketSize ("Bracket Size", Range(0.05, 0.5)) = 0.15
        _BracketWidth ("Bracket Width", Range(0.002, 0.05)) = 0.01

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

                // 所有尺寸均在 UV 空间 [0,1]
                float bs = _BracketSize;
                float bw = _BracketWidth;

                // 四个角的 L 形括号
                // 水平线 + 垂直线组成 L 形
                float tlH = step(uv.x, bs) * step(1.0 - bs, uv.y) * (1.0 - smoothstep(0, bw, abs(uv.y - (1.0 - bs * 0.5))));
                float tlV = step(1.0 - bs, uv.y) * step(uv.x, bs) * (1.0 - smoothstep(0, bw, abs(uv.x - bs * 0.5)));
                float tl = max(tlH, tlV);

                float trH = step(1.0 - bs, uv.x) * step(1.0 - bs, uv.y) * (1.0 - smoothstep(0, bw, abs(uv.y - (1.0 - bs * 0.5))));
                float trV = step(1.0 - bs, uv.y) * step(1.0 - bs, uv.x) * (1.0 - smoothstep(0, bw, abs(uv.x - (1.0 - bs * 0.5))));
                float tr = max(trH, trV);

                float blH = step(uv.x, bs) * step(uv.y, bs) * (1.0 - smoothstep(0, bw, abs(uv.y - bs * 0.5)));
                float blV = step(uv.y, bs) * step(uv.x, bs) * (1.0 - smoothstep(0, bw, abs(uv.x - bs * 0.5)));
                float bl = max(blH, blV);

                float brH = step(1.0 - bs, uv.x) * step(uv.y, bs) * (1.0 - smoothstep(0, bw, abs(uv.y - bs * 0.5)));
                float brV = step(uv.y, bs) * step(1.0 - bs, uv.x) * (1.0 - smoothstep(0, bw, abs(uv.x - (1.0 - bs * 0.5))));
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
