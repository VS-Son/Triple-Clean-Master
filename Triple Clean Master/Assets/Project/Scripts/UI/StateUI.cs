using Project.Scripts.Manager;
using Project.Scripts.UI.Screen;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class StateUI : MonoBehaviour
    {
        private static ScreenType _currentScreen;
        private static ScreenType _previousScreen;

        public static ScreenType CurrentScreen => _currentScreen;
        public static ScreenType PreviousScreen => _previousScreen;

        private void Start()
        {
            ChangeState(ScreenType.HomeScreen);
            UIManager.OpenCommon<StatusBar>(CommonUIType.StatusBar);
        }

        public static void ChangeState(ScreenType state)
        {
            _previousScreen = _currentScreen;
            _currentScreen = state;

            switch (_currentScreen)
            {
                case ScreenType.HomeScreen:
                    UIManager.OpenScreen<HomeScreen>(ScreenType.HomeScreen);
                    break;

                case ScreenType.PlayScreen:
                    UIManager.OpenScreen<PlayScreen>(ScreenType.PlayScreen);
                    break;

                case ScreenType.Shop:
                    UIManager.OpenScreen<ShopScreen>(ScreenType.Shop);
                    break;

                case ScreenType.NextScreen:
                    UIManager.OpenScreen<NextScreen>(ScreenType.NextScreen);
                    break;
                
            }
        }

        public static bool IsState(ScreenType type)
        {
            return _currentScreen == type;
        }
    }
}