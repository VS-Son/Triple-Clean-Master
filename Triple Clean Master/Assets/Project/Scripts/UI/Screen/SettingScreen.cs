using System;
using System.Collections.Generic;
using Project.Core.UI;
using Project.Manager;
using Project.Scripts.Scroll;
using Project.Scripts.TileMatch.Tiles.Theme;
using Project.Scripts.UI.Popup;
using Project.Services;
using UI.Screen;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using AudioType = Project.Manager.AudioType;

namespace Project.Scripts.UI.Screen
{
    public class SettingScreen : UICanvas
    {
        [SerializeField] private GameObject turnOnBgm;
        [SerializeField] private GameObject turnOnSfx;
        [SerializeField] private List<Image> imageTiles;
        [SerializeField] private ListThemeTileConfig listThemeTileConfig;
        private ThemeTileData Data => listThemeTileConfig.listThemeTileData.Find(t => t.isSelected);
        private void OnEnable()
        {
            SetEditTiles();
        }

       
        public void SetEditTiles()
        {
            for (int i = 0; i < 3; i++)
            {
                imageTiles[i].sprite = Data.listThemeTiles[i];
            }
        }

        public void OnClose()
        {
            Close();
            UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).setting.SetActive(true);
        }

        public void TurnOnVolumeBgm(bool turnOn)
        {
            AudioManager.Instance.ToggleMute(turnOn, AudioType.BGM);
            turnOnBgm.SetActive(!turnOn);
        }

        public void TurnOnVolumeSfx(bool turnOn)
        {
            AudioManager.Instance.ToggleMute(turnOn, AudioType.Sfx);
            turnOnSfx.SetActive(!turnOn);
        }

        public void OnEditTileset()
        {
            StateUI.ChangeState(TypeScreen.PopupEditTheme);
        }
    }
}
