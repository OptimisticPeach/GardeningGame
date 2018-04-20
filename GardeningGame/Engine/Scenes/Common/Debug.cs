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
        public const string Debugstring = "Cam_X:{0}\nCam_Y:{1}\nCam_Z:{2}\nCT_X:{3}\nCT_Y:{4}\nCT_Z:{5}\nDist:{8}\nFPS:{6:N2}\nScroll:{7}\nRot:{9}\n{10}";
        public static SpriteFont DebugFont;
    }
}
