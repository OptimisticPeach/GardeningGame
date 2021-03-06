﻿using System;
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

        Terrain.WaterWithEffect WaterWithEffect = new Terrain.WaterWithEffect();
        WaterEffect WEffect;

        UI.SwingingSelector Selector;

        UI.Elements.HelpButton HelpButton = new UI.Elements.HelpButton();

        BasicEffect PrimitivesEffect;

        GameCam Cam = new GameCam();

        EffectReloader EffectReloader;

        Dictionary<string, List<Model>> OrderedModels = new Dictionary<string, List<Model>>();

        public bool ContentLoaded { get; set; }

        bool Initialized;

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

            InitGraphics(true);

            LoadData();

            if (!CouldLoadFile)
            {

                for (int x = 0; x < GSV.PlantTileCountX; x++)
                {
                    for (int y = 0; y < GSV.PlantTileCountY; y++)
                    {
                        Tiles[x, y] = new PlantTile
                        {
                            Position = new Vector3(((x * GSV.spacingX)) - (GSV.spacingX * 0.5f * GSV.PlantTileCountX), 0, ((y * GSV.spacingY)) - (GSV.spacingY * 0.5f * GSV.PlantTileCountY))
                        };

                        Entities.Bush shrub = new Entities.Bush(ref OrderedModels);
                        //bush.Position = new Vector3(Utils.RNG.Next((int)(0.2f * Consts.spacing), (int)(0.8f * Consts.spacing)), 0, Utils.RNG.Next((int)(0.2f * Consts.spacing), (int)(0.8f * Consts.spacing)));
                        shrub.Position = Tiles[x, y].GenPositionForEntity(shrub, GSV, ref OrderedModels);

                        for (int i = 0; i != 10; i++)
                        {
                            Entities.Flower flower = new Entities.Flower(ref OrderedModels);
                            flower.Position = Tiles[x, y].GenPositionForEntity(flower, GSV, ref OrderedModels);
                            flower.ScaleObject(new Vector3(1f));
                            Tiles[x, y].EntityList.Add(flower);
                        }

                        Tiles[x, y].EntityList.Add(shrub);

                        Tiles[x, y].Terrain = new Terrain.DirtPatch();

                        Tiles[x, y].Terrain.Generate(GSV.TerrainDepth, GSV.TerrainWidth, GSV.TerrainPointSpacing, Graphics.GraphicsDevice);
                    }
                }
            }

            ////////////Water////////////


            Water = new Terrain.Water();

            Water.GenerateCircle(GSV.WaterSize, GSV.WaterSize, GSV.WaterPointSpacing, GSV.WaterRadius, Graphics.GraphicsDevice);

            Water.Update(Graphics.GraphicsDevice, GSV, null, null, null);

            //WaterWithEffect.Generate()

            HelpButton.Position = new Point(0);

            HelpButton.Scale = 1;

            EffectReloader = new EffectReloader(@"..\..\..\..\Content", @"PrimitivesEffect.fx", GD.GraphicsDevice);
            EffectReloader.OnEffectChanged += onPrimitivesEffectChanged;

            WEffect.InitShaders(@"Effects\WaterShader.fx", Graphics.GraphicsDevice);
            WaterWithEffect.GenerateCircle(GSV.WaterSize, GSV.WaterSize, GSV.WaterPointSpacing, GSV.WaterRadius, Graphics.GraphicsDevice);

            Initialized = true;
        }

        public void onPrimitivesEffectChanged(Effect E)
        {
            Cam.PEffect.InternalEffect = E;
        }

        public void InitGraphics(bool? Override)
        {
            if (Override.HasValue ? Override.Value : false || Initialized)
            {
                Cam.Initialize(Graphics.GraphicsDevice, 1200, 800, false, 1400);

                Graphics.GraphicsDevice.Clear(clearColor);

                Graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;

                OnRequestedSceneChanged += GameScene_OnRequestedSceneChanged;

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
            }
        }

        public void OnSizeChanged(object sender, EventArgs e)
        {
            InitGraphics(null);
        }

        private void GameScene_OnRequestedSceneChanged(Scene Sender, SceneType TypeToSwitchTo, EventArgs args)
        {
            SaveData();
        }

        bool CouldLoadFile = false;
        public void LoadData()
        {
            if (File.Exists("Game_Data.json"))
            {
                var gamedata = GameIO.ReadFromJsonFile<DataObjects.GameDataObject>("Game_Data.json");
                CouldLoadFile = true;
                GSV = gamedata.GameSceneVariables;
                Tiles = new PlantTile[gamedata.Tiles.GetLength(0), gamedata.Tiles.GetLength(1)];
                for (int i = 0; i != gamedata.Tiles.GetLength(0); i++)
                {
                    for (int j = 0; j != gamedata.Tiles.GetLength(1); j++)
                    {
                        Tiles[i, j] = gamedata.Tiles[i, j];
                        Tiles[i, j].Terrain = new Terrain.DirtPatch();
                        Tiles[i, j].Terrain.Generate(GSV.TerrainDepth, GSV.TerrainWidth, GSV.TerrainPointSpacing, Graphics.GraphicsDevice);
                    }
                }
            }
        }

        public void SaveData()
        {
            GameIO.WriteToJsonFile("Game_Data.json", GetGameDataObject());
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

            ContentLoaded = true;

            Cam.PEffect = new PrimitiveEffect()
            {
                InternalEffect = Content.Load<Effect>("PrimitivesEffect")
            };

            WEffect = new WaterEffect()
            {
                InternalEffect = Content.Load<Effect>("WaterShader")
            };
        }

        public DataObjects.GameDataObject GetGameDataObject()
        {
            var r = new DataObjects.GameDataObject()
            {
                GameSceneVariables = GSV
            };
            r.Tiles = new MockPlantTile[Tiles.GetLength(0), Tiles.GetLength(1)];
            for(int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    r.Tiles[i, j] = Tiles[i, j].GetMockPlantTile();
                }
            }
            return r;
        }
    }
}
