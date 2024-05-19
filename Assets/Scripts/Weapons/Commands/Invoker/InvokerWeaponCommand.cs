using System;
using System.Collections.Generic;
using Game.Player.Weapons.Commands.Factory;
using Zenject;

namespace Game.Player.Weapons.Commands.Invoker
{
    public class InvokerWeaponCommand : IInitializable, IDisposable
    {
        private readonly List<Command> _weaponCommands = new();
        private readonly FactoryCommands _factoryCommands;
        private event Action<WeaponComponent> InvokeCommands;

        public InvokerWeaponCommand(FactoryCommands factoryCommands)
        {
            _factoryCommands = factoryCommands;
        }
        
        public void Dispose()
        {
            InvokeCommands -= CreateCommands;
        }

        public void Initialize()
        {
            InvokeCommands += CreateCommands;
        }
        
        public void OnInvokeCommands(WeaponComponent weaponComponent)
        {
            InvokeCommands?.Invoke(weaponComponent);
        }

        private void CreateCommands(WeaponComponent weaponComponent)
        {
            _weaponCommands.Add(_factoryCommands.CreateCommand<InitializeConfigCommand>());
            _weaponCommands.Add(_factoryCommands.CreateCommand<ChangeWeaponCommand>());
            _weaponCommands.Add(_factoryCommands.CreateCommand<ChangePrefabWeapon>());
            _weaponCommands.Add(_factoryCommands.CreateCommand<CommandSetFireMode>());
            
            Invoke(weaponComponent);
        }

        private async void Invoke(WeaponComponent weaponComponent)
        {
            foreach (var command in _weaponCommands)
                await command.Execute(weaponComponent);
        }
    }
}