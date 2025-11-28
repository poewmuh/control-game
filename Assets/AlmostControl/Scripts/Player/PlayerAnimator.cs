using AlmostControl.Player;
using AlmostControl.Player.Movement;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash(IsWalkingKey);
    private static readonly int OnJump = Animator.StringToHash(OnJumpKey);
    private static readonly int OnJumpDown = Animator.StringToHash(OnJumpdownKey);
    private static readonly int IsGrounded = Animator.StringToHash(IsGroundedKey);
    private const string IsWalkingKey = "IsWalking";
    private const string OnJumpKey = "OnJump";
    private const string OnJumpdownKey = "OnJumpDown";
    private const string IsGroundedKey = "IsGrounded";
    
    [SerializeField] private Animator _animator;
    
    public void Init(Player player)
    {
        ResetParameters();

        player.PlayerMovement.JumpHandler.OnJumpStateChange += OnJumpStateChange;
        player.PlayerMovement.MoveHandler.OnMoveStateChange += OnMoveStateChange;
    }

    private void OnJumpStateChange(JumpState state)
    {
        switch (state)
        {
            case JumpState.Falling:
            case JumpState.FastFalling:
                _animator.SetTrigger(OnJumpDown);
                _animator.SetBool(IsGrounded, false);
                break;
            case JumpState.Jumping:
                _animator.SetTrigger(OnJump);
                _animator.SetBool(IsGrounded, false);
                break;
            case JumpState.Grounded:
                _animator.SetBool(IsGrounded, true);
                break;
        }
    }

    private void OnMoveStateChange(bool isMoving)
    {
        _animator.SetBool(IsWalking, isMoving);
    }

    private void ResetParameters()
    {
        _animator.SetBool(IsWalking, false);
        _animator.SetBool(IsGrounded, true);
        _animator.ResetTrigger(OnJump);
        _animator.ResetTrigger(OnJumpDown);
    }
}
