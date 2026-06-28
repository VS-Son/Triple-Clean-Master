using DG.Tweening;
using Project.Scripts.Manager;
using Project.Scripts.TileMatch.Board;
using Project.Scripts.TileMatch.Manager;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UI.Screen
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
            textLevel.text = ("Level " + GameplayManager.CurrentLevel);
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
