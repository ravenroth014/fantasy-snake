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
        private readonly MainMenuUIController _mainMenuUIController = NavigatorManager.Instance.MainMenuUIController;
        private readonly SettingMenuUIController _settingMenuUIController = NavigatorManager.Instance.SettingMenuUIController;
        private readonly LoadingUIController _loadingUIController = NavigatorManager.Instance.LoadingUIController;
        private readonly LogManager _logManager = LogManager.Instance;

        #endregion
        
        public override void OnEnter()
        {
            _loadingUIController.Open();
            
            if (Init() == false)
                GameManager.Instance.ChangeState(EGameState.GameError);
            
            _loadingUIController.Close();
        }

        public override void OnExit()
        {
            _loadingUIController.Open();
            
            _settingMenuUIController.Close();
            _mainMenuUIController.Close();
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
            if (_mainMenuUIController is null) return false;
            if (_settingMenuUIController is null) return false;
            
            _mainMenuUIController.Init();
            _settingMenuUIController.Init();

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
                    _mainMenuUIController.OnPlayerTrigger(playerAction);
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