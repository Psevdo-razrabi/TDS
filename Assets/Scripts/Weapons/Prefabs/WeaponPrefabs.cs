using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.AsyncWorker.Interfaces;
using UnityEngine;
using Weapons;
using Zenject;

namespace Game.Player.Weapons.Prefabs
{
    public class WeaponPrefabs : ILoadable
    {
        public IReadOnlyDictionary<string, (GameObject weapon, GameObject bulletSpawnPoint)> PrefabsWeapon { get;
            private set;
        }
        
        public Dictionary<string, GameObject> InitPrefab { get; set; }
        
        public bool IsLoaded { get; private set; }

        public readonly string NameLoadPistolPrefab = "Pistol";
        public readonly string NameLoadRiflePrefab = "Rifle";
        public readonly string NameLoadShotgunPrefab = "Shotgun";
        public GameObject CurrentPrefabWeapon { get; set; }
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
            InitDictionary(new [] { pistolPrefab.resources, riflePrefab.resources, shotgunPrefab.resources });
            
            IsLoaded = true;
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