using Games.TileMatch.Board.Scripts;
using Games.TileMatch.Manager;
using Project.Core.UI;
using Project.Manager;
using Project.Services;

namespace UI.Screen
{
   public class HomeScreen : UICanvas
   {
      //public override TypeScreen Type => TypeScreen.HomeScreen;
      private void Start()
      {
         
      }

      public void OnPlay()
      {
         StateUI.ChangeState(TypeScreen.PlayScreen);
         if (StateUI.IsState(TypeScreen.PlayScreen))
         {
            UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).back.SetActive(true);
            PlayManager.SetActive(true);
         }

      }
   }
}
