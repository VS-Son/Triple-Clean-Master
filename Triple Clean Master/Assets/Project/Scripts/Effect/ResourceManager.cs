using System;
using Project.Manager;
using Project.Scripts.UI.Screen;
using Project.Services;
using UI.Components.booster;
using UI.Screen;
using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Effect
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        [Min(0)] [SerializeField] int coin;
        [Min(0)] [SerializeField] int undo;
        [Min(0)] [SerializeField] int magicWand;
        [Min(0)] [SerializeField] private int shuffle;

        public static int CurrentUndo
        {
            get => Instance.undo;
            set
            {
                Instance.undo = value;
                UIManager.GetUI<ShopScreen>(TypeScreen.Shop).UpdateTextUndo(Instance.undo);

            }
        }

        public static int CurrentMagicWand
        {
            get => Instance.magicWand;
            set
            {
                Instance.magicWand = value;
                UIManager.GetUI<ShopScreen>(TypeScreen.Shop).UpdateTextMagicWand(Instance.magicWand);

            }

        }

        public static int CurrentShuffle
        {
            get => Instance.shuffle;
            set
            {
                Instance.shuffle = value;
                UIManager.GetUI<ShopScreen>(TypeScreen.Shop).UpdateTextShuffle(Instance.shuffle);

            }

        }

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

        public static void AddBoosters(int amountUndo, int amountMagic, int amountShuffle)
        {
            CurrentUndo += amountUndo;
            CurrentMagicWand += amountMagic;
            CurrentShuffle += amountShuffle;
        }

    }

}
