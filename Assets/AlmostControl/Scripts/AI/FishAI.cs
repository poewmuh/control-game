using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AlmostControl.AI.BackgroundFish
{
    public class FishAI : MonoBehaviour
    {
        [SerializeField] private float _patrolDistance = 5f;
        [SerializeField] private float _patrolTime = 3f;
        
        private Vector3 _startPosition;
        private Vector3 _endPosition;

        private float _timer;

        private void Start()
        {
            _startPosition = transform.position;
            _endPosition = transform.position + Vector3.left * _patrolDistance;
            _timer = 0f;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            transform.position = Vector3.Lerp(_startPosition, _endPosition, _timer / _patrolTime);

            if ((transform.position - _endPosition).sqrMagnitude < .1)
            {
                var transformLocalScale = transform.localScale;
                transformLocalScale.x *= -1;
                transform.localScale = transformLocalScale;
                _timer = 0;
                (_startPosition, _endPosition) = (_endPosition, _startPosition);
            }
        }
    }
}
