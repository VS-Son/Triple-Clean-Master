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
            UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).levelText.gameObject.SetActive(true);
            UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).UpdateLevelText();
            if (GameplayManager.LevelUnlockUndo)
            {
               PlayScreen.SetUndoAlpha(TypeBooster.Undo,0.6f);
            }
            if (GameplayManager.LevelUnlockMagic)
            {
                PlayScreen.SetUndoAlpha(TypeBooster.MagicWand,1);
            }
            if (GameplayManager.LevelUnlockShuffle)
            {
                PlayScreen.SetUndoAlpha(TypeBooster.Shuffle, 1);
            }
            Close();
        }
    }
}
