using UnityEngine;

namespace Project.Manager
{
    public class PlayManager : Singleton<PlayManager>
    {
        [Min(1)][SerializeField] private int level;

        public static int CurrentLevel
        {
            get => Instance.level;
            set => Instance.level = value;
        }
        
        public static void SetActive(bool isActive)
        {
            Instance.gameObject.SetActive(isActive);
        }
    }
}
