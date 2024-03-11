using System.Collections.Generic;
using UnityEngine;

namespace FS_Runtimes.Utilities.Setting
{
    [CreateAssetMenu(fileName = "GameplayButtonSetting", menuName = "Settings/Gameplay Button Setting")]
    public class GameplayButtonSetting : ScriptableObject
    {
        #region Fields & Properties

        [SerializeField, Tooltip("Action Key")] private EPlayerAction _action;
        [SerializeField, Tooltip("Action unavailable")] private List<EPlayerAction> _unavailableActions;

        public EPlayerAction Action => _action;
        public IReadOnlyList<EPlayerAction> UnavailableActions => _unavailableActions;

        #endregion
    }
}
