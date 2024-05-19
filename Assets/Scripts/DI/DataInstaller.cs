using Customs.Data;
using SaveSystem;
using SaveSystem.Repositories;
using Zenject;

namespace DI
{
    public sealed class DataInstaller : BaseBindings
    {
        public override void InstallBindings()
        {
            BindIdGenerator();
            BindJsonContext();
            BindUnitOfWorks();
            BindRepositories();
        }

        private void BindUnitOfWorks() => BindNewInstance<UnitOfWorks>();

        private void BindIdGenerator() => BindNewInstance<IdGenerator>();
        private void BindJsonContext() => Container.Bind<DataContext>().To<JsonDataContext>().AsSingle().NonLazy();

        private void BindRepositories()
        {
            Container.Bind<Repository<MethodName>>().To<MethodNamesRepository>().AsSingle().NonLazy();
            Container.Bind<Repository<MethodLabelName>>().To<MethodLabelsRepository>().AsSingle().NonLazy();
        }
    }
}