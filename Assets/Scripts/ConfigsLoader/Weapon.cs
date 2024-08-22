using Cysharp.Threading.Tasks;
using Game.AsyncWorker.Interfaces;
using UnityEngine;
using Zenject;

namespace Game.Player.Weapons.WeaponConfigs
{
    public class Weapon : ILoadable
    {
        public PistolConfig PistolConfig { get; private set; }
        public RifleConfig RifleConfig { get; private set; }
        public ShotgunConfig ShotgunConfig { get; private set; }
        public PistolConfig PistolAimConfig { get; private set; }
        public RifleConfig RifleAimConfig { get; private set; }
        public ShotgunConfig ShotgunAimConfig { get; private set; }
        public BulletConfig BulletConfig { get; private set; }
        
        public bool IsLoaded { get; private set; }

        private const string NameLoadPistolConfig = "PistolConfig";
        private const string NameLoadRifleConfig = "RifleConfig";
        private const string NameLoadShotgunConfig = "ShotgunConfig";
        private const string NameLoadPistolAimConfig = "PistolAimConfig";
        private const string NameLoadRifleAimConfig = "RifleAimConfig";
        private const string NameLoadShotgunAimConfig = "ShotgunAimConfig";
        private const string NameLoadBulletConfig = "Bullet";
        
        private Loader _loader;
            
        [Inject]
        private async void Construct(Loader loader)
        {
            _loader = loader;
            await LoadConfigs();
        }
        
        private async UniTask LoadConfigs()
        {
            BulletConfig = (await _loader.LoadResources<ScriptableObject>(NameLoadBulletConfig)).resources as BulletConfig;
            PistolConfig = (await _loader.LoadResources<ScriptableObject>(NameLoadPistolConfig)).resources as PistolConfig;
            RifleConfig = (await _loader.LoadResources<ScriptableObject>(NameLoadRifleConfig)).resources as RifleConfig;
            ShotgunConfig = (await _loader.LoadResources<ScriptableObject>(NameLoadShotgunConfig)).resources as ShotgunConfig;
            PistolAimConfig = (await _loader.LoadResources<ScriptableObject>(NameLoadPistolAimConfig)).resources as PistolConfig;
            RifleAimConfig = (await _loader.LoadResources<ScriptableObject>(NameLoadRifleAimConfig)).resources as RifleConfig;
            ShotgunAimConfig = (await _loader.LoadResources<ScriptableObject>(NameLoadShotgunAimConfig)).resources as ShotgunConfig;

            IsLoaded = true;
        }
    }
}