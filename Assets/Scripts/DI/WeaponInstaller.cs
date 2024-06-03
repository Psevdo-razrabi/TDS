using Game.Player.Weapons;
using Game.Player.Weapons.ChangeWeapon;
using Game.Player.Weapons.Mediators;
using Game.Player.Weapons.Prefabs;
using Game.Player.Weapons.ReloadStrategy;
using Game.Player.Weapons.StrategyFire;
using Game.Player.Weapons.WeaponClass;
using Game.Player.Weapons.WeaponConfigs;
using Input;
using UnityEngine;

namespace DI
{
    public class WeaponInstaller : BaseBindings
    {
        [SerializeField] private ChangeModeFire fireMode;
        [SerializeField] private Crosshair _crosshair;
        [SerializeField] private ChangeCrosshair _changeCrosshair;
        [SerializeField] private WeaponPivots _weaponPivots;

        public override void InstallBindings()
        {
            BindCursor();
            BindPool();
            BindShootComponent();
            BindActionCleaner();
            BindWeaponComponent();
            BindMediator();
            BindChangeFire();
            BindWeapon();
            BindWeaponChange();
            BindConfigs();
            BindCurrentWeapon();
            BindWeaponPrefab();
        }

        private void BindWeaponPrefab()
        {
            BindNewInstance<WeaponPrefabs>();
        }

        private void BindShootComponent()
        {
            BindNewInstance<BulletLifeCycle>();;
            BindNewInstance<CameraShake>();
            BindNewInstance<Recoil>();
            BindNewInstance<Spread>();
            BindNewInstance<ShootComponent>();
            BindNewInstance<WeaponData>();
        }

        private void BindCursor()
        {
            BindInstance(_crosshair);
            BindInstance(_changeCrosshair);
        }
        
        private void BindConfigs()
        {
            BindNewInstance<WeaponConfigs>();
            BindNewInstance<CameraShakeConfigs>();
        }
        private void BindPool() => BindNewInstance<PoolObject<Bullet>>();

        private void BindActionCleaner() => BindNewInstance<ActionsCleaner>();

        private void BindWeaponChange()
        {
            BindNewInstance<WeaponChange>();
            BindInstance(_weaponPivots);
        }
        
        private void BindWeaponComponent()
        {
            BindNewInstance<FireComponent>();
            BindNewInstance<ReloadComponent>();
            BindNewInstance<ReloadImage>();
        }
        private void BindChangeFire() => BindInstance(fireMode);

        private void BindMediator() => BindNewInstance<MediatorFireStrategy>();
        
        private void BindWeapon()
        {
            Container.BindInterfacesAndSelfTo<Pistol>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<Rifle>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<Shotgun>().AsSingle().NonLazy();
        }

        private void BindCurrentWeapon() => Container.BindInterfacesAndSelfTo<CurrentWeapon>().AsSingle().NonLazy();
    }
}