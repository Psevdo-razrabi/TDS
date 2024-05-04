using System;
using Game.Player.Weapons.InterfaseWeapon;
using Game.Player.Weapons.ReloadStrategy;
using UnityEngine;

namespace Game.Player.Weapons
{
    public class ReloadComponent : IReload
    {
        private IReloadStrategy _reloadStrategy = new ReloadImage();
        
        public void Reload()
        {
            _reloadStrategy.Reload();
        }

        public void ChangeReloadStrategy(IReloadStrategy reloadStrategy)
        {
            _reloadStrategy = reloadStrategy ?? throw new ArgumentNullException($"{(IReloadStrategy)null} is null");
            Debug.LogWarning($"сменил реализацию перезарядки на {_reloadStrategy.GetType()}");
        }
    }
}