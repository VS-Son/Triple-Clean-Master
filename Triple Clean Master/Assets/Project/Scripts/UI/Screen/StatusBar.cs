using Project.Scripts.Manager;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Project.Scripts.UI.Screen
{
    public enum ShopOpenSource
    {
        HomeScreen,
        Gameplay
    }
    public class StatusBar : UICanvas
    {
        public TMP_Text textTitle;
        public TMP_Text textCoin;
        public GameObject setting;
        public GameObject back;
        [SerializeField] private Button statusCoin;
        public Transform iconCoin;
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
            setting.SetActive(false);
        }

        public void OnBack()
        {
            if (StateUI.IsState(TypeScreen.PlayScreen))
            {
                StateUI.ChangeState(TypeScreen.HomeScreen);
                AudioManager.Instance.StopBGM();
                GameplayManager.SetBoardActive(false);
                GameplayManager.SetTileManagerActive(false);
                back.SetActive(false);
                textTitle.gameObject.SetActive(false);
                return;
            }
            switch (_source)
            {
                case ShopOpenSource.HomeScreen:
                    StateUI.ChangeState(TypeScreen.HomeScreen);
                    AudioManager.Instance.StopBGM();
                    GameplayManager.SetBoardActive(false);
                    back.SetActive(false);
                    setting.SetActive(true);
                    textTitle.gameObject.SetActive(false);
                    UIManager.CloseUI<ShopScreen>(TypeScreen.Shop);
                    UIManager.GetUI<HomeScreen>(TypeScreen.HomeScreen).UpdayeTextLevel();
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
            var source = StateUI.IsState(TypeScreen.PlayScreen) ? ShopOpenSource.Gameplay : ShopOpenSource.HomeScreen;
            StateUI.ChangeState(TypeScreen.Shop);
            setting.SetActive(false);
            back.SetActive(true);
            textTitle.gameObject.SetActive(true);
            textTitle.text = "Store";
            UIManager.GetUI<ShopScreen>(TypeScreen.Shop).UpdateTextUndo(PlayerInventoryManager.UndoCount);
            UIManager.GetUI<ShopScreen>(TypeScreen.Shop).UpdateTextMagicWand(PlayerInventoryManager.MagicWandCount);
            UIManager.GetUI<ShopScreen>(TypeScreen.Shop).UpdateTextShuffle(PlayerInventoryManager.ShuffleCount);

            _source = source;
            
        }
        

        public  void UpdateLevelText()
        {
            textTitle.text = $"Level {GameplayManager.CurrentLevel}";
        }

        public void UpdateTextCoin(int coin)
        {
            textCoin.text = coin >= 1000 ? $"{coin/1000}K" : $"{coin.ToString()}";
        }

        public void OnActiveStatus(bool isActive)
        {
            statusCoin.enabled = isActive;
            textTitle.gameObject.SetActive(isActive);
            setting.SetActive(isActive);
            back.SetActive(isActive);

        }


        public void UpdateCoinPerSecond(int coinPerSecond)
        {
            
        }
    }
}
