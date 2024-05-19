using Game.AsyncWorker.Interfaces;
using Game.Player.Weapons.Commands.Factory;
using Game.Player.Weapons.Prefabs;
using Game.Player.Weapons.WeaponClass;
using Game.Player.Weapons.WeaponConfigs;
using Sirenix.Utilities;
using Weapons.InterfaceWeapon;
using Zenject;

namespace Game.Player.Weapons.Commands.Recievers
{
    public class WeaponChanger : IInitializable, IVisitWeaponType
    {
        private WeaponPrefabs _weaponPrefabs;
        private readonly WeaponPivots _weaponPivots;
        private readonly FactoryWeapon _factoryWeapon;
        private readonly IAwaiter _awaiter;

        public WeaponChanger(WeaponPrefabs weaponPrefabs, WeaponPivots weaponPivots, FactoryWeapon factoryWeapon, IAwaiter awaiter)
        {
            _weaponPrefabs = weaponPrefabs;
            _weaponPivots = weaponPivots;
            _factoryWeapon = factoryWeapon;
            _awaiter = awaiter;
        }

        public void WeaponChange(WeaponComponent weaponComponent)
        {
            _weaponPrefabs.PrefabsWeapon
                .ForEach(x => x.Value.weapon.SetActive(false));
            VisitWeapon(weaponComponent);
        }

        public async void Initialize()
        {
            await _awaiter.AwaitLoadPrefabConfigs(_weaponPrefabs);
            
            var pistol = _factoryWeapon.CreateWeapon(_weaponPrefabs.PrefabsWeapon[_weaponPrefabs.NameLoadPistolPrefab].weapon);
            var rifle = _factoryWeapon.CreateWeapon(_weaponPrefabs.PrefabsWeapon[_weaponPrefabs.NameLoadRiflePrefab].weapon);
            var shotgun = _factoryWeapon.CreateWeapon(_weaponPrefabs.PrefabsWeapon[_weaponPrefabs.NameLoadShotgunPrefab].weapon);
            
            pistol.transform.SetParent(_weaponPivots.PistolPivot.transform);
            rifle.transform.SetParent(_weaponPivots.RiflePivot.transform);
            shotgun.transform.SetParent(_weaponPivots.ShotgunPivot.transform);
            
            pistol.SetActive(false);
            rifle.SetActive(false);
            shotgun.SetActive(false);
        }

        public void Visit(Pistol pistol)
        {
            _weaponPrefabs.PrefabsWeapon[_weaponPrefabs.NameLoadPistolPrefab].weapon.SetActive(true);
        }

        public void Visit(Rifle rifle)
        {
            _weaponPrefabs.PrefabsWeapon[_weaponPrefabs.NameLoadRiflePrefab].weapon.SetActive(true);
        }

        public void Visit(Shotgun shotgun)
        {
            _weaponPrefabs.PrefabsWeapon[_weaponPrefabs.NameLoadShotgunPrefab].weapon.SetActive(true);
        }

        public void VisitWeapon(WeaponComponent component)
        {
            Visit((dynamic)component);
        }
    }
}