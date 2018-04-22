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

        public static void Initialize(GraphicsDevice gd, float radius)
        {
            RotatingCam.Radius = radius;
            SWidth = gd.PresentationParameters.BackBufferWidth;
            SHeight = gd.PresentationParameters.BackBufferHeight;
            var Max = Math.Max(SWidth, SHeight);
            var Min = Math.Min(SWidth, SHeight);
            double AspectRatio = (double)Min / Max;
            float Angle = 90 * (float)AspectRatio;
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                               MathHelper.ToRadians(Angle), gd.Viewport.AspectRatio,
                800, 3821f);
            worldMatrix = Matrix.CreateWorld(Target, Vector3.
                          Forward, Vector3.Up);
            spriteBatch = new SpriteBatch(gd);
            setPosition(0);
        }

        public static void setPosition(float rotation)
        {
            double x, y, z;
            double angle = MathHelper.ToRadians((rotation) * 45);
            x = Radius * Math.Cos(angle);
            y = Height;
            z = Radius * Math.Sin(angle);
            Position = new Vector3((float)x, (float)y, (float)z);
        }

        public static void Rotate(float r)
        {
            Rotation += r;
            //_rotation %= 8;
            Rotation = Utils.mod(Rotation, 8);
            setPosition(Rotation);
        }

        public static float Rotation = 0f;

        public static SpriteBatch spriteBatch;

        //Camera
        public static Vector3 Target;
        public static Vector3 Position;

        public static Vector3 CylindricalCoords
        {
            get
            {
                return new Vector3(Height, Radius, Rotation);
            }
            set
            {
                Height = value.X;
                Radius = value.Y;
                Rotation = value.Z;
            }
        }

        public static Matrix projectionMatrix;
        public static Matrix viewMatrix
        {
            get
            {
                return Matrix.CreateLookAt(Position, Target,
                         Vector3.Up);
            }
        }
        public static Matrix worldMatrix;
    }
}