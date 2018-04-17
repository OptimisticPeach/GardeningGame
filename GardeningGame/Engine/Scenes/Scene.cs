using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace GardeningGame.Engine.Scenes
{
    public delegate void SceneChangeHandler(Scene Sender, ChangeArgs Args);

    public class ChangeArgs : EventArgs
    {

    }

    public interface Scene
    {
        void Draw(GameTime GT);
        void Update(GameTime GT, MouseState MS, KeyboardState KS);
        void LoadContent(ContentManager Content);
        void Initialize(GraphicsDeviceManager GD);
        event SceneChangeHandler OnRequestedSceneChanged;
    }
}
