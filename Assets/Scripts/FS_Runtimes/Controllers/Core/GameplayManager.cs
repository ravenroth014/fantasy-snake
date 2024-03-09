using System;
using FS_Runtimes.Controllers.Level;
using FS_Runtimes.Controllers.Player;
using FS_Runtimes.Controllers.Pooling;
using FS_Runtimes.States;
using FS_Runtimes.Utilities;
using UnityEngine;

namespace FS_Runtimes.Controllers.Core
{
    public class GameplayManager : MonoBehaviour
    {
        #region Fields & Properties

        public CharacterPooling HeroPooling => _heroPooling;
        [SerializeField, Tooltip("Hero Pooling")]
        private CharacterPooling _heroPooling;

        public CharacterPooling EnemyPooling => _enemyPooling;
        [SerializeField, Tooltip("Enemy Pooling")]
        private CharacterPooling _enemyPooling;

        public LevelManager LevelManager => _levelManager;
        [SerializeField, Tooltip("Level Manager")]
        private LevelManager _levelManager;

        public PlayerManager PlayerManager => _playerManager;
        [SerializeField, Tooltip("Player Manager")]
        private PlayerManager _playerManager;
        
        public static GameplayManager Instance => _instance;
        private static GameplayManager _instance;

        private GameState _currentState;
        
        #endregion

        #region Methods

        #region Unity Event Methods
        
        private void Awake()
        {
            _instance = this;
        }

        #endregion

        public void ChangeState(EGameState gameState, Action onComplete = null)
        {
            _currentState?.OnExit();
            _currentState = GameStateHelper.GetGameState(gameState);
            _currentState.OnEnter();
            
            onComplete?.Invoke();
        }

        #endregion
    }
}
