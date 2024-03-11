using UnityEngine;

namespace FS_Runtimes.Utilities
{
    [CreateAssetMenu(fileName = "LevelObjectiveSetting", menuName = "Settings/Level Objective Setting")]
    public class LevelObjectiveSetting : ScriptableObject
    {
        #region Fields & Properties

        [SerializeField, Tooltip("Base target")] private int _baseTarget = 5;
        [SerializeField, Tooltip("Level exponential")] private float _levelExponential = 1.5f;

        public int BaseTarget => _baseTarget;
        public float LevelExponential => _levelExponential;

        #endregion
    }
}
