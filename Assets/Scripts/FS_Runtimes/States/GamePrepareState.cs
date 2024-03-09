using FS_Runtimes.Controllers.Character;
using FS_Runtimes.Controllers.Core;
using FS_Runtimes.Controllers.Level;
using FS_Runtimes.Controllers.Pooling;
using FS_Runtimes.Utilities;
using UnityEngine;

namespace FS_Runtimes.States
{
    public class GamePrepareState : GameState
    {
        #region Fields & Properties

        private readonly CharactersManager _charactersManager = GameManager.Instance.CharactersManager;
        private readonly CharacterPooling _heroPooling = GameManager.Instance.HeroPooling;
        private readonly LevelManager _levelManager = GameManager.Instance.LevelManager;

        #endregion

        #region Methods
        
        public override void OnEnter()
        {
            PrepareProcess();
            GameManager.Instance.ChangeState(EGameState.GamePlay);
        }

        public override void OnExit()
        {
            // Do Nothing.
        }

        private void PrepareProcess()
        {
            _charactersManager.ResetManager();
            _levelManager.ResetManager();

            CharacterGameObject characterGameObject = _heroPooling.GetFromPool();
            Vector2 position = _levelManager.GetFreePosition();

            _charactersManager.AddCharacter(characterGameObject, ECharacterType.Hero, position);
            _levelManager.UpdateGridData(position, characterGameObject.UniqueID, EGridState.Occupied, ECharacterType.Hero);
        }
        
        #endregion
    }
}