using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using static GardeningGame.Engine.Scenes.Game.GameSceneVariables;
using GardeningGame.Engine.Scenes;

namespace GardeningGame.Engine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Gardening : Game
    {
        GraphicsDeviceManager Graphics;

        Scenes.Game.GameScene GameScene = new Scenes.Game.GameScene();
        Scenes.LevelSelect.LevelSelectScene SelectScene = new Scenes.LevelSelect.LevelSelectScene();

        Scene CurrentScene;

        public Gardening()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Graphics.PreparingDeviceSettings += SetMultiSampling;
            Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            //Graphics.PreferredBackBufferHeight = 1080;
            //Graphics.PreferredBackBufferWidth = 1920;
            //Graphics.ToggleFullScreen();
            CurrentScene = SelectScene;

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += GameScene.OnSizeChanged;
            Window.ClientSizeChanged += SelectScene.OnSizeChanged;
        }

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
        protected override void Initialize()
        {
            base.Initialize();

            IsMouseVisible = true;

            GameScene.OnRequestedSceneChanged += ChangeScene;
            SelectScene.OnRequestedSceneChanged += ChangeScene;

            CurrentScene.Initialize(Graphics);
        }

        float Delta = 0;

        public void ChangeScene (Scene Sender, SceneType TypeToSwitchTo, EventArgs args)
        {
            if (Delta <= 0)
            {
                switch (TypeToSwitchTo)
                {
                    case SceneType.Game:
                        CurrentScene = GameScene;
                        if (!CurrentScene.ContentLoaded)
                            CurrentScene.LoadContent(Content);
                        CurrentScene.Initialize(Graphics);
                        break;
                    case SceneType.LevelSelect:
                        SelectScene = null;
                        SelectScene = new Scenes.LevelSelect.LevelSelectScene();
                        SelectScene.OnRequestedSceneChanged += ChangeScene;
                        CurrentScene = SelectScene;
                        if (!CurrentScene.ContentLoaded)
                            CurrentScene.LoadContent(Content);
                        CurrentScene.Initialize(Graphics);
                        break;
                }
                Delta = 10f;
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            CurrentScene.LoadContent(Content);
            //if (Debug.DEBUG)
            //{
                Scenes.Common.Debug.DebugFont = Content.Load<SpriteFont>("UI\\arial");
            //}
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            CurrentScene.Draw(gameTime);

            if (true)
            {
                Scenes.Common.Camera.SpriteBatch.Begin();
                Scenes.Common.Camera.SpriteBatch.DrawString(Scenes.Common.Debug.DebugFont, Delta.ToString(), new Vector2(100, 100), Color.DarkSalmon);
                Scenes.Common.Camera.SpriteBatch.End();
            }

            base.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            var KeyBoardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || KeyBoardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            MouseState ms = Mouse.GetState();

            CurrentScene.Update(gameTime, ms, KeyBoardState, IsActive);

            if (Delta > 0)
                Delta--;

            base.Update(gameTime);

        }
    }
}
