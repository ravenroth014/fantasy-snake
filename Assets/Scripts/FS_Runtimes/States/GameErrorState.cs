using FS_Runtimes.Controllers.Core;
using FS_Runtimes.Controllers.UI;

namespace FS_Runtimes.States
{
    public class GameErrorState : GameState
    {
        #region Fields & Properites

        private readonly GameErrorUIController _gameErrorUIController = NavigatorManager.Instance.GameErrorUIController;

        #endregion
        
        public override void OnEnter()
        {
            _gameErrorUIController.Open();
        }

        public override void OnExit()
        {
            // Do nothing.
        }
    }
}