using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;



namespace GardeningGame.Engine
{
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexPositionColorNormal : IVertexType
    {
        public Vector3 Position;
        public Color Color;
        public Vector3 Normal;

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get
            {
                return VertexDeclaration;
            }
        }

        public readonly static VertexDeclaration VertexDeclaration;

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Position.GetHashCode();
                hashCode = (hashCode * 397) ^ Color.GetHashCode();
                hashCode = (hashCode * 397) ^ Normal.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return "Position:" + this.Position + "             Color:" + this.Color + "                Normal:" + this.Normal;
        }

        public static bool operator ==(VertexPositionColorNormal left, VertexPositionColorNormal right)
        {
            return ((left.Color == right.Color) && (left.Position == right.Position) && (left.Normal == right.Normal));
        }

        public static bool operator !=(VertexPositionColorNormal left, VertexPositionColorNormal right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != base.GetType())
            {
                return false;
            }
            return (this == ((VertexPositionColorNormal)obj));
        }

        static VertexPositionColorNormal()
        {
            VertexDeclaration = new VertexDeclaration
            (
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                new VertexElement(16, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
            );
        }
    }
}
