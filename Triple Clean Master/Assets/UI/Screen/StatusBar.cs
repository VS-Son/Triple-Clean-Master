using System;
using Games.TileMatch.Board.Scripts;
using Games.TileMatch.Manager;
using Project.Core.UI;
using Project.Manager;
using Project.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Screen
{
    public class StatusBar : UICanvas
    {
        public TMP_Text levelText;
        public GameObject setting;
        public GameObject back;

        private void OnEnable()
        {
            if (StateUI.IsState(TypeScreen.HomeScreen))
            {
                levelText.gameObject.SetActive(false);
            }
            UpdateLevelText();
           
        }

        public void OnSetting()
        {
            StateUI.ChangeState(TypeScreen.Setting);
        }
        public void OnHome()
        {
            StateUI.ChangeState(TypeScreen.HomeScreen);
            if (StateUI.IsState(TypeScreen.HomeScreen))
            {
               GameplayManager.Show(false);
               back.SetActive(false);
               levelText.gameObject.SetActive(false);
            }
        }

        public void UpdateLevelText()
        {
            levelText.text = "Level " + (GameplayManager.CurrentLevel);
        }
       
    }
}
