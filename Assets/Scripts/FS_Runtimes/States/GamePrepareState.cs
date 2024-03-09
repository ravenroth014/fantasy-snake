using FS_Runtimes.Controllers.Character;
using FS_Runtimes.Controllers.Level;
using FS_Runtimes.Controllers.Player;
using FS_Runtimes.Controllers.Pooling;
using FS_Runtimes.Utilities;
using UnityEngine;

namespace FS_Runtimes.States
{
    public class GamePrepareState : GameState
    {
        #region Fields & Properties

        private CharactersManager _charactersManager;
        private CharacterPooling _heroPooling;
        private LevelManager _levelManager;

        #endregion

        #region Constructors

        public GamePrepareState()
        {
            _charactersManager = _gameplayManager.CharactersManager;
            _heroPooling = _gameplayManager.HeroPooling;
            _levelManager = _gameplayManager.LevelManager;
        }

        #endregion
        
        #region Methods
        
        public override void OnEnter()
        {
            PrepareProcess();
            
            _gameplayManager.ChangeState(EGameState.GamePlay);
        }

        public override void OnExit()
        {
            
        }

        private void PrepareProcess()
        {
            _charactersManager.ResetManager();
            _levelManager.ResetManager();

            CharacterGameObject characterGameObject = _heroPooling.GetFromPool();
            Vector2 position = _levelManager.GetFreePosition();

            string uniqueID = _charactersManager.AddCharacter(characterGameObject, ECharacterType.Hero);
            _levelManager.UpdateGridData(position, uniqueID, EGridState.Occupied, ECharacterType.Hero);
        }
        
        #endregion
    }
}