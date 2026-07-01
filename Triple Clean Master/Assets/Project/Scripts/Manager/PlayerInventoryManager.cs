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
        public static Action <int> OnCoinchange;

        private const string CoinKey = "Coin";
        private const string UndoKey = "Undo";
        private const string MagicWandKey = "MagicWand";
        private const string ShuffleKey = "Shuffle";
        
        private const int UndoPrice = 100;
        private const int MagicWandPrice = 200;
        private const int ShufflePrice = 300;
        
        private static int _coin = 100;
        private static int _undo = 2;
        private static int _magicWand = 2;
        private static int _shuffle = 2;

        public static int UndoCount
        {
            get => _undo;
            set
            {
                _undo = Mathf.Max(0, value);
                SaveBooster(TypeBooster.Undo);
                RefreshBoosterButtonState( TypeBooster.Undo);
                
                
            }
        }

        public static int MagicWandCount
        {
            get => _magicWand;
            set
            {
                _magicWand = Mathf.Max(0, value);
                SaveBooster(TypeBooster.MagicWand);
                RefreshBoosterButtonState( TypeBooster.MagicWand);
            }

        }

        public static int ShuffleCount
        {
            get => _shuffle;
            set
            {
               _shuffle = Mathf.Max(0, value);
                SaveBooster(TypeBooster.Shuffle);
                RefreshBoosterButtonState(TypeBooster.Shuffle);


            }

        }

        private void Start()
        {
            LoadInventory();
            OnCoinchange?.Invoke(_coin);
        }

        public static int Coin
        {
            get => _coin;
            set
            {
                _coin = Mathf.Max(0, value);
                SaveCoin();
                OnCoinchange?.Invoke(_coin);
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
            _coin = PlayerPrefs.GetInt(CoinKey, _coin);
            _undo = PlayerPrefs.GetInt(UndoKey, _undo);
            _magicWand = PlayerPrefs.GetInt(MagicWandKey, _magicWand);
            _shuffle = PlayerPrefs.GetInt(ShuffleKey, _shuffle);
        }
        private static void SaveCoin()
        {
            PlayerPrefs.SetInt(CoinKey, _coin);
            PlayerPrefs.Save();
        }
        private static void SaveBooster(TypeBooster type)
        {
            switch (type)
            {
                case TypeBooster.Undo:
                    PlayerPrefs.SetInt(UndoKey, _undo);
                    break;

                case TypeBooster.MagicWand:
                    PlayerPrefs.SetInt(MagicWandKey, _magicWand);
                    break;

                case TypeBooster.Shuffle:
                    PlayerPrefs.SetInt(ShuffleKey, _shuffle);
                    break;
            }

            PlayerPrefs.Save();
        }
    }

}
