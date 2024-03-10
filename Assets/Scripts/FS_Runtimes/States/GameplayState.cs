using FS_Runtimes.Controllers.Character;
using FS_Runtimes.Controllers.Core;
using FS_Runtimes.Controllers.Gameplay;
using FS_Runtimes.Controllers.Level;
using FS_Runtimes.Controllers.Utilities;
using FS_Runtimes.Utilities;

namespace FS_Runtimes.States
{
    public class GameplayState : GameState
    {
        #region Fields & Properties

        private readonly CharactersManager _charactersManager = GameManager.Instance.CharactersManager;
        private readonly LevelManager _levelManager = GameManager.Instance.LevelManager;
        private readonly GameplayManager _gameplayManager = GameManager.Instance.GameplayManager;

        private EDirection _lastDirection;

        #endregion

        #region Methods
        
        public override void OnEnter()
        {
            // // TODO: Open Loading UI.
            //
            // if (Init() == false)
            // {
            //     GameManager.Instance.ChangeState(EGameState.GameError);
            //     return;
            // }
            //
            // // TODO: Close Loading UI, and countdown before start game.
            //
            // StartGame();
        }

        public override void OnExit()
        {
            _levelManager.ResetManager();
            _charactersManager.ResetManager();
            // TODO: Reset decoration and obstacle.
        }

        #region Init Methods
        
        private bool Init()
        {
            bool isComplete = true;
            
            isComplete &= InitCallback();
            isComplete &= InitPlayer();
            isComplete &= InitStageLevel();

            return isComplete;
        }

        private bool InitCallback()
        {
            if (_gameplayManager is null) return false;
            
            _gameplayManager.SetOnPlayerActionTriggerCallback(OnPlayerTrigger);

            return true;
        }

        private bool InitPlayer()
        {
            CharacterGameObject character = _levelManager.GenerateHero();
            _charactersManager.AddCharacter(character, ECharacterType.Hero);

            return true;
        }

        private bool InitStageLevel()
        {
            if (_levelManager is null) return false;

            LogManager.Instance.Log("Generating level decoration...");
            _levelManager.GenerateDecoration();
            
            LogManager.Instance.Log("Generating obstacle decoration...");
            _levelManager.GenerateObstacle();
            
            return true;
        }
        
        #endregion

        private void StartGame()
        {
            GenerateEnlist();
            GenerateEnemy();
        }
        
        private void OnPlayerTrigger(EPlayerAction playerAction)
        {
            
        }

        #region Generation Methods
        
        private void GenerateEnlist()
        {
            _levelManager.GenerateEnlist();
        }

        private void GenerateEnemy()
        {
            _levelManager.GenerateEnemy();
        }
        
        #endregion
        
        private bool CheckActionAvailable(EDirection action)
        {
            if (_lastDirection == EDirection.None)
                return true;

            if (_lastDirection == EDirection.Right && action is EDirection.None or EDirection.Left)
                return false;
            if (_lastDirection == EDirection.Left && action is EDirection.None or EDirection.Right)
                return false;
            if (_lastDirection == EDirection.Up && action is EDirection.None or EDirection.Down)
                return false;
            if (_lastDirection == EDirection.Down && action is EDirection.None or EDirection.Up)
                return false;
            return true;
        }
        
        #endregion
    }
}