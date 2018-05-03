using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GardeningGame.Engine.Scenes.Game
{
    public class GameCam : Common.Camera
    {
        public int SWidth;
        public int SHeight;

        public float Height;

        public float Radius;

        public void Initialize(GraphicsDevice gd, float radius, float near, bool setY, float Height)
        {
            Radius = radius;
            this.Height = Height;
            if (Target != null)
            {
                Target.X = 0;
                Target.Y = 0;
                Target.Z = 0;
            }
            else
                Target = new Vector3(0);
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
            SpriteBatch = new SpriteBatch(gd);
            setPosition(setY);
        }

        public void setPosition(bool setY)
        {
            double angle = Rotation;
            Position.X = Radius * (float)Math.Cos(angle);
            Position.Y = Height;
            Position.Z = Radius * (float)Math.Sin(angle);
            if (setY)
                Target.Y = Height;
        }

        public void Rotate(float r, bool setY)
        {
            Rotation += r;
            //_rotation %= 8;
            Rotation = Common.Utils.mod(Rotation, MathHelper.TwoPi);
            setPosition(setY);
        }

        public float Rotation = 0f;
    }
}