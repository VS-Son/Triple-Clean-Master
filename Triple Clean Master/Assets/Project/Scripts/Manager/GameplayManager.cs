using System.Collections.Generic;
using Project.Scripts.TileMatch.Tiles;
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
        public static bool IsUndoUnlocked => CurrentLevel >= Instance.levelUndo;

        [Min(1)] [SerializeField] private int levelMagicWand;
        public static bool IsMagicWandUnlocked => CurrentLevel >= Instance.levelMagicWand;
        
        [Min(1)] [SerializeField] private int levelShuffle;
        public static bool IsShuffleUnlocked => CurrentLevel >= Instance.levelShuffle;

        public static void SetBoardActive(bool isActive)
        {
            if (Instance.board != null)
            {
                Instance.board.SetActive(isActive);

            }
        }

        public static void SetTileManagerActive(bool isActive)
        {
            Instance.tileManager.SetActive(isActive);
        }


    }
}
