using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GardeningGame.Engine.Scenes.Common
{
    [Serializable]
    public struct Vector3Serializable
    {
        public float X;
        public float Y;
        public float Z;
        public static implicit operator Vector3Serializable(Vector3 inV3)
        {
            return new Vector3Serializable() { X = inV3.X, Y = inV3.Y, Z = inV3.Z };
        }
        public static implicit operator Vector3(Vector3Serializable inV3)
        {
            return new Vector3(inV3.X, inV3.Y, inV3.Z);
        }
    }
}
