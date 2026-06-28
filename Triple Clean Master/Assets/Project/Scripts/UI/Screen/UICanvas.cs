using Project.Scripts.Manager;
using UnityEngine;

namespace Project.Scripts.UI.Screen
{
    public abstract class UICanvas : MonoBehaviour
    {
        public TypeScreen typeBooster;

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
        
        public void CloseDirectly()
        {
            UIManager.RemoveBackUI(this);
            gameObject.SetActive(false);
        }
    }
}
