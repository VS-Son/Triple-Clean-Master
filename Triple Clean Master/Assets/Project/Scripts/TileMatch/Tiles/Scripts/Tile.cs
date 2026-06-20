using System.Collections;
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

using DG.Tweening;
using Unity.VisualScripting;
using Sequence = DG.Tweening.Sequence;

namespace Games.TileMatch.Tiles.Scripts
{
    public class Tile : TileData, IPointerClickHandler,IPointerDownHandler,IPointerUpHandler

    {
        public List<Tile> coveredBy = new();
        public List<Tile> covers = new();
        
        private bool _isShaking = false;
        private bool _isHolding;
        private Tween  _swayTween;
        private Tween _scaleTween;
        private Tween _rotateTween;
        private float TransY => transform.position.y;
        private Vector2 LocalScale => transform.localScale;
        
        private readonly float _holdScale = 1.2f;

        public void SetData(LayersData layer, int x, int y, Vector2 localPos)
        {
            var transform1 = transform;
            transform1.localRotation = Quaternion.identity;
            collider.enabled = true;
            isCollected = false;
            isClearing = false;
            currentLayer = layer.layer;
            transform1.localPosition =  originalPos =localPos;
            transform.localScale = originalScale =Util.SetScale();
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
            if (!isSelect)
            {
                if (_isShaking) return;
                _isShaking = true;
                transform.DOShakePosition(
                    duration: 0.3f,
                    strength: new Vector3(0.1f, 0f, 0f),
                    vibrato: 10,
                    randomness: 0f,
                    snapping: false,
                    fadeOut: true
                ).OnComplete(() => { _isShaking = false; });
                return;
            }
            DoKill();   
            AudioManager.Instance.PlaySfx(AudioConstants.Select);
            collider.enabled = false;
            isCollected = true;
            transform.localRotation = Quaternion.identity;
            TileManager.Instance.HandleTileCollected(this);
            BoardCollectTile.Instance.CollectTile(this);
        }
        

         public void OnPointerDown(PointerEventData eventData)
         {
            
             if (isSelect)
             {
                 _isHolding = true;
                 DoKill();
                 _scaleTween = (transform.DOScale(LocalScale * _holdScale, 0.12f).SetEase(Ease.OutBack));

                 _swayTween = transform.DOMoveY(TransY + 0.3f, 1.5f).SetEase(Ease.InOutSine).OnComplete(() =>
                 {
                     if (!_isHolding) return;

                     _swayTween = transform.DOMoveY(TransY - 0.5f, 1.5f).SetEase(Ease.InOutSine)
                         .SetLoops(-1, LoopType.Yoyo);
                 });
                 _rotateTween = transform
                     .DOLocalRotate(new Vector3(0f, 0f, 4f), 1.3f)
                     .SetEase(Ease.InOutSine)
                     .OnComplete(() =>
                     {
                         if (!_isHolding) return;

                         _rotateTween = transform
                             .DOLocalRotate(new Vector3(0f, 0f, -4f), 1.3f)
                             .SetEase(Ease.InOutSine)
                             .SetLoops(-1, LoopType.Yoyo);
                     });
             }


         }

         public void OnPointerUp(PointerEventData eventData)
         {
             if (isSelect)
             {
                 DoKill();
                 _isHolding = false;
                 _scaleTween = (transform.DOScale(LocalScale / _holdScale, 0.1f).SetEase(Ease.OutQuad));
                 _swayTween = transform.DOMoveY(originalPos.y, 0.5f).SetEase(Ease.InOutSine);
                 _rotateTween = transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 0.3f);
             }
            
         }


         private void DoKill()
         {
             _swayTween?.Kill();
             _scaleTween?.Kill(); 
             _rotateTween.Kill();
         }
    }
}
