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
using static GardeningGame.Engine.Scenes.Game.GameSceneVariables;
using GardeningGame.Engine.Scenes.Common;

namespace GardeningGame.Engine.Scenes.Game
{
    public partial class GameScene : Scene
    {
        public event SceneChangeHandler OnRequestedSceneChanged;

        GraphicsDeviceManager Graphics;
        //Camera Cam = Camera.Empty;
        //List<Model> RawModels = new List<Model>();

        TimeSpan ButtonTime = new TimeSpan(0, 0, 0, 0, 250);
        RenderTarget2D SelectionBackBuffer;
        RenderTarget2D ScreenBackBuffer;
        bool ScreenOrSelection = false;

        GameSceneVariables GSV = new GameSceneVariables();

        PlantTile[,] Tiles;

        Terrain.Water Water;

        UI.SwingingSelector Selector;

        UI.Elements.HelpButton HelpButton = new UI.Elements.HelpButton();

        BasicEffect PrimitivesEffect;

        Dictionary<string, List<Model>> OrderedModels = new Dictionary<string, List<Model>>();

        private void SetMultiSampling(object sender, PreparingDeviceSettingsEventArgs e)
        {
            var pp = e.GraphicsDeviceInformation.PresentationParameters;
            pp.MultiSampleCount = 8;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public void Initialize(GraphicsDeviceManager GD)
        {
            /*//////////////////////////////////////////////////////////
            Cam.Width = GraphicsDevice.PresentationParameters.BackBufferWidth;
            Cam.Height = GraphicsDevice.PresentationParameters.BackBufferHeight;
            *///////////////////////////////////////////////////////////

            Tiles = new PlantTile[GSV.PlantTileCountX, GSV.PlantTileCountY];

            Graphics = GD;

            PrimitivesEffect = new BasicEffect(Graphics.GraphicsDevice);


            SelectionBackBuffer = new RenderTarget2D(Graphics.GraphicsDevice,
                Graphics.GraphicsDevice.PresentationParameters.BackBufferWidth,
                Graphics.GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24Stencil8);
            ScreenBackBuffer = new RenderTarget2D(Graphics.GraphicsDevice,
                Graphics.GraphicsDevice.PresentationParameters.BackBufferWidth,
                Graphics.GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24Stencil8);

            for (int x = 0; x < GSV.PlantTileCountX; x++)
            {
                for (int y = 0; y < GSV.PlantTileCountY; y++)
                {
                    Tiles[x, y] = new PlantTile();
                    Tiles[x, y].Position = new Vector3(((x * GSV.spacingX)) - (GSV.spacingX * 0.5f * GSV.PlantTileCountX), 0, ((y * GSV.spacingY)) - (GSV.spacingY * 0.5f * GSV.PlantTileCountY));

                    Entities.Bush shrub = new Entities.Bush(ref OrderedModels);
                    //bush.Position = new Vector3(Utils.RNG.Next((int)(0.2f * Consts.spacing), (int)(0.8f * Consts.spacing)), 0, Utils.RNG.Next((int)(0.2f * Consts.spacing), (int)(0.8f * Consts.spacing)));
                    shrub.Position = Tiles[x, y].GenPositionForEntity(shrub, GSV, ref OrderedModels);

                    for (int i = 0; i != 10; i++)
                    {
                        Entities.Flower flower = new Entities.Flower(ref OrderedModels);
                        flower.Position = Tiles[x, y].GenPositionForEntity(flower, GSV, ref OrderedModels);
                        flower.ScaleObject(new Vector3(1f));
                        Tiles[x, y].Add(flower);
                    }

                    Tiles[x, y].Add(shrub);

                    Tiles[x, y].Terrain = new Terrain.DirtPatch(PrimitivesEffect);

                    Tiles[x, y].Terrain.Generate(GSV.TerrainDepth, GSV.TerrainWidth, GSV.TerrainPointSpacing, Graphics.GraphicsDevice);
                }
            }

            ////////////Water////////////


            Water = new Terrain.Water(PrimitivesEffect);

            Water.GenerateCircle(GSV.WaterSize, GSV.WaterSize, GSV.WaterPointSpacing, GSV.WaterRadius, Graphics.GraphicsDevice);

            Water.Update(Graphics.GraphicsDevice, GSV, null, null, null);

            HelpButton.Position = new Point(0);

            HelpButton.Scale = 1;

            //IsFixedTimeStep = false;
            //TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 100.0f);

            Common.RotatingCam.Initialize(Graphics.GraphicsDevice, 1200);

            Graphics.GraphicsDevice.Clear(GameSceneVariables.clearColor);
        }



        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent(ContentManager Content)
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            /*/////////////////////////////////////////////////////////////////
            Cam.spriteBatch = new SpriteBatch(GraphicsDevice);
            *//////////////////////////////////////////////////////////////////

            UI.UIText.UIFrame.Initialize(Content);

            Utils.ModelList ModelNames = new Utils.ModelList();

            ModelNames = Newtonsoft.Json.JsonConvert.DeserializeObject<Utils.ModelList>(File.ReadAllText("VegitationPack.json"));

            UInt32 UUIDIndex = 0;

            foreach (var ModelList in ModelNames.Models)
            {
                if (ModelList[0] != "UnusedGrass")
                {
                    List<Model> CurrentModelList = new List<Model>();
                    for (int i = 1; i < ModelList.Count(); i++)
                    {
                        if (!ModelList[i].EndsWith(".psd"))
                        {
                            var model = Content.Load<Model>(ModelNames.RelativePath + ModelList[i]);
                            foreach (var a in model.Meshes)
                            {
                                a.Tag = new MeshTag()
                                {
                                    Scale = (((GSV.spacingX + GSV.spacingY) / 2f) - 275) / a.BoundingSphere.Radius,
                                    UUID = new Color((float)Utils.RNG.NextDouble(), (float)Utils.RNG.NextDouble(), 0).ToVector3()
                                };
                                UUIDIndex += 1;
                            }
                            CurrentModelList.Add(model);
                        }
                    }
                    OrderedModels.Add(ModelList[0], CurrentModelList);
                }
            }

            HelpButton.Load(Content);

            Entities.Bush.Sprite = Content.Load<Texture2D>(@"GUI\Bush");
            Entities.Flower.Sprite = Content.Load<Texture2D>(@"GUI\Flower");
            Entities.Reed.Sprite = Content.Load<Texture2D>(@"GUI\Reed");
            Entities.Shrub.Sprite = Content.Load<Texture2D>(@"GUI\Shrub");
            Entities.FlowerBush.Sprite = Content.Load<Texture2D>(@"GUI\FlowerBush");
        }
    }
}
