using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.AsyncWorker.Interfaces;
using Game.Player.Weapons.Prefabs;
using Weapons.InterfaceWeapon;

namespace Game.Player.Weapons.Commands.Recievers
{
    public class DistributionConfigs
    {
        private readonly IAwaiter _awaiter;
        private readonly WeaponConfigs.Weapon _weapon;
        private readonly CameraShakeConfigs _shake;
        private readonly WeaponPrefabs _weaponPrefabs;
        public List<IConfigRelize> ClassesWantConfig { get; private set; } = new();

        public DistributionConfigs(IAwaiter awaiter, WeaponConfigs.Weapon weapon, CameraShakeConfigs shake, WeaponPrefabs weaponPrefabs)
        {
            _awaiter = awaiter;
            _weapon = weapon;
            _shake = shake;
            _weaponPrefabs = weaponPrefabs;
        }
        
        public async UniTask Distribution(WeaponComponent weaponComponent)
        {
            await UniTask.WhenAll(new[] { _awaiter.AwaitLoadConfigs(_weapon),
                _awaiter.AwaitLoadConfigs(_shake),
                _awaiter.AwaitLoadConfigs(_weaponPrefabs)
            });
            
            ClassesWantConfig
                .ForEach(x => x.GetWeaponConfig(weaponComponent));
        }
    }
}