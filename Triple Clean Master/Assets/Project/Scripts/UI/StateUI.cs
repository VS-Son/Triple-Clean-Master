using System;
using Project.Manager;
using Project.Scripts.UI.Screen;
using Project.Services;
using UI.Screen;
using UI.Screen.Shop;
using UnityEngine;

namespace Project.Core.UI
{
    public class StateUI : MonoBehaviour
    {
        private static TypeScreen _currentScreen;
        private static TypeScreen _previousScreen;

        public static TypeScreen CurrentScreen => _currentScreen;
        public static TypeScreen PreviousScreen => _previousScreen;

        private void Start()
        {
            ChangeState(TypeScreen.HomeScreen);
        }

        public static void ChangeState(TypeScreen state)
        {
            _previousScreen = _currentScreen;
            _currentScreen = state;

            switch (_currentScreen)
            {
                case TypeScreen.HomeScreen:
                    UIManager.OpenUI<HomeScreen>(TypeScreen.HomeScreen);
                    UIManager.OpenUI<StatusBar>(TypeScreen.StatusBar);
                    UIManager.CloseUI<PlayScreen>(TypeScreen.PlayScreen);
                    break;

                case TypeScreen.PlayScreen:
                    UIManager.OpenUI<PlayScreen>(TypeScreen.PlayScreen);
                    UIManager.CloseUI<HomeScreen>(TypeScreen.HomeScreen);
                    break;
                case TypeScreen.Revive:
                    UIManager.OpenUI<ReviveScreen>(TypeScreen.Revive);
                    break;
                case TypeScreen.NextScreen:
                    UIManager.OpenUI<NextScreen>(TypeScreen.NextScreen);
                    UIManager.CloseUI<PlayScreen>(TypeScreen.PlayScreen);
                    break;

                case TypeScreen.Setting:
                    UIManager.OpenUI<SettingScreen>(TypeScreen.Setting);
                    break;

                case TypeScreen.Shop:
                    UIManager.OpenUI<ShopScreen>(TypeScreen.Shop);
                    break;
            }
        }

        public static bool IsState(TypeScreen typeScreen)
        {
            return _currentScreen == typeScreen;
        }
    }
}
