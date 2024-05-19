using UI.Storage;
using UnityEngine.InputSystem;
using Zenject;

namespace Input
{
    public class InputSystemUi : InputSystemBase
    {
        private bool _isActive;
        private BoolStorage _boolStorage;

        [Inject]
        private void Construct(BoolStorage boolStorage)
        {
            _boolStorage = boolStorage;
        }

        private void OnEnable() => InputSystemNew.UI.HideStorage.performed += OnСallingPause;

        private void OnDisable() => InputSystemNew.UI.HideStorage.performed -= OnСallingPause;

        private void OnСallingPause(InputAction.CallbackContext obj)
        {
            _isActive = !_isActive;
            _boolStorage.ChangeBoolValue(_isActive);
        }
    }
}