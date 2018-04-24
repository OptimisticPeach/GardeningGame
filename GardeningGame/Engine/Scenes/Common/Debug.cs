using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GardeningGame.Engine.Scenes
{
    public static class Debug
    {
        public static readonly bool DEBUG = false;
        public static readonly Vector2 Position = new Vector2(10, 50);
        public const string Debugstring =
            @"Cam_X:{0}
Cam_Y:{1}
Cam_Z:{2}
CamHeight:{3}
CamRadius:{4}
CamRotation:{5}
S_H:{6}
S_R:{7}
S_R:{8}
E_H:{9}
E_R:{10}
E_R:{11}";
        public static SpriteFont DebugFont;
    }
}
