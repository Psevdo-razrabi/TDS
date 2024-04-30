using System;
using Game.Player.Weapons.InterfaseWeapon;
using UnityEngine;

namespace Game.Player.Weapons
{
    public abstract class WeaponComponent : IFireMediator, IFire
    {
        protected IFireStrategy FireStrategy;
        protected IReloadStrategy ReloadStrategy;
        protected ReloadComponent ReloadComponent;

        public abstract void ReloadWeapon();
        public abstract void FireBullet();

        public void Reload()
        {
            ReloadStrategy.Reload(ReloadComponent);
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

        public void ChangeReloadStrategy(IReloadStrategy reloadStrategy)
        {
            ReloadStrategy = reloadStrategy ?? throw new ArgumentNullException($"{(IReloadStrategy)null} is null");
            Debug.LogWarning($"сменил реализацию перезарядки на {ReloadComponent.GetType()}");
        }
    }
}