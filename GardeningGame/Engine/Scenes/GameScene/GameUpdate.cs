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

namespace GardeningGame.Engine.Scenes.Game
{
    public partial class GameScene
    {
        public Vector3 AffectedModelUUID = new Vector3();
        public Color CurrentColorUnderMouse;
        private KeyboardState PrevKBS;
        MouseState PreviousMouseState;
        MouseState PreviousMouseState2;
        public int Interpolation = 0;
        float SpeedInterpolation = 0;
        float IslandMovement = 0;
        const float IslandMovingTime = 500;

        protected void updateKeys(GameTime time)
        {

            // TODO: Add your update logic here

            var KeyBoardState = Keyboard.GetState();

            ScreenOrSelection = KeyBoardState.IsKeyDown(Keys.A);

            if (KeyBoardState.IsKeyDown(Keys.Z))
            {
                OnRequestedSceneChanged(this, SceneType.LevelSelect, null);
            }

            if (KeyBoardState.IsKeyDown(Keys.Y) && IslandMovement <= 0)
            {
                PlantTile[,] newTiles = new PlantTile[++GSV.PlantTileCountX, ++GSV.PlantTileCountY];
                for (int i = 0; i < GSV.PlantTileCountX; i++)
                {
                    for (int j = 0; j < GSV.PlantTileCountY; j++)
                    {
                        if (i < GSV.PlantTileCountX - 1 && j < GSV.PlantTileCountY - 1)
                        {
                            newTiles[i, j] = Tiles[i, j];
                            newTiles[i, j].OldPosition = newTiles[i, j].Position;
                            newTiles[i, j].NewPosition = new Vector3((((i) * GSV.spacingX)) - (GSV.spacingX * 0.5f * GSV.PlantTileCountX), 0, (((j) * GSV.spacingY)) - (GSV.spacingY * 0.5f * GSV.PlantTileCountY));
                        }
                        else
                        {
                            newTiles[i, j] = new PlantTile();
                            newTiles[i, j].OldPosition = new Vector3((((i - 0.5f) * GSV.spacingX)) - (GSV.spacingX * 0.5f * GSV.PlantTileCountX), 0, (((j - 0.5f) * GSV.spacingY)) - (GSV.spacingY * 0.5f * GSV.PlantTileCountY));
                            newTiles[i, j].NewPosition = new Vector3((((i) * GSV.spacingX)) - (GSV.spacingX * 0.5f * GSV.PlantTileCountX), 0, (((j) * GSV.spacingY)) - (GSV.spacingY * 0.5f * GSV.PlantTileCountY));
                            newTiles[i, j].Position = newTiles[i, j].OldPosition;
                            Entities.Bush shrub = new Entities.Bush(ref OrderedModels);
                            //bush.Position = new Vector3(Utils.RNG.Next((int)(0.2f * Consts.spacing), (int)(0.8f * Consts.spacing)), 0, Utils.RNG.Next((int)(0.2f * Consts.spacing), (int)(0.8f * Consts.spacing)));
                            shrub.Position = newTiles[i, j].GenPositionForEntity(shrub, GSV, ref OrderedModels);

                            for (int k = 0; k != 10; k++)
                            {
                                Entities.Flower flower = new Entities.Flower(ref OrderedModels);
                                flower.Position = newTiles[i, j].GenPositionForEntity(flower, GSV, ref OrderedModels);
                                flower.ScaleObject(new Vector3(1f));
                                newTiles[i, j].EntityList.Add(flower);
                            }

                            newTiles[i, j].EntityList.Add(shrub);

                            newTiles[i, j].Terrain = new Terrain.DirtPatch();

                            newTiles[i, j].Terrain.Generate(GSV.TerrainDepth, GSV.TerrainWidth, GSV.TerrainPointSpacing, Graphics.GraphicsDevice);
                        }
                    }
                }
                IslandMovement = IslandMovingTime;
                Tiles = newTiles;
            }
            else if (IslandMovement > 0)
            {
                IslandMovement -= (float)time.ElapsedGameTime.TotalMilliseconds;
                foreach (var b in Tiles)
                {
                    b.Position = Vector3.Lerp(b.NewPosition, b.OldPosition, IslandMovement / IslandMovingTime);
                } 
            }


            if (!KeyBoardState.IsKeyDown(Keys.F) && PrevKBS.IsKeyDown(Keys.F))
            {
                Selector = new SwingingSelector(Entities.Flower.Sprite);
                Selector.Entity = new Entities.Flower(ref OrderedModels);
            }
            if (!KeyBoardState.IsKeyDown(Keys.B) && PrevKBS.IsKeyDown(Keys.B))
            {
                Selector = new SwingingSelector(Entities.Bush.Sprite);
                Selector.Entity = new Entities.Bush(ref OrderedModels);
            }
            if (!KeyBoardState.IsKeyDown(Keys.R) && PrevKBS.IsKeyDown(Keys.R))
            {
                Selector = new SwingingSelector(Entities.Reed.Sprite);
                Selector.Entity = new Entities.Reed(ref OrderedModels);
            }
            if (!KeyBoardState.IsKeyDown(Keys.U) && PrevKBS.IsKeyDown(Keys.U))
            {
                Selector = new SwingingSelector(Entities.FlowerBush.Sprite);
                Selector.Entity = new Entities.FlowerBush(ref OrderedModels);
            }
            if (!KeyBoardState.IsKeyDown(Keys.S) && PrevKBS.IsKeyDown(Keys.S))
            {
                Selector = new SwingingSelector(Entities.Shrub.Sprite);
                Selector.Entity = new Entities.Shrub(ref OrderedModels);
            }


            if (KeyBoardState.IsKeyDown(Keys.X))
            {
                for (int i = 1; i != 100000; i++)
                {
                    var b = Math.Log10(Math.Tan(Math.Tanh(Math.Sqrt(i)))) / Math.Log10(Math.E);
                }
            }

            PrevKBS = KeyBoardState;

        }


        public void Update(GameTime GT, MouseState MS, KeyboardState KS, bool IsActive)
        {
            //int i = 1;
            //foreach (var b in Tiles)
            //{
            //    b.Position.Y = Utils.generateOffset(i++, i++, 0.45f, 0.8f) * 2;
            //}

            Utils.waveTime += GameSceneVariables.WaveFactor / 10;

            updateKeys(GT);

            if (ButtonStatus.DraggedButton == null && ButtonStatus.PreviouslyDraggedButton == null && ButtonStatus.ClickedButton == null && Selector == null && IsActive)
            {
                if (MS.LeftButton == ButtonState.Pressed)
                {
                    Cam.Rotate(Utils.Map(
                        ((MS.X - PreviousMouseState.X) + (PreviousMouseState.X - PreviousMouseState2.X)) / 2,
                        -200, 200, -1, 1
                        ), false);

                }
                else if (MS.LeftButton == ButtonState.Released && PreviousMouseState.LeftButton == ButtonState.Pressed)
                {
                    SpeedInterpolation = Utils.Map(
                    ((MS.X - PreviousMouseState.X) + (PreviousMouseState.X - PreviousMouseState2.X)) / 2,
                    -300, 300, -1, 1
                    );
                    Interpolation = (int)MathHelper.Clamp((150f * SpeedInterpolation), -100, 100);
                }
            }
            else if (Selector != null)
            {
                Selector.Location = MS.Position.ToVector2();
                Selector.Update();
                if (PreviousMouseState.LeftButton == ButtonState.Pressed && MS.LeftButton == ButtonState.Released)
                {
                    CurrentColorUnderMouse = Utils.getColorAt(SelectionBackBuffer, MS.X, MS.Y);
                    foreach (var b in Tiles)
                    {
                        if (b.Terrain.ID == CurrentColorUnderMouse)
                        {
                            Selector.Entity.Position = b.GenPositionForEntity(Selector.Entity, GSV, ref OrderedModels);
                            Selector.Entity.ScaleObject(new Vector3(1f));
                            b.EntityList.Add(Selector.Entity);
                        }
                    }
                    Selector = null;
                }
            }
            //Water.Update(Graphics.GraphicsDevice, GSV, GT, MS, KS);

            PreviousMouseState2 = PreviousMouseState;
            PreviousMouseState = MS;

            if (Interpolation != 0)
            {
                var N = (1f / (200f * SpeedInterpolation)) * Interpolation;

                Cam.Rotate(MathHelper.Lerp(0, SpeedInterpolation, N), false);
                if (Interpolation > 0)
                {
                    Interpolation--;
                }
                else
                {
                    Interpolation++;
                }
            }
            else
            {
                SpeedInterpolation = 0;
            }

            HelpButton.Update(GT, MS, KS);

            if (HelpButton.Toggled)
                UI.UIText.UIFrame.Overlay.Update(MS);
        }
    }
}
