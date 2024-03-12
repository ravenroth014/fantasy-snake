using System.Text;
using FS_Runtimes.Controllers.Character;
using FS_Runtimes.Utilities;
using FS_Runtimes.Utilities.Setting;
using UnityEngine;
using UnityEngine.Pool;

namespace FS_Runtimes.Controllers.Pooling
{
    public class CharacterPooling : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("Setting")]
        [SerializeField, Tooltip("Pooling Setting")] private PoolingSetting _poolingSetting;
        [SerializeField, Tooltip("Character Type")] private ECharacterType _characterType;
        
        private IObjectPool<CharacterGameObject> _pool;
        private int _currentID;
        private StringBuilder _stringBuilder;
        
        #endregion

        #region Methods

        #region Init Methods
        
        /// <summary>
        /// Call this method to initialize pooling.
        /// </summary>
        public void Init()
        {
            if (_pool != null) return;

            if (_poolingSetting.PoolingType == EPoolingType.Stack)
                _pool = new ObjectPool<CharacterGameObject>(CreatePooledItem, OnGetFromPool, OnReleaseToPool, OnDestroyPoolObject, _poolingSetting.CollectionCheck, maxSize: _poolingSetting.PoolMaxSize);
            else if (_poolingSetting.PoolingType == EPoolingType.LinkedList)
                _pool = new LinkedPool<CharacterGameObject>(CreatePooledItem, OnGetFromPool, OnReleaseToPool, OnDestroyPoolObject, _poolingSetting.CollectionCheck, _poolingSetting.PoolMaxSize);

            _currentID = 0;
            _stringBuilder = new StringBuilder();
        }
        
        #endregion

        #region Pooling Methods
        
        /// <summary>
        /// Callback method when first time creating item from this pool.
        /// </summary>
        /// <returns></returns>
        private CharacterGameObject CreatePooledItem()
        {
            if (_poolingSetting.ObjectInPool is null or {Count: 0}) return null;
            if (_poolingSetting.ObjectInPool[0].HasComponent<CharacterGameObject>() == false) return null;

            CharacterGameObject item = Instantiate(_poolingSetting.ObjectInPool[0]).GetComponent<CharacterGameObject>();

            _stringBuilder.Clear();
            _stringBuilder.Append(_characterType.ToString());
            _stringBuilder.Append(_currentID.ToString("0000"));
            _currentID++;
            
            item.InitOnCreate(this, _stringBuilder.ToString());
            item.SetParent(gameObject.transform);

            return item;
        }

        /// <summary>
        /// Callback method when item is returned into pool.
        /// </summary>
        /// <param name="item"></param>
        private void OnReleaseToPool(CharacterGameObject item)
        {
            item.SetParent(gameObject.transform);
            item.gameObject.SetActive(false);
        }

        /// <summary>
        /// Callback method when item is retrieved from this pool.
        /// </summary>
        /// <param name="item"></param>
        private void OnGetFromPool(CharacterGameObject item)
        {
            item.gameObject.SetActive(true);
        }

        /// <summary>
        /// Callback method when item is designated to be destroyed from this pool.
        /// </summary>
        /// <param name="item"></param>
        private void OnDestroyPoolObject(CharacterGameObject item)
        {
            Destroy(item.gameObject);
        }

        /// <summary>
        /// Call this method to return item to this pool.
        /// </summary>
        /// <param name="item"></param>
        public void ReturnItemToPool(CharacterGameObject item)
        {
            _pool.Release(item);
        }

        /// <summary>
        /// Call this method to get item from this pool.
        /// </summary>
        /// <returns></returns>
        public CharacterGameObject GetFromPool()
        {
            return _pool.Get();
        }
        
        #endregion
        
        #endregion
    }
}
