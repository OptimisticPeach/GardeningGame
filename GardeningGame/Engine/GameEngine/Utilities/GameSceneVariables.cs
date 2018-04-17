using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GardeningGame.Engine.Scenes.Game
{
    public class GameSceneVariables
    {
        #region Public Non-Static Variables
        public int TerrainWidth = 10;
        public int TerrainDepth = 10;
        public int TerrainPointSpacing = 57;
        public int WaterSize = 100;
        public int WaterPointSpacing = 51;
        public int TotalWaterSize => WaterSize * WaterPointSpacing;
        public int PlantTileCountX = 3;
        public int PlantTileCountY = 3;
        public float WaterRadius = 2500;
        public int spacingX => TerrainWidth * TerrainPointSpacing;
        public int spacingY => TerrainDepth * TerrainPointSpacing;
        #endregion


        public const float WaveFactor = 0.06f;
        public static readonly Color clearColor = Color.AliceBlue;
        public static Color AccentColor { get; private set; }
        public static readonly bool DEBUG = true;
        public const string Debugstring = "Cam_X:{0}\nCam_Y:{1}\nCam_Z:{2}\nCT_X:{3}\nCT_Y:{4}\nCT_Z:{5}\nDist:{8}\nFPS:{6:N2}\nScroll:{7}\nRot:{9}\n{10}";

        static GameSceneVariables()
        {
            AccentColor = Utils.GetAccent();
            System.Windows.SystemParameters.StaticPropertyChanged += UpdateColour;
        }

        private static void UpdateColour(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            AccentColor = Utils.GetAccent();
        }
    }
}
