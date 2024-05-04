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
        [SerializeField] private RifleConfig _rifle;
        [SerializeField] private CameraShakeConfig _cameraShake;
        [SerializeField] private BulletConfig _bulletCFG;

        public override void InstallBindings()
        {
            BindWeapons();
            BindActionCleaner();
            BindWeaponComponent();
            BindMediator();
            BindChangeFire();
            BindWeapon();
            BindWeaponChange();
        }

        private void BindWeapons()
        {
            Container.Bind<RifleConfig>().FromInstance(_rifle).AsSingle();
            Container.Bind<CameraShakeConfig>().FromInstance(_cameraShake).AsSingle();
            Container.Bind<BulletConfig>().FromInstance(_bulletCFG).AsSingle();
            Container.BindInterfacesAndSelfTo<PoolObject<Bullet>>().AsSingle();
        }
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