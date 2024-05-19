using System;
using Cysharp.Threading.Tasks;
using Game.Player.Interfaces;
using Game.Player.Weapons;
using Game.Player.Weapons.Commands.Recievers;
using Game.Player.Weapons.WeaponClass;
using Game.Player.Weapons.WeaponConfigs;
using UI.Storage;
using UnityEngine;
using Weapons.InterfaceWeapon;
using Zenject;

public class BulletLifeCycle : IConfigRelize, IVisitWeaponType, IInitializable
{
    private const float BulletLifeTime = 2f;
        
    private PoolObject<Bullet> _pool;
    private readonly WeaponConfigs _weaponConfigs;
    private EventController _eventController;
    private Spread _spread;
    private Rigidbody _bulletRigidbody;
    private BaseWeaponConfig _weaponConfig;
    private DistributionConfigs _distributionConfigs;
    
    public BulletLifeCycle(PoolObject<Bullet> pool, WeaponConfigs weaponConfigs, 
        Spread spread, EventController eventController, DistributionConfigs distributionConfigs)
    {
        _pool = pool;
        _weaponConfigs = weaponConfigs;
        _spread = spread;
        _eventController = eventController;
        _distributionConfigs = distributionConfigs;
    }
    
    public void Initialize()
    {
        _eventController.BulletStoped += StopBullet;
        _distributionConfigs.ClassesWantConfig.Add(this);
    }
    
    public void GetWeaponConfig(WeaponComponent weaponComponent)
    {
        VisitWeapon(weaponComponent);
    }

    public void Visit(Pistol pistol)
    {
        _weaponConfig = _weaponConfigs.PistolConfig;
    }

    public void Visit(Rifle rifle)
    {
        _weaponConfig = _weaponConfigs.RifleConfig;
    }

    public void Visit(Shotgun shotgun)
    {
        _weaponConfig = _weaponConfigs.ShotgunConfig;
    }

    public void VisitWeapon(WeaponComponent component)
    {
        Visit((dynamic)component);
    }
    
    public async void BulletSpawn()
    {
        _pool.AddElementsInPool("bullet", _weaponConfigs.BulletConfig.BulletPrefab, _weaponConfig.TotalAmmo);
        Bullet bullet = _pool.GetElementInPool("bullet");
        bullet.Initialize(_weaponConfig.TotalAmmo);
        bullet.transform.position = _weaponConfig.BulletPoint.transform.position;
        bullet.transform.rotation = Quaternion.LookRotation(_weaponConfig.BulletPoint.transform.forward);
        await BulletLaunch(bullet);
    }

    private async UniTask BulletLaunch(Bullet bullet)
    {
        _bulletRigidbody = bullet.GetComponent<Rigidbody>();
        Vector3 velocity = _weaponConfig.BulletPoint.transform.forward * _weaponConfigs.BulletConfig.BulletSpeed;
        _bulletRigidbody.velocity = velocity + _spread.CalculatingSpread(velocity);
        await ReturnBullet(bullet);
    }
    
    private void StopBullet()
    {
        _bulletRigidbody.velocity = Vector3.zero;
    }
    
    private async UniTask ReturnBullet(Bullet bullet)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(BulletLifeTime));
        bullet.gameObject.SetActive(false);
    }
}
