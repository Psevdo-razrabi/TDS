using System.Linq;
using Game.AsyncWorker.Interfaces;
using Game.Player.PlayerStateMashine.Configs;
using Game.Player.PlayerStateMashine.Configs.Providers;
using UnityEngine;
using Zenject;

namespace Game.Player.PlayerStateMashine
{
    public class PlayerConfigs : IInitializable, ILoadable
    {
        public bool IsLoaded { get; private set; }
        private readonly Loader _loader; 
        private readonly StorageAssetReference _storageAssetReference;

        public ObstacleConfigsProvider ObstacleConfigsProvider { get; private set; }
        public CrouchConfigsProvider CrouchConfigsProvider { get; private set; }
        public MovementConfigsProvider MovementConfigsProvider { get; private set; }
        public AnyPlayerConfigsProvider AnyPlayerConfigs { get; private set; }
        
        public PlayerConfigs(Loader loader, StorageAssetReference storageAssetReference)
        {
            _loader = loader;
            _storageAssetReference = storageAssetReference;
        }

        public async void Initialize()
        {
           var resources = await _loader.LoadAllResourcesUseLabel<ScriptableObject>(_storageAssetReference.PlayerScriptableObjectLabel);
           
           ObstacleConfigsProvider = resources.resources.FirstOrDefault(x => x is ObstacleConfigsProvider) as ObstacleConfigsProvider;
           CrouchConfigsProvider = resources.resources.FirstOrDefault(x => x is CrouchConfigsProvider) as CrouchConfigsProvider;
           MovementConfigsProvider = resources.resources.FirstOrDefault(x => x is MovementConfigsProvider) as MovementConfigsProvider;
           AnyPlayerConfigs = resources.resources.FirstOrDefault(x => x is AnyPlayerConfigsProvider) as AnyPlayerConfigsProvider;
           
           IsLoaded = true;
        }
    }
}