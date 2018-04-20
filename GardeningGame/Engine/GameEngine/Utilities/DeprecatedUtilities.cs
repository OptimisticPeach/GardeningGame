using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GardeningGame.Engine.Scenes.Common;

namespace GardeningGame.Engine.Scenes.Game
{
    public static class DeprecatedUtilities
    {
        public static IEnumerable<VertexPositionColorNormal> getTris(VertexPositionColorNormal[] source, int Depth, int Width)
        {
            VertexPositionColorNormal[,] Data = new VertexPositionColorNormal[Width, Depth]; //The new, easy to read 2D array
            for (int x = 0; x < Width; x++) //Convert!
                for (int y = 0; y < Depth; y++)
                    Data[x, y] = source[x + y * Width];

            var result = new List<VertexPositionColorNormal>();
            for (int x = 0; x < Width - 1; x++)
            {
                for (int y = 0; y < Depth - 1; y++)
                {
                    var a = Data[x, y];
                    var b = Data[x + 1, y];
                    var c = Data[x, y + 1];

                    if (a.Color != Color.Transparent && b.Color != Color.Transparent && c.Color != Color.Transparent)
                    {
                        result.Add(a);
                        result.Add(b);
                        result.Add(c);
                    }

                }
            }

            for (int x = 1; x < Width; x++)
            {
                for (int y = 0; y < Depth - 1; y++)
                {
                    var a = Data[x, y];
                    var b = Data[x, y + 1];
                    var c = Data[x - 1, y + 1];

                    if (a.Color != Color.Transparent && b.Color != Color.Transparent && c.Color != Color.Transparent)
                    {
                        result.Add(a);
                        result.Add(b);
                        result.Add(c);
                    }
                }
            }

            return result;

        }

        public static IEnumerable<VertexPositionColorNormal> getTris(VertexPositionColorNormal[,] source, int Depth, int Width)
        {

            var result = new List<VertexPositionColorNormal>();
            for (int x = 0; x < Width - 1; x++)
            {
                for (int y = 0; y < Depth - 1; y++)
                {
                    var a = source[x, y];
                    var b = source[x + 1, y];
                    var c = source[x, y + 1];

                    if (a.Color != Color.Transparent && b.Color != Color.Transparent && c.Color != Color.Transparent)
                    {
                        result.Add(a);
                        result.Add(b);
                        result.Add(c);
                    }
                }
            }

            for (int x = 1; x < Width; x++)
            {
                for (int y = 0; y < Depth - 1; y++)
                {
                    var a = source[x, y];
                    var b = source[x, y + 1];
                    var c = source[x - 1, y + 1];

                    if (a.Color != Color.Transparent && b.Color != Color.Transparent && c.Color != Color.Transparent)
                    {
                        result.Add(a);
                        result.Add(b);
                        result.Add(c);
                    }
                }
            }

            return result;

        }


        public static IEnumerable<VertexPositionColorNormal> getTrisAlternatingOld(VertexPositionColorNormal[,] source, int Depth, int Width)
        {
            var result = new List<VertexPositionColorNormal>();
            for (int x = 0; x < Width - 1; x++)
            {
                for (int y = 0; y < Depth - 1; y++)
                {
                    if ((y + x % 2) % 2 == 0)
                    {
                        var a = source[x, y];
                        var b = source[x + 1, y];
                        var c = source[x + 1, y + 1];

                        if (a.Color != Color.Transparent && b.Color != Color.Transparent && c.Color != Color.Transparent)
                        {
                            result.Add(a);
                            result.Add(b);
                            result.Add(c);
                        }

                        b = source[x + 1, y + 1];
                        c = source[x, y + 1];

                        if (a.Color != Color.Transparent && b.Color != Color.Transparent && c.Color != Color.Transparent)
                        {
                            result.Add(a);
                            result.Add(b);
                            result.Add(c);
                        }
                    }
                    else
                    {
                        var a = source[x, y];
                        var b = source[x + 1, y];
                        var c = source[x, y + 1];

                        if (a.Color != Color.Transparent && b.Color != Color.Transparent && c.Color != Color.Transparent)
                        {
                            result.Add(a);
                            result.Add(b);
                            result.Add(c);
                        }

                        a = source[x + 1, y];
                        b = source[x + 1, y + 1];
                        c = source[x, y + 1];

                        if (a.Color != Color.Transparent && b.Color != Color.Transparent && c.Color != Color.Transparent)
                        {
                            result.Add(a);
                            result.Add(b);
                            result.Add(c);
                        }
                    }
                }
            }

            return result;
        }

        public static IEnumerable<VertexPositionColorNormal> getTrisAlternatingOld(VertexPositionColorNormal[] Data, int Depth, int Width)
        {
            VertexPositionColorNormal[,] source = new VertexPositionColorNormal[Width, Depth]; //The new, easy to read 2D array
            for (int x = 0; x < Width; x++) //Convert!
                for (int y = 0; y < Depth; y++)
                    source[x, y] = Data[x + y * Width];
            var result = new List<VertexPositionColorNormal>();
            for (int x = 0; x < Width - 1; x++)
            {
                for (int y = 0; y < Depth - 1; y++)
                {
                    if ((y + x % 2) % 2 == 0)
                    {
                        var a = source[x, y];
                        var b = source[x + 1, y];
                        var c = source[x + 1, y + 1];

                        if (a.Color != Color.Transparent && b.Color != Color.Transparent && c.Color != Color.Transparent)
                        {
                            result.Add(a);
                            result.Add(b);
                            result.Add(c);
                        }

                        b = source[x + 1, y + 1];
                        c = source[x, y + 1];

                        if (a.Color != Color.Transparent && b.Color != Color.Transparent && c.Color != Color.Transparent)
                        {
                            result.Add(a);
                            result.Add(b);
                            result.Add(c);
                        }
                    }
                    else
                    {
                        var a = source[x, y];
                        var b = source[x + 1, y];
                        var c = source[x, y + 1];

                        if (a.Color != Color.Transparent && b.Color != Color.Transparent && c.Color != Color.Transparent)
                        {
                            result.Add(a);
                            result.Add(b);
                            result.Add(c);
                        }

                        a = source[x + 1, y];
                        b = source[x + 1, y + 1];
                        c = source[x, y + 1];

                        if (a.Color != Color.Transparent && b.Color != Color.Transparent && c.Color != Color.Transparent)
                        {
                            result.Add(a);
                            result.Add(b);
                            result.Add(c);
                        }
                    }
                }
            }

            return result;
        }

        public static Model toModel(VertexBuffer vb, IndexBuffer ib, Effect effect, GraphicsDevice gd)
        {
            ModelMeshPart meshPart = new ModelMeshPart();
            meshPart.IndexBuffer = ib;
            //meshPart.Effect = effect;//>>1
            meshPart.NumVertices = vb.VertexCount;
            //meshPart.PrimitiveCount = vb.VertexCount / 3;
            meshPart.PrimitiveCount = ib.IndexCount / 3;
            meshPart.StartIndex = 0;
            meshPart.VertexBuffer = vb;
            meshPart.VertexOffset = 0;

            ModelMesh modelMesh = new ModelMesh(gd, new ModelMeshPart[] { meshPart }.ToList());
            meshPart.Effect = effect;//1

            List<ModelBone> bones = new List<ModelBone>();
            bones.Add(new ModelBone() { Transform = Matrix.Identity });//
            modelMesh.ParentBone = bones[0];

            Model returnval = new Model(gd, bones, new ModelMesh[] { modelMesh }.ToList());

            return returnval;
        }

        public static T getRandom<T>(IEnumerable<T> source)
        {
            return source.ToArray()[Utils.RNG.Next(source.Count() - 1)];
        }
    }
}
