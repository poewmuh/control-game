using AlmostControl.Player;
using UnityEngine;

namespace AlmostControl.Hazards.Trap
{
    public class DamageOnCollidePlayer : MonoBehaviour
    {
        private const int DAMAGE = 100;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var playerHealth = other.GetComponent<PlayerHealthComponent>();
            if (!playerHealth) return;
            
            playerHealth.TakeDamage(DAMAGE);
        }
    }
}