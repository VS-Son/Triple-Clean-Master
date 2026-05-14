using System.Collections.Generic;
using System.Linq;
using Games.TileMatch.Board.Scripts;
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
using Random = UnityEngine.Random;

namespace Games.TileMatch.Manager
{
    public class TileManager : Singleton<TileManager>
    {
        [SerializeField] public int tileIndex;
        [SerializeField] private Tile prefab;
        [SerializeField] private ListDataSprite listDataSprites;
        [SerializeField] private ListTileThemesData listThemesData;

        private Dictionary<int, LevelData> _levelData;
        private readonly Dictionary<int, Transform> _layerParent = new();
        private readonly Dictionary<int, Tile[,]> _layerTile = new();
        private List<TileId> _distributeTiles;
        private readonly Dictionary<TileId, int> _countTileId = new Dictionary<TileId, int>();
        private Dictionary<TileId, Sprite> _spriteLookUp;

        private readonly List<TileId> _tileId = new List<TileId>()
            {TileId.Id1, TileId.Id2, TileId.Id3, TileId.Id4, TileId.Id5 };
        
       
        private void Awake()
        {
            LoadFileJson.LoadResource("Json/LevelTile" );
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
            Debug.Log("total tile: "+ totalTiles);
            CalculateDistributeId(totalTiles);
            foreach (var config in layerConfig)
            {
                var tileGrid = GridTileSpawner(config);
                _layerTile[config.layer] = tileGrid;
            }

            Physics2D.SyncTransforms();
            BuildCoverGraph();
            InitCoverageCount();
            if (StateUI.IsState(TypeScreen.HomeScreen))
            {
                PlayManager.SetActive(false);
            }

        }

        private void BuildCoverGraph()
        {
            foreach (var tile in LayerTile())
            {
                if (tile == null) continue;
                tile.coveredBy.Clear();
                tile.covers.Clear();
            }

            foreach (var upperLayer in _layerTile)
            {
                foreach (var lowerLayer in _layerTile)
                {
                    if (upperLayer.Key <= lowerLayer.Key) continue;

                    LinkLayer(upperLayer.Value, lowerLayer.Value);
                }
            }
        }

        public void InitCoverageCount()
        {
           
            foreach (var tile in LayerTile())
            {
                if (tile == null) continue;
            
                tile.coverCount = tile.coveredBy.Count(t => !t.isCollected);
                UpdateVisual(tile);
            }
        }

        private void LinkLayer(Tile[,] upper, Tile[,] lower)
        {
            foreach (var up in upper)
            {
                if (up == null) continue;

                foreach (var down in lower)
                {
                    if (down == null) continue;

                    if (IsOverlapping(up, down))
                    {
                        down.coveredBy.Add(up);
                        up.covers.Add(down);
                    }
                }
            }
        }
        private bool IsOverlapping(Tile a, Tile b)
        {
            var aCollider = a.collider as BoxCollider2D;
            var bCollider = b.collider as BoxCollider2D;

            if (aCollider == null || bCollider == null) return false;

            return aCollider.bounds.Intersects(bCollider.bounds);
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
            if (_levelData.TryGetValue(PlayManager.CurrentLevel, out var data)) return data.layers;
            return null;
        }

        private Tile[,] GridTileSpawner(LayersData layer)
        {
            Tile[,] tiles = new Tile[layer.rows, layer.cols];
            var spacing = Util.SetSpacing(PlayManager.CurrentLevel);

            var offsetY = (layer.rows - 1 ) * spacing/ 2f;
            var offsetX = (layer.cols - 1) * spacing/ 2f;
            float posZ = 0.1f;
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
                    if (!layer.inactiveCells.Contains(new Vector2Int(x, y)))
                    {
                        posZ++;
                        var position = new Vector2(x * spacing - offsetX, -y * spacing + offsetY);
                        var tile = Instantiate(prefab, position, Quaternion.identity);
                        tile.transform.SetParent(_layerParent[layer.layer]);
                        tile.SetData(layer,PlayManager.CurrentLevel, x, y,posZ);
                        tile.name = $"Tile_y:{y}_x:{x}";
                        tiles[y, x] = tile;
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

        
        public void HandleTileCollected(Tile tile)
        {
            tile.isCollected = true;
            foreach (var down in tile.covers)
            {
                if (down == null || down.isCollected) continue;

                down.coverCount--;

                UpdateVisual(down);
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

        private IEnumerable<Tile> LayerTile()
        {
            foreach (var layer in _layerTile.Values)
            {
                for (int y = 0; y < layer.GetLength(0); y++)
                {
                    for (int x = 0; x < layer.GetLength(1); x++)
                    {
                        yield return layer[y, x];
                    }
                }
            }
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
            DisableInput(true);
        }

        public  void CollectMatchThreeTiles()
        {
            List<Tile> listCollect = BoardCollectTile.Instance.GetListCollect();
            if (listCollect.Count == 0)
            {
                Dictionary<TileId, List<Tile>> availableTiles = new();
                foreach (var tile in LayerTile())
                {
                    if (tile!= null && !tile.isCollected)
                    {

                        if (!availableTiles.ContainsKey((tile.tileId)))
                        {
                            availableTiles[tile.tileId] = new List<Tile>();
                        }
                        availableTiles[tile.tileId].Add(tile);
                    }
                }
               
                
                List<TileId> validTypes =
                    availableTiles.Where(kvp => kvp.Value.Count >= 3).Select(kvl => kvl.Key).ToList();
                TileId randomId = validTypes[Random.Range(0, validTypes.Count)];
                List<Tile> collectTile = availableTiles[randomId];
                List<Tile> result = collectTile.GetRange(0, 3);
                Util.ShuffleList(result);
                foreach (var tile in result)
                {
                    tile.isCollected = true;
                    BoardCollectTile.Instance.CollectTile(tile);
                    HandleTileCollected(tile);
                    tile.spriteTile.color = Util.SetAlphaSprite(255);
                   

                }

                return;
            }

            TileId tileId = listCollect[0].tileId;
            int countSlot = listCollect.Count(t => t.tileId == tileId);
            int missingCount = 3 - countSlot;
            List<Tile> remaining = FindTileIdSame(tileId, missingCount);
            if (remaining.Count == missingCount)
            {
                foreach (var tile in remaining)
                {
                    BoardCollectTile.Instance.CollectTile(tile);
                    HandleTileCollected(tile);
                    tile.spriteTile.color = Util.SetAlphaSprite(255);

                }

            }
        }

        private List<Tile> FindTileIdSame(TileId tileId, int missingCount = 3)
        {
            List<Tile> result = new List<Tile>();
            foreach (var tile in LayerTile())
            {
                if (tile!=null && !tile.isCollected && tile.tileId == tileId)
                {
                    result.Add(tile);
                    if (result.Count >= missingCount)
                    {
                        return result;
                    }
                }
            }
            return result;
        }

        public void DisableInput(bool isActive)
        {
            foreach (var tile in LayerTile())
            {
                if (tile != null && !tile.isCollected)
                {
                    tile.collider.enabled = isActive;

                }
            }
        }

        public void ShuffleGridTiles()
        {
            List<Tile> remainingTile = new List<Tile>();
            List<TileId> listId = new List<TileId>();
            foreach (var tile in LayerTile())
            {
                if (tile!=null && !tile.isCollected)
                {
                    remainingTile.Add(tile);
                    var tileId = GetSprite(tile.spriteTile.sprite);
                    listId.Add(tileId);
                }
            }
            Util.ShuffleList(listId);
            for (int i = 0; i < remainingTile.Count; i++)
            {
                if (_spriteLookUp.TryGetValue(listId[i], out var sprite))
                {
                    remainingTile[i].spriteTile.sprite = sprite;
                    remainingTile[i].tileId = listId[i];
                }
            }
        }

        private TileId GetSprite(Sprite sprite)
        {
            foreach (var s in _spriteLookUp)
            {
                if (s.Value == sprite)
                  return s.Key;
            }
            return TileId.None;
        }

        public bool CheckGridEmptyTile()
        {
            tileIndex --;
            if (tileIndex == 0)
            {
                return true;
            }

            return false;
        }

        public void NextLevel()
        {
            PlayManager.CurrentLevel++;
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
