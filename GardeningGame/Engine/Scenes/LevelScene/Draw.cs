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

namespace GardeningGame.Engine.Scenes.LevelSelect
{
    public partial class LevelSelectScene
    {
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime)
        {
            //DrawSelectionBuffer(gameTime, tile);
            DrawScreenBuffer();
            DrawSelectionBuffer();

            //var ScreenBackBuffe= Utils.CreateWobble(ScreenBackBuffer, gameTime);

            RotatingCam.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.LinearClamp, DepthStencilState.Default,
                RasterizerState.CullNone);

            RotatingCam.spriteBatch.Draw(ScreenOrSelection ? SelectionBackBuffer : ScreenBackBuffer, new Rectangle(0, 0, Graphics.GraphicsDevice.PresentationParameters.BackBufferWidth,
                Graphics.GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
            
            
            if (Debug.DEBUG)
            {
                //RotatingCam.spriteBatch.DrawLine3D(Graphics.GraphicsDevice, RotatingCam.viewMatrix, RotatingCam.projectionMatrix, RotatingCam.worldMatrix, Color.Red, new Vector3(-1000, 0, 0), new Vector3(1000, 0, 0), 2);
                //RotatingCam.spriteBatch.DrawLine3D(Graphics.GraphicsDevice, RotatingCam.viewMatrix, RotatingCam.projectionMatrix, RotatingCam.worldMatrix, Color.Green, new Vector3(0, -1000, 0), new Vector3(0, 1000, 0), 2);
                //RotatingCam.spriteBatch.DrawLine3D(Graphics.GraphicsDevice, RotatingCam.viewMatrix, RotatingCam.projectionMatrix, RotatingCam.worldMatrix, Color.Blue, new Vector3(0, 0, -1000), new Vector3(0, 0, 1000), 2);

                //RotatingCam.spriteBatch.DrawText3D(Graphics.GraphicsDevice, RotatingCam.viewMatrix, RotatingCam.projectionMatrix, RotatingCam.worldMatrix, Color.Blue, new Vector3(0, 0, -1000), Debug.DebugFont, "Negative Z");
                //RotatingCam.spriteBatch.DrawText3D(Graphics.GraphicsDevice, RotatingCam.viewMatrix, RotatingCam.projectionMatrix, RotatingCam.worldMatrix, Color.Blue, new Vector3(0, 0, 1000), Debug.DebugFont, "Positive Z");
                //RotatingCam.spriteBatch.DrawText3D(Graphics.GraphicsDevice, RotatingCam.viewMatrix, RotatingCam.projectionMatrix, RotatingCam.worldMatrix, Color.Green, new Vector3(0, -1000, 0), Debug.DebugFont, "Negative Y");
                //RotatingCam.spriteBatch.DrawText3D(Graphics.GraphicsDevice, RotatingCam.viewMatrix, RotatingCam.projectionMatrix, RotatingCam.worldMatrix, Color.Green, new Vector3(0, 1000, 0), Debug.DebugFont, "Positive Y");
                //RotatingCam.spriteBatch.DrawText3D(Graphics.GraphicsDevice, RotatingCam.viewMatrix, RotatingCam.projectionMatrix, RotatingCam.worldMatrix, Color.Red, new Vector3(-1000, 0, 0), Debug.DebugFont, "Negative X");
                //RotatingCam.spriteBatch.DrawText3D(Graphics.GraphicsDevice, RotatingCam.viewMatrix, RotatingCam.projectionMatrix, RotatingCam.worldMatrix, Color.Red, new Vector3(1000, 0, 0), Debug.DebugFont, "Positive X");
                //RotatingCam.spriteBatch.DrawText3D(Graphics.GraphicsDevice, RotatingCam.viewMatrix, RotatingCam.projectionMatrix, RotatingCam.worldMatrix, Color.Black, new Vector3(0, 0, 0), Debug.DebugFont, "0, 0");

                //var LastTreeBushEffectDiffColour = new Color(((BasicEffect)LTree.Meshes["L10"].Effects.First()).DiffuseColor);
                var LastTreeBushEffectDiffColour = LTree.Meshes["L10"].Tag;
                RotatingCam.spriteBatch.DrawString(Debug.DebugFont,
                    String.Format(
                        Debug.Debugstring,
                        CurrentColorUnderMouse,0,0,
                        RotatingCam.Height,
                        RotatingCam.Radius,
                        RotatingCam.Rotation,
                        Start.X,Start.Y,Start.Z,
                        //LastTreeBushEffectDiffColour.R,
                        //LastTreeBushEffectDiffColour.G,
                        //LastTreeBushEffectDiffColour.B
                        End.X, End.Y, End.Z
                        ),
                    //Utils.TextureTo2DArray(ScreenBackBuffe)[Mouse.GetState().X, Mouse.GetState().Y]),
                    Debug.Position, Color.Black);
            }

            RotatingCam.spriteBatch.End();
        }

        Matrix[] LTreeTransForms;

        protected void DrawSelectionBuffer()
        {
            Graphics.GraphicsDevice.SetRenderTarget(SelectionBackBuffer);
            Graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Graphics.GraphicsDevice.Clear(Color.AliceBlue);

            foreach (var Mesh in LTree.Meshes)
            {
                if (Mesh.Tag is Vector3)
                {
                    foreach (BasicEffect effect in Mesh.Effects)
                    {
                        //effect.DirectionalLight0.Direction = new Vector3(0, 0, 0);
                        //effect.DirectionalLight0.Enabled = true;
                        //effect.DirectionalLight0.DiffuseColor = new Vector3(0.8f);

                        //effect.EmissiveColor = new Vector3(1, 0, 0);

                        /*
                         * Effect.CurrentTechnique = Effect.Techniques["BasicEffect"];
                         * Effect.LightingEnabled = false; // turn on the lighting subsystem
                         * Effect.FogEnabled = false;
                         * Effect.VertexColorEnabled = false;
                         * Effect.DiffuseColor = ID.ToVector3(); //1, 1, 1 by default
                         */

                        //effect.SpecularColor = new Vector3(1);
                        effect.Alpha = 1f;

                        effect.CurrentTechnique = effect.Techniques["BasicEffect"];
                        effect.LightingEnabled = false; // turn on the lighting subsystem
                        effect.FogEnabled = false;
                        effect.VertexColorEnabled = false;
                        effect.DiffuseColor = (Vector3)Mesh.Tag; //1, 1, 1 by default
                        effect.View = RotatingCam.viewMatrix; //* Matrix.CreateRotationX(MathHelper.ToRadians(offset.Y));
                                                                           //effect.View *= Matrix.CreateRotationY(MathHelper.ToRadians(offset.X));
                        effect.Projection = RotatingCam.projectionMatrix;

                        effect.World = LTreeTransForms[Mesh.ParentBone.Index];
                        //effect.World = SmartGardenCamera.worldMatrix;
                    }
                    Mesh.Draw();
                }
            }

            Graphics.GraphicsDevice.SetRenderTarget(null);
        }
        protected void DrawScreenBuffer()
        {
            Graphics.GraphicsDevice.SetRenderTarget(ScreenBackBuffer);
            Graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Graphics.GraphicsDevice.Clear(Color.AliceBlue);

            foreach (var Mesh in LTree.Meshes)
            {
                foreach (BasicEffect effect in Mesh.Effects)
                {
                    //effect.DirectionalLight0.Direction = new Vector3(0, 0, 0);
                    //effect.DirectionalLight0.Enabled = true;
                    //effect.DirectionalLight0.DiffuseColor = new Vector3(0.8f);
                    effect.EnableDefaultLighting();
                    effect.DiffuseColor = (Vector3)effect.Tag; //1, 1, 1 by default

                    //effect.EmissiveColor = new Vector3(1, 0, 0);
                    effect.Alpha = 1;
                    //effect.SpecularColor = new Vector3(1);
                    effect.AmbientLightColor = new Vector3(0.35f);

                    effect.View = RotatingCam.viewMatrix; //* Matrix.CreateRotationX(MathHelper.ToRadians(offset.Y));
                                                                       //effect.View *= Matrix.CreateRotationY(MathHelper.ToRadians(offset.X));
                    effect.Projection = RotatingCam.projectionMatrix;
                    effect.World = LTreeTransForms[Mesh.ParentBone.Index];
                    //effect.World = SmartGardenCamera.worldMatrix;
                }
                Mesh.Draw();
                    if (Mesh.Tag != null ? ((Vector3)Mesh.Tag).Y == CurrentColorUnderMouse : false)
                    {
                        foreach (BasicEffect effect in Mesh.Effects)
                        {
                            //effect.DirectionalLight0.Direction = new Vector3(0, 0, 0);
                            //effect.DirectionalLight0.Enabled = true;
                            //effect.DirectionalLight0.DiffuseColor = new Vector3(0.8f);
                            effect.EnableDefaultLighting();
                            effect.DiffuseColor = (Vector3)effect.Tag; //1, 1, 1 by default

                            //effect.EmissiveColor = new Vector3(1, 0, 0);
                            effect.Alpha = 0.5f;
                            //effect.SpecularColor = new Vector3(1);
                            effect.AmbientLightColor = new Vector3(1, 0, 0);

                            effect.View = RotatingCam.viewMatrix; //* Matrix.CreateRotationX(MathHelper.ToRadians(offset.Y));
                                                                  //effect.View *= Matrix.CreateRotationY(MathHelper.ToRadians(offset.X));
                            effect.Projection = RotatingCam.projectionMatrix;

                            effect.World = LTreeTransForms[Mesh.ParentBone.Index];
                            //effect.World = SmartGardenCamera.worldMatrix;
                        }
                        Mesh.Draw();
                    }
                


            }

        }
    }
}
