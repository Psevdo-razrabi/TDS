using System;
using Game.Player.Weapons.InterfaseWeapon;
using UnityEngine;

namespace Game.Player.Weapons
{
    public class FireComponent : IFireMediator
    {
        protected IFireStrategy FireStrategy;

        public void FireBullet()
        {
            
        }
        
        public void Fire()
        {
            FireStrategy.Fire(this);
        }

        public void ChangeFireMode(IFireStrategy fireMediator)
        {
            FireStrategy = fireMediator ?? throw new ArgumentNullException($"{(IFireStrategy)null} is null");
            Debug.LogWarning($"сменил стрельбу на {FireStrategy.GetType()}");
        }
    }
}