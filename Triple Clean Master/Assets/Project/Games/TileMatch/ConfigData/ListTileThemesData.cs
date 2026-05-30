using UnityEngine;
using System;
using System.Collections.Generic;

namespace Games.TileMatch.ConfigData
{
    [Serializable]
    public class TileThemeData
    {
        public int id;
        public TypeTileTheme typeTileTheme;
        public List<Sprite> spriteTile;
        public bool isSelected;
    }
    [Serializable]
    [CreateAssetMenu(fileName = "List Themes Data")]
    public class ListTileThemesData : ScriptableObject
    {
        public List<TileThemeData> listTileSprite;

    }
}
