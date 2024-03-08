using UnityEngine;

namespace FS_Runtimes.Utilities
{
    [CreateAssetMenu(fileName = "PoolingSetting", menuName = "Settings/Pooling Setting")]
    public class PoolingSetting : ScriptableObject
    {
        #region Fields & Properties
        
        [Header("Pooling Setting")]
        [SerializeField, Tooltip("Pool Type")] private EPoolingType _poolingType;
        [SerializeField, Tooltip("Max Pool Size")] private int _maxPoolSize = 30;

        public EPoolingType PoolingType => _poolingType;
        public int MaxPoolSize => _maxPoolSize;
        
        #endregion
    }
}
