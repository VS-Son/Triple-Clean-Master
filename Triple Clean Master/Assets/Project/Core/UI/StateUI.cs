using System;
using Games.TileMatch.Manager;
using Project.Manager;
using Project.Services;
using UI.Screen.Gameplay;
using UI.Screen.Home;
using UnityEngine;

namespace Project.Core.UI
{
    public  class StateUI: MonoBehaviour
    {
        private  void Start()
        {
            ChangeState(TypeScreen.HomeScreen);
        }
        public static void ChangeState(TypeScreen state)
        {
            switch (state)
            {
                case TypeScreen.HomeScreen:
                    UIManager.Instance.OpenUI<HomeScreen>(TypeScreen.HomeScreen);
                    UIManager.Instance.CloseUI<PlayScreen>(TypeScreen.PlayScreen);
                    TileManager.Instance.gameObject.SetActive(false);
                    break;
                case TypeScreen.PlayScreen:
                    UIManager.Instance.OpenUI<PlayScreen>(TypeScreen.PlayScreen);
                    UIManager.Instance.CloseUI<HomeScreen>(TypeScreen.HomeScreen);
                    TileManager.Instance.gameObject.SetActive(true);
                    TileManager.Instance.OnInit();
                    break;

            }
        }

      
    }
}
