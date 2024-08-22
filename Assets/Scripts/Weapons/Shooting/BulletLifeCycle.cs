using System;
using CharacterOrEnemyEffect.Factory;
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
    private readonly Weapon _weapon;
    private CompositeDisposable _compositeDisposable = new();
    private FactoryComponentWithMonoBehaviour _factory;
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
    
    public BulletLifeCycle(Weapon weapon, FactoryComponentWithMonoBehaviour factory,
        Spread spread, DistributionConfigs distributionConfigs, CurrentWeapon currentWeapon, WeaponData weaponData, HeightCheck heightCheck, AimRay aimRay)
    {
        _factory = factory;
        _weapon = weapon;
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
        _factory.CreatePool<Bullet>(_weapon.BulletConfig.BulletPrefab);
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
        Bullet bullet = _factory.CreateWithPoolObject<Bullet>().Item2;
        bullet.Init();

        bullet.transform.position = _weaponData.BulletPoint.position;
        bullet.transform.rotation = Quaternion.LookRotation(_weaponData.BulletPoint.forward);
        await BulletLaunch(bullet);
    }

    private async UniTask BulletLaunch(Bullet bullet)
    {
        Vector3? hitPoint = _heightCheck.CheckHeight();
    
        Vector3 baseDirection = _weaponData.BulletPoint.forward;
    
        if (hitPoint.HasValue)
        {
            Vector3 adjustedHitPoint = hitPoint.Value + new Vector3(0, 0.5f, 0);
            baseDirection = (adjustedHitPoint - _weaponData.BulletPoint.position).normalized;
        }

        if (_aimPoint.HasValue)
        {
            baseDirection = (_aimPoint.Value - _weaponData.BulletPoint.position).normalized;
        }
        
        Vector3 spread = new Vector3();
        Vector3 finalDirection = (baseDirection + spread).normalized;

        Vector3 startPosition = _weaponData.BulletPoint.position;
        bullet.transform.position = startPosition;

        Vector3 velocity = finalDirection * _weapon.BulletConfig.BulletSpeed;

        _bulletRigidbody = bullet.GetComponent<Rigidbody>();
        _bulletRigidbody.velocity = velocity;

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
