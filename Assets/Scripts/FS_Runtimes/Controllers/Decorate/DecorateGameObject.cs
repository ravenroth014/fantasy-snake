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
            
        }

        #endregion
    }
}
