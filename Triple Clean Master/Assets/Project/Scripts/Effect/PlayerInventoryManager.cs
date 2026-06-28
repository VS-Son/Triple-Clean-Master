using System;
using System.Security.Cryptography;
using Project.Core.UI;
using Project.Manager;
using Project.Scripts.UI.Components.booster;
using Project.Scripts.UI.Screen;
using Project.Services;
using UI.Screen;
using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Effect
{
    public class PlayerInventoryManager : Singleton<PlayerInventoryManager>
    {
        public static event Action<TypeBooster> DisplayValueBooster;
        public static event Action<TypeBooster> DisplayCoinBooster;
        public static event Action DisplayAdsBooster;


        [Min(0)] [SerializeField] int coin;
        [Min(0)] [SerializeField] int undo;
        [Min(0)] [SerializeField] int magicWand;
        [Min(0)] [SerializeField] private int shuffle;

        public static int ValueUndo
        {
            get => Instance.undo;
            set
            {
                Instance.undo = value;
                CheckEnoughBooster(ValueUndo, TypeBooster.Undo,100);
                
                
            }
        }

        public static int ValueMagicWand
        {
            get => Instance.magicWand;
            set
            {
                Instance.magicWand = value;
                CheckEnoughBooster(ValueMagicWand, TypeBooster.MagicWand,200);
            }

        }

        public static int ValueShuffle
        {
            get => Instance.shuffle;
            set
            {
                Instance.shuffle = value;
                CheckEnoughBooster(ValueShuffle, TypeBooster.Shuffle, 300);


            }

        }

        private void Start()
        {
            UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).UpdateTextCoin(coin);
        }

        public static int Coin
        {
            get => Instance.coin;
            set
            {
                Instance.coin = value;
                UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).UpdateTextCoin(Instance.coin);
            }

        }

        public static void AddCoin(int amount)
        {
            Coin += amount;
           
            if(ValueUndo <= 0)
            {
                if (HasEnoughCoin(amount))
                {
                    DisplayCoinBooster?.Invoke(TypeBooster.Undo);
                }
            }
            if(ValueMagicWand <= 0)
            {
                if (HasEnoughCoin(amount))
                {
                    DisplayCoinBooster?.Invoke(TypeBooster.MagicWand);
                }
            }
            if(ValueShuffle <= 0)
            {
                if (HasEnoughCoin(amount))
                {
                    DisplayCoinBooster?.Invoke(TypeBooster.Shuffle);
                }
            }
           
        }
        
        public static void SpendCoin(int amount)
        {
            Coin -= amount;
            DisplayAdsBooster?.Invoke();
        }
        

        private static void CheckEnoughBooster(int value, TypeBooster type, int amount)
        {
            if (value > 0)
            {
                DisplayValueBooster?.Invoke(type);
            }
            else
            {
                if (HasEnoughCoin(amount))
                {
                    DisplayCoinBooster?.Invoke(type);
                }
                else
                {
                    DisplayAdsBooster?.Invoke();
                }
            }
        }
        public static bool HasEnoughCoin(int amount)
        {
            return Coin >= amount;
        }
        public void SpendBooster(TypeBooster type, int amount)
        {
            switch (type)
            {
                case TypeBooster.Undo:
                    ValueUndo -= amount;
                    break;
                case TypeBooster.MagicWand:
                    ValueMagicWand -= amount;
                    break;
                case TypeBooster.Shuffle:
                    ValueShuffle -= amount;
                    break;
                case TypeBooster.None:
                    break;
            }
        }

        public static void AddBoosters(int amountUndo, int amountMagic, int amountShuffle)
        {
            ValueUndo += amountUndo;
            ValueMagicWand += amountMagic;
            ValueShuffle += amountShuffle;
        }

    }

}
