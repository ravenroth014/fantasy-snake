using UnityEngine;
using UnityEngine.Serialization;

namespace FS_Runtimes.Utilities
{
    [CreateAssetMenu(fileName = "PoolingSetting", menuName = "Settings/Pooling Setting")]
    public class PoolingSetting : ScriptableObject
    {
        #region Fields & Properties
        
        [Header("Pooling Setting")]
        [SerializeField, Tooltip("Pool Type")] private EPoolingType _poolingType;
        [FormerlySerializedAs("_maxPoolSize")] [SerializeField, Tooltip("Max Pool Size")] private int _poolMaxSize = 30;
        [SerializeField, Tooltip("Game object for pooling")] private GameObject _objectInPool;
        [SerializeField, Tooltip("Collection Check")] private bool _collectionCheck = true;

        public EPoolingType PoolingType => _poolingType;
        public int PoolMaxSize => _poolMaxSize;
        public GameObject ObjectInPool => _objectInPool;
        public bool CollectionCheck => _collectionCheck;

        #endregion
    }
}
