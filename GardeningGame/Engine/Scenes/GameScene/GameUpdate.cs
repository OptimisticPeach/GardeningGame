using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GardeningGame.Engine.Scenes.Common;

namespace GardeningGame.Engine.Scenes.Game
{
    public partial class GameScene
    {
        public Vector3 AffectedModelUUID = new Vector3();
        public Color CurrentColorUnderMouse;
        MouseState PreviousMouseState;
        public int Interpolation = 0;
        float SpeedInterpolation = 0;

        protected void updateKeys(GameTime time) {}


        public void Update(GameTime GT, MouseState MS, KeyboardState KS, bool IsActive)
        {
            Utils.waveTime += GameSceneVariables.WaveFactor / 10;

            updateKeys(GT);

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
        }
    }
}
