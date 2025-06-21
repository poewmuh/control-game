using System;
using AlmostControl.Data;
using AlmostControl.InputSystem;
using UnityEngine;

namespace AlmostControl.Player.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private MovementData _movementData;
        [SerializeField] private Collider2D _bodyCollider;
        [SerializeField] private Collider2D _groundCollider;
        [SerializeField] private Rigidbody2D _rb;

        private MoveHandler _moveHandler;
        private CollisionChecker _collisionChecker;
        private JumpHandler _jumpHandler;
        
        
        private RaycastHit2D _headHit;
        private bool _isFacingRight = true;

        private void Start()
        {
            _collisionChecker = new CollisionChecker(_movementData, _groundCollider, _bodyCollider);
            _jumpHandler = new JumpHandler(_movementData, _collisionChecker, _rb);
            _moveHandler = new MoveHandler(_movementData, _rb, _collisionChecker);
        }

        private void FixedUpdate()
        {
            _collisionChecker.Tick();
            
            var movementInput = PlayerInputSystem.Instance.MovementInput;
            TryRotate(movementInput);
            _moveHandler.Move(movementInput);
            _jumpHandler.FixedTick();
        }

        private void Update()
        {
            _jumpHandler.Tick();
        }

        private void TryRotate(Vector2 moveInput)
        {
            if (_isFacingRight && moveInput.x < 0)
            {
                _isFacingRight = false;
                transform.Rotate(0f, -180f, 0f);
            }
            else if (!_isFacingRight && moveInput.x > 0)
            {
                _isFacingRight = true;
                transform.Rotate(0f, 180f, 0f);
            }
        }
    }
}