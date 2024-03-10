using FS_Runtimes.Controllers.Character;
using FS_Runtimes.Controllers.Core;
using FS_Runtimes.Controllers.Level;
using FS_Runtimes.Utilities;

namespace FS_Runtimes.States
{
    public class GamePrepareState : GameState
    {
        #region Fields & Properties

        private readonly CharactersManager _charactersManager = GameManager.Instance.CharactersManager;
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

            CharacterGameObject character = _levelManager.GenerateHero();
            _charactersManager.AddCharacter(character, ECharacterType.Hero);
        }
        
        #endregion
    }
}