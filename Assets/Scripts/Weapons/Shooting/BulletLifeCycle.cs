using System;
using Cysharp.Threading.Tasks;
using Game.Player.Weapons.WeaponConfigs;
using UnityEngine;
using Zenject;

public class BulletLifeCycle
{
    private const float BulletLifeTime = 2f;
        
    private PoolObject<Bullet> _pool;
    private readonly WeaponConfigs _weaponConfigs;
    
    private BulletConfig _bulletConfig;
    private RifleConfig _gunConfig;
    private Spread _spread;
    
    public BulletLifeCycle(PoolObject<Bullet> pool,WeaponConfigs weaponConfigs, Spread spread)
    {
        _pool = pool;
        _weaponConfigs = weaponConfigs;
        _spread = spread;
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
        await BulletLaunch(bullet);
    }

    private async UniTask BulletLaunch(Bullet bullet)
    {
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        Vector3 velocity = _gunConfig.BulletPoint.transform.forward * _bulletConfig.BulletSpeed;
        bulletRigidbody.velocity = velocity + _spread.CalculatingSpread(velocity);
        await ReturnBullet(bullet);
    }
    
    private async UniTask ReturnBullet(Bullet bullet)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(BulletLifeTime));
        bullet.gameObject.SetActive(false);
    }
}
