using System;
using Games.TileMatch.Manager;
using Project.Core.UI;
using Project.Manager;
using Project.Services;
using TMPro;
using UI.Screen.Shop;
using UnityEngine;
using UnityEngine.UIElements;
using AudioType = Project.Manager.AudioType;
using Button = UnityEngine.UI.Button;

namespace UI.Screen
{
    public enum ShopOpenSource
    {
        HomeScreen,
        Gameplay
    }
    public class StatusBar : UICanvas
    {
        public TMP_Text textTitle;
        public GameObject setting;
        public GameObject back;
        [SerializeField] private Button statusCoin;
        public  TypeScreen currentScreen;
        private static ShopOpenSource _source;

        private void OnEnable()
        {
            if (StateUI.IsState(TypeScreen.HomeScreen))
            {
                textTitle.gameObject.SetActive(false);
            }
            UpdateLevelText();
           
        }

        public void OnSetting()
        {
            StateUI.ChangeState(TypeScreen.Setting);
        }

        public void OnBack()
        {
            if (StateUI.IsState(TypeScreen.PlayScreen))
            {
                StateUI.ChangeState(TypeScreen.HomeScreen);
                AudioManager.Instance.StopBGM();
                GameplayManager.Show(false);
                back.SetActive(false);
                textTitle.gameObject.SetActive(false);
                return;
            }
            switch (_source)
            {
                case ShopOpenSource.HomeScreen:
                    StateUI.ChangeState(TypeScreen.HomeScreen);
                    AudioManager.Instance.StopBGM();
                    GameplayManager.Show(false);
                    back.SetActive(false);
                    setting.SetActive(true);
                    textTitle.gameObject.SetActive(false);
                    UIManager.CloseUI<ShopScreen>(TypeScreen.Shop);
                    break;

                case ShopOpenSource.Gameplay:
                    StateUI.ChangeState(TypeScreen.PlayScreen);
                    UIManager.CloseUI<ShopScreen>(TypeScreen.Shop);
                    UpdateLevelText();
                    setting.SetActive(true);
                    break;
            }
        }

        public void OnShop()
        {
            var source = StateUI.IsState(TypeScreen.PlayScreen)
                ? ShopOpenSource.Gameplay
                : ShopOpenSource.HomeScreen;

            StateUI.ChangeState(TypeScreen.Shop);
            setting.SetActive(false);
            back.SetActive(true);
            textTitle.gameObject.SetActive(true);
            textTitle.text = "Store";
            _source = source;
            
        }
        

        public  void UpdateLevelText()
        {
            textTitle.text = $"Level {GameplayManager.CurrentLevel}";
        }

        public void OnActiveStatus(bool isActive)
        {
            statusCoin.enabled = isActive;
            textTitle.gameObject.SetActive(isActive);
            setting.SetActive(isActive);
            back.SetActive(isActive);

        }


    }
}
