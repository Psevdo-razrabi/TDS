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

    public void GetWeaponConfig(WeaponComponent weaponComponent)
    {
        _currentWeapon.LoadConfig(weaponComponent);
        _gunConfig = _currentWeapon.CurrentWeaponConfig;
        OperationWithWeaponData();
        InitDamageForType();
    }

    public void Initialize()
    {
        _distributionConfigs.ClassesWantConfig.Add(this);
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
        HandleShoot();
    }
    private void InitDamageForType()
    {
        foreach (var bodyTypeDamage in _gunConfig.DamageSettings)
        {
            _weaponData.DamageForType[bodyTypeDamage.BodyType] = bodyTypeDamage.Damage;
        }
    }
    private void OperationWithWeaponData()
    {
        _weaponData.AmmoInMagazine = new ReactiveProperty<int>(_gunConfig.TotalAmmo);
        _weaponData.Dispose();
        var currentWeaponAudio = _currentWeapon.WeaponComponent.AudioComponent;
        var currentWeapon = _currentWeapon.WeaponComponent;
        _weaponData.Subscribe(() => currentWeaponAudio.PlayOneShot(currentWeapon.WeaponAudioType.WeaponOutAmmo(),
            _currentWeapon.WeaponPrefabs.CurrentPrefabWeapon.transform.position));
    }
}
 