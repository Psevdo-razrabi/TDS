using Game.AsyncWorker.Interfaces;
using Game.Player.Weapons;
using Game.Player.Weapons.Commands.Recievers;
using Game.Player.Weapons.WeaponClass;
using Game.Player.Weapons.WeaponConfigs;
using UniRx;
using UnityEngine;
using Weapons.InterfaceWeapon;
using Zenject;

public class ShootComponent : IInitializable, IConfigRelize, IVisitWeaponType
{     
    private readonly CameraShake _cameraShake;
    private readonly BulletLifeCycle _bulletLifeCycle;
    private readonly Recoil _recoil;
    private readonly Spread _spread;
    private readonly EventController _eventController;
    private readonly IAwaiter _awaiter;
    private readonly WeaponConfigs _weaponConfigs;
    private readonly WeaponData _weaponData;
    private readonly DistributionConfigs _distributionConfigs;
    private BaseWeaponConfig _baseWeaponConfig;
    
    public ShootComponent(CameraShake cameraShake, BulletLifeCycle bulletLifeCycle, Recoil recoil, Spread spread, 
        EventController eventController, IAwaiter awaiter, 
        WeaponConfigs weaponConfigs, WeaponData weaponData, DistributionConfigs distributionConfigs)
    {
        _cameraShake = cameraShake;
        _bulletLifeCycle = bulletLifeCycle;
        _recoil = recoil;
        _spread = spread;
        _eventController = eventController;
        _awaiter = awaiter;
        _weaponConfigs = weaponConfigs;
        _weaponData = weaponData;
        _distributionConfigs = distributionConfigs;
    }
    
    public void Initialize()
    {
        _distributionConfigs.ClassesWantConfig.Add(this);
        LoadConfigs();
    }

    private void HandleShoot()
    {
        _bulletLifeCycle.BulletSpawn();
        _spread.StartSpreadReduction();
        _recoil.RecoilCursor();
        _cameraShake.ShakeCamera();

        _weaponData.AmmoInMagazine.Value--;
    }
    
    private async void LoadConfigs()
    {
        await _awaiter.AwaitLoadWeaponConfigs(_weaponConfigs);
        _eventController.ShotFired += ShotFired;
    }

    private void ShotFired()
    {
        if(_weaponData.AmmoInMagazine.Value > 0)
            HandleShoot();
    }

    public void GetWeaponConfig(WeaponComponent weaponComponent)
    {
        VisitWeapon(weaponComponent);
    }

    public void Visit(Pistol pistol)
    {
        _baseWeaponConfig = _weaponConfigs.PistolConfig;
        _weaponData.AmmoInMagazine = new ReactiveProperty<int>(_baseWeaponConfig.TotalAmmo);
    }

    public void Visit(Rifle rifle)
    {
        _baseWeaponConfig  = _weaponConfigs.RifleConfig;
        _weaponData.AmmoInMagazine = new ReactiveProperty<int>(_baseWeaponConfig.TotalAmmo);
    }

    public void Visit(Shotgun shotgun)
    {
        _baseWeaponConfig  = _weaponConfigs.ShotgunConfig;
        _weaponData.AmmoInMagazine = new ReactiveProperty<int>(_baseWeaponConfig.TotalAmmo);
    }

    public void VisitWeapon(WeaponComponent component)
    {
        Visit((dynamic)component);
    }
}
 