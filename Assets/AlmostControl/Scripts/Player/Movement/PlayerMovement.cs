using System;
using AlmostControl.Data;
using AlmostControl.InputSystem;
using UnityEngine;
using VContainer;

namespace AlmostControl.Player.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Transform _viewTransform;
        [SerializeField] private MovementData _movementData;
        [SerializeField] private Collider2D _bodyCollider;
        [SerializeField] private Collider2D _groundCollider;
        [SerializeField] private Rigidbody2D _rb;

        public MoveHandler MoveHandler { get; private set; }
        private CollisionChecker _collisionChecker;
        public JumpHandler JumpHandler { get; private set; }
        
        
        private RaycastHit2D _headHit;
        private bool _isFacingRight = true;

        [Inject] private PlayerInputService _inputService;

        private void Awake()
        {
            _collisionChecker = new CollisionChecker(_movementData, _groundCollider, _bodyCollider);
            JumpHandler = new JumpHandler(_inputService, _movementData, _collisionChecker, _rb);
            MoveHandler = new MoveHandler(_movementData, _rb, _collisionChecker);
        }

        private void FixedUpdate()
        {
            _collisionChecker.Tick();
            
            var movementInput = _inputService.MovementInput;
            TryRotate(movementInput);
            MoveHandler.Move(movementInput);
            JumpHandler.FixedTick();
        }

        private void Update()
        {
            JumpHandler.Tick();
        }

        private void TryRotate(Vector2 moveInput)
        {
            if (_isFacingRight && moveInput.x < 0)
            {
                _isFacingRight = false;
                _viewTransform.Rotate(0f, -180f, 0f);
            }
            else if (!_isFacingRight && moveInput.x > 0)
            {
                _isFacingRight = true;
                _viewTransform.Rotate(0f, 180f, 0f);
            }
        }
    }
}