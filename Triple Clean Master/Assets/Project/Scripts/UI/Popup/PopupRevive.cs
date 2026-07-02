using DG.Tweening;
using Project.Scripts.Constants;
using Project.Scripts.Manager;
using Project.Scripts.TileMatch.Board;
using Project.Scripts.TileMatch.Manager;
using Project.Scripts.UI.Components.booster;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UI.Screen
{
    public class PopupRevive : UIPopup
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
            AudioManager.Instance.PlaySfx(AudioConstants.HighPitchDefault);
            BoardCollectTile.Instance.UndoTile(5);
            TileManager.Instance.DisableInput(true);
            StatusBar.Source = ShopOpenSource.Gameplay;
            Close();
            SetScale();
        }

        public void OnReplay()
        {
            AudioManager.Instance.PlaySfx(AudioConstants.HighPitchDefault);
            PlayScreen.SetBoosterAlpha(TypeBooster.Undo, 0.6f);
            BoardCollectTile.Instance.ResetBoard();
            TileManager.Instance.ResetTiles();
            StatusBar.Source = ShopOpenSource.Gameplay;
            Close();
            SetScale();

        }

        private void SetScale()
        {
            bg.localScale = new Vector3(0.1f, 0.1f);
            
        }
    }
}
