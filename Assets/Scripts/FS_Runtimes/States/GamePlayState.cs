using FS_Runtimes.Controllers.Character;
using FS_Runtimes.Controllers.Level;
using FS_Runtimes.Controllers.Pooling;

namespace FS_Runtimes.States
{
    public class GamePlayState : GameState
    {
        #region Fields & Properties

        private readonly CharacterPooling _heroPooling;
        private readonly CharacterPooling _enemyPooling;
        private readonly CharactersManager _charactersManager;
        private readonly LevelManager _levelManager;

        #endregion

        #region Constructors

        public GamePlayState()
        {
            _heroPooling = _gameplayManager.HeroPooling;
            _enemyPooling = _gameplayManager.EnemyPooling;
            _charactersManager = _gameplayManager.CharactersManager;
            _levelManager = _gameplayManager.LevelManager;
        }

        #endregion

        #region Methods
        
        public override void OnEnter()
        {
        }

        public override void OnExit()
        {
        }
        
        #endregion
    }
}