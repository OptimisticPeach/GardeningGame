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
        public bool IsMoving = false;
        public float CurrentColorUnderMouse;

        float Delta = 0;
        Vector3 Start = new Vector3(); //Height, Radius, Rotation
        Vector3 End = new Vector3();
        bool PreviouslyPressed = false;
        private Color[] SBBData;
        string LevelSelected = "";

        public class LevelSelectSceneSendingArgs : EventArgs
        {
            public string Name;
        }

        protected void updateKeys(GameTime time, KeyboardState KeyBoardState)
        {
            ScreenOrSelection = KeyBoardState.IsKeyDown(Keys.A);

            //if (KeyBoardState.IsKeyDown(Keys.Enter))
            //{
            //    new Vector3();
            //}

            if (KeyBoardState.IsKeyDown(Keys.Z))
            {
                OnRequestedSceneChanged(this, SceneType.Game, new LevelSelectSceneSendingArgs() { Name = LevelSelected });
            }
            if (KeyBoardState.IsKeyDown(Keys.X))
            {
                for (int i = 1; i != 100000; i++)
                {
                    var b = Math.Log10(Math.Tan(Math.Tanh(Math.Sqrt(i)))) / Math.Log10(Math.E);
                }
            }
            if (KeyBoardState.IsKeyDown(Keys.Space))
                Cam.Rotate(0.1f, true);
        }


        public void Update(GameTime GT, MouseState MS, KeyboardState KS, bool IsActive)
        {
            //int i = 1;
            //foreach (var b in Tiles)
            //{
            //    b.Position.Y = Utils.generateOffset(i++, i++, 0.45f, 0.8f) * 2;
            //}
            updateKeys(GT, KS);

            if (MS.LeftButton == ButtonState.Pressed && !PreviouslyPressed)
            {
                foreach (var m in LTree.Meshes)
                {
                    if (m.Name.StartsWith("L"))
                    {
                        if (((Vector3)m.Tag).Y == CurrentColorUnderMouse)
                        {
                            LevelSelected = m.Name;
                            End = new Vector3(0);
                            IsMoving = true;
                            //Start = (Vector3)((object)RotatingCam.CylindricalCoords); //Height Radius Rotation
                            Start.X = Cam.Height;
                            Start.Y = Cam.Radius;
                            Start.Z = Cam.Rotation;

                            var MeshPos = m.BoundingSphere.Center;

                            MeshPos += m.ParentBone.ModelTransform.Translation;
                            MeshPos += m.ParentBone.Transform.Translation;



                            End.X = MeshPos.Y;
                            End.Y = (float)Math.Sqrt(MeshPos.X * MeshPos.X + MeshPos.Z * MeshPos.Z) + 800;
                            End.Z = (float)Math.Atan2(MeshPos.Z, MeshPos.X);
                            
                            Delta = 0;
                            break;
                        }
                    }
                }
            }

            if (IsMoving)
            {
                Delta += (float)GT.ElapsedGameTime.TotalSeconds;
                //RotatingCam.CylindricalCoords = Vector3.Lerp(Start, End, Delta);

                Start.X += Cam.Height;
                Start.Y += Cam.Radius;
                Start.Z += Cam.Rotation;

                Start.X /= 2;
                Start.Y /= 2;
                Start.Z /= 2;

                Cam.Height = MathHelper.Lerp(Start.X, End.X, Delta);
                Cam.Radius = MathHelper.Lerp(Start.Y, End.Y, Delta);
                Cam.Rotation = MathHelper.Lerp(Start.Z, End.Z, Delta);

                Cam.setPosition(true);

                //RotatingCam.Position = Vector3.Lerp(Start, End, Delta / 1000f);

                if (Delta >= 1)
                {
                    IsMoving = false;
                    Delta = 0;
                    //End = new Vector3(0);
                    //Start = new Vector3(0);
                }
            }

            //RotatingCam.Rotate(0.01f);

            SelectionBackBuffer.GetData(SBBData);

            if (MS.X > 0 && MS.Y > 0 && MS.X < SelectionBackBuffer.Width && MS.Y < SelectionBackBuffer.Height)
                CurrentColorUnderMouse = SBBData[MS.X + MS.Y * SelectionBackBuffer.Width].G / 255f;

            foreach (var m in LTree.Meshes)
            {
                if (m.Name.StartsWith("L"))
                {
                    if (((Vector3)m.Tag).Y == CurrentColorUnderMouse)
                    {
                        var MeshPos = m.BoundingSphere.Center;

                        MeshPos += m.ParentBone.ModelTransform.Translation;
                        MeshPos += m.ParentBone.Transform.Translation;
                    }
                }
            }
            PreviouslyPressed = MS.LeftButton == ButtonState.Pressed;
        }
    }
}
