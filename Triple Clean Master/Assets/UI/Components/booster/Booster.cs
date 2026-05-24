using System;
using Games.TileMatch.Board.Scripts;
using Project.Manager;
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
        [SerializeField] private GameObject redDot;
        [SerializeField] private GameObject iconLock;
        public TypeBooster typeBooster;

        private void OnEnable()
        {
            PlayScreen.AlphaBooster += OnAlphaBooster;
           // NextScreen.Display += OnDisplayBooster;
        }
        private void OnDisable()
        {
            PlayScreen.AlphaBooster -= OnAlphaBooster;
           // NextScreen.Display -= OnDisplayBooster;


        }

        private void OnAlphaBooster(TypeBooster type, float alpha)
        {
            if (type == typeBooster)
            {
                canvasGroup.alpha = alpha;
                redDot.SetActive(true);
                iconLock.SetActive(false);
            }
        }
        private void OnDisplayBooster(TypeBooster type)
        {
            if (type == typeBooster)
            {
                redDot.SetActive(true);
                iconLock.SetActive(false);
            }
        }

        public void HideRedDot(TypeBooster type)
        {
            if (type == typeBooster )
            {
                redDot.SetActive(false);
                iconLock.SetActive(true);

            }
        }
        
    }
}
