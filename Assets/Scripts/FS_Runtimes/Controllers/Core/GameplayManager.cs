using System;
using System.Collections.Generic;
using FS_Runtimes.Controllers.Character;
using FS_Runtimes.Controllers.Level;
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

        public CharactersManager CharactersManager => _charactersManager;
        [SerializeField, Tooltip("Characters Manager")]
        private CharactersManager _charactersManager;
        
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

        private void Start()
        {
            InitControllers();

            ChangeState(EGameState.GamePrepare);
        }

        #endregion

        private void InitControllers()
        {
            _heroPooling.Init();
            _enemyPooling.Init();
            _levelManager.Init();
            _charactersManager.Init();
        }

        public void ChangeState(EGameState gameState, Action onComplete = null)
        {
            _currentState?.OnExit();
            _currentState = GameHelper.GetGameState(gameState);
            _currentState.OnEnter();
            
            onComplete?.Invoke();
        }

        public void ReturnItemListToPool(List<CharacterGameObject> characterList, ECharacterType characterType)
        {
            if (characterType == ECharacterType.Enemy)
                _enemyPooling.ReturnItemListToPool(characterList);
            else if (characterType == ECharacterType.Hero)
                _heroPooling.ReturnItemListToPool(characterList);
        }

        #endregion
    }
}
