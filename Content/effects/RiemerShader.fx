
float4x4 xWorld;
float4x4 xViewProjection;
Texture xTexture;

sampler TextureSampler = sampler_state { 
    texture = <xTexture>; 
    magfilter = LINEAR; 
    minfilter = LINEAR; 
    mipfilter = LINEAR; 
    AddressU = mirror; 
    AddressV = mirror;
};

float3 xLightPos;
float xLightPower;
float xAmbient;


struct VertexToPixel
{
    float4 Position     : POSITION;
    float2 TexCoords    : TEXCOORD0;
    float3 Normal       : TEXCOORD1;
    float3 Position3D   : TEXCOORD2;
};
struct PixelToFrame
{
    float4 Color        : COLOR0;
};

VertexToPixel SimplestVertexShader(float4 inPos : POSITION, float3 inNormal : NORMAL0, float2 inTexCoords : TEXCOORD0)
{
    VertexToPixel Output = (VertexToPixel)0;

    Output.Position = mul(inPos, xViewProjection);
    Output.Normal = normalize(mul(inNormal, (float3x3)xWorld));
    Output.Position3D = mul(xWorld, inPos);
    Output.TexCoords = inTexCoords;
    
    return Output;
}
float DotProduct(float3 lightPos, float3 pos3D, float3 normal)
{
    float3 lightDir = normalize(pos3D - lightPos);
    return dot(-lightDir, normal);
}


PixelToFrame SimplestPixelShader(VertexToPixel PSIn)
{
    PixelToFrame Output = (PixelToFrame)0;

    float diffuseLightingFactor = DotProduct(xLightPos, PSIn.Position3D, PSIn.Normal);
    diffuseLightingFactor = saturate(diffuseLightingFactor);
    diffuseLightingFactor *= xLightPower;

    float4 baseColor = tex2D(TextureSampler, PSIn.TexCoords);
    Output.Color = baseColor*(diffuseLightingFactor + xAmbient);

    return Output;

}

technique Simplest
{
    pass Pass0
    {
        VertexShader = compile vs_3_0 SimplestVertexShader();
        PixelShader = compile ps_3_0 SimplestPixelShader();
    }
}