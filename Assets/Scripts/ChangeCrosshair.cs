using UnityEngine;

public class ChangeCrosshair : MonoBehaviour
{
    [SerializeField] private Crosshair _crosshair;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _player;
    [SerializeField] private RectTransform[] _crosshairParts;
    [SerializeField] private float _forceChanges;
    [SerializeField] private float _maxExpandDistance;
    private Vector2[] _initialPositions;
    private float _expandMultiplier;
    private float _additionalExpansion;
    private float _recoverySpeed;
    
    void Start()
    {
        _initialPositions = new Vector2[_crosshairParts.Length];
        for (int i = 0; i < _crosshairParts.Length; i++)
        {
            _initialPositions[i] = _crosshairParts[i].anchoredPosition;
        }

        _additionalExpansion = 0f;
    }

    void Update()
    {
        Vector2 playerScreenPosition = RectTransformUtility.WorldToScreenPoint(_camera, _player.position);
        Vector2 crosshairScreenPosition = _crosshair.CrossHair.position;
        float distance = Vector2.Distance(playerScreenPosition, crosshairScreenPosition);
        _expandMultiplier = Mathf.Clamp(distance / _maxExpandDistance, 0, 1);
        _additionalExpansion = Mathf.Max(0, _additionalExpansion - _recoverySpeed * 2);
        Debug.Log(_additionalExpansion);
        ChangeSize();
    }

    private void ChangeSize()
    {
        for (int i = 0; i < _crosshairParts.Length; i++)
        {
            Vector2 direction = _initialPositions[i].normalized;
            float totalExpansion =_expandMultiplier * _forceChanges + _additionalExpansion;
            _crosshairParts[i].anchoredPosition = _initialPositions[i] + direction * totalExpansion;
        }
    }

    public void IncreaseFiredSize(float additionalExpansion,float recoverySpeed)
    {
        _additionalExpansion += additionalExpansion;
        _recoverySpeed = recoverySpeed;
        //Debug.Log("Дополнительный курсор "+_recoverySpeed);
    }
}
