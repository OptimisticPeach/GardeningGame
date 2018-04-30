using System;
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
    public class DirtPatch : Terrain
    {
        public DirtPatch()
            : base()
        {
        }

        public override void Generate(int TerrainDepth, int TerrainWidth, int Spacing, GraphicsDevice gd)
        {
            var Terrain = new VertexPositionColorNormal[TerrainDepth * TerrainWidth];
            for (int i = 0; i != TerrainWidth; i++) 
            {
                for (int j = 0; j != TerrainDepth; j++)
                {
                    VertexPositionColorNormal vpc = new VertexPositionColorNormal();
                    
                    vpc.Color = Utils.BrownishColours[Utils.RNG.Next(Utils.BrownishColours.Length - 1)];
                    if (i == 0 || j == 0 ||
                        i == TerrainWidth - 1 || j == TerrainDepth - 1)
                    {
                        vpc.Position = new Vector3(Spacing * i, 0, Spacing * j);
                        vpc.Position *= 1.125f;
                        vpc.Position.Y = -500;
                        vpc.Color.A = 0;
                    }
                    else
                    {
                        float X = (Spacing * i);// + (x - 1) * Consts.spacing) - (Consts.spacing / 2);
                        //float Y = (proceduralGeneration.generateHeight(
                        //        Spacing * i, Spacing * j) * 1.5f);// - 60;
                        //Y = Utils.Map(Y, -40, 40, -10, 10);
                        //Y *= 2;
                        float Y = Utils.generateOffset(Spacing * Utils.RNG.Next() * i, Spacing * Utils.RNG.Next() * j, 0.5f, 0.324234f) * 5 ;
                        float Z = (Spacing * j);// + (y - 1) * Consts.spacing) - (Consts.spacing / 2);
                        vpc.Position = new Vector3(X, Y, Z);
                    }
                    Terrain[i + j * TerrainWidth] = vpc;
                }
            }

            uint[,][] empty;

            Terrain = Utils.getTrisAlternating(Terrain, TerrainDepth, TerrainWidth, out empty).ToArray();

            Utils.GenerateNormals(ref Terrain);

            VertexBuffer = new VertexBuffer(gd, VertexPositionColorNormal.VertexDeclaration, Terrain.Length, BufferUsage.WriteOnly);
            VertexBuffer.SetData(Terrain);

            ID = new Color(Utils.RNG.Next(255), Utils.RNG.Next(255), Utils.RNG.Next(255));
        }

        public override void Update(GraphicsDevice GD, GameSceneVariables GSV, GameTime GT = null, MouseState? MS = null, KeyboardState? KS = null)
        {
        }
    }
}
