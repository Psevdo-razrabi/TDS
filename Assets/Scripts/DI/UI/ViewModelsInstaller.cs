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
            BindNewInstance<HealthViewModel>();
        }
    }
}