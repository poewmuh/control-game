using AlmostControl.Game;
using TMPro;
using UnityEngine;
using VContainer;

namespace AlmostControl.HUD
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        
        [Inject] private LevelsManager _levelsManager;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            ChangeLevelText(_levelsManager.CurrentLevel, _levelsManager.MaxLevels);
            _levelsManager.OnLevelChange += OnLevelChange;
            _levelsManager.OnGameComplete += OnGameComplete;
        }

        private void OnLevelChange(int newLevel)
        {
            ChangeLevelText(newLevel, _levelsManager.MaxLevels);
        }

        private void OnGameComplete()
        {
            _levelsManager.OnLevelChange -= OnLevelChange;
            _levelsManager.OnGameComplete -= OnGameComplete;
        }

        private void ChangeLevelText(int currentLevel, int maxLevel)
        {
            _levelText.text = $"LEVEL {currentLevel + 1}/{maxLevel}";
        }
    }
}