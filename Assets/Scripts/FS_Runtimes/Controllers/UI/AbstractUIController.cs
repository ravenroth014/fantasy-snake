using UnityEngine;

namespace FS_Runtimes.Controllers.UI
{
    public abstract class AbstractUIController : MonoBehaviour
    {
        #region Methods
        
        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }
        
        #endregion
    }
}
