using FS_Runtimes.Models.Levels;
using UnityEngine;

namespace FS_Runtimes.Controllers.Level
{
    public class LevelManager : MonoBehaviour
    {
        #region Fields & Properties

        private LevelModelManager _modelManager;

        #endregion

        #region Methods

        public void Init()
        {
            _modelManager = new LevelModelManager();
        }

        #endregion
    }
}
