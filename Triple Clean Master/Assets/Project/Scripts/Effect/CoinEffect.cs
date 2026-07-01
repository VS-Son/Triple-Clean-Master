using System;
using System.Collections;
using DG.Tweening;
using Project.Scripts.Manager;
using Project.Scripts.UI.Screen;
using TMPro;
using UnityEngine;

namespace Project.Scripts.Effect
{
    public class CoinEffect : MonoBehaviour
    {
        public static event Action OnCompleteGoal;
        private readonly int _coinPerSecond = 10;
        private int _countPerSecond;
        private int _completeCount;
        private int _currentCoin;
        public Transform startSpawnPoint;
        public Coin prefab;
        public GameObject coinObject;
        public TMP_Text amountCoin;
        private  Transform TargetCoin => UIManager.GetCommon<StatusBar>(CommonUIType.StatusBar).iconCoin;

        private void OnEnable()
        {
            NextScreen.GetCoin += RewardCoin;
        }
        private void OnDisable()
        {
            NextScreen.GetCoin -= RewardCoin;
        }

        private void RewardCoin(int amount)
        {
            coinObject.SetActive(true);
            amountCoin.text = $"x{amount}";
            _countPerSecond = amount / _coinPerSecond;
            _completeCount = 0;
            StartCoroutine(SpawnCoins());
        }

        private IEnumerator SpawnCoins()
        {
            yield return new WaitForSeconds(0.5f);
            coinObject.SetActive(false);
            for (int i = 0; i < _countPerSecond; i++)
            {
                var startPos = startSpawnPoint.position;
                //  var Coin = Instantiate(prefab, startPos, Quaternion.identity, transform);
                var coin = GameplayManager.PoolCoin.GetPool(prefab, startPos, transform);
                StartCoroutine(ParabolaMove(coin, startPos, TargetCoin.position, 0.7f, 1f));
                yield return new WaitForSeconds(0.1f);
            }
        
        }

        private IEnumerator ParabolaMove(Coin coin, Vector2 start, Vector2 end, float height, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float time = Mathf.Clamp01(elapsed / duration);
                Vector2 current = Vector2.Lerp(start, end, time);
                current.x -= height * 7 * time * (1 - time);
                coin.transform.position = current;
                yield return null;
            }
            _completeCount++;
            StartCoroutine(CoinAnimated(_coinPerSecond, coin));
            if (_completeCount >= _countPerSecond)
            {
                OnCompleteGoal?.Invoke();
            }

        }
    
        IEnumerator CoinAnimated(int amount, Coin coin)
        {
            int start = _currentCoin;
            int target = start + amount;
            float duration = 0.001f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float time = elapsed / duration;
                _currentCoin = Mathf.RoundToInt(Mathf.Lerp(start, target, time));
                yield return null;
            }

            _currentCoin = target;
            TargetCoin.transform.DOScale(new Vector2(1.4f, 1.4f), 0.4f).OnComplete((() =>
            {
                TargetCoin.transform.DOScale(1, 0.4f);
            }));
            GameplayManager.PoolCoin.Release(coin);
            PlayerInventoryManager.AddCoin(_coinPerSecond);

        }
    }
}