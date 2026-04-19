using System;
using System.Collections.Generic;
using Games.TileMatch.ConfigData;
using Games.TileMatch.Level.Data;
using Games.TileMatch.Level.Scripts;
using Games.TileMatch.Tiles.Data;
using Project.Manager;
using UnityEngine;
using Games.TileMatch.Tiles.Scripts;
namespace Games.TileMatch.Manager
{
    public class TileManager : Singleton<TileManager>
    {
        [SerializeField] private Tile prefab;
        [SerializeField] private ListDataSprite listDataSprites;
        [SerializeField] private ListTileThemesData listThemesData;
        private List<LevelData> _levelData;
        private List<TileId> _tileIds = new List<TileId>()
            { TileId.Id1, TileId.Id2, TileId.Id3, TileId.Id4, TileId.Id5 };

        private void Awake()
        {
            LoadFileJson.LoadJsonPath("Json/LevelTile");
        }

        private void Start()
        {
            OnInit();
            GridTileSpawner(5,5, 5, 2);
        }

        private void OnInit()
        {
            var level = LoadFileJson.GetJsonTile<LevelRoot>().levelTile;
            _levelData = level;


        }

        public void CheckLevel()
        {
            int currentLevel = 1;
            string lvName;
            LevelData levelData = _levelData.Find(l => l.level == currentLevel);
            if (levelData != null)
            {
                lvName = levelData.levelName;
                Debug.Log("lvName" + lvName);
            }
            
        }

        public Tile[,] GridTileSpawner(int sizeGrid, int height, int width, int spacing)
        {
            Tile[,] tiles = new Tile[height, width];
            for (int y = 0; y < sizeGrid; y++)
            {
                for (int x = 0; x < sizeGrid; x++)
                {
                    var position = new Vector2(x * spacing, -y * spacing);
                    var tile = Instantiate(prefab, position, Quaternion.identity);
                    tile.name = $"Tile_x:{x}_y:{y}";
                    tiles[x, y] = tile;
                }
            }
            return tiles;
        }
    }

   
}
