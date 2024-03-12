using FS_Runtimes.Controllers.Pooling;
using FS_Runtimes.Utilities;
using UnityEngine;

namespace FS_Runtimes.Controllers.Decorate
{
    public class DecorateGameObject : MonoBehaviour
    {
        #region Fields & Properties

        private bool _isInitOnCreate;
        private DecoratePooling _pooling;
        private EDecorateType _decorateType;

        #endregion

        #region Methods

        /// <summary>
        /// Call this method to initialize when it's created from object pool.
        /// </summary>
        /// <param name="pooling"></param>
        /// <param name="decorateType"></param>
        public void InitOnCreate(DecoratePooling pooling, EDecorateType decorateType)
        {
            _pooling = pooling;
            _decorateType = decorateType;
        }
        
        /// <summary>
        /// Call this method to set item's position.
        /// </summary>
        /// <param name="position"></param>
        public void SetDecoratePosition(Vector2 position)
        {
            float height = transform.position.y;
            gameObject.transform.position = new Vector3(position.x, height, position.y);
        }
        
        /// <summary>
        /// Call this method to return item to its pool.
        /// </summary>
        public void Release()
        {
            if (_pooling is null)
                Destroy(gameObject);
            else
            {
                _pooling.ReturnItemToPool(this);
            }
        }

        /// <summary>
        /// Call this method to set its parent.
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(Transform parent)
        {
            gameObject.transform.parent = parent;
        }

        #endregion
    }
}
