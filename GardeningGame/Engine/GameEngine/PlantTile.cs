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
        private List<Entity> _internalEntityList = new List<Entity>();
        public System.Collections.ObjectModel.ReadOnlyCollection<Entity> EntityList { get => _internalEntityList.AsReadOnly(); }
        public Terrain.DirtPatch Terrain;
        public Vector3 Position;
        public Vector3 OldPosition;
        public Vector3 NewPosition;
        public Rectangle AlphaChangingArea; 
        public Model TerrainModel;
        
        public void Add(Entity entity)
        {
            _internalEntityList.Add(entity);
        }

        public Vector3 GenPositionForEntity(Entity NewEntity, GameSceneVariables GSV, ref Dictionary<string, List<Model>> sourceForModels)
        {
            return new Vector3(Utils.RNG.Next(GSV.TerrainPointSpacing, GSV.TerrainPointSpacing * (GSV.TerrainWidth - 2)),
                               10,
                               Utils.RNG.Next(GSV.TerrainPointSpacing, GSV.TerrainPointSpacing * (GSV.TerrainWidth - 2)));
        }
        
        public PlantTile() { }
    }
}
