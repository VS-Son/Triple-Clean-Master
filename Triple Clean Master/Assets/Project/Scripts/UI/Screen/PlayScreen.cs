using System;
using Project.Constants;
using Project.Core.Notification;
using Project.Games.TileMatch.Board.Scripts;
using Project.Manager;
using Project.Scripts.Manager;
using Project.Scripts.TileMatch.Manager;
using Project.Scripts.UI.Components.booster;
using Project.Services;
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
        
        private void OnEnable()
        {
            GameplayManager.ActiveChild(true);
            ResourceManager.DisplayValueBooster += DisplayValueBooster;
            ResourceManager.DisplayCoinBooster += DisplayCoinBooster;
            ResourceManager.DisplayAdsBooster += DisplayAdsBooster;
            UpdateUnlockBooster();
            UpdateTextBoosters();
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

        private void UpdateUnlockBooster()
        {
            if (GameplayManager.LevelUnlockUndo)
            {
                undo.OnDisplayBooster(TypeBooster.Undo, !BoardCollectTile.HasTileInBoard() ? 0.6f : 1f,
                    ResourceManager.ValueUndo);
            }

            if (GameplayManager.LevelUnlockMagic)
            {
                magicWand.OnDisplayBooster(TypeBooster.MagicWand, 1f,ResourceManager.ValueUndo );
                
            }
            if (GameplayManager.LevelUnlockShuffle)
            {
                shuffle.OnDisplayBooster(TypeBooster.Shuffle, 1f,ResourceManager.ValueUndo);
                
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
                if (undo.IsDisplay(TypeBooster.Undo))
                {
                    BoardCollectTile.Instance.UndoTile(1);
                    if (ResourceManager.ValueUndo > 0)
                    {
                        ResourceManager.Instance.SpendBooster(TypeBooster.Undo, 1);
                        undo.UpdateTextBooster(TypeBooster.Undo, ResourceManager.ValueUndo,100);
                       
                    }
                    else
                    {
                        if (ResourceManager.HasEnoughCoin(100))
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
                        magicWand.UpdateTextBooster(TypeBooster.MagicWand,ResourceManager.ValueMagicWand,200);
                    }
                    else
                    {
                        if (ResourceManager.HasEnoughCoin(200))
                        {
                            ResourceManager.SpendCoin(200);
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
                if (!TileManager.IsCompleteShuffle && shuffle.IsDisplay(TypeBooster.Shuffle))
                {
                    TileManager.Instance.ShuffleGridTiles();
                    if (ResourceManager.ValueShuffle > 0)
                    {
                        ResourceManager.Instance.SpendBooster(TypeBooster.Shuffle, 1);
                        shuffle.UpdateTextBooster(TypeBooster.Shuffle, ResourceManager.ValueShuffle,300);
                    }
                    else
                    {
                        if (ResourceManager.HasEnoughCoin(300))
                        {
                            ResourceManager.SpendCoin(300);

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

        private void DisplayValueBooster(TypeBooster type)
        {
            undo.DisplayValue(type);
            magicWand.DisplayValue(type);
            shuffle.DisplayValue(type);
        }

        private void DisplayCoinBooster(TypeBooster type)
        {
            undo.DisplayCoin(type);
            magicWand.DisplayCoin(type);
            shuffle.DisplayCoin(type);
        }

        private void DisplayAdsBooster( )
        {
            if (ResourceManager.Coin < 300)
            {
                shuffle.DisplayAds(ResourceManager.ValueShuffle);
            }
            if (ResourceManager.Coin < 200)
            {
                shuffle.DisplayAds(ResourceManager.ValueShuffle);
                magicWand.DisplayAds(ResourceManager.ValueMagicWand);
            }
            if (ResourceManager.Coin < 100)
            {
                undo.DisplayAds(ResourceManager.ValueUndo);
                magicWand.DisplayAds(ResourceManager.ValueMagicWand);
                shuffle.DisplayAds(ResourceManager.ValueShuffle);
            }
           
            
        }

        public void UpdateTextBoosters()
        {
            undo.UpdateTextBooster(TypeBooster.Undo,ResourceManager.ValueUndo,100);
            magicWand.UpdateTextBooster(TypeBooster.MagicWand,ResourceManager.ValueMagicWand,200);
            shuffle.UpdateTextBooster(TypeBooster.Shuffle,ResourceManager.ValueShuffle,300);
        }
        
    }
}