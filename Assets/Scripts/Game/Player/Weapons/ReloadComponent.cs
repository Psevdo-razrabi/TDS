using System;
using Game.Player.Weapons.InterfaseWeapon;
using UnityEngine;

namespace Game.Player.Weapons
{
    public abstract class ReloadComponent 
    {
        protected IReloadStrategy ReloadStrategy;

        public abstract void ReloadWeapon();

        public void Reload()
        {
            ReloadStrategy.Reload(this);
        }

        public void ChangeReloadStrategy(IReloadStrategy reloadStrategy)
        {
            ReloadStrategy = reloadStrategy ?? throw new ArgumentNullException($"{(IReloadStrategy)null} is null");
            Debug.LogWarning($"сменил реализацию перезарядки на {ReloadComponent.GetType()}");
        }
    }
}