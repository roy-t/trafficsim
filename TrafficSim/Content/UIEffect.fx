﻿#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_5_0
	#define PS_SHADERMODEL ps_5_0
#else
	#define VS_SHADERMODEL vs_5_0
	#define PS_SHADERMODEL ps_5_0
#endif

float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 InverseViewProjection;

float Index;
float Contrast;
float Channels;

Texture2DArray Texture;
sampler textureSampler = sampler_state
{
    Texture = (Texture);
    AddressU = CLAMP;
    AddressV = CLAMP;
    MagFilter = POINT;
    MinFilter = POINT;
    Mipfilter = POINT;
};

struct VertexShaderInput
{
    float3 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
    float4 Color : COLOR0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float2 TexCoord : TEXCOORD0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;

    float4 worldPosition = mul(float4(input.Position.xyz, 1), World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
    output.TexCoord = input.TexCoord;
    output.Color = input.Color;

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR0
{
    if (Channels == 1)
    {
        float color = Texture.Sample(textureSampler, float3(input.TexCoord, Index), 0).r * input.Color.r;
        color = pow(color, Contrast);
        return float4(color, color, color, 1.0f);
    }

    float4 colors = Texture.Sample(textureSampler, float3(input.TexCoord, Index), 0) * input.Color;
    colors.r = pow(colors.r, Contrast);
    colors.g = pow(colors.g, Contrast);
    colors.b = pow(colors.b, Contrast);

    return float4(colors);
}


technique UIEffect
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
}

