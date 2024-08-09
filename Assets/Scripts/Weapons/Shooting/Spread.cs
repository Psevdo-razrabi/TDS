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
    private WeaponData _weaponData;
    
    private float _spreadMultiplier;
    private float _multiplierIncreaseRate;
    
    private int _initialBulletsCount;
    
    
    public Spread(WeaponConfigs weaponConfigs, ChangeCrosshair changeCrosshair, Recoil recoil, CurrentWeapon currentWeapon, DistributionConfigs distributionConfigs, WeaponData weaponData)
    {
        _weaponConfigs = weaponConfigs;
        _changeCrosshair = changeCrosshair;
        _recoil = recoil;
        _currentWeapon = currentWeapon;
        _distributionConfigs = distributionConfigs;
        _weaponData = weaponData;
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
                _reductionSubscription.Dispose();
            }).AddTo(_compositeDisposable);
    }

    private void SpreadReduce()
    {
        _changeCrosshair.DecreaseFiredSize();
    }

    public Vector3 CalculatingSpread()
    {
        RectTransform crosshairTop = _changeCrosshair.CrosshairParts[0];
        RectTransform crosshairBottom = _changeCrosshair.CrosshairParts[1];
        RectTransform crosshairLeft = _changeCrosshair.CrosshairParts[2];
        RectTransform crosshairRight = _changeCrosshair.CrosshairParts[3];
        
        float randomX = Random.Range(crosshairLeft.anchoredPosition.x, crosshairRight.anchoredPosition.x);
        float randomY = Random.Range(crosshairBottom.anchoredPosition.y, crosshairTop.anchoredPosition.y);
        
        Vector3 randomScreenPoint = new Vector3(randomX, randomY, 0);
        Vector3 worldPoint = ConvertScreenToWorld(randomScreenPoint);
        
        Vector3 spreadDirection = (worldPoint - _weaponData.BulletPoint.position).normalized;

        return spreadDirection;
    }
    private Vector3 ConvertScreenToWorld(Vector3 screenPoint)
    {
        Camera camera = Camera.main;
        
        Ray ray = camera.ScreenPointToRay(screenPoint);
        
        Plane plane = new Plane(Vector3.up, _weaponData.BulletPoint.position);
        float distance;

        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }
    public void GetWeaponConfig(WeaponComponent weaponComponent)
    {
        _currentWeapon.LoadConfig(weaponComponent);
        _gunConfig = _currentWeapon.CurrentWeaponConfig;
    }

    public void Initialize()
    {
        _distributionConfigs.ClassesWantConfig.Add(this);
    }
} 
