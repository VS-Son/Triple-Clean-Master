using System;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using Project.Scripts.TileMatch.Manager;
using Project.Scripts.TileMatch.Tiles.Theme;
using Project.Scripts.UI.Screen;
using Project.Services;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.Scripts.Scroll
{
    public class ThemeTileScroller : MonoBehaviour, IEnhancedScrollerDelegate
    {
        public static event Action<ThemeTileData> ApplyThemeTiles;
        [SerializeField] private EnhancedScroller themeScroller;
        [SerializeField] private EnhancedScrollerCellView prefab;
        [SerializeField] private ListThemeTileConfig listThemeTileConfig;
        private readonly List<ThemeTileCellView> _activeCellViews = new();
        private int _selectedId = -1;
        private bool _isInitialized;

        
        private void OnEnable()
        {
            if (!_isInitialized) return;

            OnOpenSelected();
        }
        private void Start()
        {
            themeScroller.Delegate = this;
            themeScroller.ReloadData();

            _isInitialized = true;

            OnOpenSelected();
        }
        
        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return listThemeTileConfig.listThemeTileData.Count;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return 319f;
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var themeData = listThemeTileConfig.listThemeTileData[dataIndex];
            var themeView = themeScroller.GetCellView(prefab);
            if (themeView is ThemeTileCellView cellView)
            {
                cellView.SetData(themeData, _selectedId);
                cellView.Onclick = OnSelect;
                if (!_activeCellViews.Contains(cellView))
                {
                    _activeCellViews.Add(cellView);
                }
            }

            return themeView;
        }

        private void OnSelect(ThemeTileCellView cellView)
        {
            _selectedId = cellView.id;

            RefreshOnSelect();

            
        }
        private void RefreshOnSelect()
        {
            foreach (var cell in _activeCellViews)
            {
                if (cell == null) continue;

                bool isSelected = cell.id == _selectedId;
                cell.SetSelected(isSelected);
            }
        }

        private void OnOpenSelected(bool scroll = true)
        {
            ThemeTileData selectedTheme = listThemeTileConfig.listThemeTileData.Find(t => t.isSelected);

            if (selectedTheme == null)
            {
                _selectedId = -1;
                RefreshOnSelect();
                return;
            }

            _selectedId = selectedTheme.id;

            RefreshOnSelect();

            if (scroll)
            {
                int dataIndex = listThemeTileConfig.listThemeTileData.FindIndex(t => t.id == _selectedId);

                if (dataIndex >= 0)
                {
                    themeScroller.JumpToDataIndex(dataIndex);
                }
            }
        }
        public void ApplyThemeTile()
        {
            if (_selectedId != -1)
            {
                foreach (var theme in listThemeTileConfig.listThemeTileData)
                {
                    theme.isSelected = (theme.id == _selectedId);
                }

                ThemeTileData confirmed = listThemeTileConfig.listThemeTileData.Find(t => t.isSelected);
                TileManager.Instance.ApplyThemeTiles(confirmed);
                ApplyThemeTiles?.Invoke(confirmed);
                UIManager.GetUI<SettingScreen>(TypeScreen.Setting).SetEditTiles();
                 _selectedId = -1;
                 themeScroller.ReloadData();
                 
            }
           
        }
        
    }
}
