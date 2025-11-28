using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace AlmostControl.Game
{
    public class LevelsManager : IInitializable, IDisposable
    {
        public event Action OnGameComplete;
        public event Action<int> OnLevelChange;
        
        [Inject] private GameObject[] _levels;

        public int MaxLevels => _levels.Length;
        public int CurrentLevel => _currentLevel;

        private int _currentLevel = 0;

        public LevelsManager(GameObject[] levels)
        {
            _levels = levels;
        }
        
        public void Initialize()
        {
            ActivateLevel(_currentLevel);
        }

        public void NextLevel()
        {
            if (_currentLevel + 1 >= _levels.Length)
            {
                OnGameComplete?.Invoke();
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

        public void Dispose()
        {
            
        }
    }
}