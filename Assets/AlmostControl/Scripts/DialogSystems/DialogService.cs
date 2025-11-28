using System;
using AlmostControl.Game;
using AlmostControl.InputSystem;
using AlmostControl.Tools;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace AlmostControl.DialogSystems
{
    public class DialogService : IInitializable, IDisposable
    {
        private AddressablesLoader _addressablesLoader;
        private AllDialogs _allDialogs;
        [Inject] private DialogView _dialogView;
        [Inject] private PlayerInputService _inputService;
        [Inject] private LevelsManager _levelsManager;
        
        public void Initialize()
        {
            _addressablesLoader = new AddressablesLoader();
            var dialogsJson = _addressablesLoader.LoadImmediate<TextAsset>("dialogs");
            _allDialogs = JsonConvert.DeserializeObject<AllDialogs>(dialogsJson.text);
        }

        public void ShowDialog(DialogType dialogType, int customInt = 0)
        {
            _inputService.DisableInput();
            var dialogNumber = 0;
            _dialogView.EnableDialogs();
            var textKey = $"{dialogType}_{dialogNumber}_{customInt}_{_levelsManager.CurrentLevel}";

            if (TryShowDialogForText(dialogType, textKey))
            {
                _inputService.OnPressAny += NextPage;
            }
            void NextPage()
            {
                dialogNumber++;
                var textKey = $"{dialogType}_{dialogNumber}_{customInt}_{_levelsManager.CurrentLevel}";
                if (!TryShowDialogForText(dialogType, textKey))
                {
                    _inputService.OnPressAny -= NextPage;
                }
            }
        }

        private bool TryShowDialogForText(DialogType dialogType, string textKey)
        {
            var dialogText = "";
            switch (dialogType)
            {
                case DialogType.Compliment:
                    _allDialogs.Compliments.TryGetValue(textKey, out dialogText);
                    break;
                case DialogType.Respawn:
                    _allDialogs.RespawnDialogs.TryGetValue(textKey, out dialogText);
                    break;
            }
            
            if (!string.IsNullOrEmpty(dialogText))
            {
                _dialogView.ShowText(dialogText);
                return true;
            }
            else
            {
                _dialogView.DisableDialogs();
                _inputService.EnableInput();
                return false;
            }
        }

        public void Dispose()
        {
            
        }
    }
}