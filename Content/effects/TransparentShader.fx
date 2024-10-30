float4x4 World;
float4x4 View;
float4x4 Projection;

float4 Color;
float Intensity;

struct VSInput
{
    float4 Position : POSITION0;
};

struct PSInput
{
    float4 Position : POSITION0;
};

PSInput VertexFunction(VSInput input)
{
    PSInput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    return output;
}

float4 PixelFunction(PSInput input) : COLOR0
{
    return Color * Intensity;
}


technique Transparent
{
    pass Pass1
    {
        AlphaBlendEnable = TRUE;
        DestBlend = INVSRCALPHA;
        SrcBlend = SRCALPHA;
        VertexShader = compile vs_3_0 VertexFunction();
        PixelShader = compile ps_3_0 PixelFunction();
    }
}