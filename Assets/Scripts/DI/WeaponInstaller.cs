using Game.Player.Weapons;
using Game.Player.Weapons.ChangeWeapon;
using Game.Player.Weapons.Mediators;
using Game.Player.Weapons.StrategyFire;
using Game.Player.Weapons.WeaponClass;
using Game.Player.Weapons.WeaponConfigs;
using Input;
using UnityEngine;
using UnityEngine.Serialization;

namespace DI
{
    public class WeaponInstaller : BaseBindings
    {
        [SerializeField] private Pistol pistol;
        [SerializeField] private ChangeModeFire fireMode;
        [SerializeField] private Crosshair _crosshair;
        [SerializeField] private ChangeCrosshair _changeCrosshair;
        
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
        }

        private void BindShootComponent()
<<<<<<< Updated upstream
        {
            BindNewInstance<BulletLifeCycle>();;
            BindNewInstance<CameraShake>();
            BindNewInstance<Recoil>();
            BindNewInstance<Spread>();
            BindNewInstance<ShootComponent>();
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
            BindNewInstance<CrosshairConfigs>();
=======
        {
            BindNewInstance<BulletLifeCycle>();;
            BindNewInstance<CameraShake>();
            BindNewInstance<Recoil>();
            BindNewInstance<Spread>();
            BindNewInstance<ShootComponent>();
            BindNewInstance<WeaponData>();
>>>>>>> Stashed changes
        }
        private void BindPool() => BindNewInstance<PoolObject<Bullet>>();

<<<<<<< Updated upstream
=======
        private void BindCursor()
        {
            BindInstance(_crosshair);
            BindInstance(_changeCrosshair);
        }
        private void BindConfigs()
        {
            BindNewInstance<WeaponConfigs>();
            BindNewInstance<CameraShakeConfigs>();
            BindNewInstance<CrosshairConfigs>();
        }
        private void BindPool() => BindNewInstance<PoolObject<Bullet>>();

>>>>>>> Stashed changes
        private void BindActionCleaner() => BindNewInstance<ActionsCleaner>();

        private void BindWeaponChange() => BindNewInstance<WeaponChange>();
        
        private void BindWeaponComponent()
        {
            BindNewInstance<FireComponent>();
            BindNewInstance<ReloadComponent>();
        }
        private void BindChangeFire() => BindInstance(fireMode);

        private void BindMediator() => BindNewInstance<MediatorFireStrategy>();
        
        private void BindWeapon()
        {
            Container.Bind<WeaponComponent>().To<Pistol>().FromInstance(pistol).AsSingle().NonLazy();
        }
    }
}