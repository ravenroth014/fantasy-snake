using FS_Runtimes.Controllers.Core;
using FS_Runtimes.Models.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FS_Runtimes.Controllers.UI
{
    public class SettingMenuUIController : AbstractUIController
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
        [SerializeField, Tooltip("Discard Popup Setting Button")] private Button _discardPopupButton;
        [SerializeField, Tooltip("Apply Popup Setting Button")] private Button _applyPopupButton;

        [Header("Setting Option UI")] 
        [SerializeField, Tooltip("Entity Option UI")] private GameObject _entityUI;
        [SerializeField, Tooltip("Stat Option UI")] private GameObject _statUI;
        [SerializeField, Tooltip("Growth Option UI")] private GameObject _growthUI;
        [SerializeField, Tooltip("Spawn Option UI")] private GameObject _spawnUI;
        [SerializeField, Tooltip("Back Popup UI")] private GameObject _backPopupUI;

        [Header("Entity Option UI")] 
        [SerializeField, Tooltip("Entity Warning Text")] private TextMeshProUGUI _entityWarningText;
        [SerializeField, Tooltip("Entity InputField")] private TMP_InputField _entityInputField;

        [Header("Stat Option UI")] 
        [SerializeField, Tooltip("Stat Min Atk InputField")] private TMP_InputField _statMinAtkInputField;
        [SerializeField, Tooltip("Stat Max Atk InputField")] private TMP_InputField _statMaxAtkInputField;
        [SerializeField, Tooltip("Stat Min HP InputField")] private TMP_InputField _statMinHpInputField;
        [SerializeField, Tooltip("Stat Min HP InputField")] private TMP_InputField _statMaxHpInputField;

        [Header("Growth Option UI")] 
        [SerializeField, Tooltip("Growing by Count InputField")] private TMP_InputField _growByCountInputField;
        [SerializeField, Tooltip("Stat Growing rate")] private TMP_InputField _statGrowingRateInputField;

        [Header("Spawn Option UI")] 
        [SerializeField, Tooltip("Spawn Max Entity InputField")] private TMP_InputField _spawnMaxEntityInputField;
        [SerializeField, Tooltip("Spawn Max Spawnable")] private TMP_InputField _spawnMaxSpawnableField;

        private SettingManager _settingManager;
        private PersistenceGameSetting _cacheSetting;
        
        #endregion

        #region Methods

        #region Init Methods
        
        public void Init()
        {
            InitButtonCallback();
            InitInputFieldCallback();
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

            if (_applyPopupButton is not null)
            {
                _applyPopupButton.onClick.RemoveAllListeners();
                _applyPopupButton.onClick.AddListener(OnClickApplySettingBackPopupButton);
            }

            if (_discardPopupButton is not null)
            {
                _discardPopupButton.onClick.RemoveAllListeners();
                _discardPopupButton.onClick.AddListener(OnClickDiscardSettingBackPopupButton);
            }
        }

        private void InitInputFieldCallback()
        {
            if (_entityInputField is not null)
            {
                _entityInputField.onEndEdit.RemoveAllListeners();
                _entityInputField.onEndEdit.AddListener(OnStartEntityInputUpdate);
            }

            if (_growByCountInputField is not null)
            {
                _growByCountInputField.onEndEdit.RemoveAllListeners();
                _growByCountInputField.onEndEdit.AddListener(OnGrowByCountInputUpdate);
            }

            if (_statMinAtkInputField is not null)
            {
                _statMinAtkInputField.onEndEdit.RemoveAllListeners();
                _statMinAtkInputField.onEndEdit.AddListener(OnMinAtkInputUpdate);
            }

            if (_statMinHpInputField is not null)
            {
                _statMinHpInputField.onEndEdit.RemoveAllListeners();
                _statMinHpInputField.onEndEdit.AddListener(OnMinHpInputUpdate);
            }

            if (_statMaxAtkInputField is not null)
            {
                _statMaxAtkInputField.onEndEdit.RemoveAllListeners();
                _statMaxAtkInputField.onEndEdit.AddListener(OnMaxAtkInputUpdate);
            }

            if (_statMaxHpInputField is not null)
            {
                _statMaxHpInputField.onEndEdit.RemoveAllListeners();
                _statMaxHpInputField.onEndEdit.AddListener(OnMaxHpInputUpdate);
            }

            if (_spawnMaxEntityInputField is not null)
            {
                _spawnMaxEntityInputField.onEndEdit.RemoveAllListeners();
                _spawnMaxEntityInputField.onEndEdit.AddListener(OnMaxEntityInputUpdate);
            }

            if (_spawnMaxSpawnableField is not null)
            {
                _spawnMaxSpawnableField.onEndEdit.RemoveAllListeners();
                _spawnMaxSpawnableField.onEndEdit.AddListener(OnMaxSpawnableInputUpdate);
            }

            if (_statGrowingRateInputField is not null)
            {
                _statGrowingRateInputField.onEndEdit.RemoveAllListeners();
                _statGrowingRateInputField.onEndEdit.AddListener(OnGrowingStatRateInputUpdate);
            }
        }
        
        private void InitUI()
        {
            if (_entityWarningText is not null)
                _entityWarningText.text = $"Cannot set to 0 or more than {SettingManager.Instance.DefaultStartEntity.ToString()}";
        }
        
        #endregion

        #region Override Methods
        
        public override void Open()
        {
            base.Open();
            
            _cacheSetting = new PersistenceGameSetting(SettingManager.Instance.GetCurrentGameplaySetting());
            UpdateUI(SettingManager.Instance.GetCurrentGameplaySetting());
            
            _entityUI.SetActive(true);
            _statUI.SetActive(false);
            _growthUI.SetActive(false);
            _spawnUI.SetActive(false);
        }
        
        #endregion

        #region Callback Methods
        
        private void OnClickBackButton()
        {
            if (IsSettingChanged())
            {
                _backPopupUI.SetActive(true);
            }
            else
            {
                NavigatorManager.Instance.MainMenuUIController.Open();
                Close();
            }
        }

        private void OnClickDefaultButton()
        {
            UpdateUI(SettingManager.Instance.GetDefaultGameplaySetting());
        }

        private void OnClickApplyButton()
        {
            int startEntity = int.Parse(_entityInputField.text);
            int minAtk = int.Parse(_statMinAtkInputField.text);
            int maxAtk = int.Parse(_statMaxAtkInputField.text);
            int minHp = int.Parse(_statMinHpInputField.text);
            int maxHp = int.Parse(_statMaxHpInputField.text);
            int growthByMove = int.Parse(_growByCountInputField.text);
            float growRate = float.Parse(_statGrowingRateInputField.text);
            int maxActiveEntity = int.Parse(_spawnMaxEntityInputField.text);
            int maxSpawnable = int.Parse(_spawnMaxSpawnableField.text);

            PersistenceGameSetting newSetting = new PersistenceGameSetting(startEntity, minAtk, maxAtk, minHp, maxHp, growthByMove, growRate, maxActiveEntity, maxSpawnable);
            SettingManager.Instance.UpdateCustomSetting(newSetting);
            _cacheSetting = newSetting;
        }

        private void OnClickDiscardSettingBackPopupButton()
        {
            _backPopupUI.SetActive(false);
            NavigatorManager.Instance.MainMenuUIController.Open();
            Close();
        }

        private void OnClickApplySettingBackPopupButton()
        {
            OnClickApplyButton();
            _backPopupUI.SetActive(false);
            NavigatorManager.Instance.MainMenuUIController.Open();
            Close();
        }
        
        private void OnStartEntityInputUpdate(string value)
        {
            bool isParsed = int.TryParse(value, out int result);
            int defaultValue = SettingManager.Instance.DefaultStartEntity;

            if (isParsed == false)
            {
                _entityInputField.text = defaultValue.ToString("D");
                return;
            }

            if (result <= 0 || result > defaultValue)
                _entityInputField.text = defaultValue.ToString("D");
        }

        private void OnGrowByCountInputUpdate(string value)
        {
            bool isParsed = int.TryParse(value, out int result);
            int defaultValue = SettingManager.Instance.MinGrowthMoveCount;

            if (isParsed == false)
            {
                _growByCountInputField.text = defaultValue.ToString("D");
                return;
            }

            if (result <= 0)
                _growByCountInputField.text = defaultValue.ToString("D");
        }

        private void OnMinAtkInputUpdate(string value)
        {
            int maxResult = int.MaxValue;
            bool isMinParsed = int.TryParse(value, out int minResult);
            bool isMaxParsed = _statMaxAtkInputField is not null && int.TryParse(_statMaxAtkInputField.text, out maxResult);
            
            int defaultValue = 1;

            if (isMinParsed == false)
            {
                _statMinAtkInputField.text = defaultValue.ToString("D");
                return;
            }

            if (minResult <= 0)
            {
                _statMinAtkInputField.text = defaultValue.ToString("D");
                return;
            }
            
            if (isMaxParsed == false)
                return;

            if (minResult > maxResult)
            {
                _statMinAtkInputField.text = (maxResult - 1).ToString("D");
            }
        }

        private void OnMinHpInputUpdate(string value)
        {
            int maxResult = int.MaxValue;
            bool isMinParsed = int.TryParse(value, out int minResult);
            bool isMaxParsed = _statMaxHpInputField is not null && int.TryParse(_statMaxHpInputField.text, out maxResult);
            
            int defaultValue = 1;

            if (isMinParsed == false)
            {
                _statMinHpInputField.text = defaultValue.ToString("D");
                return;
            }

            if (minResult <= 0)
            {
                _statMinHpInputField.text = defaultValue.ToString("D");
                return;
            }
            
            if (isMaxParsed == false)
                return;

            if (minResult > maxResult)
            {
                _statMinHpInputField.text = (maxResult - 1).ToString("D");
            }
        }

        private void OnMaxAtkInputUpdate(string value)
        {
            int minResult = 1;
            bool isMaxParsed = int.TryParse(value, out int maxResult);
            bool isMinParsed = _statMinAtkInputField is not null && int.TryParse(_statMinAtkInputField.text, out minResult);

            int defaultValue = SettingManager.Instance.DefaultMaxAtkStat;

            if (isMaxParsed == false)
            {
                _statMaxAtkInputField.text = defaultValue.ToString("D");
                return;
            }
            
            if (isMinParsed == false)
                return;

            if (maxResult < minResult)
            {
                _statMaxAtkInputField.text = (minResult + 1).ToString("D");
            }
        }

        private void OnMaxHpInputUpdate(string value)
        {
            int minResult = 1;
            bool isMaxParsed = int.TryParse(value, out int maxResult);
            bool isMinParsed = _statMinHpInputField is not null && int.TryParse(_statMinHpInputField.text, out minResult);

            int defaultValue = SettingManager.Instance.DefaultMaxHpStat;

            if (isMaxParsed == false)
            {
                _statMaxHpInputField.text = defaultValue.ToString("D");
                return;
            }
            
            if (isMinParsed == false)
                return;

            if (maxResult < minResult)
            {
                _statMaxHpInputField.text = (minResult + 1).ToString("D");
            }
        }

        private void OnMaxEntityInputUpdate(string value)
        {
            bool isParsed = int.TryParse(value, out int result);

            int defaultValue = 1;

            if (isParsed == false)
            {
                _spawnMaxEntityInputField.text = defaultValue.ToString("D");
                return;
            }

            if (result <= 0)
            {
                _spawnMaxEntityInputField.text = defaultValue.ToString("D");
            }
        }

        private void OnMaxSpawnableInputUpdate(string value)
        {
            bool isParsed = int.TryParse(value, out int result);

            int defaultValue = 1;

            if (isParsed == false)
            {
                _spawnMaxSpawnableField.text = defaultValue.ToString("D");
                return;
            }

            if (result <= 0)
            {
                _spawnMaxSpawnableField.text = defaultValue.ToString("D");
            }
        }

        private void OnGrowingStatRateInputUpdate(string value)
        {
            bool isParsed = float.TryParse(value, out float result);

            float defaultValue = SettingManager.Instance.DefaultGrowingRate;

            if (isParsed == false)
            {
                _spawnMaxSpawnableField.text = defaultValue.ToString("F1");
                return;
            }

            if (result <= 0)
            {
                _spawnMaxSpawnableField.text = defaultValue.ToString("F1");
            }
        }
        
        #endregion

        #region UI Methods

        private void UpdateUI(PersistenceGameSetting setting)
        {
            if (_entityInputField is not null)
                _entityInputField.text = setting.StartEntity.ToString("D");

            if (_statMinAtkInputField is not null)
                _statMinAtkInputField.text = setting.MinAttack.ToString("D");

            if (_statMaxAtkInputField is not null)
                _statMaxAtkInputField.text = setting.MaxAttack.ToString("D");

            if (_statMinHpInputField is not null)
                _statMinHpInputField.text = setting.MinHealth.ToString("D");

            if (_statMaxHpInputField is not null)
                _statMaxHpInputField.text = setting.MaxHealth.ToString("D");

            if (_growByCountInputField is not null)
                _growByCountInputField.text = setting.GrowthWithMove.ToString("D");

            if (_spawnMaxEntityInputField is not null)
                _spawnMaxEntityInputField.text = setting.MaxActiveEntity.ToString("D");

            if (_spawnMaxSpawnableField is not null)
                _spawnMaxSpawnableField.text = setting.MaxSpawnable.ToString("D");

            if (_statGrowingRateInputField is not null)
                _statGrowingRateInputField.text = setting.StatGrowthRate.ToString("F1");
        }
        
        #endregion

        #region Utility Methods

        private bool IsSettingChanged()
        {
            bool isDataChanged = false;
            isDataChanged |= int.Parse(_entityInputField.text) != _cacheSetting.StartEntity;
            isDataChanged |= int.Parse(_statMinAtkInputField.text) != _cacheSetting.MinAttack;
            isDataChanged |= int.Parse(_statMaxAtkInputField.text) != _cacheSetting.MaxAttack;
            isDataChanged |= int.Parse(_statMinHpInputField.text) != _cacheSetting.MinHealth;
            isDataChanged |= int.Parse(_statMaxHpInputField.text) != _cacheSetting.MaxHealth;
            isDataChanged |= int.Parse(_growByCountInputField.text) != _cacheSetting.GrowthWithMove;
            isDataChanged |= int.Parse(_spawnMaxEntityInputField.text) != _cacheSetting.MaxActiveEntity;
            isDataChanged |= int.Parse(_spawnMaxSpawnableField.text) != _cacheSetting.MaxSpawnable;
            
            return isDataChanged;
        }

        #endregion
        
        #endregion
    }
}