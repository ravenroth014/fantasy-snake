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

        #endregion

        #region Methods

        public void Init()
        {
        
        }

        #endregion
    }
}
