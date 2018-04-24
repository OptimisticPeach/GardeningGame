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
        bool PreviouslyPressed = false;
        private Color[] SBBData;

        protected void updateKeys(GameTime time, KeyboardState KeyBoardState)
        {
            ScreenOrSelection = KeyBoardState.IsKeyDown(Keys.A);

            if (KeyBoardState.IsKeyDown(Keys.W))
            {

            }

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

            if (MS.LeftButton == ButtonState.Pressed && !PreviouslyPressed)
            {
                foreach (var m in LTree.Meshes)
                {
                    if (m.Name.StartsWith("L"))
                    {
                        if ((int)m.Tag == CurrentColorUnderMouse)
                        {
                            End = new Vector3(0);
                            AffectedModelUUID = (int)m.Tag;
                            Start = (Vector3)((object)RotatingCam.CylindricalCoords); //Height Radius Rotation

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

            if (AffectedModelUUID >= 0)
            {
                Delta += (float)GT.ElapsedGameTime.TotalSeconds;
                RotatingCam.CylindricalCoords = Vector3.Lerp(Start, End, Delta);
                RotatingCam.setPosition();

                //RotatingCam.Position = Vector3.Lerp(Start, End, Delta / 1000f);

                if (Delta >= 1)
                {
                    AffectedModelUUID = -1;
                    Delta = 0;
                    //End = new Vector3(0);
                    //Start = new Vector3(0);
                }
            }

            //RotatingCam.Rotate(0.01f);

            SelectionBackBuffer.GetData(SBBData);

            if (MS.X > 0 && MS.Y > 0 && MS.X < SelectionBackBuffer.Width && MS.Y < SelectionBackBuffer.Height)
                CurrentColorUnderMouse = SBBData[MS.X + MS.Y * SelectionBackBuffer.Width].G;

            foreach (var m in LTree.Meshes)
            {
                if (m.Name.StartsWith("L"))
                {
                    if ((int)m.Tag == CurrentColorUnderMouse)
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
