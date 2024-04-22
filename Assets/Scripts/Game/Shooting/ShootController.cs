using System;
using Unity.VisualScripting;
using Zenject;

public class ShootController : IDisposable
{
    private TestGun _gun;
    private CameraShake _shake;
    private Bullet _bullet;
    private PoolObject<Bullet> _pool;
    
    [Inject]
    public ShootController(TestGun gun, CameraShake shake, Bullet bullet)
    {
        _gun = gun;
        _shake = shake;
        _bullet = bullet;
        _gun.ShotFired += HandleShoot;
    }
    
    private void HandleShoot(CameraShakeConfig cameraShakeConfig,BulletConfig bulletConfig, GunConfig gunConfig)
    {
        _shake.ShakeCamera(cameraShakeConfig);
        
        
        _pool.AddElementsInPool("bullet",bulletConfig.BulletPrefab,gunConfig.CountBullet);
        Bullet bullet = _pool.GetElementInPool("bullet");
        /*
        bullet.Init(bulletConfig,gunConfig);
        */
    }
    
    public void Dispose()
    {
        _gun.ShotFired -= HandleShoot;
    }
}
