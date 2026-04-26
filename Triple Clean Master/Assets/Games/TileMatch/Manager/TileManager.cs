using System;
using System.Collections.Generic;
using System.Linq;
using Games.TileMatch.ConfigData;
using Games.TileMatch.Level.Data;
using Games.TileMatch.Level.Scripts;
using Games.TileMatch.Tiles.Data;
using Project.Manager;
using UnityEngine;
using Games.TileMatch.Tiles.Scripts;
using System.Threading.Tasks; 

namespace Games.TileMatch.Manager
{
    public class TileManager : Singleton<TileManager>
    {
        public GameObject spawner;
        [SerializeField] private Tile prefab;
        [SerializeField] private ListDataSprite listDataSprites;
        [SerializeField] private ListTileThemesData listThemesData;
        private Dictionary<int, LevelData> _levelData;
        private readonly Dictionary<int, Transform> _layerParent = new ();
        private readonly Dictionary<int, Tile[,]> _layerTile = new ();
        private List<TileId> _tileIds = new List<TileId>()
            { TileId.Id1, TileId.Id2, TileId.Id3, TileId.Id4, TileId.Id5 };

        [Min(1)]private readonly int _currentLv = 1;
        private void Awake()
        {
            LoadFileJson.LoadResource("Json/LevelTile");
        }

        private void Start()
        {
            OnInit();
        }

        public void OnInit()
        {
            var level = LoadFileJson.GetData<LevelRoot>().levelTile;
            _levelData = level.ToDictionary(l => l.level);

           
        }

        public void Spawner()
        {
            
            var layerConfig =LoadLayerConfigs();
            foreach (var config in layerConfig)
            {
               var tileGrid = GridTileSpawner(config);
               _layerTile[config.layer] = tileGrid;
            }

            SetTileBlocked();

        }

        private void SetTileBlocked()
        {
            foreach (var layerValue in _layerTile.Values)
            {
                for (int x = 0; x < layerValue.GetLength(0); x++)
                {
                    for (int y = 0; y < layerValue.GetLength(1); y++)
                    {
                        var tile = layerValue[x, y];
                        if (tile != null)
                        {
                            foreach (var layer in _layerTile)
                            {
                                var layerHigh = layer.Key;
                                if (layerHigh > tile.currentLayer)
                                {
                                    tile.isSelect = false;
                                    tile.spriteTile.color = Color.black;
                                }
                                else
                                {
                                    tile.isSelect = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private List<LayersData> LoadLayerConfigs()
        {
            if (_levelData.TryGetValue(_currentLv, out var data)) return data.layers;
            return null;
        }
        
        private Tile[,] GridTileSpawner(LayersData layer)
        {
            Tile[,] tiles = new Tile[layer.cols, layer.rows];
            var spacing = SetSpacing(layer);
            float offsetX = (layer.rows - 1) * spacing / 2f;
            float offsetY = (layer.cols - 1) * spacing / 2f; // layer 1: y_1.05f, layer 2: y_0.525f
            if (!_layerParent.ContainsKey(layer.layer))
            {
                var layerParent = new GameObject(layer.layerName);
                _layerParent[layer.layer] = layerParent.transform;
                layerParent.transform.SetParent(gameObject.transform);
            }
            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    if (layer.inactiveCells.Contains(new Vector2Int(x,y)))
                    {
                       // continue;
                    }
                    var position =  new Vector2(x *spacing - offsetX, -y * spacing + offsetY ) ;//layer 1: y_0, layer 2: y = -1.05
                    var tile = Instantiate(prefab, position, Quaternion.identity);
                    tile.transform.SetParent(_layerParent[layer.layer]);
                    tile.currentLayer = layer.layer;
                    tile.transform.localScale = SetScale(layer);
                    tile.row = x;
                    tile.col = y;
                    tile.name = $"Tile_x:{x}_y:{y}";
                    tiles[x, y] = tile;
                }
            }
            return tiles;
        }

        private float SetSpacing(LayersData layer)
        {
            float spacing = 0;
            if (layer.layer % 2 == 0)
            {
                spacing = layer.cols switch
                {
                    <= 2 => 1.05f,
                    <= 4 and > 2 => 0.83f,
                    <= 7 and > 4 => 0.3f,
                    _ => spacing
                };
            }
            else
            {
                spacing = layer.cols switch
                {
                    <= 3 => 1.05f,
                    <= 5 and > 2 => 0.83f,
                    <= 8 and > 4 => 0.3f,
                    _ => spacing
                };
            }

            return spacing;
        }

        private Vector3 SetScale(LayersData layer)
        {
            float scale = 1;
            if (layer.layer % 2 == 0)
            {
                scale = layer.cols switch
                {
                    <= 2 => 1f,
                    <= 4 and > 2 => 0.8f,
                    <= 7 and > 4 => 0.5f,
                    _ => scale
                };
            }
            else
            {
                scale = layer.cols switch
                {
                    <= 3 => 1,
                    <= 5 and > 3 => 0.8f,
                    <= 8 and > 5 => 0.5f,
                    _ => scale
                };
            }
            return new Vector3(scale, scale);
        }
    }

   
}
