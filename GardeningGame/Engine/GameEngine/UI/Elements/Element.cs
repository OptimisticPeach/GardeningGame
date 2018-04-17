using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GardeningGame.Engine.Scenes.Game.UI.Elements
{
    public interface Element
    {
        object Usage { get; set; }
        object Tag { get; set; }

        Texture2D Texture { get; set; }

        Point Position { get; set; }

        float Rotation { get; set; }

        float Scale { get; set; }


        void Load(ContentManager Content);

        void Update(GameTime gt, MouseState? ms, KeyboardState? ks);

        void Draw(GameTime gt, SpriteBatch sb, float Layer);

    }
}
