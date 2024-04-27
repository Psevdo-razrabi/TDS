using Customs.Data;
using SaveSystem;
using SaveSystem.Repositories;
using Zenject;

namespace DI
{
    public class DataInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindIdGenerator();
            BindJsonContext();
            BindUnitOfWorks();
            BindRepositories();
            BindMethodListEditor();
        }

        private void BindUnitOfWorks() => BindNewInstance<UnitOfWorks>();

        private void BindIdGenerator() => BindNewInstance<IdGenerator>();

        private void BindMethodListEditor() => BindNewInstance<MethodListEditor>();
        private void BindJsonContext() => Container.Bind<DataContext>().To<JsonDataContext>().AsSingle().NonLazy();

        private void BindRepositories()
        {
            Container.Bind<Repository<MethodName>>().To<MethodNamesRepository>().AsSingle().NonLazy();
            Container.Bind<Repository<MethodLabelName>>().To<MethodLabelsRepository>().AsSingle().NonLazy();
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