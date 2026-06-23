#ifndef REEND_COMMON_INCLUDED
#define REEND_COMMON_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

// ── 共享纹理声明 ──
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

// ── 共享顶点/片元结构 ──
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

// ── 标准 UI 顶点着色器 ──
Varyings VertDefault(Attributes input)
{
    Varyings output;
    output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
    output.uv = input.uv;
    output.color = input.color;
    return output;
}

#endif
