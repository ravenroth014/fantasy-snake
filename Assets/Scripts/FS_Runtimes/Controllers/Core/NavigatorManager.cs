using FS_Runtimes.Controllers.UI;
using UnityEngine;

namespace FS_Runtimes.Controllers.Core
{
    public class NavigatorManager : MonoBehaviour
    {
        #region Fields & Properties

        public MainMenuController MainMenuController => _mainMenuController;
        [SerializeField, Tooltip("Main Menu Controller")]
        private MainMenuController _mainMenuController;
        
        public static NavigatorManager Instance => _instance;
        private static NavigatorManager _instance;

        #endregion

        #region Methods

        private void Awake()
        {
            _instance = this;
        }

        #endregion
    }
}
