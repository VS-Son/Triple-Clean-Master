using System.Collections.Generic;
using Games.TileMatch.Level.Data;
using Games.TileMatch.Manager;
using Games.TileMatch.Tiles.Data;
using Project.Extensions;
using DG.Tweening;
using Project.Constants;
using Project.Games.TileMatch.Board.Scripts;
using Project.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Games.TileMatch.Tiles.Scripts
{
    public class Tile : TileData, IPointerClickHandler,IPool
    {
        public List<Tile> coveredBy = new();
        public List<Tile> covers = new(); 
        public void SetData(LayersData layer, int x, int y)
        {
            transform.localRotation = Quaternion.identity;
            collider.enabled = true;
            isCollected = false;
            currentLayer = layer.layer;
            transform.localScale = Util.SetScale();
            row = x;
            col = y;
            tileId = TileManager.Instance.GetDistributedTileType();
            spriteTile.sprite = TileManager.Instance.SetSpriteForTile(tileId);
            spriteTile.sortingOrder = layer.layer;
        }

        public void SetDataUndo()
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
                AudioManager.Instance.PlaySfx(AudioConstants.Select);
                collider.enabled = false;
                TileManager.Instance.HandleTileCollected(this);
                BoardCollectTile.Instance.CollectTile(this);
            }
            else
            {
                
            }
            
        }
    }
}
