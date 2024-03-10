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
        private readonly SettingManager _settingManager = SettingManager.Instance;
        private readonly LogManager _logManager = LogManager.Instance;

        private EPlayerAction _lastAction;
        
        #endregion

        #region Methods
        
        public override void OnEnter()
        {
            // TODO: Open Loading UI.
            
            if (Init() == false)
            {
                GameManager.Instance.ChangeState(EGameState.GameError);
                return;
            }
            
            // TODO: Close Loading UI, and countdown before start game.
            
            StartGame();
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

            _lastAction = EPlayerAction.None;

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
            if (_charactersManager.IsMoving) return;
            if (_settingManager.IsGameplayButtonActionValid(_lastAction, playerAction) == false)
            {
                _logManager.Log($"Action {playerAction} is not valid for previous action, {_lastAction}");
                return;
            }
            
            _logManager.Log($"Action {playerAction} is valid for previous action, {_lastAction}");
            _lastAction = playerAction;
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
        
        #endregion
    }
}