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
        [Min(1)] [SerializeField] private int levelUnlockUndo;
        public static bool LevelUnlockUndo => CurrentLevel >= Instance.levelUnlockUndo;

        [Min(1)] [SerializeField] private int levelUnlockMagic;
        public static bool LevelUnlockMagic => CurrentLevel >= Instance.levelUnlockMagic;
        
        [Min(1)] [SerializeField] private int levelUnlockShuffle;
        public static bool LevelUnlockShuffle => CurrentLevel >= Instance.levelUnlockShuffle;

        public static void Show(bool isActive)
        {
            foreach (Transform child in Instance.transform)
            {
                child.gameObject.SetActive(isActive);
            }
        }
        
    }
}
