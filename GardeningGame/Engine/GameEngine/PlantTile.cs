using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GardeningGame.Engine.Scenes.Game.Entities;
using GardeningGame.Engine.Scenes.Common;

namespace GardeningGame.Engine.Scenes.Game
{
    public class PlantTile
    {
        public List<Entity> EntityList = new List<Entity>();
        public Terrain.DirtPatch Terrain;
        public Vector3 Position;
        public Vector3 OldPosition;
        public Vector3 NewPosition;

        public Vector3 GenPositionForEntity(Entity NewEntity, GameSceneVariables GSV, ref Dictionary<string, List<Model>> sourceForModels)
        {
            return new Vector3(Utils.RNG.Next(GSV.TerrainPointSpacing, GSV.TerrainPointSpacing * (GSV.TerrainWidth - 2)),
                               10,
                               Utils.RNG.Next(GSV.TerrainPointSpacing, GSV.TerrainPointSpacing * (GSV.TerrainWidth - 2)));
        }
        
        public PlantTile() { }
        public MockPlantTile GetMockPlantTile()
        {
            return new MockPlantTile()
            {
                Position = Position,
                EntityList = EntityList.Select((a)=>a.toMockEntity()).ToList()
            };
        }
    }
    /// <summary>
    /// Used for saving to disk
    /// </summary>
    [Serializable]
    public class MockPlantTile
    {
        public List<MockEntity> EntityList;
        public Vector3Serializable Position;
        public static implicit operator PlantTile(MockPlantTile source)
        {
            return new PlantTile()
            {
                EntityList = source.EntityList.Select((a)=>a.toEntity()).ToList(),
                Position = source.Position
            };
        }
    }

}
