using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GardeningGame.Engine.Scenes.Common
{
    public abstract class Camera
    {
        public static SpriteBatch SpriteBatch;

        public BasicEffect PrimitivesEffect;

        //Camera
        public Vector3 Target;
        public Vector3 Position;

        public Matrix projectionMatrix { get; protected set; }
        public Matrix viewMatrix { get; protected set; }
        public Matrix worldMatrix { get; protected set; }
    }
}
