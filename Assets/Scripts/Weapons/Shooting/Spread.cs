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
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private IDisposable _reductionSubscription;
    private EventController _eventController;
    private ChangeCrosshair _changeCrosshair;
    private Recoil _recoil;
    
    private float _currentSpread;
    private float _baseStepSpread;
    private float _currentCoefficient;
    private int _currentBulletCount;

    public Spread(WeaponConfigs weaponConfigs, EventController eventController, ChangeCrosshair changeCrosshair, Recoil recoil)
    {
        _weaponConfigs = weaponConfigs;
        _eventController = eventController;
        _changeCrosshair = changeCrosshair;
        _recoil = recoil;
        LoadConfigs();
    }
    
    private async void LoadConfigs()
    {
        while (_weaponConfigs.IsLoadConfigs == false)
            await UniTask.Yield();

        _gunConfig = _weaponConfigs.RifleConfig;
        CalculateStepSpread();
    }
    
    private void CalculateStepSpread()
    {
        _baseStepSpread = _gunConfig.MaxSpread / _gunConfig.MaxSpreadBullet;
        _currentSpread = _baseStepSpread;
        _currentCoefficient = _gunConfig.BaseSpreadCoefficient;
        _currentBulletCount = 0;
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
        _currentBulletCount++;

        float spreadX = Random.Range(-_currentSpread, _currentSpread);
        Vector3 velocityWithSpread = velocity + new Vector3(spreadX, 0, 0);
        
        float spreadAcceleration = _baseStepSpread * _currentCoefficient;
        _currentCoefficient = Mathf.Min(_currentCoefficient * _gunConfig.SpreadMultiplierCoefficient, _gunConfig.MaxSpreadCoefficient);
        
        _currentSpread += spreadAcceleration;
        _currentSpread = Mathf.Clamp(_currentSpread, 0, _gunConfig.MaxSpread);
        Debug.Log(_currentSpread);
        float stepsToReduce = _currentSpread / _baseStepSpread;
        _changeCrosshair.IncreaseFiredSize(_gunConfig.RecoilForce, stepsToReduce);
        _recoil.UpdateSpread(_currentSpread);
        return velocityWithSpread;
    }
    
    private void SpreadReduce()
    {
        _currentSpread -= _baseStepSpread;
        _currentSpread = Mathf.Clamp(_currentSpread, 0, _gunConfig.MaxSpread);
        
        _currentCoefficient = _gunConfig.BaseSpreadCoefficient;
        _currentBulletCount = 0;

        _eventController.SpreadReduce();
    }
} 
