using System.Text;
using FS_Runtimes.Controllers.Character;
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
        [SerializeField, Tooltip("Character Type")] private ECharacterType _characterType;
        
        private IObjectPool<CharacterGameObject> _pool;
        private int _currentID;
        private StringBuilder _stringBuilder;
        
        #endregion

        #region Methods

        #region Init Methods
        
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
        
        private CharacterGameObject CreatePooledItem()
        {
            if (_poolingSetting.ObjectInPool is null) return null;
            if (_poolingSetting.ObjectInPool.HasComponent<CharacterGameObject>() == false) return null;

            CharacterGameObject item = Instantiate(_poolingSetting.ObjectInPool).GetComponent<CharacterGameObject>();

            _stringBuilder.Clear();
            _stringBuilder.Append(_characterType.ToString());
            _stringBuilder.Append(_currentID.ToString("0000"));
            
            item.InitOnCreate(this, _stringBuilder.ToString());

            return item;
        }

        private void OnReleaseToPool(CharacterGameObject item)
        {
            item.gameObject.transform.parent = gameObject.transform;
            item.gameObject.SetActive(false);
        }

        private void OnGetFromPool(CharacterGameObject item)
        {
            item.gameObject.SetActive(true);
        }

        private void OnDestroyPoolObject(CharacterGameObject item)
        {
            Destroy(item.gameObject);
        }

        public void ReturnItemToPool(CharacterGameObject item)
        {
            _pool.Release(item);
        }

        public CharacterGameObject GetFromPool()
        {
            return _pool.Get();
        }
        
        #endregion
        
        #endregion
    }
}
