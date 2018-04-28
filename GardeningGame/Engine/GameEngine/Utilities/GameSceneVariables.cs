using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GardeningGame.Engine.Scenes.Common;

namespace GardeningGame.Engine.Scenes.Game
{
    [Serializable]
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
