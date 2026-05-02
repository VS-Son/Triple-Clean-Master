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
using Project.Core.UI;
using Project.Extensions;
using Project.Services;

namespace Games.TileMatch.Manager
{
    public class TileManager : Singleton<TileManager>
    {
        [Min(1)] public int currentLv;
        [SerializeField] private Tile prefab;
        [SerializeField] private ListDataSprite listDataSprites;
        [SerializeField] private ListTileThemesData listThemesData;
        [SerializeField] private int tileIndex;

        private Dictionary<int, LevelData> _levelData;
        private readonly Dictionary<int, Transform> _layerParent = new();
        private readonly Dictionary<int, Tile[,]> _layerTile = new();
        private List<TileId> _distributeTiles;
        private readonly Dictionary<TileId, int> _countTileId = new Dictionary<TileId, int>();
        private Dictionary<TileId, Sprite> _spriteLookUp;

        private readonly List<TileId> _tileId = new List<TileId>()
            { TileId.Id1, TileId.Id2, TileId.Id3, TileId.Id4, TileId.Id5 };

        private readonly int[,] _offsets = { { 0, 0 }, { 1, 0 }, { 0, 1 }, { 1, 1 } };

        private void Awake()
        {
            LoadFileJson.LoadResource("Json/LevelTile");
        }

        private void Start()
        {
            OnInit();
            GenerateTileManager();
        }

        private void OnInit()
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

            InitCoverageCount();
            if (StateUI.IsState(TypeScreen.HomeScreen))
            {
                PlayManager.SetActive(false);
            }

        }
        
        public void InitCoverageCount()
        {
            foreach (var layer in _layerTile)
            {
                Tile[,] grid = layer.Value;

                for (int c = 0; c < grid.GetLength(0); c++)
                {
                    for (int r = 0; r < grid.GetLength(1); r++)
                    {
                        Tile tile = grid[c, r];
                        if (tile == null) continue;

                        tile.coverCount = CountCoveringTiles(tile);
                        UpdateVisual(tile);
                    }
                }
            }
        }
        private int CountCoveringTiles(Tile lowerTile)
        {
            int count = 0;
            foreach (var layer in _layerTile)
            {
                if (layer.Key <= lowerTile.currentLayer) continue; 
                Tile[,] higherGrid = layer.Value;
                for (int i = 0; i < _offsets.GetLength(0); i++)
                {
                    int checkCol = lowerTile.col - _offsets[i, 0];
                    int checkRow = lowerTile.row - _offsets[i, 1];

                    if (IsInsideGrid(checkCol, checkRow, higherGrid))
                    {
                        Tile coveringTile = higherGrid[checkCol, checkRow];
                        if (coveringTile != null && !coveringTile.isCollected)
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }
        private bool IsInsideGrid(int c, int r, Tile[,] grid)
        {
            return c >= 0 && c < grid.GetLength(0) && r >= 0 && r < grid.GetLength(1);
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
            var spacing = Util.SetSpacing(layer);
            float offsetX = (layer.rows - 1) * spacing / 2f;
            float offsetY = (layer.cols - 1) * spacing / 2f; // layer 1: y_1.05f, layer 2: y_0.525f
            if (!_layerParent.ContainsKey(layer.layer))
            {
                var layerParent = new GameObject(layer.layerName);
                _layerParent[layer.layer] = layerParent.transform;
                layerParent.transform.SetParent(gameObject.transform);
            }

            for (int c = 0; c < tiles.GetLength(0); c++)
            {
                for (int r = 0; r < tiles.GetLength(1); r++)
                {
                    if (!layer.inactiveCells.Contains(new Vector2Int(r, c)))
                    {
                        var position = new Vector2(r * spacing - offsetX, -c * spacing + offsetY); //layer 1: y_0, layer 2: y = -1.05
                        var tile = Instantiate(prefab, position, Quaternion.identity);
                        tile.transform.SetParent(_layerParent[layer.layer]);
                        tile.SetData(layer, r, c);
                        tile.name = $"Tile_x:{c}_y:{r}";
                        tiles[c, r] = tile;
                    }
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
        
        public TileId GetDistributedTileType()
        {
            var tileType = _distributeTiles[tileIndex];
            tileIndex++;
            return tileType;
        }

        
        public void OnTileCollected(Tile collectedTile)
        {
            collectedTile.isCollected = true;
            foreach (var layer in _layerTile)
            {
                if (layer.Key >= collectedTile.currentLayer) continue; 

                Tile[,] lowerGrid = layer.Value;
                for (int i = 0; i < _offsets.GetLength(0); i++)
                {
                    int checkCol = collectedTile.col + _offsets[i, 0];
                    int checkRow = collectedTile.row + _offsets[i, 1];

                    if (IsInsideGrid(checkCol, checkRow, lowerGrid))
                    {
                        Tile lowerTile = lowerGrid[checkCol, checkRow];
                        if (lowerTile != null && !lowerTile.isCollected)
                        {
                            lowerTile.coverCount--;
                            UpdateVisual(lowerTile);
                        }
                    }
                }
            }
        }

        private void UpdateVisual(Tile tile)
        {
            bool covered = tile.coverCount <= 0;
            tile.isSelect = covered;
            tile.spriteTile.color = covered ? Util.SetAlphaSprite(255) : Util.SetAlphaSprite(150);
        }

        public Transform SetLayerParent(int layer)
        {
            return _layerParent[layer];
        }

        public void ResetTiles()
        {
            tileIndex = 0;
            foreach (var layer in _layerParent.Values)
            {
                Destroy(layer.gameObject);
            }
            _layerTile.Clear();
            _layerParent.Clear();
            _countTileId.Clear();
            _distributeTiles = null;
            GenerateTileManager();
        }
    }
}
