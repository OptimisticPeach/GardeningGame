

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

const float PI = 3.14159265358979323;
const float TAU = 2 * 3.14159265358979323846264338327950;
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

float4 mod289(float4 x)
{
    return x - floor(x * (1.0 / 289.0)) * 289.0;
}

float4 permute(float4 x)
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

float4 taylorInvSqrt(float4 r)
{
    return 1.79284291400159 - 0.85373472095314 * r;
}

float snoise(float3 v)
{
    const float2 C = float2(1.0 / 6.0, 1.0 / 3.0);
    const float4 D = float4(0.0, 0.5, 1.0, 2.0);

// First corner
    float3 i = floor(v + dot(v, C.yyy));
    float3 x0 = v - i + dot(i, C.xxx);

// Other corners
    float3 g = step(x0.yzx, x0.xyz);
    float3 l = 1.0 - g;
    float3 i1 = min(g.xyz, l.zxy);
    float3 i2 = max(g.xyz, l.zxy);

  //   x0 = x0 - 0.0 + 0.0 * C.xxx;
  //   x1 = x0 - i1  + 1.0 * C.xxx;
  //   x2 = x0 - i2  + 2.0 * C.xxx;
  //   x3 = x0 - 1.0 + 3.0 * C.xxx;
    float3 x1 = x0 - i1 + C.xxx;
    float3 x2 = x0 - i2 + C.yyy; // 2.0*C.x = 1/3 = C.y
    float3 x3 = x0 - D.yyy; // -1.0+3.0*C.x = -0.5 = -D.y

// Permutations
    i = mod289(i);
    float4 p = permute(permute(permute(
             i.z + float4(0.0, i1.z, i2.z, 1.0))
           + i.y + float4(0.0, i1.y, i2.y, 1.0))
           + i.x + float4(0.0, i1.x, i2.x, 1.0));

// Gradients: 7x7 points over a square, mapped onto an octahedron.
// The ring size 17*17 = 289 is close to a multiple of 49 (49*6 = 294)
    float n_ = 0.142857142857; // 1.0/7.0
    float3 ns = n_ * D.wyz - D.xzx;

    float4 j = p - 49.0 * floor(p * ns.z * ns.z); //  mod(p,7*7)

    float4 x_ = floor(j * ns.z);
    float4 y_ = floor(j - 7.0 * x_); // mod(j,N)

    float4 x = x_ * ns.x + ns.yyyy;
    float4 y = y_ * ns.x + ns.yyyy;
    float4 h = 1.0 - abs(x) - abs(y);

    float4 b0 = float4(x.xy, y.xy);
    float4 b1 = float4(x.zw, y.zw);

  //float4 s0 = float4(lessThan(b0,0.0))*2.0 - 1.0;
  //float4 s1 = float4(lessThan(b1,0.0))*2.0 - 1.0;
    float4 s0 = floor(b0) * 2.0 + 1.0;
    float4 s1 = floor(b1) * 2.0 + 1.0;
    float4 sh = -step(h, float4(0.0, 0.0, 0.0, 0.0));

    float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
    float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;

    float3 p0 = float3(a0.xy, h.x);
    float3 p1 = float3(a0.zw, h.y);
    float3 p2 = float3(a1.xy, h.z);
    float3 p3 = float3(a1.zw, h.w);

//Normalise gradients
    float4 norm = taylorInvSqrt(float4(dot(p0, p0), dot(p1, p1), dot(p2, p2), dot(p3, p3)));
    p0 *= norm.x;
    p1 *= norm.y;
    p2 *= norm.z;
    p3 *= norm.w;

// Mix final noise value
    float4 m = max(0.6 - float4(dot(x0, x0), dot(x1, x1), dot(x2, x2), dot(x3, x3)), 0.0);
    m = m * m;
    return 42.0 * dot(m * m, float4(dot(p0, x0), dot(p1, x1),
                                dot(p2, x2), dot(p3, x3)));
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
    float radiansX = ((((x + z * x * val1) % WaveLength) / WaveLength) + Time * ((x * 0.8 + z) % 1.5)) * PI;
    float radiansZ = ((((val2 * (z * x + x * z)) % WaveLength) / WaveLength) + Time * 2.0 * (x % 2.0)) * PI;
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
    float xDistortion = generateOffset(vertex.x, vertex.z, 1,1);
    float yDistortion = generateOffset(vertex.x, vertex.z, 1,1);
    float zDistortion = generateOffset(vertex.x, vertex.z, 1,1);
    return float3(xDistortion, yDistortion, zDistortion);
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
    r.Position = input.Position;

    //r.Position = input.Position;

    r.Color = input.Color;
    //float noisevalx = snoise(float3(input.Position.xz, -Time));
    float noisevaly = snoise(float3(input.Position.xz / 20, Time * 2));
    float noisevalx = snoise(float3(input.Position.xz / 25, Time * 5));
    float noisevalz = snoise(float3(input.Position.zx / 22.5, Time * 5));
    //float noisevalz = snoise(float3(input.Position.zx, Time));

    //float noise = noiseFunc(float3(input.Position.xz, Time)) * 30;
    
    //noise = normalize(noise);

    r.Position += float4(noisevalx, noisevaly + 10, noisevalz, 0) * 10;
    
    r.PositionPS = mul(r.Position, World);
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