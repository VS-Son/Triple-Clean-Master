using System.Collections.Generic;
using System.Linq;
using Editor;
using Games.TileMatch.Level.Data;
using Project.Constants;
using Project.Extensions;
using UnityEditor;
using UnityEngine;

namespace Tools.Editor
{
    public class LevelEditorWindow : EditorWindow 
    {
        private Vector2 _scrollLevel;
        private Vector2 _scrollLayer;
        private int _selectedIndex;
        private float _cellSize = 50f;


        readonly List<LevelData> _level = new();
        private LevelData CurrentLevel => _level[_selectedIndex];
        private List<LayersData> Layer => CurrentLevel.layers;

        private int CurrentLayer
        {
            get => CurrentLevel.currentLayerIndex;
            set => CurrentLevel.currentLayerIndex = value;
        }
        

        [MenuItem("Tools/Level Editor")]
        public static void Open()
        {
            GetWindow<LevelEditorWindow>("Level Editor");
        }

        private void OnEnable()
        {
            LevelEditorDataHandler.InitDataFile();
            LevelEditorDataHandler.LoadEditor(_level);
            InitDefaultLevel();
            _selectedIndex = Mathf.Clamp(0, 0, _level.Count - 1);
            Repaint();
        }

        private void OnDisable()
        {
            LevelEditorDataHandler.SaveDataEditor(_level);
        }

        private void InitDefaultLevel()
        {
            if (_level.Count == 0)
            {
                var level = new LevelData();
                int rows = level.CurrentRows;
                int cols = level.CurrentCols;

                level.layers.Add(new LayersData(cols, rows));
                _level.Add(level);
                _selectedIndex = 0;
            }
        }

        private void OnGUI()
        {
            DrawScrollList();
            DrawGrid();
            DrawLayer();

            
        }

        private void DrawScrollList()
        {
            Rect scrollRect = new Rect(
                position.width * 0.01f,
                position.height * 0.01f,
                position.width * 0.3f,
                position.height *0.98f
            );
            GUILayout.BeginArea(scrollRect);
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.BeginHorizontal();
                    {
                        var height = GUILayout.Height(40);
                        if (GUILayout.Button("Add Level", height))
                        {
                            LevelLogicHandler.AddLevel(CurrentLevel, _level);
                            CheckChangeGridsValue();
                            Repaint();
                            
                        }

                        if (GUILayout.Button("Remove Level",height))
                        {
                            LevelLogicHandler.RemoveLevel(_level, _selectedIndex);
                            _selectedIndex = Mathf.Clamp(_selectedIndex, 0, _level.Count - 1);
                            Repaint();
                        }
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.Space(10);
                    GUILayout.BeginVertical("box");
                    {
                        _scrollLevel = EditorGUILayout.BeginScrollView(_scrollLevel, GUIStyle.none, GUIStyle.none);
                        {
                            for (int i = 0; i < _level.Count; i++)
                            {
                                if (GUILayout.Toggle(_selectedIndex == i, "Level " + (i + 1), "Button",
                                        GUILayout.Height(30)))
                                {
                                    _selectedIndex = i;
                                }
                            }
                        }
                        EditorGUILayout.EndScrollView();
                    }
                    GUILayout.EndVertical();

                }
                GUILayout.EndVertical();
               

            }
            GUILayout.EndArea();
        }

        private void DrawGrid()
        {
            Rect gridRect = new Rect(
                position.width * 0.3f + 30,
                position.height * 0.01f,
                position.width * 0.4f - 40,
                position.height * 0.9f
            );
           
            GUILayout.BeginArea(gridRect);
            {
                GUILayout.BeginVertical();
                {
                    DrawLayoutButtonLayer();
                    
                    GUILayout.Space(30);
                    GUILayout.BeginVertical("box");
                    {
                        Rect contentRect = GUILayoutUtility.GetRect(
                            0,
                            10000,
                            0,
                            10000,
                            GUILayout.ExpandWidth(true),
                            GUILayout.ExpandHeight(true)
                        );
                        DrawGridEditor(contentRect);
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndVertical();

            }
            GUILayout.EndArea();
        }

        private void DrawGridEditor(Rect container)
        {
            float padding = 20f;
            var baseLayer = Layer[0];

            float maxCellWidth =
                (container.width - padding) / baseLayer.cols;

            float maxCellHeight =
                (container.height - padding) / baseLayer.rows;

            _cellSize = Mathf.Min(
                maxCellWidth,
                maxCellHeight
            );
            _cellSize = Mathf.Clamp(
                _cellSize,
                15f,
                80f
            );
            float gridWidth = baseLayer.cols * _cellSize;
            float gridHeight = baseLayer.rows * _cellSize;
            Rect baseRect = new Rect(
                container.x + (container.width - gridWidth) * 0.5f,
                container.y + (container.height - gridHeight) * 0.5f,
                gridWidth,
                gridHeight
            );
            EditorGUI.DrawRect(
                baseRect,
                ColorConstants.BgCell

            );

            Rect activeBgRect = GetLayerBackgroundRect(
                baseRect,
                CurrentLayer,
                Layer[CurrentLayer],
                baseLayer
            );

            EditorGUI.DrawRect(
                activeBgRect,
                ColorConstants.BgCell
            );
            for (int l = 0; l < CurrentLayer; l++)
            {
                DrawLayerCells(
                    l,
                    baseRect,
                    false
                );
            }

            DrawLayerCells(CurrentLayer, baseRect, true);

            
            HandleClick(baseRect);
        }

        private Rect GetLayerBackgroundRect(Rect baseRect, int currentLayer, LayersData layersData, LayersData baseLayer)
        {
            Vector2 origin = LevelLogicHandler.GetLayerOrigin(baseRect, currentLayer,_cellSize);
            Vector2Int centerOffset = LevelLogicHandler.GetCenteredOffset(layersData, baseLayer);

            float x = origin.x + centerOffset.x * _cellSize;
            float y = origin.y + centerOffset.y * _cellSize;

            float width = layersData.cols * _cellSize;
            float height = layersData.rows * _cellSize;

            return new Rect(x, y, width, height);
        }
        void DrawLayerCells(int layerIndex, Rect baseRect, bool isActive)
        {
            var layer = Layer[layerIndex];
            var baseLayer = Layer[0];

            Vector2 origin = LevelLogicHandler.GetLayerOrigin(baseRect, layerIndex,_cellSize);
            Vector2Int centerOffset = LevelLogicHandler.GetCenteredOffset(layer, baseLayer);
            for (int y = 0; y < layer.rows; y++)
            {
                for (int x = 0; x < layer.cols; x++)
                {
                    Rect cell = new Rect(
                        origin.x + (x + centerOffset.x) * _cellSize,
                        origin.y + (y + centerOffset.y) * _cellSize,
                        _cellSize,
                        _cellSize
                    );

              
                    if (layer.Cells[y, x])
                    {
                        EditorGUI.DrawRect(cell, isActive ? ColorConstants.ActiveColor  : ColorConstants.InactiveColor);
                    }

                    if (isActive)
                    {
                       LevelLogicHandler.DrawBorder(cell);
                    }

                }
            }
        }
        
        void HandleClick(Rect baseRect)
        {
            
            Event e = Event.current;
            if (e.type != EventType.MouseDown)
                return;

            var layer = Layer[CurrentLayer];
            var baseLayer = Layer[0];

            Vector2 origin = LevelLogicHandler.GetLayerOrigin(baseRect, CurrentLayer,_cellSize);
            Vector2Int centerOffset = LevelLogicHandler.GetCenteredOffset(layer, baseLayer);

            Vector2 mouse = e.mousePosition;

            for (int y = 0; y < layer.rows; y++)
            {
                for (int x = 0; x < layer.cols; x++)
                {
                    Rect cellRect = new Rect(
                        origin.x + (x + centerOffset.x) * _cellSize,
                        origin.y + (y + centerOffset.y) * _cellSize,
                        _cellSize,
                        _cellSize
                    );

                    if (cellRect.Contains(mouse))
                    {
                        bool isActive = layer.Cells[y, x];
                        if (!isActive)
                        {
                            CurrentLevel.countCellActive++;
                        }
                        else
                        {
                            CurrentLevel.countCellActive--;
                        }
                        layer.Cells[y, x] = !isActive;
                        e.Use();
                        Repaint();
                        return;
                    }
                }
            }
        }

        private void DrawLayoutButtonLayer()
        {
            GUILayout.BeginHorizontal();
            {
                var height = GUILayout.Height(40);
                if (GUILayout.Button("Add Layer", height))
                {
                    LevelLogicHandler.AddLayer(CurrentLevel, Layer);
                }

                if (GUILayout.Button("Remove Layer", height)) 
                {
                    LevelLogicHandler.RemoveLayer(Layer);
                    CurrentLayer = Mathf.Clamp(CurrentLayer, 0, Layer.Count - 1);

                }
            }
            GUILayout.EndHorizontal();
            CurrentLevel.levelName =  GUILayout.TextField(("Level " + (_selectedIndex + 1)) + (":") + (" "), EditorStyles.boldLabel);
            CheckChangeGridsValue();
            CurrentLevel.optionRowIndex = EditorGUILayout.Popup("Option Row: ", CurrentLevel.optionRowIndex,
                LevelData.OptionRow.Select(x => x.ToString()).ToArray(), GUILayout.Width(200));
            CurrentLevel.optionColIndex = EditorGUILayout.Popup("Option Col: ", CurrentLevel.optionColIndex,
                LevelData.OptionCol.Select(x => x.ToString()).ToArray(), GUILayout.Width(200));
            CurrentLevel.coinReward = EditorGUILayout.IntField("Coin Reward", CurrentLevel.coinReward);
            if (GUILayout.Button("Clear Current Grid" ,GUILayout.Height(40)))
            {
                        
            }
            EditorGUILayout.IntField("Coin Reward", CurrentLevel.countCellActive);
        }

        private void DrawLayer()
        {
            Rect gridRect = new Rect(
                position.width * 0.7f,
                position.height * 0.01f,
                position.width * 0.3f,
                position.height * 0.98f
            );
            GUILayout.BeginArea(gridRect);
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.BeginVertical("Box");
                    {
                        _scrollLayer = GUILayout.BeginScrollView(_scrollLayer, GUIStyle.none, GUIStyle.none);
                        {
                            for (int i = 0; i < Layer.Count; i++)
                            {
                                Layer[i].layerName = "Layer" + ( i + 1 );
                                if (GUILayout.Toggle(CurrentLayer == (i), $"Layer {i + 1} ({Layer[i].cols}x{Layer[i].rows})", "Button",GUILayout.Height(80)))
                                {
                                    CurrentLayer = i;
                                }
                            }
                        }
                        GUILayout.EndScrollView();
                    }
                    GUILayout.EndVertical();
                    GUILayout.Space(10);
                    if (GUILayout.Button("Export File Json",GUILayout.Height(40)))
                    {
                        LevelEditorDataHandler.ExportFileJson();
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }
        private void CheckChangeGridsValue()
        {
            if (EditorGUI.EndChangeCheck())
            {
                int newRows = CurrentLevel.CurrentRows;
                int newCols = CurrentLevel.CurrentCols;
                for (int i = 0; i < Layer.Count; i++)
                {
                    LayersData layer = Layer[i];
                    if (layer.rows != newRows || layer.cols != newCols)
                    {
                        if (i % 2 == 0)
                        {
                            LevelLogicHandler.ResizeLayer(layer, newRows, newCols);
                            Repaint();

                        }

                        if (i % 2 != 0)
                        {
                           LevelLogicHandler.ResizeLayer(layer, newRows - 1, newCols - 1);
                            Repaint();

                        }

                    }
                }
            }

        }
    }
}
