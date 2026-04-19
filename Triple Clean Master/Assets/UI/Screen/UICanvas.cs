using Project.Services;
using UnityEngine;

namespace UI.Screen
{
    public abstract class UICanvas : MonoBehaviour
    {
        public TypeScreen type;

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
            UIManager.Instance.RemoveBackUI(this);
            gameObject.SetActive(false);
        }
    }
}
