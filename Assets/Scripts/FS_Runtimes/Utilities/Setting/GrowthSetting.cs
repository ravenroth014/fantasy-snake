using UnityEngine;
using UnityEngine.Serialization;

namespace FS_Runtimes.Utilities.Setting
{
    [CreateAssetMenu(fileName = "GrowthSetting", menuName = "Settings/Growth Setting")]
    public class GrowthSetting : ScriptableObject
    {
        #region Fields & Properties

        [FormerlySerializedAs("_minGrowthMoveCount")] [SerializeField, Tooltip("Minimum Move Count To Level up")] private int _minMoveCountToLevelUp = 1;
        [SerializeField, Tooltip("Default Move Count To Level up")] private int _defaultMoveCountLevelUp = 4;
        [FormerlySerializedAs("_defaultGrowthRate")] [SerializeField, Tooltip("Default Growth Rate Stat")] private float _defaultGrowthRateStat = 0.2f;
        [SerializeField, Tooltip("Default Growth Rate Move Count")] private float _defaultGrowthRateMove = 0.5f;

        public int MinMoveCountToLevelUp => _minMoveCountToLevelUp;
        public int DefaultMoveCountLevelUp => _defaultMoveCountLevelUp;
        public float DefaultGrowthRateStat => _defaultGrowthRateStat;
        public float DefaultGrowthRateMove => _defaultGrowthRateMove;

        #endregion
    }
}
