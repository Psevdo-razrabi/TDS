using Cysharp.Threading.Tasks;
using Game.Player.PlayerStateMashine.Configs;
using UnityEngine;
using Zenject;

namespace Game.Player.PlayerStateMashine
{
    public class PlayerConfigs
    {
        private Loader _loader;
        
        public PlayerMoveConfig BaseMove { get; private set; }
        public PlayerMoveConfig MoveWithAim { get; private set; }
        public PlayerDashConfig DashConfig { get; private set; }
        
        public bool IsLoadMoveConfig { get; private set; }
        public bool IsLoadMoveAimConfig { get; private set; }
        public bool IsLoadDashConfig { get; private set; }

        private const string NameBaseMoveConfig = "Move";
        private const string NameMoveWithAimConfig = "MoveStateAim";
        private const string NameDashConfig = "Dash";
            
        public async UniTask AwaitLoadConfig()
        {
            while (!IsLoadDashConfig)
                await UniTask.Yield();
        }
        
        [Inject]
        private async void Construct(Loader loader)
        {
            _loader = loader;
            
            BaseMove = await _loader.LoadResources<ScriptableObject>(NameBaseMoveConfig) as PlayerMoveConfig;
            IsLoadMoveConfig = true;
            MoveWithAim = await _loader.LoadResources<ScriptableObject>(NameMoveWithAimConfig) as PlayerMoveConfig;
            IsLoadMoveAimConfig = true;
            DashConfig = await _loader.LoadResources<ScriptableObject>(NameDashConfig) as PlayerDashConfig;
            IsLoadDashConfig = true;
        }
    }
}