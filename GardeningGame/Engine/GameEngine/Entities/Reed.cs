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
    public class Reed : Entity
    {
        public override byte Size { get => (byte)PlantSize.Flower; }
        public Reed(ref Dictionary<string, List<Model>> Lists)
        {
            Kind = Utils.RNG.Next(Lists["Reeds"].Count);
            Rotation.Y += Utils.RNG.Next(360);
        }
        public override Model getModel(ref Dictionary<string, List<Model>> Source)
        {
            return Source["Reeds"][Kind];
        }
        public static new Texture2D Sprite;

        public Reed() { }
    }
}
