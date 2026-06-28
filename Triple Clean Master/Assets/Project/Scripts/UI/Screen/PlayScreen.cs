using System;
using Project.Scripts.Constants;
using Project.Scripts.Manager;
using Project.Scripts.Message;
using Project.Scripts.TileMatch.Board;
using Project.Scripts.TileMatch.Manager;
using Project.Scripts.UI.Components.booster;
using UnityEngine;

namespace Project.Scripts.UI.Screen
{
    public class PlayScreen:UICanvas
    {
        [SerializeField] private Booster undo;
        [SerializeField] private Booster magicWand;
        [SerializeField] private Booster shuffle;
        public static event Action<TypeBooster, float> AlphaBooster;
        public static bool IsClick = false;
        
        private void OnEnable()
        {
            GameplayManager.SetBoardActive(true);
            PlayerInventoryManager.OnShowBoosterCountRequested += OnShowBoosterCountRequested;
            PlayerInventoryManager.OnShowCoinPurchaseRequested += OnShowCoinPurchaseRequested;
            PlayerInventoryManager.OnShowAdUnlockRequested += OnShowAdUnlockRequested;
            UpdateUnlockBooster();
            UpdateTextBoosters();
        }

        private void OnDisable()
        {
            GameplayManager.SetBoardActive(false);
            PlayerInventoryManager.OnShowBoosterCountRequested -= OnShowBoosterCountRequested;
            PlayerInventoryManager.OnShowCoinPurchaseRequested -= OnShowCoinPurchaseRequested;
            PlayerInventoryManager.OnShowAdUnlockRequested -= OnShowAdUnlockRequested;

        }


        public void OnLoadLevel()
        {
            BoardCollectTile.Instance.ResetBoard();
            TileManager.Instance.ResetTiles();
            //TileManager.Instance.OnInit();
            UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).UpdateLevelText();
            UpdateUnlockBooster();
        }

        private void UpdateUnlockBooster()
        {
            if (GameplayManager.IsUndoUnlocked)
            {
                undo.OnDisplayBooster(TypeBooster.Undo, !BoardCollectTile.HasTileInBoard() ? 0.6f : 1f,
                    PlayerInventoryManager.UndoCount);
            }

            if (GameplayManager.IsMagicWandUnlocked)
            {
                magicWand.OnDisplayBooster(TypeBooster.MagicWand, 1f,PlayerInventoryManager.UndoCount );
                
            }
            if (GameplayManager.IsShuffleUnlocked)
            {
                shuffle.OnDisplayBooster(TypeBooster.Shuffle, 1f,PlayerInventoryManager.UndoCount);
                
            }       
        }
      
        public static void SetBoosterAlpha(TypeBooster type, float alpha)
        {
            AlphaBooster?.Invoke(type, alpha);
        }

        public void Undo()
        {
            if (GameplayManager.IsUndoUnlocked)
            {
                if (undo.IsDisplay(TypeBooster.Undo))
                {
                    BoardCollectTile.Instance.UndoTile(1);
                    if (PlayerInventoryManager.UndoCount > 0)
                    {
                        PlayerInventoryManager.Instance.ConsumeBooster(TypeBooster.Undo, 1);
                        undo.UpdateTextBooster(TypeBooster.Undo, PlayerInventoryManager.UndoCount,100);
                       
                    }
                    else
                    {
                        if (PlayerInventoryManager.HasEnoughCoin(100))
                        {
                            PlayerInventoryManager.SpendCoin(100);
                        }
                        else
                        {
                            Debug.Log("Display ads");
                            
                        }
                    }

                }
                else
                {
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
            
            if (GameplayManager.IsMagicWandUnlocked )
            {
                IsClick = true;
                if (!BoardCollectTile.IsMatching && magicWand.IsDisplay(TypeBooster.MagicWand))
                {
                    TileManager.Instance.CollectMatchThreeTiles();
                    if (PlayerInventoryManager.MagicWandCount > 0)
                    {
                        PlayerInventoryManager.Instance.ConsumeBooster(TypeBooster.MagicWand, 1);
                        magicWand.UpdateTextBooster(TypeBooster.MagicWand,PlayerInventoryManager.MagicWandCount,200);
                    }
                    else
                    {
                        if (PlayerInventoryManager.HasEnoughCoin(200))
                        {
                            PlayerInventoryManager.SpendCoin(200);
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
            if (GameplayManager.IsShuffleUnlocked)
            {
                if (!TileManager.IsCompleteShuffle && shuffle.IsDisplay(TypeBooster.Shuffle))
                {
                    TileManager.Instance.ShuffleGridTiles();
                    if (PlayerInventoryManager.ShuffleCount > 0)
                    {
                        PlayerInventoryManager.Instance.ConsumeBooster(TypeBooster.Shuffle, 1);
                        shuffle.UpdateTextBooster(TypeBooster.Shuffle, PlayerInventoryManager.ShuffleCount,300);
                    }
                    else
                    {
                        if (PlayerInventoryManager.HasEnoughCoin(300))
                        {
                            PlayerInventoryManager.SpendCoin(300);

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
                ToastMessage.ShowMessage(MessageConstants.MessageUnlock(4));
            }
        }

        private void OnShowBoosterCountRequested(TypeBooster type)
        {
            undo.DisplayValue(type);
            magicWand.DisplayValue(type);
            shuffle.DisplayValue(type);
        }

        private void OnShowCoinPurchaseRequested(TypeBooster type)
        {
            undo.DisplayCoin(type);
            magicWand.DisplayCoin(type);
            shuffle.DisplayCoin(type);
        }

        private void OnShowAdUnlockRequested( )
        {
            if (PlayerInventoryManager.Coin < 300)
            {
                shuffle.DisplayAds(PlayerInventoryManager.ShuffleCount);
            }
            if (PlayerInventoryManager.Coin < 200)
            {
                shuffle.DisplayAds(PlayerInventoryManager.ShuffleCount);
                magicWand.DisplayAds(PlayerInventoryManager.MagicWandCount);
            }
            if (PlayerInventoryManager.Coin < 100)
            {
                undo.DisplayAds(PlayerInventoryManager.UndoCount);
                magicWand.DisplayAds(PlayerInventoryManager.MagicWandCount);
                shuffle.DisplayAds(PlayerInventoryManager.ShuffleCount);
            }
           
            
        }

        public void UpdateTextBoosters()
        {
            undo.UpdateTextBooster(TypeBooster.Undo,PlayerInventoryManager.UndoCount,100);
            magicWand.UpdateTextBooster(TypeBooster.MagicWand,PlayerInventoryManager.MagicWandCount,200);
            shuffle.UpdateTextBooster(TypeBooster.Shuffle,PlayerInventoryManager.ShuffleCount,300);
        }
        
    }
}