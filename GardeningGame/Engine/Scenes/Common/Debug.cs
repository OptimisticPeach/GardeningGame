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
        public static readonly bool DEBUG = true;
        public const string Debugstring =
            @"Cam_X:{0}
Cam_Y:{1}
Cam_Z:{2}
CT_X:{3}
CT_Y:{4}
CT_Z:{5}
M_R:{6}
M_G:{7}
M_B:{8}
MSR:{9}
MSG:{10}
MSB:{11}";
        public static SpriteFont DebugFont;
    }
}
