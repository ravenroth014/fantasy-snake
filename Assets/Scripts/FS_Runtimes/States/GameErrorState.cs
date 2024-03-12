using FS_Runtimes.Controllers.Core;
using FS_Runtimes.Controllers.UI;
using FS_Runtimes.Controllers.Utilities;

namespace FS_Runtimes.States
{
    public class GameErrorState : GameState
    {
        #region Fields & Properites

        private readonly GameErrorUIController _gameErrorUIController = NavigatorManager.Instance.GameErrorUIController;
        private readonly LogManager _logManager = LogManager.Instance;

        #endregion

        #region Methods
        
        public override void OnEnter()
        {
            _logManager.LogError("Enter game error state !!!");
            _gameErrorUIController.Init();
            _gameErrorUIController.Open();
        }

        public override void OnExit()
        {
            // Do nothing.
        }
        
        #endregion
    }
}