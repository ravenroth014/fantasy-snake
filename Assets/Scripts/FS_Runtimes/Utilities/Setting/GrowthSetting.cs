using UnityEngine;

namespace FS_Runtimes.Utilities.Setting
{
    [CreateAssetMenu(fileName = "GrowthSetting", menuName = "Settings/Growth Setting")]
    public class GrowthSetting : ScriptableObject
    {
        #region Fields & Properties

        [SerializeField, Tooltip("Minimum Growth Move Count")] private int _minGrowthMoveCount = 1;
        [SerializeField, Tooltip("Default Growth Move Count")] private int _defaultGrowthMoveCount = 4;

        public int MinGrowthMoveCount => _minGrowthMoveCount;
        public int DefaultGrowthMoveCount => _defaultGrowthMoveCount;

        #endregion
    }
}
