Shader "ReEnd/UI/Diamond"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _DiamondColor ("Diamond Color", Color) = (1, 0.83, 0.16, 0.25)
        _DiamondSize ("Diamond Size", Range(0.02, 0.3)) = 0.08
        _DiamondCount ("Diamond Count", Range(1, 20)) = 3
        _DiamondSpacing ("Diamond Spacing", Range(0.05, 0.5)) = 0.2
        _DiamondRotate ("Diamond Rotation", Range(0, 45)) = 0
        _DiamondSoftness ("Diamond Softness", Range(0.001, 0.02)) = 0.003

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
            Name "Diamond"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Assets/ReEndUnity/Shaders/Include/ReEndCommon.hlsl"
            #include "Assets/ReEndUnity/Shaders/Include/ReEndSDF.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _DiamondColor;
                float _DiamondSize;
                float _DiamondCount;
                float _DiamondSpacing;
                float _DiamondRotate;
                float _DiamondSoftness;
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
                int count = max(1, (int)_DiamondCount);

                // ── 计算最近的菱形 ──────────────────────────────
                float bestDist = 1e10;
                float totalWidth = (count - 1) * _DiamondSpacing;
                float startX = 0.5 - totalWidth * 0.5;

                for (int i = 0; i < 20; i++)
                {
                    if (i >= count) break;
                    float2 centerPos = float2(startX + i * _DiamondSpacing, 0.5);

                    // 旋转 UV 围绕菱形中心
                    float rad = radians(_DiamondRotate + _Time.y * 20.0 * (i % 3 == 0 ? 1.0 : -1.0));
                    float s = sin(rad);
                    float c = cos(rad);
                    float2 local = uv - centerPos;
                    float2 rotatedUV = float2(c * local.x - s * local.y, s * local.x + c * local.y) + centerPos;

                    float d = sdDiamond(rotatedUV, centerPos, _DiamondSize);
                    bestDist = min(bestDist, d);
                }

                // 负值 = 内部（标准 SDF 约定）
                float diamond = smoothstep(_DiamondSoftness, 0.0, bestDist);

                color.rgb = lerp(color.rgb, _DiamondColor.rgb, diamond * _DiamondColor.a);
                color.a = diamond * _DiamondColor.a;

                return color;
            }
            ENDHLSL
        }
    }

    Fallback "UI/Default"
}
