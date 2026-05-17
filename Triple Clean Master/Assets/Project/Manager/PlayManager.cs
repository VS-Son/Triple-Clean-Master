using Games.TileMatch.Tiles.Scripts;
using UnityEngine;

namespace Project.Manager
{
    public class PlayManager : Singleton<PlayManager>
    {
        [Min(1)][SerializeField] private int level;
        private readonly PoolManager<Tile> _poolTile = new();
        public static PoolManager<Tile> PoolTile => Instance._poolTile;
        public static int CurrentLevel
        {
            get => Instance.level;
            set => Instance.level = value;
        }
        
        public static void SetActive(bool isActive)
        {
            Instance.gameObject.SetActive(isActive);
        }
    }
}
