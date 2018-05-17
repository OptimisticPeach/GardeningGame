using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.InteropServices;

namespace GardeningGame.Engine
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexPositionColor_Vector4 : IVertexType
    {
        public Vector4 Position;

        public Vector4 Color;

        public static readonly VertexDeclaration VertexDeclaration;

        public VertexPositionColor_Vector4(Vector3 position, Vector4 color)
        {
            Position = new Vector4(position, 1);
            Color = color;
        }

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get
            {
                return VertexDeclaration;
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Position.GetHashCode() * 397) ^ Color.GetHashCode();
            }
        }

        public override string ToString()
        {
            return "{{Position:" + this.Position + " Color:" + this.Color + "}}";
        }

        public static bool operator ==(VertexPositionColor_Vector4 left, VertexPositionColor_Vector4 right)
        {
            return ((left.Color == right.Color) && (left.Position == right.Position));
        }

        public static bool operator !=(VertexPositionColor_Vector4 left, VertexPositionColor_Vector4 right)
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
            return (this == ((VertexPositionColor_Vector4)obj));
        }

        static VertexPositionColor_Vector4()
        {
            VertexElement[] elements = new VertexElement[] { new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.Position, 0), new VertexElement(16, VertexElementFormat.Vector4, VertexElementUsage.Color, 0) };
            VertexDeclaration declaration = new VertexDeclaration(elements);
            VertexDeclaration = declaration;
        }
    }
}
