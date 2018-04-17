using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace GardeningGame.Engine.Scenes.Game.UI.UIText
{
    public class UIElement
    {
        public enum UIElementUsage : byte
        {
            Text,
            Image,
            Sound,
            Video,
            Link
        }

        public enum Font : byte
        {
            Arial,
            Arial_Large,
            Arial_Small,
            Arial_Bold,
            Arial_Italic,
        }

        public UIElementUsage Usage { get; set; }
        
        public struct DataUnion
        {
            public Vector2 Position;
            public Color Colour;
            public Font TextFont;
            public Font OnHoverFont;
            public string Text;
            public Texture2D Image;
            public SoundEffectInstance SoundEffect;
            public Video Video;
            public string Link;
            public Rectangle BoundingRect;
        }
        public DataUnion Data { get; set; }
        public bool PreviouslyClicked;
    }
}
