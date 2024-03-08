using FS_Runtimes.Controllers.Player;
using FS_Runtimes.Utilities;
using UnityEngine;
using UnityEngine.Pool;

namespace FS_Runtimes.Controllers.Pooling
{
    public class CharacterPooling : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("Setting")]
        [SerializeField, Tooltip("Pooling Setting")] private PoolingSetting _poolingSetting;
        
        private IObjectPool<CharacterModule> _pool;
        
        #endregion

        #region Methods

        #region Unity Event Methods
        
        private void Start()
        {
            Init();
        }
        
        #endregion

        #region Init Methods
        
        private void Init()
        {
            if (_pool != null) return;

            if (_poolingSetting.PoolingType == EPoolingType.Stack)
                _pool = new ObjectPool<CharacterModule>(CreatePooledItem, OnGetFromPool, OnReleaseToPool, OnDestroyPoolObject, _poolingSetting.CollectionCheck, maxSize: _poolingSetting.PoolMaxSize);
            else if (_poolingSetting.PoolingType == EPoolingType.LinkedList)
                _pool = new LinkedPool<CharacterModule>(CreatePooledItem, OnGetFromPool, OnReleaseToPool, OnDestroyPoolObject, _poolingSetting.CollectionCheck, _poolingSetting.PoolMaxSize);
        }
        
        #endregion

        #region Pooling Methods
        
        private CharacterModule CreatePooledItem()
        {
            if (_poolingSetting.ObjectInPool is null) return null;
            if (_poolingSetting.ObjectInPool.HasComponent<CharacterModule>() == false) return null;

            CharacterModule item = Instantiate(_poolingSetting.ObjectInPool).GetComponent<CharacterModule>();

            return item;
        }

        private void OnReleaseToPool(CharacterModule item)
        {
            item.gameObject.SetActive(false);
        }

        private void OnGetFromPool(CharacterModule item)
        {
            item.gameObject.SetActive(true);
        }

        private void OnDestroyPoolObject(CharacterModule item)
        {
            Destroy(item.gameObject);
        }

        public void ReturnItemToPool(CharacterModule item)
        {
            _pool.Release(item);
        }
        
        #endregion
        
        #endregion
    }
}
