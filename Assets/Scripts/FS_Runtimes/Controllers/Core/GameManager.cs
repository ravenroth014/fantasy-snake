using System;
using FS_Runtimes.Controllers.Character;
using FS_Runtimes.Controllers.Gameplay;
using FS_Runtimes.Controllers.Level;
using FS_Runtimes.Controllers.Player;
using FS_Runtimes.Controllers.Pooling;
using FS_Runtimes.States;
using FS_Runtimes.Utilities;
using UnityEngine;

namespace FS_Runtimes.Controllers.Core
{
    public class GameManager : MonoBehaviour
    {
        #region Fields & Properties

        public LevelManager LevelManager => _levelManager;
        [SerializeField, Tooltip("Level Manager")]
        private LevelManager _levelManager;

        public CharactersManager CharactersManager => _charactersManager;
        [SerializeField, Tooltip("Characters Manager")]
        private CharactersManager _charactersManager;

        public PlayerController PlayerController => _playerController;
        [SerializeField, Tooltip("Player Controller")]
        private PlayerController _playerController;

        public GameplayManager GameplayManager => _gameplayManager;
        [SerializeField, Tooltip("Gameplay Manager")]
        private GameplayManager _gameplayManager;

        public static GameManager Instance => _instance;
        private static GameManager _instance;
        
        private GameState _currentState;

        #endregion

        #region Methods

        #region Unity Event Methods

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            InitControllers();

            ChangeState(EGameState.GamePrepare);
        }

        #endregion

        #region Init Methods

        private void InitControllers()
        {
            _levelManager.Init();
            _charactersManager.Init();
            _playerController.Init();
            _gameplayManager.Init();
        }

        #endregion

        #region Management Methods

        public void ChangeState(EGameState gameState, Action onComplete = null)
        {
            _currentState?.OnExit();
            _currentState = GameHelper.GetGameState(gameState);
            _currentState.OnEnter();
            
            onComplete?.Invoke();
        }

        #endregion

        #endregion
    }
}
