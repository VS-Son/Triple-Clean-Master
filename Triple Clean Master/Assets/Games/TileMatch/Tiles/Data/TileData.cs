using System;
using UnityEngine;

namespace Games.TileMatch.Tiles.Data
{
    public enum TileId {Id1, Id2, Id3, Id4, Id5 }
    public class TileData : MonoBehaviour
    {
        public SpriteRenderer spriteTile;
        public TileId tileId;
        public int col;
        public int row;
        public int currentLayer;
        public Vector2 originalPos;
        public int originalLayer;
    }
}
