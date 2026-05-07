using System.Collections.Generic;
using Games.TileMatch.Board.Scripts;
using Games.TileMatch.Level.Data;
using Games.TileMatch.Manager;
using Games.TileMatch.Tiles.Data;
using Project.Extensions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Games.TileMatch.Tiles.Scripts
{
    public class Tile : TileData, IPointerClickHandler
    {
        public List<Tile> coveredBy = new();
        public List<Tile> covers = new(); 
        public void SetData(LayersData layer, int x, int y)
        {
            currentLayer = layer.layer;
            transform.localScale = Util.SetScale(layer);
            row = x;
            col = y;
            tileId = TileManager.Instance.GetDistributedTileType();
            spriteTile.sprite = TileManager.Instance.SetSpriteForTile(tileId);
            spriteTile.sortingOrder = layer.layer;
        }

        public void SetUndoData()
        {
            isCollected = false;
            collider.enabled = true;
            spriteTile.sortingOrder = originalLayer;
            transform.SetParent(null);
           transform.SetParent( TileManager.Instance.SetLayerParent(originalLayer));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Click");
            if (isSelect)
            {
                collider.enabled = false;
                TileManager.Instance.HandleTileCollected(this);
                BoardCollectTile.Instance.CollectTile(this);
            }
            
        }
    }
}
