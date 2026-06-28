using UnityEngine;

namespace Project.Scripts.TileMatch.Tiles.Data
{
    public enum TileId {None,Id1, Id2, Id3, Id4, Id5,Id6,Id7,Id8,Id9,Id10 }
    public class TileData : MonoBehaviour
    {
        public TileId tileId;
        public int currentLayer;
        public int row;
        public int col;
        public Vector2 originalPos;
        public Vector2 originalScale;
        public int originalLayer;
        public bool isSelect;
        public bool isCollected;
        public bool isClearing;
        public int coverCount = 0;
        public SpriteRenderer spriteTile;
        public new Collider2D collider;
       
    }
}
