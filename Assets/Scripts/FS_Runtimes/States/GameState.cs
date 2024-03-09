using FS_Runtimes.Controllers.Core;
using FS_Runtimes.Controllers.Gameplay;

namespace FS_Runtimes.States
{
    public abstract class GameState 
    {
        #region Methods

        #region Abstract Methods

        public abstract void OnEnter();

        public abstract void OnExit();

        #endregion

        #endregion
    }
}
