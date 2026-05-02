using Games.TileMatch.Board.Scripts;
using Games.TileMatch.Manager;
using UnityEngine;

namespace UI.Screen
{
    public class ReviveScreen : UICanvas
    {
        public void OnRevive()
        {
            BoardCollectTile.Instance.UndoTile(3);
            Close();
        }

        public void OnReplay()
        {
            BoardCollectTile.Instance.ResetBoard();
            TileManager.Instance.ResetTiles();
            Close();
        }
    }
}
