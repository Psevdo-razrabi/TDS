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
        
        _gunConfig = _weaponConfigs.PistolConfig;
        CalculateStepSpread();
    }
    
    private void CalculateStepSpread()
    {
        _stepSpread = _gunConfig.MaxSpread / _gunConfig.MaxSpreadBullet;
        _currentSpread = _stepSpread;
    }
    
    public void StartSpreadReduction()
    {
        if (_reductionSubscription != null)
        {
            _reductionSubscription.Dispose();
        }
        
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
        _currentSpread += _stepSpread;
        _currentSpread = Mathf.Clamp(_currentSpread, 0, _gunConfig.MaxSpread);
        float stepsToReduce = _currentSpread / _stepSpread; 
        _changeCrosshair.IncreaseFiredSize(_gunConfig.RecoilForce, stepsToReduce);
        return velocityWithSpread;
    }
    
    private void SpreadReduce()
    {
        _currentSpread -= _stepSpread;
        _currentSpread = Mathf.Clamp(_currentSpread, 0, _gunConfig.MaxSpread);
        _eventController.SpreadReduce();
    }
} 
