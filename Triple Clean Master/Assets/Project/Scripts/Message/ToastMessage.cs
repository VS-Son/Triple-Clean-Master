using DG.Tweening;
using Project.Scripts.Manager;
using TMPro;
using UnityEngine;

namespace Project.Scripts.Message
{
    public class ToastMessage : Singleton<ToastMessage>
    {
        [SerializeField] private RectTransform messagePanel;
        [SerializeField] private TMP_Text messageText;
        private static Sequence  _currentSequence;
    
        public static void ShowMessage(string message)
        {
            _currentSequence?.Kill();

            Instance.messagePanel.gameObject.SetActive(true);
            Instance.messageText.text = message;
            _currentSequence = DOTween.Sequence().Append(Instance.messagePanel.DOScale(1.2f, 0.2f)).Append(Instance.messagePanel.DOScale(1f, 0.15f)).AppendInterval(1f).Append(Instance.messagePanel.DOScale(0f, 0.15f)).OnComplete(
                () =>
                {
                    Instance.messagePanel.gameObject.SetActive(false);
                });
        }
        
    }
}
