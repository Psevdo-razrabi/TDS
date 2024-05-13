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
            var valueFromHealthPlayer = CreateStorage();
            var valueFromDash = new ValueCountStorage<int>();
            var valueHealthToEnemy = CreateStorage();
            Container.Bind<ValueCountStorage<float>>().To<ValueCountStorage<float>>().FromInstance(valueFromReload)
                .WhenInjectedInto<ReloadComponent>();
            Container.Bind<ValueCountStorage<float>>().To<ValueCountStorage<float>>().FromInstance(valueFromReload)
                .WhenInjectedInto<ReloadViewModel>();
            
            Container.Bind<ValueCountStorage<float>>().To<ValueCountStorage<float>>().FromInstance(valueFromHealthPlayer)
                .WhenInjectedInto<Player>();
            Container.Bind<ValueCountStorage<float>>().To<ValueCountStorage<float>>().FromInstance(valueFromHealthPlayer)
                .WhenInjectedInto<HealthPlayerViewModel>();
            
            Container.Bind<ValueCountStorage<int>>().To<ValueCountStorage<int>>().FromInstance(valueFromDash)
                .WhenInjectedInto<AsyncWorker>();
            Container.Bind<ValueCountStorage<int>>().To<ValueCountStorage<int>>().FromInstance(valueFromDash)
                .WhenInjectedInto<DashViewModel>();
            
            Container.Bind<ValueCountStorage<float>>().To<ValueCountStorage<float>>().FromInstance(valueHealthToEnemy)
                .WhenInjectedInto<Enemy.Enemy>();
            Container.Bind<ValueCountStorage<float>>().To<ValueCountStorage<float>>().FromInstance(valueHealthToEnemy)
                .WhenInjectedInto<HealthEnemyViewModel>();
        }
        
        private ValueCountStorage<float> CreateStorage()
        {
            return new ValueCountStorage<float>();
        }
    }
}