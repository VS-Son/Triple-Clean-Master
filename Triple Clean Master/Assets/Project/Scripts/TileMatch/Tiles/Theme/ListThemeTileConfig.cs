using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.TileMatch.Tiles.Theme
{
    public enum ThemeType
    {
        Animal,
        Fruit,
        Candy
    }
    [Serializable]
    public class ThemeTileData
    {
        public int id;
        public ThemeType themeType;
        public List<Sprite> listThemeTiles;
        public bool isSelected;
    }
    [CreateAssetMenu(fileName = "ListTheme", menuName = "ListThemeData")]
    public class ListThemeTileConfig : ScriptableObject
    {
        public List<ThemeTileData> listThemeTileData;
    }
}
