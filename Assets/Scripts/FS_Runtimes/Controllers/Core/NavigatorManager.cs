using FS_Runtimes.Controllers.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace FS_Runtimes.Controllers.Core
{
    public class NavigatorManager : MonoBehaviour
    {
        #region Fields & Properties

        public MainMenuUIController MainMenuUIController => _mainMenuUIController;
        [SerializeField, Tooltip("Main Menu Controller")]
        private MainMenuUIController _mainMenuUIController;

        public SettingMenuUIController SettingMenuUIController => _settingMenuUIController;
        [SerializeField, Tooltip("Setting Menu Controller")]
        private SettingMenuUIController _settingMenuUIController;

        public LoadingUIController LoadingUIController => _loadingUIController;
        [SerializeField, Tooltip("Loading Controller")]
        private LoadingUIController _loadingUIController;

        public GameplayUIController GameplayUIController => _gameplayUIController;
        [SerializeField, Tooltip("Gameplay UI Controller")]
        private GameplayUIController _gameplayUIController;

        public GameOverUIController GameOverUIController => _gameOverUIController;
        [SerializeField, Tooltip("Game over UI Controller")]
        private GameOverUIController _gameOverUIController;

        public GameErrorUIController GameErrorUIController => _gameErrorUIController;
        [SerializeField, Tooltip("Game error UI Controller")]
        private GameErrorUIController _gameErrorUIController;
        
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
