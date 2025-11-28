using System;
using UnityEngine;

namespace AlmostControl.Player
{
    public class PlayerHealthComponent : MonoBehaviour
    {
        public event Action<int> OnHealthChange;
        public event Action OnPlayerDeath;

        private const int MAX_HEALTH = 100;
        private int _currentHealth;

        private void Awake()
        {
            ResetHealth();
        }

        public void ResetHealth()
        {
            _currentHealth = MAX_HEALTH;
        }

        public void TakeDamage(int amount)
        {
            _currentHealth -= amount;
            OnHealthChange?.Invoke(_currentHealth);
            
            if (_currentHealth <= 0)
            {
                OnPlayerDeath?.Invoke();
            }
        }
    }
}