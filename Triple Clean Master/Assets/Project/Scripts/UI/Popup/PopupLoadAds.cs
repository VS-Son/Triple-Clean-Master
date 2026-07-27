using System;
using System.Collections;
using Project.Scripts.Manager;
using UnityEngine;

namespace Project.Scripts.UI.Popup
{
    public class PopupLoadAds : MonoBehaviour
    {
        [SerializeField] private float duration = 1f;
        private void OnEnable()
        {
            StartCoroutine(TimeLoadAds());
        }
        
        private IEnumerator TimeLoadAds()
        {
            float timeElapsed = 0;
            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            UIManager.ClosePopup(PopupType.LoadAds);
            UIManager.OpenPopup<PopupSorry>(PopupType.Sorry);
        }
    }
}
