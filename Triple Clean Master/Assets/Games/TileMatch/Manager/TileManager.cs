using System;
using Project.Manager;
using UnityEngine;
using Games.TileMatch.Tiles.Scripts;
namespace Games.TileMatch.Manager
{
    public class TileManager : Singleton<TileManager>
    {
        public int count = 5;
        [SerializeField] private Tile prefab;

        private void Start()
        {
            GridTileSpawner(5,5, 5, 2);
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
