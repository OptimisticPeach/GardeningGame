using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame;
using MonoGame.Utilities;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace GardeningGame.Engine.Scenes.Game
{
    public static class Utils
    {
        public static Color[] BrownishColours;
        public static Color[] BlueishColours;
        public static float FPS = 0;
        public static Random RNG = new Random();

        public const float waveLength = 17;
        public static float waveTime = 0;

        public static float generateOffset(float x, float z, float val1, float val2)
        {
            float radiansX = ((((x + z * x * val1) % waveLength) / waveLength) + waveTime * ((x * 0.8f + z )% 1.5f)) * 6.283185307179586476925286766559f;
            float radiansZ = (((val2 * (z * x + x * z) % waveLength) / waveLength) + waveTime * 2.0f * (x % 2.0f)) * 6.283185307179586476925286766559f;
            return GameSceneVariables.WaveFactor * 50 * (float)(Math.Sin(radiansZ) + Math.Cos(radiansX));
        }

        public static Vector3 applyDistortion(Vector3 vertex)
        {
            float xDistortion = generateOffset(vertex.X, vertex.Z, 0.2f, 0.1f);
            float yDistortion = generateOffset(vertex.X, vertex.Z, 0.8f, 0.95f);
            float zDistortion = generateOffset(vertex.X, vertex.Z, 0.15f, 0.2f);
            return vertex + new Vector3(xDistortion * 1.5f, yDistortion * 1.5f, zDistortion * 1.5f);
        }

        public static float Map(float n, float start1, float stop1, float start2, float stop2)
        {
            return ((n - start1) / (stop1 - start1)) * (stop2 - start2) + start2;
        }

        public static Color[,] TextureTo2DArray(Texture2D texture)
        {
            Color[] colorsOne = new Color[texture.Width * texture.Height]; //The hard to read,1D array
            texture.GetData(colorsOne); //Get the colors and add them to the array

            Color[,] colorsTwo = new Color[texture.Width, texture.Height]; //The new, easy to read 2D array
            for (int x = 0; x < texture.Width; x++) //Convert!
                for (int y = 0; y < texture.Height; y++)
                    colorsTwo[x, y] = colorsOne[x + y * texture.Width];

            return colorsTwo; //Done!
        }

        public class ModelList
        {
            public string[] this[string Index]
            {
                get
                {
                    for (int i = 0; i != Models.Count(); i++)
                    {
                        if (Models[i][0] == Index)
                            return Models[i];
                    }
                    throw new Exception("Can't find " + Index);
                }
            }

            public string RelativePath;
            public string[][] Models;
            public static List<string> getModelFromType(string ModelType, ModelList source)
            {
                for (int i = 0; i != source.Models.Count(); i++)
                {
                    if (source.Models[i][0] == ModelType)
                    {
                        return source.Models[i].ToList().GetRange(1, source.Models.Count() - 2);
                    }
                }
                return null;
            }
        }

        public static float Slerp(float a, float b, float t)
        {
            //t = t * t * t * (t * (6f * t - 15f) + 10f);
            t = t * t * (3f - 2f * t);
            return MathHelper.Lerp(a, b, t);
        }

        public static IEnumerable<VertexPositionColorNormal> getTrisAlternating(VertexPositionColorNormal[,] source, int Depth, int Width, out uint[,][] Indices)
        {
            List<uint>[,] Indexes = new List<uint>[Width, Depth];
            for(int i = 0; i != Width; i++)
            {
                for (int j = 0; j != Width; j++)
                {
                    Indexes[i, j] = new List<uint>();
                }
            }
            var result = new List<VertexPositionColorNormal>();
            uint Index = 0;
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
                            //Indexes[x, y].Add((uint)((x + y * Width) * 6));
                            //Indexes[x + 1, y].Add((uint)((x + y * Width) * 6) + 1);
                            //Indexes[x + 1, y + 1].Add((uint)((x + y * Width) * 6) + 2);
                            Indexes[x, y].Add(Index++);
                            Indexes[x + 1, y].Add(Index++);
                            Indexes[x + 1, y + 1].Add(Index++);
                        }

                        b = source[x + 1, y + 1];
                        c = source[x, y + 1];

                        

                        if (a.Color != Color.Transparent && b.Color != Color.Transparent && c.Color != Color.Transparent)
                        {
                            result.Add(a);
                            result.Add(b);
                            result.Add(c);
                            Indexes[x, y].Add(Index++);
                            Indexes[x + 1, y + 1].Add(Index++);
                            Indexes[x, y + 1].Add(Index++);
                        }
                    }
                    else
                    {
                        var a = source[x, y];
                        var b = source[x+1,y];
                        var c = source[x, y + 1];

                        

                        if (a.Color != Color.Transparent && b.Color != Color.Transparent && c.Color != Color.Transparent)
                        {
                            result.Add(a);
                            result.Add(b);
                            result.Add(c);
                            Indexes[x, y].Add(Index++);
                            Indexes[x + 1, y].Add(Index++);
                            Indexes[x, y + 1].Add(Index++);
                        }

                        a = source[x + 1, y];
                        b = source[x + 1, y+1];
                        c = source[x, y + 1];

                        

                        if (a.Color != Color.Transparent && b.Color != Color.Transparent && c.Color != Color.Transparent)
                        {
                            result.Add(a);
                            result.Add(b);
                            result.Add(c);
                            Indexes[x + 1, y].Add(Index++);
                            Indexes[x + 1, y + 1].Add(Index++);
                            Indexes[x, y + 1].Add(Index++);
                        }
                    }
                }
            }

            Indices = new uint[Width, Depth][];

            for (int i = 0; i != Width; i++)
            {
                for (int j = 0; j != Width; j++)
                {
                    if (Indexes[i, j].Count != 0)
                        Indices[i, j] = Indexes[i, j].ToArray();
                    else
                        Indices[i, j] = null;
                }
            }

            return result;
        }

        public static IEnumerable<VertexPositionColorNormal> getTrisAlternating(VertexPositionColorNormal[] Data, int Depth, int Width, out uint[,][] Indices)
        {
            VertexPositionColorNormal[,] source = new VertexPositionColorNormal[Width, Depth]; //The new, easy to read 2D array
            for (int x = 0; x < Width; x++) //Convert!
                for (int y = 0; y < Depth; y++)
                    source[x, y] = Data[x + y * Width];
            List<uint>[,] Indexes = new List<uint>[Width, Depth];
            for (int i = 0; i != Width; i++)
            {
                for (int j = 0; j != Width; j++)
                {
                    Indexes[i, j] = new List<uint>();
                }
            }
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
                            Indexes[x, y].Add((uint)((x + y * Width) * 6));
                            Indexes[x + 1, y].Add((uint)((x + y * Width) * 6) + 1);
                            Indexes[x + 1, y + 1].Add((uint)((x + y * Width) * 6) + 2);
                        }

                        b = source[x + 1, y + 1];
                        c = source[x, y + 1];

                        

                        if (a.Color != Color.Transparent && b.Color != Color.Transparent && c.Color != Color.Transparent)
                        {
                            result.Add(a);
                            result.Add(b);
                            result.Add(c);
                            Indexes[x, y].Add((uint)((x + y * Width) * 6) + 3);
                            Indexes[x + 1, y + 1].Add((uint)((x + y * Width) * 6) + 4);
                            Indexes[x, y + 1].Add((uint)((x + y * Width) * 6) + 5);
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
                            Indexes[x, y].Add((uint)((x + y * Width) * 6));
                            Indexes[x + 1, y].Add((uint)((x + y * Width) * 6) + 1);
                            Indexes[x, y + 1].Add((uint)((x + y * Width) * 6) + 2);
                        }

                        a = source[x + 1, y];
                        b = source[x + 1, y + 1];
                        c = source[x, y + 1];

                        

                        if (a.Color != Color.Transparent && b.Color != Color.Transparent && c.Color != Color.Transparent)
                        {
                            result.Add(a);
                            result.Add(b);
                            result.Add(c);
                            Indexes[x + 1, y].Add((uint)((x + y * Width) * 6) + 3);
                            Indexes[x + 1, y + 1].Add((uint)((x + y * Width) * 6) + 4);
                            Indexes[x, y + 1].Add((uint)((x + y * Width) * 6) + 5);
                        }
                    }
                }
            }

            Indices = new uint[Width, Depth][];

            for (int i = 0; i != Width; i++)
            {
                for (int j = 0; j != Width; j++)
                {
                    Indices[i, j] = Indexes[i, j].ToArray();
                }
            }

            return result;
        }

        public static float mod(float x, float m)
        {
            return (x % m + m) % m;
        }

        //func(v* Vect) Slerp(o* Vect, t Float) (slerp* Vect) {
        //  omega := v.Dot(o).Acos()
        //    if !(omega.IsNaN() || omega == 0.0) {
        //      denom := 1.0 / omega.Sin()
        //      slerp = v.Mult(((1.0 - t) * omega).Sin() * denom).
        //      Add(o.Mult(((t * omega) * denom).Sin()));
        //    } else {
        //      slerp = v
        //    }
        //  return
        //}

        /// <summary>
        /// Spherically interpolates a pair of vectors given a percentage <c>t</c>
        /// </summary>
        /// <param name="end">The End result once t is equal to 1</param>
        /// <param name="t">The percent such that 0 < t < 1 </param>
        /// <remarks>
        /// Adapted from https://gist.github.com/manveru/384873
        /// </remarks>
        public static Vector3 Slerp(this Vector3 source, Vector3 end, float t)
        {
            float Omega = (float)Math.Acos(Vector3.Dot(source, end));
            if (Omega != float.NaN && Omega != 0f)
            {
                float Denom = 1f / (float)Math.Sin(Omega);
                return source * (float)(Math.Sin((1 - t) * Omega) * Denom) +
                    (end * (float)Math.Sin((t * Omega) * Denom));
            }
            else
                return source;
        }

        public static Vector4 Slerp(this Vector4 source, Vector4 end, float t)
        {
            float Omega = (float)Math.Acos(Vector4.Dot(source, end));
            if (Omega != float.NaN && Omega != 0f)
            {
                float Denom = 1f / (float)Math.Sin(Omega);
                return source * (float)(Math.Sin((1 - t) * Omega) * Denom) +
                    (end * (float)Math.Sin((t * Omega) * Denom));
            }
            else
                return source;
        }
        
        public static void GenerateNormals(ref VertexPositionColorNormal[] verts)
        {
            for (int i = 0; i < verts.Count() - 2; i += 3)
            {
                VertexPositionColorNormal vpcn1 = verts[i];
                VertexPositionColorNormal vpcn2 = verts[i + 1];
                VertexPositionColorNormal vpcn3 = verts[i + 2];

                Vector3 v1 = vpcn2.Position - vpcn1.Position;
                Vector3 v2 = vpcn3.Position - vpcn1.Position;
                Vector3 normal = Vector3.Cross(v1, v2);

                normal.Normalize();

                vpcn1.Normal = normal;
                vpcn2.Normal = normal;
                vpcn3.Normal = normal;

                verts[i] = vpcn1;
                verts[i + 1] = vpcn2;
                verts[i + 2] = vpcn3;
            }
        }

        public static void DrawTextCentered(SpriteFont font, SpriteBatch spriteBatch, Vector2 Position, string Text, Color colour)
        {
            Vector2 Size = font.MeasureString(Text);
            Position -= Size * 0.5f;
            spriteBatch.DrawString(font, Text, Position, colour);
        }

        

        public static Color GetAccent()
        {
            return new Color(System.Windows.SystemParameters.WindowGlassColor.R, System.Windows.SystemParameters.WindowGlassColor.G, System.Windows.SystemParameters.WindowGlassColor.B, System.Windows.SystemParameters.WindowGlassColor.A);
        }

        static Utils()
        {
            int[] Colours = new int[]
            {11, 15, 42, 55, 80, 94, 117, 122};

            BrownishColours = new Color[Colours.Length];

            List<Color> colours = new List<Color>();

            Type type = typeof(Color); // MyClass is static class with static properties
            foreach (var p in type.GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.FlattenHierarchy))
            {
                var v = p.GetValue(null, null);
                if (((Color)v) != new Color(0, 0, 0, 0))
                    colours.Add((Color)v); // static classes cannot be instanced, so use null...
            }

            for(int i = 0; i != Colours.Length; i++)
            {
                BrownishColours[i] = colours[Colours[i]];
            }

            Colours = new int[]
            {2, 9, 20, 21, 22, 36, 39, 41, 134, 87, 96, 129};

            BlueishColours = new Color[Colours.Length];

            for (int i = 0; i != Colours.Length; i++)
            {
                BlueishColours[i] = colours[Colours[i]];
                BlueishColours[i].A -= 60;
            }
        }
    }
}