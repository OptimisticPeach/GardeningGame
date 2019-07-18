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
using GardeningGame.Engine.Scenes.Common;

namespace GardeningGame.Engine.Scenes.Game
{
    public partial class GameScene
    {
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime)
        {
            DrawScreenBuffer(gameTime);

            Utils.FPS = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;

            //var ScreenBackBuffe= Utils.CreateWobble(ScreenBackBuffer, gameTime);

            Camera.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.LinearClamp, DepthStencilState.Default,
                RasterizerState.CullNone);

            Camera.SpriteBatch.Draw(ScreenBackBuffer, new Rectangle(0, 0, Graphics.GraphicsDevice.PresentationParameters.BackBufferWidth,
                Graphics.GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);

            Camera.SpriteBatch.End();
        }
        protected void DrawScreenBuffer(GameTime gameTime)
        {
            //var CurMouseState = Mouse.GetState();
            Graphics.GraphicsDevice.SetRenderTarget(ScreenBackBuffer);
            Graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Graphics.GraphicsDevice.Clear(GameSceneVariables.clearColor);
            WaterWithEffect.DrawWithWaterEffect(Graphics.GraphicsDevice, new Vector3(0, -180, 0), false, WEffect, Cam);
            Graphics.GraphicsDevice.SetRenderTarget(null);
        }

    }
}
