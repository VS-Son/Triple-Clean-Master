using Project.Scripts.Constants;
using Project.Scripts.Manager;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class UIBase : MonoBehaviour
    {
        public virtual void Open()
        {
            gameObject.SetActive(true);
            OnOpened();
            
        }

        public virtual void Close()
        {
            OnClosed();
            AudioManager.Instance.PlaySfx(AudioConstants.HighPitchDefault);
            gameObject.SetActive(false);
        }

        protected virtual void OnOpened() { }

        protected virtual void OnClosed() { }
    }

    public abstract class UIScreen : UIBase
    {
        
    }

    public abstract class UICommon : UIBase
    {
       
    }

    public abstract class UIPopup : UIBase
    {
        public virtual bool CloseOnBack => true;

        public virtual void CloseSelf()
        {
            UIManager.ClosePopup(this);
        }
    }
}
