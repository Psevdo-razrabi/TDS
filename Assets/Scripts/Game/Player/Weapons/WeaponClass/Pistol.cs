namespace Game.Player.Weapons.WeaponClass
{
    public class Pistol : WeaponComponent
    {
        public override void ReloadWeapon()
        {
            Reload();
        }

        public override void FireBullet()
        {
            Fire();
        }
    }
}