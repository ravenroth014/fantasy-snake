using FS_Runtimes.Controllers.Core;
using FS_Runtimes.Controllers.Gameplay;
using FS_Runtimes.Controllers.UI;
using FS_Runtimes.Controllers.Utilities;
using FS_Runtimes.Utilities;

namespace FS_Runtimes.States
{
    public class GameMenuState : GameState
    {
        #region Fields & Properties

        private readonly GameplayManager _gameplayManager = GameManager.Instance.GameplayManager;
        private readonly MainMenuController _mainMenuController = NavigatorManager.Instance.MainMenuController;
        private readonly SettingMenuController _settingMenuController = NavigatorManager.Instance.SettingMenuController;
        private readonly LoadingStateController _loadingStateController = NavigatorManager.Instance.LoadingStateController;
        private readonly LogManager _logManager = LogManager.Instance;

        #endregion
        
        public override void OnEnter()
        {
            if (Init() == false)
                GameManager.Instance.ChangeState(EGameState.GameError);
        }

        public override void OnExit()
        {
            _loadingStateController.Open();
            
            _settingMenuController.Close();
            _mainMenuController.Close();
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
            if (_settingMenuController is null) return false;
            
            _mainMenuController.Init();
            _settingMenuController.Init();

            return true;
        }
        
        private bool InitCallback()
        {
            if (_gameplayManager is null) return false;
            _gameplayManager.SetOnPlayerActionTriggerCallback(OnPlayerTrigger);
            
            return true;
        }

        private void OnPlayerTrigger(EPlayerAction playerAction)
        {
            switch (playerAction)
            {
                case EPlayerAction.Up:
                case EPlayerAction.Right:
                case EPlayerAction.Down:
                case EPlayerAction.Left:
                {
                    _mainMenuController.OnPlayerTrigger(playerAction);
                    break;
                }
                case EPlayerAction.RotateLeft:
                case EPlayerAction.RotateRight:
                case EPlayerAction.None:
                default:
                {
                    _logManager.LogWarning($"Action {playerAction} is not valid for gameplay state");
                    break;
                }
            }
        }
    }
}