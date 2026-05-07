using System;
using Games.TileMatch.Board.Scripts;
using Games.TileMatch.Manager;
using Project.Core.UI;
using Project.Services;

namespace UI.Screen
{
    public class PlayScreen:UICanvas
    {
        private void Start()
        {
            
        }

        public void OnReset()
        {
            BoardCollectTile.Instance.ResetBoard();
            TileManager.Instance.ResetTiles();
        }
        public void Undo()
        {
            BoardCollectTile.Instance.UndoTile(1);
        }

        public void MagicWand()
        {
            TileManager.Instance.CollectMatchThreeTiles();
        }

        public void OnShuffle()
        {
            TileManager.Instance.ShuffleGridTiles();
        }
    }
}