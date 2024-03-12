using UnityEngine;
using UnityEngine.UI;

namespace FS_Runtimes.Controllers.UI
{
    public class GameErrorUIController : AbstractUIController
    {
        #region Fields & Properties

        [Header("Button UI")] 
        [SerializeField, Tooltip("Confirm Button")] private Button _confirmButton;

        #endregion

        #region Methods

        /// <summary>
        /// Call this method to initialize button's callback.
        /// </summary>
        public void Init()
        {
            if (_confirmButton is not null)
            {
                _confirmButton.onClick.RemoveAllListeners();
                _confirmButton.onClick.AddListener(Application.Quit);
            }
        }

        #endregion
    }
}
