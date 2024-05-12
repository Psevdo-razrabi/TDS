using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Game.Player.Weapons.WeaponConfigs
{
    public class WeaponConfigs
    {
        public PistolConfig PistolConfig { get; private set; }
        
        public bool IsLoadPistolConfig { get; private set; }

        private const string NameLoadPistolConfig = "Pistol";
        
        private Loader _loader;
        
        [Inject]
        private async void Construct(Loader loader)
        {
            _loader = loader;

            await LoadConfigs();
        }
        
        private async UniTask LoadConfigs()
        {
            PistolConfig = await _loader.LoadResources<ScriptableObject>(NameLoadPistolConfig) as PistolConfig;
            IsLoadPistolConfig = true;
        }
    }
}