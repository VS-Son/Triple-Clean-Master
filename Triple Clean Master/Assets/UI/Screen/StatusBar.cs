using Games.TileMatch.Board.Scripts;
using Games.TileMatch.Manager;
using Project.Core.UI;
using Project.Manager;
using Project.Services;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Screen
{
    public class StatusBar : UICanvas
    {
        public GameObject setting;
        public GameObject back;

        public void OnHome()
        {
            StateUI.ChangeState(TypeScreen.HomeScreen);
            if (StateUI.IsState(TypeScreen.HomeScreen))
            {
               PlayManager.SetActive(false);
               back.SetActive(false);
            }
        }

        public void OnUndo()
        {
            
        }

        public void OnMagicWand()
        {
            
        }

        public void OnShuffle()
        {
            
        }
    }
}
