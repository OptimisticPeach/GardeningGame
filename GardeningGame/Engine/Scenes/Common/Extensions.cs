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
        public static Color NextColor(this Random R)
        {
            return new Color(R.Next(255), R.Next(255), R.Next(255));
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

        public static Vector3 GetPosition(this ModelMesh mesh, Matrix[] Bones, Matrix OtherTransforms)
        {
            Matrix Original = Bones[mesh.ParentBone.Index] * OtherTransforms;

            Vector3 Pos = new Vector3();

            Vector3 Scale = new Vector3();

            Quaternion Rotation = new Quaternion();

            Original.Decompose(out Scale, out Rotation, out Pos);

            return Pos * Scale;
        }
        /*
         inline void decompose ( Matrix44f matrix, Vec3f& scaling, Quatf& rotation,
                                                                        Vec3f& position)
{
        // extract translation
        position.x = matrix.at(0, 3);
        position.y = matrix.at(1, 3);
        position.z = matrix.at(2, 3);
       
        // extract the rows of the matrix
       
        Vec3f columns[3] = {
                matrix.getColumn(0).xyz(),
                matrix.getColumn(1).xyz(),
                matrix.getColumn(2).xyz()
        };
       
        // extract the scaling factors
        scaling.x = columns[0].length();
        scaling.y = columns[1].length();
        scaling.z = columns[2].length();
       
        // and remove all scaling from the matrix
        if(scaling.x)
        {
                columns[0] /= scaling.x;
        }
        if(scaling.y)
        {
                columns[1] /= scaling.y;
        }
        if(scaling.z)
        {
                columns[2] /= scaling.z;
        }
       
        // build a 3x3 rotation matrix
        Matrix33f m(columns[0].x,columns[1].x,columns[2].x,
                                  columns[0].y,columns[1].y,columns[2].y,
                                  columns[0].z,columns[1].z,columns[2].z, true);
       
        // and generate the rotation quaternion from it
        rotation = Quatf(m);
}*/

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

        private static Texture2D GetTexture(SpriteBatch spriteBatch, Color C)
        {
            Texture2D Texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Texture.SetData(new[] { C });
            return Texture;
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
