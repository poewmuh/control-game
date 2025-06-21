using System;
using AlmostControl.Game;
using UnityEngine;

namespace AlmostControl.Player
{
    public class Player : MonoBehaviour
    {
        private Vector2 _defaultPosition;

        private void Start()
        {
            _defaultPosition = transform.position;
            LevelsManager.Instance.OnLevelChange += OnLevelChange;
        }

        private void OnLevelChange(int newLevel)
        {
            transform.position = _defaultPosition;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Star")
            {
                LevelsManager.Instance.NextLevel();
            }
        }
    }
}