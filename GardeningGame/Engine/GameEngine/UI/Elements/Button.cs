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
    public abstract class Button : Element
    {
        public bool Clicked { get; protected set; }

        public abstract Texture2D Texture { get; set; }
        public abstract Point Position { get; set; }
        public abstract float Rotation { get; set; }
        public abstract float Scale { get; set; }

        public virtual Rectangle Bounds {
            get
            {
                Rectangle r = new Rectangle(Position, Texture.Bounds.Size);
                r.Width *= (int)Scale;
                r.Height *= (int)Scale;
                return r;
            }
        }

        public object Usage { get; set; }
        public object Tag { get; set; }

        public virtual void Draw(GameTime gt, SpriteBatch sb, float Layer)
        {
            sb.Draw(Texture, Position.ToVector2(), null, Color.White, Rotation, new Vector2(Texture.Width / 2, Texture.Height / 2), Scale, SpriteEffects.None, Layer);
        }

        public abstract void Load(ContentManager Content);

        public abstract void Update(GameTime gt, MouseState? ms, KeyboardState? ks);
    }
}
