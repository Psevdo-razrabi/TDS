using UniRx;
using UnityEngine;

public class ChangeCrosshair : MonoBehaviour
{
    [SerializeField] private Crosshair _crosshair;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _player;
    [SerializeField] private RectTransform[] _crosshairParts;
    [SerializeField] private float _forceChanges;
    [SerializeField] private float _maxExpandDistance;

    private CompositeDisposable _compositeDisposable = new();
    private Vector2[] _initialPositions;
    private float _expandMultiplier;
    private float _additionalExpansion;
    private float _stepValue;
    private bool _canMove;
    
    void Start()
    {
        _canMove = true; 
        
        _initialPositions = new Vector2[_crosshairParts.Length];
        for (int i = 0; i < _crosshairParts.Length; i++)
        {
            _initialPositions[i] = _crosshairParts[i].anchoredPosition;
        }

        _additionalExpansion = 0f;
        SubscribeUpdate();
    }
    
    private void SubscribeUpdate()
    {
        Observable
            .EveryUpdate()
            .Where(_ => _canMove = true)
            .Subscribe(_ => CalculatePosition())
            .AddTo(_compositeDisposable);
    }

    private void CalculatePosition()
    {
        Vector2 playerScreenPosition = RectTransformUtility.WorldToScreenPoint(_camera, _player.position);
        Vector2 crosshairScreenPosition = _crosshair.CrossHair.position;
        float distance = Vector2.Distance(playerScreenPosition, crosshairScreenPosition);
        _expandMultiplier = Mathf.Clamp(distance / _maxExpandDistance, 0, 1);
        ChangeSize();
    }

    private void ChangeSize()
    {
        for (int i = 0; i < _crosshairParts.Length; i++)
        {
            Vector2 direction = _initialPositions[i].normalized;
            float totalExpansion = _expandMultiplier * _forceChanges + _additionalExpansion;
            Vector2 targetPosition = _initialPositions[i] + direction * totalExpansion;
            _crosshairParts[i].anchoredPosition = Vector2.Lerp(_crosshairParts[i].anchoredPosition, targetPosition, 10 * Time.deltaTime);
        }
    }

    public void IncreaseFiredSize(float additionalExpansion,float stepToReduce)
    {
        _additionalExpansion += additionalExpansion;
        _stepValue = _additionalExpansion / stepToReduce;
    }                                                                 

    public void DecreaseFiredSize()
    {
        _additionalExpansion = Mathf.Max(0, _additionalExpansion - _stepValue);
    }
}
