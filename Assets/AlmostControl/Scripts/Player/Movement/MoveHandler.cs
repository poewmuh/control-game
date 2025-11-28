using System;
using AlmostControl.Data;
using UnityEngine;

namespace AlmostControl.Player.Movement
{
    public class MoveHandler
    {
        public event Action<bool> OnMoveStateChange;
        private readonly CollisionChecker _collisionChecker;
        private readonly MovementData _data;
        private readonly Rigidbody2D _rb;
        
        private Vector2 _currentVelocity;
        
        private float currentAcceleration => _collisionChecker.isGrounded ? _data.groundAcceleration : _data.airAcceleration;
        private float currentDeceleration => _collisionChecker.isGrounded ? _data.groundDeceleration : _data.airDeceleration;
        
        public MoveHandler(MovementData movementData, Rigidbody2D rb, CollisionChecker collisionChecker)
        {
            _collisionChecker = collisionChecker;
            _data = movementData;
            _rb = rb;
        }

        public void Move(Vector2 moveInput)
        {
            _currentVelocity = GetNewVelocity(moveInput);
            _rb.linearVelocity = new Vector2(_currentVelocity.x, _rb.linearVelocity.y);
        }
        
        private Vector2 GetNewVelocity(Vector2 moveInput)
        {
            if (moveInput == Vector2.zero)
            {
                OnMoveStateChange?.Invoke(false);
                return Vector2.Lerp(_currentVelocity, Vector2.zero, currentDeceleration * Time.fixedDeltaTime);
                
            }
            else
            {
                OnMoveStateChange?.Invoke(true);
                var targetVelocity = new Vector2(moveInput.x, 0) * _data.moveSpeed;
                return Vector2.Lerp(_currentVelocity, targetVelocity, currentAcceleration * Time.fixedDeltaTime);
            }
        }
    }
}