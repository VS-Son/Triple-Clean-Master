using System;
using System.Collections.Generic;
using UnityEngine;
using Games.TileMatch.Tiles.Data;
namespace Games.TileMatch.Level.Data
{
    [Serializable]
    public class LevelRoot
    {
        public string name;
        public List<LevelData> levelTile;
    }
   
    [Serializable]
    public class LevelData 
    {
        public int level;
        public string levelName;
        public int countCellActive;
            
        public static int[] OptionRow= {1,2, 3, 4, 5, 6, 7, 8, 9, 10 };
        public static int[] OptionCol= { 3, 4, 5, 6, 7, 8, 9, 10 };

        public int optionRowIndex = 0;
        public int optionColIndex = 0;
        public int CurrentRows => OptionRow[optionRowIndex];
        public int CurrentCols => OptionCol[optionColIndex];
        public List<LayersData> layers = new List<LayersData>();
        public int currentLayerIndex;
        public int coinReward;
    }
    [Serializable]
    public class LayersData
    {
        public int rows;
        public int cols;
        public int layer;
        public string layerName;
        public List<Vector2Int> inactiveCells;
        public bool[,] Cells;

        public LayersData(int cols, int rows)
        {
            this.rows = rows;
            this.cols = cols;
            Cells = new bool[rows, cols];
        }

        
    }
}
