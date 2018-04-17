﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GardeningGame.Engine.Scenes.Game.Entities
{
    public class Flower : Entity
    {
        int FlowerType;
        public override byte Size { get => (byte)PlantSize.Flower; }
        public Flower(ref Dictionary<string, List<Model>> Lists)
        {
            FlowerType = Utils.RNG.Next(Lists["Flowers"].Count);
            Rotation.Y += Utils.RNG.Next(360);
        }
        public override Model getModel(ref Dictionary<string, List<Model>> Source)
        {
            return Source["Flowers"][FlowerType];
        }
        public static new Texture2D Sprite;
    }
}
