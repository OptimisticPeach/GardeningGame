using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace GardeningGame.Engine.Scenes
{
    public delegate void SceneChangeHandler(Scene Sender, SceneType TypeToSwitchTo, EventArgs args);

    public enum SceneType
    {
        Game,
        LevelSelect
    }

    public interface Scene
    {
        bool ContentLoaded { get; set; }
        void Draw(GameTime GT);
        void Update(GameTime GT, MouseState MS, KeyboardState KS);
        void LoadContent(ContentManager Content);
        void Initialize(GraphicsDeviceManager GD);
        void SaveData(DataWriter S);
        void LoadData(DataLoader S);
        event SceneChangeHandler OnRequestedSceneChanged;
    }
}
