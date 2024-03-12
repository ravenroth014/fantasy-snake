using System;
using System.Collections;
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
        private LevelManager _levelManager;
        private CharactersManager _charactersManager;
        
        #endregion

        #region Methods
        
        /// <summary>
        /// Call this method to execute the player action into current active game state.
        /// </summary>
        /// <param name="playerAction"></param>
        public void OnPlayerAction(EPlayerAction playerAction)
        {
            _onPlayerTrigger?.Invoke(playerAction);
        }

        /// <summary>
        /// Call this method to set callback of player trigger of current active game state.
        /// </summary>
        /// <param name="callback"></param>
        public void SetOnPlayerActionTriggerCallback(Action<EPlayerAction> callback = null)
        {
            _onPlayerTrigger = callback;
        }

        /// <summary>
        /// Call this method to execute coroutine task from current active game state.
        /// </summary>
        /// <param name="coroutine"></param>
        public void ExecuteCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }

        #endregion
    }
}
