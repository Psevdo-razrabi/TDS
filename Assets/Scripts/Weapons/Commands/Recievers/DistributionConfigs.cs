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
        private readonly WeaponConfigs.WeaponConfigs _weaponConfigs;
        private readonly CameraShakeConfigs _shakeConfigs;
        private readonly WeaponPrefabs _weaponPrefabs;
        public List<IConfigRelize> ClassesWantConfig { get; private set; } = new();

        public DistributionConfigs(IAwaiter awaiter, WeaponConfigs.WeaponConfigs weaponConfigs, CameraShakeConfigs shakeConfigs, WeaponPrefabs weaponPrefabs)
        {
            _awaiter = awaiter;
            _weaponConfigs = weaponConfigs;
            _shakeConfigs = shakeConfigs;
            _weaponPrefabs = weaponPrefabs;
        }
        
        public async UniTask Distribution(WeaponComponent weaponComponent)
        {
            await UniTask.WhenAll(new[] { _awaiter.AwaitLoadWeaponConfigs(_weaponConfigs),
                _awaiter.AwaitLoadShakeCameraConfigs(_shakeConfigs),
                _awaiter.AwaitLoadPrefabConfigs(_weaponPrefabs)
            });
            ClassesWantConfig
                .ForEach(x => x.GetWeaponConfig(weaponComponent));
        }
    }
}