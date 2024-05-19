using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Game.Player.Weapons.Prefabs
{
    public class WeaponPrefabs
    {
        public IReadOnlyDictionary<string, (GameObject weapon, GameObject bulletSpawnPoint)> PrefabsWeapon { get;
            private set;
        }
        public bool IsLoadConfigs { get; private set; }

        public readonly string NameLoadPistolPrefab = "PistolPrefab";
        public readonly string NameLoadRiflePrefab = "RiflePrefab";
        public readonly string NameLoadShotgunPrefab = "ShotgunPrefab";
        private Dictionary<string, (GameObject weapon, GameObject bulletSpawnPoint)> _prefabsWeapon;
        
        private Loader _loader;
            
        [Inject]
        private async void Construct(Loader loader)
        {
            _loader = loader;
            await LoadPrefabs();
        }
        
        private async UniTask LoadPrefabs()
        {
            var pistolPrefab = await _loader.LoadResources<GameObject>(NameLoadPistolPrefab);
            var riflePrefab = await _loader.LoadResources<GameObject>(NameLoadRiflePrefab);
            var shotgunPrefab = await _loader.LoadResources<GameObject>(NameLoadShotgunPrefab);
            InitDictionary(new [] { pistolPrefab, riflePrefab, shotgunPrefab });
            
            IsLoadConfigs = true;
        }

        private void InitDictionary(IReadOnlyList<GameObject> prefabsWeapon)
        {
            _prefabsWeapon = new()
            {
                { NameLoadPistolPrefab, (prefabsWeapon[0], prefabsWeapon[0].GetComponentInChildren<BulletSpawnPoint>().gameObject) },
                { NameLoadRiflePrefab, (prefabsWeapon[1], prefabsWeapon[1].GetComponentInChildren<BulletSpawnPoint>().gameObject) },
                { NameLoadShotgunPrefab, (prefabsWeapon[2], prefabsWeapon[2].GetComponentInChildren<BulletSpawnPoint>().gameObject) }
            };
            PrefabsWeapon = new Dictionary<string, (GameObject weapon, GameObject bulletSpawnPoint)>(_prefabsWeapon);
        }
    }
}