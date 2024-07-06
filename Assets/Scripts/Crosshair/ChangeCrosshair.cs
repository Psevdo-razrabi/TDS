using System;
using UniRx;
using UnityEngine;
using Zenject;

public class ChangeCrosshair : MonoBehaviour
{
    [SerializeField] private Crosshair _crosshair;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _player;
    [SerializeField] private RectTransform[] _crosshairParts;
    [SerializeField] private float _forceChanges;
    [SerializeField] private float _maxExpandDistance;
    [SerializeField] private float _maxAdditionalExpansion;
    [SerializeField] private float _speedExpand;
    
    private CompositeDisposable _compositeDisposable = new();
    private Spread _spread;
    private Vector2[] _initialPositions;    
    private float _expandMultiplier;
    private float _additionalExpansion;
    private float _stepValue;
    private bool _canMove;
    private float _totalExpansion;

    public float TotalExpansion => _totalExpansion;
    public Camera CameraObject => _camera;
    public Transform Crosshair => _crosshair.transform;
    
    private void Start()
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
        _totalExpansion = _expandMultiplier * _forceChanges + _additionalExpansion;
        
        for (int i = 0; i < _crosshairParts.Length; i++)
        {
            Vector2 direction = _initialPositions[i].normalized;
            Vector2 targetPosition = _initialPositions[i] + direction * _totalExpansion;
            
            _crosshairParts[i].anchoredPosition = Vector2.Lerp(_crosshairParts[i].anchoredPosition, targetPosition, _speedExpand * Time.deltaTime);
        }
    }
    
    public void IncreaseFiredSize(float additionalExpansion,float stepToReduce)
    {
        _additionalExpansion += additionalExpansion;
        _additionalExpansion = Mathf.Min(_additionalExpansion, _maxAdditionalExpansion);
        _stepValue = _additionalExpansion / stepToReduce;
    }                                                                 

    public void DecreaseFiredSize()
    {
        _additionalExpansion = Mathf.Max(0, _additionalExpansion - _stepValue);
    }
}
