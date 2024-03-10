using System;
using FS_Runtimes.Controllers.Character;
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

        public void SetOnPlayerActionTriggerCallback(Action<EPlayerAction> callback = null)
        {
            _onPlayerTrigger = callback;
        }

        #endregion
    }
}
