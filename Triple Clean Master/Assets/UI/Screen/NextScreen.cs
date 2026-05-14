using System;
using Games.TileMatch.Manager;
using Project.Manager;
using TMPro;
using UnityEngine;

namespace UI.Screen
{
    public class NextScreen : UICanvas
    {
        [SerializeField] private TMP_Text textNext; 
        private void OnEnable()
        {
            textNext.text = "Level " + (PlayManager.CurrentLevel + 1);
        }

        public void OnNext()
        {
            TileManager.Instance.NextLevel();
            Close();
        }
    }
}
