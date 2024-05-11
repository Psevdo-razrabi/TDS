using MVVM;
using UI.Binders;
using Zenject;

namespace DI
{
    public sealed class BindersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BinderInstaller();
        }

        private void BinderInstaller()
        {
            BinderFactory.RegisterBinder<TextBinder>();
            BinderFactory.RegisterBinder<GameObjectBinder>();
            BinderFactory.RegisterBinder<ImageBinder>();
            BinderFactory.RegisterBinder<ButtonBinder>();
        }
    }
}