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

        public void InitOnCreate(DecoratePooling pooling, EDecorateType decorateType)
        {
            _pooling = pooling;
            _decorateType = decorateType;
        }
        
        public void SetDecoratePosition(Vector2 position)
        {
            float height = transform.position.y;
            gameObject.transform.position = new Vector3(position.x, height, position.y);
        }
        
        public void Release()
        {
            if (_pooling is null)
                Destroy(gameObject);
            else
            {
                _pooling.ReturnItemToPool(this);
            }
        }

        #endregion
    }
}
