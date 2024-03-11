using FS_Runtimes.Controllers.Core;
using FS_Runtimes.Models.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FS_Runtimes.Controllers.UI
{
    public class SettingMenuController : AbstractUIController
    {
        #region Fields & Properties

        [Header("Setting Buttons")] 
        [SerializeField, Tooltip("Entity Setting Button")] private Button _entityButton;
        [SerializeField, Tooltip("Stat Setting Button")] private Button _statButton;
        [SerializeField, Tooltip("Growth Setting Button")] private Button _growthButton;
        [SerializeField, Tooltip("Spawn Setting Button")] private Button _spawnButton;
        [SerializeField, Tooltip("Apply Setting Button")] private Button _applyButton;
        [SerializeField, Tooltip("Back Setting Button")] private Button _backButton;
        [SerializeField, Tooltip("Default Setting Button")] private Button _defaultButton;

        [Header("Setting Option UI")] 
        [SerializeField, Tooltip("Entity Option UI")] private GameObject _entityUI;
        [SerializeField, Tooltip("Stat Option UI")] private GameObject _statUI;
        [SerializeField, Tooltip("Growth Option UI")] private GameObject _growthUI;
        [SerializeField, Tooltip("Spawn Option UI")] private GameObject _spawnUI;

        [Header("Entity Option UI")] 
        [SerializeField, Tooltip("Entity Warning Text")] private TextMeshProUGUI _entityWarningText;
        [SerializeField, Tooltip("Entity InputField")] private TMP_InputField _entityInputField;

        [Header("Stat Option UI")] 
        [SerializeField, Tooltip("Stat Limit Toggle")] private Toggle _statLimitToggle;
        [SerializeField, Tooltip("Stat Min Atk InputField")] private TMP_InputField _statMinAtkInputField;
        [SerializeField, Tooltip("Stat Max Atk InputField")] private TMP_InputField _statMaxAtkInputField;
        [SerializeField, Tooltip("Stat Min HP InputField")] private TMP_InputField _statMinHpInputField;
        [SerializeField, Tooltip("Stat Min HP InputField")] private TMP_InputField _statMaxHpInputField;

        [Header("Growth Option UI")] 
        [SerializeField, Tooltip("Growing by Count InputField")] private TMP_InputField _growByCountInputField;

        [Header("Spawn Option UI")] 
        [SerializeField, Tooltip("Spawn Max Entity InputField")] private TMP_InputField _spawnMaxEntityInputField;
        [SerializeField, Tooltip("Spawn Max Spawnable")] private TMP_InputField _spawnMaxSpawnableField;

        private SettingManager _settingManager;
        
        #endregion

        #region Methods

        public void Init()
        {
            InitButtonCallback();
            InitUI();
        }

        private void InitButtonCallback()
        {
            if (_entityButton is not null)
            {
                _entityButton.onClick.RemoveAllListeners();
                _entityButton.onClick.AddListener(() =>
                {
                    _entityUI.SetActive(true);
                    _statUI.SetActive(false);
                    _growthUI.SetActive(false);
                    _spawnUI.SetActive(false);
                });
            }

            if (_statButton is not null)
            {
                _statButton.onClick.RemoveAllListeners();
                _statButton.onClick.AddListener(() =>
                {
                    _entityUI.SetActive(false);
                    _statUI.SetActive(true);
                    _growthUI.SetActive(false);
                    _spawnUI.SetActive(false);
                });
            }

            if (_growthButton is not null)
            {
                _growthButton.onClick.RemoveAllListeners();
                _growthButton.onClick.AddListener(() =>
                {
                    _entityUI.SetActive(false);
                    _statUI.SetActive(false);
                    _growthUI.SetActive(true);
                    _spawnUI.SetActive(false);
                });
            }

            if (_spawnButton is not null)
            {
                _spawnButton.onClick.RemoveAllListeners();
                _spawnButton.onClick.AddListener(() =>
                {
                    _entityUI.SetActive(false);
                    _statUI.SetActive(false);
                    _growthUI.SetActive(false);
                    _spawnUI.SetActive(true);
                });
            }

            if (_backButton is not null)
            {
                _backButton.onClick.RemoveAllListeners();
                _backButton.onClick.AddListener(OnClickBackButton);
            }

            if (_defaultButton is not null)
            {
                _defaultButton.onClick.RemoveAllListeners();
                _defaultButton.onClick.AddListener(OnClickDefaultButton);
            }

            if (_applyButton is not null)
            {
                _applyButton.onClick.RemoveAllListeners();
                _applyButton.onClick.AddListener(OnClickApplyButton);
            }
        }

        private void InitUI()
        {
            if (_entityWarningText is not null)
                _entityWarningText.text = $"Cannot set to 0 or mor than {SettingManager.Instance.DefaultStartEntity.ToString()}";
        }

        public override void Open()
        {
            base.Open();
            
            UpdateUI(SettingManager.Instance.GetCurrentGameplaySetting());
            
            _entityUI.SetActive(true);
            _statUI.SetActive(false);
            _growthUI.SetActive(false);
            _spawnUI.SetActive(false);
        }

        private void OnClickBackButton()
        {
            // TODO: Check if there is a change in setting
            // If so, will ask to apply the value or not.
            
            NavigatorManager.Instance.MainMenuController.Open();
            Close();
        }

        private void OnClickDefaultButton()
        {
            
        }

        private void OnClickApplyButton()
        {
            
        }

        private void UpdateUI(PersistenceGameSetting setting)
        {
            
        }
        
        #endregion
    }
}
