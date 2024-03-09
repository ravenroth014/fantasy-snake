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

        #endregion

        #region Methods

        private void OnEnable()
        {
            _moveInput.action.started += OnMoveTrigger;
        }

        private void OnDisable()
        {
            _moveInput.action.started -= OnMoveTrigger;
        }

        private void OnMoveTrigger(InputAction.CallbackContext action)
        {
            switch (action.control)
            {
                case KeyControl keyControl:
                    OnKeyboardTrigger(keyControl);
                    break;
                case DiscreteButtonControl gamepadControl:
                    OnGamepadTrigger(gamepadControl);
                    break;
            }
        }

        private void OnKeyboardTrigger(KeyControl keyControl)
        {
            EDirection direction = GameHelper.GetDirection(keyControl.keyCode);
        }

        private void OnGamepadTrigger(DiscreteButtonControl gamepadControl)
        {
            EDirection direction = GameHelper.GetDirection(gamepadControl.minValue, gamepadControl.maxValue);
        }

        #endregion
    }
}
