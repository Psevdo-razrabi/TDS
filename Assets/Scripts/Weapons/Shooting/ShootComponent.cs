using Game.AsyncWorker.Interfaces;
using Game.Player.Weapons;
using Game.Player.Weapons.Commands.Recievers;
using Game.Player.Weapons.WeaponClass;
using Game.Player.Weapons.WeaponConfigs;
using UI.Storage;
using UniRx;
using UnityEngine;
using Weapons.InterfaceWeapon;
using Zenject;

public class ShootComponent : IInitializable, IConfigRelize
{     
    private readonly CameraShake _cameraShake;
    private readonly BulletLifeCycle _bulletLifeCycle;
    private readonly Recoil _recoil;
    private readonly Spread _spread;
    private readonly WeaponConfigs _weaponConfigs;
    private readonly WeaponData _weaponData;
    private readonly DistributionConfigs _distributionConfigs;
    private readonly CurrentWeapon _currentWeapon;
    private BaseWeaponConfig _gunConfig;
    public readonly ValueCountStorage<int> AmmoReloadValue;
    
    public ShootComponent(CameraShake cameraShake, BulletLifeCycle bulletLifeCycle, Recoil recoil, Spread spread,
        WeaponConfigs weaponConfigs, WeaponData weaponData, DistributionConfigs distributionConfigs, FireComponent fireComponent, CurrentWeapon currentWeapon
        , ValueCountStorage<int> ammoReloadValue)
    {
        _cameraShake = cameraShake;
        _bulletLifeCycle = bulletLifeCycle;
        _recoil = recoil;
        _spread = spread;
        _weaponConfigs = weaponConfigs;
        _weaponData = weaponData;
        _distributionConfigs = distributionConfigs;
        _currentWeapon = currentWeapon;
        AmmoReloadValue = ammoReloadValue;
        fireComponent.ShotFired += ShotFired;
    }

    private void HandleShoot()
    {
        _bulletLifeCycle.BulletSpawn();
        _spread.StartSpreadReduction();
        _recoil.RecoilCursor();
        _cameraShake.ShakeCamera();
        
        AmmoReloadValue.SetValue(_weaponData.AmmoInMagazine.Value--);
    }

    private void ShotFired()
    {
        if(_weaponData.AmmoInMagazine.Value > 0)
            HandleShoot();
    }

    public void GetWeaponConfig(WeaponComponent weaponComponent)
    {
        _currentWeapon.LoadConfig(weaponComponent);
        _gunConfig = _currentWeapon.CurrentWeaponConfig;
        _weaponData.AmmoInMagazine = new ReactiveProperty<int>(_gunConfig.TotalAmmo);
    }

    public void Initialize()
    {
        _distributionConfigs.ClassesWantConfig.Add(this);
    }
}
 