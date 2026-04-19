using System;
using Project.Core.UI;
using Project.Services;

namespace UI.Screen.Gameplay
{
    public class PlayScreen:UICanvas
    {
        //public override TypeScreen Type => TypeScreen.PlayScreen;

        public void OnHome()
        {
           StateUI.ChangeState(TypeScreen.HomeScreen); 
        }
    }
}