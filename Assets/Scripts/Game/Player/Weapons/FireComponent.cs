using System;
using Game.Player.Weapons.InterfaceWeapon;
using Game.Player.Weapons.InterfaseWeapon;
using Game.Player.Weapons.StrategyFire;
using Input;
using UnityEngine;

namespace Game.Player.Weapons
{
    public class FireComponent : IFireMediator, IFire
    {
        private FireStrategy _fireStrategy;
        public InputSystemWeapon InputSystemWeapon { get; private set; }
        public MouseInputObserver MouseInputObserver { get; private set; }
        public ActionsCleaner ActionsCleaner { get; private set; }

        public FireComponent(InputSystemWeapon inputSystemWeapon, MouseInputObserver mouseInputObserver, ActionsCleaner actionsCleaner)
        {
            MouseInputObserver = mouseInputObserver;
            ActionsCleaner = actionsCleaner;
            InputSystemWeapon = inputSystemWeapon;
            _fireStrategy = new SingleFire(this);
        }

        public void FireBullet()
        {
            Debug.LogWarning($"Shooooooooooooooooot {_fireStrategy.GetType()}");
            //ссылка на bullet component?? bullet? bulletComponent
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