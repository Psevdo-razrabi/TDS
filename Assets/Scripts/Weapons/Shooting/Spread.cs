using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Player.Weapons.WeaponConfigs;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spread 
{
    private WeaponConfigs _weaponConfigs;
    private RifleConfig _gunConfig;
    private CompositeDisposable _compositeDisposable = new();
    private IDisposable _reductionSubscription;
    private EventController _eventController;
    private ChangeCrosshair _changeCrosshair;
    
    private float _currentSpread;
    private float _stepSpread;
    private int _bulletsFired = 0;
    private float _maxSpread;
    private float _baseSpread;
    private float _growthFactor;
    public Spread(WeaponConfigs weaponConfigs, EventController eventController,ChangeCrosshair changeCrosshair)
    {
        _weaponConfigs = weaponConfigs;
        _eventController = eventController;
        _changeCrosshair = changeCrosshair;
        LoadConfigs();
    }
    
    private async void LoadConfigs()
    {
        while (_weaponConfigs.IsLoadConfigs == false)
            await UniTask.Yield();
        
        _gunConfig = _weaponConfigs.RifleConfig;
        CalculateMaxSpread();
    }
    
    private void CalculateMaxSpread()
    {
        _maxSpread = _gunConfig.MaxSpread;
        _baseSpread = _gunConfig.BaseSpread;
        _growthFactor = _gunConfig.GrowthFactor;
        _currentSpread = 0;
        _bulletsFired = 0;
    }

    public void StartSpreadReduction()
    {
        _reductionSubscription?.Dispose();

        _reductionSubscription = Observable
            .Interval(TimeSpan.FromSeconds(_gunConfig.TimeToSpreadReduce))
            .Subscribe(_ =>
            {
                SpreadReduce();
                if (_currentSpread <= 0)
                {
                    _reductionSubscription.Dispose();
                }
            }).AddTo(_compositeDisposable);
    }

    public Vector3 CalculatingSpread(Vector3 velocity)
    {
        float spreadX = Random.Range(-_currentSpread, _currentSpread);
        Vector3 velocityWithSpread = velocity + new Vector3(spreadX, 0, 0);

        _bulletsFired++;
        _currentSpread = _baseSpread * Mathf.Pow(_bulletsFired, _growthFactor);
        _currentSpread = Mathf.Clamp(_currentSpread, 0, _maxSpread);
        Debug.Log(_currentSpread);
        float stepsToReduce = _currentSpread / _currentSpread / _gunConfig.TimeToSpreadReduce;
        _changeCrosshair.IncreaseFiredSize(_gunConfig.RecoilForce, stepsToReduce);
        return velocityWithSpread;
    }

    private void SpreadReduce()
    {
        float stepReduce = _currentSpread / _gunConfig.TimeToSpreadReduce;
        _currentSpread -= stepReduce;
        _currentSpread = Mathf.Clamp(_currentSpread, 0, _maxSpread);
        _eventController.SpreadReduce();
    }
} 
