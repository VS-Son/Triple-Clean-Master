using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Project.Scripts.Constants;
using Project.Scripts.Manager;
using Project.Scripts.TileMatch.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Screen
{
   public class HomeScreen : UIScreen
   {
      //public override TypeScreen Type => TypeScreen.HomeScreen;
      [SerializeField] private Transform btPlay;
      [SerializeField] private List<RectTransform> logoTiles;
      [SerializeField] private Image bannerExplorer;
      [SerializeField] private TMP_Text textLevel;
      public static Action UpdateLevelText;
      public static Action<bool,bool> OnActveStatusBar;
      private void Start()
      {
         foreach (var tileIndex in logoTiles)
         {
            tileIndex.transform.localScale = new Vector3(0, 0);
         }
         btPlay.localScale = Vector3.zero;
         StartCoroutine(ScaleTilesSequentially());
         UpdayeTextLevel();
      }

      IEnumerator ScaleTilesSequentially(int index = 0)
      {
         if (index >= logoTiles.Count)
         {
            StartCoroutine(OnBanner());
            yield break;
         }

         var tile = logoTiles[index];
         tile.DOScale(new Vector3(1.23f, 1.23f), 0.25f)
            .OnComplete(() => { StartCoroutine(ScaleTilesSequentially(index + 1)); });

         yield return null;
      }
      IEnumerator OnBanner()
      {
         var duration = 0.005f;
         while (bannerExplorer.fillAmount < 1f)
         {
            bannerExplorer.fillAmount += 0.01f;
            float clampedTime = Mathf.Clamp01(bannerExplorer.fillAmount);
            bannerExplorer.fillAmount = clampedTime;
            yield return new WaitForSeconds(duration);
         }
         btPlay.DOScale(1, 0.4f);
      }
      public void OnPlay()
      {
         StateUI.ChangeState(ScreenType.PlayScreen);
         AudioManager.Instance.PlayBGM(AudioConstants.BGM, 1);
         AudioManager.Instance.PlaySfx(AudioConstants.HighPitchDefault);
         UpdateLevelText?.Invoke();
         OnActveStatusBar?.Invoke(false,true);
         TileManager.ZoomScaleTile();
         GameplayManager.SetBoardActive(true);
         GameplayManager.SetTileManagerActive(true);
      }

      public void UpdayeTextLevel()
      {
         textLevel.text = $"Level {GameplayManager.CurrentLevel}";
      }
   }
}
