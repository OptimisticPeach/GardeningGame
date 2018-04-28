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
    public class Bush : Entity
    {
        public Bush(ref Dictionary<string, List<Model>> Lists)
        {
            Kind = Utils.RNG.Next(Lists["Bushes"].Count);
            Rotation.Y += Utils.RNG.Next(360);
        }

        public override byte Size { get => (byte)PlantSize.Bush; }

        public override Model getModel(ref Dictionary<string, List<Model>> Source)
        {
            return Source["Bushes"][Kind];
        }

        public static new Texture2D Sprite;

        public override void ScaleObject(Vector3 Offset)
        {
            if((Scales.X + Scales.Y + Scales.Z + Offset.X + Offset.Y + Offset.Z) / 6 <= 2)
            {
                Scales += Offset;
            }
        }
        public Bush() { }
    }
}
