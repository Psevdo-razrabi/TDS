using System;
using Cysharp.Threading.Tasks;
using Game.Player.Weapons.InterfaceWeapon;
using Input;

namespace Game.Player.Weapons.StrategyFire
{
    public class BurstFire : FireStrategy
    {
        public BurstFire(FireComponent fireComponent) : base(fireComponent)
        {
            FireComponent.ActionsCleaner.RemoveAction();
            FireComponent.ActionsCleaner.AddAction(this);
        }

        public override async void Fire(FireComponent component)
        {
            await BurstShoot(component);
        }
        
        private async UniTask BurstShoot(FireComponent component)
        {
            for (int i = 0; i < 3; i++) //кол-во выстрелов берста
            {
                component.FireBullet();
                await UniTask.DelayFrame(100); //кол-во кадров между выстрелами
            }

            await UniTask.Delay(TimeSpan.FromSeconds(2.0f)); //время между burstami
        }
    }
}