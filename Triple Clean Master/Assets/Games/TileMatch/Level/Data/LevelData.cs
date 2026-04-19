using System;
using System.Collections.Generic;
using UnityEngine;
using Games.TileMatch.Tiles.Data;
namespace Games.TileMatch.Level.Data
{
    [Serializable]
    public class TileSpriteData
    {
        public TileId  typeId;
        public Sprite sprite;
    }
    [Serializable]
    public class LevelRoot
    {
        public string name;
        public List<LevelData> levelTile;
    }
   
    [Serializable]
    public class LevelData 
    {
        public int levels;
        public string nameLevel;
        public List<LayersData> layersData;
    }
    [Serializable]
    public class LayersData
    {
        public int rows;
        public int cols;
        public float posY;
        public float posX;
        public int layerSort;
        public string tileNameLayer;
        public int girdSize;
        public int tileSpace;
        
    }
}
