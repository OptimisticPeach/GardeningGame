using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GardeningGame.Engine.Scenes.Game.UI.Elements
{
    public class HelpButton : Button
    {
        public override Texture2D Texture { get; set; }
        public override Point Position { get; set; }
        public override float Rotation { get; set; }
        public override float Scale { get; set; }

        public bool Toggled;

        bool previous;

        public override void Load(ContentManager Content)
        {
            Texture = Content.Load<Texture2D>(@"GUI\Help");
        }

        public bool Hovering;// (MouseState? ms) => ms.HasValue ? _HoveringCache = Bounds.Contains(ms.Value.Position) : _HoveringCache;

        public override void Update(GameTime gt, MouseState? ms, KeyboardState? ks)
        {
            if (ms.HasValue)
            {
                Hovering = Bounds.Contains(ms.Value.Position);
                Clicked = Hovering && ms.Value.LeftButton == ButtonState.Pressed;
                Toggled = (previous && !Clicked) ^ Toggled;
            }
            previous = Clicked;
        }

        public override void Draw(GameTime gt, SpriteBatch sb, float Layer)
        {
            sb.Draw(Texture, Position.ToVector2(), null, Hovering ? Color.AntiqueWhite : Color.White, Rotation, new Vector2(0), Scale, SpriteEffects.None, Layer);
        }
    }
}
