using System;
using Game.Player.Weapons;
using Game.Player.Weapons.Commands.Recievers;
using Game.Player.Weapons.WeaponClass;
using Game.Player.Weapons.WeaponConfigs;
using UniRx;
using UnityEngine;
using Weapons.InterfaceWeapon;
using Zenject;
using Random = UnityEngine.Random;

public class Spread : IConfigRelize, IVisitWeaponType, IInitializable
{
    private readonly WeaponConfigs _weaponConfigs;
    private BaseWeaponConfig _gunConfig;
    private readonly CompositeDisposable _compositeDisposable = new ();
    private IDisposable _reductionSubscription;
    private readonly EventController _eventController;
    private readonly ChangeCrosshair _changeCrosshair;
    private readonly Recoil _recoil;
    private DistributionConfigs _distributionConfigs;
    
    private float _currentSpread;
    private float _baseStepSpread;
    private int _currentBulletCount;

    private int _initialBulletsCount;

    public Spread(WeaponConfigs weaponConfigs, EventController eventController, 
        ChangeCrosshair changeCrosshair, Recoil recoil, DistributionConfigs distributionConfigs)
    {
        _weaponConfigs = weaponConfigs;
        _eventController = eventController;
        _changeCrosshair = changeCrosshair;
        _recoil = recoil;
        _distributionConfigs = distributionConfigs;
    }
    
    public void Initialize()
    {
        _distributionConfigs.ClassesWantConfig.Add(this);
    }
    
    public void GetWeaponConfig(WeaponComponent weaponComponent)
    {
        VisitWeapon(weaponComponent);
        _initialBulletsCount = _gunConfig.InitialBulletsCount;
        CalculateStepSpread();
    }

    public void Visit(Pistol pistol)
    {
        _gunConfig = _weaponConfigs.PistolConfig;
    }

    public void Visit(Rifle rifle)
    {
        _gunConfig = _weaponConfigs.RifleConfig;
    }

    public void Visit(Shotgun shotgun)
    {
        _gunConfig = _weaponConfigs.ShotgunConfig;
    }

    public void VisitWeapon(WeaponComponent component)
    {
        Visit((dynamic)component);
    }
    
    private void CalculateStepSpread()
    {
        _baseStepSpread = _gunConfig.MaxSpread / _gunConfig.MaxSpreadBullet;
        _currentSpread = _baseStepSpread;
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
        
        float spreadAcceleration;
        
        if (_currentBulletCount <= _initialBulletsCount)
        {
            spreadAcceleration = _baseStepSpread * (_currentBulletCount / (float)_initialBulletsCount);
        }
        else
        {
            int excessBullets = _currentBulletCount - _initialBulletsCount;
            spreadAcceleration = _baseStepSpread + Mathf.Pow(excessBullets, _gunConfig.SpreadIncreaseCoefficient);
        }

        _currentSpread += spreadAcceleration;
        _currentSpread = Mathf.Clamp(_currentSpread, 0, _gunConfig.MaxSpread);
        float stepsToReduce = _currentSpread / _baseStepSpread;
        _changeCrosshair.IncreaseFiredSize(_gunConfig.RecoilForce, stepsToReduce);
        _recoil.UpdateSpread(_currentSpread);
        return velocityWithSpread;
    }
    
    private void SpreadReduce()
    {
        _currentSpread -= _baseStepSpread;
        _currentSpread = Mathf.Clamp(_currentSpread, 0, _gunConfig.MaxSpread);
        
        _currentBulletCount = 0;

        _eventController.SpreadReduce();
    }
} 
