using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardeningGame.Engine.DataObjects
{
    [Serializable]
    public struct GameDataObject
    {
        public Scenes.Game.GameSceneVariables GameSceneVariables;
        public Scenes.Game.MockPlantTile[,] Tiles;
    }
}
