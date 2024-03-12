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
        private ECurrentMainMenu _currentMainMenuState;

        #endregion

        #region Methods

        public void Init()
        {
            _settingMenuUIController ??= NavigatorManager.Instance.SettingMenuUIController;
            _eventSystem.firstSelectedGameObject = _startButton.gameObject;

            InitCallback();
        }

        public void OnPlayerTrigger(EPlayerAction playerAction)
        {
            if (_currentMainMenuState == ECurrentMainMenu.Main)
                OnControlMainMenu();
            if (_currentMainMenuState == ECurrentMainMenu.Setting)
                OnControlSettingMenu(playerAction);
        }

        private void OnControlMainMenu()
        {
            if (_eventSystem.currentSelectedGameObject is not null) return;
            _eventSystem.SetSelectedGameObject(_startButton.gameObject);
        }

        private void OnControlSettingMenu(EPlayerAction playerAction)
        {
            
        }

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
    }
}
