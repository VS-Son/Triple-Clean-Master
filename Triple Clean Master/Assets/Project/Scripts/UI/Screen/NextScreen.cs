using System;
using System.Collections;
using DG.Tweening;
using Project.Core.UI;
using Project.Games.TileMatch.Board.Scripts;
using Project.Manager;
using Project.Scripts.Manager;
using Project.Scripts.TileMatch.Manager;
using Project.Services;
using TMPro;
using UI.Screen;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Screen
{
    public class NextScreen : UICanvas
    {
        [SerializeField] private TMP_Text textNext;
        [SerializeField] private Slider progressionFill;
        [SerializeField] private TMP_Text textProgression;
        [SerializeField] private Button btNext;
        [SerializeField] private Animator animGift;
        [SerializeField] private Image iconGift;
        [SerializeField] private int amountReward;
        [SerializeField] private int countProgression;
        public static event Action<int> GetCoin;
        private static int NextLevel => GameplayManager.CurrentLevel + 1; 


       
        private void OnEnable()
        {
            textNext.text = "Level " + ( GameplayManager.CurrentLevel + 1);
            UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).OnActiveStatus(false);
            btNext.transform.localPosition = Vector3.zero;
            if (countProgression <= 0)
            {
                progressionFill.gameObject.SetActive(true);
                iconGift.gameObject.SetActive(true);
            }
            UpdateProvenceReward();
            CoinEffect.OnCompleteGoal += OnCompleteGoal;

            
        }

        private void OnDisable()
        {
            CoinEffect.OnCompleteGoal -= OnCompleteGoal;
        }

        private void UpdateProvenceReward()
        {
            ProgressionFill();
        }

        private void ProgressionFill()
        {
            progressionFill.value = countProgression/4f;
            countProgression = Mathf.Min(countProgression + 1, 4);
            textProgression.text = $"Provence {countProgression}/4";
            var amountValue = progressionFill.maxValue / 4f;
            var targetValue = Mathf.Min(progressionFill.value + amountValue, progressionFill.maxValue);
            StartCoroutine(IncreaseProgressionFill(progressionFill.value,targetValue, 0.3f ));


        }

        private IEnumerator IncreaseProgressionFill(float from, float to, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                progressionFill.value = Mathf.Lerp(from, to, elapsed / duration);
                yield return null;
            }
            progressionFill.value = to;
            if (countProgression <= 3)
            {
                OnCompleteGoal();
            }
            if (to >= progressionFill.maxValue)
            {
                progressionFill.gameObject.SetActive(false);
                animGift.enabled = true;
                iconGift.gameObject.SetActive(false);
                StartCoroutine(OpenRewardGift());
                progressionFill.value = 0.5f; 
                countProgression = 0;
                textProgression.text = $"Provence {countProgression}/4";
                
            }
        }
        private void OnCompleteGoal()
        {
            btNext.gameObject.SetActive(true);
            btNext.transform.DOScale(1, 0.6f).OnComplete(()=>
            {
                btNext.enabled = true;
            });
        }

        IEnumerator OpenRewardGift()
        {
            while (animGift.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f || animGift.IsInTransition(0))
            {
                yield return null;
            }
            animGift.enabled = false;
            animGift.gameObject.SetActive(false);
            GetCoin?.Invoke(amountReward);

        }

        public void OnNext()
        {
            //TileManager.ZoomScaleTile();
            TileManager.Instance.NextLevel();
            BoardCollectTile.Instance.ResetBoard();

            UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).textTitle.gameObject.SetActive(true);
            UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).UpdateLevelText();
            UIManager.GetUI<StatusBar>(TypeScreen.StatusBar).OnActiveStatus(true);
            StateUI.ChangeState(TypeScreen.PlayScreen);
            Close();
        }
    }
}
