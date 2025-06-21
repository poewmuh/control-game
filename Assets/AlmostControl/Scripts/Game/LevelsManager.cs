using System;
using AlmostControl.Tools;
using UnityEngine;

namespace AlmostControl.Game
{
    public class LevelsManager : MonoSingleton<LevelsManager>
    {
        public event Action OnLevelsEnd;
        public event Action<int> OnLevelChange;
        
        [SerializeField] private GameObject[] _levels;

        public int MaxLevels => _levels.Length;
        public int CurrentLevel => _currentLevel;

        private int _currentLevel = 0;
        
        protected override void Init()
        {
            ActivateLevel(_currentLevel);
        }

        public void NextLevel()
        {
            if (_currentLevel + 1 >= _levels.Length)
            {
                OnLevelsEnd?.Invoke();
                Debug.Log("No more levels");
                return;
            }
            
            DeactivateLevel(_currentLevel);
            _currentLevel++;
            ActivateLevel(_currentLevel);
        }

        private void ActivateLevel(int level)
        {
            _levels[level].SetActive(true);
            OnLevelChange?.Invoke(level);
        }

        private void DeactivateLevel(int level)
        {
            _levels[level].SetActive(false);
        }
    }
}