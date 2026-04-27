using Games.TileMatch.Level.Data;
using Games.TileMatch.Manager;
using Games.TileMatch.Tiles.Data;
using Project.Services;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Games.TileMatch.Tiles.Scripts
{
    public class Tile : TileData, IPointerClickHandler
    {
        public void SetPropertyTile(LayersData layer, int x, int y)
        {
            currentLayer = layer.layer;
            transform.localScale = SetScale(layer);
            row = x;
            col = y;
            tileId = TileManager.Instance.GetDistributedTileType();
            spriteTile.sprite = TileManager.Instance.SetSpriteForTile(tileId);
            spriteTile.sortingOrder = layer.layer;
        }
        private Vector3 SetScale(LayersData layer)
        {
            float scale = 1;
            if (layer.layer % 2 == 0)
            {
                scale = layer.cols switch
                {
                    <= 2 => 1f,
                    <= 4 and > 2 => 0.8f,
                    <= 7 and > 4 => 0.5f,
                    _ => scale
                };
            }
            else
            {
                scale = layer.cols switch
                {
                    <= 3 => 1,
                    <= 5 and > 3 => 0.8f,
                    <= 8 and > 5 => 0.5f,
                    _ => scale
                };
            }
            return new Vector3(scale, scale);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Click");
        }
    }
}
