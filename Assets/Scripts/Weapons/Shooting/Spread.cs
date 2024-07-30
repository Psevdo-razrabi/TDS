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

    public Vector3 CalculatingSpread(Vector3 velocity)
    {
        float top = _changeCrosshair.CrosshairParts[0].anchoredPosition.y;
        float bottom = _changeCrosshair.CrosshairParts[1].anchoredPosition.y;
        
        float left = _changeCrosshair.CrosshairParts[2].anchoredPosition.x;
        float right = _changeCrosshair.CrosshairParts[3].anchoredPosition.x;
        
        float offsetX = Random.Range(left, right);
        float offsetY = Random.Range(bottom, top);
        
        Vector2 screenPoint = new Vector2(offsetX, offsetY);
        Vector3 worldPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(_changeCrosshair.CrosshairParts[0].parent as RectTransform, screenPoint, _changeCrosshair.CameraObject, out worldPoint);
        
        worldPoint += velocity;
        
        _changeCrosshair.IncreaseFiredSize(_gunConfig.RecoilForce, 0.3f);
        _recoil.UpdateSpread(_currentSpread);
        
        return worldPoint;
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
