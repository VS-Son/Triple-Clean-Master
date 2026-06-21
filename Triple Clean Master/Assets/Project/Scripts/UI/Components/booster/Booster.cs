using System;
using Project.Manager;
using Project.Scripts.UI.Screen;
using TMPro;
using UI.Screen;
using UnityEngine;

namespace UI.Components.booster
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
        [SerializeField] private CanvasGroup canvasGroup;
        public GameObject redValue;
        public GameObject coin;
        public GameObject ads;
        [SerializeField] private GameObject iconLock;
        [SerializeField] private TMP_Text boosterValue;

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
                //redValue.SetActive(true);
                iconLock.SetActive(false);
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

        public void UpdateTextBooster(TypeBooster type, int amount)
        {
            if (type == typeBooster)
            {
                boosterValue.text = $"{amount}";
            }
          
        }
    }
}
