using UnityEngine;

namespace FS_Runtimes.Utilities
{
    [CreateAssetMenu(fileName = "ObstacleSetting", menuName = "Settings/Obstacle Setting")]
    public class ObstacleSetting : ScriptableObject
    {
        #region Fields & Properties

        [SerializeField, Tooltip("Total obstacle")] private int _totalObstacle = 12;
        [SerializeField, Tooltip("Max width size")] private int _maxWidthSize = 2;
        [SerializeField, Tooltip("Max height size")] private int _maxHeightSize = 2;

        public int TotalObstacle => _totalObstacle;
        public int MaxWidthSize => _maxWidthSize;
        public int MaxHeightSize => _maxHeightSize;

        #endregion
    }
}
