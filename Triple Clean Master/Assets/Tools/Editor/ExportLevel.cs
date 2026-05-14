using System;
using System.Collections.Generic;
using UnityEngine;

namespace Editor
{
    [Serializable]
    public class ExportLevel
    {
        public string levelName;
        public int level;
        public int optionRowIndex = 0;
        public int optionColIndex = 0;
        public int coin;
        public List<ExportLayer> layers = new();
    }
    [Serializable]
    public class ExportLayer
    {
        public int layer;
        public int rows;
        public int cols;
        public List<CellPos> inactiveCells = new();
    }

    [Serializable]
    public class ExportData
    {
        public List<ExportLevel> levelTile = new();
    }
    [Serializable]
    public class CellPos
    {
        public int x;
        public int y;

        public CellPos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
