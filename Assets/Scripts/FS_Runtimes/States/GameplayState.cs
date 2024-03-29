﻿using System;
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
        
        private readonly GameplayUIController _gameplayUIController = NavigatorManager.Instance.GameplayUIController;
        
        private readonly SettingManager _settingManager = SettingManager.Instance;
        private readonly LogManager _logManager = LogManager.Instance;

        private CharacterPairData _currentHero;
        private CharacterPairData _currentEnemy;
        private EPlayerAction _lastAction;
        private bool _isBattle;
        private bool _isReady;
        
        #endregion

        #region Methods

        #region Derived Methods

        public override void OnEnter()
        {
            _logManager.Log("Enter gameplay state ...");
            
            if (Init() == false)
            {
                GameManager.Instance.ChangeState(EGameState.GameError);
                return;
            }

            StartGame();
        }

        public override void OnExit()
        {
            _isReady = false;
            _gameplayUIController.Close();
            _gameplayManager.SetOnPlayerActionTriggerCallback();
            _logManager.Log("Exit gameplay state ...");
        }

        #endregion

        #region Init Methods
        
        /// <summary>
        /// Call this method to initialize game state.
        /// </summary>
        /// <returns></returns>
        private bool Init()
        {
            bool isComplete = true;
            
            isComplete &= InitCallback();
            isComplete &= InitStageLevel();
            isComplete &= InitPlayer();

            _lastAction = EPlayerAction.None;

            return isComplete;
        }

        /// <summary>
        /// Call this method to initialize game state callback.
        /// </summary>
        /// <returns></returns>
        private bool InitCallback()
        {
            if (_gameplayManager is null) return false;
            
            _gameplayManager.SetOnPlayerActionTriggerCallback(OnPlayerTrigger);

            return true;
        }

        /// <summary>
        /// Call this method to initialize player in this game state.
        /// </summary>
        /// <returns></returns>
        private bool InitPlayer()
        {
            CharacterPairData character = _levelManager.GenerateHero();
            _charactersManager.AddCharacter(character, Vector2.zero);
            _levelManager.GenerateCharactersOnStart();
            
            _gameplayUIController.UpdatePlayerText(_charactersManager.CurrentMainHero);
            _gameplayUIController.UpdateEnemyText(null);
            
            return true;
        }

        /// <summary>
        /// Call this method to initialize stage level of this game state.
        /// </summary>
        /// <returns></returns>
        private bool InitStageLevel()
        {
            if (_levelManager is null) return false;

            _levelManager.GenerateLevel();
            _gameplayUIController.UpdateKillCountText(_levelManager.TotalKillEnemies);
            
            return true;
        }
        
        #endregion

        #region Decision Making Methods

        /// <summary>
        /// Call this method to start game flow.
        /// </summary>
        private void StartGame()
        {
            _gameplayUIController.Open();
            _isReady = true;
            _logManager.Log("Game Start !!!");
        }
        
        /// <summary>
        /// Callback method when player trigger action.
        /// </summary>
        /// <param name="playerAction"></param>
        private void OnPlayerTrigger(EPlayerAction playerAction)
        {
            if (_isReady == false) return;
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

        /// <summary>
        /// Call this method to execute move character to grid and decide action of target grid.
        /// </summary>
        /// <param name="playerAction"></param>
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
                {
                    OnMoveCharacter(targetPosition);
                    OnEndPhase(_charactersManager.CurrentMainHero);
                    break;
                }
                case EGridState.Occupied:
                {
                    OnMoveToOccupiedGrid(targetPosition);
                    break;
                }
                case EGridState.Obstacle:
                case EGridState.Walled:
                    OnRemoveCharacter(targetPosition);
                    break;
                default:
                    GameManager.Instance.ChangeState(EGameState.GameError);
                    break;
            }
        }

        /// <summary>
        /// Call this method to execute switch character action.
        /// </summary>
        /// <param name="playerAction"></param>
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
        
        /// <summary>
        /// Call this method to move character to new grid.
        /// </summary>
        /// <param name="targetPos"></param>
        private void OnMoveCharacter(Vector2 targetPos)
        {
            _logManager.Log("Moving characters ...");
            _charactersManager.MoveCharacter(targetPos, OnUpdateGridCallback);
        }

        /// <summary>
        /// Call this method to remove character from grid.
        /// </summary>
        /// <param name="targetPos"></param>
        private void OnRemoveCharacter(Vector2 targetPos)
        {
            _logManager.Log("Removing character ...");
            _charactersManager.RemoveMainCharacter(targetPos, OnUpdateGridCallback);
            
            if (_charactersManager.CurrentMainHero == null)
            {
                OnGameOver();
                return;
            }
            
            OnEndPhase(_charactersManager.CurrentMainHero);
        }

        /// <summary>
        /// Call this method to move to occupied grid and make decision.
        /// </summary>
        /// <param name="targetPos"></param>
        private void OnMoveToOccupiedGrid(Vector2 targetPos)
        {
            _logManager.Log("Moving to occupied grid ...");
            ECharacterType characterType = _levelManager.GetGridOccupiedType(targetPos);

            switch (characterType)
            {
                case ECharacterType.Enlist:
                {
                    OnRecruitEnlist(targetPos);
                    OnEndPhase(_charactersManager.CurrentMainHero);
                    break;
                }
                case ECharacterType.Enemy:
                {
                    _gameplayManager.ExecuteCoroutine(OnAttackEnemy(targetPos, () =>
                    {
                        OnEndPhase(_charactersManager.CurrentMainHero);
                    }));
                    break;
                }
                case ECharacterType.Hero:
                {
                    OnGameOver();
                    break;
                }
                default:
                    _logManager.LogWarning($"Character type, {characterType} is not a valid type for move to occupied grid action.");
                    break;
            }
        }

        /// <summary>
        /// Call this method to recruit enlist from target grid.
        /// </summary>
        /// <param name="targetPos"></param>
        private void OnRecruitEnlist(Vector2 targetPos)
        {
            _logManager.Log("Recruiting enlist ...");
            CharacterPairData character = _levelManager.GetEnlistCharacter(targetPos);
            
            _charactersManager.AddCharacter(character, targetPos, OnUpdateGridCallback);
        }

        /// <summary>
        /// Call this method to execute battle with enemy on target grid.
        /// </summary>
        /// <param name="targetPos"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        private IEnumerator OnAttackEnemy(Vector2 targetPos, Action onComplete = null)
        {
            _isBattle = true;

            _currentEnemy = _levelManager.GetEnemyCharacter(targetPos);
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
                
                _gameplayUIController.UpdatePlayerText(_currentHero);
                _gameplayUIController.UpdateEnemyText(_currentEnemy);

                yield return new WaitForSeconds(1.5f);
                
            } while (_currentEnemy.CharacterData.IsDead == false && _charactersManager.CurrentMainHero.CharacterData.IsDead == false);

            _isBattle = false;

            if (_currentHero.CharacterData.IsDead)
            {
                _charactersManager.RemoveMainCharacter(targetPos, OnUpdateGridCallback);
            }
            if (_currentEnemy.CharacterData.IsDead)
            {
                _levelManager.RemoveEnemy(targetPos);
                _levelManager.GenerateCharacters();
            }

            if (_charactersManager.CurrentMainHero == null)
            {
                OnGameOver();
                yield return null;
            }
            
            onComplete?.Invoke();
        }

        /// <summary>
        /// Call this method when it reach game over condition.
        /// </summary>
        private void OnGameOver()
        {
            _logManager.Log("Game Over!!!");
            GameManager.Instance.ChangeState(EGameState.GameOver);
        }

        /// <summary>
        /// Call this method to execute end turn.
        /// </summary>
        /// <param name="heroData"></param>
        /// <param name="enemyData"></param>
        private void OnEndPhase(CharacterPairData heroData = null, CharacterPairData enemyData = null)
        {
            _levelManager.OnTriggerActionEndPhase();
            _charactersManager.OnTriggerActionEndPhase();
            
            _gameplayUIController.UpdatePlayerText(heroData);
            _gameplayUIController.UpdateEnemyText(enemyData);
            _gameplayUIController.UpdateKillCountText(_levelManager.TotalKillEnemies);
        }
        
        #endregion

        #region Callback Methods
        
        /// <summary>
        /// Callback when require to update grid data.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="uniqueID"></param>
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