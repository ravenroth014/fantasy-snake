using FS_Runtimes.Controllers.Character;
using FS_Runtimes.Controllers.Core;
using FS_Runtimes.Controllers.Level;
using FS_Runtimes.Controllers.UI;
using FS_Runtimes.Utilities;

namespace FS_Runtimes.States
{
    public class GameOverState : GameState
    {
        #region Fields & Properties

        private readonly LevelManager _levelManager = GameManager.Instance.LevelManager;
        private readonly CharactersManager _charactersManager = GameManager.Instance.CharactersManager;

        private readonly LoadingUIController _loadingUIController = NavigatorManager.Instance.LoadingUIController;
        private readonly GameOverUIController _gameOverUIController = NavigatorManager.Instance.GameOverUIController;

        #endregion

        #region Methods

        public override void OnEnter()
        {
            if (Init() == false)
                GameManager.Instance.ChangeState(EGameState.GameError);
            
            _gameOverUIController.Open();
        }

        public override void OnExit()
        {
            _gameOverUIController.Close();
            _charactersManager.ResetManager();
            _levelManager.ResetManager();
        }

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
    }
}