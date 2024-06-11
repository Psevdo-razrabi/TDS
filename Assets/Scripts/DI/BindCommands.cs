using CharacterOrEnemyEffect.Factory;
using Customs;
using Game.Player.Weapons.Commands;
using Game.Player.Weapons.Commands.Invoker;
using Game.Player.Weapons.Commands.Recievers;
using UnityEngine;

namespace DI
{
    public class BindCommands : BaseBindings
    {
        public override void InstallBindings()
        {
            BindRecievers();
            BindCommand();
            BindInvoker();
        }

        private void BindInvoker()
        {
            BindNewInstance<InvokerWeaponCommand>();
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