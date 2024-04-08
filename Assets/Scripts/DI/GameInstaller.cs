using Game.Core;
using UnityEngine;
using Zenject;

namespace DI
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private RandomSpawnPoint _randomSpawnPoint;
        public override void InstallBindings()
        {
            Container.Bind<RandomSpawnPoint>().FromInstance(_randomSpawnPoint).AsSingle().NonLazy();
        }
    }
}