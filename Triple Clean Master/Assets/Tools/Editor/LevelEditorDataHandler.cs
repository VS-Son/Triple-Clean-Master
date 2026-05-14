using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using Games.TileMatch.Level.Data;
using Games.TileMatch.Level.Scripts;
using UnityEditor;

namespace Editor
{
    public static class LevelEditorDataHandler 
    {
        public static void InitDataFile()
        {
            if (!Directory.Exists(FileConstants.FolderPath))
            {
                Directory.CreateDirectory(FileConstants.FolderPath);
                AssetDatabase.Refresh();
            }

            if (!File.Exists(FileConstants.FullPath))
            {
                ExportData data = new ExportData(); 

                string json = JsonUtility.ToJson(data, true);

                File.WriteAllText(FileConstants.FullPath, json);
                AssetDatabase.Refresh();

                Debug.Log("🆕 Created Editor JSON: " + FileConstants.FullPath);
            }
        }

        public static void LoadEditor(List<LevelData> levels)
        {
            if (!File.Exists(FileConstants.FullPath))
                return;

            string json = File.ReadAllText(FileConstants.FullPath);
            ExportData data = JsonUtility.FromJson<ExportData>(json);

            if (data == null || data.levelTile == null)
            {
                return;
            }
            levels.Clear(); 

            foreach (var exportLevel in data.levelTile)
            {
                LevelData level = new LevelData();
                level.levelName = exportLevel.levelName;
                level.coinReward = exportLevel.coin;
                level.optionRowIndex = exportLevel.optionRowIndex;
                level.optionColIndex = exportLevel.optionColIndex;
                foreach (var exportLayer in exportLevel.layers)
                {
                    int rows = exportLayer.rows;
                    int cols = exportLayer.cols;

                    if (rows <= 0 || cols <= 0)
                    {
                        continue;
                    }

                    var layer = new LayersData(cols, rows);

                    for (int y = 0; y < rows; y++)
                    {
                        for (int x = 0; x < cols; x++)
                        {
                            layer.Cells[y, x] = true;
                        }
                    }

                    if (exportLayer.inactiveCells != null)
                    {
                        foreach (var cell in exportLayer.inactiveCells)
                        {
                            int x = cell.x;
                            int y = cell.y;

                            if (y >= 0 && y < rows && x >= 0 && x < cols)
                            {
                                layer.Cells[y, x] = false;
                            }
                            else
                            {
                            }
                        }
                    }

                    level.layers.Add(layer);
                }

                levels.Add(level);
            }
            
        }

        public static void SaveDataEditor(List<LevelData> levels)
        {
            ExportData data = BuildExportData(levels);

            string json = JsonUtility.ToJson(data, true);
       
            File.WriteAllText(FileConstants.FullPath, json);
            AssetDatabase.Refresh();

            Debug.Log("Saved Editor Data");
        }

        private static ExportData BuildExportData(List<LevelData> levels )
        {
            ExportData data = new ExportData();

            for (int lv = 0; lv < levels.Count; lv++)
            {
                var level = levels[lv];

                ExportLevel exportLevel = new ExportLevel();
                exportLevel.levelName = level.levelName;
                exportLevel.level = lv + 1;
                exportLevel.optionRowIndex = level.optionRowIndex;
                exportLevel.optionColIndex = level.optionColIndex;
                exportLevel.coin = level.coinReward;
                for (int l = 0; l < level.layers.Count; l++)
                {
                    var layer = level.layers[l];

                    ExportLayer exportLayer = new ExportLayer();
                    exportLayer.layer = l + 1;
                    exportLayer.rows = layer.rows;
                    exportLayer.cols = layer.cols;

                    for (int y = 0; y < layer.rows; y++)
                    {
                        for (int x = 0; x < layer.cols; x++)
                        {
                            if (!layer.Cells[y, x])
                            {
                                exportLayer.inactiveCells.Add(new CellPos(x, y));
                            }
                        }
                    }

                    exportLevel.layers.Add(exportLayer);
                }

                data.levelTile.Add(exportLevel);
            }

            return data;
        }

        public static void ExportFileJson()
        {
           
        }
    }
}
