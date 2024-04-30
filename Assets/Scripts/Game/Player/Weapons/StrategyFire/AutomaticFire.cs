using Game.Player.Weapons.InterfaseWeapon;

namespace Game.Player.Weapons.StrategyFire
{
    public class AutomaticFire : IFireStrategy
    {
        public void Fire(FireComponent component)
        {
            component.FireBullet();
        }
    }
}