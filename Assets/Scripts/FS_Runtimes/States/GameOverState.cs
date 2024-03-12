using FS_Runtimes.Controllers.Character;
using FS_Runtimes.Controllers.Core;
using FS_Runtimes.Controllers.Level;
using FS_Runtimes.Controllers.UI;
using FS_Runtimes.Controllers.Utilities;
using FS_Runtimes.Utilities;

namespace FS_Runtimes.States
{
    public class GameOverState : GameState
    {
        #region Fields & Properties

        private readonly LevelManager _levelManager = GameManager.Instance.LevelManager;
        private readonly CharactersManager _charactersManager = GameManager.Instance.CharactersManager;

        private readonly GameOverUIController _gameOverUIController = NavigatorManager.Instance.GameOverUIController;

        private readonly LogManager _logManager = LogManager.Instance;
        
        #endregion

        #region Methods

        #region Derived Methods
        
        public override void OnEnter()
        {
            _logManager.Log("Enter game over state ...");
            
            if (Init() == false)
                GameManager.Instance.ChangeState(EGameState.GameError);
            
            _gameOverUIController.Open();
        }

        public override void OnExit()
        {
            _gameOverUIController.Close();
            _charactersManager.ResetManager();
            _levelManager.ResetManager();
            
            _logManager.Log("Exit game over state ...");
        }
        
        #endregion

        #region Init Methods
        
        /// <summary>
        /// Call this method to initialize this game state.
        /// </summary>
        /// <returns></returns>
        private bool Init()
        {
            if (_gameOverUIController is null)
                return false;
            if (_levelManager is null)
                return false;
            
            _gameOverUIController.Init();
            _gameOverUIController.SetScore(_levelManager.TotalKillEnemies);
            
            return true;
        }
        
        #endregion
        
        #endregion
    }
}