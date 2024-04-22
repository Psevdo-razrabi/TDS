using Game.Player.Weapons.InterfaseWeapon;

namespace Game.Player.Weapons.StrategyFire
{
    public class BurstFire : IFireStrategy
    {
        public void Fire(WeaponComponent component)
        {
            component.FireBullet();
        }
    }
}