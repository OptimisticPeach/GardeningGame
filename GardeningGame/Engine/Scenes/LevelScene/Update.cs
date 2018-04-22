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
using GardeningGame.Engine.Scenes.Common;

namespace GardeningGame.Engine.Scenes.LevelSelect
{
    public partial class LevelSelectScene
    {
        public int AffectedModelUUID = -1;
        public int CurrentColorUnderMouse;

        float Delta = 0;
        Vector3 Start; //Height, Radius, Rotation
        Vector3 End;

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
            if (KeyBoardState.IsKeyDown(Keys.Space))
                RotatingCam.Rotate(0.1f);
        }


        public void Update(GameTime GT, MouseState MS, KeyboardState KS)
        {
            //int i = 1;
            //foreach (var b in Tiles)
            //{
            //    b.Position.Y = Utils.generateOffset(i++, i++, 0.45f, 0.8f) * 2;
            //}
            updateKeys(GT, KS);

            if(MS.LeftButton == ButtonState.Pressed)
            {
                foreach(var m in LTree.Meshes)
                {
                    if (m.Name.StartsWith("L"))
                    {
                        if((int)m.Tag == CurrentColorUnderMouse)
                        {
                            AffectedModelUUID = -1;//(int)m.Tag;
                            Start = RotatingCam.CylindricalCoords; //Height Radius Rotation
                            End = new Vector3(0);
                            Vector3 MeshPos;
                            {
                                Matrix[] modelTransforms = new Matrix[LTree.Bones.Count];
                                LTree.CopyAbsoluteBoneTransformsTo(modelTransforms);
                                MeshPos = m.GetPosition(modelTransforms, Matrix.CreateTranslation(0, 0, 0));
                                MeshPos += m.BoundingSphere.Center;
                            }

                            End.X = MeshPos.Y;
                            End.Y = (float)Math.Sqrt(MeshPos.X * MeshPos.X + MeshPos.Y * MeshPos.Y);
                            End.Z = (float)Math.Atan2(MeshPos.Z, MeshPos.X);
                            RotatingCam.Height = End.X;
                            RotatingCam.setPosition();
                            Delta = 0;
                            break;
                        }
                    }
                }
            }

            if(AffectedModelUUID >= 0)
            {
                Delta += (float)GT.ElapsedGameTime.Milliseconds;
                //RotatingCam.CylindricalCoords = Vector3.Lerp(Start, End, Delta / 1000f);
                RotatingCam.Height = MathHelper.Lerp(Start.X, End.X, Delta / 1000f);
                RotatingCam.Radius = MathHelper.Lerp(Start.Y, End.Y, Delta / 1000f);
                RotatingCam.Rotation = MathHelper.Lerp(Start.Z, End.Z, Delta / 1000f);
                RotatingCam.setPosition();

                if (Delta >= 1000)
                {
                    AffectedModelUUID = -1;
                    Delta = 0;
                    End = new Vector3(0);
                    Start = new Vector3(0);
                }
            }

            //RotatingCam.Rotate(0.01f);

            Color[] SBBData = new Color[SelectionBackBuffer.Width * SelectionBackBuffer.Height];

            SelectionBackBuffer.GetData(SBBData);

            try
            {
                CurrentColorUnderMouse = SBBData[MS.X + MS.Y * SelectionBackBuffer.Width].G;
            }
            catch
            {
                CurrentColorUnderMouse = 0;
            }
        }
    }
}
