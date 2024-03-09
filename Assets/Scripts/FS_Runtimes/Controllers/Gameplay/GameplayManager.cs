using System;
using FS_Runtimes.Controllers.Character;
using FS_Runtimes.Controllers.Core;
using FS_Runtimes.Controllers.Level;
using FS_Runtimes.Utilities;
using UnityEngine;

namespace FS_Runtimes.Controllers.Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        #region Fields & Properties

        private Action _onAttackEnemyAction;
        private Action _onRecruitEnlistAction;
        private Action _onHeroIsDeadAction;
        private Action _onGameOver;
        private Action _onEnemyIsDeadAction;

        private bool _isGameStart;
        private EDirection _currentDirection;

        private LevelManager _levelManager;
        private CharactersManager _charactersManager;
        
        #endregion

        #region Methods

        public void Init()
        {
            _levelManager = GameManager.Instance.LevelManager;
            _charactersManager = GameManager.Instance.CharactersManager;
        }
        
        public void StartGame()
        {
            _isGameStart = true;
            _currentDirection = EDirection.None;
        }

        public void OnCharacterAction(EDirection directionAction)
        {
            if (_isGameStart == false) return;
            if (CheckActionAvailable(directionAction) == false) return;
            if (_charactersManager.IsMoving) return;

            Vector2 currentPosition = _charactersManager.GetMainCharacterPosition();
            Vector3 direction = GameHelper.GetWorldSpaceDirection(directionAction);
            Vector2 targetPosition = currentPosition + new Vector2(direction.x, direction.z);
            EGridState gridState = _levelManager.GetGridState(targetPosition);
            
            if (gridState == EGridState.Empty)
            {
                _charactersManager.MoveCharacter(directionAction);
                _currentDirection = directionAction;
            }
            else if (gridState == EGridState.Occupied)
            {
                ECharacterType characterType = _levelManager.GetGridOccupiedType(targetPosition);
                if (characterType == ECharacterType.Enlist)
                {
                    CharacterGameObject character = _levelManager.GetGridOccupiedCharacter(targetPosition);
                    _charactersManager.AddCharacter(character, ECharacterType.Hero, directionAction);
                    _currentDirection = directionAction;
                    //_levelManager.GenerateEnlist();
                }
            }
        }
        
        public void SetOnAttackEnemyCallback(Action callback = null)
        {
            _onAttackEnemyAction = callback;
        }

        public void SetOnRecruitEnlistCallback(Action callback = null)
        {
            _onRecruitEnlistAction = callback;
        }

        public void SetOnHeroIsDeadCallback(Action callback = null)
        {
            _onHeroIsDeadAction = callback;
        }

        public void SetOnGameOverCallback(Action callback = null)
        {
            _onGameOver = callback;
        }

        public void SetOnEnemyIsDeadCallback(Action callback = null)
        {
            _onEnemyIsDeadAction = callback;
        }

        private bool CheckActionAvailable(EDirection action)
        {
            if (_currentDirection == EDirection.None)
                return true;

            if (_currentDirection == EDirection.Right && action is EDirection.None or EDirection.Left)
                return false;
            if (_currentDirection == EDirection.Left && action is EDirection.None or EDirection.Right)
                return false;
            if (_currentDirection == EDirection.Up && action is EDirection.None or EDirection.Down)
                return false;
            if (_currentDirection == EDirection.Down && action is EDirection.None or EDirection.Up)
                return false;
            return true;
        }

        #endregion
    }
}
