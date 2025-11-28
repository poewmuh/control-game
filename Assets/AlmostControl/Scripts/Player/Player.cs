using AlmostControl.DialogSystems;
using AlmostControl.Game;
using AlmostControl.Player.Movement;
using AlmostControl.Tools;
using UnityEngine;
using VContainer;

namespace AlmostControl.Player
{
    public class Player : MonoBehaviour
    {
        public PlayerMovement PlayerMovement { get; private set; }
        public PlayerHealthComponent HealthComponent { get; private set; }
        public PlayerAnimator PlayerAnimator { get; private set; }
        
        private int _allDeathCounter;
        
        private Vector2 _defaultPosition;
        
        [Inject] private LevelsManager _levelsManager;
        [Inject] private DialogService _dialogService;

        private void Start()
        {
            InitComponents();
            InitStartParameters();
            
            Subscribes();
            
            RespawnPlayer();
        }
        
        private void InitComponents()
        {
            HealthComponent = GetComponent<PlayerHealthComponent>();
            PlayerAnimator = GetComponent<PlayerAnimator>();
            PlayerMovement = GetComponent<PlayerMovement>();
            
            PlayerAnimator.Init(this);
        }

        private void InitStartParameters()
        {
            _defaultPosition = transform.position;
            _allDeathCounter = 0;
        }

        private void OnDestroy()
        {
            Unsubscribes();
        }

        private void OnDeath()
        {
            _allDeathCounter++;
            RespawnPlayer();
        }

        private void RespawnPlayer()
        {
            _dialogService.ShowDialog(DialogType.Respawn, _allDeathCounter);
            
            HealthComponent.ResetHealth();
            transform.position = _defaultPosition;
        }

        private void OnLevelChange(int newLevel)
        {
            _allDeathCounter = 0;
            RespawnPlayer();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Star")
            {
                _levelsManager.NextLevel();
            }
        }
        
        private void Subscribes()
        {
            HealthComponent.OnPlayerDeath += OnDeath;
            _levelsManager.OnLevelChange += OnLevelChange;
        }

        private void Unsubscribes()
        {
            HealthComponent.OnPlayerDeath -= OnDeath;
            _levelsManager.OnLevelChange -= OnLevelChange;
        }
    }
}