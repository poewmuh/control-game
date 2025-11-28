using AlmostControl.DialogSystems;
using UnityEngine;
using VContainer;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] private DialogType _dialogType;
    [SerializeField] private int _dialogNumber;
    
    [Inject] private DialogService _dialogService;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _dialogService.ShowDialog(_dialogType, _dialogNumber);
        }
    }
}
