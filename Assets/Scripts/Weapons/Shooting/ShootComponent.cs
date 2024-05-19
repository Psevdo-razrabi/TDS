using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Player.Weapons;
using Game.Player.Weapons.InterfaseWeapon;
using Game.Player.Weapons.ReloadStrategy;
using Game.Player.Weapons.WeaponConfigs;
using UniRx;
using UnityEngine;
using Zenject;

public class ShootComponent
{        
    
    private CameraShake _cameraShake;
    private BulletLifeCycle _bulletLifeCycle;
    private Recoil _recoil;
    private Spread _spread;
    private WeaponConfigs _weaponConfigs;
    private FireComponent _shootComponent;
    private ReloadComponent _reloadComponent;
    
    public ShootComponent(CameraShake cameraShake, BulletLifeCycle bulletLifeCycle, Recoil recoil, Spread spread, FireComponent shootComponent , WeaponConfigs weaponConfigs, ReloadComponent reloadComponent)
    {
        _cameraShake = cameraShake;
        _bulletLifeCycle = bulletLifeCycle;
        _recoil = recoil;
        _spread = spread;
        _weaponConfigs = weaponConfigs;
        _shootComponent = shootComponent;
        _reloadComponent = reloadComponent;
        
        LoadConfigs();  
    }
    public void HandleShoot()
    {
        _bulletLifeCycle.BulletSpawn();
        _spread.StartSpreadReduction();
        _recoil.RecoilCursor();
        _cameraShake.ShakeCamera();
        
       _reloadComponent.AmmoInMagazine.Value--;
    }
    
    private async void LoadConfigs()
    {
        while (_weaponConfigs.IsLoadConfigs == false)
            await UniTask.Yield();
        
        _shootComponent.ShotFired += ShotFire;
    }

    private void ShotFire()
    {
        if(_reloadComponent.AmmoInMagazine.Value > 0)
            HandleShoot();
    }
}
 