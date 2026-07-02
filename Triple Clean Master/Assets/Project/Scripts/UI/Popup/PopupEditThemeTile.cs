using System;
using Project.Scripts.Constants;
using Project.Scripts.Manager;
using Project.Scripts.Scroller;
using Project.Scripts.UI.Screen;
using UnityEngine;

namespace Project.Scripts.UI.Popup
{
    public class PopupEditThemeTile : UIPopup
    {
        private ThemeTileScroller ThemeScroll => FindObjectOfType<ThemeTileScroller>();
        

        public void OnConfirm()
        {
            AudioManager.Instance.PlaySfx(AudioConstants.HighPitchDefault);
            ThemeScroll.ApplyThemeTile();
            Close();
        }
    }
}
