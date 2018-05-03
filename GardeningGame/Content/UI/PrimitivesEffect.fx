float4 FogVector;
float3 FogColor;
float4x4 World;
float4x4 WorldViewProj;
float3 EyePosition;
float3x3 WorldInverseTranspose;
float3 DirLight0Direction;
    float3 DirLight0DiffuseColor;
    float3 DirLight0SpecularColor;
    float3 DirLight1Direction;
    float3 DirLight1DiffuseColor;
    float3 DirLight1SpecularColor;
    float3 DirLight2Direction;
    float3 DirLight2DiffuseColor;
    float3 DirLight2SpecularColor;
    float4 DiffuseColor;
    float3 EmissiveColor;
    float3 SpecularColor;
    float  SpecularPower;


// Vertex shader output structures.

struct VSOutput
{
    float4 PositionPS : SV_Position;
    float4 Diffuse    : COLOR0;
    float4 Specular   : COLOR1;
};

struct VSInputNmVc
{
    float4 Position;
    float4 Color;
    float3 Normal;
};

struct CommonVSOutput
{
    float4 Pos_ps;
    float4 Diffuse;
    float3 Specular;
    float  FogFactor;
};

struct ColorPair
{
    float3 Diffuse;
    float3 Specular;
};

ColorPair ComputeLights(float3 eyeVector, float3 worldNormal, uniform int numLights)
{
    float3x3 lightDirections = 0;
    float3x3 lightDiffuse = 0;
    float3x3 lightSpecular = 0;
    float3x3 halfVectors = 0;

    for (int i = 0; i < numLights; i++)
    {
        lightDirections[i] = float3x3(DirLight0Direction,     DirLight1Direction,     DirLight2Direction)    [i];
        lightDiffuse[i]    = float3x3(DirLight0DiffuseColor,  DirLight1DiffuseColor,  DirLight2DiffuseColor) [i];
        lightSpecular[i]   = float3x3(DirLight0SpecularColor, DirLight1SpecularColor, DirLight2SpecularColor)[i];
        
        halfVectors[i] = normalize(eyeVector - lightDirections[i]);
    }

    float3 dotL = mul(-lightDirections, worldNormal);
    float3 dotH = mul(halfVectors, worldNormal);
    
    float3 zeroL = step(float3(0,0,0), dotL);

    float3 diffuse  = zeroL * dotL;
    float3 specular = pow(max(dotH, 0) * zeroL, SpecularPower);

    ColorPair result;
    
    result.Diffuse  = mul(diffuse,  lightDiffuse)  * DiffuseColor.rgb + EmissiveColor;
    result.Specular = mul(specular, lightSpecular) * SpecularColor;

    return result;
}

float ComputeFogFactor(float4 position)
{
    return saturate(dot(position, FogVector));
}

void AddSpecular(inout float4 color, float3 specular)
{
    color.rgb += specular * color.a;
}

void ApplyFog(inout float4 color, float fogFactor)
{
    color.rgb = lerp(color.rgb, FogColor * color.a, fogFactor);
}

CommonVSOutput ComputeCommonVSOutputWithLighting(float4 position, float3 normal, uniform int numLights)
{
    CommonVSOutput vout;
    
    float4 pos_ws = mul(position, World);
    float3 eyeVector = normalize(EyePosition - pos_ws.xyz);
    float3 worldNormal = normalize(mul(normal, WorldInverseTranspose));

    ColorPair lightResult = ComputeLights(eyeVector, worldNormal, numLights);
    
    vout.Pos_ps = mul(position, WorldViewProj);
    vout.Diffuse = float4(lightResult.Diffuse, DiffuseColor.a);
    vout.Specular = lightResult.Specular;
    vout.FogFactor = ComputeFogFactor(position);
    
    return vout;
}

// Vertex shader: vertex lighting + vertex color.
VSOutput VSBasicVertexLightingVc(VSInputNmVc vin)
{
    VSOutput vout;
    
    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(vin.Position, vin.Normal, 3);
    vout.PositionPS = cout.Pos_ps;
    vout.Diffuse = cout.Diffuse;
    vout.Specular = float4(cout.Specular, cout.FogFactor);
    
    vout.Diffuse *= vin.Color;
    
    return vout;
}
// Pixel shader: vertex lighting.
float4 PSBasicVertexLighting(VSOutput pin) : SV_Target0
{
    float4 color = pin.Diffuse;

    AddSpecular(color, pin.Specular.rgb);
    ApplyFog(color, pin.Specular.w);
    
    return color;
}
Technique PrimitiveEffect
{
    pass
    {
        VertexShader = compile vs_4_0_level_9_1 VSBasicVertexLightingVc();
        PixelShader  = compile ps_4_0_level_9_1 PSBasicVertexLighting();
    }
}