using System;
using Cysharp.Threading.Tasks;
using Game.Player.Weapons.InterfaceWeapon;
using UnityEngine;

namespace Game.Player.Weapons.StrategyFire
{
    public class BurstFire : FireStrategy, IDisposable
    {
        private bool _isDisposed = false;
    
        public BurstFire(FireComponent fireComponent) : base(fireComponent)
        {
            FireComponent.ActionsCleaner.RemoveAction();
            FireComponent.ActionsCleaner.AddAction(this);
        }

        public override async void Fire(FireComponent component)
        {
            if (_isDisposed) return;
        
            await BurstShoot(component);
        }

        private async UniTask BurstShoot(FireComponent component)
        {
            for (int i = 0; i < 3; i++)
            {
                if (_isDisposed) return;

                component.FireBullet();
                await UniTask.Delay(
                    TimeSpan.FromSeconds(FireComponent.CurrentWeapon.CurrentWeaponConfig.TimeBetweenShoots));
            }
        
            await UniTask.Delay(TimeSpan.FromSeconds(FireComponent.CurrentWeapon.CurrentWeaponConfig.BurstReloadTime));
        }
    }
}