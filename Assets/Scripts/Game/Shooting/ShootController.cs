using System;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Player.Weapons.WeaponConfigs;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class ShootController : IDisposable
{
    private TestGun _gun;
    private PoolObject<Bullet> _pool;
    private CameraShakeConfig _cameraShakeConfig;
    private BulletConfig _bulletConfig;
    private RifleConfig _gunConfig;
    private Bullet _bullet;
    private Camera _camera;
    private Crosshair _crosshair;
    
    private float _currentSpread;
    private float _stepSpread;
    
        
    [Inject]
    public ShootController(Bullet bullet, TestGun gun, CameraShakeConfig shake, CameraShakeConfig cameraShakeConfig, BulletConfig bulletConfig, RifleConfig gunConfig,Camera camera, PoolObject<Bullet> pool, Crosshair crosshair)
    {
        _crosshair = crosshair;
        _bullet = bullet;
        _camera = camera;
        _gun = gun;
        _cameraShakeConfig = shake;
        _cameraShakeConfig = cameraShakeConfig;
        _bulletConfig = bulletConfig;
        _gunConfig = gunConfig;
        _pool = pool;
        _gun.ShotFired += HandleShoot;
        Start();
    }

    private void Start()
    {
        _stepSpread = _gunConfig.MaxSpread / _gunConfig.MaxSpreadBullet;
        _currentSpread = _stepSpread;
        _pool.AddElementsInPool("bullet",_bulletConfig.BulletPrefab,_gunConfig.TotalAmmo);

    }
    private void HandleShoot()
    {
        BulletSpawn();
        ShakeCamera();
        RecoilCursor();
    }

    private async void BulletSpawn()
    {
        Bullet bullet = _pool.GetElementInPool("bullet");
        bullet.transform.position = _gunConfig.BulletPoint.transform.position;
        await BulletLaunch(bullet);
    }

    private async UniTask BulletLaunch(Bullet bullet)
    {
        _bullet.Init(_gunConfig.TotalAmmo);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        Vector3 velocity = _gunConfig.BulletPoint.transform.forward * _bulletConfig.BulletSpeed;
        bulletRigidbody.velocity =  CalculatingSpread(velocity);
        ReturnBullet(bullet);
    }
    
    private Vector3 CalculatingSpread(Vector3 velocity)
    {
        float spreadX = Random.Range(-_currentSpread, _currentSpread);
        Vector3 velocityWithSpread = velocity + new Vector3(spreadX, 0, 0);
        _currentSpread += _stepSpread;
        return velocityWithSpread;
    }
    private void ShakeCamera()
    {
        _camera.transform.DOShakePosition(_cameraShakeConfig.ShakeDuration, _cameraShakeConfig.ShakeStrength, 1, 90f, false, true, ShakeRandomnessMode.Harmonic)
            .SetEase(Ease.InOutBounce).SetLink(_camera.gameObject);
    }

    private void RecoilCursor()
    {
        Vector2 recoil = new Vector2(_gunConfig.BulletPoint.transform.forward.x * Random.Range(-2f,2f), _gunConfig.BulletPoint.transform.forward.z) * _gunConfig.RecoilForce;
        _crosshair.RecoilPlus(recoil);
    }
    
    
    private async UniTask ReturnBullet(Bullet bullet)
    {
        await UniTask.Delay(2000);
        bullet.gameObject.SetActive(false);
    }
    


    public void Dispose()
    {
        _gun.ShotFired -= HandleShoot;
    }
}
