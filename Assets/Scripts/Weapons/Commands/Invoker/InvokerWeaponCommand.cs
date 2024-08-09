using System;
using System.Collections.Concurrent;
using CharacterOrEnemyEffect.Factory;
using UnityEngine;
using Zenject;

namespace Game.Player.Weapons.Commands.Invoker
{
    public class InvokerWeaponCommand : IInitializable, IDisposable
    {
        private readonly ConcurrentQueue<Command> _weaponCommands = new();
        private readonly FactoryComponent _factoryCommands;
        private event Action<WeaponComponent> InvokeCommands;
        private event Action<WeaponComponent> InvokeAimCommands;
        public InvokerWeaponCommand(FactoryComponent factoryCommands)
        {
            _factoryCommands = factoryCommands;
        }
        
        public void Dispose()
        {
            InvokeCommands -= CreateCommands;
            InvokeAimCommands -= CreateAimCommands;
        }

        public void Initialize()
        {
            InvokeCommands += CreateCommands;
            InvokeAimCommands += CreateAimCommands;
        }
        
        public void OnInvokeCommands(WeaponComponent weaponComponent) 
        {
            InvokeCommands?.Invoke(weaponComponent);
        }

        public void OnInvokeSwitchAimComand(WeaponComponent weaponComponent)
        {
            InvokeAimCommands?.Invoke(weaponComponent);
        }
        private void CreateCommands(WeaponComponent weaponComponent)
        {
            _weaponCommands.Enqueue(_factoryCommands.CreateWithDiContainer<InitializeConfigCommand>());
            _weaponCommands.Enqueue(_factoryCommands.CreateWithDiContainer<ChangeWeaponCommand>());
            _weaponCommands.Enqueue(_factoryCommands.CreateWithDiContainer<ChangePrefabWeapon>());
            _weaponCommands.Enqueue(_factoryCommands.CreateWithDiContainer<CommandSetFireMode>());
            _weaponCommands.Enqueue(_factoryCommands.CreateWithDiContainer<AudioWeaponCommand>());
            _weaponCommands.Enqueue(_factoryCommands.CreateWithDiContainer<ParticleWeaponComand>());
            var count = _weaponCommands.Count;

            for (int i = 0; i < count; i++)
                Invoke(weaponComponent);
        }
        private void CreateAimCommands(WeaponComponent weaponComponent)
        {
            _weaponCommands.Enqueue(_factoryCommands.CreateWithDiContainer<InitializeConfigCommand>());

            var count = _weaponCommands.Count;

            for (int i = 0; i < count; i++)
                Invoke(weaponComponent);
        }
        private async void Invoke(WeaponComponent weaponComponent)
        {
            if (_weaponCommands.TryDequeue(out var command))
                await command.Execute(weaponComponent);
        }
    }
}