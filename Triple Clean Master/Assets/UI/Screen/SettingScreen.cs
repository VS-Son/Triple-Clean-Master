using Project.Core.UI;
using Project.Services;

namespace UI.Screen
{
    public class SettingScreen : UICanvas
    {
        public void OnClose()
        {
            //StateUI.ChangeState(TypeScreen.HomeScreen);
            Close();
        }
    }
}
