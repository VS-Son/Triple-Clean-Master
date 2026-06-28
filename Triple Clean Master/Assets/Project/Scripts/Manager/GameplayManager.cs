using System.Collections.Generic;
using Games.TileMatch.Tiles.Scripts;
using Project.Games.TileMatch.Board.Scripts;
using Project.Manager;
using UnityEngine;

namespace Project.Scripts.Manager
{
    public class GameplayManager : Singleton<GameplayManager>
    {
        [SerializeField] GameObject board;
        [SerializeField] private GameObject tileManager;
        private readonly PoolManager<Tile> _poolTile = new();
        private readonly PoolManager<CoinEffect> _poolCoin = new();
        public static PoolManager<Tile> PoolTile => Instance._poolTile;
        public static PoolManager<CoinEffect> PoolCoin => Instance._poolCoin;
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

        public static void ActiveBoard(bool isActive)
        {
            if (Instance.board != null)
            {
                Instance.board.SetActive(isActive);

            }
        }
        public static void ActiveTileManager(bool isActive)
        {
            Instance.tileManager.SetActive(isActive);
        }
        
        

      

        
    }
}
