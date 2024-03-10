using System;
using FS_Runtimes.Controllers.Character;
using FS_Runtimes.Controllers.Core;
using FS_Runtimes.Controllers.Gameplay;
using FS_Runtimes.Controllers.Level;
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

        private EPlayerAction _lastAction;
        private int _currentLevel;
        
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
            _currentLevel = 1;

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
                    OnAttackEnemy(targetPos);
                    break;
                default:
                    _logManager.LogWarning($"Character type, {characterType} is not a valid type for move to occupied grid action.");
                    break;
            }
        }

        private void OnRecruitEnlist(Vector2 targetPos)
        {
            _logManager.Log("Recruiting enlist ...");
            CharacterPairData character = _levelManager.GetGridOccupiedCharacter(targetPos, _currentLevel);
            
            _charactersManager.AddCharacter(character, targetPos, OnUpdateGridCallback);
            _levelManager.GenerateEnlist();
        }

        private void OnAttackEnemy(Vector2 targetPos)
        {
            // TODO: Attack enemy logic.
            
            // TODO: If player is dead, Remove player
            // TODO: If enemy is dead, Remove enemy and generate new one.
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