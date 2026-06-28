using System;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using Project.Scripts.Config;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.TileMatch.Theme
{
    public class ThemeTileCellView : EnhancedScrollerCellView
    {
        public int id;
        public List<Image> image;
        public GameObject bgSelected;
        private ThemeTileData _data;
        public Action<ThemeTileCellView> Onclick;

        public void OnSelect()
        {
            Onclick?.Invoke(this);
        }

        public void SetData(ThemeTileData data, int selectedId)
        {
            _data = data;
            id = data.id;
            for (int index = 0; index < image.Count; index++)
            {
                image[index].sprite = data.listThemeTiles[index];
            }
            bgSelected.SetActive(selectedId == -1 ? data.isSelected : (data.id == selectedId));
        }

        public ThemeTileData GetData()
        {
            return _data;
        }

        public void SetSelected(bool isSelected)
        {
            bgSelected.SetActive(isSelected);
        }
    }
}
