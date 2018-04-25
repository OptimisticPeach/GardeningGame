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
using MonoGame.Extended;

namespace GardeningGame.Engine.Scenes.LevelSelect
{
    public partial class LevelSelectScene : Scene
    {
        public event SceneChangeHandler OnRequestedSceneChanged;

        GraphicsDeviceManager Graphics;
        //Camera Cam = Camera.Empty;
        //List<Model> RawModels = new List<Model>();

        RenderTarget2D SelectionBackBuffer;
        RenderTarget2D ScreenBackBuffer;
        bool ScreenOrSelection = false;

        Model LTree { get; set; }

        BasicEffect PrimitivesEffect;

        public bool ContentLoaded { get; set; }

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

            ////////////Water////////////

            //IsFixedTimeStep = false;
            //TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 100.0f);

            Common.RotatingCam.Initialize(Graphics.GraphicsDevice, 2000, 100, true, 1400);

            Graphics.GraphicsDevice.Clear(Color.AliceBlue);

            foreach (var mesh in LTree.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.Tag = effect.DiffuseColor;
                }
            }

            RotatingCam.PrimitivesEffect = new BasicEffect(Graphics.GraphicsDevice);

            SBBData = new Color[SelectionBackBuffer.Height * SelectionBackBuffer.Width];
        }



        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent(ContentManager Content)
        {
            LTree = Content.Load<Model>("LevelTree");

            Random R = new Random();

            int i = 3;

            foreach (var mesh in LTree.Meshes)
            {
                if (mesh.Name.StartsWith("L"))
                {
                    mesh.Tag = new Color(255, i, 255).ToVector3();
                    i +=10;
                }
            }
            
            LTreeTransForms = new Matrix[LTree.Bones.Count];
            LTree.CopyAbsoluteBoneTransformsTo(LTreeTransForms);

            ContentLoaded = true;
        }
    }
}
