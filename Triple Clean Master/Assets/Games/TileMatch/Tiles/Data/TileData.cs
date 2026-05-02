using System;
using UnityEngine;

namespace Games.TileMatch.Tiles.Data
{
    public enum TileId {Id1, Id2, Id3, Id4, Id5 }
    public class TileData : MonoBehaviour
    {
        public TileId tileId;
        public int currentLayer;
        public int row;
        public int col;
        public Vector2 originalPos;
        public int originalLayer;
        public bool isSelect;
        public bool isCollected;
        public int coverCount = 0;
        public SpriteRenderer spriteTile;
        public new Collider2D collider;
    }
}
