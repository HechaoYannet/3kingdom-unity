Shader "ReEnd/UI/GradientLine"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _LineColor ("Line Color", Color) = (1, 0.83, 0.16, 0.6)
        _GradientSharpness ("Gradient Sharpness", Range(0.1, 2)) = 0.5
        _Direction ("Direction", Range(0, 1)) = 0

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
            Name "GradientLine"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Assets/ReEndUnity/Shaders/Include/ReEndCommon.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _LineColor;
                float _GradientSharpness;
                float _Direction;
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

                // ── 渐变方向 ──────────────────────────────────
                // _Direction: 0 = 水平 (gradient along X), 1 = 垂直 (gradient along Y)
                float t = lerp(uv.x, uv.y, _Direction);

                // 中心峰值（两端透明 → 中间亮）
                float gradient = 1.0 - abs(t - 0.5) * 2.0;
                gradient = smoothstep(0.0, _GradientSharpness, gradient);

                float alpha = gradient * _LineColor.a;

                color.rgb = lerp(color.rgb, _LineColor.rgb, alpha);
                color.a = alpha;

                return color;
            }
            ENDHLSL
        }
    }

    Fallback "UI/Default"
}
