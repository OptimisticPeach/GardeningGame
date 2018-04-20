using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GardeningGame.Engine.Scenes.Common
{

    public static class SmartGardenCamera
    {
        public static int Width;
        public static int Height;

        public static float radius = 400;

        static SmartGardenCamera()
        {
            Target = new Vector3(0, 0, 0);
            //Position -= new Vector3(1800, 0, 0);
        }

        public static void Initialize(GraphicsDevice gd, float radius)
        {
            SmartGardenCamera.radius = radius;
            Width = gd.PresentationParameters.BackBufferWidth;
            Height = gd.PresentationParameters.BackBufferHeight;
            var Max = Math.Max(Width, Height);
            var Min = Math.Min(Width, Height);
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
            x = radius * Math.Cos(angle);
            y = 1400;
            z = radius * Math.Sin(angle);
            Position = new Vector3((float)x, (float)y, (float)z);
        }

        public static void Rotate(float r)
        {
            _rotation += r;
            //_rotation %= 8;
            _rotation = Utils.mod(_rotation, 8);
            setPosition(_rotation);
        }

        public static float _rotation = 0f;

        public static SpriteBatch spriteBatch;

        //Camera
        public static readonly Vector3 Target;
        public static Vector3 Position;

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