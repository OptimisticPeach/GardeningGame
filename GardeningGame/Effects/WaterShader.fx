

cbuffer Data : register(b0)
{
    float4x4 World;
    float4x4 WorldViewProj;
    float3 EyePosition;
    float3x3 WorldInverseTranspose;
    float3 LightDirection;
    float3 LightDiffuseColor;
    float3 LightSpecularColor;
    float4 DiffuseColor;
    float3 EmissiveColor;
    float3 SpecularColor;
    float SpecularPower;
    float Time;
};

const float WaveFactor = 0.2;
const float WaveLength = 17.0;

const float PI = 3.14159265358979323846264338327950;
const float TwoThirdsPI = 2.094395102393195492308428922186;

// Vertex shader output structures.

struct VSOutput
{
    float4 Position : S_POSITION;
    float4 Color : COLOR;
    float4 PositionPS : SV_Position;
};

struct CommonVSOutput
{
    float4 Pos_ps;
    float4 Diffuse;
    float3 Specular;
};

struct LightColorPair
{
    float3 Diffuse;
    float3 Specular;
};

struct ColorPair
{
    float4 Diffuse;
    float3 Specular;
};

struct VSInput
{
    float4 Position : POSITION;
    float4 Color : COLOR;
};

struct GSInput
{
    float4 Position : S_POSITION;
    float4 Color : COLOR;
};

struct GSOutput
{
    float4 PositionPS : SV_Position;
    float4 Diffuse : COLOR0;
    float3 Specular : COLOR1;
};

struct PSInput
{
    float4 PositionPS : SV_Position;
    float4 Diffuse : COLOR0;
    float3 Specular : COLOR1;
};

LightColorPair ComputeLights(float3 eyeVector, float3 worldNormal, uniform int numLights)
{
    // float3x3 lightDirections = 0;
    // float3x3 lightDiffuse = 0;
    // float3x3 lightSpecular = 0;
    // float3x3 halfVectors = 0;
    
    //     lightDirections[0] = float3x3(LightDirection,     DirLight1Direction,     DirLight2Direction)[0];
    //     lightDiffuse[0]    = float3x3(DirLight1DiffuseColor,  DirLight1DiffuseColor,  DirLight2DiffuseColor)[0];
    //     lightSpecular[0]   = float3x3(DirLight0SpecularColor, DirLight1SpecularColor, DirLight2SpecularColor)[0];
        
    //     halfVectors[0] = normalize(eyeVector - lightDirections[0]);

    float3 lightDirection = LightDirection;
    float3 lightDiffuse = LightDiffuseColor;
    float3 lightSpecular = LightSpecularColor;
    float3 halfVectors = 0;
        
    halfVectors = normalize(eyeVector - lightDirection);

    float3 dotL = mul(-lightDirection, worldNormal);
    float3 dotH = mul(halfVectors, worldNormal);
    
    float3 zeroL = step(float3(0, 0, 0), dotL);

    float3 diffuse = zeroL * dotL;
    float3 specular = pow(max(dotH, 0) * zeroL, SpecularPower);

    LightColorPair result;
    
    result.Diffuse = mul(diffuse, lightDiffuse) * DiffuseColor.rgb + EmissiveColor;
    result.Specular = mul(specular, lightSpecular) * SpecularColor;

    return result;
}

/////////////////////////////////////////////
float3 mod289(float3 x)
{
    return x - floor(x * (1.0 / 289.0)) * 289.0;
}
float2 mod289(float2 x)
{
    return x - floor(x * (1.0 / 289.0)) * 289.0;
}
float3 permute(float3 x)
{
    return mod289(((x * 34.0) + 1.0) * x);
}

//
// Description : GLSL 2D simplex noise function
//      Author : Ian McEwan, Ashima Arts
//  Maintainer : ijm
//     Lastmod : 20110822 (ijm)
//     License :
//  Copyright (C) 2011 Ashima Arts. All rights reserved.
//  Distributed under the MIT License. See LICENSE file.
//  https://github.com/ashima/webgl-noise
//
float snoise(float2 v)
{

    // Precompute values for skewed triangular grid
    const float4 C = float4(0.211324865405187,
                        // (3.0-sqrt(3.0))/6.0
                        0.366025403784439,
                        // 0.5*(sqrt(3.0)-1.0)
                        -0.577350269189626,
                        // -1.0 + 2.0 * C.x
                        0.024390243902439);
                        // 1.0 / 41.0

    // First corner (x0)
    float2 i = floor(v + dot(v, C.yy));
    float2 x0 = v - i + dot(i, C.xx);

    // Other two corners (x1, x2)
    float2 i1 = float2(0.0, 0.0);
    i1 = (x0.x > x0.y) ? float2(1.0, 0.0) : float2(0.0, 1.0);
    float2 x1 = x0.xy + C.xx - i1;
    float2 x2 = x0.xy + C.zz;

    // Do some permutations to avoid
    // truncation effects in permutation
    i = mod289(i);
    float3 p = permute(
            permute(i.y + float3(0.0, i1.y, 1.0))
                + i.x + float3(0.0, i1.x, 1.0));

    float3 m = max(0.5 - float3(
                        dot(x0, x0),
                        dot(x1, x1),
                        dot(x2, x2)
                        ), 0.0);

    m = m * m;
    m = m * m;

    // Gradients:
    //  41 pts uniformly over a line, mapped onto a diamond
    //  The ring size 17*17 = 289 is close to a multiple
    //      of 41 (41*7 = 287)

    float3 x = 2.0 * frac(p * C.www) - 1.0;
    float3 h = abs(x) - 0.5;
    float3 ox = floor(x + 0.5);
    float3 a0 = x - ox;

    // Normalise gradients implicitly by scaling m
    // Approximation of: m *= inversesqrt(a0*a0 + h*h);
    m *= 1.79284291400159 - 0.85373472095314 * (a0 * a0 + h * h);

    // Compute final noise value at P
    float3 g = float3(0.0, 0.0, 0.0);
    g.x = a0.x * x0.x + h.x * x0.y;
    g.yz = a0.yz * float2(x1.x, x2.x) + h.yz * float2(x1.y, x2.y);
    return 130.0 * dot(m, g);
}
/////////////////////////////////////////////


void AddSpecular(inout float4 color, float3 specular)
{
    color.rgb += specular * color.a;
}

CommonVSOutput ComputeCommonVSOutputWithLighting(float4 position, float3 normal, uniform int numLights)
{
    CommonVSOutput vout;
    
    float4 pos_ws = mul(position, World);
    float3 eyeVector = normalize(EyePosition - pos_ws.xyz);
    float3 worldNormal = normalize(mul(normal, WorldInverseTranspose));

    LightColorPair lightResult = ComputeLights(eyeVector, worldNormal, numLights);
    
    vout.Pos_ps = mul(position, WorldViewProj);
    vout.Diffuse = float4(lightResult.Diffuse, DiffuseColor.a);
    vout.Specular = lightResult.Specular;
    
    return vout;
}

float generateOffset(float x, float z, float val1, float val2)
{
    float Wav = sin(Time) * 50;
    float radiansX = ((((x + z * x * val1) % Wav) / Wav) + Time * ((x * 0.8 + z) % 1.5)) * 2.0 * 3.1415926535897932386426433832795;
    float radiansZ = ((((val2 * (z * x + x * z)) % Wav) / Wav) + Time * 2.0 * (x % 2.0)) * 2.0 * 3.1415926535897932386426433832795;
    return WaveFactor * 0.5 * (sin(radiansZ) + cos(radiansX));
}

float genOffset(float g)
{
    float thetaPlusG = g + Time;
    float Contents = thetaPlusG * TwoThirdsPI;
    float Sine = sin(Contents);
    float Cosine = cos(Contents);
    float Tangent = tan(Sine + Cosine);
    return thetaPlusG + Tangent * Time;
}

float3 applyDistortion(float3 vertex)
{
    float xDistortion = generateOffset(vertex.x, vertex.z, 0.2, 0.1);
    float yDistortion = generateOffset(vertex.x, vertex.z, 0.1, 0.3);
    float zDistortion = generateOffset(vertex.x, vertex.z, 0.15, 0.2);
    return vertex;
    //+float3(xDistortion, yDistortion, zDistortion);
}

[maxvertexcount(3)]
void GeometryShader_(triangle GSInput input[3], inout TriangleStream<GSOutput> outstream)
{
    float3 v1 = input[1].Position.xyz - input[0].Position.xyz;
    float3 v2 = input[2].Position.xyz - input[0].Position.xyz;
    float3 normal = cross(v1, v2);

    normalize(normal);

    GSOutput output;

    CommonVSOutput cout = ComputeCommonVSOutputWithLighting(input[0].Position, normal, 1);
    output.PositionPS = cout.Pos_ps;
    output.Diffuse = cout.Diffuse;
    output.Specular = cout.Specular;
    
    output.Diffuse *= input[0].Color;

    outstream.Append(output);

    cout = ComputeCommonVSOutputWithLighting(input[1].Position, normal, 1);
    output.PositionPS = cout.Pos_ps;
    output.Diffuse = cout.Diffuse;
    output.Specular = cout.Specular;
    
    output.Diffuse *= input[1].Color;

    outstream.Append(output);

    cout = ComputeCommonVSOutputWithLighting(input[2].Position, normal, 1);
    output.PositionPS = cout.Pos_ps;
    output.Diffuse = cout.Diffuse;
    output.Specular = cout.Specular;
    
    output.Diffuse *= input[2].Color;

    outstream.Append(output);
}

VSOutput VertexShader_(VSInput input)
{
    VSOutput r;
    r.Position = float4(0, 0, 0, 0);

    //////Weird thing happens to this file. I explicitly say for it to use this file,
    //////but unfortunately it doesn't. Please use the other file for future reference.

    r.Color = float4(0, 0, 0, 0);
    r.PositionPS = float4(0, 0, 0, 0);
    return r;
}

float4 PixelShader_(PSInput pin) : SV_Target
{
    float4 color = pin.Diffuse;

    AddSpecular(color, pin.Specular.rgb);
    
    return color;
}
technique defaultTechnique
{
    pass
    {
        VertexShader = compile vs_4_0 VertexShader_();
        PixelShader = compile ps_4_0 PixelShader_();
    }
}