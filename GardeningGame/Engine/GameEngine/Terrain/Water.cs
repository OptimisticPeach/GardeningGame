﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GardeningGame.Engine.Scenes.Common;

namespace GardeningGame.Engine.Scenes.Game.Terrain
{
    public class Water : Terrain
    {
        public Water()
            : base() { }

        public VertexPositionColorNormal[] Vertices;

        public int Width;
        public int Depth;
        public int Spacing;
        public int TotalWaterSizeX;
        public int TotalWaterSizeZ;

        public uint[,][] Indices;

        private Vector3[] tempVecs;

        public override void Generate(int TerrainDepth, int TerrainWidth, int Spacing, GraphicsDevice gd)
        {
            var TempVertices = new VertexPositionColorNormal[TerrainDepth, TerrainWidth];

            this.Spacing = Spacing;
            Width = TerrainWidth;
            Depth = TerrainDepth;
            TotalWaterSizeX = TerrainWidth * Spacing;
            TotalWaterSizeZ = TerrainDepth * Spacing;
            tempVecs = new Vector3[Width * Depth];

            for (int x = 0; x < TerrainDepth; x++)
            {
                for (int z = 0; z < TerrainWidth; z++)
                {
                    VertexPositionColorNormal vpc = new VertexPositionColorNormal();
                    vpc.Position = new Vector3((x * Spacing) - (TotalWaterSizeX / 2f), -45, (z * Spacing) - (TotalWaterSizeZ / 2f));
                    vpc.Color = Utils.BlueishColours[Utils.RNG.Next(Utils.BlueishColours.Length - 1)];
                    if (x == 0 || z == 0 ||
                       x == TerrainWidth - 1 || z == TerrainDepth - 1)
                    {
                        vpc.Color.A = 0;
                    }
                    TempVertices[x, z] = vpc;
                    tempVecs[x + z * Width] = vpc.Position;
                }
            }

            Indices = new uint[TerrainWidth, TerrainDepth][];

            Vertices = Utils.getTrisAlternating(TempVertices, TerrainWidth, TerrainDepth, out Indices).ToArray();

            Utils.GenerateNormals(ref Vertices);

            VertexBuffer = new VertexBuffer(gd, VertexPositionColorNormal.VertexDeclaration, Vertices.Length, BufferUsage.WriteOnly);
            VertexBuffer.SetData(Vertices);
        }

        public void GenerateCircle(int TerrainDepth, int TerrainWidth, int Spacing, float Radius, GraphicsDevice gd)
        {
            var TempVertices = new VertexPositionColorNormal[TerrainDepth, TerrainWidth];

            this.Spacing = Spacing;
            ID = Color.White;
            Width = TerrainWidth;
            Depth = TerrainDepth;
            TotalWaterSizeX = TerrainWidth * Spacing;
            TotalWaterSizeZ = TerrainDepth * Spacing;
            tempVecs = new Vector3[Width * Depth];

            for (int x = 0; x < TerrainDepth; x++)
            {
                for (int z = 0; z < TerrainWidth; z++)
                {
                    VertexPositionColorNormal vpc = new VertexPositionColorNormal();
                    vpc.Position = new Vector3((x * Spacing) - (TotalWaterSizeX / 2f), -85, (z * Spacing) - (TotalWaterSizeZ / 2f));
                    vpc.Color = new Color(0, 0, 0, 0);
                    if (Vector2.Distance(new Vector2(vpc.Position.X, vpc.Position.Z), new Vector2(0)) <= Radius)
                    {
                        vpc.Color = Utils.BlueishColours[Utils.RNG.Next(Utils.BlueishColours.Length - 1)];
                        //vpc.Color = Color.Lerp(vpc.Color, Color.Transparent, Vector2.Distance(new Vector2(vpc.Position.X, vpc.Position.Z), new Vector2(0)) / (Radius*2));
                        //vpc.Color = new Color(vpc.Color.ToVector4().Slerp(Color.Transparent.ToVector4(), Vector2.Distance(new Vector2(vpc.Position.X, vpc.Position.Z), new Vector2(0)) / (Radius)));
                    }
                    else if (Vector2.Distance(new Vector2(vpc.Position.X, vpc.Position.Z), new Vector2(0)) <= Radius + Spacing + 25)
                    {
                        vpc.Color = new Color(0, 0, 0, 1);
                    }
                    TempVertices[x, z] = vpc;
                    tempVecs[x + z * Width] = vpc.Position;
                }
            } 

            Indices = new uint[TerrainWidth, TerrainDepth][];

            Vertices = Utils.getTrisAlternating(TempVertices, TerrainWidth, TerrainDepth, out Indices).ToArray();

            Utils.GenerateNormals(ref Vertices);

            VertexBuffer = new VertexBuffer(gd, VertexPositionColorNormal.VertexDeclaration, Vertices.Length, BufferUsage.WriteOnly);
            VertexBuffer.SetData(Vertices);

        }

        public override void Update(GraphicsDevice GD, GameSceneVariables GSV,  GameTime GT = null, MouseState? MS = null, KeyboardState? KS = null)
        {
            for (int i = 0; i < GSV.WaterSize; i++)
            {
                for (int j = 0; j < GSV.WaterSize; j++)
                {
                    if (Indices[i, j] != null)
                    {
                        var Distortion = Utils.applyDistortion(new Vector3((i * Spacing) - Spacing / 2f, -85, (j * Spacing) - Spacing / 2f));
                        Distortion.X += TotalWaterSizeX * -0.5f +Spacing * 0.5f;
                        Distortion.Z += TotalWaterSizeZ * -0.5f +Spacing * 0.5f;

                        //for(int k = 0; k != Indices[i, j].Count(); k++)
                        //{
                        //    Vertices[Indices[i, j][k]].Position = Distortion;
                        //}
                        foreach (var b in Indices[i, j])
                        {
                            Vertices[b].Position = Distortion;
                        }
                    }
                }
            }

            Utils.GenerateNormals(ref Vertices);

            VertexBuffer.SetData(Vertices);
        }
        
    }
}
