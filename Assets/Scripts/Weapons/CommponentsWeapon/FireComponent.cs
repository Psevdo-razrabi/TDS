using System;
using Game.Player.Weapons.Decorator;
using Game.Player.Weapons.InterfaceWeapon;
using Game.Player.Weapons.InterfaseWeapon;
using Game.Player.Weapons.StrategyFire;
using Game.Player.Weapons.WeaponConfigs;
using Input;
using UnityEngine;
using Weapons.InterfaceWeapon;

namespace Game.Player.Weapons
{
    public class FireComponent : IFireMediator, IFire
    {
        public readonly InputObserver InputObserver;
        public readonly ActionsCleaner ActionsCleaner;
        public readonly CurrentWeapon CurrentWeapon;
        public Action ShotFired;
        private FireStrategy _fireStrategy;
        private IAudioWeapon _audioWeapon;
        private readonly WeaponData _weaponData;
        
        public FireComponent(WeaponData weaponData, InputObserver inputObserver, 
            ActionsCleaner actionsCleaner, CurrentWeapon currentWeapon, IAudioWeapon audioWeapon)
        {
            _weaponData = weaponData;
            InputObserver = inputObserver;
            ActionsCleaner = actionsCleaner;
            CurrentWeapon = currentWeapon;
            _audioWeapon = audioWeapon;
        }
        
        public void FireBullet()
        {
            var fireAction = new FireBulletAction(_weaponData, _fireStrategy,() => ShotFired?.Invoke());
            var handler = new HandlerDecoratorActions(() => !_weaponData.IsReloading && _weaponData.IsShoot, fireAction);
            if(handler.CanExecute() == false) return;

            var weaponPrefabs = CurrentWeapon.WeaponPrefabs;
            _audioWeapon.PlayOneShot(CurrentWeapon.WeaponComponent.WeaponAudioType.WeaponShoot(), weaponPrefabs.CurrentPrefabWeapon.transform.position);
        }
        
        public void Fire()
        {
            _fireStrategy.Fire(this);
        }

        public void ChangeFireMode(FireStrategy fireMediator)
        {
            _fireStrategy = fireMediator ?? throw new ArgumentNullException($"{(FireStrategy)null} is null");
            _weaponData.FireStrategy = _fireStrategy;
            Debug.LogWarning($"сменил стрельбу на {_fireStrategy.GetType()}");
        }
    }
}