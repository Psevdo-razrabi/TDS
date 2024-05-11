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
            var valueFromHealth = CreateStorage();
            Container.Bind<ValueCountStorage<float>>().To<ValueCountStorage<float>>().FromInstance(valueFromReload)
                .WhenInjectedInto<ReloadComponent>();
            Container.Bind<ValueCountStorage<float>>().To<ValueCountStorage<float>>().FromInstance(valueFromReload)
                .WhenInjectedInto<ReloadViewModel>();
            
            Container.Bind<ValueCountStorage<float>>().To<ValueCountStorage<float>>().FromInstance(valueFromHealth)
                .WhenInjectedInto<Player>();
            Container.Bind<ValueCountStorage<float>>().To<ValueCountStorage<float>>().FromInstance(valueFromHealth)
                .WhenInjectedInto<HealthViewModel>();
        }
        
        private ValueCountStorage<float> CreateStorage()
        {
            return new ValueCountStorage<float>();
        }
    }
}