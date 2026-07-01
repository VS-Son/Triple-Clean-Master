using System;
using System.Collections.Generic;
using Project.Scripts.Effect;
using Project.Scripts.TileMatch.Tiles;
using UnityEngine;

namespace Project.Scripts.Manager
{
    public class GameplayManager : Singleton<GameplayManager>
    {
        [SerializeField] GameObject board;
        [SerializeField] private GameObject tileManager;
        private readonly PoolManager<CoinEffect> _poolCoin = new();
        private const string LevelKey = "Level";

        public static PoolManager<Tile> PoolTile { get; } = new();

        public static PoolManager<Coin> PoolCoin { get; } = new();
        private void Awake()
        {
            LoadLevel();
        }

        [Min(1)][SerializeField] private int level;
        public static int CurrentLevel
        {
            get => Instance.level;
            set
            {
                Instance.level = Mathf.Max(0, value);
                SaveLevel();
            }
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
            if (Instance.tileManager != null)
            {
                Instance.tileManager.SetActive(isActive);

            }
        }

        private void LoadLevel()
        {
            level = PlayerPrefs.GetInt(LevelKey, level);
           
        }
        private static void SaveLevel()
        {
            PlayerPrefs.SetInt(LevelKey, Instance.level);
            PlayerPrefs.Save();
        }
      

    }
}
