using FS_Runtimes.Controllers.Core;
using FS_Runtimes.Controllers.Gameplay;
using FS_Runtimes.Controllers.Level;

namespace FS_Runtimes.States
{
    public class GameplayState : GameState
    {
        #region Fields & Properties

        private readonly LevelManager _levelManager = GameManager.Instance.LevelManager;
        private readonly GameplayManager _gameplayManager = GameManager.Instance.GameplayManager;

        #endregion

        #region Methods
        
        public override void OnEnter()
        {
            _gameplayManager.SetOnRecruitEnlistCallback(GenerateEnlist);
            _gameplayManager.SetOnAttackEnemyCallback();
            _gameplayManager.StartGame();

            GenerateEnlist();
        }

        public override void OnExit()
        {
        }

        private void StartProcess()
        {
            
        }
        
        private void GenerateEnlist()
        {
            _levelManager.GenerateEnlist();
        }

        private void GenerateEnemy()
        {
            _levelManager.GenerateEnemy();
        }
        
        #endregion
    }
}