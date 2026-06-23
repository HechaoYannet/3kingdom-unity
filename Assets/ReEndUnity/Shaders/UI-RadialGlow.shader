Shader "ReEnd/UI/RadialGlow"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _GlowColor ("Glow Color", Color) = (1, 0.83, 0.16, 0.12)
        _GlowCenterX ("Glow Center X", Range(0, 1)) = 0.5
        _GlowCenterY ("Glow Center Y", Range(0, 1)) = 0.0
        _GlowRadiusX ("Glow Radius X", Range(0, 2)) = 0.8
        _GlowRadiusY ("Glow Radius Y", Range(0, 2)) = 0.5
        _GlowFalloff ("Glow Falloff", Range(0.1, 1.5)) = 0.7
        _GlowPulse ("Glow Pulse", Range(0, 0.5)) = 0.0

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
            Name "RadialGlow"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Assets/ReEndUnity/Shaders/Include/ReEndCommon.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _GlowColor;
                float _GlowCenterX;
                float _GlowCenterY;
                float _GlowRadiusX;
                float _GlowRadiusY;
                float _GlowFalloff;
                float _GlowPulse;
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

                // ── 椭圆径向渐变 ──────────────────────────────
                float2 center = float2(_GlowCenterX, _GlowCenterY);
                float2 dist = (uv - center) / float2(_GlowRadiusX, _GlowRadiusY);
                float radialDist = length(dist);

                // 脉冲动画
                float pulse = 1.0 + _GlowPulse * sin(_Time.y * 1.5) * 0.5;

                // 从中心向外的渐变过渡: glow at center → transparent at falloff
                float glow = 1.0 - smoothstep(0.0, _GlowFalloff * pulse, radialDist);
                float glowAlpha = glow * _GlowColor.a;

                color.rgb = lerp(color.rgb, _GlowColor.rgb, glowAlpha);
                color.a = max(color.a, glowAlpha);

                return color;
            }
            ENDHLSL
        }
    }

    Fallback "UI/Default"
}
