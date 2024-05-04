using Game.Player.Weapons;
using Game.Player.Weapons.ChangeWeapon;
using Game.Player.Weapons.Mediators;
using Game.Player.Weapons.StrategyFire;
using Game.Player.Weapons.WeaponClass;
using Input;
using UnityEngine;
using UnityEngine.Serialization;

namespace DI
{
    public class WeaponInstaller : BaseBindings
    {
        [SerializeField] private Pistol pistol;
        [SerializeField] private ChangeModeFire fireMode;
        
        public override void InstallBindings()
        {
            BindActionCleaner();
            BindWeaponComponent();
            BindMediator();
            BindChangeFire();
            BindWeapon();
            BindWeaponChange();
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