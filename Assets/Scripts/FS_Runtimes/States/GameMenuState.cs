using FS_Runtimes.Controllers.Core;
using FS_Runtimes.Controllers.UI;
using FS_Runtimes.Utilities;

namespace FS_Runtimes.States
{
    public class GameMenuState : GameState
    {
        #region Fields & Properties

        private readonly MainMenuController _mainMenuController = NavigatorManager.Instance.MainMenuController;

        #endregion
        
        public override void OnEnter()
        {
            if (Init() == false)
                GameManager.Instance.ChangeState(EGameState.GameError);
        }

        public override void OnExit()
        {
        }

        private bool Init()
        {
            bool isComplete = true;

            isComplete &= InitController();
            isComplete &= InitCallback();

            return isComplete;
        }

        private bool InitController()
        {
            if (_mainMenuController is null) return false;
            
            _mainMenuController.Init();

            return true;
        }
        
        private bool InitCallback()
        {
            return true;
        }
    }
}