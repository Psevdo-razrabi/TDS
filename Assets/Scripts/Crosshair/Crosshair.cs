using UnityEngine;
<<<<<<< Updated upstream
using Zenject;
=======
>>>>>>> Stashed changes

public class Crosshair : MonoBehaviour
{
    [SerializeField] private RectTransform _crosshair;
    [SerializeField] private float _speed;
    
    private Vector2 _crosshairPos;
    private Vector2 _recoilPosition;
<<<<<<< Updated upstream
    private Recoil _recoil;
    
    [Inject]
    public Crosshair(Recoil recoil)
    {
        Debug.Log("рекоиль");
        _recoil = recoil;
    }
=======
>>>>>>> Stashed changes

    public RectTransform CrossHair => _crosshair;
    
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        ReadPosition();
    }
    
    private void ReadPosition()
    {
        Vector2 inputMouse = new Vector2(UnityEngine.Input.GetAxis("Mouse X"), UnityEngine.Input.GetAxis("Mouse Y"));
        _crosshairPos = _crosshair.anchoredPosition + inputMouse * _speed;
        UpdateCrosshairPosition(_crosshairPos);
    }

    public void RecoilPlus(Vector2 recoil)
    {
        _crosshair.anchoredPosition += recoil;
        UpdateCrosshairPosition(_crosshair.anchoredPosition);
    }
    public void UpdateCrosshairPosition(Vector2 limitedInput)
    {
        limitedInput.x = Mathf.Clamp(limitedInput.x, -Screen.width / 2, Screen.width / 2);
        limitedInput.y = Mathf.Clamp(limitedInput.y, -Screen.height / 2, Screen.height / 2);

        _crosshair.anchoredPosition = limitedInput;
    }
}
