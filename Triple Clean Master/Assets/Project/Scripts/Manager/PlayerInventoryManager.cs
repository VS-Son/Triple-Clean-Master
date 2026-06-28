using System;
using Project.Scripts.UI.Components.booster;
using Project.Scripts.UI.Screen;
using UnityEngine;

namespace Project.Scripts.Manager
{
    public class PlayerInventoryManager : Singleton<PlayerInventoryManager>
    {
        public static event Action<TypeBooster> OnShowBoosterCountRequested;
        public static event Action<TypeBooster> OnShowCoinPurchaseRequested;
        public static event Action OnShowAdUnlockRequested;

        private const string CoinKey = "Coin";
        private const string UndoKey = "Undo";
        private const string MagicWandKey = "MagicWand";
        private const string ShuffleKey = "Shuffle";
        
        private const int UndoPrice = 100;
        private const int MagicWandPrice = 200;
        private const int ShufflePrice = 300;
        
        [Min(0)] [SerializeField] int coin;
        [Min(0)] [SerializeField] int undo;
        [Min(0)] [SerializeField] int magicWand;
        [Min(0)] [SerializeField] private int shuffle;

        public static int UndoCount
        {
            get => Instance.undo;
            set
            {
                Instance.undo = Mathf.Max(0, value);
                SaveBooster(TypeBooster.Undo);
                RefreshBoosterButtonState( TypeBooster.Undo);
                
                
            }
        }

        public static int MagicWandCount
        {
            get => Instance.magicWand;
            set
            {
                Instance.magicWand = Mathf.Max(0, value);
                SaveBooster(TypeBooster.MagicWand);
                RefreshBoosterButtonState( TypeBooster.MagicWand);
            }

        }

        public static int ShuffleCount
        {
            get => Instance.shuffle;
            set
            {
                Instance.shuffle = Mathf.Max(0, value);
                SaveBooster(TypeBooster.Shuffle);
                RefreshBoosterButtonState(TypeBooster.Shuffle);


            }

        }

        private void Start()
        {
            LoadInventory();
            UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).UpdateTextCoin(coin);
        }

        public static int Coin
        {
            get => Instance.coin;
            set
            {
                Instance.coin = Mathf.Max(0, value);
                SaveCoin();
                UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).UpdateTextCoin(Instance.coin);
            }

        }

        public static void AddCoin(int amount)
        {
            Coin += amount;
           
            if(UndoCount <= 0)
            {
                if (HasEnoughCoin(amount))
                {
                    OnShowCoinPurchaseRequested?.Invoke(TypeBooster.Undo);
                }
            }
            if(MagicWandCount <= 0)
            {
                if (HasEnoughCoin(amount))
                {
                    OnShowCoinPurchaseRequested?.Invoke(TypeBooster.MagicWand);
                }
            }
            if(ShuffleCount <= 0)
            {
                if (HasEnoughCoin(amount))
                {
                    OnShowCoinPurchaseRequested?.Invoke(TypeBooster.Shuffle);
                }
            }
           
        }
        
        public static void SpendCoin(int amount)
        {
            Coin -= amount;
            OnShowAdUnlockRequested?.Invoke();
        }


        public static bool HasEnoughCoin(int amount)
        {
            return Coin >= amount;
        }

        public void ConsumeBooster(TypeBooster type, int amount)
        {
            switch (type)
            {
                case TypeBooster.Undo:
                    UndoCount -= amount;
                    break;
                case TypeBooster.MagicWand:
                    MagicWandCount -= amount;
                    break;
                case TypeBooster.Shuffle:
                    ShuffleCount -= amount;
                    break;
                case TypeBooster.None:
                    break;
            }
        }

        public static void AddBoosterCounts(int amountUndo, int amountMagic, int amountShuffle)
        {
            UndoCount += amountUndo;
            MagicWandCount += amountMagic;
            ShuffleCount += amountShuffle;
        }

        private static void RefreshBoosterButtonState(TypeBooster type)
        {
            int boosterCount = GetBoosterCount(type);
            int boosterPrice = GetBoosterPrice(type);

            if (boosterCount > 0)
            {
                OnShowBoosterCountRequested?.Invoke(type);
                return;
            }
            if (HasEnoughCoin(boosterPrice))
            {
                OnShowCoinPurchaseRequested?.Invoke(type);
                return;
            }
            OnShowAdUnlockRequested?.Invoke();
        }

        private static int GetBoosterCount(TypeBooster type)
        {
            return type switch
            {
                TypeBooster.Undo => UndoCount,
                TypeBooster.MagicWand => MagicWandCount,
                TypeBooster.Shuffle => ShuffleCount,
                _ => 0
            };
        }

        private static int GetBoosterPrice(TypeBooster type)
        {
            return type switch
            {
                TypeBooster.Undo => UndoPrice,
                TypeBooster.MagicWand => MagicWandPrice,
                TypeBooster.Shuffle => ShufflePrice,
                _ => 0
            };
        }

        private void LoadInventory()
        {
            coin = PlayerPrefs.GetInt(CoinKey, coin);
            undo = PlayerPrefs.GetInt(UndoKey, undo);
            magicWand = PlayerPrefs.GetInt(MagicWandKey, magicWand);
            shuffle = PlayerPrefs.GetInt(ShuffleKey, shuffle);
        }
        private static void SaveCoin()
        {
            PlayerPrefs.SetInt(CoinKey, Instance.coin);
            PlayerPrefs.Save();
        }
        private static void SaveBooster(TypeBooster type)
        {
            switch (type)
            {
                case TypeBooster.Undo:
                    PlayerPrefs.SetInt(UndoKey, Instance.undo);
                    break;

                case TypeBooster.MagicWand:
                    PlayerPrefs.SetInt(MagicWandKey, Instance.magicWand);
                    break;

                case TypeBooster.Shuffle:
                    PlayerPrefs.SetInt(ShuffleKey, Instance.shuffle);
                    break;
            }

            PlayerPrefs.Save();
        }
    }

}
