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

public class Spread
{
    private readonly WeaponConfigs _weaponConfigs;
    private BaseWeaponConfig _gunConfig;
    private readonly CompositeDisposable _compositeDisposable = new();
    private IDisposable _reductionSubscription;
    private readonly ChangeCrosshair _changeCrosshair;
    private readonly Recoil _recoil;
    private DistributionConfigs _distributionConfigs;
    private readonly CurrentWeapon _currentWeapon;
    
    private float _currentSpread;
    private float _baseIncrement;
    private int _currentBulletCount;
    
    private float _spreadMultiplier;
    private float _multiplierIncreaseRate;
    
    private int _initialBulletsCount;
    
    public Spread(WeaponConfigs weaponConfigs, ChangeCrosshair changeCrosshair, Recoil recoil, DistributionConfigs distributionConfigs, CurrentWeapon currentWeapon)
    {
        _weaponConfigs = weaponConfigs;
        _changeCrosshair = changeCrosshair;
        _recoil = recoil;
        _distributionConfigs = distributionConfigs;
        _currentWeapon = currentWeapon;
    }
    
    private void GetCurrentConfig()
    {
        _gunConfig = _currentWeapon.GunConfig;
        Debug.Log(_gunConfig.TotalAmmo);
    }
    
    private void CalculateBaseIncrement()
    {
        _baseIncrement = _gunConfig.BaseIncrement;
        _currentSpread = _baseIncrement;
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
                if (_currentSpread <= 0.1f)
                {
                    _currentSpread = 0.1f;
                    _reductionSubscription.Dispose();
                }
            }).AddTo(_compositeDisposable);
    }

    private void SpreadReduce()
    {
        float reductionAmount = _baseIncrement;
        _currentSpread = Mathf.Max(0, _currentSpread - reductionAmount);
        _spreadMultiplier -= _multiplierIncreaseRate;
        _changeCrosshair.DecreaseFiredSize();
    }

    public Vector3 CalculatingSpread(Vector3 velocity)
    {
        GetCurrentConfig();
        float spreadX = Random.Range(-_currentSpread, _currentSpread);
        Vector3 velocityWithSpread = velocity + new Vector3(spreadX, 0, 0);

        _currentSpread *= _spreadMultiplier;
        _spreadMultiplier += _multiplierIncreaseRate;

        _currentSpread = Mathf.Clamp(_currentSpread, 0, _gunConfig.MaxSpread);
        Debug.Log(_currentSpread);
        float stepsToReduce = _currentSpread / _baseIncrement;

        _changeCrosshair.IncreaseFiredSize(_gunConfig.RecoilForce, stepsToReduce);
        _recoil.UpdateSpread(_currentSpread);

        return velocityWithSpread;
    }
} 
