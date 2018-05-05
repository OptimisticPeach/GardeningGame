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
            //DrawSelectionBuffer(gameTime, tile);
            DrawScreenBuffer(gameTime, Tiles);
            DrawSelectionBuffer(gameTime, Tiles);

            Utils.FPS = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;

            //var ScreenBackBuffe= Utils.CreateWobble(ScreenBackBuffer, gameTime);

            Camera.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.LinearClamp, DepthStencilState.Default,
                RasterizerState.CullNone);

            Camera.SpriteBatch.Draw(ScreenOrSelection ? SelectionBackBuffer : ScreenBackBuffer, new Rectangle(0, 0, Graphics.GraphicsDevice.PresentationParameters.BackBufferWidth,
                Graphics.GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
            if (Selector != null)
                Selector.Draw(Camera.SpriteBatch);
            if (Debug.DEBUG)
            {
                Camera.SpriteBatch.DrawString(UI.UIText.UIFrame.fonts[(int)UI.UIText.UIElement.Font.Arial],
                    String.Format(
                        Debug.Debugstring,
                        Cam.Position.X,
                        Cam.Position.Y,
                        Cam.Position.Z,
                        Cam.Height,
                        Cam.Radius,
                        Cam.Rotation,
                        Cam.Target.X,
                        Cam.Target.Y,
                        Cam.Target.Z,
                        Cam.Rotation,
                        Tiles.Length, 0),
                    //Utils.TextureTo2DArray(ScreenBackBuffe)[Mouse.GetState().X, Mouse.GetState().Y]),
                    new Vector2(10, 50), Color.Black);
            }
            HelpButton.Draw(gameTime, Camera.SpriteBatch, 0);

            if (HelpButton.Toggled)
            {
                UI.UIText.UIFrame.Overlay.DrawImage(Camera.SpriteBatch, Graphics.GraphicsDevice, gameTime, Mouse.GetState(), false);
                //Utils.DrawTextCentered(Font, SmartGardenCamera.spriteBatch, new Vector2(Graphics.PreferredBackBufferWidth / 2f, Graphics.PreferredBackBufferHeight / 2f), Consts.HelpString, Color.Black);
            }



            Camera.SpriteBatch.End();
        }

        protected void DrawSelectionBuffer(GameTime gameTime, PlantTile[,] pta)
        {
            Graphics.GraphicsDevice.SetRenderTarget(SelectionBackBuffer);
            Graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Graphics.GraphicsDevice.Clear(GameSceneVariables.clearColor);

            foreach (var pt in pta)
            {
                if(pt != null)
                pt.Terrain.Draw(Graphics.GraphicsDevice, pt.Position, true, Cam.PEffect, Cam);
            }
            Water.Draw(Graphics.GraphicsDevice, new Vector3(0), true, Cam.PEffect, Cam);

            Graphics.GraphicsDevice.SetRenderTarget(null);
        }
        protected void DrawScreenBuffer(GameTime gameTime, PlantTile[,] pta)
        {
            //var CurMouseState = Mouse.GetState();
            Graphics.GraphicsDevice.SetRenderTarget(ScreenBackBuffer);
            Graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Graphics.GraphicsDevice.Clear(GameSceneVariables.clearColor);

            foreach (var pt in pta)
            {
                if (pt != null)
                {
                    foreach (var entity in pt.EntityList)
                    {
                        foreach (var mesh in entity.getModel(ref OrderedModels).Meshes)//entity.getModel(ref OrderedModels).Meshes)
                        {
                            foreach (BasicEffect effect in mesh.Effects)
                            {
                                //effect.DirectionalLight0.Direction = new Vector3(0, 0, 0);
                                //effect.DirectionalLight0.Enabled = true;
                                //effect.DirectionalLight0.DiffuseColor = new Vector3(0.8f);
                                effect.EnableDefaultLighting();

                                //effect.EmissiveColor = new Vector3(1, 0, 0);

                                //effect.SpecularColor = new Vector3(1);
                                effect.AmbientLightColor = new Vector3(0.35f);

                                effect.View = Cam.viewMatrix; //* Matrix.CreateRotationX(MathHelper.ToRadians(offset.Y));
                                                                      //effect.View *= Matrix.CreateRotationY(MathHelper.ToRadians(offset.X));
                                effect.Projection = Cam.projectionMatrix;
                                effect.World = Cam.worldMatrix * entity.ScaleMatrix * entity.RotationMatrix * Matrix.CreateTranslation(entity.Position) * Matrix.CreateTranslation(pt.Position);// * Matrix.CreateTranslation(new Vector3((x - 1) * spacing, 0, (y - 1) * spacing));// * entity.Transformation;// * Matrix.CreateScale(((MeshTag)mesh.Tag).Scale);
                            }
                            mesh.Draw();
                        }
                    }


                    pt.Terrain.Draw(Graphics.GraphicsDevice, pt.Position, false, Cam.PEffect, Cam);
                }

            }
            Water.Draw(Graphics.GraphicsDevice, new Vector3(0), false, Cam.PEffect, Cam);

            Graphics.GraphicsDevice.SetRenderTarget(null);
        }

    }
}
