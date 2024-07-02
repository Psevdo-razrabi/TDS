namespace Game.Player.Weapons.AudioWeapon
{
    public class RifleAudioType : WeaponAudioType
    {
        public override AudioType WeaponShoot() => AudioType.RifleShoot;

        public override AudioType WeaponReload() => AudioType.Reload;

        public override AudioType WeaponOutAmmo() => AudioType.OutOfAmmoShoot;
    }
}