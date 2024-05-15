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
    private EventController _eventController;
    private ReloadComponent _reloadComponent;
    
    public ShootComponent(CameraShake cameraShake, BulletLifeCycle bulletLifeCycle, Recoil recoil, Spread spread, EventController eventController , WeaponConfigs weaponConfigs, ReloadComponent reloadComponent)
    {
        _cameraShake = cameraShake;
        _bulletLifeCycle = bulletLifeCycle;
        _recoil = recoil;
        _spread = spread;
        _weaponConfigs = weaponConfigs;
        _eventController = eventController;
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
        Debug.Log(_reloadComponent.AmmoInMagazine.Value + "ткущие пулькф");
    }
    
    private async void LoadConfigs()
    {
        while (_weaponConfigs.IsLoadConfigs == false)
            await UniTask.Yield();
        
        _eventController.ShotFired += ShotFired;
    }

    private void ShotFired()
    {
        if(_reloadComponent.AmmoInMagazine.Value > 0)
            HandleShoot();
    }
}
 