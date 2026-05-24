using System.Collections.Generic;
using Games.TileMatch.Level.Data;
using UnityEditor;
using UnityEngine;

namespace Tools.Editor
{
    public static class LevelLogicHandler
    {
        public static void AddLevel(LevelData currentLevel, List<LevelData> listLevel)
        {
            var level = new LevelData();
            level.layers.Add(new LayersData(currentLevel.CurrentCols,currentLevel.CurrentRows));
            listLevel.Add(level);
        }

        public static void RemoveLevel(List<LevelData> level, int selectedIndex)
        {
            if (level.Count <= 1)
            {
                return;
            }
            level.RemoveAt(selectedIndex);
        }
        public static void AddLayer(LevelData currentLevel, List<LayersData> layers)
        {
            int baseRows = currentLevel.CurrentRows;
            int baseCols = currentLevel.CurrentCols;

            bool isEven = layers.Count % 2 == 0;

            int r = isEven ? baseRows : baseRows - 1;
            int c = isEven ? baseCols : baseCols - 1;

            r = Mathf.Max(1, r);
            c = Mathf.Max(1, c);

            layers.Add(new LayersData(c, r));
        }

        public static void RemoveLayer(List<LayersData> layers)
        {
            if (layers.Count <= 1)
            {
                return;
            }

            int removeIndex = layers.Count - 1;
            layers.RemoveAt(removeIndex);
        }
        public static void ResizeLayer(LayersData layer, int newRows, int newCols)
        {
            bool[,] newCells = new bool[newRows, newCols];

            int copyRows = Mathf.Min(layer.rows, newRows);
            int copyCols = Mathf.Min(layer.cols, newCols);

            for (int y = 0; y < copyRows; y++)
            {
                for (int x = 0; x < copyCols; x++)
                {
                    newCells[y, x] = layer.Cells[y, x];
                }
            }

            layer.rows = newRows;
            layer.cols = newCols;
            layer.Cells = newCells;
        }
        public static void ClearCurrentLayer()
        {
            
        }

        public static void SaveData()
        {
            
        }

        public static void HandleClickCell(Rect baseRect)
        {
            
        }
        public static Vector2 GetLayerOrigin(Rect baseRect, int layerIndex,float cellSize)
        {
            if (layerIndex % 2 == 1)
            {
                return new Vector2(
                    baseRect.x + cellSize * 0.5f,
                    baseRect.y + cellSize * 0.5f
                );
            }

            return new Vector2(baseRect.x, baseRect.y);
        }
        public static Vector2Int GetCenteredOffset(LayersData layer, LayersData baseLayer)
        {
            return new Vector2Int(
                (layer.cols - baseLayer.cols) / 2,
                (layer.rows - baseLayer.rows) / 2
            );
        }
        public static void DrawBorder(Rect cell)
        {
            Handles.color = Color.black;
            Handles.DrawAAPolyLine(1.5f,
                new Vector3(cell.x, cell.y),
                new Vector3(cell.xMax, cell.y),
                new Vector3(cell.xMax, cell.yMax),
                new Vector3(cell.x, cell.yMax),
                new Vector3(cell.x, cell.y)
            );
        }
    }
}
