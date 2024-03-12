using FS_Runtimes.Controllers.Core;
using FS_Runtimes.Controllers.Gameplay;
using FS_Runtimes.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace FS_Runtimes.Controllers.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Fields & Properties

        [SerializeField, Tooltip("Player Input")] private InputActionReference _moveInput;

        private EDirection _currentDirection;
        private GameplayManager _gameplayManager;

        #endregion

        #region Methods

        #region Unity Event Methods
        
        private void OnEnable()
        {
            _moveInput.action.started += OnMoveTrigger;
        }

        private void OnDisable()
        {
            _moveInput.action.started -= OnMoveTrigger;
        }
        
        #endregion

        #region Init Methods
        
        /// <summary>
        /// Call this method to initialize required init setting. 
        /// </summary>
        public void Init()
        {
            _gameplayManager = GameManager.Instance.GameplayManager;
        }
        
        #endregion

        #region Player Action Trigger Methods

        /// <summary>
        /// Call this method to determine player action by player's input decision.
        /// </summary>
        /// <param name="action"></param>
        private void OnMoveTrigger(InputAction.CallbackContext action)
        {
            switch (action.control)
            {
                case KeyControl keyControl:
                    OnKeyboardTrigger(keyControl);
                    break;
                case DiscreteButtonControl discreteButtonControl:
                    OnDiscreteGamepadTrigger(discreteButtonControl);
                    break;
                case ButtonControl buttonControl:
                    OnButtonGamepadTrigger(buttonControl);
                    break;
            }
        }

        /// <summary>
        /// Call this method to execute player action by keyboard.
        /// </summary>
        /// <param name="keyControl"></param>
        private void OnKeyboardTrigger(KeyControl keyControl)
        {
            EPlayerAction playerAction = GameHelper.GetPlayerAction(keyControl.keyCode);
            _gameplayManager.OnPlayerAction(playerAction);
        }

        /// <summary>
        /// Call this method to execute player action by d-Pad on gamepad.
        /// </summary>
        /// <param name="gamepadControl"></param>
        private void OnDiscreteGamepadTrigger(DiscreteButtonControl gamepadControl)
        {
            EPlayerAction playerAction = GameHelper.GetPlayerAction(gamepadControl.minValue, gamepadControl.maxValue);
            _gameplayManager.OnPlayerAction(playerAction);
        }

        /// <summary>
        /// Call this method to execute player action by corner buttons on gamepad.
        /// </summary>
        /// <param name="gamepadControl"></param>
        private void OnButtonGamepadTrigger(ButtonControl gamepadControl)
        {
            EPlayerAction playerAction = GameHelper.GetPlayerAction(gamepadControl.name);
            _gameplayManager.OnPlayerAction(playerAction);
        }
        
        #endregion

        #endregion
    }
}
