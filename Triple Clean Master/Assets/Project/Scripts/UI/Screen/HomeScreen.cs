using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Games.TileMatch.Manager;
using Project.Constants;
using Project.Core.UI;
using Project.Manager;
using Project.Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screen
{
   public class HomeScreen : UICanvas
   {
      //public override TypeScreen Type => TypeScreen.HomeScreen;
      [SerializeField] private Transform btPlay;
      [SerializeField] private List<RectTransform> logoTiles;
      [SerializeField] private Image bannerExplorer;

      private void Start()
      {
         foreach (var tileIndex in logoTiles)
         {
            tileIndex.transform.localScale = new Vector3(0, 0);
         }
         btPlay.localScale = Vector3.zero;
         StartCoroutine(ScaleTilesSequentially());
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
         
         StateUI.ChangeState(TypeScreen.PlayScreen);
         AudioManager.Instance.PlayBGM(AudioConstants.BGM, 1);
         AudioManager.Instance.PlaySfx(AudioConstants.HighPitchDefault);
         UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).UpdateLevelText();
         UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).back.SetActive(true);
         UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).textTitle.gameObject.SetActive(true);
         TileManager.ZoomScaleTile();
         GameplayManager.Show(true);
         PlayScreen.UpdateUnlockBooster();
         
      }
   }
}
