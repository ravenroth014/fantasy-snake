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
        public int MinGrowthMoveCount => _defaultGrowthSetting.MinMoveCountToLevelUp;
        public int DefaultMaxAtkStat => _defaultStatSetting.DefaultMaxAttack;
        public int DefaultMaxHpStat => _defaultStatSetting.DefaultMaxHealth;
        public float DefaultGrowingRateStat => _defaultGrowthSetting.DefaultGrowthRateStat;
        public float DefaultGrowingRateMove => _defaultGrowthSetting.DefaultGrowthRateMove;
        
        private PersistenceGameSetting _defaultSetting;
        private PersistenceGameSetting _customSetting;
        
        public static SettingManager Instance => _instance;
        private static SettingManager _instance;

        #endregion

        #region Methods

        #region Unit Event Methods
        
        private void Awake()
        {
            _instance = this;
            
            InitSetting();
        }
        
        #endregion

        #region Init Methods
        
        /// <summary>
        /// Call this method to initialize game setting.
        /// </summary>
        private void InitSetting()
        {
            // Set default setting value from scriptable object.
            _defaultSetting = new PersistenceGameSetting(_defaultEntitySetting.DefaultStartEntity
                , _defaultStatSetting.DefaultMinAttack
                , _defaultStatSetting.DefaultMaxAttack
                , _defaultStatSetting.DefaultMinHealth
                , _defaultStatSetting.DefaultMaxHealth
                , _defaultGrowthSetting.DefaultMoveCountLevelUp
                , _defaultGrowthSetting.DefaultGrowthRateStat
                , _defaultGrowthSetting.DefaultGrowthRateMove
                , _defaultEntitySetting.DefaultMaxActiveEntity
                , _defaultEntitySetting.DefaultMaxSpawnable);
            
            if (PlayerPrefs.HasKey(GameHelper.GameSettingKey) == false) return;

            string customSettingJson = PlayerPrefs.GetString(GameHelper.GameSettingKey);
            if (string.IsNullOrEmpty(customSettingJson)) return;
                
            _customSetting = JsonConvert.DeserializeObject<PersistenceGameSetting>(customSettingJson);
        }
        
        #endregion

        #region Get Methods
        
        /// <summary>
        /// Call this method to get current game setting.
        /// </summary>
        /// <returns></returns>
        public PersistenceGameSetting GetCurrentGameplaySetting()
        {
            return _customSetting ?? _defaultSetting;
        }

        /// <summary>
        /// Call this method to get default game setting.
        /// </summary>
        /// <returns></returns>
        public PersistenceGameSetting GetDefaultGameplaySetting()
        {
            return _defaultSetting;
        }
        
        /// <summary>
        /// Call this method to check if the action button is valid to execute or not during gameplay mode.
        /// </summary>
        /// <param name="lastAction"></param>
        /// <param name="currentAction"></param>
        /// <returns></returns>
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

        #region Update Methods
        
        /// <summary>
        /// Call this method to update new custom setting.
        /// </summary>
        /// <param name="newSetting"></param>
        public void UpdateCustomSetting(PersistenceGameSetting newSetting)
        {
            _customSetting = new PersistenceGameSetting(newSetting);

            string customSettingJson = string.Empty;
            
            if (newSetting != null)
                customSettingJson = JsonConvert.SerializeObject(_customSetting);
            
            PlayerPrefs.SetString(GameHelper.GameSettingKey, customSettingJson);
            PlayerPrefs.Save();
        }
        
        #endregion

        #endregion
    }
}
