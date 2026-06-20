using System;
using Project.Manager;
using Project.Services;
using UI.Components.booster;
using UI.Screen;
using UnityEngine;

namespace Project.Scripts.Effect
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        public int coin;
        public int undo;
        public int magicWand;
        public int shuffle;
        public static int CurrentUndo { get => Instance.undo; set => Instance.undo = value; }
        public static int CurrentMagicWand { get => Instance.magicWand; set => Instance.magicWand = value; }
        public static int CurrentShuffle { get => Instance.shuffle; set => Instance.shuffle = value; }

        private void Start()
        {
            UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).UpdateTextCoin(coin);
        }

        private static int Coin
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
        }

        public void SpendCoin(int amount)
        {
            coin -= amount;
        }

        public void SpendBooster(TypeBooster type, int amount)
        {
            switch (type)
            {
                case TypeBooster.Undo:
                    CurrentUndo -= amount;
                    break;
                case TypeBooster.MagicWand:
                    CurrentMagicWand -= amount;
                    break;
                case TypeBooster.Shuffle:
                    CurrentShuffle -= amount;
                    break;
                case TypeBooster.None:
                    break;
            }
        }
        public void AddBoosters( int amountUndo, int amountMagic, int amountShuffle)
        {
            undo += amountUndo;
            magicWand += amountMagic;
            shuffle += amountShuffle;
        }

       
    }
}
