using System;
using Game.Player.PlayerStateMashine;
using Game.Player.Weapons.Decorator;
using Game.Player.Weapons.InterfaceWeapon;
using Game.Player.Weapons.InterfaseWeapon;
using Game.Player.Weapons.StrategyFire;
using Game.Player.Weapons.WeaponConfigs;
using Input;
using UnityEngine;

namespace Game.Player.Weapons
{
    public class FireComponent : IFireMediator, IFire
    {
        private FireStrategy _fireStrategy;
        private StateMachineData _stateMachineData;
        public readonly MouseInputObserver MouseInputObserver;
        public readonly ActionsCleaner ActionsCleaner;
        public readonly WeaponData WeaponData;
        public readonly WeaponConfigs.WeaponConfigs WeaponConfigs;
        public readonly CurrentWeapon CurrentWeapon;
        public Action ShotFired;
        
        public FireComponent(WeaponData weaponData, MouseInputObserver mouseInputObserver, ActionsCleaner actionsCleaner, WeaponConfigs.WeaponConfigs weaponConfigs, CurrentWeapon currentWeapon,StateMachineData stateMachineData)
        {
            _stateMachineData = stateMachineData;
            WeaponData = weaponData;
            MouseInputObserver = mouseInputObserver;
            ActionsCleaner = actionsCleaner;
            WeaponConfigs = weaponConfigs;
            CurrentWeapon = currentWeapon;
        }
        
        public void FireBullet()
        {
            var fireAction = new FireBulletAction(WeaponData, _fireStrategy,() => ShotFired?.Invoke());
            var handler = new HandlerDecoratorActions(() => WeaponData.IsReloading == false && _stateMachineData.IsDashing == false, fireAction);
            handler.Execute();
        }
        
        public void Fire()
        {
            _fireStrategy.Fire(this);
        }

        public void ChangeFireMode(FireStrategy fireMediator)
        {
            _fireStrategy = fireMediator ?? throw new ArgumentNullException($"{(FireStrategy)null} is null");
            Debug.LogWarning($"сменил стрельбу на {_fireStrategy.GetType()}");
        }
    }
}