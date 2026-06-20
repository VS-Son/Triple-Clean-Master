using System;
using Games.TileMatch.Manager;
using Project.Constants;
using Project.Core.Notification;
using Project.Games.TileMatch.Board.Scripts;
using Project.Manager;
using Project.Scripts.Effect;
using Project.Services;
using UI.Components.booster;
using UnityEngine;
using UnityEngine.ResourceManagement;
using UnityEngine.UI;
using ResourceManager = Project.Scripts.Effect.ResourceManager;

namespace UI.Screen
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
                    if (ResourceManager.CurrentUndo > 0)
                    {
                        ResourceManager.Instance.SpendBooster(TypeBooster.Undo, 1);
                        undo.UpdateTextBooster(TypeBooster.Undo,ResourceManager.CurrentUndo);
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
                    ResourceManager.Instance.SpendBooster(TypeBooster.MagicWand, 1);
                    magicWand.UpdateTextBooster(TypeBooster.MagicWand,ResourceManager.CurrentMagicWand);
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
                ResourceManager.Instance.SpendBooster(TypeBooster.Shuffle, 1);
                shuffle.UpdateTextBooster(TypeBooster.Shuffle,ResourceManager.CurrentShuffle);

                
            }
            else
            {
                ToastMessage.ShowMessage(MessageConstants.MessageUnlock(4));
            }
        }

        public void UpdateTextBoosters()
        {
            undo.UpdateTextBooster(TypeBooster.Undo,ResourceManager.CurrentUndo);
            magicWand.UpdateTextBooster(TypeBooster.MagicWand,ResourceManager.CurrentMagicWand);
            shuffle.UpdateTextBooster(TypeBooster.Shuffle,ResourceManager.CurrentShuffle);
        }
    }
}