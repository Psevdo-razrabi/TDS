using System;
using Cysharp.Threading.Tasks;
using Game.Player.Interfaces;
using Game.Player.Weapons;
using Game.Player.Weapons.Commands.Recievers;
using Game.Player.Weapons.Prefabs;
using Game.Player.Weapons.WeaponClass;
using Game.Player.Weapons.WeaponConfigs;
using UI.Storage;
using UniRx;
using UnityEngine;
using Weapons.InterfaceWeapon;
using Zenject;

public class BulletLifeCycle : IConfigRelize, IInitializable
{
    private const float BULLET_LIFE_TIME = 2f;
    private CompositeDisposable _compositeDisposable = new();
    private PoolObject<Bullet> _pool;
    private readonly WeaponConfigs _weaponConfigs;
    private Spread _spread;
    private Rigidbody _bulletRigidbody;
    private BaseWeaponConfig _gunConfig;
    private DistributionConfigs _distributionConfigs;
    private CurrentWeapon _currentWeapon;
    private WeaponData _weaponData;
    private HeightCheck _heightCheck;
    private AimRay _aimRay;
    
    private Vector3? _hitPoint;
    private Vector3? _aimPoint;
    
    public BulletLifeCycle(PoolObject<Bullet> pool, WeaponConfigs weaponConfigs, 
        Spread spread, DistributionConfigs distributionConfigs, CurrentWeapon currentWeapon, WeaponData weaponData, HeightCheck heightCheck, AimRay aimRay)
    {
        _pool = pool;
        _weaponConfigs = weaponConfigs;
        _spread = spread;
        _distributionConfigs = distributionConfigs;
        _currentWeapon = currentWeapon;
        _weaponData = weaponData;
        _heightCheck = heightCheck;
        _aimRay = aimRay;
    }
    
    public void GetWeaponConfig(WeaponComponent weaponComponent)
    {
        _currentWeapon.LoadConfig(weaponComponent);
        _gunConfig = _currentWeapon.CurrentWeaponConfig;
    }

    public void Initialize()
    {
        _distributionConfigs.ClassesWantConfig.Add(this);
        _aimRay.AimPointUpdates
            .Subscribe(hitPoint =>
            {
                _aimPoint = hitPoint;
            })
            .AddTo(_compositeDisposable);
    }
    
    
    public async void BulletSpawn()
    {
        _pool.AddElementsInPool("bullet", _weaponConfigs.BulletConfig.BulletPrefab , _gunConfig.TotalAmmo);
        Bullet bullet = _pool.GetElementInPool("bullet");
        bullet.Init();

        bullet.transform.position = _weaponData.BulletPoint.position;
        bullet.transform.rotation = Quaternion.LookRotation(_weaponData.BulletPoint.forward);
        await BulletLaunch(bullet);
    }

    private async UniTask BulletLaunch(Bullet bullet)
    {
       
        Vector3? hitPoint = _heightCheck.CheckHeight();
        
        Vector3 directionHeight;
        
        if (hitPoint.HasValue)
        {
            Vector3 adjustedHitPoint = hitPoint.Value + new Vector3(0, 0.5f, 0);
            directionHeight = (adjustedHitPoint - _weaponData.BulletPoint.position).normalized;
        }
        else
        {
            directionHeight = _weaponData.BulletPoint.forward;
        }

        directionHeight = _aimPoint.HasValue ? (_aimPoint.Value - _weaponData.BulletPoint.position).normalized : directionHeight;
        
        _bulletRigidbody = bullet.GetComponent<Rigidbody>();
        Vector3 velocity = directionHeight * _weaponConfigs.BulletConfig.BulletSpeed;
        _bulletRigidbody.velocity = velocity + _spread.CalculatingSpread(velocity) + _spread.CalculateCrosshairSpread();
        await ReturnBullet(bullet);
    }
    private void StopBullet()
    {
        _bulletRigidbody.velocity = Vector3.zero;
    }
    
    private async UniTask ReturnBullet(Bullet bullet)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(BULLET_LIFE_TIME));
        bullet.gameObject.SetActive(false);
    }
}
