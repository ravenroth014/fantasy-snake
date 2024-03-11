using System.Collections;
using FS_Runtimes.Controllers.Character;
using FS_Runtimes.Controllers.Core;
using FS_Runtimes.Controllers.Gameplay;
using FS_Runtimes.Controllers.Level;
using FS_Runtimes.Controllers.UI;
using FS_Runtimes.Controllers.Utilities;
using FS_Runtimes.Models.Characters;
using FS_Runtimes.Utilities;
using UnityEngine;

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
        private readonly LoadingStateController _loadingStateController = NavigatorManager.Instance.LoadingStateController;

        private CharacterPairData _currentHero;
        private CharacterPairData _currentEnemy;
        private EPlayerAction _lastAction;
        private int _currentLevel;
        private int _currentObjective;
        private bool _isBattle;
        
        #endregion

        #region Methods
        
        public override void OnEnter()
        {
            if (Init() == false)
            {
                GameManager.Instance.ChangeState(EGameState.GameError);
                return;
            }
            
            _loadingStateController.Close();
            // TODO: Countdown before start game.
            
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
            _currentLevel = 1;
            
            bool isComplete = true;
            
            isComplete &= InitCallback();
            isComplete &= InitPlayer();
            isComplete &= InitStageLevel();
            isComplete &= InitStageObjective();

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
            CharacterPairData character = _levelManager.GenerateHero(_currentLevel);
            _charactersManager.AddCharacter(character, Vector2.zero);

            return true;
        }

        private bool InitStageLevel()
        {
            if (_levelManager is null) return false;

            _levelManager.GenerateLevel();
            
            return true;
        }

        private bool InitStageObjective()
        {
            if (_levelManager is null) return false;

            _currentObjective = _levelManager.GetCurrentLevelObjective(_currentLevel);

            return true;
        }
        
        #endregion

        #region Decision Making Methods
        
        private void StartGame()
        {
            GenerateEnlist();
            GenerateEnemy();
        }
        
        private void OnPlayerTrigger(EPlayerAction playerAction)
        {
            if (_isBattle) return;
            if (_charactersManager.IsMoving) return;
            if (_settingManager.IsGameplayButtonActionValid(_lastAction, playerAction) == false)
            {
                _logManager.Log($"Action {playerAction} is not valid for previous action, {_lastAction}");
                return;
            }
            
            _logManager.Log($"Action {playerAction} is valid for previous action, {_lastAction}");

            switch (playerAction)
            {
                case EPlayerAction.Up:
                case EPlayerAction.Right:
                case EPlayerAction.Down:
                case EPlayerAction.Left:
                {
                    _lastAction = playerAction;
                    OnMovementAction(playerAction);
                    break;
                }
                case EPlayerAction.RotateLeft:
                case EPlayerAction.RotateRight:
                {
                    OnSwitchCharacterAction(playerAction);
                    break;
                }
                case EPlayerAction.None:
                default:
                {
                    _logManager.LogWarning($"Action {playerAction} is not valid for gameplay state");
                    break;
                }
            }
        }

        private void OnMovementAction(EPlayerAction playerAction)
        {
            
            Vector2 currentPosition = _charactersManager.GetMainCharacterPosition();
            Vector2 direction = GameHelper.GetVector2Direction(playerAction);
            Vector2 targetPosition = currentPosition + direction;
            EGridState gridState = _levelManager.GetGridState(targetPosition);
            _logManager.Log($"Target grid : {targetPosition}, Grid state : {gridState}");

            switch (gridState)
            {
                case EGridState.Empty:
                    OnMoveCharacter(targetPosition);
                    break;
                case EGridState.Occupied:
                    OnMoveToOccupiedGrid(targetPosition);
                    break;
                case EGridState.Obstacle:
                case EGridState.Walled:
                    OnRemoveCharacter(targetPosition);
                    break;
                default:
                    GameManager.Instance.ChangeState(EGameState.GameError);
                    break;
            }
        }

        private void OnSwitchCharacterAction(EPlayerAction playerAction)
        {
            _logManager.Log("Switching characters ...");
            ECharacterSwitch switchAction = GameHelper.GetCharacterSwitchAction(playerAction);
            if (switchAction == ECharacterSwitch.None)
            {
                _logManager.LogWarning($"Action {playerAction} is not valid for switching player characters");
                return;
            }
            
            _charactersManager.SwitchCharacter(switchAction, OnUpdateGridCallback);
        }
        
        private void OnMoveCharacter(Vector2 targetPos)
        {
            _logManager.Log("Moving characters ...");
            _charactersManager.MoveCharacter(targetPos, OnUpdateGridCallback);
        }

        private void OnRemoveCharacter(Vector2 targetPos)
        {
            _logManager.Log("Removing character ...");
            _charactersManager.RemoveMainCharacter(targetPos, OnUpdateGridCallback);
            
            // TODO: Need to check if this is the last character
            // If so, game over.
        }

        private void OnMoveToOccupiedGrid(Vector2 targetPos)
        {
            _logManager.Log("Moving to occupied grid ...");
            ECharacterType characterType = _levelManager.GetGridOccupiedType(targetPos);

            switch (characterType)
            {
                case ECharacterType.Enlist:
                    OnRecruitEnlist(targetPos);
                    break;
                case ECharacterType.Enemy:
                    _gameplayManager.ExecuteCoroutine(OnAttackEnemy(targetPos));
                    break;
                default:
                    _logManager.LogWarning($"Character type, {characterType} is not a valid type for move to occupied grid action.");
                    break;
            }
        }

        private void OnRecruitEnlist(Vector2 targetPos)
        {
            _logManager.Log("Recruiting enlist ...");
            CharacterPairData character = _levelManager.GetEnlistCharacter(_currentLevel);
            
            _charactersManager.AddCharacter(character, targetPos, OnUpdateGridCallback);
            _levelManager.GenerateEnlist();
        }

        private IEnumerator OnAttackEnemy(Vector2 targetPos)
        {
            _isBattle = true;

            _currentEnemy = _levelManager.GetEnemyCharacter();
            _currentHero = _charactersManager.CurrentMainHero;
            _currentHero.CharacterGameObject.SetCharacterDirection(targetPos);

            int targetDamage = _currentEnemy.CharacterData.AtkPoint;
            int heroDamage = _currentHero.CharacterData.AtkPoint;

            do
            {
                _currentEnemy.CharacterData.TakeDamage(heroDamage);
                _currentHero.CharacterData.TakeDamage(targetDamage);
                
                _logManager.Log($"{_currentEnemy.CharacterData.UniqueID} take damage {_currentHero.CharacterData.AtkPoint} from {_currentHero.CharacterData.UniqueID}, HP: {_currentEnemy.CharacterData.CurrentHp.ToString()}/{_currentEnemy.CharacterData.MaxHp}");
                _logManager.Log($"{_currentHero.CharacterData.UniqueID} take damage {_currentEnemy.CharacterData.AtkPoint} from {_currentEnemy.CharacterData.UniqueID}, HP: {_currentHero.CharacterData.CurrentHp.ToString()}/{_currentHero.CharacterData.MaxHp}");
                
                // TODO: Update UI.

                yield return new WaitForSeconds(1.5f);
                
            } while (_currentEnemy.CharacterData.IsDead == false && _charactersManager.CurrentMainHero.CharacterData.IsDead == false);

            _isBattle = false;
            
            if (_currentHero.CharacterData.IsDead)
            {
                _charactersManager.RemoveMainCharacter(targetPos, OnUpdateGridCallback);
                // TODO: If it's last one, Game Over.
            }
            if (_currentEnemy.CharacterData.IsDead)
            {
                _levelManager.RemoveEnemy(targetPos);
                OnUpdateObjective();
                _levelManager.GenerateEnemy(_currentLevel);

            }
        }

        private void OnUpdateObjective()
        {
            _currentObjective--;

            if (_currentObjective == 0)
            {
                _currentLevel++;
                _currentObjective = _levelManager.GetCurrentLevelObjective(_currentLevel);
                _charactersManager.UpdateCharacterLevel(_currentLevel);
            }
        }

        #endregion
        
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

        #region Callback Methods

        private void OnUpdateGridCallback(Vector2 position, string uniqueID)
        {
            EGridState gridState = _levelManager.GetGridState(position);
            if (gridState is EGridState.Walled or EGridState.Obstacle)
                return;
            
            if (string.IsNullOrEmpty(uniqueID))
                _levelManager.UpdateGridData(position, string.Empty, EGridState.Empty, ECharacterType.None);
            else
                _levelManager.UpdateGridData(position, uniqueID, EGridState.Occupied, ECharacterType.Hero);
        }

        #endregion
        
        #endregion
    }
}