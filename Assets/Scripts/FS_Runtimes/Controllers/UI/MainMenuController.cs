using FS_Runtimes.Controllers.Core;
using FS_Runtimes.UI_Extension;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FS_Runtimes.Controllers.UI
{
    public class MainMenuController : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Buttons")] 
        [SerializeField, Tooltip("Start Button")] private SelectedButton _startButton;
        [SerializeField, Tooltip("Setting Button")] private SelectedButton _settingButton;
        [SerializeField, Tooltip("Exit Button")] private SelectedButton _exitButton;

        [Header("Event Systems")] 
        [SerializeField, Tooltip("Event System")] private EventSystem _eventSystem;

        private SettingManager _settingManager;

        #endregion

        #region Methods

        public void Init()
        {
            _settingManager ??= SettingManager.Instance;
            _eventSystem.firstSelectedGameObject = _startButton.gameObject;
        }

        #endregion
    }
}
