using UI.ViewModel;

namespace DI
{
    public sealed class ViewModelsInstaller : BaseBindings
    {
        public override void InstallBindings()
        {
            BindDashViewModel();
        }

        private void BindDashViewModel()
        {
            BindNewInstance<DashViewModel>();
            BindNewInstance<ReloadViewModel>();
            BindNewInstance<HealthPlayerViewModel>();
            BindNewInstance<HealthEnemyViewModel>();

            // Container.Bind
            //     .WithId("PlayerHealth")
            //     .AsTransient()
            //     .WithArguments("ValuePlayerHealth")
            //     .NonLazy();
            //     
            // Container.Bind(typeof(HealthPlayerViewModel),typeof(IDisposable),typeof(IInitializable))
            //     .WithId("EnemyHealth")
            //     .AsTransient()
            //     .WithArguments("ValueEnemyHealth")
            //     .NonLazy();

        }
    }
}