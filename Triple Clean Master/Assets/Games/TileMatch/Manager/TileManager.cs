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
using Project.Extensions;

namespace Games.TileMatch.Manager
{
    public class TileManager : Singleton<TileManager>
    {
        [Min(1)] public int currentLv = 2;
        [SerializeField] private Tile prefab;
        [SerializeField] private ListDataSprite listDataSprites;
        [SerializeField] private ListTileThemesData listThemesData;
        [SerializeField] private int tileIndex;
        
        private Dictionary<int, LevelData> _levelData;
        private readonly Dictionary<int, Transform> _layerParent = new ();
        private readonly Dictionary<int, Tile[,]> _layerTile = new ();
        private  List<TileId> _distributeTiles;
        private readonly Dictionary<TileId, int> _countTileId = new Dictionary<TileId, int>();
        private Dictionary<TileId, Sprite> _spriteLookUp;
        private readonly List<TileId> _tileId = new List<TileId>()
            { TileId.Id1, TileId.Id2, TileId.Id3, TileId.Id4, TileId.Id5};

        private void Awake()
        {
            LoadFileJson.LoadResource("Json/LevelTile");
        }

        private void Start()
        {
            OnInit();
            GenerateTileManager();
        }

        public void OnInit()
        {
            var level = LoadFileJson.GetData<LevelRoot>().levelTile;
            _levelData = level.ToDictionary(l => l.level);
            _spriteLookUp = new Dictionary<TileId, Sprite>();
            foreach (var dataSprite in listDataSprites.listTileSprite)
            {
                if (!_spriteLookUp.ContainsKey(dataSprite.typeId))
                {
                    _spriteLookUp.Add(dataSprite.typeId, dataSprite.sprite);
                }
            }


        }

        private void GenerateTileManager()
        {
            
            var layerConfig = LoadLayerConfigs();
            int totalTiles = layerConfig.Sum(config => (config.rows * config.cols) - config.inactiveCells.Count);
            CalculateDistributeId(totalTiles);
            foreach (var config in layerConfig)
            {
                var tileGrid = GridTileSpawner(config);
               _layerTile[config.layer] = tileGrid;
            }

            SetTileBlocked();

        }

        private void SetTileBlocked()
        {
            var layerMax = _layerTile.Keys.Max();
            foreach (var layerValue in _layerTile.Values)
            {
                for (int x = 0; x < layerValue.GetLength(0); x++)
                {
                    for (int y = 0; y < layerValue.GetLength(1); y++)
                    {
                        var tile = layerValue[x, y];
                        if (tile != null)
                        {
                            if (layerMax > tile.currentLayer)
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

        private void CalculateDistributeId(int totalTiles)
        {
            _distributeTiles = GenerateDistributedId(totalTiles, _tileId);
            Util.ShuffleList(_distributeTiles);
            foreach (var tileType in _distributeTiles)
            {
                if (!_countTileId.ContainsKey(tileType)) _countTileId[tileType] = 0;
                _countTileId[tileType]++;
            }
        }

        private List<TileId> GenerateDistributedId(int totalTiles, List<TileId> tileId)
        {
            var listCount = new List<int>();
            int baseCount = (totalTiles / tileId.Count) / 3 * 3;
            for (int i = 0; i < tileId.Count; i++)
            {
                listCount.Add(baseCount);
            }
            int used = baseCount * tileId.Count;
            int remaining = totalTiles - used;
            int extra = remaining / 3;

            List<int> indices = new List<int>();
            for (int i = 0; i < tileId.Count; i++) indices.Add(i);
            for (int i = 0; i < extra; i++)
            {
                listCount[indices[i]] += 3;
            }
            Util.ShuffleList(listCount);
            var result = new List<TileId>();
            for (int i = 0; i < tileId.Count; i++)
            {
                for (int j = 0; j < listCount[i]; j++)
                {
                    result.Add(tileId[i]);
                }
            }
            
            return result;
        }

        private List<LayersData> LoadLayerConfigs()
        {
            if (_levelData.TryGetValue(currentLv, out var data)) return data.layers;
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
                        continue;
                    }
                    var position =  new Vector2(x *spacing - offsetX, -y * spacing + offsetY ) ;//layer 1: y_0, layer 2: y = -1.05
                    var tile = Instantiate(prefab, position, Quaternion.identity);
                    tile.transform.SetParent(_layerParent[layer.layer]);
                    tile.SetPropertyTile(layer, x,y);
                    tile.name = $"Tile_x:{x}_y:{y}";
                    tiles[x, y] = tile;
                }
            }
            return tiles;
        }

        public Sprite SetSpriteForTile(TileId tileId)
        {
            if (_spriteLookUp.TryGetValue(tileId, out var sprite))
            {
                return sprite;
            }
            return null;
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

        public TileId GetDistributedTileType()
        {
            var tileType = _distributeTiles[tileIndex];
            tileIndex++;
            return tileType;
        }
    }
    

   
}
