using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Game.Player.Weapons.WeaponConfigs
{
    public class WeaponConfigs
    {
        public RifleConfig PistolConfig { get; private set; }
        public RifleConfig RifleConfig { get; private set; }
        public BulletConfig BulletConfig { get; private set; }
        public bool IsLoadConfigs { get; private set; }

        private const string NameLoadPistolConfig = "Pistol";
        private const string NameLoadRifleConfig = "Rifle";
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
            BulletConfig = await _loader.LoadResources<ScriptableObject>(NameLoadBulletConfig) as BulletConfig;
            PistolConfig = await _loader.LoadResources<ScriptableObject>(NameLoadPistolConfig) as RifleConfig;
            RifleConfig = await _loader.LoadResources<ScriptableObject>(NameLoadRifleConfig) as RifleConfig;
            
            IsLoadConfigs = true;
        }
    }
}