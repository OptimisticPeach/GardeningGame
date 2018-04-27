using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using System.Reflection;

namespace GardeningGame.Engine
{
    public class DataLoader
    {
        public void Initialize(BinaryReader FS)
        {
            Fs = FS;
        }

        private BinaryReader Fs { get; set; }

        public int ReadInt32 => Fs.ReadInt32();
        public float ReadSingle => Fs.ReadSingle();

        public Matrix GetReadMatrix() => new Matrix(ReadVector4, ReadVector4, ReadVector4, ReadVector4);

        public Color ReadColor => new Color(ReadByte, ReadByte, ReadByte, ReadByte);
        public Vector3 ReadVector3 => new Vector3(ReadSingle, ReadSingle, ReadSingle);
        public Vector4 ReadVector4 => new Vector4(ReadSingle, ReadSingle, ReadSingle, ReadSingle);
        public bool ReadBoolean => Fs.ReadBoolean();
        public char ReadChar => Fs.ReadChar();
        public byte ReadByte => Fs.ReadByte();
        public Decimal ReadDecimal => Fs.ReadDecimal();
        public DateTime ReadDateTime => new DateTime(ReadInt64);
        public Double ReadDouble => Fs.ReadDouble();
        public Int16 ReadInt16 => Fs.ReadInt16();
        public Int64 ReadInt64 => Fs.ReadInt64();
        public Point ReadPoint => new Point(ReadInt32, ReadInt32);
        public String ReadString => Fs.ReadString();
        public Vector2 ReadVector2 => new Vector2(ReadSingle, ReadSingle);
        public UInt16 ReadUInt16 => Fs.ReadUInt16();
        public UInt32 ReadUInt32 => Fs.ReadUInt32();
        public UInt64 ReadUInt64 => Fs.ReadUInt64();
        public List<T> ReadList<T>(int count) where T : struct
        {
            if (!typeof(T).Name.StartsWith("List") && typeof(T).Name != "Object")
            {
                var thisobject = typeof(DataLoader);
                List<MethodInfo> Methods = thisobject.GetMethods().ToList();
                var MethodName = "Read" + typeof(T).Name;
                var Method = Methods.Where((a) => a.Name == MethodName).First();
                List<T> Objects = new List<T>();
                for (int i = Math.Abs(count); i != 0; i--)
                {
                    Objects.Add((T)Method.Invoke(this, null));
                }
                return Objects;
            }
            throw new ArgumentException();
        }

        public T Read<T>()
        {
            if (!typeof(T).Name.StartsWith("List") && typeof(T).Name != "Object")
            {
                var thisobject = typeof(DataLoader);
                List<MethodInfo> Methods = thisobject.GetMethods().ToList();
                var MethodName = "Read" + typeof(T).Name;
                var Method = Methods.Where((a) => a.Name == MethodName).First();
                T outObject = (T)Method.Invoke(this, null);
                return outObject;
            }
            throw new ArgumentException();
        }

        public void End() => Fs.Close();
    }
    public class DataWriter
    {
        string fileName;
        public void Initialize(BinaryWriter FS)
        {
            fs = FS;
            fileName = ((FileStream)fs.BaseStream).Name;
        }
        BinaryWriter fs { get; set; }

        public void WriteInt32(int a) => fs.Write(a);
        public void WriteSingle(float a) => fs.Write(a);
        public void WriteMatrix(Matrix M)
        {
            WriteSingle(M.M11);
            WriteSingle(M.M12);
            WriteSingle(M.M13);
            WriteSingle(M.M14);
            WriteSingle(M.M21);
            WriteSingle(M.M22);
            WriteSingle(M.M23);
            WriteSingle(M.M24);
            WriteSingle(M.M31);
            WriteSingle(M.M32);
            WriteSingle(M.M33);
            WriteSingle(M.M34);
            WriteSingle(M.M41);
            WriteSingle(M.M42);
            WriteSingle(M.M43);
            WriteSingle(M.M44);
        }
        public void WriteColor(Color a)
        {
            WriteByte(a.R);
            WriteByte(a.G);
            WriteByte(a.B);
            WriteByte(a.A);
        }
        public void WriteVector3(Vector3 a)
        {
            WriteSingle(a.X);
            WriteSingle(a.Y);
            WriteSingle(a.Z);
        }
        public void WriteVector4(Vector4 a)
        {
            WriteSingle(a.X);
            WriteSingle(a.Y);
            WriteSingle(a.Z);
            WriteSingle(a.W);
        }
        public void WriteBoolean(bool a) => fs.Write(a);
        public void WriteChar(char a) => fs.Write(a);
        public void WriteByte(byte a) => fs.Write(a);
        public void WriteDecimal(decimal a) => fs.Write(a);
        public void WriteDateTime(DateTime a) => fs.Write(a.Ticks);
        public void WriteDouble(double a) => fs.Write(a);
        public void WriteInt16(short a) => fs.Write(a);
        public void WriteInt64(long a) => fs.Write(a);
        public void WritePoint(Point a) { WriteInt32(a.X); WriteInt32(a.Y); }
        public void WriteString(string a) => fs.Write(a);
        public void WriteVector2(Vector2 a) { WriteSingle(a.X); WriteSingle(a.Y); }
        public void WriteUInt16(ushort a) => fs.Write(a);
        public void WriteUInt32(uint a) => fs.Write(a);
        public void WriteUInt64(ulong a) => fs.Write(a);
        public void WriteList<T>(List<T> a) where T : struct
        {
            if (!typeof(T).Name.StartsWith("List") && typeof(T).Name != "Object")
            {
                var thisobject = typeof(DataWriter);
                List<MethodInfo> Methods = thisobject.GetMethods().ToList();
                var MethodName = "Write" + typeof(T).Name;
                var Method = Methods.Where((b) => b.Name == MethodName).First();
                foreach (var s in a)
                {
                    Method.Invoke(this, new object[] { s });
                }
                return;
            }
            throw new ArgumentException();
        }

        public void Write<T>(T a)
        {
            if (!typeof(T).Name.StartsWith("List") && typeof(T).Name != "Object")
            {
                var thisobject = typeof(DataWriter);
                List<MethodInfo> Methods = thisobject.GetMethods().ToList();
                var MethodName = "Write" + typeof(T).Name;
                var Method = Methods.Where((b) => b.Name == MethodName).First();
                Method.Invoke(this, new[] {(object)a });
                return;
            }
            throw new ArgumentException();
        }
        public void End() => fs.Close();
        public void Clear() => File.WriteAllText(fileName, string.Empty);
    }
}
