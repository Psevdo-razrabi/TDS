using System;
using Game.Player.PlayerStateMashine;
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
    [SerializeField] private float _minSize;
    
    private CompositeDisposable _compositeDisposable = new();
    private Spread _spread;
    private Vector2[] _initialPositions;    
    private float _expandMultiplier;
    private float _additionalExpansion;
    private float _stepValue;
    private bool _canMove;
    private float _totalExpansion;
    private float _aimingExpansion;
    private StateMachineData _stateMachineData;
    private CurrentWeapon _currentWeapon;
    
    public RectTransform[] CrosshairParts => _crosshairParts;
    public float TotalExpansion => _totalExpansion;
    public Camera CameraObject => _camera;
    public Transform Crosshair => _crosshair.transform;

    [Inject]
    private void Cunstruct(StateMachineData stateMachineData, CurrentWeapon currentWeapon)
    {
        _stateMachineData = stateMachineData;
        _currentWeapon = currentWeapon;
        SubscribeAim();
        SubscribeUpdate();
    }
    
    private void Start()
    {
        _canMove = true; 
        
        _initialPositions = new Vector2[_crosshairParts.Length];
        for (int i = 0; i < _crosshairParts.Length; i++)
        {
            _initialPositions[i] = _crosshairParts[i].anchoredPosition;
        }
        _additionalExpansion = 0f;
        _totalExpansion = 0f;
    }
    
    private void SubscribeUpdate()
    {
        Observable
            .EveryUpdate()
            .Where(_ => _canMove = true)
            .Subscribe(_ => CalculatePosition())
            .AddTo(_compositeDisposable);
    }

    private void SubscribeAim()
    {
        _stateMachineData.IsAiming
            .Subscribe(isAiming =>
            {
                if (isAiming == true)
                    OnAimingStart();
                else
                    OnAimingEnd();
            })
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
        _totalExpansion = _expandMultiplier * _forceChanges + _additionalExpansion + _aimingExpansion;
        
        for (int i = 0; i < _crosshairParts.Length; i++)
        {
            Vector2 direction = _initialPositions[i].normalized;
            Vector2 targetPosition = _initialPositions[i] + direction * Mathf.Max(_totalExpansion, _minSize);
            
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
    
    private void OnAimingStart()
    {
        _aimingExpansion -= _currentWeapon.CurrentWeaponConfig.ReduceCrosshairValue;
    }

    private void OnAimingEnd()
    {
        _aimingExpansion = 0;
    }
}
    