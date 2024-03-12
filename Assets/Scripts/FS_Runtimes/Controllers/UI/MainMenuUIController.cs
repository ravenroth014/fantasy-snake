using FS_Runtimes.Controllers.Core;
using FS_Runtimes.UI_Extension;
using FS_Runtimes.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FS_Runtimes.Controllers.UI
{
    public class MainMenuUIController : AbstractUIController
    {
        #region Fields & Properties

        [Header("Main Menu Buttons")] 
        [SerializeField, Tooltip("Start Button")] private SelectedButton _startButton;
        [SerializeField, Tooltip("Setting Button")] private SelectedButton _settingButton;
        [SerializeField, Tooltip("Exit Button")] private SelectedButton _exitButton;

        [Header("Event Systems")] 
        [SerializeField, Tooltip("Event System")] private EventSystem _eventSystem;

        private SettingMenuUIController _settingMenuUIController;

        #endregion

        #region Methods

        #region Init Methods

        /// <summary>
        /// Call this method to initialize UI.
        /// </summary>
        public void Init()
        {
            _settingMenuUIController ??= NavigatorManager.Instance.SettingMenuUIController;
            _eventSystem.firstSelectedGameObject = _startButton.gameObject;

            InitCallback();
        }
        
        /// <summary>
        /// Call this method to initialize button's callback.
        /// </summary>
        private void InitCallback()
        {
            if (_startButton is not null)
            {
                _startButton.SetCallback(() =>
                {
                    GameManager.Instance.ChangeState(EGameState.GamePlay);
                });
            }

            if (_settingButton is not null)
            {
                _settingButton.SetCallback(() =>
                {
                    _settingMenuUIController.Open();
                    Close();
                });
            }

            if (_exitButton is not null)
            {
                _exitButton.SetCallback(Application.Quit);
            }
        }
        
        #endregion

        #region UI Controller Methods

        /// <summary>
        /// Call this method to handle UI from gamepad.
        /// </summary>
        /// <param name="playerAction"></param>
        public void OnPlayerTrigger(EPlayerAction playerAction)
        {
            OnControlMainMenu();
        }

        /// <summary>
        /// Call this method to select
        /// </summary>
        private void OnControlMainMenu()
        {
            if (_eventSystem.currentSelectedGameObject is not null) return;
            _eventSystem.SetSelectedGameObject(_startButton.gameObject);
        }
        
        #endregion

        #endregion
    }
}
