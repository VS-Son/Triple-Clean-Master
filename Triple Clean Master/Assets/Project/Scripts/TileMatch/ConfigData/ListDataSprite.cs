using System;
using System.Collections.Generic;
using Project.Scripts.TileMatch.Tiles.Data;
using UnityEngine;

namespace Project.Scripts.TileMatch.ConfigData
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
