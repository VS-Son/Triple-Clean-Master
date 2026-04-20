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
        [SerializeField] private Tile prefab;
        [SerializeField] private ListDataSprite listDataSprites;
        [SerializeField] private ListTileThemesData listThemesData;
        private Dictionary<int, LevelData> _levelData;
        private List<TileId> _tileIds = new List<TileId>()
            { TileId.Id1, TileId.Id2, TileId.Id3, TileId.Id4, TileId.Id5 };

        private readonly int _currentLv = 1;
        private void Awake()
        {
            LoadFileJson.LoadResource("Json/LevelTile");
        }

        private void Start()
        {
            OnInit();
        }

        private void OnInit()
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
            }
           
        }
        private List<LayersData> LoadLayerConfigs()
        {
            if (_levelData.TryGetValue(_currentLv, out var data)) return data.layers;
            return null;
        }
        public void CheckLevel()
        {
        }


        private Tile[,] GridTileSpawner(LayersData layer)
        {
            var spacing = 1.05f;
            float offsetX = (layer.rows - 1) * spacing / 2f;
            float offsetY = (layer.cols - 1) * spacing / 2f; // layer 1: y_1.05f, layer 2: y_0.525f
            Tile[,] tiles = new Tile[layer.cols, layer.rows];
            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    if (layer.inactiveCells.Contains(new Vector2Int(x,y)))
                    {
                       // continue;
                    }
                    var position = layer.layer == 2 ? new Vector2(x *spacing -offsetX, -y * spacing - offsetY - 1.05f ):new Vector2(x *spacing -offsetX, -y * spacing - offsetY) ;//layer 1: y_0, layer 2: y = -1.05
                    var tile = Instantiate(prefab, position, Quaternion.identity);
                    tile.name = $"Tile_x:{x}_y:{y}";
                    tiles[x, y] = tile;
                }
            }
            return tiles;
        }
    }

   
}
