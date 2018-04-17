﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GardeningGame.Engine.Scenes.Game.Entities
{
    public class Shrub : Entity
    {
        int ShrubType;
        public Shrub(ref Dictionary<string, List<Model>> Lists)
        {
            ShrubType = Utils.RNG.Next(Lists["RandomPlants"].Count);
            Rotation.Y += Utils.RNG.Next(360);
        }

        public override byte Size { get => (byte)PlantSize.Shrub; }

        public override Matrix ScaleMatrix => Matrix.CreateScale(1.25f) * base.ScaleMatrix;

        public override Model getModel(ref Dictionary<string, List<Model>> Source)
        {
            return Source["RandomPlants"][ShrubType];
        }
        public static new Texture2D Sprite;
    }
}
