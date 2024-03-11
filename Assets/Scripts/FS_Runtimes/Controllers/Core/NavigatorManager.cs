using FS_Runtimes.Controllers.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace FS_Runtimes.Controllers.Core
{
    public class NavigatorManager : MonoBehaviour
    {
        #region Fields & Properties

        public MainMenuController MainMenuController => _mainMenuController;
        [SerializeField, Tooltip("Main Menu Controller")]
        private MainMenuController _mainMenuController;

        public SettingMenuController SettingMenuController => _settingMenuController;
        [SerializeField, Tooltip("Setting Menu Controller")]
        private SettingMenuController _settingMenuController;

        public LoadingStateController LoadingStateController => _loadingStateController;
        [SerializeField, Tooltip("Loading Controller")]
        private LoadingStateController _loadingStateController;
        
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
