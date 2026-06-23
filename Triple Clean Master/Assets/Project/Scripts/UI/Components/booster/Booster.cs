using Project.Scripts.Effect;
using Project.Scripts.UI.Screen;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UI.Components.booster
{
    public enum TypeBooster
    {
        Undo,
        MagicWand,
        Shuffle,
        None
    }
    public class Booster : MonoBehaviour
    {
        [SerializeField] private GameObject iconLock;
        public GameObject redValue;
        public GameObject coin;
        public GameObject ads;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text boosterValue;
        [SerializeField] private TMP_Text boosterCoin;

        public TypeBooster typeBooster;

        private void OnEnable()
        {
            PlayScreen.AlphaBooster += OnAlphaBooster;
        }
        private void OnDisable()
        {
            PlayScreen.AlphaBooster -= OnAlphaBooster;
        }

        private void OnAlphaBooster(TypeBooster type, float alpha)
        {
            if (type == typeBooster)
            {
                canvasGroup.alpha = alpha;
            }
        }
        public void OnDisplayBooster(TypeBooster type, float alpha, int value)
        {
            if (type == typeBooster)
            {
                iconLock.SetActive(false);
                canvasGroup.alpha = alpha;
                redValue.SetActive(value >= 1);
            }
        }
        public bool IsDisplay(TypeBooster type)
        {
            if (type == typeBooster)
            {
                if ( canvasGroup.alpha.Equals(1))
                {
                    return true;
                }
            }
            return false;
        }
        private void OnDisplayBooster(TypeBooster type)
        {
            if (type == typeBooster)
            {
                redValue.SetActive(true);
                iconLock.SetActive(false);
            }
        }

        public void HideRedDot(TypeBooster type)
        {
            if (type == typeBooster )
            {
                redValue.SetActive(false);
                iconLock.SetActive(true);

            }
        }

        public void UpdateTextBooster(TypeBooster type, int value, int spendCoin)
        {
            if (type == typeBooster)
            {
                if (value > 0)
                {
                    boosterValue.text = $"{value}";
                }
                else
                {
                    boosterCoin.text = $"{spendCoin}";
                }
                
            }
          
        }

        public void DisplayValue(TypeBooster type)
        {
            if (type == typeBooster)
            {
                coin.SetActive(false);
                redValue.SetActive(true);
                ads.SetActive(false);
            }
        }
        public void DisplayCoin(TypeBooster type)
        {
            if (type == typeBooster)
            {
                coin.SetActive(true);
                redValue.SetActive(false);
                ads.SetActive(false);
            }
        }
        public void DisplayAds(int value)
        {
            if (value <= 0 )
            {
                coin.SetActive(false);
                redValue.SetActive(false);
                ads.SetActive(true);
            }
        }
    }
}
