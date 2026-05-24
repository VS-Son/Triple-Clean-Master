using System;
using Games.TileMatch.Board.Scripts;
using Games.TileMatch.Manager;
using Project.Constants;
using Project.Core.Notification;
using Project.Manager;
using UI.Components.booster;
using UnityEngine;

namespace UI.Screen
{
    public class PlayScreen:UICanvas
    {
        [SerializeField] private GameObject undo;
        [SerializeField] private GameObject magicWand;
        [SerializeField] private GameObject shuffle;
        public static event Action<TypeBooster, float> AlphaBooster;
        public static bool IsClick = false;
        private void Start()
        {
            
        }

        
        public void OnReset()
        {
            BoardCollectTile.Instance.ResetBoard();
            TileManager.Instance.ResetTiles();
            TileManager.Instance.OnInit();
        }
        public static void SetUndoAlpha(TypeBooster type,float alpha)
        {
            if (GameplayManager.LevelUnlockUndo)
            {
                AlphaBooster?.Invoke(type,alpha);

            }
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
            if (GameplayManager.LevelUnlockMagic)
            {
                TileManager.Instance.CollectMatchThreeTiles();
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