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
        private readonly LogManager _logManager = LogManager.Instance;

        #endregion

        #region Methods

        #region Derived Methods
        
        public override void OnEnter()
        {
            _logManager.Log("Enter main menu state ...");
            
            if (Init() == false)
                GameManager.Instance.ChangeState(EGameState.GameError);
            
            _mainMenuUIController.Open();
        }

        public override void OnExit()
        {
            _settingMenuUIController.Close();
            _mainMenuUIController.Close();
            _gameplayManager.SetOnPlayerActionTriggerCallback();
            
            _logManager.Log("Exit main menu state ...");
        }
        
        #endregion

        #region Init Methods
        
        /// <summary>
        /// Call this method to initialize game state.
        /// </summary>
        /// <returns></returns>
        private bool Init()
        {
            bool isComplete = true;

            isComplete &= InitController();
            isComplete &= InitCallback();

            return isComplete;
        }

        /// <summary>
        /// Call this method to initialize UI controllers.
        /// </summary>
        /// <returns></returns>
        private bool InitController()
        {
            if (_mainMenuUIController is null) return false;
            if (_settingMenuUIController is null) return false;
            
            _mainMenuUIController.Init();
            _settingMenuUIController.Init();

            return true;
        }
        
        /// <summary>
        /// Call this method to initialize UI callback.
        /// </summary>
        /// <returns></returns>
        private bool InitCallback()
        {
            if (_gameplayManager is null) return false;
            _gameplayManager.SetOnPlayerActionTriggerCallback(OnPlayerTrigger);
            
            return true;
        }
        
        #endregion

        #region Callback Methods
        
        /// <summary>
        /// Callback when player trigger action with gamepad.
        /// </summary>
        /// <param name="playerAction"></param>
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
        
        #endregion
        
        #endregion
    }
}