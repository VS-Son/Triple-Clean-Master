using UnityEngine;

namespace Project.Manager
{
    public class PlayManager : Singleton<PlayManager>
    {
        public static void SetActive(bool isActive)
        {
            Instance.gameObject.SetActive(isActive);
        }
    }
}
