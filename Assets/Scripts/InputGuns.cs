using UI.ViewModel;
using UnityEngine;
using Zenject;

public class InputGuns : MonoBehaviour
{
    [Inject] private StorageViewModel _storageViewModel;

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            _storageViewModel.PistolEquip();
        }
        else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2))
        {
            _storageViewModel.RifleEquip();
        }
        else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3))
        {
            _storageViewModel.ShotgunEquip();
        }
    }
}
