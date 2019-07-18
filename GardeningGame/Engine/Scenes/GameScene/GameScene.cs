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
        GraphicsDeviceManager Graphics;

        TimeSpan ButtonTime = new TimeSpan(0, 0, 0, 0, 250);
        RenderTarget2D ScreenBackBuffer;
        bool ScreenOrSelection = false;

        GameSceneVariables GSV = new GameSceneVariables();

        Terrain.Water Water;

        Terrain.WaterWithEffect WaterWithEffect = new Terrain.WaterWithEffect();
        WaterEffect WEffect;

        BasicEffect PrimitivesEffect;

        GameCam Cam = new GameCam();

        EffectReloader EffectReloader;

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
            Graphics = GD;

            PrimitivesEffect = new BasicEffect(Graphics.GraphicsDevice);

            InitGraphics(true);

            ////////////Water////////////

            EffectReloader = new EffectReloader(@"..\..\..\..\Content", @"WaterShader.fx", GD.GraphicsDevice);
            EffectReloader.OnEffectChanged += onWaterEffectChanged;

            WEffect.InitShaders(@"..\..\..\..\Content\WaterShader.fx", Graphics.GraphicsDevice);
            WaterWithEffect.GenerateCircle(GSV.WaterSize, GSV.WaterSize, GSV.WaterPointSpacing, GSV.WaterRadius, Graphics.GraphicsDevice);

            Initialized = true;
        }

        public void onWaterEffectChanged(Effect E)
        {
            WEffect.InternalEffect = E;
        }

        public void InitGraphics(bool? Override)
        {
            if (Override.HasValue ? Override.Value : false || Initialized)
            {
                Cam.Initialize(Graphics.GraphicsDevice, 1200, 800, false, 1400);

                Graphics.GraphicsDevice.Clear(clearColor);

                Graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                
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

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent(ContentManager Content)
        {
            WEffect = new WaterEffect()
            {
                InternalEffect = Content.Load<Effect>("WaterShader")
            };
        }
    }
}
