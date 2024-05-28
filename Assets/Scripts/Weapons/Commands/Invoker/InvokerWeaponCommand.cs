﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Game.Player.Weapons.Commands.Factory;
using UnityEngine;
using Zenject;

namespace Game.Player.Weapons.Commands.Invoker
{
    public class InvokerWeaponCommand : IInitializable, IDisposable
    {
        private readonly ConcurrentQueue<Command> _weaponCommands = new();
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
            _weaponCommands.Enqueue(_factoryCommands.CreateCommand<InitializeConfigCommand>());
            _weaponCommands.Enqueue(_factoryCommands.CreateCommand<ChangeWeaponCommand>());
            _weaponCommands.Enqueue(_factoryCommands.CreateCommand<ChangePrefabWeapon>());
            _weaponCommands.Enqueue(_factoryCommands.CreateCommand<CommandSetFireMode>());

            var count = _weaponCommands.Count;

            for (int i = 0; i < count; i++)
                Invoke(weaponComponent);
            
            Debug.Log("Completed");
        }

        private async void Invoke(WeaponComponent weaponComponent)
        {
            if (_weaponCommands.TryDequeue(out var command))
                await command.Execute(weaponComponent);
        }
    }
}