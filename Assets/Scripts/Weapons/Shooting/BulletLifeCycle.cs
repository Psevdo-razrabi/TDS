using System;
using Cysharp.Threading.Tasks;
using Game.Player.Interfaces;
using Game.Player.Weapons;
using Game.Player.Weapons.Commands.Recievers;
using Game.Player.Weapons.Prefabs;
using Game.Player.Weapons.WeaponClass;
using Game.Player.Weapons.WeaponConfigs;
using UI.Storage;
using UnityEngine;
using Weapons.InterfaceWeapon;
using Zenject;

public class BulletLifeCycle : IConfigRelize, IInitializable
{
    private const float BulletLifeTime = 2f;
        
    private PoolObject<Bullet> _pool;
    private readonly WeaponConfigs _weaponConfigs;
    private Spread _spread;
    private Rigidbody _bulletRigidbody;
    private BaseWeaponConfig _gunConfig;
    private DistributionConfigs _distributionConfigs;
    private CurrentWeapon _currentWeapon;
    private WeaponPrefabs _weaponPrefabs;
    
    public BulletLifeCycle(PoolObject<Bullet> pool, WeaponConfigs weaponConfigs, 
        Spread spread, DistributionConfigs distributionConfigs, CurrentWeapon currentWeapon, WeaponPrefabs weaponPrefabs)
    {
        _pool = pool;
        _weaponConfigs = weaponConfigs;
        _spread = spread;
        _distributionConfigs = distributionConfigs;
        _currentWeapon = currentWeapon;
        _weaponPrefabs = weaponPrefabs;
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
    
    
    public async void BulletSpawn()
    {
        _pool.AddElementsInPool("bullet", _weaponConfigs.BulletConfig.BulletPrefab , _gunConfig.TotalAmmo);
        Bullet bullet = _pool.GetElementInPool("bullet");
        bullet.Initialize(_gunConfig.TotalAmmo);
        Debug.Log(_weaponPrefabs.PrefabsWeapon[_gunConfig.Name].bulletSpawnPoint.transform.position);
        bullet.transform.position = _weaponPrefabs.PrefabsWeapon[_gunConfig.Name].bulletSpawnPoint.transform.position;
        bullet.transform.rotation = Quaternion.LookRotation(_weaponPrefabs.PrefabsWeapon[_gunConfig.Name].bulletSpawnPoint.transform.forward);
        await BulletLaunch(bullet);
    }

    private async UniTask BulletLaunch(Bullet bullet)
    {
        _bulletRigidbody = bullet.GetComponent<Rigidbody>();
        Vector3 velocity = _gunConfig.BulletPoint.transform.forward * _weaponConfigs.BulletConfig.BulletSpeed;
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
