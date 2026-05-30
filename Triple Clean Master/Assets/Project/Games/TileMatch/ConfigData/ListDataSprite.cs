using System;
using System.Collections.Generic;
using Games.TileMatch.Tiles.Data;
using UnityEngine;

namespace Games.TileMatch.ConfigData
{
    public enum TypeTileTheme
    {
        Fruits,
        Element,
        Candy
    }
    
    [Serializable]
    public class TileSpriteData
    {
        public TileId  typeId;
        public Sprite sprite;
    }
    [Serializable]
    [CreateAssetMenu(fileName = "List Data Sprite")]
    public class ListDataSprite : ScriptableObject
    {
        public List<TileSpriteData> listTileSprite;
    }
   
}
