using System;
using Game.Player.Weapons.InterfaceWeapon;
using Input;

namespace Game.Player.Weapons.StrategyFire
{
    public class SingleFire : FireStrategy
    {
        public SingleFire(FireComponent fireComponent) : base(fireComponent)
        {
            FireComponent.ActionsCleaner.RemoveAction();
            FireComponent.ActionsCleaner.AddAction(this);
        }
        
        public override void Fire(FireComponent component)
        {
            component.FireBullet();
        }
    }
}