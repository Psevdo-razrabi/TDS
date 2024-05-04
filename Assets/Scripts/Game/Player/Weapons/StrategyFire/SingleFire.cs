using System;
using Game.Player.Weapons.InterfaceWeapon;
using Input;

namespace Game.Player.Weapons.StrategyFire
{
    public class SingleFire : FireStrategy
    {
        public SingleFire(InputSystemWeapon inputSystemWeapon, MouseInputObserver mouseInputObserver) : base(inputSystemWeapon, mouseInputObserver)
        {
            
        }
        

        public override void Fire(FireComponent component)
        {
            component.FireBullet();
        }
    }
}