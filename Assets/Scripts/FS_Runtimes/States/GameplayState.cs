using FS_Runtimes.Controllers.Character;
using FS_Runtimes.Controllers.Core;
using FS_Runtimes.Controllers.Gameplay;
using FS_Runtimes.Controllers.Level;
using FS_Runtimes.Controllers.Pooling;
using FS_Runtimes.Utilities;
using UnityEngine;

namespace FS_Runtimes.States
{
    public class GameplayState : GameState
    {
        #region Fields & Properties

        private readonly CharacterPooling _heroPooling = GameManager.Instance.HeroPooling;
        private readonly CharacterPooling _enemyPooling = GameManager.Instance.EnemyPooling;
        private readonly CharactersManager _charactersManager = GameManager.Instance.CharactersManager;
        private readonly LevelManager _levelManager = GameManager.Instance.LevelManager;
        private readonly GameplayManager _gameplayManager = GameManager.Instance.GameplayManager;

        #endregion

        #region Methods
        
        public override void OnEnter()
        {
            _gameplayManager.SetOnRecruitEnlistCallback(GenerateHero);
            _gameplayManager.StartGame();

            GenerateHero();
        }

        public override void OnExit()
        {
        }

        private void StartProcess()
        {
            
        }
        
        private void GenerateHero()
        {
            CharacterGameObject character = _heroPooling.GetFromPool();
            Vector2 position = _levelManager.GetFreePosition();

            _levelManager.UpdateGridData(position, character.UniqueID, EGridState.Occupied, ECharacterType.Enlist);
            character.SetCharacterPosition(position);
        }
        
        #endregion
    }
}