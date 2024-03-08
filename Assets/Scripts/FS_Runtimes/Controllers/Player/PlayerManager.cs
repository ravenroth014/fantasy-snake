using FS_Runtimes.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace FS_Runtimes.Controllers.Player
{
    public class PlayerManager : MonoBehaviour
    {
        #region Fields & Properties

        [SerializeField, Tooltip("Player Input")] private InputActionReference _moveInput;

        public CharacterModule _test;

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
            EDirection direction = KeyboardHelper.GetDirection(keyControl.keyCode);
            _test.SetCharacterPosition(direction);
        }

        private void OnGamepadTrigger(DiscreteButtonControl gamepadControl)
        {
            EDirection direction = GamePadHelper.GetDirection(gamepadControl.minValue, gamepadControl.maxValue);
            _test.SetCharacterPosition(direction);
        }

        #endregion
    }
}
