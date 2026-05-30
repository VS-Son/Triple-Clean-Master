using Games.TileMatch.Manager;
using Project.Constants;
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
         AudioManager.Instance.PlayBGM(AudioConstants.BGM, 1);
         AudioManager.Instance.PlaySfx(AudioConstants.HighPitchDefault);

         UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).back.SetActive(true);
         UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).levelText.gameObject.SetActive(true);
         TileManager.ZoomScaleTile();
         GameplayManager.Show(true);
         PlayScreen.UpdateUnlockBooster();
         
      }
   }
}
