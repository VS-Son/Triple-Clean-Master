using System;
using System.Collections.Generic;
using Project.Scripts.Config;
using Project.Scripts.Manager;
using Project.Scripts.Scroller;
using Project.Scripts.UI.Screen;
using UnityEngine;
using UnityEngine.UI;
using AudioType = Project.Scripts.Manager.AudioType;

namespace Project.Scripts.UI.Popup
{
    public class PopupSetting : UIPopup
    {
        [SerializeField] private GameObject turnOnBgm;
        [SerializeField] private GameObject turnOnSfx;
        [SerializeField] private List<Image> imageTiles;
        [SerializeField] private ListThemeTileConfig listThemeTileConfig;
        public event Action OnClosePopup;

        private void Awake()
        {
            turnOnBgm.SetActive(!AudioManager.Instance.IsMuted(AudioType.BGM));
            turnOnSfx.SetActive(!AudioManager.Instance.IsMuted(AudioType.Sfx));
        }

        private ThemeTileData Data => listThemeTileConfig.listThemeTileData.Find(t => t.isSelected);
        private void OnEnable()
        {
            ThemeTileScroller.OnEditTiles += SetEditTiles;
            SetEditTiles();
        }

        private void OnDisable()
        {
            ThemeTileScroller.OnEditTiles -= SetEditTiles;
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            OnClosePopup?.Invoke();
        }

        public void SetEditTiles()
        {
            for (int i = 0; i < 3; i++)
            {
                imageTiles[i].sprite = Data.listThemeTiles[i];
            }
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
            UIManager.OpenPopup<PopupEditThemeTile>(PopupType.PopupEditTheme);
        }
    }
}
