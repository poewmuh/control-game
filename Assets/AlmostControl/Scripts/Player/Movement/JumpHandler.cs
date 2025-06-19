using AlmostControl.Data;
using AlmostControl.InputSystem;
using UnityEngine;

namespace AlmostControl.Player.Movement
{
    public class JumpHandler
    {
        private enum JumpState
        {
            Grounded,
            Jumping,
            Falling,
            FastFalling
        }
        
        private readonly CollisionChecker _collisionChecker;
        private readonly MovementData _data;
        private readonly Rigidbody2D _rb;

        private JumpState _state = JumpState.Grounded;
        
        private float _apexPoint;
        private float _verticalVelocity;
        private float _jumpBufferTimer;
        private float _coyoteTimer;
        private float _fastFallTimer;
        private float _fastFallReleaseSpeed;
        private float _timePastApex;

        private int _jumpUsedCount;

        private bool _jumpReleasedDuringBuffer;
        
        public JumpHandler(MovementData data, CollisionChecker collisionChecker, Rigidbody2D rb)
        {
            _data = data;
            _collisionChecker = collisionChecker;
            _rb = rb;

            PlayerInputSystem.Instance.OnPressJump += OnPressJump;
            PlayerInputSystem.Instance.OnReleaseJump += OnReleaseJump;
        }

        ~JumpHandler()
        {
            PlayerInputSystem.Instance.OnPressJump -= OnPressJump;
            PlayerInputSystem.Instance.OnReleaseJump -= OnReleaseJump;
        }
        
        public void FixedTick()
        {
            ApplyVerticalPhysics();
            _verticalVelocity = Mathf.Clamp(_verticalVelocity, -_data.maxFallSpeed, 50f);
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _verticalVelocity);
        }

        public void Tick()
        {
            UpdateTimers();
            TryStartJump();
            TryLand();
        }
        
        private void ApplyVerticalPhysics()
        {
            HandleHeadCollision();

            switch (_state)
            {
                case JumpState.Jumping:
                    HandleJumping();
                    break;

                case JumpState.Falling:
                    _verticalVelocity += _data.gravity * Time.fixedDeltaTime;
                    break;

                case JumpState.FastFalling:
                    HandleFastFall();
                    break;
            }

            TryFall();
        }
        
        private void HandleHeadCollision()
        {
            if (_collisionChecker.isBumpedHead && _state == JumpState.Jumping)
            {
                StartFastFall();
            }
        }
        
        private void TryFall()
        {
            if (!_collisionChecker.isGrounded && _state == JumpState.Grounded)
            {
                _state = JumpState.Falling;
            }
        }
        
        private void HandleJumping()
        {
            if (_verticalVelocity >= 0)
            {
                _apexPoint = Mathf.InverseLerp(_data.initialJumpVelocity, 0f, _verticalVelocity);
                if (_apexPoint > _data.apexThreshold)
                {
                    _timePastApex += Time.fixedDeltaTime;

                    if (_timePastApex < _data.apexHangTime)
                    {
                        _verticalVelocity = 0f;
                    }
                    else
                    {
                        _verticalVelocity -= 0.01f;
                    }
                }
                else
                {
                    _verticalVelocity += _data.gravity * Time.fixedDeltaTime;
                    _timePastApex = 0f;
                }
            }
            else
            {
                _verticalVelocity += _data.gravity * _data.gravityOnReleaseMultiplier * Time.fixedDeltaTime;
                _state = JumpState.Falling;
            }
        }

        private void HandleFastFall()
        {
            _fastFallTimer += Time.fixedDeltaTime;

            if (_fastFallTimer >= _data.timeForUpwardsCancel)
            {
                _verticalVelocity += _data.gravity * _data.gravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else
            {
                _verticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, _fastFallTimer / _data.timeForUpwardsCancel);
            }

            if (_verticalVelocity < 0 && _state != JumpState.Falling)
            {
                _state = JumpState.Falling;
            }
        }

        private void TryStartJump()
        {
            if (_jumpBufferTimer <= 0) return;
            
            if (_state == JumpState.Grounded || _coyoteTimer > 0)
            {
                StartJump(1);
                if (_jumpReleasedDuringBuffer)
                {
                    StartFastFall();
                }
            }
            else if (_state == JumpState.Jumping && _jumpUsedCount < _data.numberOfJumps)
            {
                StartJump(1);
            }
            else if (_state is JumpState.Falling && _jumpUsedCount < _data.numberOfJumps)
            {
                StartJump(2);
            }
        }
        
        private void StartFastFall()
        {
            _state = JumpState.FastFalling;
            _fastFallTimer = 0f;
            _fastFallReleaseSpeed = _verticalVelocity;
        }
        
        private void TryLand()
        {
            if (_collisionChecker.isGrounded && _verticalVelocity <= 0 &&
                _state is JumpState.Jumping or JumpState.Falling or JumpState.FastFalling)
            {
                _state = JumpState.Grounded;
                _verticalVelocity = Physics2D.gravity.y;
                _jumpUsedCount = 0;
                _fastFallTimer = 0f;
                _timePastApex = 0f;
            }
        }

        private void OnPressJump()
        {
            _jumpBufferTimer = _data.jumpBufferTime;
            _jumpReleasedDuringBuffer = false;
        }

        private void StartJump(int jumpUsing)
        {
            _state = JumpState.Jumping;
            _jumpBufferTimer = 0f;
            _jumpUsedCount += jumpUsing;
            _verticalVelocity = _data.initialJumpVelocity;
            _timePastApex = 0f;
        }

        private void OnReleaseJump()
        {
            if (_jumpBufferTimer > 0)
            {
                _jumpReleasedDuringBuffer = true;
            }

            if (_state == JumpState.Jumping && _verticalVelocity > 0)
            {
                StartFastFall();
                _verticalVelocity = 0f;
            }
        }

        private void UpdateTimers()
        {
            _jumpBufferTimer -= Time.deltaTime;
            if (!_collisionChecker.isGrounded)
            {
                _coyoteTimer -= Time.deltaTime;
            }
            else
            {
                _coyoteTimer = _data.jumpCoyoteTime;
            }
        }
    }
}