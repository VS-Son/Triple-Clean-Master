using System;
using Games.TileMatch.Manager;
using Project.Manager;
using Project.Services;
using TMPro;
using UI.Components.booster;
using UnityEngine;

namespace UI.Screen
{
    public class NextScreen : UICanvas
    {
        [SerializeField] private TMP_Text textNext;
        private void OnEnable()
        {
            textNext.text = "Level " + (GameplayManager.CurrentLevel + 1);
        }

        public void OnNext()
        {
            TileManager.ZoomScaleTile();
            TileManager.Instance.NextLevel();
            UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).levelText.gameObject.SetActive(true);
            UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).UpdateLevelText();
            PlayScreen.UpdateUnlockBooster();
            Close();
        }
    }
}
