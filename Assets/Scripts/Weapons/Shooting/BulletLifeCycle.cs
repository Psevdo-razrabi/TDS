using System;
using Cysharp.Threading.Tasks;
using Game.Player.Weapons.WeaponConfigs;
using UnityEngine;

public class BulletLifeCycle
{
    private const float BulletLifeTime = 2f;
        
    private PoolObject<Bullet> _pool;
    private readonly WeaponConfigs _weaponConfigs;
    private EventController _eventController;
    
    private BulletConfig _bulletConfig;
    private RifleConfig _gunConfig;
    private Spread _spread;
    private Rigidbody _bulletRigidbody;
    
    public BulletLifeCycle(PoolObject<Bullet> pool,WeaponConfigs weaponConfigs, Spread spread, EventController eventController)
    {
        _pool = pool;
        _weaponConfigs = weaponConfigs;
        _spread = spread;
        _eventController = eventController;
        _eventController.BulletStoped += StopBullet;
        LoadConfigs();
    }

    private async void LoadConfigs()
    {
        while (_weaponConfigs.IsLoadConfigs == false)
            await UniTask.Yield();

        _bulletConfig = _weaponConfigs.BulletConfig;
        _gunConfig = _weaponConfigs.RifleConfig;
    }
    
    public async void BulletSpawn()
    {
        _pool.AddElementsInPool("bullet", _bulletConfig.BulletPrefab, _gunConfig.TotalAmmo);
        Bullet bullet = _pool.GetElementInPool("bullet");
        bullet.Initialize(_gunConfig.TotalAmmo);
        bullet.transform.position = _gunConfig.BulletPoint.transform.position;
        bullet.transform.rotation = Quaternion.LookRotation(_gunConfig.BulletPoint.transform.forward);
        await BulletLaunch(bullet);
    }

    private async UniTask BulletLaunch(Bullet bullet)
    {
        _bulletRigidbody = bullet.GetComponent<Rigidbody>();
        Vector3 velocity = _gunConfig.BulletPoint.transform.forward * _bulletConfig.BulletSpeed;
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
