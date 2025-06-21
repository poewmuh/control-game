using System;
using AlmostControl.Game;
using TMPro;
using UnityEngine;

namespace AlmostControl.HUD
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _hintText;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            var levelsManager = LevelsManager.Instance;
            ChangeLevelText(levelsManager.CurrentLevel, levelsManager.MaxLevels);
            levelsManager.OnLevelChange += OnLevelChange;
            levelsManager.OnLevelsEnd += OnLevelsEnd;
        }

        private void OnLevelChange(int newLevel)
        {
            ChangeLevelText(newLevel, LevelsManager.Instance.MaxLevels);

            switch (newLevel)
            {
                case 0:
                    ShowHintText("Use SPACE for JUMP");
                    break;
                case 1:
                    ShowHintText("Use SPACE in AIR for DOUBLE JUMP");
                    break;
                default: HideHintText(); 
                    break;
            }
        }

        private void OnLevelsEnd()
        {
            var levelsManager = LevelsManager.Instance;
            levelsManager.OnLevelChange -= OnLevelChange;
            levelsManager.OnLevelsEnd -= OnLevelsEnd;

            ShowHintText("CONGRATULATION YOU? WIN!");
        }

        private void ShowHintText(string text)
        {
            _hintText.enabled = true;
            _hintText.text = text;
        }
        
        private void HideHintText()
        {
            _hintText.enabled = false;
        }

        private void ChangeLevelText(int currentLevel, int maxLevel)
        {
            _levelText.text = $"LEVEL {currentLevel + 1}/{maxLevel}";
        }
    }
}