using System;
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
        public readonly MouseInputObserver MouseInputObserver;
        public readonly ActionsCleaner ActionsCleaner;
        public readonly WeaponData WeaponData;
        public readonly WeaponConfigs.WeaponConfigs WeaponConfigs;
        private EventController _eventController;

        public FireComponent(WeaponData weaponData, MouseInputObserver mouseInputObserver, ActionsCleaner actionsCleaner, WeaponConfigs.WeaponConfigs weaponConfigs, EventController eventController)
        {
            WeaponData = weaponData;
            MouseInputObserver = mouseInputObserver;
            ActionsCleaner = actionsCleaner;
            WeaponConfigs = weaponConfigs;
            _eventController = eventController;
        }
        
        public void FireBullet()
        {
            var fireAction = new FireBulletAction(WeaponData, _fireStrategy);
            var handler = new HandlerDecoratorActions(() => !WeaponData.IsReloading, fireAction);
            handler.Execute();
            _eventController.ShotFire();
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