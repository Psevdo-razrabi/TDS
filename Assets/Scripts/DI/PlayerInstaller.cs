using Game.Player;
using Game.Player.Weapons;
using Game.Player.Weapons.FrozenWeapon;
using Game.Player.Weapons.Suriken;
using UnityEngine;
using Zenject;

namespace DI
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private SurikenWeapon _surikenWeapon;
        [SerializeField] private RadiationWeapon _radiationWeapon;
        [SerializeField] private FrozenWeapon _frozenWeapon;
        public override void InstallBindings()
        {
            Container.Bind<PlayerMovement>().FromInstance(_playerMovement).AsSingle().NonLazy();
            Container.Bind<PlayerHealth>().FromInstance(_playerHealth).AsSingle().NonLazy();
            Container.Bind<SurikenWeapon>().FromInstance(_surikenWeapon).AsSingle().NonLazy();
            Container.Bind<RadiationWeapon>().FromInstance(_radiationWeapon).AsSingle().NonLazy();
            Container.Bind<FrozenWeapon>().FromInstance(_frozenWeapon).AsSingle().NonLazy();
        }
    }
}