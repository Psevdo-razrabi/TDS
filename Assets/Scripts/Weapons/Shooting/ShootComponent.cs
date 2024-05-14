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
    
    private CompositeDisposable _compositeDisposable = new();
    private ReloadImage _reloadImage;
    public ReactiveProperty<int> AmmoInMagazine { get; private set; } 
    
    public ShootComponent(CameraShake cameraShake, BulletLifeCycle bulletLifeCycle, Recoil recoil, Spread spread, EventController eventController , WeaponConfigs weaponConfigs, ReloadImage reloadImage)
    {
        _cameraShake = cameraShake;
        _bulletLifeCycle = bulletLifeCycle;
        _recoil = recoil;
        _spread = spread;
        _weaponConfigs = weaponConfigs;
        _eventController = eventController;
        _reloadImage = reloadImage;
        
        _reloadImage.ReloadCompletedSubject
            .Subscribe(_ => Debug.Log("перезарядка не отрыгнула"))
            .AddTo(_compositeDisposable);
        
        LoadConfigs();  
    }
    private async void LoadConfigs()
    {
        while (_weaponConfigs.IsLoadConfigs == false)
            await UniTask.Yield();
        
        AmmoInMagazine = new ReactiveProperty<int>(_weaponConfigs.RifleConfig.TotalAmmo);
        //SubscribeAmmo();
        _eventController.ShotFired += ShotFired;
    }

    private void SubscribeAmmo()
    {
        
    }

    private void ShotFired()
    {
        if(AmmoInMagazine.Value > 0)
            HandleShoot();
    }
    public void HandleShoot()
    {
        _bulletLifeCycle.BulletSpawn();
        _spread.StartSpreadReduction();
        _recoil.RecoilCursor();
        _cameraShake.ShakeCamera();
        
        AmmoInMagazine.Value--;
        Debug.Log(AmmoInMagazine.Value);
    }
}
 