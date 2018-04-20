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

            Common.SmartGardenCamera.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.LinearClamp, DepthStencilState.Default,
                RasterizerState.CullNone);

            Common.SmartGardenCamera.spriteBatch.Draw(ScreenOrSelection ? SelectionBackBuffer : ScreenBackBuffer, new Rectangle(0, 0, Graphics.GraphicsDevice.PresentationParameters.BackBufferWidth,
                Graphics.GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
            if (Debug.DEBUG)
            {
                Common.SmartGardenCamera.spriteBatch.DrawString(Debug.DebugFont,
                    String.Format(
                        Debug.Debugstring,
                        Common.SmartGardenCamera.Position.X,
                        Common.SmartGardenCamera.Position.Y,
                        Common.SmartGardenCamera.Position.Z,
                        Common.SmartGardenCamera.Target.X,
                        Common.SmartGardenCamera.Target.Y,
                        Common.SmartGardenCamera.Target.Z,
                        Mouse.GetState().ScrollWheelValue,
                        Vector3.Distance(Common.SmartGardenCamera.Position, new Vector3(0)),
                        Common.SmartGardenCamera._rotation,
                        0,0),
                    //Utils.TextureTo2DArray(ScreenBackBuffe)[Mouse.GetState().X, Mouse.GetState().Y]),
                    new Vector2(10, 50), Color.Black);
            }

            Common.SmartGardenCamera.spriteBatch.End();
        }

        protected void DrawSelectionBuffer()
        {
            Graphics.GraphicsDevice.SetRenderTarget(SelectionBackBuffer);
            Graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Graphics.GraphicsDevice.Clear(Color.AliceBlue);

            Matrix[] modelTransforms = new Matrix[LTree.Bones.Count];
            LTree.CopyAbsoluteBoneTransformsTo(modelTransforms);

            foreach (var Mesh in LTree.Meshes)
            {
                if (Mesh.Tag is Color)
                {
                    foreach (BasicEffect effect in Mesh.Effects)
                    {
                        //effect.DirectionalLight0.Direction = new Vector3(0, 0, 0);
                        //effect.DirectionalLight0.Enabled = true;
                        //effect.DirectionalLight0.DiffuseColor = new Vector3(0.8f);
                        effect.EnableDefaultLighting();

                        //effect.EmissiveColor = new Vector3(1, 0, 0);

                        /*
                         * Effect.CurrentTechnique = Effect.Techniques["BasicEffect"];
                         * Effect.LightingEnabled = false; // turn on the lighting subsystem
                         * Effect.FogEnabled = false;
                         * Effect.VertexColorEnabled = false;
                         * Effect.DiffuseColor = ID.ToVector3(); //1, 1, 1 by default
                         */

                        //effect.SpecularColor = new Vector3(1);

                        effect.CurrentTechnique = effect.Techniques["BasicEffect"];
                        effect.LightingEnabled = false; // turn on the lighting subsystem
                        effect.FogEnabled = false;
                        effect.VertexColorEnabled = false;
                        effect.DiffuseColor = ((Color)Mesh.Tag).ToVector3(); //1, 1, 1 by default

                        effect.AmbientLightColor = new Vector3(1);

                        effect.View = Common.SmartGardenCamera.viewMatrix; //* Matrix.CreateRotationX(MathHelper.ToRadians(offset.Y));
                                                                    //effect.View *= Matrix.CreateRotationY(MathHelper.ToRadians(offset.X));
                        effect.Projection = Common.SmartGardenCamera.projectionMatrix;

                        effect.World = modelTransforms[Mesh.ParentBone.Index];
                        //effect.World = SmartGardenCamera.worldMatrix;
                    }
                }
                Mesh.Draw();
            }

            Graphics.GraphicsDevice.SetRenderTarget(null);
        }
        protected void DrawScreenBuffer()
        {
            Graphics.GraphicsDevice.SetRenderTarget(ScreenBackBuffer);
            Graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Graphics.GraphicsDevice.Clear(Color.AliceBlue);

            Matrix[] modelTransforms = new Matrix[LTree.Bones.Count];
            LTree.CopyAbsoluteBoneTransformsTo(modelTransforms);

            foreach (var Mesh in LTree.Meshes)
            {
                foreach (BasicEffect effect in Mesh.Effects)
                {
                    //effect.DirectionalLight0.Direction = new Vector3(0, 0, 0);
                    //effect.DirectionalLight0.Enabled = true;
                    //effect.DirectionalLight0.DiffuseColor = new Vector3(0.8f);
                    effect.EnableDefaultLighting();

                    //effect.EmissiveColor = new Vector3(1, 0, 0);

                    //effect.SpecularColor = new Vector3(1);
                    effect.AmbientLightColor = new Vector3(0.35f);

                    effect.View = Common.SmartGardenCamera.viewMatrix; //* Matrix.CreateRotationX(MathHelper.ToRadians(offset.Y));
                                                                //effect.View *= Matrix.CreateRotationY(MathHelper.ToRadians(offset.X));
                    effect.Projection = Common.SmartGardenCamera.projectionMatrix;

                    effect.World = modelTransforms[Mesh.ParentBone.Index];
                    //effect.World = SmartGardenCamera.worldMatrix;
                }

                Mesh.Draw();
            }

            Graphics.GraphicsDevice.SetRenderTarget(null);
        }
    }
}
