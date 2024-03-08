using FS_Runtimes.Controllers.Core;

namespace FS_Runtimes.States
{
    public abstract class GameState 
    {
        #region Fields & Properties

        protected GameplayManager _gameplayManager;

        #endregion

        #region Constructor

        protected GameState()
        {
            if (GameplayManager.Instance is not null)
                _gameplayManager = GameplayManager.Instance;
        }

        #endregion

        #region Methods

        #region Abstract Methods

        public abstract void OnEnter();

        public abstract void OnExit();

        #endregion

        #endregion
    }
}
