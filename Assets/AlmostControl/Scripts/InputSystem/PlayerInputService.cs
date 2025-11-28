using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace AlmostControl.InputSystem
{
    public class PlayerInputService : IInitializable, ITickable, IDisposable
    {
        public Action OnPressJump;
        public Action OnReleaseJump;
        public Action OnPressAny;
        
        public Vector2 MovementInput => _movementInput;
        
        private Vector2 _movementInput;
        private InputSystemActions _playerInput;
        
        private bool _isMoving;

        public void Initialize()
        {
            _playerInput = new InputSystemActions();
            _playerInput.UI.Enable();

            _playerInput.Player.Move.started += OnStartMove;
            _playerInput.Player.Move.canceled += OnStopMove;
            _playerInput.Player.Jump.started += OnStartJump;
            _playerInput.Player.Jump.canceled += OnStopJump;
            _playerInput.UI.Any.started += OnAnyTap;

            EnableInput();
        }

        private void OnAnyTap(InputAction.CallbackContext context)
        {
            OnPressAny?.Invoke();
        }

        public void EnableInput()
        {
            _playerInput.Player.Enable();
        }

        public void DisableInput()
        {
            _playerInput.Player.Disable();
        }
        public void Tick()
        {
            if (_isMoving)
            {
                _movementInput = _playerInput.Player.Move.ReadValue<Vector2>();
            }
        }

        private void OnStartMove(InputAction.CallbackContext context)
        {
            _isMoving = true;
        }

        private void OnStopMove(InputAction.CallbackContext context)
        {
            _movementInput = Vector2.zero;
            _isMoving = false;
        }

        private void OnStartJump(InputAction.CallbackContext context)
        {
            OnPressJump?.Invoke();
        }
        
        private void OnStopJump(InputAction.CallbackContext context)
        {
            OnReleaseJump?.Invoke();
        }
        
        public void Dispose()
        {
            DisableInput();
            _playerInput.UI.Disable();
        }
    }
}