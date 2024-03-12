using System.Linq;
using FS_Runtimes.Controllers.Decorate;
using FS_Runtimes.Utilities;
using FS_Runtimes.Utilities.Setting;
using UnityEngine;
using UnityEngine.Pool;

namespace FS_Runtimes.Controllers.Pooling
{
    public class DecoratePooling : MonoBehaviour
    {
        #region Fields & Properties

        [SerializeField, Tooltip("Pooling Setting")] private PoolingSetting _poolingSetting;
        [SerializeField, Tooltip("Decorate Type")] private EDecorateType _decorateType;

        private IObjectPool<DecorateGameObject> _pool;

        #endregion

        #region Methods

        #region Init Methods

        /// <summary>
        /// Call this method to initialize this pool
        /// </summary>
        public void Init()
        {
            if (_pool != null) return;

            if (_poolingSetting.PoolingType == EPoolingType.Stack)
                _pool = new ObjectPool<DecorateGameObject>(CreatePoolItem, OnGetFromPool, OnReleaseToPool, OnDestroyPoolObject, _poolingSetting.CollectionCheck, maxSize: _poolingSetting.PoolMaxSize);
            else if (_poolingSetting.PoolingType == EPoolingType.LinkedList)
                _pool = new LinkedPool<DecorateGameObject>(CreatePoolItem, OnGetFromPool, OnReleaseToPool, OnDestroyPoolObject, _poolingSetting.CollectionCheck, _poolingSetting.PoolMaxSize);
        }
        
        #endregion

        #region Pooling Methods
        
        /// <summary>
        /// Callback method when first time creating item from this pool.
        /// </summary>
        /// <returns></returns>
        private DecorateGameObject CreatePoolItem()
        {
            if (_poolingSetting.ObjectInPool is null or { Count: 0 }) return null;
            if (_poolingSetting.ObjectInPool.Any(decorate => decorate.HasComponent<DecorateGameObject>() == false)) return null;

            int randIndex = Random.Range(0, _poolingSetting.ObjectInPool.Count);
            DecorateGameObject item = Instantiate(_poolingSetting.ObjectInPool[randIndex]).GetComponent<DecorateGameObject>();
            item.InitOnCreate(this, _decorateType);
            item.SetParent(gameObject.transform);
            return item;
        }

        /// <summary>
        /// Callback method when item is returned into pool.
        /// </summary>
        /// <param name="item"></param>
        private void OnReleaseToPool(DecorateGameObject item)
        {
            item.gameObject.transform.parent = gameObject.transform;
            item.gameObject.SetActive(false);
        }

        /// <summary>
        /// Callback method when item is retrieved from this pool.
        /// </summary>
        /// <param name="item"></param>
        private void OnGetFromPool(DecorateGameObject item)
        {
            item.gameObject.SetActive(true);
        }

        /// <summary>
        /// Callback method when item is designated to be destroyed from this pool.
        /// </summary>
        /// <param name="item"></param>
        private void OnDestroyPoolObject(DecorateGameObject item)
        {
            Destroy(item.gameObject);
        }

        /// <summary>
        /// Call this method to return item to this pool.
        /// </summary>
        /// <param name="item"></param>
        public void ReturnItemToPool(DecorateGameObject item)
        {
            _pool.Release(item);
        }

        /// <summary>
        /// Call this method to get item from this pool.
        /// </summary>
        /// <returns></returns>
        public DecorateGameObject GetFromPool()
        {
            return _pool.Get();
        }
        
        #endregion
        
        #endregion
    }
}
