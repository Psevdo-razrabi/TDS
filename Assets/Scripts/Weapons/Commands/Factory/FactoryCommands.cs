using Zenject;

namespace Game.Player.Weapons.Commands.Factory
{
    public class FactoryCommands
    {
        private DiContainer _diContainer;

        [Inject]
        public void Construct(DiContainer diContainer) => _diContainer = diContainer;

        public Command CreateCommand<T>() where T : Command
        {
            return _diContainer.Resolve<T>();
        }
    }
}