using Game.AsyncWorker;
using Game.Player;
using Game.Player.Weapons;
using UI.Storage;
using UI.ViewModel;

namespace DI
{
    public sealed class ModelInstaller : BaseBindings
    {
        public override void InstallBindings()
        {
            BindModels();
        }

        private void BindModels()
        {
            BindNewInstance<ValueCountStorage<int>>();
            BindNewInstance<BoolStorage>();
            Bind();
        }

        private void Bind()
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
    }
}