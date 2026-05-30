using System;
using Games.TileMatch.Manager;
using Project.Constants;
using Project.Core.Notification;
using Project.Games.TileMatch.Board.Scripts;
using Project.Manager;
using Project.Services;
using UI.Components.booster;
using UnityEngine;

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
            IsClick = true;
            if (GameplayManager.LevelUnlockMagic )
            {
                if (!BoardCollectTile.IsMatching && magicWand.IsDisplay(TypeBooster.MagicWand))
                {
                    TileManager.Instance.CollectMatchThreeTiles();
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
            }
            else
            {
                ToastMessage.ShowMessage(MessageConstants.MessageUnlock(4));
            }
        }

       
    }
}