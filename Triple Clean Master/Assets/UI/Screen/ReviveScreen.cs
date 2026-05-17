using System;
using Games.TileMatch.Board.Scripts;
using Games.TileMatch.Manager;
using UnityEngine;
using DG.Tweening;
using Project.Manager;
using TMPro;

namespace UI.Screen
{
    public class ReviveScreen : UICanvas
    {
        public Transform bg;
        [SerializeField] private TMP_Text textLevel;
        [Header("Time Duration")] [SerializeField]
        private float timeScale;
        private void Start()
        {
            SetScale();

        }

        private void OnEnable()
        {
            bg.DOScale(1, timeScale);
            textLevel.text = ("Level " + PlayManager.CurrentLevel);
        }

        public void OnRevive()
        {
            BoardCollectTile.Instance.UndoTile(5);
            TileManager.Instance.DisableInput(true);
            Close();
            SetScale();
        }

        public void OnReplay()
        {
            BoardCollectTile.Instance.ResetBoard();
            TileManager.Instance.ResetTiles();
            Close();
            SetScale();

        }

        private void SetScale()
        {
            bg.localScale = new Vector3(0.1f, 0.1f);
            
        }
    }
}
