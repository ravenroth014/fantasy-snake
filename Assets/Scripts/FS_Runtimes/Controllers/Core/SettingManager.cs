using System.Collections.Generic;
using System.Linq;
using FS_Runtimes.Models.Settings;
using FS_Runtimes.Utilities;
using FS_Runtimes.Utilities.Setting;
using Newtonsoft.Json;
using UnityEngine;

namespace FS_Runtimes.Controllers.Core
{
    public class SettingManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Game Setting")]
        [SerializeField, Tooltip("Gameplay Button Setting")] private List<GameplayButtonSetting> _gameplayButtonSettingList;
        [SerializeField, Tooltip("Gameplay Default Growth Setting")] private GrowthSetting _defaultGrowthSetting;
        [SerializeField, Tooltip("Gameplay Default Entity Setting")] private EntitySetting _defaultEntitySetting;
        [SerializeField, Tooltip("Gameplay Default Stat Setting")] private StatSetting _defaultStatSetting;

        public int DefaultStartEntity => _defaultEntitySetting.DefaultStartEntity;
        
        private PersistenceGameSetting _defaultSetting;
        private PersistenceGameSetting _customSetting;
        
        public static SettingManager Instance => _instance;
        private static SettingManager _instance;

        #endregion

        #region Methods

        private void Awake()
        {
            _instance = this;
            
            InitSetting();
        }

        private void InitSetting()
        {
            _defaultSetting = new PersistenceGameSetting(_defaultEntitySetting.DefaultStartEntity
                , _defaultStatSetting.DefaultMinAttack
                , _defaultStatSetting.DefaultMaxAttack
                , _defaultStatSetting.DefaultMinHealth
                , _defaultStatSetting.DefaultMaxHealth
                , _defaultGrowthSetting.DefaultGrowthMoveCount
                , _defaultGrowthSetting.MinGrowthMoveCount
                , _defaultEntitySetting.DefaultMaxActiveEntity
                , _defaultEntitySetting.DefaultMaxSpawnable);
            
            if (PlayerPrefs.HasKey(GameHelper.GameSettingKey) == false) return;

            string customSettingJson = PlayerPrefs.GetString(GameHelper.GameSettingKey);
            _customSetting = JsonConvert.DeserializeObject<PersistenceGameSetting>(customSettingJson);
        }

        public PersistenceGameSetting GetCurrentGameplaySetting()
        {
            return _customSetting ?? _defaultSetting;
        }

        public PersistenceGameSetting GetDefaultGameplaySetting()
        {
            return _defaultSetting;
        }

        public void UpdateCustomSetting(PersistenceGameSetting newSetting)
        {
            _customSetting = newSetting;

            string customSettingJson = string.Empty;
            
            if (newSetting != null)
                customSettingJson = JsonConvert.SerializeObject(_customSetting);
            
            PlayerPrefs.SetString(GameHelper.GameSettingKey, customSettingJson);
            PlayerPrefs.Save();
        }

        public bool IsGameplayButtonActionValid(EPlayerAction lastAction, EPlayerAction currentAction)
        {
            if (_gameplayButtonSettingList is null or { Count: 0 })
                return false;

            GameplayButtonSetting gameSetting = _gameplayButtonSettingList.Find(setting => setting.Action == lastAction);
            
            if (gameSetting == null) return false;
            if (gameSetting.UnavailableActions is { Count: > 0 } && gameSetting.UnavailableActions.Any(action => action == currentAction)) return false;

            return true;
        }

        #endregion
    }
}
