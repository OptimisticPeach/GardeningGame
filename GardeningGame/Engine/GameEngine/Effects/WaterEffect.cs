using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using Device = SharpDX.Direct3D11.Device;
using MonoGame.Framework.Content.Pipeline.Builder;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System.IO;

namespace GardeningGame.Engine.Scenes.Game
{
    public class WaterEffect
    {
        public void InitShaders(string WaterEffectPath, GraphicsDevice GD)
        {
            var CompiledGS = ShaderBytecode.CompileFromFile(WaterEffectPath, "GeometryShader_", "gs_4_1");
            GS = new GeometryShader((Device)GD.Handle, CompiledGS.Bytecode);

            var CompiledPS = ShaderBytecode.CompileFromFile(WaterEffectPath, "PixelShader_", "ps_4_1");
            PS = new PixelShader((Device)GD.Handle, CompiledPS.Bytecode);

            var CompiledVS = ShaderBytecode.CompileFromFile(WaterEffectPath, "VertexShader_", "vs_4_1");
            VS = new VertexShader((Device)GD.Handle, CompiledVS.Bytecode);
        }
        public GeometryShader GS;
        public PixelShader PS;
        public VertexShader VS;
        private float waveTime = 0;
        Effect internalEffect;
        public Effect InternalEffect
        {
            get
            {
                if (!SetParams)
                {
                    waveTime += 0.002f;
                    internalEffect.Parameters["World"].SetValue(World);
                    internalEffect.Parameters["WorldViewProj"].SetValue(World * View * Projection);
                    internalEffect.Parameters["WorldInverseTranspose"].SetValueTranspose(Matrix.Invert(World));
                    internalEffect.Parameters["EyePosition"].SetValue(Matrix.Invert(View).Translation);
                    internalEffect.Parameters["DiffuseColor"].SetValue(DiffuseColor);
                    internalEffect.Parameters["EmissiveColor"].SetValue(EmissiveColor);
                    internalEffect.Parameters["SpecularColor"].SetValue(SpecularColor);
                    internalEffect.Parameters["SpecularPower"].SetValue(SpecularPower);
                    internalEffect.Parameters["LightDirection"].SetValue(LightDirection);
                    internalEffect.Parameters["LightDiffuseColor"].SetValue(LightDiffuseColor);
                    internalEffect.Parameters["LightSpecularColor"].SetValue(LightSpecularColor);
                    internalEffect.Parameters["Time"].SetValue(waveTime);
                    SetParams = true;
                }
                return internalEffect;
            }
            set
            {
                internalEffect = value;
                SetParams = false;
            }
        }

        private bool SetParams = false;
        private Vector3 _lightSpecularColor = new Vector3(1);
        private Vector3 _lightDiffuseColor = new Vector3(1);
        private Vector3 _lightDirection = new Vector3(1);
        private float _specularPower = 1;
        private Vector3 _specularColor = new Vector3(1);
        private Vector3 _emissiveColor = new Vector3(1);
        private Vector4 _diffuseColor = new Vector4(1);
        private Matrix _projection = Matrix.Identity;
        private Matrix _view = Matrix.Identity;
        private Matrix _world = Matrix.Identity;

        //float4x4 World;
        //float4x4 WorldViewProj;
        //float3 EyePosition;
        //float3x3 WorldInverseTranspose;
        //float3 LightDirection;
        //float3 LightDiffuseColor;
        //float3 LightSpecularColor;
        //float4 DiffuseColor;
        //float3 EmissiveColor;
        //float3 SpecularColor;
        //float SpecularPower;
        //float Time;
        //float WaveFactor;
        //float WaveLength;

        public Vector4 DiffuseColor
        {
            get => _diffuseColor; set
            {
                _diffuseColor = value;
                SetParams = false;
            }
        }

        public Vector3 EmissiveColor
        {
            get => _emissiveColor; set
            {
                _emissiveColor = value;
                SetParams = false;
            }
        }

        public Vector3 SpecularColor
        {
            get => _specularColor; set
            {
                _specularColor = value;
                SetParams = false;
            }
        }

        public float SpecularPower
        {
            get => _specularPower; set
            {
                _specularPower = value;
                SetParams = false;
            }
        }

        public Vector3 LightDirection
        {
            get => _lightDirection; set
            {
                _lightDirection = value;
                SetParams = false;
            }
        }

        public Vector3 LightDiffuseColor
        {
            get => _lightDiffuseColor; set
            {
                _lightDiffuseColor = value;
                SetParams = false;
            }
        }

        public Vector3 LightSpecularColor
        {
            get => _lightSpecularColor; set
            {
                _lightSpecularColor = value;
                SetParams = false;
            }
        }

        public Matrix Projection
        {
            get => _projection; set
            {
                _projection = value;
                SetParams = false;
            }
        }
        public Matrix View
        {
            get => _view; set
            {
                _view = value;
                SetParams = false;
            }
        }
        public Matrix World
        {
            get => _world; set
            {
                _world = value;
                SetParams = false;
            }
        }

        public static explicit operator Effect(WaterEffect source)
        {
            return source.InternalEffect;
        }
    }
}
