using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GardeningGame.Engine.Scenes.Common;

namespace GardeningGame.Engine.Scenes.Game.UI
{
    public sealed class SwingingSelector
    {
        public Texture2D Texture { get; set; }
        public float Tick;
        public float Rotation;
        public bool Left = false;
        public Vector2 Location;
        public Entities.Entity Entity;

        public SwingingSelector(Texture2D texture)
        {
            Texture = texture;
        }

        public void Update()
        {
            //currentFrame = (currentFrame + 1) % totalFrames;
            Tick += Left ? -0.1f : 0.1f;// 0.08f;
            if (Tick > Math.PI / 2)
                Left = true;
            else if (Tick <= 0)
                Left = false;
            Rotation = Utils.Slerp(-0.1f, 0.00002f, (float)Math.Sin(Tick) + 1f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //int width = Texture.Width / Columns;
            //int height = Texture.Height / Rows;
            //int row = (int)(currentFrame / (float)Columns);
            //int column = currentFrame % Columns;

            spriteBatch.Draw(Texture, Location, null, Color.White, Rotation, new Vector2(24, 24), 1, SpriteEffects.None, 0);
            
        }
    }
}