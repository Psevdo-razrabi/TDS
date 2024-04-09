using Game.Core;
using UnityEngine;
using Zenject;

namespace DI
{
    public class GameInstaller : MonoInstaller
    {
        [field: SerializeField] public InputSystemPC InputSystemPC { get; private set; }
        
        public override void InstallBindings()
        { 
            BindInput();
        }

        private void BindInput()
        {
            BindNewInstance<InputSystem>();
            BindInstance(InputSystemPC);
        }

        private void BindNewInstance<T>() => Container
            .BindInterfacesAndSelfTo<T>()
            .AsSingle()
            .NonLazy();

        private void BindInstance<T>(T instance) =>
            Container
                .BindInterfacesAndSelfTo<T>()
                .FromInstance(instance)
                .AsSingle()
                .NonLazy();
    }
}