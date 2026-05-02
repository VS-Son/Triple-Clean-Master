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
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor;

namespace Games.TileMatch.Board.Scripts
{
    public class BoardCollectTile : Singleton<BoardCollectTile>
    {
       [SerializeField] public List<Transform> slots = new ();
       private readonly List<Tile> _collectedTile = new List<Tile>();
       private readonly List<Tile> _originalTile = new List<Tile>();
       private int _slotIndex;
       private int _countSlot;
       private bool _isMatching;

       private Vector2 _originalScale;
       public void CollectTile(Tile tile)
        {
            tile.spriteTile.sortingOrder += 3;
            tile.originalPos = tile.transform.position;
            tile.originalLayer = tile.currentLayer;

            _originalScale = tile.transform.lossyScale;
            _collectedTile.Add(tile);
            _originalTile.Add(tile);
            _slotIndex = _collectedTile.Count;
            _slotIndex = _countSlot;
            _countSlot++;
            tile.transform.parent = slots[_slotIndex];
            SetMatchThree(tile);
            tile.transform.DOMove(slots[_slotIndex].position, 0.5f).OnComplete((() =>
            {
                ReArrangeBoard();
                if(_isMatching) return;
                if (_countSlot == 7)
                {
                    StateUI.ChangeState(TypeScreen.Revive);
                }
            }));
            tile.transform.DOScale(tile.transform.localScale + new Vector3(0.2f, 0.2f), 0.4f).SetLink(tile.gameObject)
                .OnComplete((() => tile.transform.DOScale(1, 0.2f)));
           
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

            if (matchThree.Count == 2)
            {
                InsertMatching(matchThree, tile,0);

            }
            if (matchThree.Count == 3)
            {
                _isMatching = true;
                InsertMatching(matchThree, tile,1);
                StartCoroutine(ClearMatchedTiles(matchThree));
            }
        }

        private void InsertMatching(List<Tile> matchThree, Tile tile ,int index)
        {
            Tile stMatch = matchThree[index];
            int targetIndex = _collectedTile.IndexOf(stMatch) + 1;
            _slotIndex = targetIndex;
            _collectedTile.Remove(tile);
            _collectedTile.Insert(targetIndex, tile);
            ReArrangeBoard();
        }
        IEnumerator ClearMatchedTiles(List<Tile> tileMatch)
        {
            yield return new WaitForSeconds(0.6f);
            foreach (var tile in tileMatch)
            {
                yield return tile.transform.DOScale(0.001f, 0.05f).OnComplete((() =>
                {
                    _collectedTile.Remove(tile);
                    _originalTile.Remove(tile);
                    _countSlot--;
                    Destroy(tile.gameObject);
                    _isMatching = false;
                })).SetLink(tile.gameObject).WaitForCompletion();
               
            }
            ReArrangeBoard();
        }

        private void ReArrangeBoard()
        {
            for (int i = 0; i < _collectedTile.Count; i++)
            {
                var tile = _collectedTile[i];
                tile.transform.parent = slots[i];
                tile.transform.DOMove(slots[i].position, 0.3f);

            }
        }

        public void UndoTile(int numberUndo)
        {
            int count = Math.Min(numberUndo, _originalTile.Count);
            for (int i = 0; i < count; i++)
            {
                int index = _originalTile.Count - 1 - i;
                Tile tile = _originalTile[index];
                tile.transform.DOMove(tile.originalPos, 0.4f).SetEase(Ease.Flash).OnComplete((() =>
                {
                    if (numberUndo > 1)
                    {
                        
                    }

                }));
                tile.transform.DOScale(_originalScale, 0.6f);
                _collectedTile.Remove(tile);
                 tile.SetUndoData();
                 TileManager.Instance.InitCoverageCount();
            }
            _originalTile.RemoveRange(_originalTile.Count-count,count);
            _countSlot -= count;
            ReArrangeBoard();
        }

        public void ResetBoard()
        {
            foreach (var tile in _collectedTile)
            {
                Destroy(tile.gameObject);
            }

            _slotIndex = 0;
            _collectedTile.Clear();
            _originalTile.Clear();
        }
    }
}
