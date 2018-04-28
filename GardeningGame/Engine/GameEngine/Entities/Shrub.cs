using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GardeningGame.Engine.Scenes.Common;

namespace GardeningGame.Engine.Scenes.Game.Entities
{
    public class Shrub : Entity
    {
        public Shrub(ref Dictionary<string, List<Model>> Lists)
        {
            Kind = Utils.RNG.Next(Lists["RandomPlants"].Count);
            Rotation.Y += Utils.RNG.Next(360);
        }

        public override byte Size { get => (byte)PlantSize.Shrub; }

        public override Matrix ScaleMatrix => Matrix.CreateScale(1.25f) * base.ScaleMatrix;

        public override Model getModel(ref Dictionary<string, List<Model>> Source)
        {
            return Source["RandomPlants"][Kind];
        }
        public static new Texture2D Sprite;
        public Shrub() { }
    }
}
