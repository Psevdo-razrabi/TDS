namespace Game.Player.Weapons.AudioWeapon
{
    public abstract class WeaponAudioType
    {
        public abstract AudioType WeaponShoot();
        public abstract AudioType WeaponReload();
        public abstract AudioType WeaponOutAmmo();
    }
}