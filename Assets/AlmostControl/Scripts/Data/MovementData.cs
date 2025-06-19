using System;
using UnityEngine;

namespace AlmostControl.Data
{
    [CreateAssetMenu(fileName = "MovementData", menuName = "Data/Movement Data")]
    public class MovementData : ScriptableObject
    {
        [Header("Movement")]
        [Range(1, 30)] public float moveSpeed;
        [Range(0, 10)] public float groundAcceleration;
        [Range(0, 10)] public float groundDeceleration;
        [Range(0, 10)] public float airAcceleration;
        [Range(0, 10)] public float airDeceleration;
        
        [Header("Jump")]
        [Range(0, 10)] public float jumpHeight;
        [Range(1, 2)] public float jumpCompensationFactor;
        [Range(0.01f, 1)] public float timeTillJumpApex;
        [Range(0.01f, 7)] public float gravityOnReleaseMultiplier;
        [Range(10, 50)]  public float maxFallSpeed;
        [Range(0, 3)] public int numberOfJumps;
        [Space] 
        [Range(0.02f, 0.3f)] public float timeForUpwardsCancel;
        [Range(0.5f, 1)] public float apexThreshold;
        [Range(0.01f, 1)] public float apexHangTime;
        [Range(0f, 1)] public float jumpBufferTime;
        [Range(0f, 1)] public float jumpCoyoteTime;
        
        [Header("Collisions")]
        public LayerMask groundLayer;
        [Range(0, 1)]public float groundDetectionLength;
        [Range(0, 1)]public float headDetectionLength;
        [Range(0, 1)]public float headWidth;
        
        public float gravity { get; private set; }
        public float initialJumpVelocity { get; private set; }
        public float adjustedJumpHeight { get; private set; }

        private void OnValidate()
        {
            CalculateValues();
        }

        private void CalculateValues()
        {
            adjustedJumpHeight = jumpHeight * jumpCompensationFactor;
            gravity = -(2f * adjustedJumpHeight) / Mathf.Pow(timeTillJumpApex, 2);
            initialJumpVelocity = Mathf.Abs(gravity) * timeTillJumpApex;
        }

        private void OnEnable()
        {
            CalculateValues();
        }
    }
}