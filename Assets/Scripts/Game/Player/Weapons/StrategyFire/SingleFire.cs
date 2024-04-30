using Game.Player.Weapons.InterfaseWeapon;

namespace Game.Player.Weapons.StrategyFire
{
    public class SingleFire : IFireStrategy
    {
        public void Fire(FireComponent component)
        {
            component.FireBullet();
        }
    }
}