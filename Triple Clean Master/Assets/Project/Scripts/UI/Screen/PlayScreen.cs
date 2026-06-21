using System;
using Project.Constants;
using Project.Core.Notification;
using Project.Games.TileMatch.Board.Scripts;
using Project.Manager;
using Project.Scripts.Manager;
using Project.Scripts.TileMatch.Manager;
using Project.Services;
using UI.Components.booster;
using UI.Screen;
using UnityEngine;
using ResourceManager = Project.Scripts.Effect.ResourceManager;

namespace Project.Scripts.UI.Screen
{
    public class PlayScreen:UICanvas
    {
        [SerializeField] private Booster undo;
        [SerializeField] private Booster magicWand;
        [SerializeField] private Booster shuffle;
        public static event Action<TypeBooster, float> AlphaBooster;
        public static bool IsClick = false;
        private void Start()
        {
        }

        private void OnEnable()
        {
            GameplayManager.ActiveChild(true);
            ResourceManager.DisplayValueBooster += DisplayValueBooster;
            ResourceManager.DisplayCoinBooster += DisplayCoinBooster;
            ResourceManager.DisplayAdsBooster += DisplayAdsBooster;


        }

        private void OnDisable()
        {
            GameplayManager.ActiveChild(false);
            ResourceManager.DisplayValueBooster -= DisplayValueBooster;
            ResourceManager.DisplayCoinBooster -= DisplayCoinBooster;
            ResourceManager.DisplayAdsBooster -= DisplayAdsBooster;

        }


        public void OnLoadLevel()
        {
            BoardCollectTile.Instance.ResetBoard();
            TileManager.Instance.ResetTiles();
            TileManager.Instance.OnInit();
            UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).UpdateLevelText();
            UpdateUnlockBooster();
        }

        public static void UpdateUnlockBooster()
        {
            if (GameplayManager.LevelUnlockUndo)
            {
                SetBoosterAlpha(TypeBooster.Undo,0.6f);
            }
            if (GameplayManager.LevelUnlockMagic)
            {
                SetBoosterAlpha(TypeBooster.MagicWand,1);
            }
            if (GameplayManager.LevelUnlockShuffle)
            {
                SetBoosterAlpha(TypeBooster.Shuffle, 1);
            }
        }
        public static void SetBoosterAlpha(TypeBooster type, float alpha)
        {
            AlphaBooster?.Invoke(type, alpha);
        }

        public void Undo()
        {
            if (GameplayManager.LevelUnlockUndo)
            {
                if (BoardCollectTile.HasTileInBoard() && !BoardCollectTile.IsMatching)
                {
                    BoardCollectTile.Instance.UndoTile(1);
                    if (ResourceManager.ValueUndo > 0)
                    {
                        ResourceManager.Instance.SpendBooster(TypeBooster.Undo, 1);
                        undo.UpdateTextBooster(TypeBooster.Undo, ResourceManager.ValueUndo);
                       
                    }
                    else
                    {
                        if (ResourceManager.HasEnoughCoin())
                        {
                            ResourceManager.SpendCoin(100);
                        }
                        else
                        {
                            Debug.Log("Display ads");
                            
                        }
                    }

                }
                else
                {
                    AlphaBooster?.Invoke(TypeBooster.Undo, 0.6f);
                    ToastMessage.ShowMessage(MessageConstants.MessageNotUndo);
                }
            }
            else
            {
                ToastMessage.ShowMessage(MessageConstants.MessageUnlock(2));
            }

           
        }

        public void MagicWand()
        {
            
            if (GameplayManager.LevelUnlockMagic )
            {
                IsClick = true;
                if (!BoardCollectTile.IsMatching && magicWand.IsDisplay(TypeBooster.MagicWand))
                {
                    TileManager.Instance.CollectMatchThreeTiles();
                    if (ResourceManager.ValueMagicWand > 0)
                    {
                        ResourceManager.Instance.SpendBooster(TypeBooster.MagicWand, 1);
                        magicWand.UpdateTextBooster(TypeBooster.MagicWand,ResourceManager.ValueMagicWand);
                    }
                    else
                    {
                        if (ResourceManager.HasEnoughCoin())
                        {
                            ResourceManager.SpendCoin(100);

                        }
                        else
                        {
                            Debug.Log("Display ads");
                        }
                    }
                    
                }
            }
            else
            {
                ToastMessage.ShowMessage(MessageConstants.MessageUnlock(3));
            }
        }

        public void OnShuffle()
        {
            if (GameplayManager.LevelUnlockShuffle)
            {
                TileManager.Instance.ShuffleGridTiles();
                if (ResourceManager.ValueShuffle > 0)
                {
                    ResourceManager.Instance.SpendBooster(TypeBooster.Shuffle, 1);
                    shuffle.UpdateTextBooster(TypeBooster.Shuffle,ResourceManager.ValueShuffle);
                }
                else
                {
                    if (ResourceManager.HasEnoughCoin())
                    {
                        ResourceManager.SpendCoin(100);

                    }
                    else
                    {
                        Debug.Log("Display ads");
                    }
                }
               

                
            }
            else
            {
                ToastMessage.ShowMessage(MessageConstants.MessageUnlock(4));
            }
        }

        private void DisplayValueBooster(TypeBooster type)
        {
            if (type == TypeBooster.Undo)
            {
                undo.coin.SetActive(false);
                undo.redValue.SetActive(true);
                undo.ads.SetActive(false);
            }

            if (type == TypeBooster.MagicWand)
            {
                magicWand.coin.SetActive(false);
                magicWand.redValue.SetActive(true);
                magicWand.ads.SetActive(false);
            }

            if (type == TypeBooster.Shuffle)
            {
                shuffle.coin.SetActive(false);
                shuffle.redValue.SetActive(true);
                shuffle.ads.SetActive(false);
            }
           
        }

        private void DisplayCoinBooster(TypeBooster type)
        {
            if (type == TypeBooster.Undo)
            {
                undo.coin.SetActive(true);
                undo.redValue.SetActive(false);
                undo.ads.SetActive(false);
            }

            if (type == TypeBooster.MagicWand)
            {
                magicWand.coin.SetActive(true);
                magicWand.redValue.SetActive(false);
                magicWand.ads.SetActive(false);
            }

            if (type == TypeBooster.Shuffle)
            {
                shuffle.coin.SetActive(true);
                shuffle.redValue.SetActive(false);
                shuffle.ads.SetActive(false);
            }




        }

        private void DisplayAdsBooster( )
        {
            if (ResourceManager.ValueUndo <= 0)
            {
                undo.coin.SetActive(false);
                undo.redValue.SetActive(false);
                undo.ads.SetActive(true);
            }

            if (ResourceManager.ValueMagicWand <= 0)
            {
                magicWand.coin.SetActive(false);
                magicWand.redValue.SetActive(false);
                magicWand.ads.SetActive(true);
            }

            if (ResourceManager.ValueShuffle <= 0)
            {
                shuffle.coin.SetActive(false);
                shuffle.redValue.SetActive(false);
                shuffle.ads.SetActive(true);
            }

            

        }

        public void UpdateTextBoosters()
        {
            undo.UpdateTextBooster(TypeBooster.Undo,ResourceManager.ValueUndo);
            magicWand.UpdateTextBooster(TypeBooster.MagicWand,ResourceManager.ValueMagicWand);
            shuffle.UpdateTextBooster(TypeBooster.Shuffle,ResourceManager.ValueShuffle);
        }
        
    }
}