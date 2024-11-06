float4x4 World;
float4x4 View;
float4x4 Projection;

float4 AmbientColor;
float AmbientIntensity;

float3 DiffuseLightDirection;
float4 DiffuseColor;
float DiffuseIntensity;

float3 FillerLightDirection;
float FillerLightIntensity;
float3 BackLightDirection;
float3 BackLightDirection2;
float BackLightIntensity;

texture ModelTexture;
sampler2D textureSampler = sampler_state {
    Texture = (ModelTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float2 TextureCoordinate : TEXCOORD0;
};

 

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    float4 normal = normalize(mul(input.Normal, World));
    float4 primaryColor = saturate(DiffuseColor * DiffuseIntensity * dot(normal, DiffuseLightDirection));
    float4 fillerColor = saturate(DiffuseColor * FillerLightIntensity * dot(normal, FillerLightDirection));
    float4 backColor = saturate(DiffuseColor * BackLightIntensity * dot(normal, BackLightDirection));
    float4 backColor2 = saturate(DiffuseColor * BackLightIntensity * dot(normal, BackLightDirection2));

    output.Color = saturate(primaryColor + fillerColor + backColor + backColor2);

    output.TextureCoordinate = input.TextureCoordinate;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);
    textureColor.a = 1;

    return saturate(textureColor * (input.Color + AmbientColor * AmbientIntensity));
}

technique Diffuse
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}