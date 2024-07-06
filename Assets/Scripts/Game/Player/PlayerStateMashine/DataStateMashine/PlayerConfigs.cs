using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Core.Health;
using Game.Player.PlayerStateMashine.Configs;
using UnityEngine;
using Zenject;

namespace Game.Player.PlayerStateMashine
{
    public class PlayerConfigs : IInitializable
    {
        private readonly Loader _loader;
        private readonly StorageAssetReference _storageAssetReference;
        private List<ScriptableObject> _playerConfigs = new();
        
        public PlayerMoveConfig BaseMove { get; private set; }
        public PlayerMoveConfig MoveWithAim { get; private set; }
        public PlayerDashConfig DashConfig { get; private set; }
        public PlayerHealthConfig HealthConfig { get; private set; }
        public PlayerFogOfWarConfig FowConfig { get; private set; }
        public CrouchAndStandConfig StandUpCrouch { get; private set; }
        public CrouchAndStandConfig SitDownCrouch { get; private set; }
        public CrouchMovement CrouchMovement { get; private set; }
        public AnyCrouchParameters CrouchParameters { get; private set; }
        public bool IsLoadAllConfig { get; private set; } = false;
        

        public PlayerConfigs(Loader loader, StorageAssetReference storageAssetReference)
        {
            _loader = loader;
            _storageAssetReference = storageAssetReference;
        }

        public async void Initialize()
        {
           _playerConfigs = await _loader.LoadAllResourcesUseLabel<ScriptableObject>(_storageAssetReference.PlayerScriptableObjectLabel);
           await LoadToAssetReference(); 
           DashConfig = _playerConfigs.FirstOrDefault(x => x is PlayerDashConfig) as PlayerDashConfig;
           HealthConfig = _playerConfigs.FirstOrDefault(x => x is PlayerHealthConfig) as PlayerHealthConfig;
           FowConfig = _playerConfigs.FirstOrDefault(x => x is PlayerFogOfWarConfig) as PlayerFogOfWarConfig;
           CrouchMovement = _playerConfigs.FirstOrDefault(x => x is CrouchMovement) as CrouchMovement;
           CrouchParameters = _playerConfigs.FirstOrDefault(x => x is AnyCrouchParameters) as AnyCrouchParameters;
           IsLoadAllConfig = true;
        }

        private async UniTask LoadToAssetReference()
        {
            BaseMove = await _loader.LoadResourcesUsingReference(_storageAssetReference.PlayerMove);
            MoveWithAim = await _loader.LoadResourcesUsingReference(_storageAssetReference.PlayerMoveInAim);
            StandUpCrouch = await _loader.LoadResourcesUsingReference(_storageAssetReference.StandUpCrouch);
            SitDownCrouch = await _loader.LoadResourcesUsingReference(_storageAssetReference.SitDownCrouch);
            await UniTask.Yield();
        }
    }
}