using Games.TileMatch.Tiles.Scripts;
using Project.Services;
using UI.Components.booster;
using UI.Screen;
using UnityEngine;

namespace Project.Manager
{
    public class GameplayManager : Singleton<GameplayManager>
    {
        private readonly PoolManager<Tile> _poolTile = new();
        public static PoolManager<Tile> PoolTile => Instance._poolTile;
        [Min(1)][SerializeField] private int level;

        public static int CurrentLevel
        {
            get => Instance.level;
            set => Instance.level = value;
        }
        [Header("Unlock Booster")]
        [Min(1)] [SerializeField] private int levelUndo;
        public static bool LevelUnlockUndo => CurrentLevel >= Instance.levelUndo;

        [Min(1)] [SerializeField] private int levelMagicWand;
        public static bool LevelUnlockMagic => CurrentLevel >= Instance.levelMagicWand;
        
        [Min(1)] [SerializeField] private int levelShuffle;
        public static bool LevelUnlockShuffle => CurrentLevel >= Instance.levelShuffle;

        public static void Show(bool isActive)
        {
            foreach (Transform child in Instance.transform)
            {
                child.gameObject.SetActive(isActive);
            }
        }
        
    }
}
