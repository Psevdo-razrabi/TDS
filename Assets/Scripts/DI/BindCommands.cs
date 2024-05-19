using Customs;
using Game.Player.Weapons.Commands;
using Game.Player.Weapons.Commands.Factory;
using Game.Player.Weapons.Commands.Invoker;
using Game.Player.Weapons.Commands.Recievers;

namespace DI
{
    public class BindCommands : BaseBindings
    {
        public override void InstallBindings()
        {
            BindFactoryCommand();
            BindRecievers();
            BindCommand();
            BindInvoker();
        }

        private void BindInvoker()
        {
            BindNewInstance<InvokerWeaponCommand>();
        }

        private void BindFactoryCommand()
        {
            BindNewInstance<FactoryCommands>();
            BindNewInstance<FactoryWeapon>();
        }

        private void BindCommand()
        {
            BindNewInstance<ChangePrefabWeapon>();
            BindNewInstance<ChangeWeaponCommand>();
            BindNewInstance<CommandSetFireMode>();
            BindNewInstance<InitializeConfigCommand>();
        }

        private void BindRecievers()
        {
            BindNewInstance<DistributionConfigs>();
            BindNewInstance<WeaponChanger>();
            BindNewInstance<SetFireMode>();
        }
    }
}