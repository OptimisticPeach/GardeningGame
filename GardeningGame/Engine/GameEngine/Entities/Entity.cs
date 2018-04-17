using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GardeningGame.Engine.Scenes.Game.Entities
{
    public abstract class Entity
    {
        public enum PlantSize : byte
        {
            Thin = 0b0000_0001,
            Short = 0b0000_0010,
            Tall = 0b0000_0100,
            Fat = 0b0000_1000,
            MediumFat = 0b0001_0000,
            MediumTall = 0b0010_0000,
            //Predefines
            Reed = 0b0010_0001,
            LargeBush = 0b0000_1100,
            Bush = 0b0011_0000,
            Cactus = 0b0001_0001,
            DeadBush = 0b0011_0000,
            Flower = 0b0011_0000,
            Grass = 0b0001_0010,
            Fungi = 0b0011_0000,
            Shrub = 0b0001_0010
        }

        public abstract byte Size { get; }
        public Vector3 Position = new Vector3(0);
        public Vector3 Rotation = new Vector3(0);
        public Vector3 Scales = new Vector3(1);
        public object Tag;

        public abstract Model getModel(ref Dictionary<string, List<Model>> Source);

        public virtual BoundingSphere GetBoundingSphere(Vector3 PlantTilePosition, ref Dictionary<string, List<Model>> sourceModels)
        {
            float MaxScale = Math.Max(Scales.X, Math.Max(Scales.Y, Scales.Z));
            return new BoundingSphere(Position + PlantTilePosition, 
                getModel(ref sourceModels).Meshes[0].BoundingSphere.Radius * MaxScale);
        }

        [Obsolete("Use ScaleMatrix * RotationMatrix * TranslationMatrix", true)]
        public virtual Matrix Transformation
        {
            get
            {
                return Matrix.CreateScale(Scales) *
                       Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) *
                       Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y)) *
                       Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z)) *// *
                       Matrix.CreateTranslation(Position);
            }
        }

        public virtual Matrix ScaleMatrix
        {
            get
            {
                return Matrix.CreateScale(Scales);
            }
        }

        public virtual Matrix RotationMatrix
        {
            get
            {
                return Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) *
                       Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y)) *
                       Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z));
            }
        }

        public virtual Matrix TranslationMatrix
        {
            get
            {
                return Matrix.CreateTranslation(Position);
            }
        }

        public virtual void Move(Vector3 Offset)
        {
            Position += Offset;
        }
        public virtual void Rotate(Vector3 Offset)
        {
            Rotation += Offset;
        }
        public virtual void ScaleObject(Vector3 Offset)
        {
            Scales += Offset;
        }

        public virtual void ScaleObjectConstrained(Vector3 Offset, float Max, float Min)
        {
            var X = Math.Max(Min, Math.Min(Max, Offset.X + Scales.X));
            var Y = Math.Max(Min, Math.Min(Max, Offset.Y + Scales.Y));
            var Z = Math.Max(Min, Math.Min(Max, Offset.Z + Scales.Z));
            Scales = new Vector3(X, Y, Z);
        }

        public static Texture2D Sprite;
    }
}
