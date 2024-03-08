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
            _moveInput.action.started += OnKeyboardTrigger;
        }

        private void OnDisable()
        {
            _moveInput.action.started -= OnKeyboardTrigger;
        }

        private void OnKeyboardTrigger(InputAction.CallbackContext action)
        {
            if (action.control is not KeyControl keyControl) return;
            
            if (keyControl.keyCode == Key.A)
                _test.SetCharacterPosition(EDirection.Left);
            else if (keyControl.keyCode == Key.W)
                _test.SetCharacterPosition(EDirection.Up);
            else if (keyControl.keyCode == Key.S)
                _test.SetCharacterPosition(EDirection.Down);
            else if (keyControl.keyCode == Key.D)
                _test.SetCharacterPosition(EDirection.Right);
        }

        #endregion
    }
}
