Shader "ReEnd/UI/ClipCorner"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _CornerSize ("Corner Size", Range(0, 0.5)) = 0.12
        _ClipSoftness ("Clip Softness", Range(0.001, 0.05)) = 0.01
        [Toggle] _RTOnly ("RT Only Cut", Float) = 0

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
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
            Name "ClipCorner"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma target 2.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Assets/ReEndUnity/Shaders/Include/ReEndCommon.hlsl"
            #include "Assets/ReEndUnity/Shaders/Include/ReEndSDF.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float _CornerSize;
                float _ClipSoftness;
                float _RTOnly;
            CBUFFER_END

            Varyings Vert(Attributes input)
            {
                return VertDefault(input);
            }

            half4 Frag(Varyings input) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                half4 color = tex * _Color * input.color;

                float ar = ComputeAspectRatio(input.uv);

                // 切角模式：RT+LB (默认) 或 RT-only (头像)
                float d = lerp(sdCutCornerRT_LB(input.uv, _CornerSize, ar),
                               sdCutCornerRT(input.uv, _CornerSize, ar), _RTOnly);

                // SDF：正值 = 内部（可见），负值 = 外部（裁剪）
                float alpha = smoothstep(0.0, _ClipSoftness, d);
                color.a *= alpha;

                clip(color.a - 0.001);
                return color;
            }
            ENDHLSL
        }
    }

    Fallback "UI/Default"
}
