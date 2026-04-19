using System;
using Project.Core.UI;
using Project.Services;
using UI.Screen.Gameplay;
using UnityEngine;

namespace UI.Screen.Home
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
      }
   }
}
