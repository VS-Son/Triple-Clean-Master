using System;
using Games.TileMatch.Board.Scripts;
using Project.Core.UI;
using Project.Services;

namespace UI.Screen
{
    public class PlayScreen:UICanvas
    {
        private void Start()
        {
            
        }

        public void Undo()
        {
            BoardCollectTile.Instance.UndoTile(1);
        }
    }
}