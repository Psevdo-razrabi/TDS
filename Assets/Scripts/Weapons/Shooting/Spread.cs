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

public class Spread : IConfigRelize, IInitializable
{
    private readonly WeaponConfigs _weaponConfigs;
    private BaseWeaponConfig _gunConfig;
    private readonly CompositeDisposable _compositeDisposable = new();
    private IDisposable _reductionSubscription;
    private readonly ChangeCrosshair _changeCrosshair;
    private readonly Recoil _recoil;
    private readonly CurrentWeapon _currentWeapon;
    private readonly DistributionConfigs _distributionConfigs;
    private float _currentSpread;
    private float _baseIncrement;
    private int _currentBulletCount;
    private Vector3 _crosshairSpread;
    
    private float _spreadMultiplier;
    private float _multiplierIncreaseRate;
    
    private int _initialBulletsCount;
    
    
    public Spread(WeaponConfigs weaponConfigs, ChangeCrosshair changeCrosshair, Recoil recoil, CurrentWeapon currentWeapon, DistributionConfigs distributionConfigs)
    {
        _weaponConfigs = weaponConfigs;
        _changeCrosshair = changeCrosshair;
        _recoil = recoil;
        _currentWeapon = currentWeapon;
        _distributionConfigs = distributionConfigs;
    }
    
    private void CalculateBaseIncrement()
    {
        _baseIncrement = _gunConfig.BaseIncrement;
        _currentSpread = _gunConfig.StartSpread;
    }
    
    public Vector3 CalculateCrosshairSpread()
    {
        float randomX = Random.Range(-_changeCrosshair.TotalExpansion, _changeCrosshair.TotalExpansion);
        Debug.Log(randomX);
        Vector2 firePointScreenPosition = RectTransformUtility.WorldToScreenPoint(_changeCrosshair.CameraObject, _changeCrosshair.Crosshair.position);
        Vector2 targetScreenPosition = new Vector2(firePointScreenPosition.x + randomX, firePointScreenPosition.y);
        Ray ray = _changeCrosshair.CameraObject.ScreenPointToRay(targetScreenPosition);

        return ray.direction;
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
                if (_currentSpread <= _gunConfig.StartSpread)
                {
                    _currentSpread =  _gunConfig.StartSpread;
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
        float spreadX = Random.Range(-_currentSpread, _currentSpread);
        Vector3 velocityWithSpread = velocity + new Vector3(spreadX, 0, 0);
        
        _currentSpread *= _spreadMultiplier;
        _spreadMultiplier += _multiplierIncreaseRate;

        _currentSpread = Mathf.Clamp(_currentSpread, 0, _gunConfig.MaxSpread);
        Debug.Log($"ан конфиг макс спреад {_gunConfig.MaxSpread} ");
        Debug.Log(_currentSpread);
        float stepsToReduce = _currentSpread / _baseIncrement;

        _changeCrosshair.IncreaseFiredSize(_gunConfig.RecoilForce, stepsToReduce);
        _recoil.UpdateSpread(_currentSpread);

        return velocityWithSpread;
    }

    public void GetWeaponConfig(WeaponComponent weaponComponent)
    {
        _currentWeapon.LoadConfig(weaponComponent);
        _gunConfig = _currentWeapon.CurrentWeaponConfig;

        _spreadMultiplier = _gunConfig.SpreadMultiplier;
        _multiplierIncreaseRate = _gunConfig.MultiplierIncreaseRate;
        CalculateBaseIncrement();
    }

    public void Initialize()
    {
        _distributionConfigs.ClassesWantConfig.Add(this);
    }
} 
