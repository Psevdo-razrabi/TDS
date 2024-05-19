using Game.AsyncWorker;
using Game.Player.Weapons;
using Input;
using UI.Storage;
using UI.View;
using UI.ViewModel;
using UnityEngine;

namespace DI
{
    public sealed class ModelInstaller : BaseBindings
    {
        [SerializeField] private GameObjectView gameObjectViewReload;
        [SerializeField] private GameObjectView gameObjectViewStorage;
        public override void InstallBindings()
        {
            BindModels();
        }

        private void BindModels()
        {
            BindNewInstance<ValueCountStorage<int>>();
            BindNewInstance<StorageModel>();
            BindBoolStorage();
            BindValue();
        }

        private void BindBoolStorage()
        {
            var reloadBool = CreateStorageBool();
            var storageBool = CreateStorageBool();
            Container.Bind<BoolStorage>().To<BoolStorage>().FromInstance(reloadBool).WhenInjectedIntoInstance(gameObjectViewReload);
            Container.Bind<BoolStorage>().To<BoolStorage>().FromInstance(reloadBool).WhenInjectedInto<ReloadComponent>();

            Container.Bind<BoolStorage>().To<BoolStorage>().FromInstance(storageBool).WhenInjectedIntoInstance(gameObjectViewStorage);
            Container.Bind<BoolStorage>().To<BoolStorage>().FromInstance(storageBool).WhenInjectedInto<InputSystemUi>();
        }

        private void BindValue()
        {
            var valueFromReload = CreateStorage();
            var valueFromDash = new ValueCountStorage<int>();
            Container.Bind<ValueCountStorage<float>>().To<ValueCountStorage<float>>().FromInstance(valueFromReload)
                .WhenInjectedInto<ReloadComponent>();
            Container.Bind<ValueCountStorage<float>>().To<ValueCountStorage<float>>().FromInstance(valueFromReload)
                .WhenInjectedInto<ReloadViewModel>();
            
            Container.Bind<ValueCountStorage<int>>().To<ValueCountStorage<int>>().FromInstance(valueFromDash)
                .WhenInjectedInto<AsyncWorker>();
            Container.Bind<ValueCountStorage<int>>().To<ValueCountStorage<int>>().FromInstance(valueFromDash)
                .WhenInjectedInto<DashViewModel>();
        }
        
        private ValueCountStorage<float> CreateStorage()
        {
            return new ValueCountStorage<float>();
        }
        
        private BoolStorage CreateStorageBool()
        {
            return new BoolStorage();
        }
    }
}