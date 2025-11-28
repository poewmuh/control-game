using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AlmostControl.DialogSystems
{
    public class DialogView : MonoBehaviour
    {
        [SerializeField] private GameObject _dialogHolder;
        [SerializeField] private Image _speakerImage;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private TextMeshProUGUI _textField;

        public void EnableDialogs()
        {
            _dialogHolder.SetActive(true);
        }

        public void DisableDialogs()
        {
            _dialogHolder.SetActive(false);
        }

        public void ShowText(string text)
        {
            _textField.text = text;
        }
    }
}