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
