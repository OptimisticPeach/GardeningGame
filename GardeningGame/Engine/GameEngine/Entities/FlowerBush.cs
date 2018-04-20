using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GardeningGame.Engine.Scenes.Common;

namespace GardeningGame.Engine.Scenes.Game.Entities
{
    public class FlowerBush : Entity
    {
        int FlowerBushType;
        public override byte Size { get => (byte)PlantSize.Flower; }
        public FlowerBush(ref Dictionary<string, List<Model>> Lists)
        {
            FlowerBushType = Utils.RNG.Next(Lists["FlowerBushes"].Count);
            Rotation.Y += Utils.RNG.Next(360);
        }
        public override Model getModel(ref Dictionary<string, List<Model>> Source)
        {
            return Source["FlowerBushes"][FlowerBushType];
        }
        public static new Texture2D Sprite;

        public override void ScaleObject(Vector3 Offset)
        {
            if ((Scales.X + Scales.Y + Scales.Z + Offset.X + Offset.Y + Offset.Z) / 6 <= 2)
            {
                Scales += Offset;
            }
        }
    }
}
