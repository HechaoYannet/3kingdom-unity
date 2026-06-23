Shader "ReEnd/UI/Glow"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _GlowColor ("Glow Color", Color) = (1, 0.83, 0.16, 0.2)
        _GlowRadius ("Glow Radius", Range(0, 0.5)) = 0.15
        _GlowFalloff ("Glow Falloff", Range(0.1, 2.0)) = 0.5
        _GlowPulse ("Glow Pulse", Range(0, 0.5)) = 0.0
        _GlowPulseSpeed ("Pulse Speed", Range(0.1, 5.0)) = 1.5

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
            Name "Glow"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Assets/ReEndUnity/Shaders/Include/ReEndCommon.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _GlowColor;
                float _GlowRadius;
                float _GlowFalloff;
                float _GlowPulse;
                float _GlowPulseSpeed;
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

                // 脉冲调制
                float pulse = 1.0;
                if (_GlowPulse > 0.001)
                    pulse = 1.0 + _GlowPulse * sin(_Time.y * _GlowPulseSpeed) * 0.5;

                // ── 从边缘向内发光的辉光 ────────────────────
                // 用于更大尺寸的背景 Quad，产生"外发光"效果
                // 在边缘处最亮，向中心 _GlowRadius 距离处衰减为 0
                float edgeDist = min(
                    min(uv.x, 1.0 - uv.x),
                    min(uv.y, 1.0 - uv.y));
                float glow = smoothstep(_GlowRadius * pulse, 0.0, edgeDist);

                float glowAlpha = glow * _GlowColor.a * pulse;

                color.rgb = lerp(color.rgb, _GlowColor.rgb, glowAlpha);
                color.a = max(color.a, glowAlpha);

                clip(color.a - 0.001);
                return color;
            }
            ENDHLSL
        }
    }

    Fallback "UI/Default"
}
