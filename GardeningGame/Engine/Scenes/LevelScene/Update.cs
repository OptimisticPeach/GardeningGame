using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using GardeningGame.Engine.Scenes.Game.UI;
using System.Diagnostics;

namespace GardeningGame.Engine.Scenes.LevelSelect
{
    public partial class LevelSelectScene
    {
        public Vector3 AffectedModelUUID = new Vector3();
        public Color CurrentColorUnderMouse;

        protected void updateKeys(GameTime time, KeyboardState KeyBoardState)
        {
            ScreenOrSelection = KeyBoardState.IsKeyDown(Keys.A);

            if (KeyBoardState.IsKeyDown(Keys.Z))
            {
                OnRequestedSceneChanged(this, null);
            }
            if (KeyBoardState.IsKeyDown(Keys.X))
            {
                for (int i = 1; i != 100000; i++)
                {
                    var b = Math.Log10(Math.Tan(Math.Tanh(Math.Sqrt(i)))) / Math.Log10(Math.E);
                }
            }
        }


        public void Update(GameTime GT, MouseState MS, KeyboardState KS)
        {
            //int i = 1;
            //foreach (var b in Tiles)
            //{
            //    b.Position.Y = Utils.generateOffset(i++, i++, 0.45f, 0.8f) * 2;
            //}

            updateKeys(GT, KS);
        }
    }
}
