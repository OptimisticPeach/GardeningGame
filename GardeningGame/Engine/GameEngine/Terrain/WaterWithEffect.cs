using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;
using SharpDX.Direct3D;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;

namespace GardeningGame.Engine.Scenes.Game.Terrain
{
    public class WaterWithEffect : Terrain
    {
        private Vector3 offset;
        public void DrawWithWaterEffect(GraphicsDevice GD, Vector3 Position, bool UseID, WaterEffect WEffect, Common.Camera Cam)
        {
            WEffect.World = Cam.worldMatrix * Matrix.CreateTranslation(Position) * Matrix.CreateTranslation(offset);
            WEffect.View = Cam.viewMatrix;
            WEffect.Projection = Cam.projectionMatrix;

            if (UseID)
            {
                throw new NotImplementedException();
            }
            else
            {
                WEffect.InternalEffect.CurrentTechnique = WEffect.InternalEffect.Techniques[0];
                WEffect.DiffuseColor = new Vector4(0.192f, 0.192f, 0.192f, 1); //1, 1, 1 by default
                WEffect.LightSpecularColor = new Vector3(0f);
                WEffect.SpecularColor = new Vector3(1f);
                WEffect.SpecularPower = 0.14f;
                WEffect.LightDirection = new Vector3(1, .71f, 1);
                WEffect.EmissiveColor = new Vector3(0.125f);
            }

            GD.SetVertexBuffer(VertexBuffer);

            WEffect.InternalEffect.CurrentTechnique = WEffect.InternalEffect.Techniques[0];

            foreach (EffectPass pass in WEffect.InternalEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                ((Device)GD.Handle).ImmediateContext.GeometryShader.Set(WEffect.GS);
                ((Device)GD.Handle).ImmediateContext.PixelShader.Set(WEffect.PS);
                ((Device)GD.Handle).ImmediateContext.VertexShader.Set(WEffect.VS);
                
                Buffer[] vsBuffers = WEffect.InternalEffect.ConstantBuffers.Select((a) => a._cbuffer).ToArray();
                if (vsBuffers != null)
                {
                    for (int i = 0; i < vsBuffers.Length; ++i)
                    {
                        ((Device)GD.Handle).ImmediateContext.GeometryShader.SetConstantBuffer(i, vsBuffers[i]);
                        ((Device)GD.Handle).ImmediateContext.VertexShader.SetConstantBuffer(i, vsBuffers[i]);
                        ((Device)GD.Handle).ImmediateContext.PixelShader.SetConstantBuffer(i, vsBuffers[i]);
                    }
                }
                else
                    throw new Exception() ;
                GD.DrawPrimitives(PrimitiveType.TriangleList, 0, VertexBuffer.VertexCount / 3);
                ((Device)GD.Handle).ImmediateContext.GeometryShader.Set(null);

            }

            GD.SetVertexBuffer(null);
            GD.Indices = null;
        }
        public override void Generate(int TerrainDepth, int TerrainWidth, int Spacing, GraphicsDevice gd)
        {
            VertexBuffer = new VertexBuffer(gd, VertexPositionColor_Vector4.VertexDeclaration, TerrainDepth * TerrainWidth * 3 * 2, BufferUsage.WriteOnly);
            VertexBuffer.SetData(Common.Utils.getTrisAlternatingForEff(Common.Utils.BlueishColours, TerrainDepth, TerrainWidth, Spacing).ToArray());
            offset = new Vector3(-TerrainDepth / 2f * Spacing, 0, -TerrainWidth / 2f * Spacing);
        }

        public void GenerateCircle(int Depth, int Width, int Spacing, float radius, GraphicsDevice gd)
        {
            VertexBuffer = new VertexBuffer(gd, VertexPositionColor_Vector4.VertexDeclaration, Depth * Width * 3 * 2, BufferUsage.WriteOnly);
            VertexBuffer.SetData(Common.Utils.getTrisAlternatingForEffInCircle(Common.Utils.BlueishColours, Depth, Width, Spacing, radius, GameSceneVariables.clearColor).ToArray());
            offset = new Vector3(-Depth / 2f * Spacing, 0, -Width / 2f * Spacing);
        }

        public override void Update(GraphicsDevice GD, GameSceneVariables GSV, GameTime GT = null, MouseState? MS = null, KeyboardState? KS = null)
        {
            //All of the movement is done in the shader with the help of the GS, PS and VS
            return;
        }
    }
}
