using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GardeningGame.Engine.Scenes.Common
{

    public static class RotatingCam
    {
        public static int SWidth;
        public static int SHeight;

        public static float Height = 1400;

        public static float Radius = 400;

        static RotatingCam()
        {
            Target = new Vector3(0, 0, 0);
            //Position -= new Vector3(1800, 0, 0);
        }

        public static void Initialize(GraphicsDevice gd, float radius, float near)
        {
            Radius = radius;
            SWidth = gd.PresentationParameters.BackBufferWidth;
            SHeight = gd.PresentationParameters.BackBufferHeight;
            var Max = Math.Max(SWidth, SHeight);
            var Min = Math.Min(SWidth, SHeight);
            double AspectRatio = (double)Min / Max;
            float Angle = 90 * (float)AspectRatio;
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                               MathHelper.ToRadians(Angle), gd.Viewport.AspectRatio,
                near, 3821f);
            worldMatrix = Matrix.CreateWorld(new Vector3(0, 0, 0), Vector3.
                          Forward, Vector3.Up);
            spriteBatch = new SpriteBatch(gd);
            setPosition();
        }

        public static void setPosition()
        {
            double angle = Rotation;
            Position.X = Radius * (float)Math.Cos(angle);
            Position.Y = Height;
            Position.Z = Radius * (float)Math.Sin(angle);
            Target.Y = Height;
        }

        public static void Rotate(float r)
        {
            Rotation += r;
            //_rotation %= 8;
            Rotation = Utils.mod(Rotation, 8);
            setPosition();
        }

        public static float Rotation = 0f;

        public static SpriteBatch spriteBatch;

        public static BasicEffect PrimitivesEffect;

        //Camera
        public static Vector3 Target;
        public static Vector3 Position;

        public static Matrix projectionMatrix;
        public static Matrix viewMatrix
        {
            get
            {
                return Matrix.CreateLookAt(Position, Target,
                         Vector3.Up);                                   //////////////////???????????????????????????
            }
        }
        public static Matrix worldMatrix;
    }
}