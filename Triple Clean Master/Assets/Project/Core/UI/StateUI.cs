using System;
using Games.TileMatch.Board.Scripts;
using Games.TileMatch.Manager;
using Project.Manager;
using Project.Services;
using UI.Screen;
using UnityEngine;

namespace Project.Core.UI
{
    public  class StateUI: MonoBehaviour
    {
        private static TypeScreen _typeScreen;

        private  void Start()
        {
            ChangeState(TypeScreen.HomeScreen);
        }

        public static void ChangeState(TypeScreen state)
        {
            _typeScreen = state;
            switch (_typeScreen)
            {
                case TypeScreen.HomeScreen:
                    UIManager.OpenUI<HomeScreen>(TypeScreen.HomeScreen);
                    UIManager.CloseUI<PlayScreen>(TypeScreen.PlayScreen);
                    UIManager.OpenUI<StatusBar>(TypeScreen.StatusBar);
                    break;
                case TypeScreen.PlayScreen:
                    UIManager.OpenUI<PlayScreen>(TypeScreen.PlayScreen);
                    UIManager.CloseUI<HomeScreen>(TypeScreen.HomeScreen);
                    break;
                case TypeScreen.Revive:
                    UIManager.OpenUI<ReviveScreen>(TypeScreen.Revive);
                    break;

            }
        }

        public static bool IsState(TypeScreen typeScreen) => _typeScreen == typeScreen;
    }
}
