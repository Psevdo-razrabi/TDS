using UnityEngine;
public class Crosshair : MonoBehaviour
{
    [SerializeField] private RectTransform _crosshair;
    [SerializeField] private float _speed;
    
    private Vector2 _crosshairPos;
    private Vector2 _recoilPosition;
    
    public RectTransform CrossHair => _crosshair;
    
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _recoilPosition = Vector2.zero;
    }

    void Update()
    {
        CalculateRecoil();
        ReadPosition();
    }
    
    private void ReadPosition()
    {
        Vector2 inputMouse = new Vector2(UnityEngine.Input.GetAxis("Mouse X"), UnityEngine.Input.GetAxis("Mouse Y"));
        _crosshairPos = _crosshair.anchoredPosition + inputMouse * _speed;
    }

    private void CalculateRecoil()
    {
        _crosshairPos -= _recoilPosition;
        UpdateCrosshairPosition(_crosshairPos);
    }
    public void RecoilPlus(Vector2 recoil)
    {
        _recoilPosition += recoil;
    }
    public void UpdateCrosshairPosition(Vector2 limitedInput)
    {
        limitedInput.x = Mathf.Clamp(limitedInput.x, -Screen.width / 2, Screen.width / 2);
        limitedInput.y = Mathf.Clamp(limitedInput.y, -Screen.height / 2, Screen.height / 2);
        
        _crosshair.anchoredPosition = Vector2.Lerp(_crosshair.anchoredPosition, limitedInput , 5 * Time.deltaTime);
    }
}
