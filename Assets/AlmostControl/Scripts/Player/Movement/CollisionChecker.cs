using AlmostControl.Data;
using UnityEngine;

namespace AlmostControl.Player.Movement
{
    public class CollisionChecker
    {
        public bool isGrounded { get; private set; }
        public bool isBumpedHead { get; private set; }

        private readonly MovementData _data;
        private readonly Collider2D _groundCollider;
        private readonly Collider2D _bodyCollider;
        
        private RaycastHit2D _groundHit;
        private RaycastHit2D _headHit;

        public CollisionChecker(MovementData data, Collider2D groundCollider, Collider2D bodyCollider)
        {
            _data = data;
            _groundCollider = groundCollider;
            _bodyCollider = bodyCollider;
        }

        public void Tick()
        {
            CheckGrounded();
            CheckHeadBump();
        }
        
        private void CheckGrounded()
        {
            var boxCastOrigin = new Vector2(_groundCollider.bounds.center.x, _groundCollider.bounds.min.y);
            var boxCastSize = new Vector2(_groundCollider.bounds.size.x, _data.groundDetectionLength);
            _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down,
                _data.groundDetectionLength, _data.groundLayer);
            isGrounded = _groundHit.collider;
        }
        
        private void CheckHeadBump()
        {
            var boxCastOrigin = new Vector2(_groundCollider.bounds.center.x, _bodyCollider.bounds.max.y);
            var boxCastSize = new Vector2(_groundCollider.bounds.size.x * _data.headWidth, _data.headDetectionLength);
            _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up,
                _data.headDetectionLength, _data.groundLayer);
            isBumpedHead = _headHit.collider;
        }
    }
}