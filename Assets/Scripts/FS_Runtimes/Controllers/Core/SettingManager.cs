using System.Collections.Generic;
using System.Linq;
using FS_Runtimes.Utilities;
using UnityEngine;

namespace FS_Runtimes.Controllers.Core
{
    public class SettingManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Game Setting")]
        [SerializeField, Tooltip("Gameplay Button Setting")] private List<GameplayButtonSetting> _gameplayButtonSettingList;
        
        public static SettingManager Instance => _instance;
        private static SettingManager _instance;

        #endregion

        #region Methods

        private void Awake()
        {
            _instance = this;
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
