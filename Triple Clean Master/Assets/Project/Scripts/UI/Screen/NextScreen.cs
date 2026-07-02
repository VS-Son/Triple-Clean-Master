using System;
using System.Collections;
using DG.Tweening;
using Project.Scripts.Constants;
using Project.Scripts.Effect;
using Project.Scripts.Manager;
using Project.Scripts.TileMatch.Board;
using Project.Scripts.TileMatch.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Screen
{
    public class NextScreen : UIScreen
    {
        [SerializeField] private TMP_Text textNext;
        [SerializeField] private Slider progressionFill;
        [SerializeField] private TMP_Text textProgression;
        [SerializeField] private Button btNext;
        [SerializeField] private Animator animGift;
        [SerializeField] private Image iconGift;
        [SerializeField] private int amountReward;
        [SerializeField] private int countProgression;
        private const string ProgressionKey = "CountProgression";
        public static event Action<int> GetCoin;
        private static int NextLevel => GameplayManager.CurrentLevel + 1;

        public static Action<bool> OnActiveStatus;
        public static Action UpdateLevelText;

        private void Awake()
        {
            LoadCountProgression();
        }

        private void OnEnable()
        {
            textNext.text = "Level " + ( GameplayManager.CurrentLevel + 1);
            OnActiveStatus?.Invoke(false);
            
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
            if (countProgression >= 4)
            {
                countProgression = 0;
            }
            
        }

        private void UpdateProvenceReward()
        {
            ProgressionFill();
        }

        private void ProgressionFill()
        {
            progressionFill.value = countProgression/4f;
            countProgression = Mathf.Min(countProgression + 1, 4);
            SaveCountProgression();
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
                animGift.gameObject.SetActive(true);
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
            AudioManager.Instance.PlaySfx(AudioConstants.HighPitchDefault);
            TileManager.ZoomScaleTile();
            if (countProgression >= 4)
            {
                countProgression = 0;
            }
            TileManager.Instance.NextLevel();
            BoardCollectTile.Instance.ResetBoard();
            UpdateLevelText?.Invoke();
            OnActiveStatus?.Invoke(true);
            StateUI.ChangeState(ScreenType.PlayScreen);
            Close();
        }

        private void LoadCountProgression()
        {
            countProgression = PlayerPrefs.GetInt(ProgressionKey, countProgression);
        }

        private void SaveCountProgression()
        {
            PlayerPrefs.SetInt(ProgressionKey, countProgression);
            PlayerPrefs.Save();
        }
    }
}
