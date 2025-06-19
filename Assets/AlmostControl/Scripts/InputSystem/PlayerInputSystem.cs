using System;
using AlmostControl.Tools;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AlmostControl.InputSystem
{
    public class PlayerInputSystem : MonoSingleton<PlayerInputSystem>
    {
        public Action OnPressJump;
        public Action OnReleaseJump;
        
        public Vector2 MovementInput => _movementInput;
        
        private Vector2 _movementInput;
        private PlayerInput _playerInput;

        private InputAction _moveAction;
        private InputAction _jumpAction;
        
        private bool _isMoving;

        protected override void Init()
        {
            _playerInput = GetComponent<PlayerInput>();
            
            _moveAction = _playerInput.actions["Move"];
            _jumpAction = _playerInput.actions["Jump"];

            _moveAction.started += OnStartMove;
            _moveAction.canceled += OnStopMove;
            _jumpAction.started += OnStartJump;
            _jumpAction.canceled += OnStopJump;
        }

        private void Update()
        {
            if (_isMoving)
            {
                _movementInput = _moveAction.ReadValue<Vector2>();
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
    }
}