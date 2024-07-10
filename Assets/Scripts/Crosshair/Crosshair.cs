using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private RectTransform _crosshair;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _recoilSmoothingSpeed;

    private Vector2 _targetCrosshairPos;

    public RectTransform CrossHair => _crosshair;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _targetCrosshairPos = _crosshair.anchoredPosition;
    }

    void Update()
    {
        ReadMousePosition();
        SmoothCrosshairMovement();
    }

    private void ReadMousePosition()
    {
        Vector2 inputMouse = new Vector2(UnityEngine.Input.GetAxis("Mouse X"), UnityEngine.Input.GetAxis("Mouse Y"));
        _targetCrosshairPos += inputMouse * _movementSpeed * Time.deltaTime;
        _targetCrosshairPos = ClampCrosshairPosition(_targetCrosshairPos);
    }

    private void SmoothCrosshairMovement()
    {
        _crosshair.anchoredPosition = Vector2.Lerp(_crosshair.anchoredPosition, _targetCrosshairPos, _recoilSmoothingSpeed * Time.deltaTime);
        _crosshair.anchoredPosition = ClampCrosshairPosition(_crosshair.anchoredPosition);
    }

    public void RecoilPlus(Vector2 recoil)
    {
        _targetCrosshairPos += recoil;
        
        _targetCrosshairPos = ClampCrosshairPosition(_targetCrosshairPos);
    }

    private Vector2 ClampCrosshairPosition(Vector2 position)
    {
        return new Vector2(
            Mathf.Clamp(position.x, -Screen.width / 2f, Screen.width / 2f),
            Mathf.Clamp(position.y, -Screen.height / 2f, Screen.height / 2f)
        );
    }
}
