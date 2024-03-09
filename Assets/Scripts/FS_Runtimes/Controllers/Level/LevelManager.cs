using FS_Runtimes.Models.Levels;
using FS_Runtimes.Utilities;
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

        public void ResetManager()
        {
            _modelManager.Reset();
        }

        public Vector2 GetFreePosition()
        {
            return _modelManager.GetFreePosition();
        }

        public void UpdateGridData(Vector2 position, string uniqueID, EGridState gridState, ECharacterType characterType)
        {
            _modelManager.UpdateGridData(position, uniqueID, gridState, characterType);
        }

        #endregion
    }
}
