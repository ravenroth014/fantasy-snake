using FS_Runtimes.Controllers.Core;
using FS_Runtimes.UI_Extension;
using FS_Runtimes.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FS_Runtimes.Controllers.UI
{
    public class MainMenuController : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Main Menu Buttons")] 
        [SerializeField, Tooltip("Start Button")] private SelectedButton _startButton;
        [SerializeField, Tooltip("Setting Button")] private SelectedButton _settingButton;
        [SerializeField, Tooltip("Exit Button")] private SelectedButton _exitButton;

        [Header("Event Systems")] 
        [SerializeField, Tooltip("Event System")] private EventSystem _eventSystem;

        private SettingManager _settingManager;
        private ECurrentMainMenu _currentMainMenuState;

        #endregion

        #region Methods

        public void Init()
        {
            _settingManager ??= SettingManager.Instance;
            _eventSystem.firstSelectedGameObject = _startButton.gameObject;
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

        #endregion
    }
}
