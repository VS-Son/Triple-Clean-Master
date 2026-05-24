using System;
using System.Collections;
using System.Collections.Generic;
using Games.TileMatch.Tiles.Scripts;
using Project.Manager;
using UnityEngine;
using DG.Tweening;
using Games.TileMatch.Manager;
using Project.Core.UI;
using Project.Services;
using UI.Components.booster;
using UI.Screen;

namespace Games.TileMatch.Board.Scripts
{
    public class BoardCollectTile : Singleton<BoardCollectTile>
    {
        public static event Action<TypeBooster, float> AlphaBooster; 

       [SerializeField] public List<Transform> slots = new ();
       private readonly List<Tile> _collectedTile = new List<Tile>();
       private readonly List<Tile> _originalTile = new List<Tile>();
       private int _slotIndex;
       private int _countSlot;
       public static bool IsMatching;
       private Vector2 _originalScale;

       public void CollectTile(Tile tile)
       {
           tile.spriteTile.sortingOrder += 10;
           tile.originalPos = tile.transform.position;
           tile.originalLayer = tile.currentLayer;

           _originalScale = tile.transform.lossyScale;
           _collectedTile.Add(tile);
           _originalTile.Add(tile);
           _slotIndex = _collectedTile.Count;
           _slotIndex = _countSlot;
           _countSlot++;
           
           if (_collectedTile.Count > 0 && !PlayScreen.IsClick && !IsMatching)
           {
               PlayScreen.SetUndoAlpha(TypeBooster.Undo, 1f);
           }
           Debug.Log("slot index: "+_slotIndex);
           tile.transform.parent = slots[_slotIndex];
           SetMatchThree(tile);
           if (CheckShowRevive())
           {
               TileManager.Instance.DisableInput(false);
               DOVirtual.DelayedCall(0.5f, (() => StateUI.ChangeState(TypeScreen.Revive)));
              
           }
       }

       private bool CheckShowRevive()
       {
           return _countSlot >= 7 && !IsMatching;
       }
       private void SetMatchThree(Tile tile)
        {
            var matchThree = new List<Tile>();
            foreach (var t in _collectedTile)
            {
                if (tile.tileId == t.tileId)
                {
                    matchThree.Add(t);
                }
            }

            switch (matchThree.Count)
            {
                case 1:
                    tile.transform.DOMove(slots[_slotIndex].position, 0.3f).SetId("collect").OnComplete((() =>
                    {
                        DOVirtual.DelayedCall(0.1f,ReArrangeBoard);
                    }));
                    tile.transform.DOScale(1, 0.3f);
                    break;
                case 2:
                    InsertMatching(matchThree, tile,0);
                    break;
                case 3:
                    IsMatching = true;
                    PlayScreen.SetUndoAlpha(TypeBooster.Undo, 0.6f);
                    InsertMatching(matchThree, tile,1);
                    break;
            }
        }

        private void InsertMatching(List<Tile> matchThree, Tile tile ,int index)
        {
            Tile stMatch = matchThree[index];
            int targetIndex = _collectedTile.IndexOf(stMatch) + 1;
            _slotIndex = targetIndex;
            _collectedTile.Remove(tile);
            _collectedTile.Insert(targetIndex, tile);
            DOVirtual.DelayedCall(0.1f,ReArrangeBoard);
            tile.transform.DOMove(slots[_slotIndex].position, 0.3f).OnComplete((() =>
            {
                if (index == 1 && tile != null)
                {
                    DOVirtual.DelayedCall(0.1f, (() => StartCoroutine(ClearMatchedTiles(matchThree))));

                }

            }));
            tile.transform.DOScale(1, 0.3f);


        }

        IEnumerator ClearMatchedTiles(List<Tile> tileMatch)
        {
            foreach (var tile in tileMatch)
            {
                yield return tile.transform.DOScale(tile.transform.localScale, 0.05f)
                    .SetEase(Ease.OutBack).OnComplete((() =>
                    {
                        tile.transform.DOScale(0.001f, 0.1f);
                        
                    }))
                    .WaitForCompletion();
                yield return new WaitForSeconds(0.05f);
                _collectedTile.Remove(tile);
                _originalTile.Remove(tile);
                GameplayManager.PoolTile.Release(tile);
                _countSlot--;
                if (TileManager.Instance.CheckGridEmptyTile())
                {
                    DOVirtual.DelayedCall(0.5f, (() =>
                    {
                        StateUI.ChangeState(TypeScreen.NextScreen);
                        TileManager.Instance.NextLevel();
                        UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).levelText.gameObject.SetActive(false);

                    }));
                }

                
            }
            ReArrangeBoard();
            IsMatching = false;
            PlayScreen.IsClick = false;

        }

        private void ReArrangeBoard()
        {
            for (int i = 0; i < _collectedTile.Count; i++)
            {
                var tile = _collectedTile[i];
                tile.transform.parent = slots[i];
                DOTween.Kill("collect");
                tile.transform.DOMove(slots[i].position, 0.4f);
                tile.transform.DOScale(1, 0.1f);

            }
        }

        public void UndoTile(int quantity)
        {
            int count = Math.Min(quantity, _originalTile.Count);
            for (int i = 0; i < count; i++)
            {
                int index = _originalTile.Count - 1 - i;
                Tile tile = _originalTile[index];
                tile.transform.DOMove(tile.originalPos, 0.4f).SetEase(Ease.Flash).OnComplete((() =>
                {
                    if (quantity > 1)
                    {
                        DOVirtual.DelayedCall(0.1f,()=> TileManager.Instance.ShuffleGridTiles());
                    }

                }));
                tile.transform.DOScale(_originalScale, 0.6f);
                _collectedTile.Remove(tile);
                 tile.SetDataUndo();
                 TileManager.Instance.InitCoverageCount();
            }
            _originalTile.RemoveRange(_originalTile.Count-count,count);
            _countSlot -= count;
            ReArrangeBoard();
            if (_collectedTile.Count <= 0)
            {
                PlayScreen.SetUndoAlpha(TypeBooster.Undo, 0.6f);
            }
            
        }

        public void ResetBoard()
        {
            foreach (var tile in _collectedTile)
            {
                Destroy(tile.gameObject);
            }

            _slotIndex = 0;
            _countSlot = 0;
            _collectedTile.Clear();
            _originalTile.Clear();
        }

        public List<Tile> GetListCollect()
        {
            return new  List<Tile>(_collectedTile);
        }

        public static bool HasTileInBoard()
        {
            if (Instance._collectedTile.Count <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
