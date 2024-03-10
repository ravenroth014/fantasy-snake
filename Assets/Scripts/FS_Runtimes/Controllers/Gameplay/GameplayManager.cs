using System;
using FS_Runtimes.Controllers.Character;
using FS_Runtimes.Controllers.Core;
using FS_Runtimes.Controllers.Level;
using FS_Runtimes.States;
using FS_Runtimes.Utilities;
using UnityEngine;

namespace FS_Runtimes.Controllers.Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        #region Fields & Properties

        private Action<EPlayerAction> _onPlayerTrigger;
        private GameState _currentState;

        private EDirection _currentDirection;

        private LevelManager _levelManager;
        private CharactersManager _charactersManager;
        
        #endregion

        #region Methods

        public void Init()
        {
            // Do nothing (For now)
        }

        public void OnPlayerAction(EPlayerAction playerAction)
        {
            _onPlayerTrigger?.Invoke(playerAction);
        }

        public void OnPlayerAction(EDirection directionAction)
        {
            //if (CheckActionAvailable(directionAction) == false) return;
            if (_charactersManager.IsMoving) return;

            Vector2 currentPosition = _charactersManager.GetMainCharacterPosition();
            Vector3 direction = GameHelper.GetWorldSpaceDirection(directionAction);
            Vector2 targetPosition = currentPosition + new Vector2(direction.x, direction.z);
            EGridState gridState = _levelManager.GetGridState(targetPosition);
            
            if (gridState is EGridState.Empty)
            {
                _charactersManager.MoveCharacter(directionAction, OnUpdateGridCallback);
                _currentDirection = directionAction;
            }
            else if (gridState is EGridState.Occupied)
            {
                ECharacterType characterType = _levelManager.GetGridOccupiedType(targetPosition);
                if (characterType == ECharacterType.Enlist)
                {
                    CharacterGameObject character = _levelManager.GetGridOccupiedCharacter(targetPosition);
                    _charactersManager.AddCharacter(character, ECharacterType.Hero, directionAction, OnUpdateGridCallback);
                    _currentDirection = directionAction;
                    _levelManager.GenerateEnlist();
                }
                else if (characterType == ECharacterType.Enemy)
                {
                    // TODO: Implement battle mode.
                }
            }
            else if (gridState is EGridState.Walled or EGridState.Obstacle)
            {
                _charactersManager.RemoveMainCharacter(directionAction, OnUpdateGridCallback);
                _currentDirection = directionAction;
            }
        }

        public void SetOnPlayerActionTriggerCallback(Action<EPlayerAction> callback = null)
        {
            _onPlayerTrigger = callback;
        }

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
    }
}
