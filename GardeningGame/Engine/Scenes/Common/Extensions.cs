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

namespace GardeningGame.Engine.Scenes.Common
{
    public static class Extensions
    {
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

        public static void DrawLine3D(this SpriteBatch source, GraphicsDevice GD, Matrix View, Matrix Proj, Matrix World, Color C, Vector3 A, Vector3 B, float Thickness)
        {
            var NewA = GD.Viewport.Project(A, Proj, View, World);
            var NewB = GD.Viewport.Project(B, Proj, View, World);
            source.DrawLine(new Vector2(NewA.X, NewA.Y), new Vector2(NewB.X, NewB.Y), C, Thickness);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness = 1f)
        {
            var distance = Vector2.Distance(point1, point2);
            var angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            DrawLine(spriteBatch, point1, distance, angle, color, thickness);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness = 1f)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(length, thickness);
            spriteBatch.Draw(GetTexture(spriteBatch, color), point, null, Color.White, angle, origin, scale, SpriteEffects.None, 0);
        }

        private static Texture2D texture;
        private static Texture2D GetTexture(SpriteBatch spriteBatch, Color C)
        {
            if(texture == null)
            {
                texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            }
            texture.SetData(new[] { C });
            return texture;
        }

        public static void DrawText3D(this SpriteBatch source, GraphicsDevice GD, Matrix View, Matrix Proj, Matrix World, Color C, Vector3 Pos, SpriteFont Font, String Text)
        {
            Vector2 NewPos = new Vector2();

            var projectedPos = GD.Viewport.Project(Pos, Proj, View, World);

            NewPos.X = projectedPos.X;
            NewPos.Y = projectedPos.Y;

            source.DrawString(Font, Text, NewPos, C);
        }
    }
}
