using System;
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
            ThemeScroll.ApplyThemeTile();
            Close();
        }
    }
}
