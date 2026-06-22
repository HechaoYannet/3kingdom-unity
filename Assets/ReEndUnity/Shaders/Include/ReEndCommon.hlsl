#ifndef REEND_COMMON_INCLUDED
#define REEND_COMMON_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

CBUFFER_START(UnityPerMaterial)
    float4 _MainTex_TexelSize;
    float4 _Color;
    float _CornerSize;
    float _GlowRadius;
    float4 _GlowColor;
    float _ScanlineOpacity;
    float _ScanlineSpacing;
    float _GridSize;
    float _GridWidth;
    float4 _GridColor;
    float _BracketSize;
    float _BracketWidth;
    float4 _BracketColor;
    float _ClipSoftness;
CBUFFER_END

TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// ── Shared vertex-to-fragment struct ──
struct Attributes
{
    float4 positionOS : POSITION;
    float2 uv : TEXCOORD0;
    float4 color : COLOR;
};

struct Varyings
{
    float4 positionCS : SV_POSITION;
    float2 uv : TEXCOORD0;
    float4 color : COLOR;
};

// ── Standard UI vertex shader ──
Varyings VertDefault(Attributes input)
{
    Varyings output;
    output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
    output.uv = input.uv;
    output.color = input.color;
    return output;
}

#endif
